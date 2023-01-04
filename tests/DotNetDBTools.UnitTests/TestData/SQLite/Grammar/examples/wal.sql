-- ===wal.test===
PRAGMA auto_vacuum = 0;

INSERT INTO t1 VALUES(9, 10);

SELECT * FROM t2;

SELECT * FROM t2;

SELECT count(*) FROM t2;

INSERT INTO t2 SELECT blob(400), blob(400) FROM t2;

SELECT count(*) FROM t2;

PRAGMA journal_mode = WAL;
CREATE TABLE t1(a PRIMARY KEY, b);
INSERT INTO t1 VALUES(randomblob(10), randomblob(100));
INSERT INTO t1 SELECT randomblob(10), randomblob(100) FROM t1;
INSERT INTO t1 SELECT randomblob(10), randomblob(100) FROM t1;
INSERT INTO t1 SELECT randomblob(10), randomblob(100) FROM t1;

PRAGMA auto_vacuum = 0;
PRAGMA page_size = 1024;
PRAGMA journal_mode = WAL;

CREATE TABLE t1(a, b);
INSERT INTO t1 VALUES(1, 2);

BEGIN;
INSERT INTO t1 VALUES(3, 4);

BEGIN;
SELECT * FROM t1;

SELECT * FROM t1;

COMMIT;

COMMIT;

ATTACH 'test2.db' AS aux;
PRAGMA main.auto_vacuum = 0;
PRAGMA aux.auto_vacuum = 0;
PRAGMA main.journal_mode = WAL;
PRAGMA aux.journal_mode = WAL;
PRAGMA synchronous = NORMAL;

CREATE TABLE main.t1(a, b, PRIMARY KEY(a, b));
CREATE TABLE aux.t2(a, b, PRIMARY KEY(a, b));
INSERT INTO t2 VALUES(1, randomblob(1000));
INSERT INTO t2 VALUES(2, randomblob(1000));
INSERT INTO t1 SELECT * FROM t2;

PRAGMA auto_vacuum = 0;
PRAGMA page_size = 512;
PRAGMA journal_mode = WAL;
PRAGMA synchronous = FULL;

BEGIN;
CREATE TABLE t(x);

INSERT INTO t VALUES(randomblob(400));

PRAGMA page_size = 1024;
PRAGMA auto_vacuum = 0;
PRAGMA journal_mode = WAL;
PRAGMA synchronous = OFF;
CREATE TABLE t1(a, b, UNIQUE(a, b));
INSERT INTO t1 VALUES(0, 0);
PRAGMA wal_checkpoint;
INSERT INTO t1 VALUES(1, 2);          INSERT INTO t1 VALUES(3, 4);          INSERT INTO t1 VALUES(5, 6);          -- frames 5 and 6;

SELECT * FROM t1;
PRAGMA integrity_check;

PRAGMA journal_mode = WAL;
CREATE TABLE t1(a, b);
INSERT INTO t1 VALUES(1, 2);
INSERT INTO t1 VALUES(3, 4);

BEGIN; SELECT * FROM t1;

SELECT * FROM t1;

INSERT INTO t1 VALUES(5, 6);
SELECT * FROM t1;

SELECT * FROM t1;

PRAGMA journal_mode = WAL;
CREATE TABLE t1(x);
INSERT INTO t1 VALUES(randomblob(900));
SELECT count(*) FROM t1;

PRAGMA wal_autocheckpoint = 0;
INSERT INTO t1 SELECT randomblob(900) FROM t1;       /* 2 */
INSERT INTO t1 SELECT randomblob(900) FROM t1;       /* 4 */
INSERT INTO t1 SELECT randomblob(900) FROM t1;       /* 8 */
INSERT INTO t1 SELECT randomblob(900) FROM t1;       /* 16 */
INSERT INTO t1 SELECT randomblob(900) FROM t1;       /* 32 */
INSERT INTO t1 SELECT randomblob(900) FROM t1;       /* 64 */
INSERT INTO t1 SELECT randomblob(900) FROM t1;       /* 128 */
INSERT INTO t1 SELECT randomblob(900) FROM t1;       /* 256 */
INSERT INTO t1 SELECT randomblob(900) FROM t1;       /* 512 */
INSERT INTO t1 SELECT randomblob(900) FROM t1;       /* 1024 */
INSERT INTO t1 SELECT randomblob(900) FROM t1;       /* 2048 */
INSERT INTO t1 SELECT randomblob(900) FROM t1;       /* 4096 */
INSERT INTO t1 SELECT randomblob(900) FROM t1;       /* 8192 */
INSERT INTO t1 SELECT randomblob(900) FROM t1;       /* 16384 */;

