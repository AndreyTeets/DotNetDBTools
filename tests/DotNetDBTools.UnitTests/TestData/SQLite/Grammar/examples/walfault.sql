-- ===walfault.test===
PRAGMA main.journal_mode = WAL;

PRAGMA wal_autocheckpoint = 0;

CREATE TABLE t1(x);
BEGIN;
INSERT INTO t1 VALUES(randomblob(400));           /* 1 */
INSERT INTO t1 SELECT randomblob(400) FROM t1;    /* 2 */
INSERT INTO t1 SELECT randomblob(400) FROM t1;    /* 4 */
INSERT INTO t1 SELECT randomblob(400) FROM t1;    /* 8 */
INSERT INTO t1 SELECT randomblob(400) FROM t1;    /* 16 */
INSERT INTO t1 SELECT randomblob(400) FROM t1;    /* 32 */
INSERT INTO t1 SELECT randomblob(400) FROM t1;    /* 64 */
INSERT INTO t1 SELECT randomblob(400) FROM t1;    /* 128 */
INSERT INTO t1 SELECT randomblob(400) FROM t1;    /* 256 */
INSERT INTO t1 SELECT randomblob(400) FROM t1;    /* 512 */
INSERT INTO t1 SELECT randomblob(400) FROM t1;    /* 1024 */
INSERT INTO t1 SELECT randomblob(400) FROM t1;    /* 2048 */
INSERT INTO t1 SELECT randomblob(400) FROM t1;    /* 4096 */
INSERT INTO t1 SELECT randomblob(400) FROM t1;    /* 8192 */
INSERT INTO t1 SELECT randomblob(400) FROM t1;    /* 16384 */
COMMIT;
SELECT count(*) FROM t1;

PRAGMA page_size = 512;
PRAGMA journal_mode = WAL;
PRAGMA wal_autocheckpoint = 0;
CREATE TABLE t1(x);
BEGIN;
INSERT INTO t1 VALUES(randomblob(400));           /* 1 */
INSERT INTO t1 SELECT randomblob(400) FROM t1;    /* 2 */
INSERT INTO t1 SELECT randomblob(400) FROM t1;    /* 4 */
INSERT INTO t1 SELECT randomblob(400) FROM t1;    /* 8 */
INSERT INTO t1 SELECT randomblob(400) FROM t1;    /* 16 */
INSERT INTO t1 SELECT randomblob(400) FROM t1;    /* 32 */
INSERT INTO t1 SELECT randomblob(400) FROM t1;    /* 64 */
INSERT INTO t1 SELECT randomblob(400) FROM t1;    /* 128 */
INSERT INTO t1 SELECT randomblob(400) FROM t1;    /* 256 */
INSERT INTO t1 SELECT randomblob(400) FROM t1;    /* 512 */
INSERT INTO t1 SELECT randomblob(400) FROM t1;    /* 1024 */
INSERT INTO t1 SELECT randomblob(400) FROM t1;    /* 2048 */
INSERT INTO t1 SELECT randomblob(400) FROM t1;    /* 4096 */
INSERT INTO t1 SELECT randomblob(400) FROM t1;    /* 8192 */
INSERT INTO t1 SELECT randomblob(400) FROM t1;    /* 16384 */
COMMIT;

SELECT count(*) FROM t1;

PRAGMA page_size = 512;
PRAGMA journal_mode = WAL;
PRAGMA wal_autocheckpoint = 0;
CREATE TABLE t1(x);
BEGIN;
INSERT INTO t1 VALUES(randomblob(400));           /* 1 */
INSERT INTO t1 SELECT randomblob(400) FROM t1;    /* 2 */
INSERT INTO t1 SELECT randomblob(400) FROM t1;    /* 4 */
COMMIT;

SELECT count(*) FROM t1;

PRAGMA journal_mode = WAL;
CREATE TABLE abc(a PRIMARY KEY);
INSERT INTO abc VALUES(randomblob(900));

PRAGMA cache_size = 10;

BEGIN;
INSERT INTO abc SELECT randomblob(900) FROM abc;    /* 1 */
ROLLBACK;
SELECT count(*) FROM abc;

PRAGMA journal_mode = WAL;
CREATE TABLE abc(a PRIMARY KEY);
INSERT INTO abc VALUES(randomblob(900));

PRAGMA journal_mode = WAL;
BEGIN;
CREATE TABLE x(y, z, UNIQUE(y, z));
INSERT INTO x VALUES(randomblob(100), randomblob(100));
COMMIT;
PRAGMA wal_checkpoint;
INSERT INTO x SELECT randomblob(100), randomblob(100) FROM x;
INSERT INTO x SELECT randomblob(100), randomblob(100) FROM x;
INSERT INTO x SELECT randomblob(100), randomblob(100) FROM x;

PRAGMA cache_size = 10;

