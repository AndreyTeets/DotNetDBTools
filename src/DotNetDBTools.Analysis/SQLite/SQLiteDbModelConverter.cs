﻿using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.Analysis.Core;
using DotNetDBTools.Models.Agnostic;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.SQLite;

namespace DotNetDBTools.Analysis.SQLite;

public class SQLiteDbModelConverter : DbModelConverter
{
    public SQLiteDbModelConverter()
        : base(DatabaseKind.SQLite, new SQLiteDbModelPostProcessor()) { }

    protected override Database ConvertDatabase(AgnosticDatabase database)
    {
        return new SQLiteDatabase(database.Name)
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
            PrimaryKey = ConvertToMySQLModel(table.PrimaryKey, table.Name),
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
            CodePiece = ConvertCodePiece(view.CodePiece),
        };
    }

    private PrimaryKey ConvertToMySQLModel(PrimaryKey pk, string tableName)
    {
        if (pk is null)
            return null;
        return new()
        {
            ID = pk.ID,
            Name = $"PK_{tableName}",
            Columns = pk.Columns,
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
