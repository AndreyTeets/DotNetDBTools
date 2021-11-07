using DotNetDBTools.Definition.Core;

namespace DotNetDBTools.Definition.MSSQL.DataTypes
{
    /// <summary>
    /// Column is declared as 'CHAR(Length)'/'VARCHAR(Length/MAX)'/'NCHAR(Length)'/'NVARCHAR(Length/MAX)'.
    /// </summary>
    public class StringDataType : IDataType
    {
        /// <summary>
        /// Column is declared as 'CHAR(Length)/VARCHAR(Length)'/'NCHAR(Length)/NVARCHAR(Length)' if Length is in range [1,8000]/[1,4000], otherwise as 'VARCHAR(MAX)'/'NVARCHAR(MAX)' or throws for 'CHAR'/'NCHAR'.
        /// </summary>
        /// <remarks>
        /// Default value is 50.
        /// </remarks>
        public int Length { get; set; } = 50;

        /// <remarks>
        /// Default value is <see cref="StringSqlType.NVARCHAR"/>.
        /// </remarks>
        public StringSqlType SqlType { get; set; } = StringSqlType.NVARCHAR;
    }

    public enum StringSqlType
    {
        CHAR,
        VARCHAR,
        NCHAR,
        NVARCHAR,
    }
}
