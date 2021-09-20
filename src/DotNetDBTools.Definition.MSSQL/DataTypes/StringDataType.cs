using DotNetDBTools.Definition.Core.BaseDataTypes;

namespace DotNetDBTools.Definition.MSSQL.DataTypes
{
    public class StringDataType : BaseStringDataType
    {
        public bool IsFixedLength { get; set; } = false;
    }
}
