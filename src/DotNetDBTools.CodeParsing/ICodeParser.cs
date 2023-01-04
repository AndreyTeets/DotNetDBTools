using DotNetDBTools.CodeParsing.Models;

namespace DotNetDBTools.CodeParsing;

public interface ICodeParser
{
    public ObjectInfo GetObjectInfo(string input);
}
