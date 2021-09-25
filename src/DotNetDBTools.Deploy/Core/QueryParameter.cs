using System.Data;

namespace DotNetDBTools.Deploy.Core
{
    public class QueryParameter
    {
        public string Name { get; private set; }
        public object Value { get; private set; }
        public DbType Type { get; private set; }

        public QueryParameter(string name, object value, DbType type)
        {
            Name = name;
            Value = value;
            Type = type;
        }
    }
}
