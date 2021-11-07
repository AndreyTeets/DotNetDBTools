using DotNetDBTools.Definition.Core;

namespace DotNetDBTools.Definition.MySQL.DataTypes
{
    /// <summary>
    /// Column is declared as 'DECIMAL(Precision, Scale)'.
    /// </summary>
    public class DecimalDataType : IDataType
    {
        /// <remarks>
        /// Default value is 19.
        /// </remarks>
        public byte Precision { get; set; } = 19;

        /// <remarks>
        /// Default value is 2.
        /// </remarks>
        public byte Scale { get; set; } = 2;
    }
}
