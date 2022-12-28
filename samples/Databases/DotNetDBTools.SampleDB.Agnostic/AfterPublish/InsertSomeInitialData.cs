using System;
using DotNetDBTools.Definition.Agnostic;
using DotNetDBTools.Definition.Core;
using DotNetDBTools.SampleDB.Agnostic.Tables;

namespace DotNetDBTools.SampleDB.Agnostic.AfterPublish
{
    internal class InsertSomeInitialData : IScript
    {
        public Guid DNDBT_OBJECT_ID => new("100D624A-01AA-4730-B86F-F991AC3ED936");
        public ScriptType Type => ScriptType.AfterPublishOnce;
        public long MinDbVersionToExecute => 0;
        public long MaxDbVersionToExecute => long.MaxValue;
        public Func<DbmsKind, string> Code => dk =>
$@"INSERT INTO {nameof(MyTable4).Quote(dk)}({nameof(MyTable4.MyColumn1).Quote(dk)})
SELECT * FROM
(
    SELECT 1
    UNION ALL
    SELECT 2
    UNION ALL
    SELECT 3
) t{ColsDef(dk)}
WHERE NOT EXISTS (SELECT * FROM {nameof(MyTable4).Quote(dk)})";

        private string ColsDef(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.MSSQL)
                return "(Col1)";
            else
                return "";
        }
    }
}
