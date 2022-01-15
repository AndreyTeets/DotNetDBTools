using System.Reflection;
using DotNetDBTools.DefinitionParsing;
using DotNetDBTools.Models.SQLite;
using DotNetDBTools.UnitTests.Utilities;
using FluentAssertions;
using Xunit;

namespace DotNetDBTools.UnitTests.DefinitionParsing.SQLite
{
    public class SQLiteBuildDbModelTests
    {
        [Fact]
        public void Generate_Description_For_AgnosticSampleDB_CreatesCorrectDescription()
        {
            Assembly dbAssemblyCSharp = TestDbAssembliesHelper.GetSampleDbAssembly("DotNetDBTools.SampleDBv2.SQLite");
            Assembly dbAssemblySql = TestDbAssembliesHelper.GetSampleDbAssembly("DotNetDBTools.SampleDBv2SqlDef.SQLite");
            SQLiteDatabase databaseCSharp = (SQLiteDatabase)DbDefinitionParser.CreateDatabaseModel(dbAssemblyCSharp);
            SQLiteDatabase databaseSql = (SQLiteDatabase)DbDefinitionParser.CreateDatabaseModel(dbAssemblySql);
            databaseSql.Should().BeEquivalentTo(databaseCSharp, options =>
                options.WithStrictOrdering().Excluding(database => database.Name));
        }
    }
}
