-- ===shortread1.test===
CREATE TABLE t1(a TEXT);
BEGIN;
INSERT INTO t1 VALUES(hex(randomblob(5000)));
INSERT INTO t1 VALUES(hex(randomblob(100)));
PRAGMA freelist_count;

DELETE FROM t1 WHERE rowid=1;
PRAGMA freelist_count;

INSERT INTO t1 VALUES(hex(randomblob(5000)));
PRAGMA freelist_count;

COMMIT;
SELECT count(*) FROM t1;