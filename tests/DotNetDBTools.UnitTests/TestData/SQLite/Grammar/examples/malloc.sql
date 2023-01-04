-- ===malloc.test===
CREATE TABLE abc(a, b, c);

UPDATE abc SET a = 0 WHERE oid%2;

ROLLBACK;

SELECT * FROM abc LIMIT 10;

PRAGMA integrity_check;

SELECT a FROM abc ORDER BY a;

UPDATE abc SET b = b - 1 WHERE a = a;

INSERT INTO t1 VALUES(1, randomblob(210));

CREATE TABLE t1(x PRIMARY KEY);
INSERT INTO t1 VALUES(randstr(500,500));
INSERT INTO t1 VALUES(randstr(500,500));
INSERT INTO t1 VALUES(randstr(500,500));

BEGIN;
DELETE FROM t1;
ROLLBACK;

PRAGMA locking_mode = normal;
BEGIN;
CREATE TABLE t1(a PRIMARY KEY, b);
INSERT INTO t1 VALUES(1, 'one');
INSERT INTO t1 VALUES(2, 'two');
INSERT INTO t1 VALUES(3, 'three');
COMMIT;
PRAGMA locking_mode = exclusive;

ATTACH 'test2.db' as aux;

UPDATE t1 SET a = a + 3;

PRAGMA locking_mode = normal;
UPDATE t1 SET a = a + 3;

PRAGMA integrity_check;

PRAGMA cache_size = 10;

CREATE TABLE abc(a, b);

INSERT INTO abc VALUES(randstr(100,100), randstr(1000,1000));

PRAGMA cache_size = 10;

CREATE TABLE abc(a PRIMARY KEY, b);

INSERT INTO abc VALUES(randstr(100,100), randstr(1000,1000));

PRAGMA locking_mode;

SELECT * FROM t1; 
SELECT * FROM t2;

CREATE TABLE t1(a, b);
INSERT INTO t1 VALUES(1, 2);

INSERT INTO t1 VALUES(3, 4);

ANALYZE;
CREATE TABLE t4(x COLLATE test_collate);
CREATE INDEX t4x ON t4(x);
INSERT INTO sqlite_stat2 VALUES('t4', 't4x', 0, 'aaa');
INSERT INTO sqlite_stat2 VALUES('t4', 't4x', 1, 'aaa');
INSERT INTO sqlite_stat2 VALUES('t4', 't4x', 2, 'aaa');
INSERT INTO sqlite_stat2 VALUES('t4', 't4x', 3, 'aaa');
INSERT INTO sqlite_stat2 VALUES('t4', 't4x', 4, 'aaa');
INSERT INTO sqlite_stat2 VALUES('t4', 't4x', 5, 'aaa');
INSERT INTO sqlite_stat2 VALUES('t4', 't4x', 6, 'aaa');
INSERT INTO sqlite_stat2 VALUES('t4', 't4x', 7, 'aaa');
INSERT INTO sqlite_stat2 VALUES('t4', 't4x', 8, 'aaa');
INSERT INTO sqlite_stat2 VALUES('t4', 't4x', 9, 'aaa');

SELECT * FROM t1;

CREATE TABLE t1(a, b COLLATE string_compare);
INSERT INTO t1 VALUES(10, 'string');
INSERT INTO t1 VALUES(10, 'string2');

SELECT * FROM sqlite_master;

PRAGMA encoding = "UTF16be";
CREATE TABLE abc(a, b, c);

CREATE TABLE t1(x);

PRAGMA cache_size = 10;
PRAGMA locking_mode = exclusive;
BEGIN;
CREATE TABLE abc(a, b, c);
CREATE INDEX abc_i ON abc(a, b, c);
INSERT INTO abc 
VALUES(randstr(100,100), randstr(100,100), randstr(100,100));
INSERT INTO abc 
SELECT randstr(100,100), randstr(100,100), randstr(100,100) FROM abc;
INSERT INTO abc 
SELECT randstr(100,100), randstr(100,100), randstr(100,100) FROM abc;
INSERT INTO abc 
SELECT randstr(100,100), randstr(100,100), randstr(100,100) FROM abc;
INSERT INTO abc 
SELECT randstr(100,100), randstr(100,100), randstr(100,100) FROM abc;
INSERT INTO abc 
SELECT randstr(100,100), randstr(100,100), randstr(100,100) FROM abc;
COMMIT;

-- ===malloc3.test===
SELECT tbl_name FROM sqlite_master;

SELECT min(
(oid == a) AND 'String value ' || a == b AND a == length(c) 
) FROM abc;

SELECT count(*) FROM abc;

SELECT min(
(oid == a) AND 'String value ' || a == b AND a == length(c) 
) FROM abc;

SELECT a, b, c FROM abc;

SELECT a, b, c FROM abc;

PRAGMA cache_size = 10;

SELECT a, count(*) FROM abc GROUP BY a;

SELECT a, count(*) FROM abc GROUP BY a;

SELECT a, count(*) FROM abc GROUP BY a;

SELECT a, count(*) FROM abc GROUP BY a;

SELECT tbl_name FROM sqlite_master;

SELECT a, count(*) FROM abc GROUP BY a;

