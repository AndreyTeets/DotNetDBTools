using System;
using DotNetDBTools.CodeParsing.Core;
using DotNetDBTools.CodeParsing.Core.Models;
using FluentAssertions;
using Xunit;

namespace DotNetDBTools.UnitTests.CodeParsing.Core;

public class ScriptDeclarationParserTests
{
    private const string ScriptDeclaration =
@"--ScriptID:#{100D624A-01AA-4730-B86F-F991AC3ED936}#
--ScriptName:#{InsertSomeInitialData}#
--ScriptType:#{AfterPublishOnce}#
--ScriptMinDbVersionToExecute:#{0}#
--ScriptMaxDbVersionToExecute:#{9223372036854775807}#";

    [Fact]
    public void TryParseScriptInfo_GetsCorrectData_OnPositiveInput()
    {
        string input =
@$"{ScriptDeclaration}
bla bla";

        ScriptDeclarationParser.TryParseScriptInfo(input, out ScriptInfo scriptInfo).Should().BeTrue();
        scriptInfo.ID.Should().Be(Guid.Parse("100D624A-01AA-4730-B86F-F991AC3ED936"));
        scriptInfo.Name.Should().Be("InsertSomeInitialData");
        scriptInfo.Type.Should().Be(ScriptType.AfterPublishOnce);
        scriptInfo.MinDbVersionToExecute.Should().Be(0);
        scriptInfo.MaxDbVersionToExecute.Should().Be(long.MaxValue);
        scriptInfo.Code.Should().Be(input.Replace(ScriptDeclaration, "").Trim());
    }

    [Theory]
    [InlineData("--ID:#{100D624A-01AA-4730-B86F-F991AC3ED936}#\nbla bla")]
    [InlineData("bla bla")]
    public void TryParseScriptInfo_ReturnsFalse_OnNegativeInput(string input)
    {
        ScriptDeclarationParser.TryParseScriptInfo(input, out ScriptInfo _).Should().BeFalse();
    }

    [Theory]
    [InlineData("--ScriptID:#\nbla bla")]
    [InlineData("--ScriptID:#")]
    [InlineData($"\n{ScriptDeclaration}\nbla bla")]
    public void ParseFunction_ThrowsOnMalformedInput(string input)
    {
        FluentActions.Invoking(() => ScriptDeclarationParser.TryParseScriptInfo(input, out ScriptInfo _))
            .Should().Throw<Exception>().WithMessage($"Failed to parse script info from input [{input}]");
    }
}
