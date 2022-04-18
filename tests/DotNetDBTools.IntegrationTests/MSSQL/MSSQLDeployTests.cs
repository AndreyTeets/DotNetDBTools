using System;
using System.Data.Common;
using System.Data.SqlClient;
using DotNetDBTools.Analysis.MSSQL;
using DotNetDBTools.Deploy;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.MSSQL;
using DotNetDBTools.IntegrationTests.Base;
using DotNetDBTools.IntegrationTests.Utilities;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.MSSQL;
using FluentAssertions.Equivalency;
using static DotNetDBTools.IntegrationTests.Constants;

namespace DotNetDBTools.IntegrationTests.MSSQL;

public class MSSQLDeployTests : BaseDeployTests<
    MSSQLDatabase,
    SqlConnection,
    MSSQLDbModelConverter,
    MSSQLDeployManager>
{
    protected override string SpecificDbmsSampleDbV1AssemblyPath => $"{SamplesOutputDir}/DotNetDBTools.SampleDB.MSSQL.dll";
    protected override string SpecificDbmsSampleDbV2AssemblyPath => $"{SamplesOutputDir}/DotNetDBTools.SampleDBv2.MSSQL.dll";
    protected override string GeneratedFilesDir => "./DeployTests_Generated/MSSQL";
    protected override BaseDataTester DataTester { get; set; } = new MSSQLDataTester();

    private static string ConnectionStringWithoutDb => MSSQLContainerHelper.MSSQLContainerConnectionString;

    protected override EquivalencyAssertionOptions<MSSQLDatabase> AddAdditionalDbModelEquivalenceyOptions(
        EquivalencyAssertionOptions<MSSQLDatabase> options, CompareMode compareMode)
    {
        if (compareMode.HasFlag(CompareMode.NormalizeCodePieces))
            options = options.Excluding(database => database.Functions);
        return options;
    }

    protected override string GetNormalizedCodeFromCodePiece(CodePiece codePiece)
    {
        return codePiece.Code.ToUpper()
            .Replace("\r", "")
            .Replace("\n", "")
            .Replace(" ", "")
            .Replace("[", "")
            .Replace("]", "")
            .Replace("(", "")
            .Replace(")", "");
    }

    protected override void CreateDatabase(string testName)
    {
        string databaseName = testName;
        using IDisposable _ = ExclusiveExecutionScope.CreateScope(nameof(MSSQLContainerHelper));
        MSSQLDatabaseHelper.CreateDatabase(ConnectionStringWithoutDb, databaseName);
    }

    protected override void DropDatabaseIfExists(string testName)
    {
        string databaseName = testName;
        using IDisposable _ = ExclusiveExecutionScope.CreateScope(nameof(MSSQLContainerHelper));
        MSSQLDatabaseHelper.DropDatabaseIfExists(ConnectionStringWithoutDb, databaseName);
    }

    protected override string CreateConnectionString(string testName)
    {
        string databaseName = testName;
        return MSSQLDatabaseHelper.CreateConnectionString(ConnectionStringWithoutDb, databaseName);
    }

    private protected override IDbModelFromDBMSProvider CreateDbModelFromDBMSProvider(DbConnection connection)
    {
        return new MSSQLDbModelFromDBMSProvider(new MSSQLQueryExecutor(connection, new Events()));
    }
}
