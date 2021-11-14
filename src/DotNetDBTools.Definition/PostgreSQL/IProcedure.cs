using DotNetDBTools.Definition.Core;

namespace DotNetDBTools.Definition.PostgreSQL
{
    public interface IProcedure : IDbObject
    {
        public string Code { get; }
    }
}
