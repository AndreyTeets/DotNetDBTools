CREATE PROCEDURE "MyProcedure1"(in a INT, in b INT)
LANGUAGE SQL
AS
$ProcBody$
INSERT INTO "MyTable4"("MyColumn1") VALUES (a);
INSERT INTO "MyTable4"("MyColumn1") VALUES (b);
$ProcBody$