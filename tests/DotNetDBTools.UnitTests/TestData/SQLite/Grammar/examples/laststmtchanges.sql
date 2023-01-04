-- ===laststmtchanges.test===
select changes();

update t0 set x=3 where x=4;

CREATE TABLE t3(a, b, c);
INSERT INTO t3 VALUES(1, 2, 3);
INSERT INTO t3 VALUES(4, 5, 6);

BEGIN;
DELETE FROM t3;
SELECT changes();

ROLLBACK;
BEGIN;
DELETE FROM t3 WHERE a IS NOT NULL;
SELECT changes();

ROLLBACK;
CREATE INDEX t3_i1 ON t3(a);
BEGIN;
DELETE FROM t3;
SELECT changes();

ROLLBACK;

SELECT total_changes();

SELECT total_changes();
DELETE FROM t3;
SELECT total_changes();