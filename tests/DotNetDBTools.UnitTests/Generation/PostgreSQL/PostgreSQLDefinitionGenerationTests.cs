using DotNetDBTools.Models.PostgreSQL;
using DotNetDBTools.UnitTests.Generation.Base;

namespace DotNetDBTools.UnitTests.Generation.PostgreSQL;

public class PostgreSQLDefinitionGenerationTests : BaseDefinitionGenerationTests<PostgreSQLDatabase>
{
    protected override string SampleDbV1CSharpAssemblyName => "DotNetDBTools.SampleDB.PostgreSQL";
    protected override string GeneratedFilesDir => "./generated/PostgreSQL";
}
