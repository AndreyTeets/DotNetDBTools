using DotNetDBTools.Models.MySQL;
using DotNetDBTools.UnitTests.Generation.Base;

namespace DotNetDBTools.UnitTests.Generation.MySQL;

public class MySQLDefinitionGenerationTests : BaseDefinitionGenerationTests<MySQLDatabase>
{
    protected override string SampleDbV1CSharpAssemblyName => "DotNetDBTools.SampleDB.MySQL";
    protected override string GeneratedFilesDir => "./generated/MySQL";
}
