using DotNetDBTools.Definition.Core;

namespace DotNetDBTools.Definition.MSSQL.DataTypes
{
    public class IntDataType : IDataType
    {
        public IntSize Size { get; set; } = IntSize.Int32;
    }

    public enum IntSize
    {
        Int8,
        Int16,
        Int32,
        Int64,
    }
}
