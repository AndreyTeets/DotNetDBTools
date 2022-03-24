using System.Reflection;
using DotNetDBTools.DefinitionParsing;
using DotNetDBTools.Models.SQLite;
using DotNetDBTools.UnitTests.Utilities;
using FluentAssertions;
using Xunit;

namespace DotNetDBTools.UnitTests.DefinitionParsing.SQLite;

public class SQLiteBuildDbModelTests
{
    [Fact]
    public void DbModelCreatedFrom_SampleDBv2SqlDef_IsEquivalentTo_DbModelCreatedFrom_SampleDBv2()
    {
        Assembly dbAssemblyCSharp = TestDbAssembliesHelper.GetSampleDbAssembly("DotNetDBTools.SampleDBv2.SQLite");
        Assembly dbAssemblySql = TestDbAssembliesHelper.GetSampleDbAssembly("DotNetDBTools.SampleDBv2SqlDef.SQLite");
        SQLiteDatabase databaseCSharp = (SQLiteDatabase)new GenericDbModelFromDefinitionProvider().CreateDbModel(dbAssemblyCSharp);
        SQLiteDatabase databaseSql = (SQLiteDatabase)new GenericDbModelFromDefinitionProvider().CreateDbModel(dbAssemblySql);
        databaseSql.Should().BeEquivalentTo(databaseCSharp, options =>
            options.WithStrictOrdering().Excluding(database => database.Name));
    }
}
