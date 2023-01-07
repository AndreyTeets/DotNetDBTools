using DotNetDBTools.CodeParsing.Models;

namespace DotNetDBTools.CodeParsing;

public interface ICodeParser
{
    /// <summary>
    /// Parses database object information from the provided create statement.
    /// </summary>
    public ObjectInfo GetObjectInfo(string createStatement);
}
