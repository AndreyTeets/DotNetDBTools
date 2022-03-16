using System;
using DotNetDBTools.Definition.Agnostic;
using DotNetDBTools.Definition.Core;

namespace DotNetDBTools.SampleDB.Agnostic.AfterPublish.Init
{
    internal class InsertSomeInitialData : IScript
    {
        public Guid ID => new("100D624A-01AA-4730-B86F-F991AC3ED936");
        public ScriptType Type => ScriptType.AfterPublishOnce;
        public long MinDbVersionToExecute => 0;
        public long MaxDbVersionToExecute => long.MaxValue;
        public Func<DbmsKind, string> Code => dk =>
            $"AfterPublish.Init.{nameof(InsertSomeInitialData)}.sql".AsSqlResource(dk);
    }
}
