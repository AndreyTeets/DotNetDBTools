using System.Linq;
using DotNetDBTools.Definition;
using DotNetDBTools.Models.Agnostic;
using DotNetDBTools.Models.MSSQL;

namespace DotNetDBTools.DefinitionParser.MSSQL
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
                Columns = tableInfo.Columns.Select(x => ConvertToMSSQLInfo((AgnosticColumnInfo)x)).ToList(),
                ForeignKeys = tableInfo.ForeignKeys.Select(x => ConvertToMSSQLInfo((AgnosticForeignKeyInfo)x)).ToList(),
            };

        private static MSSQLViewInfo ConvertToMSSQLInfo(AgnosticViewInfo viewInfo)
            => new()
            {
                ID = viewInfo.ID,
                Name = viewInfo.Name,
                Code = viewInfo.Code,
            };

        private static MSSQLColumnInfo ConvertToMSSQLInfo(AgnosticColumnInfo columnInfo)
           => new()
           {
               ID = columnInfo.ID,
               Name = columnInfo.Name,
               DataType = MSSQLColumnTypeMapper.GetSqlType((IDbType)columnInfo.DataType),
               DefaultValue = columnInfo.DefaultValue,
           };

        private static MSSQLForeignKeyInfo ConvertToMSSQLInfo(AgnosticForeignKeyInfo foreignKeyInfo)
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