PRAGMA wal_checkpoint;
SELECT count(*) FROM t1;

SELECT count(*) FROM t1;

PRAGMA journal_mode = WAL;
CREATE TABLE t1(a, b);
INSERT INTO t1 VALUES(1, 2);
INSERT INTO t1 VALUES(3, 4);
INSERT INTO t1 VALUES(5, 6);
INSERT INTO t1 VALUES(7, 8);
INSERT INTO t1 VALUES(9, 10);
INSERT INTO t1 VALUES(11, 12);

PRAGMA cache_size = 10;
PRAGMA wal_checkpoint;
BEGIN;
SAVEPOINT s;
INSERT INTO t1 SELECT randomblob(900), randomblob(900) FROM t1;
ROLLBACK TO s;
COMMIT;
SELECT * FROM t1;

PRAGMA integrity_check;

INSERT INTO t1 VALUES(11, 12);

CREATE TABLE t1(a, b);
PRAGMA journal_mode = WAL;
INSERT INTO t1 VALUES(1, 2);
INSERT INTO t1 VALUES(3, 4);

SELECT * FROM t1;

SELECT * FROM t1;

SELECT * FROM t1;

SELECT * FROM t1;

INSERT INTO t1 VALUES(13, 14);

SELECT * FROM t1;

SELECT * FROM t1;

COMMIT; SELECT * FROM t1;

PRAGMA synchronous = normal;

BEGIN; DELETE FROM t1;

SELECT * FROM t1;

SELECT * FROM t1;

ROLLBACK;

SELECT * FROM t1;

DELETE FROM t1;
BEGIN;
INSERT INTO t1 VALUES('a', 'b');
SAVEPOINT sp;
INSERT INTO t1 VALUES('c', 'd');
SELECT * FROM t1;

ROLLBACK TO sp;
SELECT * FROM t1;

COMMIT;
SELECT * FROM t1;

SELECT * FROM t1;

PRAGMA cache_size = 10;

PRAGMA journal_mode = wal;

CREATE TABLE t2(a, b);
INSERT INTO t2 VALUES(blob(400), blob(400));
SAVEPOINT tr;
INSERT INTO t2 SELECT blob(400), blob(400) FROM t2; /*  2 */
INSERT INTO t2 SELECT blob(400), blob(400) FROM t2; /*  4 */
INSERT INTO t2 SELECT blob(400), blob(400) FROM t2; /*  8 */
INSERT INTO t2 SELECT blob(400), blob(400) FROM t2; /* 16 */
INSERT INTO t2 SELECT blob(400), blob(400) FROM t2; /* 32 */
INSERT INTO t1 SELECT blob(400), blob(400) FROM t1; /*  2 */
INSERT INTO t1 SELECT blob(400), blob(400) FROM t1; /*  4 */
INSERT INTO t1 SELECT blob(400), blob(400) FROM t1; /*  8 */
INSERT INTO t1 SELECT blob(400), blob(400) FROM t1; /* 16 */
INSERT INTO t1 SELECT blob(400), blob(400) FROM t1; /* 32 */
SELECT count(*) FROM t2;

ROLLBACK TO tr;

INSERT INTO t1 VALUES('x', 'y');
RELEASE tr;

SELECT count(*) FROM t2;

SELECT count(*) FROM t2 ; SELECT count(*) FROM t1;

PRAGMA integrity_check;

PRAGMA journal_mode = WAL;
CREATE TABLE t1(a, b);
INSERT INTO t1 VALUES('a', 'b');

