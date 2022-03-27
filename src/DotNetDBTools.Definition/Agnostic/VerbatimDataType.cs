using System;
using DotNetDBTools.Definition.Core;

namespace DotNetDBTools.Definition.Agnostic;

public class VerbatimDataType : IDataType
{
    public Func<DbmsKind, string> Name { get; private set; }

    public VerbatimDataType(Func<DbmsKind, string> name)
    {
        Name = name;
    }
}
