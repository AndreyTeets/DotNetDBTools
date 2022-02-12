using System.Collections.Generic;
using System.IO;
using Antlr4.Runtime;

namespace DotNetDBTools.CodeParsing.Core;

internal class ErrorListener : BaseErrorListener, IAntlrErrorListener<int>
{
    private readonly List<string> _errors = new();

    public List<string> GetErrors() => _errors;
    public void ClearErrors() => _errors.Clear();

    public void SyntaxError(
        TextWriter output,
        IRecognizer recognizer,
        int offendingSymbol,
        int line,
        int charPositionInLine,
        string msg,
        RecognitionException e)
    {
        AddError("LexerError", line, charPositionInLine, msg);
    }

    public override void SyntaxError(
        TextWriter output,
        IRecognizer recognizer,
        IToken offendingSymbol,
        int line,
        int charPositionInLine,
        string msg,
        RecognitionException e)
    {
        AddError("ParserError", line, charPositionInLine, msg);
    }

    private void AddError(string errorType, int line, int charPositionInLine, string msg)
    {
        _errors.Add($"{errorType}(line={line},pos={charPositionInLine}): {msg}");
    }
}
