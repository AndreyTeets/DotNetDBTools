using DotNetDBTools.Deploy;
using DotNetDBTools.UnitTests.Deploy.Base;

namespace DotNetDBTools.UnitTests.Deploy.MySQL;

public class MySQLPublishScriptGenerationTests : BasePublishScriptGenerationTests<MySQLDeployManager>
{
    protected override string SpecificDbmsSampleDbV1AssemblyName => "DotNetDBTools.SampleDB.MySQL";
    protected override string SpecificDbmsSampleDbV2AssemblyName => "DotNetDBTools.SampleDBv2.MySQL";
    protected override string ExpectedFilesDir => "./TestData/MySQL";
}