SELECT * FROM t1;

PRAGMA cache_size = 10;

CREATE TABLE t2(a, b);
BEGIN;
INSERT INTO t2 VALUES(blob(400), blob(400));
SAVEPOINT tr;
INSERT INTO t2 SELECT blob(400), blob(400) FROM t2; /*  2 */
INSERT INTO t2 SELECT blob(400), blob(400) FROM t2; /*  4 */
INSERT INTO t2 SELECT blob(400), blob(400) FROM t2; /*  8 */
INSERT INTO t2 SELECT blob(400), blob(400) FROM t2; /* 16 */
INSERT INTO t2 SELECT blob(400), blob(400) FROM t2; /* 32 */
INSERT INTO t1 SELECT blob(400), blob(400) FROM t1; /*  2 */
INSERT INTO t1 SELECT blob(400), blob(400) FROM t1; /*  4 */
INSERT INTO t1 SELECT blob(400), blob(400) FROM t1; /*  8 */
INSERT INTO t1 SELECT blob(400), blob(400) FROM t1; /* 16 */
INSERT INTO t1 SELECT blob(400), blob(400) FROM t1; /* 32 */
SELECT count(*) FROM t2;

BEGIN;
CREATE TABLE t1(a, b);

ROLLBACK TO tr;

INSERT INTO t1 VALUES('x', 'y');
RELEASE tr;
COMMIT;

SELECT count(*) FROM t2 ; SELECT count(*) FROM t1;

SELECT count(*) FROM t2 ; SELECT count(*) FROM t1;

PRAGMA integrity_check;

DELETE FROM t2;
PRAGMA wal_checkpoint;
BEGIN;
INSERT INTO t2 VALUES('w', 'x');
SAVEPOINT save;
INSERT INTO t2 VALUES('y', 'z');
ROLLBACK TO save;
COMMIT;
SELECT * FROM t2;

CREATE TEMP TABLE t2(a, b);
INSERT INTO t2 VALUES(1, 2);

BEGIN;
INSERT INTO t2 VALUES(3, 4);
SELECT * FROM t2;

ROLLBACK;
SELECT * FROM t2;

CREATE TEMP TABLE t3(x UNIQUE);
BEGIN;
INSERT INTO t2 VALUES(3, 4);
INSERT INTO t3 VALUES('abc');

SELECT * FROM sqlite_master;

COMMIT;
SELECT * FROM t2;

PRAGMA page_size = 1024;
CREATE TABLE t1(a, b);
INSERT INTO t1 VALUES(1, 2);

PRAGMA wal_checkpoint;

PRAGMA auto_vacuum = 1;
PRAGMA journal_mode = wal;
PRAGMA auto_vacuum;

PRAGMA page_size = 1024;
CREATE TABLE t1(x);
INSERT INTO t1 VALUES(blob(900));
INSERT INTO t1 VALUES(blob(900));
INSERT INTO t1 SELECT blob(900) FROM t1;       /*  4 */
INSERT INTO t1 SELECT blob(900) FROM t1;       /*  8 */
INSERT INTO t1 SELECT blob(900) FROM t1;       /* 16 */
INSERT INTO t1 SELECT blob(900) FROM t1;       /* 32 */
INSERT INTO t1 SELECT blob(900) FROM t1;       /* 64 */
PRAGMA wal_checkpoint;

DELETE FROM t1 WHERE rowid<54;
PRAGMA wal_checkpoint;

CREATE TABLE t1(x PRIMARY KEY);
INSERT INTO t1 VALUES(blob(900));
INSERT INTO t1 VALUES(blob(900));
INSERT INTO t1 SELECT blob(900) FROM t1;       /*  4 */
INSERT INTO t1 SELECT blob(900) FROM t1;       /*  8 */
INSERT INTO t1 SELECT blob(900) FROM t1;       /* 16 */
INSERT INTO t1 SELECT blob(900) FROM t1;       /* 32 */
INSERT INTO t1 SELECT blob(900) FROM t1;       /* 64 */
INSERT INTO t1 SELECT blob(900) FROM t1;       /* 128 */
INSERT INTO t1 SELECT blob(900) FROM t1;       /* 256 */;

