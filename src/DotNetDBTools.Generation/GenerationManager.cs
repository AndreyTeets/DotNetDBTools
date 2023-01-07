using System;
using System.Collections.Generic;
using System.IO;
using DotNetDBTools.Generation.Agnostic.Description;
using DotNetDBTools.Generation.Core;
using DotNetDBTools.Generation.Core.Definition;
using DotNetDBTools.Generation.Core.Sql;
using DotNetDBTools.Generation.MSSQL.Definition;
using DotNetDBTools.Generation.MSSQL.Description;
using DotNetDBTools.Generation.MSSQL.Sql;
using DotNetDBTools.Generation.MySQL.Definition;
using DotNetDBTools.Generation.MySQL.Description;
using DotNetDBTools.Generation.MySQL.Sql;
using DotNetDBTools.Generation.PostgreSQL.Definition;
using DotNetDBTools.Generation.PostgreSQL.Description;
using DotNetDBTools.Generation.PostgreSQL.Sql;
using DotNetDBTools.Generation.SQLite.Definition;
using DotNetDBTools.Generation.SQLite.Description;
using DotNetDBTools.Generation.SQLite.Sql;
using DotNetDBTools.Models.Agnostic;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.MSSQL;
using DotNetDBTools.Models.MSSQL.UserDefinedTypes;
using DotNetDBTools.Models.MySQL;
using DotNetDBTools.Models.PostgreSQL;
using DotNetDBTools.Models.PostgreSQL.UserDefinedTypes;
using DotNetDBTools.Models.SQLite;

namespace DotNetDBTools.Generation;

public class GenerationManager : IGenerationManager
{
    public GenerationOptions Options { get; set; }

    public GenerationManager()
        : this(new GenerationOptions())
    {
    }

    public GenerationManager(GenerationOptions options)
    {
        Options = options;
    }

    public void GenerateDescription(Database database, string outputPath)
    {
        string generatedDescription = GenerateDescription(database);
        string fullPath = Path.GetFullPath(outputPath);
        Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
        File.WriteAllText(fullPath, generatedDescription);
    }

    public string GenerateDescription(Database database)
    {
        return database.Kind switch
        {
            DatabaseKind.Agnostic => AgnosticDescriptionGenerator.GenerateDescription((AgnosticDatabase)database, Options),
            DatabaseKind.MSSQL => MSSQLDescriptionGenerator.GenerateDescription((MSSQLDatabase)database, Options),
            DatabaseKind.MySQL => MySQLDescriptionGenerator.GenerateDescription((MySQLDatabase)database, Options),
            DatabaseKind.PostgreSQL => PostgreSQLDescriptionGenerator.GenerateDescription((PostgreSQLDatabase)database, Options),
            DatabaseKind.SQLite => SQLiteDescriptionGenerator.GenerateDescription((SQLiteDatabase)database, Options),
            _ => throw new InvalidOperationException($"Invalid dbKind: {database.Kind}"),
        };
    }

    public void GenerateDefinition(Database database, string outputDirectory)
    {
        if (Directory.Exists(outputDirectory))
            Directory.Delete(outputDirectory, true);

        IEnumerable<DefinitionSourceFile> definitionSourceFiles = GenerateDefinition(database);
        foreach (DefinitionSourceFile file in definitionSourceFiles)
        {
            string fullPath = Path.GetFullPath(Path.Combine(outputDirectory, file.RelativePath));
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            File.WriteAllText(fullPath, file.SourceText);
        }
    }

    public IEnumerable<DefinitionSourceFile> GenerateDefinition(Database database)
    {
        IDefinitionGenerator definitionGenerator = database.Kind switch
        {
            DatabaseKind.MSSQL => new MSSQLDefinitionGenerator(),
            DatabaseKind.MySQL => new MySQLDefinitionGenerator(),
            DatabaseKind.PostgreSQL => new PostgreSQLDefinitionGenerator(),
            DatabaseKind.SQLite => new SQLiteDefinitionGenerator(),
            _ => throw new InvalidOperationException($"Invalid dbKind: {database.Kind}"),
        };
        definitionGenerator.OutputDefinitionKind = Options.OutputDefinitionKind;
        return definitionGenerator.GenerateDefinition(database, Options.DatabaseName);
    }

