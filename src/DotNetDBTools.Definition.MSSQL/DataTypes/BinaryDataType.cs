using DotNetDBTools.Definition.Core;

namespace DotNetDBTools.Definition.MSSQL.DataTypes
{
    /// <summary>
    /// Column is declared as 'binary'/'varbinary'.
    /// </summary>
    public class BinaryDataType : IDataType
    {
        /// <summary>
        /// The type is declared as 'binary(<see cref="Length"/>)'/'varbinary(<see cref="Length"/>)'<br/>
        /// if <see cref="Length"/> is in range [1,8000] otherwise as 'binary(MAX)'/'varbinary(MAX)'.
        /// </summary>
        /// <remarks>
        /// Default value is 50.
        /// </remarks>
        public int Length { get; set; } = 50;

        /// <summary>
        /// The type is declared as 'binary' if <see cref="Length"/>=true', otherwise as 'varbinary'.
        /// </summary>
        /// <remarks>
        /// Default value is false.
        /// </remarks>
        public bool IsFixedLength { get; set; } = false;
    }
}
