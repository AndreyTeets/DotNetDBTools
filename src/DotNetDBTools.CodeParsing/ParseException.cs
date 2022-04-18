using System;

namespace DotNetDBTools.CodeParsing;

public class ParseException : Exception
{
    public ParseException(string message) : base(message) { }
}
