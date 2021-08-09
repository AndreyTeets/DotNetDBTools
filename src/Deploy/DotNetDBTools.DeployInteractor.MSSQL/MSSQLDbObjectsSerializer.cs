using DotNetDBTools.Models.MSSQL;

namespace DotNetDBTools.DeployInteractor.MSSQL
{
    public static class MSSQLDbObjectsSerializer
    {
        public static string TableToJson(MSSQLTableInfo table)
        {
            string tableMetadata = table.ToString();
            return tableMetadata;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
        public static MSSQLTableInfo TableFromJson(string tableJson)
        {
            return new MSSQLTableInfo();
        }
    }
}