SELECT a, count(*) FROM abc GROUP BY a;

SELECT * FROM abc;

SELECT a, count(*) FROM abc GROUP BY a;

SELECT name, tbl_name FROM sqlite_master;

SELECT name, tbl_name FROM sqlite_master;

SELECT name, tbl_name FROM sqlite_master;

SELECT * FROM def, ghi WHERE d = g;

SELECT * FROM v1 WHERE d = g;

SELECT * FROM tbl2, def WHERE d = x;

SELECT * FROM abc;

SELECT * FROM tbl2, def WHERE d = x;

SELECT * FROM ghi;

SELECT * FROM abc;

SELECT * FROM abc ORDER BY a DESC;

SELECT name, tbl_name FROM sqlite_master ORDER BY name;
SELECT * FROM abc;

SELECT count(*) FROM abc;

SELECT min(
(oid == a) AND 'String value ' || a == b AND a == length(c) 
) FROM abc;

SELECT count(*) FROM abc;

-- ===malloc4.test===
CREATE TABLE tbl(
the_first_reasonably_long_column_name that_also_has_quite_a_lengthy_type
);
INSERT INTO tbl VALUES(
'An extra long string. Far too long to be stored in NBFS bytes.'
);

-- ===malloc5.test===
PRAGMA auto_vacuum=OFF;
BEGIN;
CREATE TABLE abc(a, b, c);

COMMIT;

BEGIN;
SELECT * FROM abc;

SELECT * FROM sqlite_master;
BEGIN;
SELECT * FROM def;

SELECT * FROM abc; COMMIT;

SELECT * FROM def; COMMIT;

PRAGMA cache_size=2000;

BEGIN;

DELETE FROM abc;

COMMIT;

SELECT * FROM abc;

SELECT * FROM sqlite_master;

SELECT * FROM abc;

SELECT count(*), sum(a), sum(b) FROM abc;

BEGIN;
CREATE TABLE abc(a, b, c);
INSERT INTO abc VALUES('abcdefghi', 1234567890, NULL);
INSERT INTO abc SELECT * FROM abc;
INSERT INTO abc SELECT * FROM abc;
INSERT INTO abc SELECT * FROM abc;
INSERT INTO abc SELECT * FROM abc;
INSERT INTO abc SELECT * FROM abc;
INSERT INTO abc SELECT * FROM abc;
INSERT INTO abc SELECT * FROM abc;

COMMIT;
PRAGMA temp_store = memory;
SELECT * FROM abc ORDER BY a;

PRAGMA page_size=1024;
PRAGMA default_cache_size=10;

PRAGMA temp_store = memory;
BEGIN;
CREATE TABLE abc(a PRIMARY KEY, b, c);
INSERT INTO abc VALUES(randstr(50,50), randstr(75,75), randstr(100,100));
INSERT INTO abc 
SELECT randstr(50,50), randstr(75,75), randstr(100,100) FROM abc;
INSERT INTO abc 
SELECT randstr(50,50), randstr(75,75), randstr(100,100) FROM abc;
INSERT INTO abc 
SELECT randstr(50,50), randstr(75,75), randstr(100,100) FROM abc;
INSERT INTO abc 
SELECT randstr(50,50), randstr(75,75), randstr(100,100) FROM abc;
INSERT INTO abc 
SELECT randstr(50,50), randstr(75,75), randstr(100,100) FROM abc;
INSERT INTO abc 
SELECT randstr(50,50), randstr(75,75), randstr(100,100) FROM abc;
COMMIT;

PRAGMA cache_size;

PRAGMA cache_size;

SELECT * FROM abc;

SELECT * FROM abc;

COMMIT;
BEGIN;
SELECT * FROM abc;

SELECT * FROM abc;

BEGIN;
UPDATE abc SET c = randstr(100,100) 
WHERE rowid = 1 OR rowid = (SELECT max(rowid) FROM abc);

SELECT * FROM abc;

COMMIT;

BEGIN;
SELECT * FROM abc;
CREATE TABLE def(d, e, f);

COMMIT;

INSERT INTO abc VALUES(1, 2, 3);
INSERT INTO abc VALUES(4, 5, 6);
INSERT INTO def VALUES(7, 8, 9);
INSERT INTO def VALUES(10,11,12);

BEGIN;
SELECT * FROM def;

SELECT * FROM abc;

-- ===mallocA.test===
CREATE TABLE t1(a COLLATE NOCASE,b,c);
INSERT INTO t1 VALUES(1,2,3);
INSERT INTO t1 VALUES(1,2,4);
INSERT INTO t1 VALUES(2,3,4);
CREATE INDEX t1i1 ON t1(a);
CREATE INDEX t1i2 ON t1(b,c);
CREATE TABLE t2(x,y,z);

-- ===mallocC.test===
ROLLBACK;

PRAGMA auto_vacuum=1;
CREATE TABLE t0(a, b, c);

-- ===mallocH.test===
CREATE TABLE t1(x UNIQUE, y);
INSERT INTO t1 VALUES(1,2);

-- ===mallocK.test===
CREATE TABLE t1(a,b);
CREATE VIRTUAL TABLE t2 USING echo(t1);