using System;

namespace DotNetDBTools.Definition
{
    public abstract class BaseTrigger : IDbObject
    {
        public Guid ID => throw new NotImplementedException();
        public string Code { get; set; }
    }
}
