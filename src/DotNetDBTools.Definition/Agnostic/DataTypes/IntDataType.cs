using DotNetDBTools.Definition.Core;

namespace DotNetDBTools.Definition.Agnostic.DataTypes;

/// <summary>
/// Column is declared with a int type appropriate to the used dbms.
/// <list type="bullet">
/// <item><term>MSSQL</term> Column is declared as 'TINYINT'/'SMALLINT'/'INT'/'BIGINT'.</item>
/// <item><term>MySQL</term> Column is declared as 'TINYINT'/'SMALLINT'/'INT'/'BIGINT'.</item>
/// <item><term>PostgreSQL</term> Column is declared as 'SMALLINT'/'INT'/'BIGINT'.</item>
/// <item><term>SQLite</term> Column is declared with 'INTEGER' affinity.</item>
/// </list>
/// </summary>
public class IntDataType : IDataType
{
    /// <remarks>
    /// Default value is <see cref="IntSize.Int32"/>.
    /// </remarks>
    public IntSize Size { get; set; } = IntSize.Int32;
}

public enum IntSize
{
    Int8,
    Int16,
    Int32,
    Int64,
}
