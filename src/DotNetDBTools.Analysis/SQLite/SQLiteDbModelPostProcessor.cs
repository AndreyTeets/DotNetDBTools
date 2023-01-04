using DotNetDBTools.Analysis.Core;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Analysis.SQLite;

internal class SQLiteDbModelPostProcessor : DbModelPostProcessor
{
    protected override void PostProcessDataTypes(Database database)
    {
        foreach (Table table in database.Tables)
        {
            foreach (Column column in table.Columns)
            {
                column.DataType.Name = column.DataType.Name.ToUpper();
            }
        }
    }
}
