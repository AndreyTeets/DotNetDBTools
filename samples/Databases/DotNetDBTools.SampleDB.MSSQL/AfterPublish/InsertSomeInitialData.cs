﻿using System;
using DotNetDBTools.Definition.Core;
using DotNetDBTools.Definition.MSSQL;
using DotNetDBTools.SampleDB.MSSQL.Tables;

namespace DotNetDBTools.SampleDB.MSSQL.AfterPublish
{
    internal class InsertSomeInitialData : IScript
    {
        public Guid DNDBT_OBJECT_ID => new("100D624A-01AA-4730-B86F-F991AC3ED936");
        public ScriptType Type => ScriptType.AfterPublishOnce;
        public long MinDbVersionToExecute => 0;
        public long MaxDbVersionToExecute => long.MaxValue;
        public string Text =>
$@"INSERT INTO {nameof(MyTable4).Quote()}({nameof(MyTable4.MyColumn1).Quote()})
SELECT * FROM
(
    SELECT 1
    UNION ALL
    SELECT 2
    UNION ALL
    SELECT 3
) t(Col1)
WHERE NOT EXISTS (SELECT * FROM {nameof(MyTable4).Quote()})";
    }
}
