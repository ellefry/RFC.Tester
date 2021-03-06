using SAP.Middleware.Connector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RFC.Common
{
    public static class IRfcTableExtension
    {
        public static List<List<RfcStructureData>> ToRfcStructureData(this IRfcTable sapTable)
        {
            var result = new List<List<RfcStructureData>>();

            foreach (IRfcStructure row in sapTable)
            {
                var items = new List<RfcStructureData>();
                for (int liElement = 0; liElement < sapTable.ElementCount; liElement++)
                {
                    RfcElementMetadata metadata = sapTable.GetElementMetadata(liElement);
                    switch (metadata.DataType)
                    {
                        case RfcDataType.DATE:
                            items.Add(new RfcStructureData { Key = string.Intern(metadata.Name), Value = 
                                row.GetString(metadata.Name).Substring(0, 4) + row.GetString(metadata.Name).Substring(5, 2) + row.GetString(metadata.Name).Substring(8, 2) });
                            break;
                        case RfcDataType.BCD:
                            items.Add(new RfcStructureData { Key = string.Intern(metadata.Name), Value = row.GetDecimal(metadata.Name) });
                            break;
                        case RfcDataType.CHAR:
                            items.Add(new RfcStructureData { Key=string.Intern(metadata.Name), Value = row.GetString(metadata.Name) });
                            break;
                        case RfcDataType.STRING:
                            items.Add(new RfcStructureData { Key=string.Intern(metadata.Name), Value = row.GetString(metadata.Name) });
                            break;
                        case RfcDataType.INT2:
                            items.Add(new RfcStructureData { Key=string.Intern(metadata.Name), Value = row.GetInt(metadata.Name) });
                            break;
                        case RfcDataType.INT4:
                            items.Add(new RfcStructureData { Key=string.Intern(metadata.Name), Value = row.GetInt(metadata.Name) });
                            break;
                        case RfcDataType.FLOAT:
                            items.Add(new RfcStructureData { Key=string.Intern(metadata.Name), Value = row.GetDouble(metadata.Name) });
                            break;
                        default:
                            items.Add(new RfcStructureData { Key=string.Intern(metadata.Name), Value = row.GetString(metadata.Name) });
                            break;
                    }
                }
                result.Add(items);
            }
            return result;
        }

        public static DataTable ToDataTable(this IRfcTable sapTable, string name)
        {
            DataTable adoTable = new DataTable(name);
            //... Create ADO.Net table.  
            for (int liElement = 0; liElement < sapTable.ElementCount; liElement++)
            {
                RfcElementMetadata metadata = sapTable.GetElementMetadata(liElement);
                adoTable.Columns.Add(metadata.Name, GetDataType(metadata.DataType));
            }

            //Transfer rows from SAP Table ADO.Net table.  
            foreach (IRfcStructure row in sapTable)
            {
                DataRow ldr = adoTable.NewRow();
                for (int liElement = 0; liElement < sapTable.ElementCount; liElement++)
                {
                    RfcElementMetadata metadata = sapTable.GetElementMetadata(liElement);

                    switch (metadata.DataType)
                    {
                        case RfcDataType.DATE:
                            ldr[metadata.Name] = row.GetString(metadata.Name).Substring(0, 4) + row.GetString(metadata.Name).Substring(5, 2) + row.GetString(metadata.Name).Substring(8, 2);
                            break;
                        case RfcDataType.BCD:
                            ldr[metadata.Name] = row.GetDecimal(metadata.Name);
                            break;
                        case RfcDataType.CHAR:
                            ldr[metadata.Name] = row.GetString(metadata.Name);
                            break;
                        case RfcDataType.STRING:
                            ldr[metadata.Name] = row.GetString(metadata.Name);
                            break;
                        case RfcDataType.INT2:
                            ldr[metadata.Name] = row.GetInt(metadata.Name);
                            break;
                        case RfcDataType.INT4:
                            ldr[metadata.Name] = row.GetInt(metadata.Name);
                            break;
                        case RfcDataType.FLOAT:
                            ldr[metadata.Name] = row.GetDouble(metadata.Name);
                            break;
                        default:
                            ldr[metadata.Name] = row.GetString(metadata.Name);
                            break;
                    }
                }
                adoTable.Rows.Add(ldr);
            }
            return adoTable;
        }

        private static Type GetDataType(RfcDataType rfcDataType)
        {
            switch (rfcDataType)
            {
                case RfcDataType.DATE:
                    return typeof(string);
                case RfcDataType.CHAR:
                    return typeof(string);
                case RfcDataType.STRING:
                    return typeof(string);
                case RfcDataType.BCD:
                    return typeof(decimal);
                case RfcDataType.INT2:
                    return typeof(int);
                case RfcDataType.INT4:
                    return typeof(int);
                case RfcDataType.FLOAT:
                    return typeof(double);
                default:
                    return typeof(string);
            }
        }
    }
}
