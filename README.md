# What is DotNetDBTools?
DotNetDBTools is a set of libraries to define, analyze and deploy (publish migrate) relational databases throughout their evolution using database as code approach.

Or in other words - state-based (declarative) database version control tools.

It provides the means to conveniently describe database objects for the supported DBMS as c# code or as sql in a declarative way, to analyze it and the means of publishing it to the selected DBMS with automatic differences calculation between what's being published and what's currently in the DBMS.

Agnostic definition for standard sql objects (Tables, Views, Indexes, Triggers) is also supported (as c# code only) and can then be used to publish to any supported DBMS.

## How it works
The calculation of differences is based on the idea of assigning unique identifiers to each object (table, column, foreign key, function, e.t.c.) in the database definition and then keeping a mapping in the DBMS between the objects and their corresponding identifiers (in a separate system table).
More specifically, during publish:
+ Creating new objects if there are no corresponding identifiers in the DBMS.
+ Altering only changed properties of objects (like changing the name of a table, or changing the data type of a column) if the corresponding identifiers exist in the DBMS.
+ Dropping objects if their corresponding identifiers exist in the DBMS but are no longer present in the database definition.

The identifiers are there to provide a reliable mapping between what's in the database definition and what's in the DBMS, because it can't be done by names only without keeping a history of every rename in the code, which isn't really a true state-based versioning.

## Example database definition as c# code ("verbatim" values are PostgreSQL only)
```
public class MyTable : ITable
{
    public Guid DNDBT_OBJECT_ID => new("299675E6-4FAA-4D0F-A36A-224306BA5BCB");

    public Column MyKeyColumn = new("A2F2A4DE-1337-4594-AE41-72ED4D05F317")
    {
        DataType = new IntDataType(),
        NotNull = true,
        Identity = true,
    };

    public Column MyDataColumn = new("FE68EE3D-09D0-40AC-93F9-5E441FBB4F70")
    {
        DataType = new StringDataType() { Length = 40 },
        Default = new StringDefaultValue("33"),
    };

    public Column MyDataColumn2 = new("34CB2ADB-3A6B-4BCD-A9F0-21321AB659C1")
    {
        DataType = new VerbatimDataType("TIMESTAMP"),
        NotNull = true,
        Default = new VerbatimDefaultValue("NOW()"),
    };

    public PrimaryKey PK_MyTable = new("37A45DEF-F4A0-4BE7-8BFB-8FBED4A7D705")
    {
        Columns = new[] { nameof(MyKeyColumn) },
    };

    public UniqueConstraint UQ_MyTable_MyDataColumn = new("F3F08522-26EE-4950-9135-22EDF2E4E0CF")
    {
        Columns = new[] { nameof(MyDataColumn) },
    };

    public Index IDX_MyTable_MyDataColumn2 = new("74390B3C-BC39-4860-A42E-12BAA400F927")
    {
        Columns = new[] { nameof(MyDataColumn2) },
        Unique = true,
    };
}
```

## Example database definition as sql (PostgreSQL)
```
--ID:#{299675E6-4FAA-4D0F-A36A-224306BA5BCB}#
CREATE TABLE "MyTable"
(
    --ID:#{A2F2A4DE-1337-4594-AE41-72ED4D05F317}#
    "MyKeyColumn" INT GENERATED ALWAYS AS IDENTITY NOT NULL,

    --ID:#{FE68EE3D-09D0-40AC-93F9-5E441FBB4F70}#
    "MyDataColumn" VARCHAR(40) DEFAULT 33,

    --ID:#{34CB2ADB-3A6B-4BCD-A9F0-21321AB659C1}#
    "MyDataColumn2" TIMESTAMP NOT NULL DEFAULT NOW(),

    --ID:#{37A45DEF-F4A0-4BE7-8BFB-8FBED4A7D705}#
    CONSTRAINT "PK_MyTable" PRIMARY KEY ("MyKeyColumn"),

    --ID:#{F3F08522-26EE-4950-9135-22EDF2E4E0CF}#
    CONSTRAINT "UQ_MyTable_MyDataColumn" UNIQUE ("MyDataColumn")
);

--ID:#{74390B3C-BC39-4860-A42E-12BAA400F927}#
CREATE UNIQUE INDEX "IDX_MyTable_MyDataColumn2"
    ON "MyTable" ("MyDataColumn2");
```

## Example usage of optionally generated description classes in business logic code
```
string sql =
$@"SELECT
    {MyTable.MyDataColumn}
FROM {MyTable}
WHERE {MyTable.MyKeyColumn} IN (1, 2)
    AND {MyTable.MyDataColumn} IS NOT NULL;";

IEnumerable<string> values = connection.Query<string>(sql); // Dapper call
```

## How is 'SQL Server Data Tools (SSDT)' different?
Although it provides analogous declaritive means for defining database objects, it still has to drag all the history of renames to make publish correctly work to support renames. And it's only available for MSSQL.

## How is 'Entity framework' different?
It also provides declarative means for defining database structure with different syntax, but the publishing process is entirely different and relies on dragging the whole history of every change done to the database. Also DotNetDBTools is not an ORM, it just provides the means to define database structure, analyze it and deploy it.

# Supported DBMS
+ MSSQL (oldest tested - 2017, latest tested - 2022)
+ MySQL (minimal required - 8.0.19, latest tested - 8.0.31)
+ PostgreSQL (oldest tested - 11.0, latest tested - 15.1)
+ SQLite (minimal required - 3.26.0, latest tested - 3.39.2)

# State of development
+ MSSQL - definition (as c# only) and generation of "Tables,Views,Indexes,Triggers,UserDefinedTypes" without specific for this DBMS advanced properties, deployment functionality without dependencies analysis, so deploy order for views or some cases with CK/Triggers dependencies recreation may be invalid, database analysis using just a few example checks. There are no plans to develop it any further.
+ MySQL - definition (as c# only) and generation of "Tables,Views,Indexes,Triggers" without specific for this DBMS advanced properties, deployment functionality without dependencies analysis, so deploy order for views or some cases with CK/Triggers dependencies recreation may be invalid, database analysis using just a few example checks. There are no plans to develop it any further.
+ PostgreSQL - definition (as c# and as sql) and generation of "Tables,Views,Indexes,Triggers,Sequences,Types,Functions,Procedures" with only some of the specific for this DBMS advanced properties, deployment functionality without some complex dependencies scenarios like recursive dependencies, database analysis using just a few example checks.
+ SQLite - definition (as c# and as sql) and generation of "Tables,Views,Indexes,Triggers", full deployment functionality, database analysis using just a few example checks.
+ Agnostic - definition (as c#) for "Tables,Views,Indexes,Triggers" and it's deployment functionality to all of the above DBMS in accordance with the corresponding development state of the specific DBMS above.

# How to use
1. Create a netstandard2.0 project for database definition.
2. Add a reference to the `DotNetDBTools.Definition` nuget package.
3. Optionally add references to the analyzer-packages for database analysis and description classes generation.
   * `DotNetDBTools.DefinitionAnalyzer`
   * `DotNetDBTools.DescriptionSourceGenerator`
4. Define database objects as c# code by creating classes derived from `ITable` (with members of type `Column`, `PrimaryKey`, `ForeignKey` e.t.c.), `IView`, `IFunction` e.t.c. or as sql by adding the assembly attribute `DatabaseSettings`, setting it's `DefinitionKind` property to the appropriate value and including .sql files containing object definitions as embedded resources. Embedded resources require creating and filling the custom `AdditionalFiles` msbuild property (see example csproj file below).
5. Create a standalone console project with a reference to the `DotNetDBTools.Deploy` package and use the provided `MSSQLDeployManager` / `MySQLDeployManager` / `PostgreSQLDeployManager` / `SQLiteDeployManager` classes to create a simple deployment tool. After that use this tool to publish the database from the compiled database definition project output dll to the DBMS as it evolves. Or alternatively reference the `DotNetDBTools.Deploy` package right in a business logic project and add the database publish logic right there.
6. Optionally reference the database definition project in a business logic project to use description classes generated by the `DotNetDBTools.DescriptionSourceGenerator` analyzer-package added to the database definition project in step 3.

Generation of publish scripts is supported as well.

It is also possible to generate definition from an already existing database and then keep managing it with DotNetDBTools.

Here is a concise list of all the available database management methods:
```
public interface IDeployManager
{
    public bool IsRegisteredAsDNDBT(IDbConnection connection);
    public void RegisterAsDNDBT(IDbConnection connection);
    public void RegisterAsDNDBT(IDbConnection connection, string dbWithDNDBTInfoAssemblyPath);
    public void RegisterAsDNDBT(IDbConnection connection, Assembly dbWithDNDBTInfoAssembly);
    public void RegisterAsDNDBT(IDbConnection connection, Database dbWithDNDBTInfo);
    public void UnregisterAsDNDBT(IDbConnection connection);

    public void PublishDatabase(string dbAssemblyPath, IDbConnection connection);
    public void PublishDatabase(Assembly dbAssembly, IDbConnection connection);
    public void PublishDatabase(Database database, IDbConnection connection);

    public string GeneratePublishScript(string dbAssemblyPath, IDbConnection connection);
    public string GeneratePublishScript(Assembly dbAssembly, IDbConnection connection);
    public string GeneratePublishScript(Database database, IDbConnection connection);

    public string GeneratePublishScript(string dbAssemblyPath);
    public string GeneratePublishScript(Assembly dbAssembly);
    public string GeneratePublishScript(Database database);

    public string GeneratePublishScript(string newDbAssemblyPath, string oldDbAssemblyPath);
    public string GeneratePublishScript(Assembly newDbAssembly, Assembly oldDbAssembly);
    public string GeneratePublishScript(Database newDatabase, Database oldDatabase);

    public string GenerateNoDNDBTInfoPublishScript(string dbAssemblyPath);
    public string GenerateNoDNDBTInfoPublishScript(Assembly dbAssembly);
    public string GenerateNoDNDBTInfoPublishScript(Database database);

    public string GenerateNoDNDBTInfoPublishScript(string newDbAssemblyPath, string oldDbAssemblyPath);
    public string GenerateNoDNDBTInfoPublishScript(Assembly newDbAssembly, Assembly oldDbAssembly);
    public string GenerateNoDNDBTInfoPublishScript(Database newDatabase, Database oldDatabase);

    public void GenerateDefinition(IDbConnection connection, string outputDirectory);
    public void GenerateDefinition(IDbConnection connection, GenerationOptions generationOptions, string outputDirectory);

    public Database CreateDatabaseModelUsingDNDBTSysInfo(IDbConnection connection);
    public Database CreateDatabaseModelUsingDBMSSysInfo(IDbConnection connection);
}
```

## Example csproj file for a database definition project
```
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <LangVersion>9.0</LangVersion>
    <TargetFramework>netstandard2.0</TargetFramework>
    <EmitCompilerGeneratedFiles>true</EmitCompilerGeneratedFiles>
    <CompilerGeneratedFilesOutputPath>obj/Generated</CompilerGeneratedFilesOutputPath>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="DotNetDBTools.Definition" PrivateAssets="all" />
    <PackageReference Include="DotNetDBTools.DefinitionAnalyzer" PrivateAssets="all" />
    <PackageReference Include="DotNetDBTools.DescriptionSourceGenerator" PrivateAssets="all" />
  </ItemGroup>

  <ItemGroup>
    <EmbeddedResource Include="**\*.sql">
      <CopyToOutputDirectory>Never</CopyToOutputDirectory>
    </EmbeddedResource>
    <CompilerVisibleItemMetadata Include="AdditionalFiles" MetadataName="Category" />
    <AdditionalFiles Include="@(EmbeddedResource)" Category="EmbeddedFile" />
  </ItemGroup>

</Project>
```

## Example code to publish a database
Note: the database must exist and has to be registered (except for generation of NoDNDBTInfo publish scripts) as DotNetDBTools-managed using the `deployManager.RegisterAsDNDBT(connection)` method before using the publish methods.

Create an instance of IDeployManager for the appropriate DBMS, using non-default options if needed, and an instance of IDbConnection to pass to the deployManager methods like this:
```
IDeployManager deployManager = new PostgreSQLDeployManager(new DeployOptions() { AllowDataLoss = true });
using IDbConnection connection = new SqlConnection(SomePostgreSQLConnectionString);
```
And then publish the database like this:
```
deployManager.PublishDatabase("./MyDatabase.dll", connection);
```
Or like this:
```
Assembly dbAssembly = Assembly.Load(File.ReadAllBytes("./MyDatabase.dll"));
deployManager.PublishDatabase(dbAssembly, connection);
```
Or like this (if the database definition project is referenced):
```
deployManager.PublishDatabase(typeof(MyDatabase.Tables.MyTable).Assembly, connection);
```
Or create a publish-sql-script and later execute it like this:
```
string scriptText = deployManager.GeneratePublishScript("./MyDatabase.dll", connection);
File.WriteAllText("./publishScript.sql", scriptText);
connection.Execute(File.ReadAllText("./publishScript.sql")); // Dapper call
```
Or do something more complex like this:
```
using Dapper;
using DotNetDBTools.Analysis;
using DotNetDBTools.Analysis.Errors;
using DotNetDBTools.DefinitionParsing;
using DotNetDBTools.Deploy;
using DotNetDBTools.Generation;
using DotNetDBTools.Models.Core;
using Npgsql;

const string ConnectionString = @"Host=127.0.0.1;Port=5007;Database=my_db;Username=postgres;Password=Strong(!)Passw0rd";
const string PathToGeneratedProjectV1 = "./generated_project_v1";
const string PathToGeneratedProjectV2 = "./generated_project_v2";
const string PathToUpdateScriptFile = "./update_v1_to_v2.sql";

// Create db model from the currently existing non-dndbt-registered database.
using NpgsqlConnection connection = new(ConnectionString);
Database dbLoadedFromDbms = new PostgreSQLDeployManager().CreateDatabaseModelUsingDBMSSysInfo(connection);

// Generate database definition projects for it using sql-definition format.
GenerationManager generator = new(new GenerationOptions() { OutputDefinitionKind = OutputDefinitionKind.Sql });
generator.GenerateDefinition(dbLoadedFromDbms, PathToGeneratedProjectV1);
generator.GenerateDefinition(dbLoadedFromDbms, PathToGeneratedProjectV2);
// Save generated files to a git repo, modify something in v2.

// Create db models from sql files in the generated database definition projects.
// (loading v1 from the DBMS again would generate new IDs for every object because it's unregistered)
static Database LoadDatabaseDirectlyFromSqlFiles(string pathToProject, int versionToSetForDbModel)
{
    List<string> statements = new();
    foreach (string filePath in Directory.GetFiles(pathToProject, "*.sql", SearchOption.AllDirectories))
    {
        string createDbObjectStatementWithIdDeclarations = File.ReadAllText(filePath);
        statements.Add(createDbObjectStatementWithIdDeclarations);
    }
    Database db = new DefinitionParsingManager().CreateDbModel(statements, versionToSetForDbModel, DatabaseKind.PostgreSQL);
    if (!new AnalysisManager().DbIsValid(db, out List<DbError> dbErrors))
        throw new Exception($"Db is invalid:\n{string.Join("\n", dbErrors.Select(x => x.ErrorMessage))}");
    return db;
}
Database dbV1 = LoadDatabaseDirectlyFromSqlFiles(PathToGeneratedProjectV1, 1);
Database dbV2 = LoadDatabaseDirectlyFromSqlFiles(PathToGeneratedProjectV2, 2);

// Print all removed tables/columns.
DatabaseDiff dbDiff = new AnalysisManager().CreateDatabaseDiff(dbV2, dbV1);
foreach (Table table in dbDiff.RemovedTables)
{
    string tableSqlDefinition = GenerationManager.GenerateSqlCreateStatement(table, includeIdDeclarations: true);
    Console.WriteLine($"Table was removed:\n{tableSqlDefinition}");
}
foreach (TableDiff tDiff in dbDiff.ChangedTables)
{
    foreach (Column c in tDiff.ColumnsToDrop)
    {
        string changeInfo = $"Column was removed: TableName: {tDiff.OldName}, ColumnName: {c.Name}";
        Console.WriteLine(changeInfo);
    }
}
// Manually inspect/validate it.

// Generate an update script without any DNDBT info, which will update the database from state V1 to state V2.
string updateScript = new PostgreSQLDeployManager(new DeployOptions() { AllowDataLoss = true })
    .GenerateNoDNDBTInfoPublishScript(dbV2, dbV1);
File.WriteAllText(PathToUpdateScriptFile, updateScript);
// Manually inspect/validate it.

// Execute the update script later.
connection.Execute(File.ReadAllText(PathToUpdateScriptFile));
```

# Additional information
More information on usage with end-to-end working examples can be found in the [/samples](/samples) directory of this project. For even more complex scenarios one can check out the available public classes in the Analysis, CodeParsing, DefinitionParsing, Generation packages and look at the code in the test projects or in the DeployManager class to find examples of their usage.

In order to run the samples, a docker container with the corresponding DBMS has to be started (except for SQLite, which works out of the box). To start containers simply run any (or all) integration test for the chosen DBMS, it will create the appropriate container with the required parameters and leave it running. SampleBusinessLogicOnlyApp.* samples require running the corresponding SampleDeployManagerUsage.* sample first to create the appropriate database.

Building solution requires a java runtime installed (required by the Antlr4BuildTasks package, which generates c# files from ANTLR grammars during the CodeParsing project build). Running samples and tests requires the appropriate for their projects dotnet runtimes installed (currently it's `net6.0` for the DeployTool project and `netcoreapp3.1` for the rest).

Here is a concise list of some potentially helpful classes/methods in other packages:
```
public interface DotNetDBTools.DefinitionParsing.IDefinitionParsingManager
{
    public Database CreateDbModel(string dbAssemblyPath);
    public Database CreateDbModel(Assembly dbAssembly);
    public Database CreateDbModel(IEnumerable<string> definitionSqlStatements, long dbVersion, DatabaseKind dbKind);
}

public interface DotNetDBTools.Generation.IGenerationManager
{
    public string GenerateDescription(Database database);
    public IEnumerable<DefinitionSourceFile> GenerateDefinition(Database database);
}

public class DotNetDBTools.Generation.GenerationManager
{
    public static string GenerateSqlCreateStatement(DbObject dbObject, bool includeIdDeclarations);
    public static string GenerateSqlDropStatement(DbObject dbObject);
    public static string GenerateSqlAlterStatement(TableDiff tableDiff);
}

public interface DotNetDBTools.Analysis.IAnalysisManager
{
    public bool DbIsValid(Database database, out List<DbError> dbErrors);
    public bool DatabasesAreEquivalentExcludingDNDBTInfo(Database database1, Database database2, out string diffLog);
    public bool DbObjectsAreEquivalentExcludingDNDBTInfo(DbObject dbObject1, DbObject dbObject2, out string diffLog);

    public DatabaseDiff CreateDatabaseDiff(Database newDatabase, Database oldDatabase);
    public bool DiffLeadsToDataLoss(DatabaseDiff dbDiff);
    public bool DiffIsEmpty(DatabaseDiff dbDiff);
    public bool DiffIsEmpty(DbObjectDiff dbObjectDiff);

    public Database ConvertFromAgnostic(Database database, DatabaseKind targetKind);
}

public interface DotNetDBTools.CodeParsing.ICodeParser
{
    public ObjectInfo GetObjectInfo(string createStatement);
}

public static class DotNetDBTools.CodeParsing.PostgreSQLStatementsSplitter
{
    public static List<string> Split(string statementsStr);
}

public class DotNetDBTools.CodeParsing.PostgreSQLCodeParser
{
    public List<Dependency> GetFunctionDependencies(string createFunctionStatement, out string language);
    public List<Dependency> GetViewDependencies(string createViewStatement);
    public List<Dependency> GetExpressionDependencies(string expressionStatement)
}

public class DotNetDBTools.CodeParsing.SQLiteCodeParser
{
    public List<Dependency> GetViewDependencies(string createViewStatement);
}

public static class DotNetDBTools.Analysis.Extensions.DbObjectsExtensions
{
    public static IEnumerable<DbObject> GetTransitiveDependencies(
        this DbObject dbObject, Func<DbObject, IEnumerable<DbObject>> getDependenciesFunc)
    public static IEnumerable<DbObject> OrderByDependenciesLast(
        this IEnumerable<DbObject> dbObjects, Func<DbObject, IEnumerable<DbObject>> getDependenciesFunc);
    public static IEnumerable<DbObject> OrderByDependenciesFirst(
        this IEnumerable<DbObject> dbObjects, Func<DbObject, IEnumerable<DbObject>> getDependenciesFunc);
}

public static class DotNetDBTools.Models.ExtensionMethods
{
    public static T CopyModel<T>(this T original)
}
```

# Licence
MIT License. See [LICENSE](LICENSE) file.