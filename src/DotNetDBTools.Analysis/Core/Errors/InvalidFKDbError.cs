namespace DotNetDBTools.Analysis.Core.Errors;

public class InvalidFKDbError : DbError
{
    public string TableName { get; private set; }
    public string ForeignKeyName { get; private set; }
    public string ReferencedTableName { get; private set; }

    public InvalidFKDbError(
        string errorMessage,
        string tableName,
        string foreignKeyName,
        string referencedTableName)
    {
        ErrorMessage = errorMessage;
        TableName = tableName;
        ForeignKeyName = foreignKeyName;
        ReferencedTableName = referencedTableName;
    }
}
