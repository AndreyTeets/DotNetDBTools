using System;
using DotNetDBTools.Definition.Core;
using DotNetDBTools.Definition.Core.CSharpDefaultValues;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.DefinitionParsing.Core;

internal abstract class DefaultValueMapper : IDefaultValueMapper
{
    public abstract CodePiece MapToDefaultValueModel(IDefaultValue defaultValue);

    protected CSharpDefaultValue CreateCSharpDefaultValueModel(IDefaultValue defaultValue)
    {
        return defaultValue switch
        {
            IntDefaultValue dv => new CSharpDefaultValue { CSharpValue = dv.Value },
            RealDefaultValue dv => new CSharpDefaultValue { CSharpValue = dv.Value },
            DecimalDefaultValue dv => new CSharpDefaultValue { CSharpValue = dv.Value },
            BoolDefaultValue dv => new CSharpDefaultValue { CSharpValue = dv.Value },

            StringDefaultValue dv => new CSharpDefaultValue { CSharpValue = dv.Value },
            BinaryDefaultValue dv => new CSharpDefaultValue { CSharpValue = dv.Value },
            GuidDefaultValue dv => new CSharpDefaultValue { CSharpValue = dv.Value },

            DateDefaultValue dv => new CSharpDefaultValue { CSharpValue = dv.Value },
            TimeDefaultValue dv => new CSharpDefaultValue { CSharpValue = dv.Value },
            DateTimeDefaultValue dv => new CSharpDefaultValue { CSharpValue = dv.Value, IsWithTimeZone = dv.IsWithTimeZone },

            _ => throw new InvalidOperationException($"Invalid csharp default value: {defaultValue}"),
        };
    }
}
