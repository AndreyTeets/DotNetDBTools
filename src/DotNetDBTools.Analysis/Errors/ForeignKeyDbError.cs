namespace DotNetDBTools.Analysis.Errors;

public class ForeignKeyDbError : DbError
{
    public string TableName { get; private set; }
    public string ForeignKeyName { get; private set; }

    public ForeignKeyDbError(
        string errorMessage,
        string tableName,
        string foreignKeyName)
    {
        ErrorMessage = errorMessage;
        TableName = tableName;
        ForeignKeyName = foreignKeyName;
    }
}
