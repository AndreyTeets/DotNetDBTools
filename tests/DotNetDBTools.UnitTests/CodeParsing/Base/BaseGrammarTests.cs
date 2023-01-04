using System;
using System.IO;
using System.Text;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using DotNetDBTools.CodeParsing;
using DotNetDBTools.CodeParsing.Core;
using DotNetDBTools.CodeParsing.Models;
using DotNetDBTools.UnitTests.Utilities;
using FluentAssertions;
using Xunit;

namespace DotNetDBTools.UnitTests.CodeParsing.Base;

public abstract class BaseGrammarTests<TParser, TLexer>
    where TParser : Parser
    where TLexer : Lexer
{
    protected abstract string TestDataDir { get; }
    protected abstract Func<TParser, IParseTree> ListOfStatementsStartRule { get; }
    protected abstract Action<TestCodeParser, IParseTree> DoAdditionalParsing { get; }

    [Fact]
    public void ParseExamples_ShouldNotThrow()
    {
        TestCodeParser parser = new();

        int totalFilesCount = 0;
        int failedFilesCount = 0;
        StringBuilder sb = new();
        foreach (string filePath in Directory.EnumerateFiles(TestDataDir, "*.sql", SearchOption.TopDirectoryOnly))
        {
            string input = FilesHelper.GetFromFile(filePath);
            try
            {
                totalFilesCount++;
                IParseTree parseTree = parser.ParseToTree(input, ListOfStatementsStartRule);
                DoAdditionalParsing(parser, parseTree);
            }
            catch (ParseException ex)
            {
                failedFilesCount++;
                sb.AppendLine($"Failed to parse file '{Path.GetFileName(filePath)}': {ex.Message}");
                sb.AppendLine();
            }
            catch (AdditionalParseException ex)
            {
                failedFilesCount++;
                sb.AppendLine($"Failed to parse file '{Path.GetFileName(filePath)}': {ex.Message}");
                sb.AppendLine();
            }
        }

        sb.ToString().Trim().Should().BeEmpty($"{failedFilesCount} failed files out of {totalFilesCount} should succeed");
    }

    protected class TestCodeParser : CodeParser<TParser, TLexer>
    {
        public override ObjectInfo GetObjectInfo(string input)
        {
            throw new NotImplementedException();
        }

        public IParseTree ParseToTree(string input, Func<TParser, IParseTree> startRule)
        {
            return Parse(input, startRule);
        }
    }

    protected class AdditionalParseException : Exception
    {
        public AdditionalParseException(string message) : base(message) { }
    }
}
