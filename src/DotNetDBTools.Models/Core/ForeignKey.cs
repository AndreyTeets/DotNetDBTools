using System.Collections.Generic;

namespace DotNetDBTools.Models.Core
{
    public class ForeignKey : DBObject
    {
        public IEnumerable<string> ThisColumnNames { get; set; }
        public string ReferencedTableName { get; set; }
        public IEnumerable<string> ReferencedTableColumnNames { get; set; }
        public string OnUpdate { get; set; }
        public string OnDelete { get; set; }
    }
}
