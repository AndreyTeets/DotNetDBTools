CREATE FUNCTION "TR_MyTable2_MyTrigger1_Handler"()
RETURNS TRIGGER
LANGUAGE PLPGSQL
AS
$FuncBody$
DECLARE
    i INT = 4;
BEGIN
    --comm--ent$NotFuncBody$;
    insert INTO "MyTable1"("MyColumn1")
    VALUES(NEW."MyColumn1" || '$NotFuncBody$' || '$$');

    i = (SELECT "MyFunc2"(44));
    i = (select c1 FROM "MyView3" LEFT JOIN LATERAL (SELECT * from "MyTable4") t2 ON true INNER JOIN MyView5 ON TRUE);

    insert INTO "MyTable6"("MyColumn1")
    SELECT "MyColumn1" from MyView7;

    insert INTO "MyTable8"("MyColumn1")
    SELECT "MyColumn1" from (SELECT MyFunc9(44)) t;

    RETURN NULL;
END;
$FuncBody$;