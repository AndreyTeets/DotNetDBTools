using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Models.Agnostic;

public class AgnosticDatabase : Database
{
    public AgnosticDatabase()
    {
        Kind = DatabaseKind.Agnostic;
    }
}
