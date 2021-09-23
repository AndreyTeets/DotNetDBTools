using System.Linq;
using DotNetDBTools.Models.Agnostic;
using DotNetDBTools.Models.MSSQL;

namespace DotNetDBTools.Analysis.MSSQL
{
    public static class AgnosticToMSSQLConverter
    {
        public static MSSQLDatabaseInfo ConvertToMSSQLInfo(AgnosticDatabaseInfo databaseInfo)
           => new(databaseInfo.Name)
           {
               Tables = databaseInfo.Tables.Select(x => ConvertToMSSQLInfo((AgnosticTableInfo)x)).ToList(),
               Views = databaseInfo.Views.Select(x => ConvertToMSSQLInfo((AgnosticViewInfo)x)).ToList(),
           };

        private static MSSQLTableInfo ConvertToMSSQLInfo(AgnosticTableInfo tableInfo)
            => new()
            {
                ID = tableInfo.ID,
                Name = tableInfo.Name,
                Columns = tableInfo.Columns,
                PrimaryKey = tableInfo.PrimaryKey,
                UniqueConstraints = tableInfo.UniqueConstraints,
                ForeignKeys = tableInfo.ForeignKeys,
            };

        private static MSSQLViewInfo ConvertToMSSQLInfo(AgnosticViewInfo viewInfo)
            => new()
            {
                ID = viewInfo.ID,
                Name = viewInfo.Name,
                Code = viewInfo.Code,
            };
    }
}
