using DotNetDBTools.Definition.Core;

namespace DotNetDBTools.Definition.PostgreSQL.DataTypes;

/// <summary>
/// Column is declared as 'JSON'/'JSONB'.
/// </summary>
public class JsonDataType : IDataType
{
    /// <remarks>
    /// Default value is <see cref="JsonSqlType.JSONB"/>.
    /// </remarks>
    public JsonSqlType SqlType { get; set; } = JsonSqlType.JSONB;
}

public enum JsonSqlType
{
    JSON,
    JSONB,
}
