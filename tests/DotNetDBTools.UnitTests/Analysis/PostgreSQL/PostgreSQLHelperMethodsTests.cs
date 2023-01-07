using System;
using DotNetDBTools.Analysis.PostgreSQL;
using FluentAssertions;
using Xunit;

namespace DotNetDBTools.UnitTests.Analysis.PostgreSQL;

public class PostgreSQLHelperMethodsTests
{
    [Theory]
    [InlineData("inTERval", null)]
    [InlineData("inT", "INT")]
    [InlineData("inTX", null)]
    [InlineData("intEGER", "INT")]
    [InlineData("DouBle  PreCision", "FLOAT8")]
    [InlineData("DouBle", null)]
    [InlineData("CharActer  VarYing", "VARCHAR")]
    [InlineData("CharActer", "CHAR")]
    [InlineData("CharActerX", null)]
    [InlineData("TimeStaMP (0, 3) WiTh TiMe ZoNe [4][3]", "TIMESTAMPTZ(0,3)[4][3]")]
    [InlineData("TimeStaMP (0, 3) WiThOuT TiMe ZoNe [4][3]", "TIMESTAMP(0,3)[4][3]")]
    [InlineData("TimeStaMP (0, 3)[4][3]", "TIMESTAMP(0,3)[4][3]")]
    [InlineData("TimeStaMPTZ (0, 3)[4][3]", "TIMESTAMPTZ(0,3)[4][3]")]
    [InlineData("NumERic  ( 3, 4 )  [  ] [ 5 ] [ 2 ]", "DECIMAL(3,4)[][5][2]")]
    [InlineData("NumERicX  ( 3, 4 )  [  ] [ 5 ] [ 2 ]", null)]
    public void TryGetNormalizedTypeName_GetCorrectResult(string typeName, string expectedNormalizedTypeName)
    {
        string normalizedTypeName = PostgreSQLHelperMethods.TryGetNormalizedTypeName(typeName, out string baseName);
        normalizedTypeName.Should().Be(expectedNormalizedTypeName);
    }

    [Theory]
    [InlineData("inTERval '3 DaY'")]
    [InlineData("DouBleX  PreCision")]
    [InlineData("DouBle  PreCisionX (0, 3) [4][3]")]
    [InlineData("CharActerX  VarYing")]
    public void TryGetNormalizedTypeName_ThrowsOnInvalidType(string typeName)
    {
        typeName.Invoking(x => PostgreSQLHelperMethods.TryGetNormalizedTypeName(x, out string y))
            .Should().Throw<Exception>().WithMessage($"Invalid datatype name '{typeName}'");
    }
}
