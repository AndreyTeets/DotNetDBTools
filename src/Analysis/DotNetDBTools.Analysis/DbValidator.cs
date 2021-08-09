using System.Linq;
using DotNetDBTools.Models;

namespace DotNetDBTools.Analysis
{
    public static class DbValidator
    {
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
