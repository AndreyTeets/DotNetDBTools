using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.Analysis.Core.Errors;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Analysis.Core;

internal abstract class DbValidator
{
    public abstract bool DbIsValid(Database database, out List<DbError> dbErrors);

    protected void AddCoreDbObjectsErrors(Database database, List<DbError> dbErrors)
    {
        AddColumnsErrors(database, dbErrors);
        AddForeignKeysErrors(database, dbErrors);
        AddTriggersErrors(database, dbErrors);
    }

    private void AddColumnsErrors(Database database, List<DbError> dbErrors)
    {
        foreach (Table table in database.Tables)
        {
            foreach (Column column in table.Columns)
            {
                if (column.DataType is null)
                {
                    string errorMessage =
$"Column '{column.Name}' in table '{table.Name}' has no data type.";

                    DbError dbError = new ColumnDbError(
                        errorMessage: errorMessage,
                        tableName: table.Name,
                        columnName: column.Name);

                    dbErrors.Add(dbError);
                }
                AddAdditionalColumnErrors(table, column, dbErrors);
            }
        }
    }
    protected virtual void AddAdditionalColumnErrors(Table table, Column column, List<DbError> dbErrors) { }

    private void AddForeignKeysErrors(Database database, List<DbError> dbErrors)
    {
        foreach (Table table in database.Tables)
        {
            foreach (ForeignKey fk in table.ForeignKeys)
            {
                if (fk is not null && !database.Tables.Any(x => x.Name == fk.ReferencedTableName))
                {
                    string errorMessage =
$"Foreign key '{fk.Name}' in table '{table.Name}' references unknown table '{fk.ReferencedTableName}'.";

                    DbError dbError = new ForeignKeyDbError(
                        errorMessage: errorMessage,
                        tableName: table.Name,
                        foreignKeyName: fk.Name);

                    dbErrors.Add(dbError);
                }
                AddAdditionalForeignKeyErrors(table, fk, dbErrors);
            }
        }
    }
    protected virtual void AddAdditionalForeignKeyErrors(Table table, ForeignKey fk, List<DbError> dbErrors) { }

    private void AddTriggersErrors(Database database, List<DbError> dbErrors)
    {
        foreach (Table table in database.Tables)
        {
            foreach (Trigger trigger in table.Triggers)
            {
                AddAdditionalTriggerErrors(table, trigger, dbErrors);
            }
        }
    }
    protected virtual void AddAdditionalTriggerErrors(Table table, Trigger trigger, List<DbError> dbErrors) { }
}
