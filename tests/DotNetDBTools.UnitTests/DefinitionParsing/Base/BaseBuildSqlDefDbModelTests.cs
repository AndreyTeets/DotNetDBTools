using System.Collections.Generic;
using System.Reflection;
using DotNetDBTools.DefinitionParsing;
using DotNetDBTools.Models.Core;
using DotNetDBTools.UnitTests.Utilities;
using FluentAssertions;
using Xunit;

namespace DotNetDBTools.UnitTests.DefinitionParsing.Base;

public abstract class BaseBuildSqlDefDbModelTests
{
    protected abstract string SpecificDbmsSampleDbV2AssemblyName { get; }
    protected abstract string SpecificDbmsSampleDbV2SqlDefAssemblyName { get; }
    protected abstract List<string> ListOfSqlStatementsForDbModelCreation { get; }
    protected abstract DatabaseKind DatabaseKindForDbModelCreation { get; }
    protected abstract Database ExpectedDbModelFromListOfSqlStatements { get; }

    private readonly Assembly _specificDbmsDbAssemblyV2;
    private readonly Assembly _specificDbmsDbAssemblyV2SqlDef;

    protected BaseBuildSqlDefDbModelTests()
    {
        _specificDbmsDbAssemblyV2 = MiscHelper.GetSampleDbAssembly(SpecificDbmsSampleDbV2AssemblyName);
        _specificDbmsDbAssemblyV2SqlDef = MiscHelper.GetSampleDbAssembly(SpecificDbmsSampleDbV2SqlDefAssemblyName);
    }

    [Fact]
    public void DbModelFromSqlDef_IsEquivalentTo_DbModelFromCSharpDef()
    {
        Database databaseCSharp = new DefinitionParsingManager().CreateDbModel(_specificDbmsDbAssemblyV2);
        Database databaseSql = new DefinitionParsingManager().CreateDbModel(_specificDbmsDbAssemblyV2SqlDef);
        databaseSql.Should().BeEquivalentTo(databaseCSharp, options => options
            .RespectingRuntimeTypes()
            .WithStrictOrdering()
            .Excluding(mi => mi.Name == nameof(DbObject.Parent) && mi.DeclaringType == typeof(DbObject))
            .Excluding(mi => mi.Name == nameof(CodePiece.DependsOn) && mi.DeclaringType == typeof(CodePiece))
            .Excluding(mi => mi.Name == nameof(DataType.DependsOn) && mi.DeclaringType == typeof(DataType)));
    }

    [Fact]
    public void CreateDbModel_FromListOfSqlStatements_CreatesCorrectModel()
    {
        Database actualDbModel = new DefinitionParsingManager().CreateDbModel(
            ListOfSqlStatementsForDbModelCreation, 3, DatabaseKindForDbModelCreation);

        actualDbModel.Should().BeEquivalentTo(ExpectedDbModelFromListOfSqlStatements, options => options
            .RespectingRuntimeTypes()
            .WithoutStrictOrdering()
            .Excluding(mi => mi.Name == nameof(DbObject.Parent) && mi.DeclaringType == typeof(DbObject))
            .Excluding(mi => mi.Name == nameof(CodePiece.DependsOn) && mi.DeclaringType == typeof(CodePiece))
            .Excluding(mi => mi.Name == nameof(DataType.DependsOn) && mi.DeclaringType == typeof(DataType)));
    }
}