BEGIN;
INSERT INTO abc SELECT randomblob(900) FROM abc;    /* 1 */
SAVEPOINT spoint;
INSERT INTO abc SELECT randomblob(900) FROM abc;    /* 2 */
INSERT INTO abc SELECT randomblob(900) FROM abc;    /* 4 */
INSERT INTO abc SELECT randomblob(900) FROM abc;    /* 8 */
ROLLBACK TO spoint;
COMMIT;
SELECT count(*) FROM abc;

ROLLBACK TO spoint;

COMMIT;

PRAGMA journal_mode = WAL;
PRAGMA wal_autocheckpoint = 0;
CREATE TABLE z(zz INTEGER PRIMARY KEY, zzz BLOB);
CREATE INDEX zzzz ON z(zzz);
INSERT INTO z VALUES(NULL, randomblob(800));
INSERT INTO z VALUES(NULL, randomblob(800));
INSERT INTO z SELECT NULL, randomblob(800) FROM z;
INSERT INTO z SELECT NULL, randomblob(800) FROM z;
INSERT INTO z SELECT NULL, randomblob(800) FROM z;
INSERT INTO z SELECT NULL, randomblob(800) FROM z;
INSERT INTO z SELECT NULL, randomblob(800) FROM z;

PRAGMA cache_size = 10;
BEGIN;
UPDATE z SET zzz = randomblob(799);

INSERT INTO z VALUES(NULL, NULL);

ROLLBACK;

SELECT count(*), sum(length(zzz)) FROM z;

PRAGMA journal_mode = WAL;
PRAGMA wal_autocheckpoint = 0;
BEGIN;
CREATE TABLE abc(a PRIMARY KEY);
INSERT INTO abc VALUES(randomblob(1500));
INSERT INTO abc VALUES(randomblob(1500));
INSERT INTO abc SELECT randomblob(1500) FROM abc;   INSERT INTO abc SELECT randomblob(1500) FROM abc;   INSERT INTO abc SELECT randomblob(1500) FROM abc;   INSERT INTO abc SELECT randomblob(1500) FROM abc;   INSERT INTO abc SELECT randomblob(1500) FROM abc;   INSERT INTO abc SELECT randomblob(1500) FROM abc;   INSERT INTO abc SELECT randomblob(1500) FROM abc;   INSERT INTO abc SELECT randomblob(1500) FROM abc;   INSERT INTO abc SELECT randomblob(1500) FROM abc;   INSERT INTO abc SELECT randomblob(1500) FROM abc;   INSERT INTO abc SELECT randomblob(1500) FROM abc;   COMMIT;

SELECT count(*) FROM x;

SELECT count(*) FROM abc;

PRAGMA journal_mode = WAL;
PRAGMA wal_autocheckpoint = 0;
BEGIN;
CREATE TABLE abc(a PRIMARY KEY);
INSERT INTO abc VALUES(randomblob(1500));
INSERT INTO abc VALUES(randomblob(1500));
COMMIT;

SELECT * FROM sqlite_master;

PRAGMA journal_mode = WAL;
PRAGMA wal_autocheckpoint = 0;
BEGIN;
CREATE TABLE abc(a PRIMARY KEY);
INSERT INTO abc VALUES(randomblob(1500));
INSERT INTO abc VALUES(randomblob(1500));
COMMIT;

PRAGMA locking_mode = exclusive;

SELECT count(*) FROM abc;

PRAGMA locking_mode = exclusive;

PRAGMA journal_mode = delete;

BEGIN;
CREATE TABLE abc(a PRIMARY KEY);
INSERT INTO abc VALUES(randomblob(1500));
INSERT INTO abc VALUES(randomblob(1500));
COMMIT;

PRAGMA locking_mode = exclusive;
PRAGMA journal_mode = WAL;
INSERT INTO abc VALUES(randomblob(1500));

SELECT count(*) FROM x;

SELECT count(*) FROM abc;

PRAGMA journal_mode = WAL;
BEGIN;
CREATE TABLE abc(a PRIMARY KEY);
INSERT INTO abc VALUES(randomblob(1500));
INSERT INTO abc VALUES(randomblob(1500));
COMMIT;

PRAGMA wal_checkpoint;
INSERT INTO abc VALUES(randomblob(1500));

SELECT count(*) FROM abc;

SELECT count(*) FROM x;

PRAGMA auto_vacuum = 1;
PRAGMA journal_mode = WAL;
CREATE TABLE abc(a PRIMARY KEY);
INSERT INTO abc VALUES(randomblob(1500));

DELETE FROM abc;
PRAGMA wal_checkpoint;

PRAGMA journal_mode = WAL;
CREATE TABLE t1(a PRIMARY KEY, b);
INSERT INTO t1 VALUES('a', 'b');
PRAGMA wal_checkpoint;
SELECT * FROM t1;

PRAGMA page_size = 512;
PRAGMA journal_mode = WAL;