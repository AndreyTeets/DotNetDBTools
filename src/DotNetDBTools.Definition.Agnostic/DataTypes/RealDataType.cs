using DotNetDBTools.Definition.Core;

namespace DotNetDBTools.Definition.Agnostic.DataTypes
{
    public class RealDataType : IDataType
    {
        public bool IsSingle { get; set; } = false;
    }
}
