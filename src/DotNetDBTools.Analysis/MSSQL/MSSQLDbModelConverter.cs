using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.Analysis.Core;
using DotNetDBTools.Models.Agnostic;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.MSSQL;

namespace DotNetDBTools.Analysis.MSSQL;

public class MSSQLDbModelConverter : DbModelConverter
{
    public MSSQLDbModelConverter()
        : base(DatabaseKind.MSSQL) { }

    public override Database FromAgnostic(Database database)
    {
        return ConvertToMSSQLModel((AgnosticDatabase)database);
    }

    private MSSQLDatabase ConvertToMSSQLModel(AgnosticDatabase database)
    {
        return new(database.Name)
        {
            Version = database.Version,
            Tables = database.Tables.Select(x => ConvertToMSSQLModel((AgnosticTable)x)).ToList(),
            Views = database.Views.Select(x => ConvertToMSSQLModel((AgnosticView)x)).ToList(),
            Scripts = database.Scripts.Select(x => ConvertScript(x)).ToList(),
        };
    }

    private MSSQLTable ConvertToMSSQLModel(AgnosticTable table)
    {
        return new()
        {
            ID = table.ID,
            Name = table.Name,
            Columns = ConvertToMSSQLModel(table.Columns, table.Name),
            PrimaryKey = table.PrimaryKey,
            UniqueConstraints = table.UniqueConstraints,
            CheckConstraints = table.CheckConstraints.Select(ck => { ck.CodePiece = ConvertCodePiece(ck.CodePiece); return ck; }).ToList(),
            Indexes = table.Indexes,
            Triggers = table.Triggers.Select(trigger => { trigger.CodePiece = ConvertCodePiece(trigger.CodePiece); return trigger; }).ToList(),
            ForeignKeys = table.ForeignKeys,
        };
    }

    private static MSSQLView ConvertToMSSQLModel(AgnosticView view)
    {
        return new()
        {
            ID = view.ID,
            Name = view.Name,
            CodePiece = new CodePiece { Code = ((AgnosticCodePiece)view.CodePiece).DbKindToCodeMap[DatabaseKind.MSSQL] },
        };
    }

    private IEnumerable<Column> ConvertToMSSQLModel(IEnumerable<Column> columns, string tableName)
    {
        List<Column> mssqlColumns = new();
        foreach (Column column in columns)
        {
            DataType dataType = column.DataType is AgnosticVerbatimDataType avdt
                ? new DataType { Name = ConvertCodePiece(avdt.NameCodePiece).Code }
                : MSSQLDataTypeConverter.ConvertToMSSQL((CSharpDataType)column.DataType);

            CodePiece defaultValue = column.Default is AgnosticCodePiece acp
                ? ConvertCodePiece(acp)
                : MSSQLDefaultValueConverter.ConvertToMSSQL((CSharpDefaultValue)column.Default);

            MSSQLColumn mssqlColumn = new()
            {
                ID = column.ID,
                Name = column.Name,
                DataType = dataType,
                NotNull = column.NotNull,
                Identity = column.Identity,
                Default = defaultValue,
                DefaultConstraintName = defaultValue.Code is not null
                    ? $"DF_{tableName}_{column.Name}"
                    : null
            };
            mssqlColumns.Add(mssqlColumn);
        }
        return mssqlColumns;
    }
}
