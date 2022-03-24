namespace DotNetDBTools.Deploy;

public class Events
{
    public event EventFiredEventHandler EventFired;

    internal void InvokeEventFired(EventType eventType, object eventData = null)
    {
        EventFiredEventArgs eventArgs = new()
        {
            EventType = eventType,
            EventData = eventData,
        };
        EventFired?.Invoke(eventArgs);
    }
}

public delegate void EventFiredEventHandler(EventFiredEventArgs eventArgs);

public class EventFiredEventArgs
{
    public EventType EventType { get; set; }
    public object EventData { get; set; }
}

public enum EventType
{
    RegisterBegan,
    RegisterFinished,
    UnregisterBegan,
    UnregisterFinished,
    PublishBegan,
    PublishFinished,
    GeneratePublishScriptBegan,
    GeneratePublishScriptFinished,
    GenerateDefinitionBegan,
    GenerateDefinitionFinished,
    CreateDbModelFromDefinitionBegan,
    CreateDbModelFromDefinitionFinished,
    CreateDbModelFromDBMSBegan,
    CreateDbModelFromDBMSFinished,
    CreateDbDiffBegan,
    CreateDbDiffFinished,
    ApplyDbDiffBegan,
    ApplyDbDiffFinished,
    OpenDbConnectionBegan,
    OpenDbConnectionFinished,
    BeginTransactionBegan,
    BeginTransactionFinished,
    CommitTransactionBegan,
    CommitTransactionFinished,
    RollbackTransactionBegan,
    RollbackTransactionFinished,
    QueryBegan,
    QueryFinished,
}
