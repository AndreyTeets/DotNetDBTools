using System;

namespace DotNetDBTools.CodeParsing.Core;

public class ParseException : Exception
{
    public ParseException(string message) : base(message) { }
}
