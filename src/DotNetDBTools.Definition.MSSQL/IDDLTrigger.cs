using DotNetDBTools.Definition.Core;

namespace DotNetDBTools.Definition.MSSQL
{
    public interface IDDLTrigger : IDbObject
    {
        public string Code { get; }
    }
}
