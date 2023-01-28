using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.Models.Agnostic;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Analysis.Core;

internal abstract class DbModelConverter<
    TDatabase,
    TTable,
    TView,
    TIndex,
    TTrigger,
    TColumn>
    : IDbModelConverter
    where TDatabase : Database, new()
    where TTable : Table, new()
    where TView : View, new()
    where TIndex : Index, new()
    where TTrigger : Trigger, new()
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
        _dbModelPostProcessor.DoSpecificDbmsDbModelCreationFromDefinitionPostProcessing(specificDbmsDatabase);
        _dbModelPostProcessor.DoPostProcessing(specificDbmsDatabase);
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
            CheckConstraints = table.CheckConstraints.Select(ck => { ck.Expression = ConvertCodePiece(ck.Expression); return ck; }).ToList(),
            Indexes = ConvertIndexes(table.Indexes),
            Triggers = ConvertTriggers(table.Triggers),
            ForeignKeys = table.ForeignKeys,
        };
    }

    private View ConvertView(AgnosticView view)
    {
        return new TView()
        {
            ID = view.ID,
            Name = view.Name,
            CreateStatement = ConvertCodePiece(view.CreateStatement),
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

            TColumn specificDbmsColumn = new()
            {
                ID = column.ID,
                Name = column.Name,
                DataType = dataType,
                NotNull = column.NotNull,
                Identity = column.Identity,
                Default = ConvertDefaultValue(column.Default),
            };
            BuildAdditionalColumnProperties(specificDbmsColumn, tableName);
            specificDbmsColumns.Add(specificDbmsColumn);
        }
        return specificDbmsColumns;

        CodePiece ConvertDefaultValue(CodePiece dValue)
        {
            if (dValue is null)
                return null;
            if (dValue is AgnosticCodePiece acp)
                return ConvertCodePiece(acp);
            return _defaultValueConverter.Convert((CSharpDefaultValue)dValue);
        }
    }
    protected virtual void BuildAdditionalColumnProperties(TColumn column, string tableName) { }

    protected virtual PrimaryKey ConvertPrimaryKey(PrimaryKey pk, string tableName)
    {
        return pk;
    }

    protected virtual List<Index> ConvertIndexes(IEnumerable<Index> indexes)
    {
        List<Index> specificDbmsIndexes = new();
        foreach (Index index in indexes)
        {
            TIndex specificDbmsIndex = new()
            {
                ID = index.ID,
                Name = index.Name,
                Columns = index.Columns,
                IncludeColumns = index.IncludeColumns,
                Unique = index.Unique,
            };
            BuildAdditionalIndexProperties(specificDbmsIndex);
            specificDbmsIndexes.Add(specificDbmsIndex);
        };
        return specificDbmsIndexes;
    }
    protected virtual void BuildAdditionalIndexProperties(TIndex index) { }

    protected virtual List<Trigger> ConvertTriggers(IEnumerable<Trigger> triggers)
    {
        List<Trigger> specificDbmsTriggers = new();
        foreach (Trigger trigger in triggers)
        {
            TTrigger specificDbmsTrigger = new()
            {
                ID = trigger.ID,
                Name = trigger.Name,
                CreateStatement = ConvertCodePiece(trigger.CreateStatement),
            };
            specificDbmsTriggers.Add(specificDbmsTrigger);
        };
        return specificDbmsTriggers;
    }

    private Script ConvertScript(Script script)
    {
        script.Text = ConvertCodePiece(script.Text);
        return script;
    }

    protected CodePiece ConvertCodePiece(CodePiece codePiece)
    {
        return new CodePiece { Code = ((AgnosticCodePiece)codePiece).DbKindToCodeMap[_databaseKind] };
    }
}
