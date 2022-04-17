using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.Analysis.Core;
using DotNetDBTools.Models.Agnostic;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.PostgreSQL;

namespace DotNetDBTools.Analysis.PostgreSQL;

public class PostgreSQLDbModelConverter : DbModelConverter
{
    public PostgreSQLDbModelConverter()
        : base(DatabaseKind.PostgreSQL, new PostgreSQLDbModelPostProcessor()) { }

    protected override Database ConvertDatabase(AgnosticDatabase database)
    {
        return new PostgreSQLDatabase(database.Name)
        {
            Version = database.Version,
            Tables = database.Tables.Select(x => ConvertToPostgreSQLModel((AgnosticTable)x)).ToList(),
            Views = database.Views.Select(x => ConvertToPostgreSQLModel((AgnosticView)x)).ToList(),
            Scripts = database.Scripts.Select(x => ConvertScript(x)).ToList(),
        };
    }

    private PostgreSQLTable ConvertToPostgreSQLModel(AgnosticTable table)
    {
        return new()
        {
            ID = table.ID,
            Name = table.Name,
            Columns = ConvertToPostgreSQLModel(table.Columns),
            PrimaryKey = table.PrimaryKey,
            UniqueConstraints = table.UniqueConstraints,
            CheckConstraints = table.CheckConstraints.Select(ck => { ck.CodePiece = ConvertCodePiece(ck.CodePiece); return ck; }).ToList(),
            Indexes = table.Indexes,
            Triggers = table.Triggers.Select(trigger => { trigger.CodePiece = ConvertCodePiece(trigger.CodePiece); return trigger; }).ToList(),
            ForeignKeys = table.ForeignKeys,
        };
    }

    private PostgreSQLView ConvertToPostgreSQLModel(AgnosticView view)
    {
        return new()
        {
            ID = view.ID,
            Name = view.Name,
            CodePiece = ConvertCodePiece(view.CodePiece),
        };
    }

    private IEnumerable<Column> ConvertToPostgreSQLModel(IEnumerable<Column> columns)
    {
        foreach (Column column in columns)
        {
            if (column.DataType is AgnosticVerbatimDataType avdt)
                column.DataType = new DataType { Name = ConvertCodePiece(avdt.NameCodePiece).Code };
            else
                column.DataType = PostgreSQLDataTypeConverter.ConvertToPostgreSQL((CSharpDataType)column.DataType);

            if (column.Default is AgnosticCodePiece acp)
                column.Default = ConvertCodePiece(acp);
            else
                column.Default = PostgreSQLDefaultValueConverter.ConvertToPostgreSQL((CSharpDefaultValue)column.Default);
        }
        return columns;
    }
}
