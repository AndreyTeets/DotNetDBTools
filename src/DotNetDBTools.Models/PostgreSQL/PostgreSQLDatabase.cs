using System.Collections.Generic;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Models.PostgreSQL
{
    public class PostgreSQLDatabase : Database
    {
        public PostgreSQLDatabase(string name)
        {
            Kind = DatabaseKind.PostgreSQL;
            Name = name;
            Tables = new List<PostgreSQLTable>();
            Views = new List<PostgreSQLView>();
            Functions = new List<PostgreSQLFunction>();
            Procedures = new List<PostgreSQLProcedure>();
        }

        public IEnumerable<PostgreSQLFunction> Functions { get; set; }
        public IEnumerable<PostgreSQLProcedure> Procedures { get; set; }
    }
}
