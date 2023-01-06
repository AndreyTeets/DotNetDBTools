using System;
using System.Data;
using System.Reflection;
using System.Text;
using DotNetDBTools.Deploy;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Editors;
using DotNetDBTools.Models.Core;
using DotNetDBTools.UnitTests.Utilities;
using FluentAssertions;
using Moq;
using Xunit;

namespace DotNetDBTools.UnitTests.Deploy.Base;

public abstract class BaseDeployManagerTests<TDatabase>
    where TDatabase : Database, new()
{
    private static string AgnosticSampleDbV1AssemblyName => "DotNetDBTools.SampleDB.Agnostic";

    private readonly IMockCreator _mockCreator;
    private readonly Assembly _agnosticDbAssemblyV1;

    private protected BaseDeployManagerTests(IMockCreator mockCreator)
    {
        _mockCreator = mockCreator;
        _agnosticDbAssemblyV1 = MiscHelper.GetSampleDbAssembly(AgnosticSampleDbV1AssemblyName);
    }

    [Fact]
    public void Methods_FireEvents_InCorrectOrder()
    {
        string expectedIsRegisteredEventsOrder =
@"IsRegisteredBegan
IsRegisteredFinished";

        string expectedRegisterEventsOrder =
@"IsRegisteredBegan
IsRegisteredFinished
CreateDbModelFromDBMSBegan
CreateDbModelFromDBMSFinished
RegisterBegan
RegisterFinished";

        string expectedUnregisterEventsOrder =
@"UnregisterBegan
UnregisterFinished";

        string expectedPublishEventsOrder =
@"CreateDbModelFromDefinitionBegan
CreateDbModelFromDefinitionFinished
IsRegisteredBegan
IsRegisteredFinished
CreateDbModelFromDBMSBegan
CreateDbModelFromDBMSFinished
PublishBegan
CreateDbDiffBegan
CreateDbDiffFinished
ApplyDbDiffBegan
ApplyDbDiffFinished
PublishFinished";

        string expectedGenScriptEventsOrder =
@"CreateDbModelFromDefinitionBegan
CreateDbModelFromDefinitionFinished
IsRegisteredBegan
IsRegisteredFinished
CreateDbModelFromDBMSBegan
CreateDbModelFromDBMSFinished
GeneratePublishScriptBegan
CreateDbDiffBegan
CreateDbDiffFinished
ApplyDbDiffBegan
ApplyDbDiffFinished
GeneratePublishScriptFinished";

        TestCase((dm, con) => dm.IsRegisteredAsDNDBT(con), expectedIsRegisteredEventsOrder);
        TestCase((dm, con) => dm.RegisterAsDNDBT(con), expectedRegisterEventsOrder, unregistered: true);
        TestCase((dm, con) => dm.UnregisterAsDNDBT(con), expectedUnregisterEventsOrder);
        TestCase((dm, con) => dm.PublishDatabase(_agnosticDbAssemblyV1, con), expectedPublishEventsOrder);
        TestCase((dm, con) => dm.GeneratePublishScript(_agnosticDbAssemblyV1, con), expectedGenScriptEventsOrder);

        void TestCase(
            Action<IDeployManager, IDbConnection> deployManagerAction,
            string expectedEventsOrder,
            bool unregistered = false)
        {
            Mock<DeployManager<TDatabase>> deployManagerMock = new(
                MockBehavior.Strict, new DeployOptions(), _mockCreator.CreateMockedFactory(unregistered));
            deployManagerMock.CallBase = true;

            StringBuilder sb = new();
            deployManagerMock.Object.Events.EventFired += eventArgs => sb.AppendLine(eventArgs.EventType.ToString());

            Mock<IDbConnection> dbConnectionMock = new(MockBehavior.Strict);
            dbConnectionMock.Setup(x => x.Dispose());
            using IDisposable _ = dbConnectionMock.Object;

            deployManagerAction(deployManagerMock.Object, dbConnectionMock.Object);
            sb.ToString().Trim().Should().Be(expectedEventsOrder);
        }
    }

    internal interface IMockCreator
    {
        public IFactory CreateMockedFactory(bool unregistered);
    }

    internal abstract class MockCreator<
        TFactory,
        TQueryExecutor,
        TGenSqlScriptQueryExecutor,
        TDbEditor,
        TDbModelFromDBMSProvider>
        : IMockCreator
        where TFactory : Factory<
            TQueryExecutor,
            TGenSqlScriptQueryExecutor,
            TDbEditor,
            TDbModelFromDBMSProvider>
        where TQueryExecutor : IQueryExecutor
        where TGenSqlScriptQueryExecutor : IGenSqlScriptQueryExecutor, new()
        where TDbEditor : IDbEditor
        where TDbModelFromDBMSProvider : IDbModelFromDBMSProvider
    {
        public IFactory CreateMockedFactory(bool unregistered)
        {
            Mock<IQueryExecutor> queryExecutorMock = new(MockBehavior.Strict);
            queryExecutorMock.Setup(x => x.BeginTransaction());
            queryExecutorMock.Setup(x => x.CommitTransaction());
            queryExecutorMock
                .Setup(x => x.QuerySingleOrDefault<It.IsAnyType>(It.IsAny<IQuery>()))
                .Returns((IQuery query) => Get_QuerySingleOrDefault_MockedResult(query, unregistered));
            queryExecutorMock
                .Setup(x => x.Execute(It.IsAny<IQuery>()))
                .Returns(0);

            Mock<IDbModelFromDBMSProvider> dbModelFromDBMSProviderMock = new(MockBehavior.Strict);
            TDatabase emptyDb = new();
            dbModelFromDBMSProviderMock
                .Setup(x => x.CreateDbModelUsingDNDBTSysInfo())
                .Returns(emptyDb);
            dbModelFromDBMSProviderMock
                .Setup(x => x.CreateDbModelUsingDBMSSysInfo())
                .Returns(emptyDb);

            Mock<TFactory> factoryMock = new(MockBehavior.Loose);
            factoryMock.CallBase = true;
            factoryMock
                .Setup(x => x.CreateQueryExecutor(It.IsAny<IDbConnection>(), It.IsAny<Events>()))
                .Returns(queryExecutorMock.Object);
            factoryMock
                .Setup(x => x.CreateDbModelFromDBMSProvider(It.IsAny<IQueryExecutor>()))
                .Returns(dbModelFromDBMSProviderMock.Object);

            return factoryMock.Object;
        }

        private object Get_QuerySingleOrDefault_MockedResult(IQuery query, bool unregistered)
        {
            string queryType = query.GetType().Name;
            if (queryType.EndsWith("CheckDNDBTSysTablesExistQuery", StringComparison.Ordinal))
                return !unregistered;
            else
                throw new Exception($"Invalid query type {query.GetType()}");
        }
    }
}
