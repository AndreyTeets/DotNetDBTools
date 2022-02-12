using DotNetDBTools.Deploy;
using DotNetDBTools.UnitTests.Deploy.Base;

namespace DotNetDBTools.UnitTests.Deploy.MySQL;

public class MySQLPublishScriptGenerationTests : BasePublishScriptGenerationTests<MySQLDeployManager>
{
    protected override string SampleDbV1AssemblyName => "DotNetDBTools.SampleDB.MySQL";
    protected override string SampleDbV2AssemblyName => "DotNetDBTools.SampleDBv2.MySQL";
    protected override string ActualFilesDir => "./generated/MySQL";
    protected override string ExpectedFilesDir => "./TestData/MySQL";
}
