using DotNetDBTools.Definition.Core;

namespace DotNetDBTools.Definition.MySQL.DataTypes
{
    /// <summary>
    /// Column is declared as 'ENUM(AllowedValues)'.
    /// </summary>
    public class EnumDataType : IDataType
    {
        public string[] AllowedValues { get; set; }
    }
}
