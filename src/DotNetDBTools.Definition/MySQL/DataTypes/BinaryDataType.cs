using DotNetDBTools.Definition.Core;

namespace DotNetDBTools.Definition.MySQL.DataTypes
{
    /// <summary>
    /// Column is declared as 'BINARY(Length/255)'/'VARBINARY(Length/65535)'/'TINYBLOB'/'BLOB'/'MEDIUMBLOB'/'LONGBLOB'.
    /// </summary>
    public class BinaryDataType : IDataType
    {
        /// <summary>
        /// If SqlType is not BINARY or VARBINARY property is ignored.<br/>
        /// Otherwise column is declared as 'BINARY(Length)'/'VARBINARY(Length)' if Length is in range [1,255]/[1,65535] or 'BINARY(255)'/'VARBINARY(65535)' if Length is out of range.
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
        TINYBLOB,
        BLOB,
        MEDIUMBLOB,
        LONGBLOB,
    }
}
