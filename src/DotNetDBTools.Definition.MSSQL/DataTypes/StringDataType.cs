using DotNetDBTools.Definition.BaseDataTypes;

namespace DotNetDBTools.Definition.MSSQL.DataTypes
{
    public class StringDataType : BaseStringDataType
    {
        public bool IsFixedLength { get; set; } = false;
    }
}
