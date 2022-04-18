namespace DotNetDBTools.Analysis.Errors;

public abstract class DbError
{
    public string ErrorMessage { get; protected set; }
    public override string ToString() => ErrorMessage;
}
