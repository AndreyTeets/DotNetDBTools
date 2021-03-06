using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.Models.Agnostic;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Analysis.Core;

internal abstract class DbModelConverter<
    TDatabase,
    TTable,
    TView,
    TColumn>
    : IDbModelConverter
    where TDatabase : Database, new()
    where TTable : Table, new()
    where TView : View, new()
    where TColumn : Column, new()
{
    private readonly DatabaseKind _databaseKind;
    private readonly IDataTypeConverter _dataTypeConverter;
    private readonly IDefaultValueConverter _defaultValueConverter;
    private readonly IDependenciesBuilder _dependenciesBuilder;
    private readonly IDbModelPostProcessor _dbModelPostProcessor;

    protected DbModelConverter(
        DatabaseKind databaseKind,
        IDataTypeConverter dataTypeConverter,
        IDefaultValueConverter defaultValueConverter,
        IDependenciesBuilder dependenciesBuilder,
        IDbModelPostProcessor dbModelPostProcessor)
    {
        _databaseKind = databaseKind;
        _dataTypeConverter = dataTypeConverter;
        _defaultValueConverter = defaultValueConverter;
        _dependenciesBuilder = dependenciesBuilder;
        _dbModelPostProcessor = dbModelPostProcessor;
    }

    public Database FromAgnostic(Database database)
    {
        TDatabase specificDbmsDatabase = new()
        {
            Version = database.Version,
            Tables = database.Tables.Select(x => ConvertTable((AgnosticTable)x)).ToList(),
            Views = database.Views.Select(x => ConvertView((AgnosticView)x)).ToList(),
            Scripts = database.Scripts.Select(x => ConvertScript(x)).ToList(),
        };
        specificDbmsDatabase.InitializeAdditionalProperties();
        _dbModelPostProcessor.DoSpecificDbmsDbModelCreationFromDefinitionPostProcessing(specificDbmsDatabase);
        _dbModelPostProcessor.OrderDbObjects(specificDbmsDatabase);
        _dependenciesBuilder.BuildDependencies(specificDbmsDatabase);
        return specificDbmsDatabase;
    }

    private Table ConvertTable(AgnosticTable table)
    {
        return new TTable()
        {
            ID = table.ID,
            Name = table.Name,
            Columns = ConvertColumns(table.Columns, table.Name),
            PrimaryKey = ConvertPrimaryKey(table.PrimaryKey, table.Name),
            UniqueConstraints = table.UniqueConstraints,
            CheckConstraints = table.CheckConstraints.Select(ck => { ck.CodePiece = ConvertCodePiece(ck.CodePiece); return ck; }).ToList(),
            Indexes = table.Indexes,
            Triggers = table.Triggers.Select(trigger => { trigger.CodePiece = ConvertCodePiece(trigger.CodePiece); return trigger; }).ToList(),
            ForeignKeys = table.ForeignKeys,
        };
    }

    private View ConvertView(AgnosticView view)
    {
        return new TView()
        {
            ID = view.ID,
            Name = view.Name,
            CodePiece = ConvertCodePiece(view.CodePiece),
        };
    }

    private List<Column> ConvertColumns(IEnumerable<Column> columns, string tableName)
    {
        List<Column> specificDbmsColumns = new();
        foreach (Column column in columns)
        {
            DataType dataType = column.DataType is AgnosticVerbatimDataType avdt
                ? new DataType { Name = ConvertCodePiece(avdt.NameCodePiece).Code }
                : _dataTypeConverter.Convert((CSharpDataType)column.DataType);

            CodePiece defaultValue = column.Default is AgnosticCodePiece acp
                ? ConvertCodePiece(acp)
                : _defaultValueConverter.Convert((CSharpDefaultValue)column.Default);

            TColumn specificDbmsColumn = new()
            {
                ID = column.ID,
                Name = column.Name,
                DataType = dataType,
                NotNull = column.NotNull,
                Identity = column.Identity,
                Default = defaultValue
            };
            BuildAdditionalColumnProperties(specificDbmsColumn, tableName);
            specificDbmsColumns.Add(specificDbmsColumn);
        }
        return specificDbmsColumns;
    }
    protected virtual void BuildAdditionalColumnProperties(TColumn column, string tableName) { }

    protected virtual PrimaryKey ConvertPrimaryKey(PrimaryKey pk, string tableName)
    {
        return pk;
    }

    private Script ConvertScript(Script script)
    {
        script.CodePiece = ConvertCodePiece(script.CodePiece);
        return script;
    }

    protected CodePiece ConvertCodePiece(CodePiece codePiece)
    {
        return new CodePiece { Code = ((AgnosticCodePiece)codePiece).DbKindToCodeMap[_databaseKind] };
    }
}
