using System.Data;
using System.Text.RegularExpressions;
using DotNetDBTools.Deploy;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.MySQL;
using DotNetDBTools.IntegrationTests.Base;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.MySQL;
using FluentAssertions.Equivalency;
using MySqlConnector;
using NUnit.Framework;
using static DotNetDBTools.IntegrationTests.Constants;

namespace DotNetDBTools.IntegrationTests.MySQL;

public class MySQLDeployTests : BaseDeployTests<
    MySqlConnection,
    MySQLDeployManager>
{
    protected override string SpecificDbmsSampleDbV1AssemblyPath => $"{SamplesOutputDir}/DotNetDBTools.SampleDB.MySQL.dll";
    protected override string SpecificDbmsSampleDbV2AssemblyPath => $"{SamplesOutputDir}/DotNetDBTools.SampleDBv2.MySQL.dll";
    private protected override BaseDataTester DataTester { get; set; } = new MySQLDataTester();

    private static string ConnectionStringWithoutDb => MySQLContainerHelper.MySQLContainerConnectionString;

    public MySQLDeployTests() : base(DatabaseKind.MySQL) { }

    protected override EquivalencyAssertionOptions<Database> AddAdditionalDbModelEquivalenceyOptions(
        EquivalencyAssertionOptions<Database> options, CompareMode compareMode)
    {
        if (compareMode.HasFlag(CompareMode.NormalizeCodePieces))
            options = options.Excluding(database => ((MySQLDatabase)database).Functions);
        return options;
    }

    protected override string GetNormalizedCodeFromCodePiece(CodePiece codePiece)
    {
        string testName = TestContext.CurrentContext.Test.Name;
        string res = codePiece.Code.ToUpper()
            .Replace("\r", "")
            .Replace("\n", "")
            .Replace(" ", "")
            .Replace(";", "")
            .Replace("`", "")
            .Replace("(", "")
            .Replace(")", "")
            .Replace(MangleDbNameIfTooLong($"AGNOSTIC_{testName}").ToUpper() + ".", "")
            .Replace(MangleDbNameIfTooLong($"SPECIFICDBMS_{testName}").ToUpper() + ".", "")
            .Replace(MangleDbNameIfTooLong($"AGNOSTICV1_{testName}").ToUpper() + ".", "")
            .Replace(MangleDbNameIfTooLong($"AGNOSTICV2_{testName}").ToUpper() + ".", "")
            .Replace(MangleDbNameIfTooLong($"SPECIFICDBMSV1_{testName}").ToUpper() + ".", "")
            .Replace(MangleDbNameIfTooLong($"SPECIFICDBMSV2_{testName}").ToUpper() + ".", "")
            ;

        string identifier = @"[\w|\d|_]+";
        res = Regex.Replace(res, $"AS{identifier},", ",");
        res = Regex.Replace(res, $"AS{identifier}FROM", "FROM");
        return res;
    }

    protected override void CreateDatabase(string testName)
    {
        string databaseName = MangleDbNameIfTooLong(testName);
        MySQLDatabaseHelper.CreateDatabase(ConnectionStringWithoutDb, databaseName);
    }

    protected override void DropDatabaseIfExists(string testName)
    {
        string databaseName = MangleDbNameIfTooLong(testName);
        MySQLDatabaseHelper.DropDatabaseIfExists(ConnectionStringWithoutDb, databaseName);
    }

    protected override string CreateConnectionString(string testName)
    {
        string databaseName = MangleDbNameIfTooLong(testName);
        return MySQLDatabaseHelper.CreateConnectionString(ConnectionStringWithoutDb, databaseName);
    }

    private protected override IDbModelFromDBMSProvider CreateDbModelFromDBMSProvider(IDbConnection connection)
    {
        return new MySQLDbModelFromDBMSProvider(new MySQLQueryExecutor(connection, new Events()));
    }

    private static string MangleDbNameIfTooLong(string databaseName)
    {
        if (databaseName.Length > 64)
            return databaseName.Substring(0, 63);
        else
            return databaseName;
    }
}
