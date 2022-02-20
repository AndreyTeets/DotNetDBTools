using System;
using DotNetDBTools.Definition.Core;
using DotNetDBTools.Definition.Agnostic;

namespace DotNetDBTools.SampleDB.Agnostic.AfterPublish
{
    internal class RestoreRecreatedColumnsData : IScript
    {
        public Guid ID => new("8CCAF36E-E587-466E-86F7-45C0061AE521");
        public ScriptType Type => ScriptType.AfterPublishOnce;
        public long MinDbVersionToExecute => 1;
        public long MaxDbVersionToExecute => 1;
        public Func<DbmsKind, string> Code => dk =>
            $"AfterPublish.Temp.V1ToV2.{nameof(RestoreRecreatedColumnsData)}.sql".AsSqlResource(dk);
    }
}
