﻿using System;
using DotNetDBTools.Definition.Agnostic;
using DotNetDBTools.Definition.Core;

namespace DotNetDBTools.SampleDB.Agnostic.AfterPublish
{
    internal class RestoreRecreatedColumnsData : IScript
    {
        public Guid DNDBT_OBJECT_ID => new("8CCAF36E-E587-466E-86F7-45C0061AE521");
        public ScriptType Type => ScriptType.AfterPublishOnce;
        public long MinDbVersionToExecute => 1;
        public long MaxDbVersionToExecute => 1;
        public Func<DbmsKind, string> Text => dk =>
            $"AfterPublish.Temp.V1ToV2.{nameof(RestoreRecreatedColumnsData)}.sql".AsSqlResource(dk);
    }
}
