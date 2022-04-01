namespace DotNetDBTools.Analysis.Core.Errors;

public class ColumnDbError : DbError
{
    public string TableName { get; private set; }
    public string ColumnName { get; private set; }

    public ColumnDbError(
        string errorMessage,
        string tableName,
        string columnName)
    {
        ErrorMessage = errorMessage;
        TableName = tableName;
        ColumnName = columnName;
    }
}
