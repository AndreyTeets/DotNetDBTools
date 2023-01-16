using System;
using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.Models;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.PostgreSQL;
using DotNetDBTools.Models.PostgreSQL.UserDefinedTypes;
using DotNetDBTools.UnitTests.Utilities;
using FluentAssertions;
using Xunit;

namespace DotNetDBTools.UnitTests.Models;

public class ExtensionMethodsTests
{
    [Fact]
    public void CopyModel_CreatesClone_EquivalentToOriginal()
    {
        PostgreSQLDatabase originalObject = CreateTestDatabaseModel();
        PostgreSQLDatabase cloneObject = originalObject.CopyModel();

        PostgreSQLDatabase expectedObject = CreateTestDatabaseModel();
        AssertEquivalence(cloneObject, expectedObject);
    }

    [Fact]
    public void CopyModel_ModifyingClone_DoesNotAffectOriginal()
    {
        PostgreSQLDatabase originalObject = CreateTestDatabaseModel();
        PostgreSQLDatabase unmodifiedObject = CreateTestDatabaseModel();
        PostgreSQLDatabase cloneObject;

        cloneObject = originalObject.CopyModel();
        cloneObject.Version = 3;
        cloneObject.Tables = null;
        cloneObject.Functions = new List<PostgreSQLFunction>();
        AssertEquivalence(originalObject, unmodifiedObject);

        cloneObject = originalObject.CopyModel();
        cloneObject.Tables.Clear();
        cloneObject.EnumTypes.Clear();
        cloneObject.CompositeTypes = new List<PostgreSQLCompositeType>();
        cloneObject.CompositeTypes.Add(new PostgreSQLCompositeType() { Name = "type1" });
        AssertEquivalence(originalObject, unmodifiedObject);

        cloneObject = originalObject.CopyModel();
        cloneObject.Tables.Single().PrimaryKey.Name = "other";
        cloneObject.Tables.Single().PrimaryKey.Columns.RemoveAt(1);
        cloneObject.Tables.Single().PrimaryKey.Columns[0] = "other";
        cloneObject.Tables.Single().PrimaryKey.Parent = cloneObject.Tables.Single().PrimaryKey;
        cloneObject.Tables.Single().Columns.RemoveAt(1);
        cloneObject.Tables.Single().Columns.Single(x => x.Name == "Col1").ID = default;
        cloneObject.Tables.Single().Columns.Single(x => x.Name == "Col3").DataType = null;
        cloneObject.Tables.Single().Columns.Single(x => x.Name == "Col3").Default.Code = "Some OTHER code";
        AssertEquivalence(originalObject, unmodifiedObject);
    }

    private static void AssertEquivalence<TModel>(TModel model1, TModel model2)
    {
        string serializedModel1 = MiscHelper.SerializeToJsonWithReferences(model1);
        string serializedModel2 = MiscHelper.SerializeToJsonWithReferences(model2);
        serializedModel1.Should().Be(serializedModel2);
    }

    private static PostgreSQLDatabase CreateTestDatabaseModel()
    {
        PostgreSQLDatabase db = new()
        {
            Version = 21,
            Tables = new List<Table>()
            {
                new PostgreSQLTable()
                {
                    PrimaryKey = new PrimaryKey()
                    {
                        Name = "pk1",
                        Columns = new List<string>() { "c1", "c2", "c3" }
                    },
                    Columns = new List<Column>()
                    {
                        new Column()
                        {
                            ID = new Guid("67A97E88-9E8B-40AE-8A50-D03B21BC8174"),
                            Name = "Col1",
                            DataType = new DataType(),
                        },
                        new Column(),
                        new Column()
                        {
                            Name = "Col3",
                            DataType = new DataType()
                            {
                                Name = "SomeType",
                            },
                            Default = new CodePiece() { Code = "Some code" }
                        },
                    },
                },
            },
            EnumTypes = new List<PostgreSQLEnumType>()
            {
                new PostgreSQLEnumType()
                {
                    AllowedValues = new List<string>() { "Val1", "Val2" },
                },
            },
            DomainTypes = new List<PostgreSQLDomainType>()
            {
                new PostgreSQLDomainType()
                {
                    Default = new CodePiece() { Code = "some expr" },
                },
            },
            Functions = null,
        };

        db.Tables.Single().PrimaryKey.Parent = db.Tables.Single();
        db.Tables.Single().Parent = db.Tables.Single().PrimaryKey;

        db.Tables.Single().Columns.First().DataType.DependsOn = new List<DbObject>() { db.DomainTypes.Single() };
        db.DomainTypes.Single().Default.DependsOn = new List<DbObject>() { db.Tables.Single().Columns.First() };

        return db;
    }
}
