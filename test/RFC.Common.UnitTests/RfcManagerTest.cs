using AutoFixture.NUnit3;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using RFC.Common.Interfaces;
using SAP.Middleware.Connector;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace RFC.Common.UnitTests
{
    [ExcludeFromCodeCoverage]
    public class RfcManagerTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        [AutoDomainData]
        public void Given_InputHeader_When_CallingProcessRequest_ThenReturnHeaderAndStructure(
            [Frozen]Mock<IRfcRepositoryCreator> repoCreatorMock,[Frozen]Mock<IRfcFunctionCreator> functionCreatorMock,
            ProcessRequestInput input, IRfcFunction function, RfcManager rfcManager)
        {
            repoCreatorMock.Setup(r=>r.Create(It.IsAny<string>())).Returns(new RfcRepoWrapper());
            functionCreatorMock.Setup(f=>f.Create(It.IsAny<string>(), It.IsAny<RfcRepository>())).Returns(function);

            input.functionParam = new RfcParameter {
                Data = new List<RfcStructureData> {
                   new RfcStructureData {Key = "Function_1", Value = "test" },
                   new RfcStructureData {Key = "Function_2", Value = 1 },
                   new RfcStructureData {Key = "Function_3", Value = 2.5 }
                }
            };

            input.headerParam = new RfcParameter
            {
                StructureName = "IM_HEADER",
                Data = new List<RfcStructureData> {
                   new RfcStructureData {Key = "FunctionHeader_1", Value = "test" },
                   new RfcStructureData {Key = "FunctionHeader_2", Value = 1 },
                   new RfcStructureData {Key = "FunctionHeader_3", Value = 2.5 }
                }
            };

            input.tableParams.StructureName = string.Empty;

            input.returnHeaders = new RfcParameter {
                StructureName = "EX_HEADER",
            };
            input.returnStructure = new RfcParameter { 
                StructureName = "EX_RETURN"
            };
            input.returnTable = new RfcParameter
            {
                StructureName = string.Empty
            };


            rfcManager.ProcessRequest(input.DestinationName, input.RfcFunctionName,
            input.functionParam, input.headerParam, input.tableParams,
            input.returnHeaders, input.returnStructure, input.returnTable);


        }
    }
}