    /// <summary>
    /// Generates appropriate create statement for the provided dbObject model.
    /// DBMS type is chosen based on the passed dbObject model type.
    /// </summary>
    public static string GenerateSqlCreateStatement(DbObject dbObject, bool includeIdDeclarations)
    {
        IStatementsGenerator statementsGenerator = CreateAppropriateStatementsGenerator(dbObject);
        statementsGenerator.IncludeIdDeclarations = includeIdDeclarations;
        return statementsGenerator.GetCreateSql(dbObject).NormalizeLineEndings();
    }

    /// <summary>
    /// Generates appropriate drop statement for the provided dbObject model.
    /// DBMS type is chosen based on the passed dbObject model type.
    /// </summary>
    public static string GenerateSqlDropStatement(DbObject dbObject)
    {
        IStatementsGenerator statementsGenerator = CreateAppropriateStatementsGenerator(dbObject);
        return statementsGenerator.GetDropSql(dbObject).NormalizeLineEndings();
    }

    /// <summary>
    /// Generates appropriate alter statement for the provided tableDiff model.
    /// DBMS type is chosen based on the passed dbObject model type.
    /// </summary>
    public static string GenerateSqlAlterStatement(TableDiff tableDiff)
    {
        IAlterStatementGenerator alterStatementGenerator = CreateAppropriateAlterStatementGenerator(tableDiff);
        return alterStatementGenerator.GetAlterSql(tableDiff).NormalizeLineEndings();
    }

    private static IStatementsGenerator CreateAppropriateStatementsGenerator(DbObject dbObject)
    {
        IStatementsGenerator statementsGenerator = dbObject switch
        {
            MSSQLTable x => new MSSQLTableStatementsGenerator(),
            MSSQLView x => new MSSQLViewStatementsGenerator(),
            MSSQLIndex x => new MSSQLIndexStatementsGenerator(),
            MSSQLTrigger x => new MSSQLTriggerStatementsGenerator(),
            MSSQLUserDefinedType x => new MSSQLTypeStatementsGenerator(),
            MySQLTable x => new MySQLTableStatementsGenerator(),
            MySQLView x => new MySQLViewStatementsGenerator(),
            MySQLIndex x => new MySQLIndexStatementsGenerator(),
            MySQLTrigger x => new MySQLTriggerStatementsGenerator(),
            PostgreSQLTable x => new PostgreSQLTableStatementsGenerator(),
            PostgreSQLView x => new PostgreSQLViewStatementsGenerator(),
            PostgreSQLIndex x => new PostgreSQLIndexStatementsGenerator(),
            PostgreSQLTrigger x => new PostgreSQLTriggerStatementsGenerator(),
            PostgreSQLCompositeType x => new PostgreSQLCompositeTypeStatementsGenerator(),
            PostgreSQLDomainType x => new PostgreSQLDomainTypeStatementsGenerator(),
            PostgreSQLEnumType x => new PostgreSQLEnumTypeStatementsGenerator(),
            PostgreSQLRangeType x => new PostgreSQLRangeTypeStatementsGenerator(),
            PostgreSQLFunction x => new PostgreSQLFunctionStatementsGenerator(),
            SQLiteTable x => new SQLiteTableStatementsGenerator(),
            SQLiteView x => new SQLiteViewStatementsGenerator(),
            SQLiteIndex x => new SQLiteIndexStatementsGenerator(),
            SQLiteTrigger x => new SQLiteTriggerStatementsGenerator(),
            _ => throw new InvalidOperationException($"Invalid dbObject type for statements generation: {dbObject.GetType()}"),
        };
        return statementsGenerator;
    }

    private static IAlterStatementGenerator CreateAppropriateAlterStatementGenerator(TableDiff tableDiff)
    {
        IAlterStatementGenerator statementsGenerator = tableDiff switch
        {
            MSSQLTableDiff x => new MSSQLTableStatementsGenerator(),
            MySQLTableDiff x => new MySQLTableStatementsGenerator(),
            PostgreSQLTableDiff x => new PostgreSQLTableStatementsGenerator(),
            SQLiteTableDiff x => new SQLiteTableStatementsGenerator(),
            _ => throw new InvalidOperationException($"Invalid tableDiff type for alter statement generation: {tableDiff.GetType()}"),
        };
        return statementsGenerator;
    }
}
