using System;
using DotNetDBTools.Definition.Agnostic;
using DotNetDBTools.Definition.Core;

namespace DotNetDBTools.SampleDB.Agnostic.BeforePublish
{
    internal class SaveRecreatedColumnsData : IScript
    {
        public Guid DNDBT_OBJECT_ID => new("7F72F0DF-4EDA-4063-99D8-99C1F37819D2");
        public ScriptType Type => ScriptType.BeforePublishOnce;
        public long MinDbVersionToExecute => 1;
        public long MaxDbVersionToExecute => 1;
        public Func<DbmsKind, string> Text => dk =>
            $"BeforePublish.Temp.V1ToV2.{nameof(SaveRecreatedColumnsData)}.sql".AsSqlResource(dk);
    }
}
