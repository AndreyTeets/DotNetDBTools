namespace DotNetDBTools.Definition.BaseDbTypes
{
    public abstract class BaseByteDbType : IDbType
    {
        public int Length { get; set; } = 50;
    }
}
