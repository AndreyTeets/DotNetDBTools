using DotNetDBTools.Models.SQLite;
using Newtonsoft.Json;

namespace DotNetDBTools.DeployInteractor.SQLite
{
    public static class SQLiteDbObjectsSerializer
    {
        private static readonly JsonSerializerSettings s_serializerSettings = new()
        {
            Formatting = Formatting.Indented,
            TypeNameHandling = TypeNameHandling.Auto,
        };

        public static string TableToJson(SQLiteTableInfo table)
        {
            string tableJson = JsonConvert.SerializeObject(table, s_serializerSettings);
            return tableJson;
        }

        public static SQLiteTableInfo TableFromJson(string tableJson)
        {
            SQLiteTableInfo table = JsonConvert.DeserializeObject<SQLiteTableInfo>(tableJson, s_serializerSettings);
            return table;
        }
    }
}
