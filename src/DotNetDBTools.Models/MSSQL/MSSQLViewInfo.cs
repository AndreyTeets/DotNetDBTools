using DotNetDBTools.Models.Common;

namespace DotNetDBTools.Models.MSSQL
{
    public class MSSQLViewInfo : BaseDBObjectInfo, IViewInfo
    {
        public string Code { get; set; }
    }
}
