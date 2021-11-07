using DotNetDBTools.Definition.Core;

namespace DotNetDBTools.Definition.MSSQL.DataTypes
{
    /// <summary>
    /// Column is declared as 'BINARY(Length)'/'VARBINARY(Length/MAX)'.
    /// </summary>
    public class BinaryDataType : IDataType
    {
        /// <summary>
        /// Column is declared as 'BINARY(Length)'/'VARBINARY(Length)' if Length is in range [1,8000], otherwise as 'VARBINARY(MAX)' or throws for 'BINARY'.
        /// </summary>
        /// <remarks>
        /// Default value is 50.
        /// </remarks>
        public int Length { get; set; } = 50;

        /// <remarks>
        /// Default value is <see cref="BinarySqlType.VARBINARY"/>.
        /// </remarks>
        public BinarySqlType SqlType { get; set; } = BinarySqlType.VARBINARY;
    }

    public enum BinarySqlType
    {
        BINARY,
        VARBINARY,
    }
}
