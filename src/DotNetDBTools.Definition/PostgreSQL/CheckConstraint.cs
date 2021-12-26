using DotNetDBTools.Definition.Core;

namespace DotNetDBTools.Definition.PostgreSQL
{
    public class CheckConstraint : BaseCheckConstraint
    {
        public CheckConstraint(string id) : base(id) { }

        public string Code { get; set; }
    }
}
