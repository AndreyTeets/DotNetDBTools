namespace DotNetDBTools.Definition.BaseDbTypes
{
    public abstract class BaseStringDbType : IDbType
    {
        public bool IsUnicode { get; set; } = false;
        public int Length { get; set; } = 50;
    }
}
