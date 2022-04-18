using System.Reflection;
using DotNetDBTools.DefinitionParsing;
using DotNetDBTools.Models.SQLite;
using DotNetDBTools.UnitTests.DefinitionParsing.Base;
using DotNetDBTools.UnitTests.Utilities;
using FluentAssertions;
using Xunit;

namespace DotNetDBTools.UnitTests.DefinitionParsing.SQLite;

public class SQLiteBuildDbModelTests : BaseBuildDbModelTests<SQLiteDatabase>
{
    protected override string SpecificDbmsSampleDbV1AssemblyName => "DotNetDBTools.SampleDB.SQLite";
    protected override string ExpectedFilesDir => "./TestData/SQLite";

    [Fact]
    public void DbModelFromSqlDef_IsEquivalentTo_DbModelFromCSharpDef()
    {
        Assembly dbAssemblyCSharp = TestDbAssembliesHelper.GetSampleDbAssembly("DotNetDBTools.SampleDBv2.SQLite");
        Assembly dbAssemblySql = TestDbAssembliesHelper.GetSampleDbAssembly("DotNetDBTools.SampleDBv2SqlDef.SQLite");
        SQLiteDatabase databaseCSharp = (SQLiteDatabase)new DefinitionParsingManager().CreateDbModel(dbAssemblyCSharp);
        SQLiteDatabase databaseSql = (SQLiteDatabase)new DefinitionParsingManager().CreateDbModel(dbAssemblySql);
        databaseSql.Should().BeEquivalentTo(databaseCSharp, options => options.WithStrictOrdering());
    }
}
