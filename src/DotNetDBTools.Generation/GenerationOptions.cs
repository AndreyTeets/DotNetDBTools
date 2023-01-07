namespace DotNetDBTools.Generation;

public class GenerationOptions
{
    /// <summary>
    /// Database name that serves as base for generated code namespaces, class names, e.t.c.
    /// </summary>
    /// <remarks>
    /// Default value is MyDatabase.
    /// </remarks>
    public string DatabaseName { get; set; } = "MyDatabase";

    /// <summary>
    /// Controls what kind of definition will be generated.
    /// </summary>
    /// <remarks>
    /// Default value is CSharp.
    /// </remarks>
    public OutputDefinitionKind OutputDefinitionKind { get; set; } = OutputDefinitionKind.CSharp;
}

public enum OutputDefinitionKind
{
    /// <summary>
    /// Generates .cs definition-files for database objects with links to sql code in embedded .sql files where appropriate.
    /// DatabaseSettings.DefinitionKind is set to DotNetDBTools.Definition.DefinitionKind.CSharp.
    /// </summary>
    CSharp,

    /// <summary>
    /// Generates embedded .sql files with create statement with ID declarations for each database object.
    /// DatabaseSettings.DefinitionKind is set to DotNetDBTools.Definition.DefinitionKind.[MSSQL|MySQL|PostgreSQL|SQLite].
    /// DBMS type is chosen based on the passed database model type.
    /// </summary>
    Sql,
}
