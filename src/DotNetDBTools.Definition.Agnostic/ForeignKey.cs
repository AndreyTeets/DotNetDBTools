namespace DotNetDBTools.Definition.Agnostic
{
    public class ForeignKey : BaseForeignKey
    {
        public ForeignKeyActions OnUpdate { get; set; }
        public ForeignKeyActions OnDelete { get; set; }
    }
}
