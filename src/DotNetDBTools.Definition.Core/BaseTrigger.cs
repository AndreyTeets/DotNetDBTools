using System;

namespace DotNetDBTools.Definition.Core
{
    public abstract class BaseTrigger : IDbObject
    {
        public Guid ID => throw new NotImplementedException();
        public string Code { get; set; }
    }
}
