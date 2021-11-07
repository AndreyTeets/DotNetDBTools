using DotNetDBTools.Definition.Core;

namespace DotNetDBTools.Definition.MySQL.DataTypes
{
    /// <summary>
    /// Column is declared as 'BIT(Length)'.
    /// </summary>
    public class BitStringDataType : IDataType
    {
        /// <remarks>
        /// Default value is 8.
        /// </remarks>
        public int Length { get; set; } = 8;
    }
}
