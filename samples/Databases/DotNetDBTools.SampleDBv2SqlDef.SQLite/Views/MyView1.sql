--ID:#{E2569AAE-D5DA-4A77-B3CD-51ADBDB272D9}#
CREATE VIEW MyView1 AS
SELECT
    t1.MyColumn1,
    t1.MyColumn4,
    t2.MyColumn2
FROM MyTable1NewName t1
LEFT JOIN MyTable2 t2
    ON t2.MyColumn1NewName = t1.MyColumn1;