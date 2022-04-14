using System.Data.Common;
using Dapper;
using FluentAssertions;

namespace DotNetDBTools.IntegrationTests.Base;

public abstract class BaseDataTester
{
    public bool IsDbmsSpecific { get; set; } = true;

    protected abstract string Quote(string identifier);
    protected abstract string BoolLiteral(bool value);
    protected abstract string BinaryLiteral(string hexBase);
    protected abstract string GuidLiteral(string guidString);

    public void Populate_SampleDb_WithData(DbConnection connection)
    {
        PopulateTable2(connection);
        PopulateTable1(connection);
        PopulateTable4(connection);
        PopulateTable5(connection);
    }

    public void Assert_SampleDb_Data(DbConnection connection, AssertKind assertKind)
    {
        AssertDataInTable2(connection, assertKind);
        AssertDataInTable1(connection, assertKind);
        AssertDataInTable4(connection, assertKind);
        AssertDataInTable5(connection);
    }

    private void PopulateTable1(DbConnection connection)
    {
        connection.Execute(
$@"INSERT INTO {Quote("MyTable1")}
(
    {Quote("MyColumn1")},
    {Quote("MyColumn2")},
    {Quote("MyColumn4")}
)
VALUES
(
    101,
    102,
    100.4
),
(
    201,
    202,
    200.4
);");
    }
    private void AssertDataInTable1(DbConnection connection, AssertKind assertKind)
    {
        string newName = assertKind == AssertKind.V2 ? "NewName" : "";
        connection.QuerySingle<int>(
$@"SELECT
    COUNT(*)
FROM {Quote($"MyTable1{newName}")}
WHERE {Quote("MyColumn1")} = 101 AND {Quote("MyColumn4")} = 100.4
    OR {Quote("MyColumn1")} = 201 AND {Quote("MyColumn4")} = 200.4;")
            .Should().Be(2);
    }

    private void PopulateTable2(DbConnection connection)
    {
        connection.Execute(
$@"INSERT INTO {Quote("MyTable2")}
(
    {Quote("MyColumn1")},
    {Quote("MyColumn2")}
)
VALUES
(
    101,
    {BinaryLiteral("000102")}
),
(
    201,
    {BinaryLiteral("000202")}
);");
    }
    private void AssertDataInTable2(DbConnection connection, AssertKind assertKind)
    {
        string newName = assertKind == AssertKind.V2 ? "NewName" : "";
        string col2Op = assertKind == AssertKind.V1Rollbacked ? "!=" : "=";
        connection.QuerySingle<int>(
$@"SELECT
    COUNT(*)
FROM {Quote("MyTable2")}
WHERE {Quote($"MyColumn1{newName}")} = 101 AND {Quote("MyColumn2")} {col2Op} {BinaryLiteral("000102")}
    OR {Quote($"MyColumn1{newName}")} = 201 AND {Quote("MyColumn2")} {col2Op} {BinaryLiteral("000202")};")
            .Should().Be(2);
    }

    private void PopulateTable4(DbConnection connection)
    {
        connection.Execute(
$@"UPDATE {Quote("MyTable4")} SET
    {Quote("MyColumn1")} = {Quote("MyColumn1")} + 500;");
    }
    private void AssertDataInTable4(DbConnection connection, AssertKind assertKind)
    {
        // Init script adds 1,2,3 and trigger on MyTable2 adds 101,201
        int expectedCount = assertKind == AssertKind.V1NoScripts ? 2 : 5;
        connection.QuerySingle<int>(
$@"SELECT
    COUNT(*)
FROM {Quote("MyTable4")}
WHERE {Quote("MyColumn1")} IN (501,502,503,601,701);")
            .Should().Be(expectedCount);
    }

    private void PopulateTable5(DbConnection connection)
    {
        string extraColumns = IsDbmsSpecific ? GetSpecificDbmsTable5ExtraColumns() : "";
        string extraValues = IsDbmsSpecific ? GetSpecificDbmsTable5ExtraValues() : "";
        connection.Execute(
$@"INSERT INTO {Quote("MyTable5")}
(
    {Quote("MyColumn1")},
    {Quote("MyColumn2")},
    {Quote("MyColumn3")},
    {Quote("MyColumn4")},
    {Quote("MyColumn5")},
    {Quote("MyColumn6")},
    {Quote("MyColumn7")},
    {Quote("MyColumn8")},
    {Quote("MyColumn9")},
    {Quote("MyColumn10")},
    {Quote("MyColumn11")},
    {Quote("MyColumn12")}{extraColumns}
)
VALUES
(
    101,
    '2a2',
    {BinaryLiteral("000103")},
    400.4,
    500.5,
    600.6,
    {BoolLiteral(false)},
    {GuidLiteral("80349D81-94B1-44CC-9C73-888888888888")},
    '2029-09-29',
    '10:10:10',
    '2011-11-11 11:11:11',
    '2012-12-12 12:12:12+00:30'{extraValues}
);");
    }
    private void AssertDataInTable5(DbConnection connection)
    {
        string extraConditions = IsDbmsSpecific ? GetSpecificDbmsTable5ExtraConditions() : "";
        connection.QuerySingle<int>(
$@"SELECT
    COUNT(*)
FROM {Quote("MyTable5")}
WHERE {Quote("MyColumn1")} = 101
    AND {Quote("MyColumn2")} = '2a2'
    AND {Quote("MyColumn3")} = {BinaryLiteral("000103")}
    AND ABS({Quote("MyColumn4")} - 400.4) < 0.01
    AND ABS({Quote("MyColumn5")} - 500.5) < 0.01
    AND {Quote("MyColumn6")} = 600.6
    AND {Quote("MyColumn7")} = {BoolLiteral(false)}
    AND {Quote("MyColumn8")} = {GuidLiteral("80349D81-94B1-44CC-9C73-888888888888")}
    AND {Quote("MyColumn9")} = '2029-09-29'
    AND {Quote("MyColumn10")} = '10:10:10'
    AND {Quote("MyColumn11")} = '2011-11-11 11:11:11'
    AND {Quote("MyColumn12")} = '2012-12-12 12:12:12+00:30'{extraConditions};")
            .Should().Be(1);
    }
    protected abstract string GetSpecificDbmsTable5ExtraColumns();
    protected abstract string GetSpecificDbmsTable5ExtraValues();
    protected abstract string GetSpecificDbmsTable5ExtraConditions();

    public enum AssertKind
    {
        V1,
        V2,
        V1Rollbacked,
        V1NoScripts,
    }
}
