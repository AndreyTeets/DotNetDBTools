using DotNetDBTools.Models.SQLite;

namespace DotNetDBTools.DeployInteractor.SQLite
{
    public static class SQLiteDbObjectsSerializer
    {
        public static string TableToJson(SQLiteTableInfo table)
        {
            string tableMetadata = table.ToString();
            return tableMetadata;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
        public static SQLiteTableInfo TableFromJson(string tableJson)
        {
            return new SQLiteTableInfo();
        }
    }
}