PRAGMA integrity_check;

PRAGMA integrity_check;

PRAGMA wal_checkpoint;

INSERT INTO t1 VALUES(1, 2);

PRAGMA integrity_check;

PRAGMA journal_mode = wal;
CREATE TABLE t1(a, b);
INSERT INTO t1 VALUES(1, 2);
SELECT * FROM t1;

BEGIN; INSERT INTO t1 VALUES(3, 4);

COMMIT;

INSERT INTO t1 VALUES(5, 6);

BEGIN ; SELECT * FROM t1;

COMMIT;

BEGIN;

INSERT INTO t1 VALUES(9, 10);

COMMIT;

INSERT INTO t1 VALUES(3, 4);

SELECT * FROM t1;

INSERT INTO t1 VALUES(11, 12);

PRAGMA wal_checkpoint;

SELECT * FROM t1;

SELECT * FROM t1;

PRAGMA wal_checkpoint;

PRAGMA wal_checkpoint;

SELECT * FROM t1;

INSERT INTO t1 VALUES(19, 20);

PRAGMA wal_checkpoint;

INSERT INTO t1 VALUES(5, 6);

BEGIN ; SELECT * FROM t1;

SELECT * FROM t1 ; COMMIT;

SELECT * FROM t1;

DELETE FROM t1;
INSERT INTO t1 VALUES('a', 'b');
INSERT INTO t1 VALUES('c', 'd');

PRAGMA wal_checkpoint;

PRAGMA cache_size = 10;
PRAGMA page_size = 1024;
CREATE TABLE t1(x PRIMARY KEY);

PRAGMA wal_checkpoint;

INSERT INTO t1 VALUES( blob(900) );

BEGIN;
INSERT INTO t1 SELECT blob(900) FROM t1;   INSERT INTO t1 SELECT blob(900) FROM t1;   INSERT INTO t1 SELECT blob(900) FROM t1;   INSERT INTO t1 SELECT blob(900) FROM t1;   -- 16;

SELECT count(*) FROM t1;
PRAGMA integrity_check;

INSERT INTO t1 VALUES(7, 8);

SELECT count(*) FROM t1;
PRAGMA integrity_check;

PRAGMA wal_checkpoint;

PRAGMA cache_size = 10;
BEGIN;
INSERT INTO t1 SELECT blob(900) FROM t1;   SELECT count(*) FROM t1;

SELECT count(*) FROM t1;
ROLLBACK;
SELECT count(*) FROM t1;

INSERT INTO t1 VALUES( blob(900) );
SELECT count(*) FROM t1;
PRAGMA integrity_check;

PRAGMA page_size = 1024;
CREATE TABLE t1(x, y);
CREATE TABLE t2(x, y);
INSERT INTO t1 VALUES('A', 1);

PRAGMA synchronous = normal;
UPDATE t1 SET y = 0 WHERE x = 'A';

INSERT INTO t2 VALUES('B', 1);

SELECT * FROM t2;

PRAGMA wal_checkpoint;
UPDATE t2 SET y = 2 WHERE x = 'B'; 
PRAGMA wal_checkpoint;
UPDATE t1 SET y = 1 WHERE x = 'A';
PRAGMA wal_checkpoint;
UPDATE t1 SET y = 0 WHERE x = 'A';
SELECT * FROM t2;

-- ===wal2.test===
PRAGMA journal_mode = WAL;
CREATE TABLE t1(a);

SELECT count(a), sum(a) FROM t1;

SELECT count(a), sum(a) FROM t1;

SELECT count(a), sum(a) FROM t1;

PRAGMA journal_mode = WAL;
CREATE TABLE t1(a);
INSERT INTO t1 VALUES(1);
INSERT INTO t1 VALUES(2);
INSERT INTO t1 VALUES(3);
INSERT INTO t1 VALUES(4);

SELECT count(a), sum(a) FROM t1;

SELECT count(a), sum(a) FROM t1;

