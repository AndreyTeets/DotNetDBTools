-- ===rollback.test===
CREATE TABLE t1(a);
INSERT INTO t1 VALUES(1);
INSERT INTO t1 VALUES(2);
INSERT INTO t1 VALUES(3);
INSERT INTO t1 VALUES(4);
SELECT * FROM t1;

CREATE TABLE t3(a unique on conflict rollback);
INSERT INTO t3 SELECT a FROM t1;
BEGIN;
INSERT INTO t1 SELECT * FROM t1;

BEGIN;
INSERT INTO t3 VALUES('hello world');

COMMIT;

SELECT distinct tbl_name FROM sqlite_master;

SELECT distinct tbl_name FROM sqlite_master;