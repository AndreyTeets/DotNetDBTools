using DotNetDBTools.Definition.Core;

namespace DotNetDBTools.Definition.MySQL
{
    public class Column : BaseColumn
    {
        public Column(string id) : base(id) { }

        public bool DefaultIsFunction { get; set; }
    }
}
