using System.Collections.Generic;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Models.Agnostic
{
    public class AgnosticDatabaseInfo : IDatabaseInfo<AgnosticTableInfo>
    {
        public AgnosticDatabaseInfo(string name)
        {
            Kind = DatabaseKind.Agnostic;
            Name = name;
        }

        public DatabaseKind Kind { get; private set; }
        public string Name { get; private set; }
        public IEnumerable<ITableInfo> Tables { get; set; } = new List<AgnosticTableInfo>();
        public IEnumerable<IViewInfo> Views { get; set; } = new List<AgnosticViewInfo>();
    }
}
