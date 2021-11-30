﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RFC.Common.Interfaces;
using SAP.Middleware.Connector;

namespace RFC.Common
{
    public class RfcManager : IRfcManager
    {
        private readonly IRfcRepositoryCreator _rfcRepositoryCreator;
        private readonly IRfcFunctionCreator _rfcFunctionCreator;

        public RfcManager(IRfcRepositoryCreator rfcRepositoryCreator, IRfcFunctionCreator rfcFunctionCreator)
        {
            _rfcRepositoryCreator = rfcRepositoryCreator;
            _rfcFunctionCreator = rfcFunctionCreator;
        }

        public void ProcessRequest(string destinationName, string rfcFunctionName,
            RfcParameter functionParam, RfcParameter headerParam, RfcParameter tableParams,
           ref RfcParameter returnHeaders, ref RfcParameter returnStructure, ref RfcParameter returnTable
        )
        {
            using (_rfcRepositoryCreator)
            {
                var repo = _rfcRepositoryCreator.Create(destinationName);
                var function = _rfcFunctionCreator.Create(rfcFunctionName, repo);

                FormatInputParameters(function, functionParam, headerParam, tableParams);
                //invoke
                function.Invoke(_rfcRepositoryCreator.Destination);

                FormatReturnResult(function, ref returnHeaders, ref returnStructure, ref returnTable);
            }

        }

        /// <summary>
        /// Format input parameters
        /// </summary>
        /// <param name="function"></param>
        /// <param name="functionParam"></param>
        /// <param name="headerParam"></param>
        /// <param name="bodyParams"></param>
        /// <returns></returns>
        private IRfcFunction FormatInputParameters(IRfcFunction function, RfcParameter functionParam, RfcParameter headerParam, RfcParameter bodyParams)
        {
            //function params
            foreach (var rfcStructureData in functionParam.Data ?? Enumerable.Empty<RfcStructureData>())
            {
                function.SetValue(rfcStructureData.Key, rfcStructureData.Value);
            }

            //header params
            if (!string.IsNullOrEmpty(headerParam.StructureName))
            {
                var headerStructure = function.GetStructure(headerParam.StructureName);
                foreach (var rfcStructureData in headerParam.Data ?? Enumerable.Empty<RfcStructureData>())
                {
                    headerStructure.SetValue(rfcStructureData.Key, rfcStructureData.Value);
                }
            }

            //table params
            if (!string.IsNullOrEmpty(headerParam.StructureName))
            {
                var items = function.GetTable(bodyParams.StructureName);
                items.Insert();
                foreach (var rfcStructureData in bodyParams.Data ?? Enumerable.Empty<RfcStructureData>())
                {
                    items.CurrentRow.SetValue(rfcStructureData.Key, rfcStructureData.Value);
                }
            }

            return function;
        }

        /// <summary>
        /// Format returns 
        /// </summary>
        /// <param name="function"></param>
        /// <param name="returnHeaders"></param>
        /// <param name="returnStructure"></param>
        /// <param name="returnTable"></param>
        private void FormatReturnResult(IRfcFunction function, ref RfcParameter returnHeaders, ref RfcParameter returnStructure, ref RfcParameter returnTable)
        {
            //return headers
            if (!string.IsNullOrEmpty(returnHeaders.StructureName))
            {
                var rfcReturnHeader = function.GetStructure(returnHeaders.StructureName);
                for (var liElement = 0; liElement < rfcReturnHeader.ElementCount; liElement++)
                {
                    var element = rfcReturnHeader.GetElementMetadata(liElement);
                    var headerRow = new RfcStructureData { Key =  element.Name, Value = rfcReturnHeader.GetValue(element.Name) };
                    returnHeaders.Data.Add(headerRow);
                }
            }

            //return structure
            if (!string.IsNullOrEmpty(returnStructure.StructureName))
            {
                var rfcReturnStructure = function.GetStructure(returnStructure.StructureName);
                for (var liElement = 0; liElement < rfcReturnStructure.ElementCount; liElement++)
                {
                    var element = rfcReturnStructure.GetElementMetadata(liElement);
                    var headerRow = new RfcStructureData { Key =  element.Name, Value = rfcReturnStructure.GetValue(element.Name) };
                    returnStructure.Data.Add(headerRow);
                }
            }

            if (!string.IsNullOrEmpty(returnTable.StructureName))
            {
                var rfcReturnTable = function.GetTable(returnTable.StructureName);
                returnTable.TableData.AddRange(rfcReturnTable.ToRfcStructureData());
            }
        }

    }
}
