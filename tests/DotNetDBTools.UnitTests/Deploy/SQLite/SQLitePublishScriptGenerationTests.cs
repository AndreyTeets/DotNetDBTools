using DotNetDBTools.Deploy;
using DotNetDBTools.UnitTests.Deploy.Base;

namespace DotNetDBTools.UnitTests.Deploy.SQLite;

public class SQLitePublishScriptGenerationTests : BasePublishScriptGenerationTests<SQLiteDeployManager>
{
    protected override string SpecificDbmsSampleDbV1AssemblyName => "DotNetDBTools.SampleDB.SQLite";
    protected override string SpecificDbmsSampleDbV2AssemblyName => "DotNetDBTools.SampleDBv2.SQLite";
    protected override string ExpectedFilesDir => "./TestData/SQLite";
}
