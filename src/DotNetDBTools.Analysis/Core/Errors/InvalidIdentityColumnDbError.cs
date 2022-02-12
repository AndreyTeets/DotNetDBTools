namespace DotNetDBTools.Analysis.Core.Errors;

public class InvalidIdentityColumnDbError : DbError
{
    public string TableName { get; private set; }
    public string IdentityColumnName { get; private set; }

    public InvalidIdentityColumnDbError(
        string errorMessage,
        string tableName,
        string identityColumnName)
    {
        ErrorMessage = errorMessage;
        TableName = tableName;
        IdentityColumnName = identityColumnName;
    }
}
