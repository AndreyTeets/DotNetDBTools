# What is DotNetDBTools?
DotNetDBTools is a set of libraries to define, analyze and deploy (publish migrate) relational databases throughout their evolution using database as code approach.

Or in other words - state-based (declarative) database version control tools.

It provides the means to conviniently describe database structure for the supported DBMS as c# code or as sql in a declarative way, to analyze that structure and the means of publishing it to the selected DBMS with automatic differences calculation between what's being published and what's currently in the DBMS now.

Agnostic description for standard sql objects (Tables, Views, Indexes, Triggers) is also supported (as c# code only) and can then be used to publish to any supported DBMS.

## How it works
Differences are calculated on the idea of assigning unique identifiers to each object (table, column, foreign key, function, e.t.c.) in database description (and saving them in DBMS as well) and then:
+ Creating new objects during publish if there's no record of this identifier in DBMS now.
+ Altering only changed properties of this object (like changing name for a table, or changing data type for a column) if there's a record of this identifier in DBMS now.
+ Dropping objects if object was deleted in description but still exists in DBMS.

Identifiers are there to provide a reliable mapping between what's in the description and what's in DBMS because it can't be done with names which often change.

## Example database structure description as c# code
```
public class MyTable : ITable
{
    public Guid ID => new("299675E6-4FAA-4D0F-A36A-224306BA5BCB");

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

## Example database structure description as sql(PostgreSQL)
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

## Example usage of additionally generated description classes for use in business logic
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
Although it provides analogous declaritive means for database structure description it still has to drag all the history of renames to make publish correctly work to support renames. Plus it's only available for MSSQL.

## How is 'Entity framework' different?
Also provides declarative means for database structure description with different syntax, but publishing process is entirely different and relies on dragging the whole history of every change done to database. Also DotNetDBTools is not an ORM, it just provides means to describe and deploy database structure and analysis on this structure.

# Supported DBMS
+ MSSQL
+ MySQL
+ PostgreSQL
+ SQLite

# State of development
+ MSSQL - definition (as c#) of only basic "standard relational db entities" and it's deployment+generation seem to work, database analysis using just a few example checks.
+ MySQL - definition (as c#) of only basic "standard relational db entities" and it's deployment+generation seem to work, database analysis using just a few example checks.
+ PostgreSQL - definition (as c#) of most "standard relational db entities" and it's deployment+generation seem to work, database analysis using just a few example checks.
+ SQLite - definition (as c# and as sql) of all "standard sqlite entities" and it's deployment+generation seem to work, database analysis using just a few example checks.
+ Agnostic definition for "standard relational db entities" and it's deployment to all the above DBMS according with corresponding development state of the specific DBMS above.

# How to use
1. Create a netstandard2.0 project for a database decription.
2. Add reference to `DotNetDBTools.Definition` nuget package.
3. Optionally add references to additional analyzer-packages for database analysis and additional description classes source-generation.
   * `DotNetDBTools.DefinitionAnalyzer`
   * `DotNetDBTools.DescriptionSourceGenerator`
4. Describe database structure as c# code by creating classes derived from `ITable` (with members of type `Column`, `PrimaryKey`, `ForeignKey` e.t.c.), `IView`, `IFunction` e.t.c. or as sql by setting assembly attribute `DatabaseSettings.DefinitionKind` and including .sql files with definition as embedded resources. In both cases embedded resources require creating and filling custom `AdditionalFiles` msbuild property.
5. Create a standalone console project with a reference to `DotNetDBTools.Deploy` nuget package and use provided `MSSQLDeployManager` / `MySQLDeployManager` / `PostgreSQLDeployManager` / `SQLiteDeployManager` classes to create a simple deployment tool, after that use this tool to publish database from compiled database project output dll to DBMS as it evolves. Or alternatively reference `DotNetDBTools.Deploy` package right in business logic application and add database publish logic right there.
6. Optionally reference database project in business application to use additional description classes generated by `DotNetDBTools.DescriptionSourceGenerator`.

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

    public Database CreateDatabaseModelUsingDNDBTSysInfo(IDbConnection connection);
    public Database CreateDatabaseModelUsingDBMSSysInfo(IDbConnection connection);
}
```

## Example csproj file for database description project
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

## Example code to publish database
Note: database must exist and has to be registered with DotNetDBTools using `deployManager.RegisterAsDNDBT(connection)` method before using publish methods.

Create an instance of IDeployManager for the appropriate DBMS using non-default options if needed and a IDbConnection instance to pass to deployManager methods
```
IDeployManager deployManager = new MSSQLDeployManager(new DeployOptions() { AllowDataLoss = true });
using IDbConnection connection = new SqlConnection(SomeMSSQLConnectionString);
```
And then publish database like this
```
deployManager.PublishDatabase("./MyDatabase.dll", connection);
```
Or like this
```
Assembly dbAssembly = Assembly.Load(File.ReadAllBytes("./MyDatabase.dll"));
deployManager.PublishDatabase(dbAssembly, connection);
```
Or like this (if database project is referenced)
```
deployManager.PublishDatabase(typeof(MyDatabase.Tables.MyTable).Assembly, connection);
```
Or create publish sql-script and later execute it like this
```
string scriptText = deployManager.GeneratePublishScript("./MyDatabase.dll", connection);
File.WriteAllText("./publishScript.sql", scriptText);
connection.Execute(File.ReadAllText("./publishScript.sql")); // Dapper call
```

# Additional information
More information on usage with end-to-end working examples can be found in [/samples](/samples) directory of this project. For even more complex scenarios one can check out available public classes in Analysis, CodeParsing, DefinitionParsing, Generation packages and look at the code in tests or in DeployManager class to find examples of their usage.

In order to run sample applications docker container with the appropriate DBMS has to be started (except for SQLite which works out of the box). To start containers simply run any (or all) integration test for the chosen DBMS, it will create container with required parameters and leave it running. SampleBusinessLogicOnlyApp.* samples require running corresponding SampleDeployManagerUsage.* sample first to create appropriate db.

# Licence
MIT License. See [LICENSE](LICENSE) file.