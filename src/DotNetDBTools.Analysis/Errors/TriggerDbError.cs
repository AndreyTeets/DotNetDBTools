namespace DotNetDBTools.Analysis.Errors;

public class TriggerDbError : DbError
{
    public string TableName { get; private set; }
    public string TriggerName { get; private set; }

    public TriggerDbError(
        string errorMessage,
        string tableName,
        string triggerName)
    {
        ErrorMessage = errorMessage;
        TableName = tableName;
        TriggerName = triggerName;
    }
}