PRAGMA journal_mode = WAL;
CREATE TABLE data(x);
INSERT INTO data VALUES('need xShmOpen to see this');
PRAGMA wal_checkpoint;

PRAGMA journal_mode = WAL;
CREATE TABLE x(y);
INSERT INTO x VALUES(1);

PRAGMA wal_checkpoint;

Pragma Journal_Mode = Wal;

INSERT INTO t1 VALUES(1);
INSERT INTO t1 VALUES(2);
INSERT INTO t1 VALUES(3);
INSERT INTO t1 VALUES(4);
SELECT count(a), sum(a) FROM t1;

PRAGMA lock_status;

SELECT * FROM sqlite_master;
Pragma Locking_Mode = Exclusive;

BEGIN;
CREATE TABLE t1(a, b);
INSERT INTO t1 VALUES(1, 2);
COMMIT;
PRAGMA lock_status;

PRAGMA locking_mode = normal; 
PRAGMA lock_status;

SELECT * FROM t1;
PRAGMA lock_status;

INSERT INTO t1 VALUES(3, 4);
PRAGMA lock_status;

Pragma Locking_Mode = Exclusive;
Pragma Journal_Mode = Wal;
Pragma Lock_Status;

BEGIN;
CREATE TABLE t1(a, b);
INSERT INTO t1 VALUES(1, 2);
COMMIT;
Pragma loCK_STATus;

SELECT * FROM sqlite_master;

PRAGMA LOCKING_MODE = EXCLUSIVE;

SELECT count(a), sum(a) FROM t1;

SELECT * FROM t1;
pragma lock_status;

INSERT INTO t1 VALUES(3, 4);
pragma lock_status;

PRAGMA locking_mode = NORMAL;
pragma lock_status;

BEGIN IMMEDIATE; COMMIT;
pragma lock_status;

PRAGMA locking_mode = EXCLUSIVE;
BEGIN IMMEDIATE; COMMIT;
PRAGMA locking_mode = NORMAL;

SELECT * FROM t1;
pragma lock_status;

INSERT INTO t1 VALUES(5, 6);
SELECT * FROM t1;
pragma lock_status;

PRAGMA journal_mode = WAL;
PRAGMA locking_mode = exclusive;
BEGIN;
CREATE TABLE t1(x);
INSERT INTO t1 VALUES('Chico');
INSERT INTO t1 VALUES('Harpo');
COMMIT;

PRAGMA journal_mode = DELETE;

PRAGMA lock_status;

INSERT INTO t1 VALUES(iInsert);

BEGIN;
INSERT INTO t1 VALUES('Groucho');

PRAGMA lock_status;

COMMIT;

PRAGMA lock_status;

PRAGMA journal_mode = wal;
PRAGMA locking_mode = exclusive;
CREATE TABLE t2(a, b);
PRAGMA wal_checkpoint;
INSERT INTO t2 VALUES('I', 'II');
PRAGMA journal_mode;

PRAGMA locking_mode = normal;
INSERT INTO t2 VALUES('III', 'IV');
PRAGMA locking_mode = exclusive;
SELECT * FROM t2;

PRAGMA wal_checkpoint;

SELECT * FROM sqlite_master;

PRAGMA locking_mode = exclusive;

INSERT INTO t2 VALUES('V', 'VI');

SELECT count(a), sum(a) FROM t1;

PRAGMA locking_mode = normal;

INSERT INTO t2 VALUES('VII', 'VIII');

INSERT INTO t2 VALUES('IX', 'X');

PRAGMA page_size = 4096;
PRAGMA journal_mode = WAL;
CREATE TABLE t1(a, b);

PRAGMA wal_checkpoint;

SELECT * FROM sqlite_master;

PRAGMA auto_vacuum=OFF;
PRAGMA page_size = 1024;
PRAGMA journal_mode = WAL;
CREATE TABLE t1(x);
INSERT INTO t1 VALUES(zeroblob(8188*1020));
CREATE TABLE t2(y);

PRAGMA wal_checkpoint;
SELECT rootpage>=8192 FROM sqlite_master WHERE tbl_name = 't2';

