using DotNetDBTools.Deploy;
using DotNetDBTools.UnitTests.Deploy.Base;

namespace DotNetDBTools.UnitTests.Deploy.PostgreSQL;

public class PostgreSQLPublishScriptGenerationTests : BasePublishScriptGenerationTests<PostgreSQLDeployManager>
{
    protected override string SampleDbV1AssemblyName => "DotNetDBTools.SampleDB.PostgreSQL";
    protected override string SampleDbV2AssemblyName => "DotNetDBTools.SampleDBv2.PostgreSQL";
    protected override string ActualFilesDir => "./generated/PostgreSQL";
    protected override string ExpectedFilesDir => "./TestData/PostgreSQL";
}
