-- ===walcksum.test===
PRAGMA page_size = 1024;
PRAGMA auto_vacuum = 0;
PRAGMA synchronous = NORMAL;
CREATE TABLE t1(a PRIMARY KEY, b);
INSERT INTO t1 VALUES(1,  'one');
INSERT INTO t1 VALUES(2,  'two');
INSERT INTO t1 VALUES(3,  'three');
INSERT INTO t1 VALUES(5,  'five');
PRAGMA journal_mode = WAL;
INSERT INTO t1 VALUES(8,  'eight');
INSERT INTO t1 VALUES(13, 'thirteen');
INSERT INTO t1 VALUES(21, 'twentyone');

PRAGMA integrity_check;
SELECT count(*) FROM t1;

PRAGMA synchronous = NORMAL;
PRAGMA page_size = 1024;
CREATE TABLE t1(a, b);
INSERT INTO t1 VALUES(1, randomblob(300));
INSERT INTO t1 VALUES(2, randomblob(300));
PRAGMA journal_mode = WAL;
INSERT INTO t1 VALUES(3, randomblob(300));

SELECT a FROM t1;

SELECT a FROM t1;

SELECT a FROM t1;

PRAGMA synchronous = NORMAL;
INSERT INTO t1 VALUES(34, 'thirtyfour');

PRAGMA integrity_check;
SELECT a FROM t1;

PRAGMA synchronous = NORMAL;
INSERT INTO t1 VALUES(55, 'fiftyfive');

PRAGMA integrity_check;
SELECT a FROM t1;

PRAGMA wal_checkpoint;
INSERT INTO t1 VALUES(89, 'eightynine');

PRAGMA integrity_check;
SELECT a FROM t1;

PRAGMA synchronous = NORMAL;
PRAGMA page_size = 1024;
PRAGMA journal_mode = WAL;
PRAGMA cache_size = 10;
CREATE TABLE t1(x PRIMARY KEY);
PRAGMA wal_checkpoint;
INSERT INTO t1 VALUES(randomblob(800));
BEGIN;
INSERT INTO t1 SELECT randomblob(800) FROM t1;   /*   2 */
INSERT INTO t1 SELECT randomblob(800) FROM t1;   /*   4 */
INSERT INTO t1 SELECT randomblob(800) FROM t1;   /*   8 */
INSERT INTO t1 SELECT randomblob(800) FROM t1;   /*  16 */
SAVEPOINT one;
INSERT INTO t1 SELECT randomblob(800) FROM t1;   /*  32 */
INSERT INTO t1 SELECT randomblob(800) FROM t1;   /*  64 */
INSERT INTO t1 SELECT randomblob(800) FROM t1;   /* 128 */
INSERT INTO t1 SELECT randomblob(800) FROM t1;   /* 256 */
ROLLBACK TO one;
INSERT INTO t1 SELECT randomblob(800) FROM t1;   /*  32 */
INSERT INTO t1 SELECT randomblob(800) FROM t1;   /*  64 */
INSERT INTO t1 SELECT randomblob(800) FROM t1;   /* 128 */
INSERT INTO t1 SELECT randomblob(800) FROM t1;   /* 256 */
COMMIT;