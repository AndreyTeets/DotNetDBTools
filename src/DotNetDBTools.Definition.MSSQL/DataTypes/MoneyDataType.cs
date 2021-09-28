using DotNetDBTools.Definition.Core;

namespace DotNetDBTools.Definition.MSSQL.DataTypes
{
    public class MoneyDataType : IDataType
    {
        public MSSQLMoneyType MSSQLType { get; set; } = MSSQLMoneyType.MONEY;
    }

    public enum MSSQLMoneyType
    {
        SMALLMONEY,
        MONEY,
    }
}
