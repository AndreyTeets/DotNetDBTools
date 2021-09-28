using DotNetDBTools.Definition.Core;

namespace DotNetDBTools.Definition.Agnostic.DataTypes
{
    public class DecimalDataType : IDataType
    {
        public byte Precision { get; set; } = 19;
        public byte Scale { get; set; } = 2;
    }
}
