INSERT INTO [MyTable4]([MyColumn1])
SELECT * FROM
(
    SELECT 1
    UNION ALL
    SELECT 2
    UNION ALL
    SELECT 3
) t
WHERE NOT EXISTS (SELECT COUNT(*) FROM [MyTable4])