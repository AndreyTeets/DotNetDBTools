using System.Collections.Generic;
using System.Text.RegularExpressions;
using DotNetDBTools.Analysis.Core;
using DotNetDBTools.Analysis.Errors;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Analysis.PostgreSQL;

internal class PostgreSQLDbValidator : DbValidator
{
    protected override DbAnalysisContext BuildCurrentAnalysisContext(Database database)
    {
        return new DbAnalysisContext()
        {
            UserDefinedTypesNames = PostgreSQLHelperMethods.GetUserDefinedTypesName(database),
        };
    }

    protected override bool DataTypeIsValid(DataType dataType, out string dataTypeErrorMessage)
    {
        if (DataTypeIsUnknown(dataType))
            dataTypeErrorMessage = $"Unknown data type '{dataType.Name}'.";
        else
            dataTypeErrorMessage = null;

        return dataTypeErrorMessage is null;

        bool DataTypeIsUnknown(DataType dataType)
        {
            string normalizedTypeNameWithoutArray = PostgreSQLHelperMethods.GetNormalizedTypeNameWithoutArray(
                dataType.Name, out string standardSqlTypeNameBase, out string _);

            return !CurrentAnalysisContext.UserDefinedTypesNames.Contains(normalizedTypeNameWithoutArray)
                && standardSqlTypeNameBase == null;
        }
    }

    protected override void AddAdditionalTriggerErrors(Table table, Trigger trigger, List<DbError> dbErrors)
    {
        if (!Regex.IsMatch(trigger.CreateStatement.Code, $@"CREATE TRIGGER ""?{trigger.Name}""?"))
        {
            string errorMessage =
$"Trigger '{trigger.Name}' in table '{table.Name}' has different name in it's creation code.";

            DbError dbError = new TriggerDbError(
                errorMessage: errorMessage,
                tableName: table.Name,
                triggerName: trigger.Name);

            dbErrors.Add(dbError);
        }
    }
}
