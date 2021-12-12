using System.Collections.Generic;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Models.Agnostic
{
    public class AgnosticDatabase : Database
    {
        public AgnosticDatabase()
            : this(null) { }

        public AgnosticDatabase(string name)
        {
            Kind = DatabaseKind.Agnostic;
            Name = name;
            Tables = new List<AgnosticTable>();
            Views = new List<AgnosticView>();
        }
    }
}
