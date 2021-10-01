using DotNetDBTools.Definition.Core;

namespace DotNetDBTools.Definition.MSSQL.DataTypes
{
    public class DateTimeDataType : IDataType
    {
        public MSSQLDateTimeType MSSQLType { get; set; } = MSSQLDateTimeType.DATETIME2;
    }

    public enum MSSQLDateTimeType
    {
        SMALLDATETIME,
        DATETIME,
        DATETIME2,
        DATETIMEOFFSET,
    }
}
