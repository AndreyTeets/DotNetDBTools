using DotNetDBTools.Analysis.Core;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.MySQL;

namespace DotNetDBTools.Analysis.MySQL;

public class MySQLDbModelPostProcessor : DbModelPostProcessor
{
    protected override void OrderAdditionalDbObjects(Database database)
    {
        MySQLDatabase db = (MySQLDatabase)database;
        db.Functions = db.Functions.OrderByName();
        db.Procedures = db.Procedures.OrderByName();
    }
}
