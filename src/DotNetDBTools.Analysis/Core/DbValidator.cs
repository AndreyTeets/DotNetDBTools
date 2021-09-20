using System.Linq;
using DotNetDBTools.Analysis.Core.Errors;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Analysis.Core
{
    public static class DbValidator
    {
        public static bool HasNoBadTables(IDatabaseInfo<ITableInfo<IColumnInfo>> database, out IDbError dbError)
        {
            dbError = null;
            foreach (ITableInfo<IColumnInfo> table in database.Tables)
            {
                foreach (IForeignKeyInfo fki in table.ForeignKeys)
                {
                    if (fki is not null && !database.Tables.Any(x => x.Name == fki.ForeignTableName))
                    {
                        string errorMessage =
$"Couldn't find table '{fki.ForeignTableName}' referenced by foreign key '{fki.Name}' in table '{table.Name}'";

                        dbError = new InvalidFKDbError(
                            errorMessage: errorMessage,
                            tableName: table.Name,
                            foreignKeyName: fki.Name,
                            referencedTableName: fki.ForeignTableName);

                        return false;
                    }
                }
            }
            return true;
        }

        public static bool ForeignKeyReferencesAreValid(IDatabaseInfo<ITableInfo<IColumnInfo>> database, out string error)
        {
            error = "";
            IForeignKeyInfo fki = database.Tables.First().ForeignKeys.FirstOrDefault();
            if (fki is not null && !database.Tables.Any(x => x.Name == fki.ForeignTableName))
            {
                error += $"Failed to find table {fki.ForeignTableName} referenced by foreign key";
                return false;
            }

            return true;
        }
    }
}
