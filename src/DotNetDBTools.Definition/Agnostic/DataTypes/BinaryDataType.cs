using DotNetDBTools.Definition.Core;

namespace DotNetDBTools.Definition.Agnostic.DataTypes
{
    /// <summary>
    /// Column is declared with a binary type appropriate to the used dbms.
    /// <list type="bullet">
    /// <item><term>SQLite</term> Column is declared with 'BLOB' affinity.</item>
    /// <item><term>MSSQL</term> Column is declared as 'binary'/'varbinary'.</item>
    /// </list>
    /// </summary>
    public class BinaryDataType : IDataType
    {
        /// <summary>
        /// Length declared for the type if the used dbms supports it.
        /// <list type="bullet">
        /// <item><term>SQLite</term> Property is ignored.</item>
        /// <item><term>MSSQL</term> The type is declared as 'binary(<see cref="Length"/>)'/'varbinary(<see cref="Length"/>)'<br/>
        /// if <see cref="Length"/> is in range [1,8000] otherwise as 'binary(MAX)'/'varbinary(MAX)'.</item>
        /// </list>
        /// </summary>
        /// <remarks>
        /// Default value is 50.
        /// </remarks>
        public int Length { get; set; } = 50;

        /// <summary>
        /// Controls the exact binary type chosen for the used dbms.
        /// <list type="bullet">
        /// <item><term>SQLite</term> Property is ignored.</item>
        /// <item><term>MSSQL</term> The type is declared as 'binary' if <see cref="Length"/>=true', otherwise as 'varbinary'.</item>
        /// </list>
        /// </summary>
        /// <remarks>
        /// Default value is false.
        /// </remarks>
        public bool IsFixedLength { get; set; } = false;
    }
}
