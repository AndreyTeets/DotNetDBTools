using System.Collections.Generic;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Generation;

public interface IGenerationManager
{
    public GenerationOptions Options { get; set; }

    public string GenerateDescription(Database database);
    public void GenerateDescription(Database database, string outputPath);

    public IEnumerable<DefinitionSourceFile> GenerateDefinition(Database database);
    public void GenerateDefinition(Database database, string outputDirectory);
}
