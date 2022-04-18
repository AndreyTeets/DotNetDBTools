using DotNetDBTools.Deploy;
using DotNetDBTools.UnitTests.Deploy.Base;

namespace DotNetDBTools.UnitTests.Deploy.PostgreSQL;

public class PostgreSQLPublishScriptGenerationTests : BasePublishScriptGenerationTests<PostgreSQLDeployManager>
{
    protected override string SpecificDbmsSampleDbV1AssemblyName => "DotNetDBTools.SampleDB.PostgreSQL";
    protected override string SpecificDbmsSampleDbV2AssemblyName => "DotNetDBTools.SampleDBv2.PostgreSQL";
    protected override string ExpectedFilesDir => "./TestData/PostgreSQL";
}
