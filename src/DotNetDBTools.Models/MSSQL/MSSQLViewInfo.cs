using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Models.MSSQL
{
    public class MSSQLViewInfo : BaseDBObjectInfo, IViewInfo
    {
        public string Code { get; set; }
    }
}
