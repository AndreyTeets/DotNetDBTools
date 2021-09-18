using System.Linq;
using DotNetDBTools.Definition;
using DotNetDBTools.Models.Agnostic;
using DotNetDBTools.Models.SQLite;

namespace DotNetDBTools.DefinitionParser.SQLite
{
    public static class AgnosticToSQLiteConverter
    {
        public static SQLiteDatabaseInfo ConvertToSQLiteInfo(AgnosticDatabaseInfo databaseInfo)
           => new()
           {
               Tables = databaseInfo.Tables.Select(x => ConvertToSQLiteInfo((AgnosticTableInfo)x)).ToList(),
               Views = databaseInfo.Views.Select(x => ConvertToSQLiteInfo((AgnosticViewInfo)x)).ToList(),
           };

        private static SQLiteTableInfo ConvertToSQLiteInfo(AgnosticTableInfo tableInfo)
            => new()
            {
                ID = tableInfo.ID,
                Name = tableInfo.Name,
                Columns = tableInfo.Columns.Select(x => ConvertToSQLiteInfo((AgnosticColumnInfo)x)).ToList(),
                ForeignKeys = tableInfo.ForeignKeys.Select(x => ConvertToSQLiteInfo((AgnosticForeignKeyInfo)x)).ToList(),
            };

        private static SQLiteViewInfo ConvertToSQLiteInfo(AgnosticViewInfo viewInfo)
            => new()
            {
                ID = viewInfo.ID,
                Name = viewInfo.Name,
                Code = viewInfo.Code,
            };

        private static SQLiteColumnInfo ConvertToSQLiteInfo(AgnosticColumnInfo columnInfo)
           => new()
           {
               ID = columnInfo.ID,
               Name = columnInfo.Name,
               DataType = SQLiteColumnTypeMapper.GetSqlType((IDbType)columnInfo.DataType),
               DefaultValue = columnInfo.DefaultValue,
           };

        private static SQLiteForeignKeyInfo ConvertToSQLiteInfo(AgnosticForeignKeyInfo foreignKeyInfo)
           => new()
           {
               ID = foreignKeyInfo.ID,
               Name = foreignKeyInfo.Name,
               ThisColumnNames = foreignKeyInfo.ThisColumnNames,
               ForeignTableName = foreignKeyInfo.ForeignTableName,
               ForeignColumnNames = foreignKeyInfo.ForeignColumnNames,
               OnDelete = foreignKeyInfo.OnDelete,
               OnUpdate = foreignKeyInfo.OnUpdate,
           };
    }
}
