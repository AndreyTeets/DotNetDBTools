using System.Collections.Generic;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Models.Agnostic
{
    public class AgnosticDatabaseInfo : DatabaseInfo
    {
        public AgnosticDatabaseInfo(string name)
        {
            Kind = DatabaseKind.Agnostic;
            Name = name;
            Tables = new List<AgnosticTableInfo>();
            Views = new List<AgnosticViewInfo>();
        }
    }
}
