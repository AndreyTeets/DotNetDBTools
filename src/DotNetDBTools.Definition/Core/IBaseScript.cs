namespace DotNetDBTools.Definition.Core;

public interface IBaseScript : IDbObject
{
    public ScriptType Type { get; }
    public long MinDbVersionToExecute { get; }
    public long MaxDbVersionToExecute { get; }
}
