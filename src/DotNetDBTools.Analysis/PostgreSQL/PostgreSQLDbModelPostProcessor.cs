using DotNetDBTools.Analysis.Core;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.PostgreSQL;

namespace DotNetDBTools.Analysis.PostgreSQL;

public class PostgreSQLDbModelPostProcessor : DbModelPostProcessor
{
    protected override void OrderAdditionalDbObjects(Database database)
    {
        PostgreSQLDatabase db = (PostgreSQLDatabase)database;

        db.CompositeTypes = db.CompositeTypes.OrderByName();
        db.DomainTypes = db.DomainTypes.OrderByName();
        db.EnumTypes = db.EnumTypes.OrderByName();
        db.RangeTypes = db.RangeTypes.OrderByName();

        db.Functions = db.Functions.OrderByName();
        db.Procedures = db.Procedures.OrderByName();
    }
}
