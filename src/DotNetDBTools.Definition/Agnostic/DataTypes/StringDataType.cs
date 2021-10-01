using DotNetDBTools.Definition.Core;

namespace DotNetDBTools.Definition.Agnostic.DataTypes
{
    public class StringDataType : IDataType
    {
        public int Length { get; set; } = 50;
        public bool IsFixedLength { get; set; } = false;
    }
}
