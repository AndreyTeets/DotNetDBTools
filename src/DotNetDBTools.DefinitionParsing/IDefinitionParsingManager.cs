using System.Reflection;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.DefinitionParsing;

public interface IDefinitionParsingManager
{
    public Assembly LoadDbAssembly(string dbAssemblyPath);
    public Database CreateDbModel(string dbAssemblyPath);
    public Database CreateDbModel(Assembly dbAssembly);
}
