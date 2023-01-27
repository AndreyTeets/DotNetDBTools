using System.Data;
using System.Text.RegularExpressions;
using DotNetDBTools.Deploy;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.PostgreSQL;
using DotNetDBTools.IntegrationTests.Base;
using DotNetDBTools.Models.Core;
using FluentAssertions.Equivalency;
using Npgsql;
using static DotNetDBTools.IntegrationTests.Constants;

namespace DotNetDBTools.IntegrationTests.PostgreSQL;

public class PostgreSQLDeployTests : BaseDeployTests<
    NpgsqlConnection,
    PostgreSQLDeployManager>
{
    protected override string SpecificDbmsSampleDbV1AssemblyPath => $"{SamplesOutputDir}/DotNetDBTools.SampleDB.PostgreSQL.dll";
    protected override string SpecificDbmsSampleDbV2AssemblyPath => $"{SamplesOutputDir}/DotNetDBTools.SampleDBv2.PostgreSQL.dll";
    private protected override BaseDataTester DataTester { get; set; } = new PostgreSQLDataTester();

    private static string ConnectionStringWithoutDb => PostgreSQLContainerHelper.PostgreSQLContainerConnectionString;

    public PostgreSQLDeployTests() : base(DatabaseKind.PostgreSQL) { }

    protected override EquivalencyAssertionOptions<Database> AddAdditionalDbModelEquivalenceyOptions(
        EquivalencyAssertionOptions<Database> options, CompareMode compareMode)
    {
        return options;
    }

    protected override string GetNormalizedCodeFromCodePiece(CodePiece codePiece)
    {
        string res = codePiece.Code.ToUpper();
        res = Regex.Replace(res, @"(\s)INT([,)\s])", m => $"{m.Groups[1].Value}INTEGER{m.Groups[2].Value}");
        res = Regex.Replace(res, @"(?:IN\s)?(\w+\s)INTEGER([,)\s])", m => $"{m.Groups[1].Value}INTEGER{m.Groups[2].Value}");

        res = res
            .Replace("\r", "")
            .Replace("\n", "")
            .Replace(" ", "")
            .Replace(";", "")
            .Replace("'", "")
            .Replace("::INTEGER", "")
            .Replace("::NUMERIC", "")
            .Replace("::TEXT", "")
            .Replace("::CHARACTERVARYING", "")
            .Replace("::BPCHAR", "")
            .Replace("::BYTEA", "")
            .Replace("::UUID", "")
            .Replace("::DATE", "")
            .Replace("::TIMEWITHOUTTIMEZONE", "")
            .Replace("::TIMESTAMPWITHOUTTIMEZONE", "")
            .Replace("::TIMESTAMPWITHTIMEZONE", "")
            .Replace("::\"MYCOMPOSITETYPE1\"", "")
            .Replace("::\"MYPROCEDURE1\".", "")
            .Replace(".00", "")
            .Replace("$FUNCBODY$", "$FUNCTION$")
            .Replace("$PROCBODY$", "$PROCEDURE$")
            .Replace("CREATEORREPLACE", "CREATE")
            .Replace("EXECUTEPROCEDURE", "EXECUTEFUNCTION")
            .Replace("PUBLIC.", "");

        return res;
    }

    protected override void CreateDatabase(string testName)
    {
        string databaseName = testName;
        PostgreSQLDatabaseHelper.CreateDatabase(ConnectionStringWithoutDb, databaseName);
    }

    protected override void DropDatabaseIfExists(string testName)
    {
        string databaseName = testName;
        PostgreSQLDatabaseHelper.DropDatabaseIfExists(ConnectionStringWithoutDb, databaseName);
    }

    protected override string CreateConnectionString(string testName)
    {
        string databaseName = testName;
        return PostgreSQLDatabaseHelper.CreateConnectionString(ConnectionStringWithoutDb, databaseName);
    }

    private protected override IDbModelFromDBMSProvider CreateDbModelFromDBMSProvider(IDbConnection connection)
    {
        return new PostgreSQLDbModelFromDBMSProvider(new PostgreSQLQueryExecutor(connection, new Events()));
    }
}
