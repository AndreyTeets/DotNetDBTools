using System.Data.Common;
using System.Text.RegularExpressions;
using DotNetDBTools.Analysis.MySQL;
using DotNetDBTools.Deploy;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.MySQL;
using DotNetDBTools.IntegrationTests.Base;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.MySQL;
using FluentAssertions.Equivalency;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySqlConnector;
using static DotNetDBTools.IntegrationTests.Constants;

namespace DotNetDBTools.IntegrationTests.MySQL;

[TestClass]
public class MySQLDeployTests : BaseDeployTests<
    MySQLDatabase,
    MySqlConnection,
    MySQLDbModelConverter,
    MySQLDeployManager>
{
    protected override string AgnosticSampleDbAssemblyPath => $"{SamplesOutputDir}/DotNetDBTools.SampleDB.Agnostic.dll";
    protected override string SpecificDBMSSampleDbAssemblyPath => $"{SamplesOutputDir}/DotNetDBTools.SampleDB.MySQL.dll";

    private static string ConnectionStringWithoutDb => MySQLContainerHelper.MySQLContainerConnectionString;

    protected override EquivalencyAssertionOptions<MySQLDatabase> AddAdditionalDbModelEquivalenceyOptions(
        EquivalencyAssertionOptions<MySQLDatabase> options)
    {
        return options.Excluding(database => database.Functions);
    }

    protected override string GetNormalizedCodeFromCodePiece(CodePiece codePiece)
    {
        string res = codePiece.Code.ToUpper()
            .Replace("\r", "")
            .Replace("\n", "")
            .Replace(" ", "")
            .Replace(";", "")
            .Replace("`", "")
            .Replace("(", "")
            .Replace(")", "")
            .Replace($"{MangleDbNameIfTooLong(TestContext.TestName).ToUpper()}.", "");
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

    private protected override IDbModelFromDbSysInfoBuilder CreateDbModelFromDbSysInfoBuilder(DbConnection connection)
    {
        return new MySQLDbModelFromDbSysInfoBuilder(new MySQLQueryExecutor(connection));
    }

    private static string MangleDbNameIfTooLong(string databaseName)
    {
        if (databaseName.Length > 64)
            return databaseName.Substring(0, 63);
        else
            return databaseName;
    }
}
