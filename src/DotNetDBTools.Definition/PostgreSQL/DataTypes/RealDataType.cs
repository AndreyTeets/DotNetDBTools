using DotNetDBTools.Definition.Core;

namespace DotNetDBTools.Definition.PostgreSQL.DataTypes
{
    /// <summary>
    /// Column is declared as 'FLOAT4'/'FLOAT8'.
    /// </summary>
    public class RealDataType : IDataType
    {
        /// <remarks>
        /// Default value is true.
        /// </remarks>
        public bool IsDoublePrecision { get; set; } = true;
    }
}
