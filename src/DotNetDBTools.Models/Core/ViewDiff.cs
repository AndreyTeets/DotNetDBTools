namespace DotNetDBTools.Models.Core
{
    public abstract class ViewDiff
    {
        public ViewInfo NewView { get; set; }
        public ViewInfo OldView { get; set; }
    }
}
