using System.Collections.Generic;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Models.MySQL;

public class MySQLDatabase : Database
{
    public MySQLDatabase()
       : this(null) { }

    public MySQLDatabase(string name)
    {
        Kind = DatabaseKind.MySQL;
        Name = name;
        Tables = new List<MySQLTable>();
        Views = new List<MySQLView>();
        Functions = new List<MySQLFunction>();
        Procedures = new List<MySQLProcedure>();
    }

    public IEnumerable<MySQLFunction> Functions { get; set; }
    public IEnumerable<MySQLProcedure> Procedures { get; set; }
}
