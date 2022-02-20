using System;
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
            column.DataType = column.DataType.Name switch
            {
                AgnosticDataTypeNames.Int => new DataType { Name = SQLiteDataTypeNames.INTEGER },
                AgnosticDataTypeNames.Real => new DataType { Name = SQLiteDataTypeNames.REAL },
                AgnosticDataTypeNames.Decimal => new DataType { Name = SQLiteDataTypeNames.NUMERIC },
                AgnosticDataTypeNames.Bool => new DataType { Name = SQLiteDataTypeNames.INTEGER },

                AgnosticDataTypeNames.String => new DataType { Name = SQLiteDataTypeNames.TEXT },
                AgnosticDataTypeNames.Binary => new DataType { Name = SQLiteDataTypeNames.BLOB },
                AgnosticDataTypeNames.Guid => new DataType { Name = SQLiteDataTypeNames.BLOB },

                AgnosticDataTypeNames.Date => new DataType { Name = SQLiteDataTypeNames.NUMERIC },
                AgnosticDataTypeNames.Time => new DataType { Name = SQLiteDataTypeNames.NUMERIC },
                AgnosticDataTypeNames.DateTime => new DataType { Name = SQLiteDataTypeNames.NUMERIC },

                _ => throw new InvalidOperationException($"Invalid agnostic column datatype name: {column.DataType.Name}"),
            };
        }
        return columns;
    }
}
