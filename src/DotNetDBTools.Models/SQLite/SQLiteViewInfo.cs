using DotNetDBTools.Models.Common;

namespace DotNetDBTools.Models.SQLite
{
    public class SQLiteViewInfo : BaseDBObjectInfo, IViewInfo
    {
        public string Code { get; set; }
    }
}
