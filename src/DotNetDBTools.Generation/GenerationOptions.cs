namespace DotNetDBTools.Generation;

public class GenerationOptions
{
    /// <summary>
    /// Database name that serves as base for generated code namespaces, class names, e.t.c.
    /// </summary>
    /// <remarks>
    /// Default value is MyDatabase.
    /// </remarks>
    public string DatabaseName { get; set; }
}
