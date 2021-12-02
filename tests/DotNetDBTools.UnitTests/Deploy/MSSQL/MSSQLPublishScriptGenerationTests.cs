using DotNetDBTools.Deploy;
using DotNetDBTools.UnitTests.Deploy.Base;

namespace DotNetDBTools.UnitTests.Deploy.MSSQL
{
    public class MSSQLPublishScriptGenerationTests : BasePublishScriptGenerationTests<MSSQLDeployManager>
    {
        protected override string SampleDbV1AssemblyName => "DotNetDBTools.SampleDB.MSSQL";
        protected override string SampleDbV2AssemblyName => "DotNetDBTools.SampleDBv2.MSSQL";
        protected override string ActualFilesDir => "./generated/MSSQL";
        protected override string ExpectedFilesDir => "./TestData/MSSQL";
    }
}
