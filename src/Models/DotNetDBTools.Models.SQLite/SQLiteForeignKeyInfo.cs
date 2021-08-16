using System.Collections.Generic;

namespace DotNetDBTools.Models.SQLite
{
    public class SQLiteForeignKeyInfo : BaseDBObjectInfo, IForeignKeyInfo
    {
        public IEnumerable<string> ThisColumnNames { get; set; }
        public string ForeignTableName { get; set; }
        public IEnumerable<string> ForeignColumnNames { get; set; }
        public string OnUpdate { get; set; }
        public string OnDelete { get; set; }
    }
}
