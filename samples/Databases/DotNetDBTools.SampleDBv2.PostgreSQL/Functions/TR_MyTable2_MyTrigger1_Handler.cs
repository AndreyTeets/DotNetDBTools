using System;
using DotNetDBTools.Definition.PostgreSQL;
using DotNetDBTools.SampleDB.PostgreSQL;
using DotNetDBTools.SampleDB.PostgreSQL.Tables;

namespace DotNetDBTools.SampleDBv2.PostgreSQL.Functions
{
    public class TR_MyTable2_MyTrigger1_Handler : IFunction
    {
        public Guid ID => new("8EDD4469-E048-48BD-956E-A26113355F80");
        public string Code =>
$@"CREATE FUNCTION {nameof(TR_MyTable2_MyTrigger1_Handler).Quote()}()
RETURNS TRIGGER
LANGUAGE PLPGSQL
AS
$FuncBody$
BEGIN
    INSERT INTO {nameof(MyTable4).Quote()}({nameof(MyTable4.MyColumn1).Quote()})
    VALUES(NEW.{nameof(MyTable2.MyColumn1NewName).Quote()});
    RETURN NULL;
END;
$FuncBody$;";
    }
}
