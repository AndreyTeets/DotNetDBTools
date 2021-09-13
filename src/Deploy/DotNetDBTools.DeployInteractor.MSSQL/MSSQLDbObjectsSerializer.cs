using DotNetDBTools.Models.MSSQL;
using Newtonsoft.Json;

namespace DotNetDBTools.DeployInteractor.MSSQL
{
    public static class MSSQLDbObjectsSerializer
    {
        private static readonly JsonSerializerSettings s_serializerSettings = new()
        {
            Formatting = Formatting.Indented,
            TypeNameHandling = TypeNameHandling.Auto,
        };

        public static string TableToJson(MSSQLTableInfo table)
        {
            string tableJson = JsonConvert.SerializeObject(table, s_serializerSettings);
            return tableJson;
        }

        public static MSSQLTableInfo TableFromJson(string tableJson)
        {
            MSSQLTableInfo table = JsonConvert.DeserializeObject<MSSQLTableInfo>(tableJson, s_serializerSettings);
            return table;
        }

        public static string UserDefinedTypeToJson(MSSQLUserDefinedTypeInfo userDefinedType)
        {
            string tableJson = JsonConvert.SerializeObject(userDefinedType, s_serializerSettings);
            return tableJson;
        }

        public static MSSQLUserDefinedTypeInfo UserDefinedTypeFromJson(string userDefinedTypeJson)
        {
            MSSQLUserDefinedTypeInfo userDefinedType = JsonConvert.DeserializeObject<MSSQLUserDefinedTypeInfo>(userDefinedTypeJson, s_serializerSettings);
            return userDefinedType;
        }
    }
}
