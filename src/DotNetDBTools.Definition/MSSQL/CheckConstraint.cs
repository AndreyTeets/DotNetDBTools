using DotNetDBTools.Definition.Common;

namespace DotNetDBTools.Definition.MSSQL
{
    public class CheckConstraint : SpecificDbmsCheckConstraint
    {
        public CheckConstraint(string id) : base(id) { }
    }
}
