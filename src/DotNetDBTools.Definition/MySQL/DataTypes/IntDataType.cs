using DotNetDBTools.Definition.Core;

namespace DotNetDBTools.Definition.MySQL.DataTypes
{
    /// <summary>
    /// Column is declared as 'TINYINT'/'SMALLINT'/'MEDIUMINT'/'INT'/'BIGINT'.
    /// </summary>
    public class IntDataType : IDataType
    {
        /// <remarks>
        /// Default value is <see cref="IntSize.Int32"/>.
        /// </remarks>
        public IntSize Size { get; set; } = IntSize.Int32;

        public byte DisplayWidth { get; set; } = 0;
        public bool IsUnsigned { get; set; } = false;
    }

    public enum IntSize
    {
        Int8,
        Int16,
        Int24,
        Int32,
        Int64,
    }
}
