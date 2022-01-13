using System;
using System.Collections.Generic;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

namespace DotNetDBTools.CodeParsing.Core
{
    public abstract class CodeParser<TParser, TLexer>
        where TParser : Parser
        where TLexer : Lexer
    {
        private readonly ErrorListener _errorListener = new();

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
}
