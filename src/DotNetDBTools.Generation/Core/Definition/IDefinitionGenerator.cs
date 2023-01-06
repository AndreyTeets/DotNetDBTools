using System.Collections.Generic;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Generation.Core.Definition;

internal interface IDefinitionGenerator
{
    public OutputDefinitionKind OutputDefinitionKind { get; set; }
    public IEnumerable<DefinitionSourceFile> GenerateDefinition(Database database, string projectNamespace);
}