PRAGMA cache_size = 10;
CREATE TABLE t3(z);
BEGIN;
INSERT INTO t3 VALUES(randomblob(900));
INSERT INTO t3 SELECT randomblob(900) FROM t3;
INSERT INTO t2 VALUES('hello');
INSERT INTO t3 SELECT randomblob(900) FROM t3;
INSERT INTO t3 SELECT randomblob(900) FROM t3;
INSERT INTO t3 SELECT randomblob(900) FROM t3;
INSERT INTO t3 SELECT randomblob(900) FROM t3;
INSERT INTO t3 SELECT randomblob(900) FROM t3;
INSERT INTO t3 SELECT randomblob(900) FROM t3;
ROLLBACK;

INSERT INTO t2 VALUES('goodbye');
INSERT INTO t3 SELECT randomblob(900) FROM t3;
INSERT INTO t3 SELECT randomblob(900) FROM t3;

PRAGMA journal_mode = WAL;
CREATE TABLE t1(a);

SELECT * FROM t2;

PRAGMA journal_mode = WAL;
CREATE TABLE x(y);
INSERT INTO x VALUES('Barton');
INSERT INTO x VALUES('Deakin');

INSERT INTO x VALUES('Watson');

SELECT * FROM x;

SELECT * FROM x;

PRAGMA journal_mode = WAL;
CREATE TABLE t1(a, b);
PRAGMA wal_checkpoint;
INSERT INTO t1 VALUES(1, 2);
INSERT INTO t1 VALUES(3, 4);

SELECT * FROM t1;

SELECT * FROM t1;

PRAGMA journal_mode = WAL;
CREATE TABLE t1(a, b, c);
INSERT INTO t1 VALUES(1, 2, 3);
INSERT INTO t1 VALUES(4, 5, 6);
INSERT INTO t1 VALUES(7, 8, 9);
SELECT * FROM t1;

SELECT name FROM sqlite_master;

INSERT INTO t1 VALUES(1);
INSERT INTO t1 VALUES(2);
INSERT INTO t1 VALUES(3);
INSERT INTO t1 VALUES(4);
SELECT count(a), sum(a) FROM t1;

CREATE TABLE tx(y, z);
PRAGMA journal_mode = WAL;

INSERT INTO tx DEFAULT VALUES;

PRAGMA journal_mode = WAL;
CREATE TABLE t1(a, b);
PRAGMA wal_checkpoint;
INSERT INTO t1 VALUES('3.14', '2.72');

INSERT INTO t1 VALUES(7, zeroblob(12*4096));

PRAGMA wal_autocheckpoint = 1000;

INSERT INTO t1 VALUES(9, 10);

INSERT INTO t1 VALUES(11, 12);

INSERT INTO t1 VALUES(13, 14);

INSERT INTO t1 VALUES('abc');

INSERT INTO t1 VALUES('def');

SELECT count(a), sum(a) FROM t1;

PRAGMA wal_checkpoint;

INSERT INTO t1 VALUES(iInsert);

-- ===wal3.test===
PRAGMA cache_size = 2000;
PRAGMA page_size = 1024;
PRAGMA auto_vacuum = off;
PRAGMA synchronous = normal;
PRAGMA journal_mode = WAL;
PRAGMA wal_autocheckpoint = 0;
BEGIN;
CREATE TABLE t1(x);
INSERT INTO t1 VALUES( a_string(800) );                  /*    1 */
INSERT INTO t1 SELECT a_string(800) FROM t1;             /*    2 */
INSERT INTO t1 SELECT a_string(800) FROM t1;             /*    4 */
INSERT INTO t1 SELECT a_string(800) FROM t1;             /*    8 */
INSERT INTO t1 SELECT a_string(800) FROM t1;             /*   16 */
INSERT INTO t1 SELECT a_string(800) FROM t1;             /*   32 */
INSERT INTO t1 SELECT a_string(800) FROM t1;             /*   64 */
INSERT INTO t1 SELECT a_string(800) FROM t1;             /*  128*/
INSERT INTO t1 SELECT a_string(800) FROM t1;             /*  256 */
INSERT INTO t1 SELECT a_string(800) FROM t1;             /*  512 */
INSERT INTO t1 SELECT a_string(800) FROM t1;             /* 1024 */
INSERT INTO t1 SELECT a_string(800) FROM t1;             /* 2048 */
INSERT INTO t1 SELECT a_string(800) FROM t1 LIMIT 1970;  /* 4018 */
COMMIT;
PRAGMA cache_size = 10;

