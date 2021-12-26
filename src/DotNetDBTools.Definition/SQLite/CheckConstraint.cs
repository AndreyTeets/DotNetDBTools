using DotNetDBTools.Definition.Core;

namespace DotNetDBTools.Definition.SQLite
{
    public class CheckConstraint : BaseCheckConstraint
    {
        public CheckConstraint(string id) : base(id) { }

        public string Code { get; set; }
    }
}
