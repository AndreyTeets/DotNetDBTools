﻿using DotNetDBTools.Deploy;
using DotNetDBTools.UnitTests.Deploy.Base;

namespace DotNetDBTools.UnitTests.Deploy.SQLite
{
    public class SQLitePublishScriptGenerationTests : BasePublishScriptGenerationTests<SQLiteDeployManager>
    {
        protected override string SampleDbV1AssemblyName => "DotNetDBTools.SampleDB.SQLite";
        protected override string SampleDbV2AssemblyName => "DotNetDBTools.SampleDBv2.SQLite";
        protected override string ActualFilesDir => "./generated/SQLite";
        protected override string ExpectedFilesDir => "./TestData/SQLite";
    }
}