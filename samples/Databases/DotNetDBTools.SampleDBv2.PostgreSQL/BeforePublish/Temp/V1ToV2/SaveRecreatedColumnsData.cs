﻿using System;
using DotNetDBTools.Definition.Core;
using DotNetDBTools.Definition.PostgreSQL;

namespace DotNetDBTools.SampleDB.PostgreSQL.BeforePublish
{
    internal class SaveRecreatedColumnsData : IScript
    {
        public Guid ID => new("7F72F0DF-4EDA-4063-99D8-99C1F37819D2");
        public ScriptType Type => ScriptType.BeforePublishOnce;
        public long MinDbVersionToExecute => 1;
        public long MaxDbVersionToExecute => 1;
        public string Code => $"BeforePublish.Temp.V1ToV2.{nameof(SaveRecreatedColumnsData)}.sql".AsSqlResource();
    }
}