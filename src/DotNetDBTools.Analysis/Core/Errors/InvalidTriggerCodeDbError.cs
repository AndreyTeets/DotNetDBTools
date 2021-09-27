namespace DotNetDBTools.Analysis.Core.Errors
{
    public class InvalidTriggerCodeDbError : DbError
    {
        public string TableName { get; private set; }
        public string TriggerName { get; private set; }

        public InvalidTriggerCodeDbError(
            string errorMessage,
            string tableName,
            string triggerName)
        {
            ErrorMessage = errorMessage;
            TableName = tableName;
            TriggerName = triggerName;
        }
    }
}
