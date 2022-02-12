using DotNetDBTools.Definition.Core;

namespace DotNetDBTools.Definition.MSSQL.DataTypes;

/// <summary>
/// Column is declared as 'SMALLMONEY'/'MONEY'.
/// </summary>
public class MoneyDataType : IDataType
{
    /// <remarks>
    /// Default value is <see cref="MoneySqlType.MONEY"/>.
    /// </remarks>
    public MoneySqlType SqlType { get; set; } = MoneySqlType.MONEY;
}

public enum MoneySqlType
{
    SMALLMONEY,
    MONEY,
}
