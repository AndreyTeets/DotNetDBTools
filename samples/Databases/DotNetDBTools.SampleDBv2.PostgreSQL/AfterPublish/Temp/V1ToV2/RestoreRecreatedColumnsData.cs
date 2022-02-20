using System;
using DotNetDBTools.Definition.Core;
using DotNetDBTools.Definition.PostgreSQL;

namespace DotNetDBTools.SampleDB.PostgreSQL.AfterPublish
{
    internal class RestoreRecreatedColumnsData : IScript
    {
        public Guid ID => new("8CCAF36E-E587-466E-86F7-45C0061AE521");
        public ScriptType Type => ScriptType.AfterPublishOnce;
        public long MinDbVersionToExecute => 1;
        public long MaxDbVersionToExecute => 1;
        public string Code => $"AfterPublish.Temp.V1ToV2.{nameof(RestoreRecreatedColumnsData)}.sql".AsSqlResource();
    }
}
