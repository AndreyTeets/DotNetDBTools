<h1 align="center">DotNetDBTools</h1>

DotNetDBTools is a set of libraries to define and deploy relational databases. Or in other words - database version control tools.<br/>
It provides the means to conviniently describe database structure for supported DBMS's in c# code, to analyze that structure and the means of publishing it to the selected DBMS with automatic differences calculation between what's being published and what's currently in the DBMS now.<br/>

## More details
Differences are calculated on the idea of assigning unique identifiers to each object (table, column, foreign key, function, e.t.c.) in database description (and saving them in DBMS as well) and then creating new objects during publish if there's no record of this identifier in DBMS now, altering only changed properties of this object (like changing name for a table, or changing data type for a column) if there's a record of this identifier in DBMS now and dropping objects if object was deleted in description but still exists in DBMS. Identifiers are there to provide a reliable mapping between what's in the description and what's in DBMS because it can't be done with names which often change.

## Example database structure description
```
public class MyTable : ITable
{
    public Guid ID => new("299675E6-4FAA-4D0F-A36A-224306BA5BCB");

    public Column MyKeyColumn = new("A2F2A4DE-1337-4594-AE41-72ED4D05F317")
    {
        DataType = new IntDataType(),
        Identity = true,
    };

    public Column MyDataColumn = new("FE68EE3D-09D0-40AC-93F9-5E441FBB4F70")
    {
        DataType = new StringDataType() { Length = 40 },
        Default = "33",
        Nullable = true,
    };

    public PrimaryKey PK_MyTable = new("37A45DEF-F4A0-4BE7-8BFB-8FBED4A7D705")
    {
        Columns = new string[] { nameof(MyKeyColumn) },
    };

    public UniqueConstraint UQ_MyTable_MyDataColumn = new("F3F08522-26EE-4950-9135-22EDF2E4E0CF")
    {
        Columns = new string[] { nameof(MyDataColumn) },
    };
}
```

## Example usage of additionally generated description classes for use in business logic
```
private static void ReadSomeData()
{
    string sql =
$@"SELECT
    {MyTable.MyDataColumn}
FROM {MyTable}
WHERE {MyTable.MyKeyColumn} IN (1, 2)
    AND {MyTable.MyDataColumn} IS NOT NULL;";

    IEnumerable<string> values = connection.Query<string>(sql); // Dapper call
}
```

## How is 'SQL Server Data Tools (SSDT)' different
Although it provides analogous declaritive means for database structure description (except that description is done by pure sql-create statements instead of c# code) it still has to drag all the history of renames to make publish correctly work to support renames. Plus it's only available for MSSQL.

## How is 'Entity framework' different
Also provides declarative means for database structure description with different syntax, but publishing process is entirely different and relies on dragging the whole history of every change done to database. Also DotNetDBTools is not an ORM, it just provides means to describe and deploy database structure and analysis on this structure.

# Currently supported DBMS
+ MSSQL (in development, basic user defined types and table descriptions and their deployment work, presumably with major bugs)
+ MySQL (development hasn't started yet)
+ Postgres (development hasn't started yet)
+ SQLite (in development, basic table descriptions and their deployment  work, presumably with major bugs)

# How to use
1. Create a netstandard2.0 project for a database decription.
2. Add reference to `DotNetDBTools.Definition` nuget package.
3. Optionally add references to additional analyzer-packages for database analysis and additional description classes source-generation.
   * `DotNetDBTools.DefinitionAnalyzer`
   * `DotNetDBTools.DescriptionSourceGenerator`
4. Describe database structure by creating classes derived from `ITable` (with members of type `Column`, `PrimaryKey`, `ForeignKey` e.t.c.), `IView`, `IFunction` e.t.c.
5. Create a standalone console project with a reference to `DotNetDBTools.Deploy` nuget package and use provided `MSSQLDeployManager` / `MySQLDeployManager` / `PostgresDeployManager` / `SQLiteDeployManager` classes to create a simple deployment tool, after that use this tool to publish database from compiled database project output dll to DBMS as it evolves. Or alternatively reference `DotNetDBTools.Deploy` package right in business logic application and add database publish logic right there.
6. Optionally reference database project in business application to use additional description classes generated by `DotNetDBTools.DescriptionSourceGenerator`.

## Example csproj file for database description project
```
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>netstandard2.0</TargetFramework>
    <EmitCompilerGeneratedFiles>true</EmitCompilerGeneratedFiles>
    <CompilerGeneratedFilesOutputPath>obj/Generated</CompilerGeneratedFilesOutputPath>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="DotNetDBTools.Definition" Version="0.1.0" PrivateAssets="all" />
    <PackageReference Include="DotNetDBTools.DefinitionAnalyzer" Version="0.1.0" PrivateAssets="all" />
    <PackageReference Include="DotNetDBTools.DescriptionSourceGenerator" Version="0.1.0" PrivateAssets="all" />
  </ItemGroup>

</Project>
```

## Example code to publish database
Like this
```
IDeployManager deployManager = new MSSQLDeployManager(true, false);
deployManager.PublishDatabase("./MyDatabase.dll", _connectionString);
```
Or like this
```
IDeployManager deployManager = new MSSQLDeployManager(true, false);
Assembly dbAssembly = Assembly.Load(File.ReadAllBytes("./MyDatabase.dll"));
deployManager.PublishDatabase(dbAssembly, _connectionString);
```
Or like this (if database project is referenced)
```
IDeployManager deployManager = new MSSQLDeployManager(true, false);
deployManager.PublishDatabase(typeof(MyDatabase.Tables.MyTable).Assembly, _connectionString);
```
Or create publish sql-script and later execute it like this
```
IDeployManager deployManager = new MSSQLDeployManager(true, false);
deployManager.GeneratePublishScript("./MyDatabase.dll", _connectionString, "./publishScript.sql");
new SqlConnection(_connectionString).Execute(File.ReadAllText("./publishScript.sql")); // Dapper call
```

# Additional information
More end-to-end working examples can be found in /samples directory of this project.<br/>
In order to run these samples docker container with appropriate DBMS have to be started (except for SQLite which works out of the box).
To start containers simply run any(or all) integration test for the chosen DBMS, it will create container with required parameters and leave it running.

# Licence
MIT License. See [LICENSE](LICENSE) file.