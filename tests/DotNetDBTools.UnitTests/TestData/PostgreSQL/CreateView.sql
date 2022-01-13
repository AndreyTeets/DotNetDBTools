create VIEW "MyView1" AS
SELECT
    t2."Col1" AS c1,
    t2."Col2" || ' ' || v3.col3 AS c2,
    v3.Col4,
    CASE
        WHEN f4."Col5" THEN 'some val'
        ELSE ''
    END AS c4
FROM "MyTable2" t2
INNER JOIN MyView3 v3 USING ("Col2")
LEFT JOIN LATERAL (SELECT * FROM "MyFunc4"(v3.Col2)) f4 ON TRUE;