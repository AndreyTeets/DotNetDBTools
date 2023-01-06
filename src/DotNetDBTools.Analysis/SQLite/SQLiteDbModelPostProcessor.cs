using System.Text.RegularExpressions;
using DotNetDBTools.Analysis.Core;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.SQLite;

namespace DotNetDBTools.Analysis.SQLite;

internal class SQLiteDbModelPostProcessor : DbModelPostProcessor
{
    protected override void PostProcessDataTypes(Database database)
    {
        foreach (Table table in database.Tables)
        {
            foreach (Column column in table.Columns)
            {
                string dataType = Regex.Replace(column.DataType.Name, @"\s", "");
                dataType = dataType.ToUpper();

                if (dataType.Contains("INT"))
                    dataType = SQLiteDataTypeNames.INTEGER;
                else if (dataType.Contains("CHAR") || dataType.Contains("CLOB") || dataType.Contains("TEXT"))
                    dataType = SQLiteDataTypeNames.TEXT;
                else if (dataType.Contains("BLOB"))
                    dataType = SQLiteDataTypeNames.BLOB;
                else if (dataType.Contains("REAL") || dataType.Contains("FLOA") || dataType.Contains("DOUB"))
                    dataType = SQLiteDataTypeNames.REAL;
                else
                    dataType = SQLiteDataTypeNames.NUMERIC;

                column.DataType.Name = dataType;
            }
        }
    }
}
