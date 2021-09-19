namespace DotNetDBTools.Deploy.Shared
{
    public class QueryParameter
    {
        public string Name { get; private set; }
        public object Value { get; private set; }

        public QueryParameter(string name, object value)
        {
            Name = name;
            Value = value;
        }
    }
}
