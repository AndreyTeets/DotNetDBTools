using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.Analysis.Core;
using DotNetDBTools.Models.Agnostic;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.SQLite;

namespace DotNetDBTools.Analysis.SQLite;

public class SQLiteDbModelConverter : DbModelConverter
{
    public SQLiteDbModelConverter()
        : base(DatabaseKind.SQLite) { }

    public override Database FromAgnostic(Database database)
    {
        return ConvertToSQLiteModel((AgnosticDatabase)database);
    }

    private SQLiteDatabase ConvertToSQLiteModel(AgnosticDatabase database)
    {
        return new(database.Name)
        {
            Version = database.Version,
            Tables = database.Tables.Select(x => ConvertToSQLiteModel((AgnosticTable)x)).ToList(),
            Views = database.Views.Select(x => ConvertToSQLiteModel((AgnosticView)x)).ToList(),
            Scripts = database.Scripts.Select(x => ConvertScript(x)).ToList(),
        };
    }

    private SQLiteTable ConvertToSQLiteModel(AgnosticTable table)
    {
        return new()
        {
            ID = table.ID,
            Name = table.Name,
            Columns = ConvertToSQLiteModel(table.Columns),
            PrimaryKey = table.PrimaryKey,
            UniqueConstraints = table.UniqueConstraints,
            CheckConstraints = table.CheckConstraints.Select(ck => { ck.CodePiece = ConvertCodePiece(ck.CodePiece); return ck; }).ToList(),
            Indexes = table.Indexes,
            Triggers = table.Triggers.Select(trigger => { trigger.CodePiece = ConvertCodePiece(trigger.CodePiece); return trigger; }).ToList(),
            ForeignKeys = table.ForeignKeys,
        };
    }

    private SQLiteView ConvertToSQLiteModel(AgnosticView view)
    {
        return new()
        {
            ID = view.ID,
            Name = view.Name,
            CodePiece = new CodePiece { Code = ((AgnosticCodePiece)view.CodePiece).DbKindToCodeMap[DatabaseKind.SQLite] },
        };
    }

    private IEnumerable<Column> ConvertToSQLiteModel(IEnumerable<Column> columns)
    {
        foreach (Column column in columns)
        {
            if (column.DataType is AgnosticVerbatimDataType avdt)
                column.DataType = new DataType { Name = ConvertCodePiece(avdt.NameCodePiece).Code };
            else
                column.DataType = SQLiteDataTypeConverter.ConvertToSQLite((CSharpDataType)column.DataType);

            if (column.Default is AgnosticCodePiece acp)
                column.Default = ConvertCodePiece(acp);
            else
                column.Default = SQLiteDefaultValueConverter.ConvertToSQLite((CSharpDefaultValue)column.Default);
        }
        return columns;
    }
}
