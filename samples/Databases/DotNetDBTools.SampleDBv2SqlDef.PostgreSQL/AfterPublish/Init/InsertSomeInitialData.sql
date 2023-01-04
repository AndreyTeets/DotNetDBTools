--ScriptID:#{100D624A-01AA-4730-B86F-F991AC3ED936}#
--ScriptName:#{InsertSomeInitialData}#
--ScriptType:#{AfterPublishOnce}#
--ScriptMinDbVersionToExecute:#{0}#
--ScriptMaxDbVersionToExecute:#{9223372036854775807}#
INSERT INTO "MyTable4"("MyColumn1")
SELECT * FROM
(
    SELECT 1
    UNION ALL
    SELECT 2
    UNION ALL
    SELECT 3
) t
WHERE NOT EXISTS (SELECT * FROM "MyTable4")