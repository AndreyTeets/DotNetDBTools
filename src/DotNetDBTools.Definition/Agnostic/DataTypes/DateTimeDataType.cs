using DotNetDBTools.Definition.Core;

namespace DotNetDBTools.Definition.Agnostic.DataTypes
{
    /// <summary>
    /// Column is declared with a datetime type appropriate to the used dbms.
    /// <list type="bullet">
    /// <item><term>MSSQL</term> Column is declared as 'DATETIME2'/'DATETIMEOFFSET'.</item>
    /// <item><term>MySQL</term> Column is declared as 'DATETIME'/'TIMESTAMP'.</item>
    /// <item><term>PostgreSQL</term> Column is declared as 'TIMESTAMP'/'TIMESTAMPTZ'.</item>
    /// <item><term>SQLite</term> Column is declared with 'NUMERIC' affinity.</item>
    /// </list>
    /// </summary>
    public class DateTimeDataType : IDataType
    {
        /// <remarks>
        /// Default value is false.
        /// </remarks>
        public bool IsWithTimeZone { get; set; } = false;
    }
}
