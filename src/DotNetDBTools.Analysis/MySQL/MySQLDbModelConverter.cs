using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.Analysis.Core;
using DotNetDBTools.Models.Agnostic;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.MySQL;

namespace DotNetDBTools.Analysis.MySQL;

public class MySQLDbModelConverter : DbModelConverter
{
    public MySQLDbModelConverter()
        : base(DatabaseKind.MySQL) { }

    public override Database FromAgnostic(Database database)
    {
        MySQLDatabase mysqlDatabase = ConvertToMySQLModel((AgnosticDatabase)database);
        MySQLPostBuildProcessingHelper.ReplaceUniqueConstraintsWithUniqueIndexes(mysqlDatabase);
        return mysqlDatabase;
    }

    private MySQLDatabase ConvertToMySQLModel(AgnosticDatabase database)
    {
        return new(database.Name)
        {
            Version = database.Version,
            Tables = database.Tables.Select(x => ConvertToMySQLModel((AgnosticTable)x)).ToList(),
            Views = database.Views.Select(x => ConvertToMySQLModel((AgnosticView)x)).ToList(),
            Scripts = database.Scripts.Select(x => ConvertScript(x)).ToList(),
        };
    }

    private MySQLTable ConvertToMySQLModel(AgnosticTable table)
    {
        return new()
        {
            ID = table.ID,
            Name = table.Name,
            Columns = ConvertToMySQLModel(table.Columns),
            PrimaryKey = table.PrimaryKey is null ? table.PrimaryKey : ConvertToMySQLModel(table.PrimaryKey, table.Name),
            UniqueConstraints = table.UniqueConstraints,
            CheckConstraints = table.CheckConstraints.Select(ck => { ck.CodePiece = ConvertCodePiece(ck.CodePiece); return ck; }).ToList(),
            Indexes = table.Indexes,
            Triggers = table.Triggers.Select(trigger => { trigger.CodePiece = ConvertCodePiece(trigger.CodePiece); return trigger; }).ToList(),
            ForeignKeys = table.ForeignKeys,
        };
    }

    private MySQLView ConvertToMySQLModel(AgnosticView view)
    {
        return new()
        {
            ID = view.ID,
            Name = view.Name,
            CodePiece = ConvertCodePiece(view.CodePiece),
        };
    }

    private PrimaryKey ConvertToMySQLModel(PrimaryKey pk, string tableName)
    {
        return new()
        {
            ID = pk.ID,
            Name = $"PK_{tableName}",
            Columns = pk.Columns,
        };
    }

    private IEnumerable<Column> ConvertToMySQLModel(IEnumerable<Column> columns)
    {
        foreach (Column column in columns)
        {
            if (column.DataType is AgnosticVerbatimDataType avdt)
                column.DataType = new DataType { Name = ConvertCodePiece(avdt.NameCodePiece).Code };
            else
                column.DataType = MySQLDataTypeConverter.ConvertToMySQL((CSharpDataType)column.DataType);

            if (column.Default is AgnosticCodePiece acp)
                column.Default = ConvertCodePiece(acp);
            else
                column.Default = MySQLDefaultValueConverter.ConvertToMySQL((CSharpDefaultValue)column.Default);
        }
        return columns;
    }
}
