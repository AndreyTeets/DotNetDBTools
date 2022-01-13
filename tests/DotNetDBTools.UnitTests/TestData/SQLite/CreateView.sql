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
INNER JOIN MyView3 v3 on v3.`Col1` = t2.Col1
LEFT JOIN pragma_index_list(t2.Col2) ON TRUE