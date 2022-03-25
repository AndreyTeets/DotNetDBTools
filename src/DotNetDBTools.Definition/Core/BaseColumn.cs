using System;

namespace DotNetDBTools.Definition.Core;

public abstract class BaseColumn : IDbObject
{
    private readonly Guid _id;
    protected BaseColumn(string id)
    {
        _id = new Guid(id);
    }

    public Guid ID => _id;
    public IDataType DataType { get; set; }
    public bool NotNull { get; set; }
    public bool Identity { get; set; }
    public object Default { get; set; }
}
