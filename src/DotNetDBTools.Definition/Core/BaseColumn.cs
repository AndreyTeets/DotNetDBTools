using System;

namespace DotNetDBTools.Definition.Core;

public abstract class BaseColumn : IDbObject
{
    private readonly Guid _id;
    protected BaseColumn(string id)
    {
        _id = new Guid(id);
    }

    public Guid DNDBT_OBJECT_ID => _id;
    public IDataType DataType { get; set; }
    public bool NotNull { get; set; }
    public bool Identity { get; set; }
    public IDefaultValue Default { get; set; }
}
