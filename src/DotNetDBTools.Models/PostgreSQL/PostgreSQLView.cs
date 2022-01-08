using System.Collections.Generic;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Models.PostgreSQL
{
    public class PostgreSQLView : View
    {
        public List<DBObject> Dependencies { get; set; }
    }
}
