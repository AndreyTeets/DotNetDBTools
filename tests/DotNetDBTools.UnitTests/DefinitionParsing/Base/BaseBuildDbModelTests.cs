using System.Reflection;
using DotNetDBTools.Analysis.Extensions;
using DotNetDBTools.DefinitionParsing;
using DotNetDBTools.Models.Core;
using DotNetDBTools.UnitTests.Utilities;
using FluentAssertions;
using Newtonsoft.Json;
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
        _specificDbmsDbAssemblyV1 = TestDbAssembliesHelper.GetSampleDbAssembly(SpecificDbmsSampleDbV1AssemblyName);
    }

    [Fact]
    public void DbModelFromCSharpDef_IsCorrect()
    {
        TDatabase db = (TDatabase)new DefinitionParsingManager().CreateDbModel(_specificDbmsDbAssemblyV1);
        string actualDbModel = SerializeDbModel(db);
        string expectedDbModel = FilesHelper.GetFromFile($@"{ExpectedFilesDir}/Expected_DbModel_V1.json");
        actualDbModel.Should().Be(expectedDbModel);
    }

    private static string SerializeDbModel(TDatabase dbModel)
    {
        JsonSerializerSettings jsonSettings = new()
        {
            Formatting = Formatting.Indented,
            PreserveReferencesHandling = PreserveReferencesHandling.Objects,
        };
        return JsonConvert.SerializeObject(dbModel, jsonSettings).NormalizeLineEndings();
    }
}
