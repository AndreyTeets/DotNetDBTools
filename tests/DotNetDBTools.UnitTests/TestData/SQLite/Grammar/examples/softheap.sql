-- ===softheap1.test===
PRAGMA auto_vacuum=1;
CREATE TABLE t1(x);
INSERT INTO t1 VALUES(hex(randomblob(1000)));
BEGIN;

CREATE TABLE t2 AS SELECT * FROM t1;

ROLLBACK;

PRAGMA integrity_check;