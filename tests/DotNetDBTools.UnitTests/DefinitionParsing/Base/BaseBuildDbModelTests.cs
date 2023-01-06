using System.Reflection;
using DotNetDBTools.DefinitionParsing;
using DotNetDBTools.Models.Core;
using DotNetDBTools.UnitTests.Utilities;
using FluentAssertions;
using Xunit;

namespace DotNetDBTools.UnitTests.DefinitionParsing.Base;

public abstract class BaseBuildDbModelTests<TDatabase>
    where TDatabase : Database, new()
{
    protected abstract string ExpectedFilesDir { get; }
    protected abstract string SpecificDbmsSampleDbV1AssemblyName { get; }

    private readonly Assembly _specificDbmsDbAssemblyV1;

    protected BaseBuildDbModelTests()
    {
        _specificDbmsDbAssemblyV1 = MiscHelper.GetSampleDbAssembly(SpecificDbmsSampleDbV1AssemblyName);
    }

    [Fact]
    public void CreateDbModel_FromCSharpDef_CreatesCorrectModel()
    {
        TDatabase db = (TDatabase)new DefinitionParsingManager().CreateDbModel(_specificDbmsDbAssemblyV1);
        string actualDbModel = MiscHelper.SerializeToJsonWithReferences(db);
        string expectedDbModel = MiscHelper.ReadFromFile($@"{ExpectedFilesDir}/Expected_DbModel_V1.json");
        actualDbModel.Should().Be(expectedDbModel);
    }
}
