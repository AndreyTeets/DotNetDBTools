DO $Block$
BEGIN

IF EXISTS (SELECT TRUE FROM "information_schema"."tables" WHERE "table_name" = '_MyTable2')
THEN
    UPDATE "MyTable2" SET
        "MyColumn2" = "t"."MyColumn2"
    FROM "_MyTable2" AS "t"
    WHERE "MyTable2"."MyColumn1NewName" = "t"."MyColumn1";

    DROP TABLE "_MyTable2";
END IF;

END;
$Block$