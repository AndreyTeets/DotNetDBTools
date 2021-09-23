using System.Linq;
using DotNetDBTools.Models.Agnostic;
using DotNetDBTools.Models.SQLite;

namespace DotNetDBTools.Analysis.SQLite
{
    public static class AgnosticToSQLiteConverter
    {
        public static SQLiteDatabaseInfo ConvertToSQLiteInfo(AgnosticDatabaseInfo databaseInfo)
           => new(databaseInfo.Name)
           {
               Tables = databaseInfo.Tables.Select(x => ConvertToSQLiteInfo((AgnosticTableInfo)x)).ToList(),
               Views = databaseInfo.Views.Select(x => ConvertToSQLiteInfo((AgnosticViewInfo)x)).ToList(),
           };

        private static SQLiteTableInfo ConvertToSQLiteInfo(AgnosticTableInfo tableInfo)
            => new()
            {
                ID = tableInfo.ID,
                Name = tableInfo.Name,
                Columns = tableInfo.Columns,
                PrimaryKey = tableInfo.PrimaryKey,
                UniqueConstraints = tableInfo.UniqueConstraints,
                ForeignKeys = tableInfo.ForeignKeys,
            };

        private static SQLiteViewInfo ConvertToSQLiteInfo(AgnosticViewInfo viewInfo)
            => new()
            {
                ID = viewInfo.ID,
                Name = viewInfo.Name,
                Code = viewInfo.Code,
            };
    }
}
