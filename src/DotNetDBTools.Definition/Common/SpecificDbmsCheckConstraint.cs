using DotNetDBTools.Definition.Core;

namespace DotNetDBTools.Definition.Common
{
    public class SpecificDbmsCheckConstraint : BaseCheckConstraint
    {
        public SpecificDbmsCheckConstraint(string id) : base(id) { }

        public string Code { get; set; }
    }
}
