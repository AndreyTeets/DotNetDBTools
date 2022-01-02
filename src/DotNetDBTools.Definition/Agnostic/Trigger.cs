using System;
using DotNetDBTools.Definition.Core;

namespace DotNetDBTools.Definition.Agnostic
{
    public class Trigger : BaseTrigger
    {
        public Trigger(string id) : base(id) { }

        public Func<DbmsKind, string> Code { get; set; }
    }
}