PRAGMA journal_mode = WAL;

CREATE TABLE x(y);
INSERT INTO x VALUES('z');
PRAGMA wal_checkpoint;

SELECT * FROM x;

SELECT * FROM x;

PRAGMA journal_mode = WAL;
CREATE TABLE t1(a, b);
INSERT INTO t1 VALUES(1, 2);
INSERT INTO t1 VALUES(3, 4);

SELECT * FROM t1;

SELECT * FROM t1;

PRAGMA journal_mode = WAL;

CREATE TABLE t1(a, b);
INSERT INTO t1 VALUES('o', 't');
INSERT INTO t1 VALUES('t', 'f');

BEGIN ; SELECT * FROM t1;

UPDATE t1 SET x = str WHERE rowid = i;

PRAGMA wal_checkpoint;

BEGIN;
SELECT * FROM t1;

COMMIT;

PRAGMA wal_checkpoint;

BEGIN;
SELECT * FROM t1;

PRAGMA journal_mode = WAL;

CREATE TABLE t1(a, b);
INSERT INTO t1 VALUES('h', 'h');
INSERT INTO t1 VALUES('l', 'b');

PRAGMA wal_checkpoint;

INSERT INTO t1 VALUES('b', 'c');

PRAGMA wal_checkpoint;

BEGIN;
INSERT INTO t1 SELECT a_string(800) FROM t1 LIMIT 100;
ROLLBACK;
PRAGMA integrity_check;

INSERT INTO t1 VALUES('n', 'o');

PRAGMA journal_mode = WAL;
CREATE TABLE blue(red PRIMARY KEY, green);

SELECT * FROM blue;

INSERT INTO blue VALUES(1, 2);

INSERT INTO blue VALUES(3, 4);

SELECT * FROM blue;

INSERT INTO blue VALUES(5, 6);

SELECT * FROM blue;

PRAGMA journal_mode = WAL;
CREATE TABLE b(c);
INSERT INTO b VALUES('Tehran');
INSERT INTO b VALUES('Qom');
INSERT INTO b VALUES('Markazi');
PRAGMA wal_checkpoint;

SELECT * FROM b;

SELECT count(*) FROM t1;

SELECT * FROM b;

INSERT INTO b VALUES('Qazvin');

INSERT INTO b VALUES('Gilan');
INSERT INTO b VALUES('Ardabil');

SELECT * FROM b;

SELECT * FROM b;

PRAGMA page_size = 1024;
PRAGMA journal_mode = WAL;
CREATE TABLE whoami(x);
INSERT INTO whoami VALUES('nobody');

UPDATE whoami SET x = c;

BEGIN;
SELECT * FROM whoami;

SELECT * FROM whoami;

PRAGMA wal_checkpoint;

SELECT x FROM t1 WHERE rowid = i;

PRAGMA wal_checkpoint;

PRAGMA integrity_check;

SELECT count(*) FROM t1;

SELECT x FROM t1 WHERE rowid = i;

PRAGMA integrity_check;

-- ===wal4.test===
PRAGMA journal_mode=WAL;
CREATE TABLE t1(x);
INSERT INTO t1 VALUES(1);
INSERT INTO t1 VALUES(2);
SELECT x FROM t1 ORDER BY x;

SELECT name FROM sqlite_master;

-- ===wal6.test===
CREATE TABLE t1(a INTEGER PRIMARY KEY, b);
INSERT INTO t1 VALUES(1,2);
SELECT * FROM t1;

PRAGMA journal_mode=WAL;
INSERT INTO t1 VALUES(3,4);
SELECT * FROM t1 ORDER BY a;

SELECT * FROM t1 ORDER BY a;