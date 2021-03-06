using System;
using DotNetDBTools.Definition.Core;
using DotNetDBTools.Definition.PostgreSQL;

namespace DotNetDBTools.SampleDB.PostgreSQL.AfterPublish.Init
{
    internal class InsertSomeInitialData : IScript
    {
        public Guid ID => new("100D624A-01AA-4730-B86F-F991AC3ED936");
        public ScriptType Type => ScriptType.AfterPublishOnce;
        public long MinDbVersionToExecute => 0;
        public long MaxDbVersionToExecute => long.MaxValue;
        public string Code => $"AfterPublish.Init.{nameof(InsertSomeInitialData)}.sql".AsSqlResource();
    }
}
