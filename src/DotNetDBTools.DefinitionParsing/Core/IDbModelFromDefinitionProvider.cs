using System.Reflection;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.DefinitionParsing.Core;

internal interface IDbModelFromDefinitionProvider
{
    Database CreateDbModel(Assembly dbAssembly);
}
