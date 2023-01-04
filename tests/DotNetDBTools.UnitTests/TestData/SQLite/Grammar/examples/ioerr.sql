-- ===ioerr.test===
pragma auto_vacuum;

CREATE TABLE t1(a,b,c);
INSERT INTO t1 VALUES(randstr(200,200), randstr(1000,1000), 2);
BEGIN;
INSERT INTO t1 VALUES(randstr(200,200), randstr(1000,1000), 2);

COMMIT;

BEGIN;
CREATE TABLE t1(a PRIMARY KEY, b);

INSERT INTO t1 VALUES(:i, 'hello world');

COMMIT;

BEGIN;
INSERT INTO t1 VALUES('abc', 123);
INSERT INTO t1 VALUES('def', 123);
INSERT INTO t1 VALUES('ghi', 123);
INSERT INTO t1 SELECT (a+500)%900, 'good string' FROM t1;

PRAGMA page_size = 1024;

CREATE TABLE t1(x);

INSERT INTO t1 VALUES(randomblob(1100));

INSERT INTO t1 VALUES(randomblob(2000));

pragma auto_vacuum;

BEGIN;
PRAGMA cache_size = 10;
CREATE TABLE t1(a);
CREATE INDEX i1 ON t1(a);
CREATE TABLE t2(a);

INSERT INTO t1 VALUES(v);

DELETE FROM t1 WHERE oid > 85;
COMMIT;

PRAGMA cache_size = 10;
BEGIN;
CREATE TABLE abc(a);
INSERT INTO abc VALUES(randstr(1500,1500)); -- Page 4 is overflow;

INSERT INTO abc VALUES(randstr(100,100));

INSERT INTO abc (a1) VALUES(NULL);

pragma auto_vacuum;

ATTACH 'test2.db' as aux;
CREATE TABLE tx(a, b);
CREATE TABLE aux.ty(a, b);

SELECT * FROM t1;

CREATE TABLE t1(a,b,c);
INSERT INTO t1 VALUES(randstr(200,200), randstr(1000,1000), 2);

-- ===ioerr2.test===
PRAGMA cache_size = 10;
PRAGMA default_cache_size = 10;
CREATE TABLE t1(a, b, PRIMARY KEY(a, b));
INSERT INTO t1 VALUES(randstr(400,400),randstr(400,400));
INSERT INTO t1 SELECT randstr(400,400), randstr(400,400) FROM t1; INSERT INTO t1 SELECT randstr(400,400), randstr(400,400) FROM t1; INSERT INTO t1 SELECT randstr(400,400), randstr(400,400) FROM t1; INSERT INTO t1 SELECT randstr(400,400), randstr(400,400) FROM t1; INSERT INTO t1 SELECT randstr(400,400), randstr(400,400) FROM t1; -- 32;

SELECT md5sum(a, b) FROM t1;

PRAGMA integrity_check;

SELECT md5sum(a, b) FROM t1;

CREATE TABLE t2 AS SELECT * FROM t1;
PRAGMA temp_store = memory;

SELECT * FROM t1 WHERE rowid IN (1, 5, 10, 15, 20);

-- ===ioerr3.test===
INSERT INTO t1(id, name) VALUES (1,
'A1234567890B1234567890C1234567890D1234567890E1234567890F1234567890G1234567890H1234567890I1234567890J1234567890K1234567890L1234567890M1234567890N1234567890O1234567890P1234567890Q1234567890R1234567890'
);

-- ===ioerr4.test===
PRAGMA auto_vacuum=INCREMENTAL;
CREATE TABLE a(i INTEGER, b BLOB);

PRAGMA auto_vacuum;

INSERT INTO a VALUES(1, zeroblob(2000));
INSERT INTO a VALUES(2, zeroblob(2000));
INSERT INTO a SELECT i+2, zeroblob(2000) FROM a;
INSERT INTO a SELECT i+4, zeroblob(2000) FROM a;
INSERT INTO a SELECT i+8, zeroblob(2000) FROM a;
INSERT INTO a SELECT i+16, zeroblob(2000) FROM a;
SELECT count(*) FROM a;

PRAGMA freelist_count;

DELETE FROM a;
PRAGMA freelist_count;

PRAGMA auto_vacuum=INCREMENTAL;

PRAGMA incremental_vacuum(5);

-- ===ioerr5.test===
pragma page_size=512;
pragma auto_vacuum=2;
pragma cache_size=16;

CREATE TABLE A(Id INTEGER, Name TEXT);

pragma locking_mode=exclusive;

BEGIN EXCLUSIVE;
INSERT INTO a VALUES(1, 'ABCDEFGHIJKLMNOP');

SELECT count(*) FROM a;

CREATE INDEX i1 ON a(id, name);

pragma locking_mode=exclusive;

BEGIN EXCLUSIVE;
INSERT INTO a VALUES(1, 'ABCDEFGHIJKLMNOP');