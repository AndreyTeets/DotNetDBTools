using System.Collections.Generic;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.DefinitionParsing.Core;

internal interface IDbModelFromSqlDefinitionProvider : IDbModelFromDefinitionProvider
{
    public Database CreateDbModel(IEnumerable<string> definitionSqlStatements, long dbVersion);
}
