using System;

namespace DotNetDBTools.Definition;

[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false, Inherited = false)]
public class DatabaseSettingsAttribute : Attribute
{
    /// <remarks>
    /// Default value is CSharp.
    /// </remarks>
    public DefinitionKind DefinitionKind { get; set; } = DefinitionKind.CSharp;

    /// <summary>
    /// Useful when Before/After publish scripts are used, mainly to protect from [Min|Max]DbVersionToExecute mistakes.
    /// Otherwise it may be desirable to disable version check during publish in DeployOptions.
    /// </summary>
    /// <remarks>
    /// Default value is 1.
    /// </remarks>
    public long DatabaseVersion { get; set; } = 1;
}
