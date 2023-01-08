using System;
using DotNetDBTools.Analysis.PostgreSQL;
using FluentAssertions;
using Xunit;
using PgDt = DotNetDBTools.Models.PostgreSQL.PostgreSQLDataTypeNames;

namespace DotNetDBTools.UnitTests.Analysis.PostgreSQL;

public class PostgreSQLHelperMethodsTests
{
    [Theory]
    [InlineData("_SomeCustomType1", @"""_SomeCustomType1""", null)]
    [InlineData("Varying", @"""Varying""", null)]
    [InlineData("inTERval", "INTERVAL", PgDt.INTERVAL)]
    [InlineData("inTERval  MiNuTe  tO   seConD  ( 6 )[]", "INTERVAL MINUTE TO SECOND(6)[]", PgDt.INTERVAL)]
    [InlineData("inT4", "INT", PgDt.INT)]
    [InlineData("inTX", @"""inTX""", null)]
    [InlineData("intEGER", "INT", PgDt.INT)]
    [InlineData("DouBle  PreCision", "FLOAT8", PgDt.FLOAT8)]
    [InlineData("DouBle", @"""DouBle""", null)]
    [InlineData("BiT  VarYing", "VARBIT", PgDt.VARBIT)]
    [InlineData("BiT", "BIT", PgDt.BIT)]
    [InlineData("BiTX", @"""BiTX""", null)]
    [InlineData("CharActer  VarYing", "VARCHAR", PgDt.VARCHAR)]
    [InlineData("CharActer", "CHAR", PgDt.CHAR)]
    [InlineData("CharActerX", @"""CharActerX""", null)]
    [InlineData("TimE (0, 3) WiTh TiMe ZoNe [4][3]", "TIMETZ(0,3)[][]", PgDt.TIMETZ)]
    [InlineData("TimE (0, 3) WiThOuT TiMe ZoNe [4][3]", "TIME(0,3)[][]", PgDt.TIME)]
    [InlineData("TimE (0, 3)[4][3]", "TIME(0,3)[][]", PgDt.TIME)]
    [InlineData("TimETZ (0, 3)[4][3]", "TIMETZ(0,3)[][]", PgDt.TIMETZ)]
    [InlineData("TimeStaMP (0, 3) WiTh TiMe ZoNe [4][3]", "TIMESTAMPTZ(0,3)[][]", PgDt.TIMESTAMPTZ)]
    [InlineData("TimeStaMP (0, 3) WiThOuT TiMe ZoNe [4][3]", "TIMESTAMP(0,3)[][]", PgDt.TIMESTAMP)]
    [InlineData("TimeStaMP (0, 3)[4][3]", "TIMESTAMP(0,3)[][]", PgDt.TIMESTAMP)]
    [InlineData("TimeStaMPTZ (0, 3)[4][3]", "TIMESTAMPTZ(0,3)[][]", PgDt.TIMESTAMPTZ)]
    [InlineData("NumERic  ( 3, 4 )  [  ] [ 5 ] [ 2 ]", "DECIMAL(3,4)[][][]", PgDt.DECIMAL)]
    [InlineData("NumERicX  [  ] [ 5 ] [ 2 ]", @"""NumERicX""[][][]", null)]
    public void TryGetNormalizedTypeName_ProducesCorrectResults(
        string typeName, string expectedNormalizedTypeName, string expectedStandardSqlTypeNameBase)
    {
        string normalizedTypeNameWithoutArray = PostgreSQLHelperMethods.GetNormalizedTypeNameWithoutArray(
            typeName, out string standardSqlTypeNameBase, out string arrayDimsStr);

        string normalizedTypeName = $"{normalizedTypeNameWithoutArray ?? typeName}{arrayDimsStr}";

        normalizedTypeName.Should().Be(expectedNormalizedTypeName);
        standardSqlTypeNameBase.Should().Be(expectedStandardSqlTypeNameBase);
    }

    [Theory]
    [InlineData("inTERval '3 DaY'")]
    [InlineData("DouBleX  PreCision")]
    [InlineData("DouBle  PreCisionX (0, 3) [4][3]")]
    [InlineData("CharActerX  VarYing")]
    public void TryGetNormalizedTypeName_ThrowsOnInvalidType(string typeName)
    {
        typeName.Invoking(x => PostgreSQLHelperMethods.GetNormalizedTypeNameWithoutArray(x, out string _, out string _))
            .Should().Throw<Exception>().WithMessage($"Invalid datatype name '{typeName}'");
    }
}
