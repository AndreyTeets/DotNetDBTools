using System.Collections.Generic;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Models.Agnostic
{
    public class AgnosticForeignKeyInfo : BaseDBObjectInfo, IForeignKeyInfo
    {
        public IEnumerable<string> ThisColumnNames { get; set; }
        public string ForeignTableName { get; set; }
        public IEnumerable<string> ForeignColumnNames { get; set; }
        public string OnUpdate { get; set; }
        public string OnDelete { get; set; }
    }
}
