using DotNetDBTools.Deploy;
using DotNetDBTools.UnitTests.Deploy.Base;

namespace DotNetDBTools.UnitTests.Deploy.MSSQL;

public class MSSQLPublishScriptGenerationTests : BasePublishScriptGenerationTests<MSSQLDeployManager>
{
    protected override string SpecificDbmsSampleDbV1AssemblyName => "DotNetDBTools.SampleDB.MSSQL";
    protected override string SpecificDbmsSampleDbV2AssemblyName => "DotNetDBTools.SampleDBv2.MSSQL";
    protected override string ExpectedFilesDir => "./TestData/MSSQL/SampleDbRelated";
}
