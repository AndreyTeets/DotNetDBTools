using System;
using DotNetDBTools.Definition.Core;

namespace DotNetDBTools.Definition.Agnostic;

public class VerbatimDataType : IDataType
{
    public Func<DbmsKind, string> Name { get; private set; }

    /// <summary>
    /// Name will appear in sql normalized and automatically quoted if it's required.
    /// </summary>
    public VerbatimDataType(Func<DbmsKind, string> name)
    {
        Name = name;
    }
}
