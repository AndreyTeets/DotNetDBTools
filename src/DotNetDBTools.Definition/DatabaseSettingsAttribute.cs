using System;

namespace DotNetDBTools.Definition;

[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false, Inherited = false)]
public class DatabaseSettingsAttribute : Attribute
{
    /// <remarks>
    /// Default value is CSharp.
    /// </remarks>
    public DefinitionKind DefinitionKind { get; set; } = DefinitionKind.CSharp;

    /// <remarks>
    /// Default value is 1.
    /// </remarks>
    public long DatabaseVersion { get; set; } = 1;
}
