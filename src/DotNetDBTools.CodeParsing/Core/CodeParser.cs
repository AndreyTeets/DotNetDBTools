using System;
using System.Collections.Generic;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using DotNetDBTools.CodeParsing.Models;

namespace DotNetDBTools.CodeParsing.Core;

public abstract class CodeParser<TParser, TLexer> : ICodeParser
    where TParser : Parser
    where TLexer : Lexer
{
    /// <summary>
    /// Ignores ID declarations if set to true.
    /// Otherwise throws ParseException if ID declaration is missing for any object that's supposed to have it.
    /// </summary>
    /// <remarks>
    /// Default value is false.
    /// </remarks>
    public bool IgnoreIdsWhenParsingObjectInfo { get; set; } = false;

    private readonly ErrorListener _errorListener = new();

    public abstract ObjectInfo GetObjectInfo(string createStatement);

    protected ObjectInfo ParseObjectInfo<TGetObjectInfoVisitor>(string input, Func<TParser, IParseTree> startRule)
        where TGetObjectInfoVisitor : AbstractParseTreeVisitor<ObjectInfo>
    {
        try
        {
            IParseTree parseTree = Parse(input, startRule);
            TGetObjectInfoVisitor visitor = (TGetObjectInfoVisitor)Activator.CreateInstance(
                typeof(TGetObjectInfoVisitor), new object[] { IgnoreIdsWhenParsingObjectInfo });
            return visitor.Visit(parseTree);
        }
        catch (ParseException ex)
        {
            throw new ParseException($"Failed to parse object info: {ex.Message}\ninput=[{input}]");
        }
    }

    protected IParseTree Parse(string input, Func<TParser, IParseTree> startRule)
    {
        TParser parser = CreateParser(input);
        IParseTree parseTree = startRule(parser);
        ProcessErrors();
        return parseTree;
    }

    private void ProcessErrors()
    {
        List<string> errors = _errorListener.GetErrors();
        if (errors.Count > 0)
        {
            string errorMsg = string.Join("\n", errors);
            _errorListener.ClearErrors();
            throw new ParseException(errorMsg);
        }
    }

    private TParser CreateParser(string input)
    {
        ICharStream charStream = CharStreams.fromString(input);
        TLexer lexer = (TLexer)Activator.CreateInstance(typeof(TLexer), charStream);
        lexer.RemoveErrorListeners();
        lexer.AddErrorListener(_errorListener);

        ITokenStream tokenStream = new CommonTokenStream(lexer);
        TParser parser = (TParser)Activator.CreateInstance(typeof(TParser), tokenStream);
        parser.RemoveErrorListeners();
        parser.AddErrorListener(_errorListener);

        return parser;
    }
}
