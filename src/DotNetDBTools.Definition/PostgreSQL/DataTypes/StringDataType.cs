using DotNetDBTools.Definition.Core;

namespace DotNetDBTools.Definition.PostgreSQL.DataTypes
{
    /// <summary>
    /// Column is declared as 'CHAR(Length)'/'VARCHAR(Length)'/'TEXT'.
    /// </summary>
    public class StringDataType : IDataType
    {
        /// <summary>
        /// If SqlType is not CHAR or VARCHAR property is ignored.<br/>
        /// Otherwise column is declared as 'CHAR(Length)'/'VARCHAR(Length)'.
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
        TEXT,
    }
}
