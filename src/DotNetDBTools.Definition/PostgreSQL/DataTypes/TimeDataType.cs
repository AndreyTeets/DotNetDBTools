using DotNetDBTools.Definition.Core;

namespace DotNetDBTools.Definition.PostgreSQL.DataTypes
{
    /// <summary>
    /// Column is declared as 'TIME(Precision)'/'TIMETZ(Precision)'.
    /// </summary>
    public class TimeDataType : IDataType
    {
        /// <remarks>
        /// Default value is 6.
        /// </remarks>
        public byte Precision { get; set; } = 6;

        /// <remarks>
        /// Default value is false.
        /// </remarks>
        public bool IsWithTimeZone { get; set; } = false;
    }
}
