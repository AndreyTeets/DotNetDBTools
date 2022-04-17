using System.Collections.Generic;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Models.MySQL;

public class MySQLDatabase : Database
{
    public MySQLDatabase()
    {
        Kind = DatabaseKind.MySQL;
        Tables = new List<MySQLTable>();
        Views = new List<MySQLView>();
        Scripts = new List<Script>();
        Functions = new List<MySQLFunction>();
        Procedures = new List<MySQLProcedure>();
    }

    public IEnumerable<MySQLFunction> Functions { get; set; }
    public IEnumerable<MySQLProcedure> Procedures { get; set; }
}
