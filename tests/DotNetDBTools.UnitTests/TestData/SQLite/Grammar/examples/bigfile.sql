-- ===bigfile.test===
BEGIN;
CREATE TABLE t1(x);
INSERT INTO t1 VALUES('abcdefghijklmnopqrstuvwxyz');
INSERT INTO t1 SELECT rowid || ' ' || x FROM t1;
INSERT INTO t1 SELECT rowid || ' ' || x FROM t1;
INSERT INTO t1 SELECT rowid || ' ' || x FROM t1;
INSERT INTO t1 SELECT rowid || ' ' || x FROM t1;
INSERT INTO t1 SELECT rowid || ' ' || x FROM t1;
INSERT INTO t1 SELECT rowid || ' ' || x FROM t1;
INSERT INTO t1 SELECT rowid || ' ' || x FROM t1;
COMMIT;

SELECT md5sum(x) FROM t2;

SELECT md5sum(x) FROM t1;

SELECT md5sum(x) FROM t2;

SELECT md5sum(x) FROM t3;

CREATE TABLE t4 AS SELECT * FROM t1;
SELECT md5sum(x) FROM t4;

SELECT md5sum(x) FROM t1;

SELECT md5sum(x) FROM t2;

SELECT md5sum(x) FROM t3;

SELECT md5sum(x) FROM t1;

SELECT md5sum(x) FROM t1;

CREATE TABLE t2 AS SELECT * FROM t1;
SELECT md5sum(x) FROM t2;

SELECT md5sum(x) FROM t1;

SELECT md5sum(x) FROM t1;

SELECT md5sum(x) FROM t2;

CREATE TABLE t3 AS SELECT * FROM t1;
SELECT md5sum(x) FROM t3;

SELECT md5sum(x) FROM t1;