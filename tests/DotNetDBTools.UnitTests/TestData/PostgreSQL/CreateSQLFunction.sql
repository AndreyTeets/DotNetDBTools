--ID:#{FE72177E-52D8-48E3-975E-408AF5A1A44B}#
CREATE FUNCTION "TR_MyTable2_MyTrigger1_Handler"()
RETURNS void
LANGUAGE SQL
AS
'
INSERT INTO "MyTable1"
SELECT *
from MyView2
left join lateral (select * from "MyFunc3"(MyView2.col1)) f3 on TRUE;
';