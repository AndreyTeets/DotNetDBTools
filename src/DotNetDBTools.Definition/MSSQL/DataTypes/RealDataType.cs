using DotNetDBTools.Definition.Core;

namespace DotNetDBTools.Definition.MSSQL.DataTypes
{
    public class RealDataType : IDataType
    {
        public bool IsSingle { get; set; } = false;
    }
}
