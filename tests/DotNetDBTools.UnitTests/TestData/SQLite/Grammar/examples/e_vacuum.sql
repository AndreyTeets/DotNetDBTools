-- ===e_vacuum.test===
PRAGMA page_size = 1024;

PRAGMA journal_mode = delete;

PRAGMA page_size = 2048;

PRAGMA auto_vacuum = NONE;

PRAGMA page_size ; PRAGMA auto_vacuum;

PRAGMA journal_mode = wal;

PRAGMA page_size ; PRAGMA auto_vacuum;

PRAGMA page_size = 1024;

PRAGMA auto_vacuum = FULL;

PRAGMA page_size ; PRAGMA auto_vacuum;

SELECT a FROM t1;

CREATE TABLE t1(a PRIMARY KEY, b UNIQUE);
INSERT INTO t1 VALUES(1, randomblob(400));
INSERT INTO t1 SELECT a+1,  randomblob(400) FROM t1;
INSERT INTO t1 SELECT a+2,  randomblob(400) FROM t1;
INSERT INTO t1 SELECT a+4,  randomblob(400) FROM t1;
INSERT INTO t1 SELECT a+8,  randomblob(400) FROM t1;
INSERT INTO t1 SELECT a+16, randomblob(400) FROM t1;
INSERT INTO t1 SELECT a+32, randomblob(400) FROM t1;
INSERT INTO t1 SELECT a+64, randomblob(400) FROM t1;
CREATE TABLE t2(a PRIMARY KEY, b UNIQUE);
INSERT INTO t2 SELECT * FROM t1;

PRAGMA auto_vacuum;

DELETE FROM t1;
DELETE FROM t2;

DELETE FROM t1;
DELETE FROM t2;
PRAGMA incremental_vacuum;

CREATE VIRTUAL TABLE temp.stat USING dbstat;

SELECT pageno FROM stat WHERE name = 't1' ORDER BY pageno;

DROP TABLE temp.stat;

PRAGMA page_size ; PRAGMA auto_vacuum;

PRAGMA page_size = 2048;

PRAGMA auto_vacuum = NONE;

PRAGMA page_size ; PRAGMA auto_vacuum