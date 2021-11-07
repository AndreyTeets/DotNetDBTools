using DotNetDBTools.Definition.Core;

namespace DotNetDBTools.Definition.MySQL.DataTypes
{
    /// <summary>
    /// Column is declared as 'CHAR(Length/255)'/'VARCHAR(Length/65535)'/'TINYTEXT'/'TEXT'/'MEDIUMTEXT'/'LONGTEXT'.
    /// </summary>
    public class StringDataType : IDataType
    {
        /// <summary>
        /// If SqlType is not CHAR or VARCHAR property is ignored.<br/>
        /// Otherwise column is declared as 'CHAR(Length)'/'VARCHAR(Length)' if Length is in range [1,255]/[1,65535] or 'CHAR(255)'/'VARCHAR(65535)' if Length is out of range.
        /// </summary>
        /// <remarks>
        /// Default value is 50.
        /// </remarks>
        public int Length { get; set; } = 50;

        /// <remarks>
        /// Default value is <see cref="StringSqlType.VARCHAR"/>.
        /// </remarks>
        public StringSqlType SqlType { get; set; } = StringSqlType.VARCHAR;
    }

    public enum StringSqlType
    {
        CHAR,
        VARCHAR,
        TINYTEXT,
        TEXT,
        MEDIUMTEXT,
        LONGTEXT,
    }
}
