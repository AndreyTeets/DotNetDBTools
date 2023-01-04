using System.Reflection;
using DotNetDBTools.DefinitionParsing;
using DotNetDBTools.Models.Core;
using DotNetDBTools.UnitTests.Utilities;
using FluentAssertions;
using Xunit;

namespace DotNetDBTools.UnitTests.DefinitionParsing.Base;

public abstract class BaseBuildSqlDefDbModelTests<TDatabase>
    where TDatabase : Database, new()
{
    protected abstract string SpecificDbmsSampleDbV2AssemblyName { get; }
    protected abstract string SpecificDbmsSampleDbV2SqlDefAssemblyName { get; }

    private readonly Assembly _specificDbmsDbAssemblyV2;
    private readonly Assembly _specificDbmsDbAssemblyV2SqlDef;

    protected BaseBuildSqlDefDbModelTests()
    {
        _specificDbmsDbAssemblyV2 = TestDbAssembliesHelper.GetSampleDbAssembly(SpecificDbmsSampleDbV2AssemblyName);
        _specificDbmsDbAssemblyV2SqlDef = TestDbAssembliesHelper.GetSampleDbAssembly(SpecificDbmsSampleDbV2SqlDefAssemblyName);
    }

    [Fact]
    public void DbModelFromSqlDef_IsEquivalentTo_DbModelFromCSharpDef()
    {
        TDatabase databaseCSharp = (TDatabase)new DefinitionParsingManager().CreateDbModel(_specificDbmsDbAssemblyV2);
        TDatabase databaseSql = (TDatabase)new DefinitionParsingManager().CreateDbModel(_specificDbmsDbAssemblyV2SqlDef);
        databaseSql.Should().BeEquivalentTo(databaseCSharp, options => options.WithStrictOrdering());
    }
}
