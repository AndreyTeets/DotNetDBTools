using System.Data;
using System.IO;
using Dapper;
using DotNetDBTools.Analysis;
using DotNetDBTools.DefinitionParsing;
using DotNetDBTools.Deploy;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Models.Core;
using FluentAssertions;
using FluentAssertions.Equivalency;
using Newtonsoft.Json;
using NUnit.Framework;
using static DotNetDBTools.IntegrationTests.Constants;

namespace DotNetDBTools.IntegrationTests.Base;

[TestFixture]
public abstract class BaseDeployTests<TDbConnection, TDeployManager>
    where TDbConnection : IDbConnection, new()
    where TDeployManager : IDeployManager, new()
{
    protected abstract string SpecificDbmsSampleDbV1AssemblyPath { get; }
    protected abstract string SpecificDbmsSampleDbV2AssemblyPath { get; }
    private protected abstract BaseDataTester DataTester { get; set; }

    private static string AgnosticSampleDbV1AssemblyPath => $"{SamplesOutputDir}/DotNetDBTools.SampleDB.Agnostic.dll";
    private static string AgnosticSampleDbV2AssemblyPath => $"{SamplesOutputDir}/DotNetDBTools.SampleDBv2.Agnostic.dll";
    private static string CurrentTestName => TestContext.CurrentContext.Test.Name;
    private string GeneratedFilesDir => $"./DeployTests_Generated/{_databaseKind}";

    private readonly DatabaseKind _databaseKind;
    private readonly TDeployManager _deployManager;
    private readonly IDefinitionParsingManager _definitionParsingManager;

    protected BaseDeployTests(DatabaseKind databaseKind)
    {
        _databaseKind = databaseKind;
        _deployManager = new();
        _definitionParsingManager = new DefinitionParsingManager();
        Directory.CreateDirectory(GeneratedFilesDir);
    }

    [Test]
    public void NoDNDBTInfoPublishScript_UpdatesToDesiredState_And_PreservesRelevantData()
    {
        DataTester.IsDbmsSpecific = false;
        TestCase(AgnosticSampleDbV1AssemblyPath, AgnosticSampleDbV2AssemblyPath, "Agnostic");

        DataTester.IsDbmsSpecific = true;
        TestCase(SpecificDbmsSampleDbV1AssemblyPath, SpecificDbmsSampleDbV2AssemblyPath, "SpecificDbms");

        void TestCase(string assemblyV1Path, string assemblyV2Path, string testCaseId)
        {
            string createV1Script = $"{GeneratedFilesDir}/{testCaseId}_CreateV1Script.sql";
            string updateV1toV2Script = $"{GeneratedFilesDir}/{testCaseId}_UpdateV1toV2Script.sql";
            string updateV2toV1Script = $"{GeneratedFilesDir}/{testCaseId}_UpdateV2toV1Script.sql";
            string db2_createV1Script = $"{GeneratedFilesDir}/{testCaseId}_Db2_CreateV1Script.sql";

            string serializedDbModelFromDefinitionV1 = $"{GeneratedFilesDir}/{testCaseId}_DbModelFromDefinitionV1.sql";
            string serializedDbModelFromDefinitionV2 = $"{GeneratedFilesDir}/{testCaseId}_DbModelFromDefinitionV2.sql";
            string serializedDbModelFromDBMSSysInfoV1 = $"{GeneratedFilesDir}/{testCaseId}_DbModelFromDBMSSysInfoV1.sql";
            string serializedDbModelFromDBMSSysInfoV2 = $"{GeneratedFilesDir}/{testCaseId}_DbModelFromDBMSSysInfoV2.sql";

            File.WriteAllText(createV1Script, _deployManager.GenerateNoDNDBTInfoPublishScript(assemblyV1Path));
            _deployManager.Options.AllowDataLoss = true;
            File.WriteAllText(updateV1toV2Script, _deployManager.GenerateNoDNDBTInfoPublishScript(assemblyV2Path, assemblyV1Path));
            File.WriteAllText(updateV2toV1Script, _deployManager.GenerateNoDNDBTInfoPublishScript(assemblyV1Path, assemblyV2Path));

            Database dbModelFromDefinitionV1 = CreateDbModelFromDefinition(assemblyV1Path);
            Database dbModelFromDefinitionV2 = CreateDbModelFromDefinition(assemblyV2Path);
            Database dbModelFromDBMSSSysInfoV1;
            Database dbModelFromDBMSSSysInfoV2;

            DumpDbModel(serializedDbModelFromDefinitionV1, dbModelFromDefinitionV1);
            DumpDbModel(serializedDbModelFromDefinitionV2, dbModelFromDefinitionV2);

            using IDbConnection connection = RecreateDbAndCreateConnection(CreateTestCaseName(testCaseId));

            connection.Execute(File.ReadAllText(createV1Script));
            dbModelFromDBMSSSysInfoV1 = CreateDbModelFromDBMSSysInfo(connection);
            DumpDbModel(serializedDbModelFromDBMSSysInfoV1, dbModelFromDBMSSSysInfoV1);
            AssertDbModelEquivalence(dbModelFromDBMSSSysInfoV1, dbModelFromDefinitionV1, CompareMode.IgnoreAllDNDBT);
            DataTester.Populate_SampleDb_WithData(connection);
            DataTester.Assert_SampleDb_Data(connection, BaseDataTester.AssertKind.V1);

            connection.Execute(File.ReadAllText(updateV1toV2Script));
            dbModelFromDBMSSSysInfoV2 = CreateDbModelFromDBMSSysInfo(connection);
            DumpDbModel(serializedDbModelFromDBMSSysInfoV2, dbModelFromDBMSSSysInfoV2);
            AssertDbModelEquivalence(dbModelFromDBMSSSysInfoV2, dbModelFromDefinitionV2, CompareMode.IgnoreAllDNDBT);
            DataTester.Assert_SampleDb_Data(connection, BaseDataTester.AssertKind.V2);

            connection.Execute(File.ReadAllText(updateV2toV1Script));
            dbModelFromDBMSSSysInfoV1 = CreateDbModelFromDBMSSysInfo(connection);
            AssertDbModelEquivalence(dbModelFromDBMSSSysInfoV1, dbModelFromDefinitionV1, CompareMode.IgnoreAllDNDBT);
            DataTester.Assert_SampleDb_Data(connection, BaseDataTester.AssertKind.V1Rollbacked);

            dbModelFromDBMSSSysInfoV1.Version = 1;
            File.WriteAllText(db2_createV1Script, _deployManager.GenerateNoDNDBTInfoPublishScript(dbModelFromDBMSSSysInfoV1));
            using IDbConnection connection2 = RecreateDbAndCreateConnection($"Db2_{CreateTestCaseName(testCaseId)}");
            connection2.Execute(File.ReadAllText(db2_createV1Script));

            AssertDbModelEquivalence(CreateDbModelFromDBMSSysInfo(connection2), dbModelFromDBMSSSysInfoV1, CompareMode.IgnoreIDsAndDbAttributes);
            DataTester.Populate_SampleDb_WithData(connection2);
            DataTester.Assert_SampleDb_Data(connection2, BaseDataTester.AssertKind.V1NoScripts);
        }
    }

    [Test]
    public void RegisterAndPublish_UpdateToDesiredState_And_PreserveRelevantData()
    {
        DataTester.IsDbmsSpecific = false;
        TestCase(AgnosticSampleDbV1AssemblyPath, AgnosticSampleDbV2AssemblyPath, "Agnostic");

        DataTester.IsDbmsSpecific = true;
        TestCase(SpecificDbmsSampleDbV1AssemblyPath, SpecificDbmsSampleDbV2AssemblyPath, "SpecificDbms");

        void TestCase(string assemblyV1Path, string assemblyV2Path, string testCaseId)
        {
            Database dbModelFromDefinitionV1 = CreateDbModelFromDefinition(assemblyV1Path);
            Database dbModelFromDefinitionV2 = CreateDbModelFromDefinition(assemblyV2Path);

            using IDbConnection connection = RecreateDbAndCreateConnection(CreateTestCaseName(testCaseId));

            _deployManager.RegisterAsDNDBT(connection);
            _deployManager.PublishDatabase(assemblyV1Path, connection);
            AssertDbModelEquivalence(CreateDbModelFromDNDBTSysInfo(connection), dbModelFromDefinitionV1, CompareMode.Everything);
            AssertDbModelEquivalence(CreateDbModelFromDBMSSysInfo(connection), dbModelFromDefinitionV1, CompareMode.IgnoreAllDNDBT);
            DataTester.Populate_SampleDb_WithData(connection);
            DataTester.Assert_SampleDb_Data(connection, BaseDataTester.AssertKind.V1);

            _deployManager.PublishDatabase(assemblyV1Path, connection);
            AssertDbModelEquivalence(CreateDbModelFromDNDBTSysInfo(connection), dbModelFromDefinitionV1, CompareMode.Everything);
            AssertDbModelEquivalence(CreateDbModelFromDBMSSysInfo(connection), dbModelFromDefinitionV1, CompareMode.IgnoreAllDNDBT);
            DataTester.Assert_SampleDb_Data(connection, BaseDataTester.AssertKind.V1);

            _deployManager.UnregisterAsDNDBT(connection);
            _deployManager.RegisterAsDNDBT(connection);
            AssertDbModelEquivalence(CreateDbModelFromDNDBTSysInfo(connection), dbModelFromDefinitionV1, CompareMode.IgnoreAllDNDBT);
            AssertDbModelEquivalence(CreateDbModelFromDBMSSysInfo(connection), dbModelFromDefinitionV1, CompareMode.IgnoreAllDNDBT);
            DataTester.Assert_SampleDb_Data(connection, BaseDataTester.AssertKind.V1);

            _deployManager.UnregisterAsDNDBT(connection);
            _deployManager.RegisterAsDNDBT(connection, dbModelFromDefinitionV1);
            AssertDbModelEquivalence(CreateDbModelFromDNDBTSysInfo(connection), dbModelFromDefinitionV1, CompareMode.Everything);
            AssertDbModelEquivalence(CreateDbModelFromDBMSSysInfo(connection), dbModelFromDefinitionV1, CompareMode.IgnoreAllDNDBT);
            DataTester.Assert_SampleDb_Data(connection, BaseDataTester.AssertKind.V1);

            _deployManager.Options.AllowDataLoss = true;
            _deployManager.PublishDatabase(assemblyV2Path, connection);
            AssertDbModelEquivalence(CreateDbModelFromDNDBTSysInfo(connection), dbModelFromDefinitionV2, CompareMode.Everything);
            AssertDbModelEquivalence(CreateDbModelFromDBMSSysInfo(connection), dbModelFromDefinitionV2, CompareMode.IgnoreAllDNDBT);
            DataTester.Assert_SampleDb_Data(connection, BaseDataTester.AssertKind.V2);

            _deployManager.Options.AllowDataLoss = false;
            _deployManager.PublishDatabase(assemblyV2Path, connection);
            AssertDbModelEquivalence(CreateDbModelFromDNDBTSysInfo(connection), dbModelFromDefinitionV2, CompareMode.Everything);
            AssertDbModelEquivalence(CreateDbModelFromDBMSSysInfo(connection), dbModelFromDefinitionV2, CompareMode.IgnoreAllDNDBT);
            DataTester.Assert_SampleDb_Data(connection, BaseDataTester.AssertKind.V2);

            _deployManager.Options.AllowDataLoss = true;
            _deployManager.PublishDatabase(assemblyV1Path, connection);
            AssertDbModelEquivalence(CreateDbModelFromDNDBTSysInfo(connection), dbModelFromDefinitionV1, CompareMode.Everything);
            AssertDbModelEquivalence(CreateDbModelFromDBMSSysInfo(connection), dbModelFromDefinitionV1, CompareMode.IgnoreAllDNDBT);
            DataTester.Assert_SampleDb_Data(connection, BaseDataTester.AssertKind.V1Rollbacked);

            Database dbModelFromDBMSSysInfoV1 = CreateDbModelFromDBMSSysInfo(connection);
            dbModelFromDBMSSysInfoV1.Version = 1;

            using IDbConnection connection2 = RecreateDbAndCreateConnection($"Db2_{CreateTestCaseName(testCaseId)}");
            _deployManager.RegisterAsDNDBT(connection2);
            _deployManager.PublishDatabase(dbModelFromDBMSSysInfoV1, connection2);

            AssertDbModelEquivalence(CreateDbModelFromDNDBTSysInfo(connection2), dbModelFromDBMSSysInfoV1, CompareMode.Everything);
            AssertDbModelEquivalence(CreateDbModelFromDBMSSysInfo(connection2), dbModelFromDBMSSysInfoV1, CompareMode.IgnoreIDsAndDbAttributes);
            DataTester.Populate_SampleDb_WithData(connection2);
            DataTester.Assert_SampleDb_Data(connection2, BaseDataTester.AssertKind.V1NoScripts);
        }
    }

    private Database CreateDbModelFromDefinition(string sampleDbAssemblyPath)
    {
        Database database = _definitionParsingManager.CreateDbModel(sampleDbAssemblyPath);

        if (database.Kind == DatabaseKind.Agnostic)
            return new AnalysisManager().ConvertFromAgnostic(database, _databaseKind);
        else
            return database;
    }

    private Database CreateDbModelFromDBMSSysInfo(IDbConnection connection)
    {
        return _deployManager.CreateDatabaseModelUsingDBMSSysInfo(connection);
    }

    private Database CreateDbModelFromDNDBTSysInfo(IDbConnection connection)
    {
        return _deployManager.CreateDatabaseModelUsingDNDBTSysInfo(connection);
    }

    private void AssertDbModelEquivalence(Database dbModel1, Database dbModel2, CompareMode compareMode)
    {
        dbModel1.Should().BeEquivalentTo(dbModel2, options =>
        {
            EquivalencyAssertionOptions<Database> configuredOptions = options
                .RespectingRuntimeTypes()
                .WithStrictOrdering()
                .Excluding(mi => mi.Name == nameof(DbObject.Parent) && mi.DeclaringType == typeof(DbObject))
                .Excluding(mi => mi.Name == nameof(CodePiece.DependsOn) && mi.DeclaringType == typeof(CodePiece))
                .Excluding(mi => mi.Name == nameof(DataType.DependsOn) && mi.DeclaringType == typeof(DataType));

            if (compareMode.HasFlag(CompareMode.IgnoreIDs))
                configuredOptions = configuredOptions.Excluding(mi => mi.Name == nameof(DbObject.ID) && mi.DeclaringType == typeof(DbObject));

            if (compareMode.HasFlag(CompareMode.NormalizeCodePieces))
                configuredOptions = configuredOptions.Using<CodePiece>(CompareCodePiece).WhenTypeIs<CodePiece>();

            if (compareMode.HasFlag(CompareMode.IgnoreScripts))
                configuredOptions = configuredOptions.Excluding(database => database.Scripts);

            if (compareMode.HasFlag(CompareMode.IgnoreDbAttributes))
                configuredOptions = configuredOptions.Excluding(database => database.Version);

            configuredOptions = AddAdditionalDbModelEquivalenceyOptions(configuredOptions, compareMode);
            return configuredOptions;
        });
    }

    private IDbConnection RecreateDbAndCreateConnection(string testName)
    {
        DropDatabaseIfExists(testName);
        CreateDatabase(testName);

        TDbConnection connection = new();
        connection.ConnectionString = CreateConnectionString(testName);
        return connection;
    }

    private static void DumpDbModel(string filePath, Database dbModel)
    {
        JsonSerializerSettings jsonSettings = new()
        {
            Formatting = Formatting.Indented,
            PreserveReferencesHandling = PreserveReferencesHandling.Objects,
        };
        string serializedDbModel = JsonConvert.SerializeObject(dbModel, jsonSettings);
        File.WriteAllText(filePath, serializedDbModel);
    }

    private string CreateTestCaseName(string testCaseId)
    {
        return testCaseId + "_" + CurrentTestName;
    }

    protected abstract EquivalencyAssertionOptions<Database> AddAdditionalDbModelEquivalenceyOptions(
        EquivalencyAssertionOptions<Database> options, CompareMode compareMode);
    protected abstract string GetNormalizedCodeFromCodePiece(CodePiece codePiece);
    protected abstract void CreateDatabase(string testName);
    protected abstract void DropDatabaseIfExists(string testName);
    protected abstract string CreateConnectionString(string testName);
    private protected abstract IDbModelFromDBMSProvider CreateDbModelFromDBMSProvider(IDbConnection connection);

    protected enum CompareMode
    {
        Everything = 0,
        IgnoreIDs = 1,
        NormalizeCodePieces = 2,
        IgnoreScripts = 4,
        IgnoreDbAttributes = 8,
        IgnoreIDsAndDbAttributes = 1 | 8,
        IgnoreAllDNDBT = 1 | 2 | 4 | 8,
    }

    private void CompareCodePiece(IAssertionContext<CodePiece> ctx)
    {
        if (ctx.Subject is null || ctx.Expectation is null)
        {
            ctx.Subject.Should().Be(ctx.Expectation);
            return;
        }

        string subjectNormalizedCode = GetNormalizedCodeFromCodePiece(ctx.Subject);
        string expectationNormalizedCode = GetNormalizedCodeFromCodePiece(ctx.Expectation);
        subjectNormalizedCode.Should().Be(expectationNormalizedCode);
    }
}
