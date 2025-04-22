using System;
using System.Data;
using DotNetDBTools.Deploy;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.MSSQL;
using DotNetDBTools.IntegrationTests.Base;
using DotNetDBTools.IntegrationTests.Utilities;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.MSSQL;
using FluentAssertions.Equivalency;
using Microsoft.Data.SqlClient;
using static DotNetDBTools.IntegrationTests.Constants;

namespace DotNetDBTools.IntegrationTests.MSSQL;

public class MSSQLDeployTests : BaseDeployTests<
    SqlConnection,
    MSSQLDeployManager>
{
    protected override string SpecificDbmsSampleDbV1AssemblyPath => $"{SamplesOutputDir}/DotNetDBTools.SampleDB.MSSQL.dll";
    protected override string SpecificDbmsSampleDbV2AssemblyPath => $"{SamplesOutputDir}/DotNetDBTools.SampleDBv2.MSSQL.dll";
    private protected override BaseDataTester DataTester { get; set; } = new MSSQLDataTester();

    private static string ConnectionStringWithoutDb => MSSQLContainerHelper.MSSQLContainerConnectionString;

    public MSSQLDeployTests() : base(DatabaseKind.MSSQL) { }

    protected override EquivalencyAssertionOptions<Database> AddAdditionalDbModelEquivalenceyOptions(
        EquivalencyAssertionOptions<Database> options, CompareMode compareMode)
    {
        if (compareMode.HasFlag(CompareMode.NormalizeCodePieces))
            options = options.Excluding(database => ((MSSQLDatabase)database).Functions);
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

    private protected override IDbModelFromDBMSProvider CreateDbModelFromDBMSProvider(IDbConnection connection)
    {
        return new MSSQLDbModelFromDBMSProvider(new MSSQLQueryExecutor(connection, new Events()));
    }
}
