using System;

namespace DotNetDBTools.Definition;

[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false, Inherited = false)]
public sealed class DefinitionSettingsAttribute : Attribute
{
    public DefinitionKind DefinitionKind { get; private set; }

    public DefinitionSettingsAttribute(DefinitionKind definitionKind)
    {
        DefinitionKind = definitionKind;
    }
}
