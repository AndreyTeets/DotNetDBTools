using System.Collections.Generic;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Generation;

public interface IGenerationManager
{
    public GenerationOptions Options { get; set; }

    /// <summary>
    /// Generates CSharp code containing additional description classes for the database.
    /// DBMS type is chosen based on the passed database model type.
    /// </summary>
    public string GenerateDescription(Database database);
    /// <summary>
    /// Generates CSharp code containing additional description classes for the database.
    /// DBMS type is chosen based on the passed database model type.
    /// </summary>
    public void GenerateDescription(Database database, string outputPath);

    /// <summary>
    /// Generates definition project for the provided database model.
    /// DBMS type is chosen based on the passed database model type.
    /// </summary>
    public IEnumerable<DefinitionSourceFile> GenerateDefinition(Database database);
    /// <summary>
    /// Generates definition project for the provided database model.
    /// DBMS type is chosen based on the passed database model type.
    /// </summary>
    public void GenerateDefinition(Database database, string outputDirectory);
}
