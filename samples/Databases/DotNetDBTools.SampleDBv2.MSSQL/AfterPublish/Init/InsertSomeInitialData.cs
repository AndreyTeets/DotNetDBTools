using System;
using DotNetDBTools.Definition.Core;
using DotNetDBTools.Definition.MSSQL;

namespace DotNetDBTools.SampleDB.MSSQL.AfterPublish.Init
{
    internal class InsertSomeInitialData : IScript
    {
        public Guid DNDBT_OBJECT_ID => new("100D624A-01AA-4730-B86F-F991AC3ED936");
        public ScriptType Type => ScriptType.AfterPublishOnce;
        public long MinDbVersionToExecute => 0;
        public long MaxDbVersionToExecute => long.MaxValue;
        public string Text => $"AfterPublish.Init.{nameof(InsertSomeInitialData)}.sql".AsSqlResource();
    }
}
