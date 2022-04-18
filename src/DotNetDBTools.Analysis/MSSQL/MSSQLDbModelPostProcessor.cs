using DotNetDBTools.Analysis.Core;
using DotNetDBTools.Analysis.Extensions;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.MSSQL;

namespace DotNetDBTools.Analysis.MSSQL;

internal class MSSQLDbModelPostProcessor : DbModelPostProcessor
{
    protected override void OrderAdditionalDbObjects(Database database)
    {
        MSSQLDatabase db = (MSSQLDatabase)database;
        db.UserDefinedTypes = db.UserDefinedTypes.OrderByName();
        db.UserDefinedTableTypes = db.UserDefinedTableTypes.OrderByName();
        db.Functions = db.Functions.OrderByName();
        db.Procedures = db.Procedures.OrderByName();
    }
}
