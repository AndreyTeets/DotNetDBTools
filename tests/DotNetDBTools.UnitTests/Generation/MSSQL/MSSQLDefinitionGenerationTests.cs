using DotNetDBTools.Models.MSSQL;
using DotNetDBTools.UnitTests.Generation.Base;

namespace DotNetDBTools.UnitTests.Generation.MSSQL;

public class MSSQLDefinitionGenerationTests : BaseDefinitionGenerationTests<MSSQLDatabase>
{
    protected override string SampleDbV1CSharpAssemblyName => "DotNetDBTools.SampleDB.MSSQL";
    protected override string GeneratedFilesDir => "./generated/MSSQL";
}
