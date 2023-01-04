-- ===shared.test===
pragma auto_vacuum;

BEGIN;
SELECT * FROM abc;

ROLLBACK;
BEGIN;
SELECT * FROM abc;

INSERT INTO def VALUES('VII', 'VIII', 'IX');

COMMIT;

SELECT * FROM sqlite_master;

PRAGMA read_uncommitted = 1;

SELECT max(i) FROM seq;

INSERT INTO seq SELECT i + :max, x FROM seq;

DELETE FROM seq WHERE i = :i;

SELECT * FROM seq;

CREATE TABLE abc(a, b, c);
INSERT INTO abc VALUES(1, 2, 3);

ATTACH 'test2.db' AS test2;

ATTACH 'test.db' AS test;

CREATE TABLE abc(a, b, c);
CREATE TABLE def(d, e, f);
INSERT INTO abc VALUES('i', 'ii', 'iii');
INSERT INTO def VALUES('I', 'II', 'III');

SELECT * FROM test.abc;

BEGIN;
SELECT * FROM test.abc;

COMMIT;

CREATE TABLE test2.ghi(g, h, i);
SELECT 'test.db:'||name FROM sqlite_master 
UNION ALL
SELECT 'test2.db:'||name FROM test2.sqlite_master;

SELECT 'test2.db:'||name FROM sqlite_master 
UNION ALL
SELECT 'test.db:'||name FROM test.sqlite_master;

BEGIN;
CREATE TABLE jkl(j, k, l);

ATTACH 'test1.db' AS test1;
ATTACH 'test2.db' AS test2;
ATTACH 'test3.db' AS test3;

SELECT * FROM abc;

ATTACH 'test3.db' AS test3;
ATTACH 'test2.db' AS test2;
ATTACH 'test1.db' AS test1;

CREATE TABLE test1.t1(a, b);
CREATE INDEX test1.i1 ON t1(a, b);

CREATE VIEW test1.v1 AS SELECT * FROM t1;

CREATE TRIGGER test1.trig1 AFTER INSERT ON t1 BEGIN
INSERT INTO t1 VALUES(new.a, new.b);
END;

DROP INDEX i1;

DROP VIEW v1;

DROP TRIGGER trig1;

DROP TABLE t1;

SELECT * FROM sqlite_master UNION ALL SELECT * FROM test1.sqlite_master;

CREATE TABLE t1(a, b);
CREATE TABLE t2(a, b);
INSERT INTO t1 VALUES(1, 2);
INSERT INTO t2 VALUES(3, 4);

BEGIN;
SELECT * FROM abc;

SELECT * FROM t1 UNION ALL SELECT * FROM t2;

BEGIN;
INSERT INTO t2 VALUES(5, 6);

COMMIT;
BEGIN;
INSERT INTO t1 VALUES(7, 8);

BEGIN;
CREATE TABLE t1(a PRIMARY KEY, b);
CREATE TABLE t2(a PRIMARY KEY, b);

INSERT INTO t1 VALUES(:a, :b);

INSERT INTO t2 SELECT * FROM t1;
COMMIT;

DELETE FROM t1;

PRAGMA encoding = 'UTF-16';
SELECT * FROM sqlite_master;

PRAGMA encoding;

PRAGMA encoding = 'UTF-8';
CREATE TABLE abc(a, b, c);

SELECT * FROM abc;

SELECT * FROM sqlite_master;

PRAGMA encoding;

ATTACH 'test2.db' AS aux;
SELECT * FROM aux.sqlite_master;

PRAGMA encoding = 'UTF-16';
CREATE TABLE def(d, e, f);

PRAGMA encoding;

CREATE TABLE def(d, e, f);

PRAGMA encoding;

CREATE TABLE abc(a, b, c);
CREATE TABLE abc_mirror(a, b, c);
CREATE TEMP TRIGGER BEFORE INSERT ON abc BEGIN 
INSERT INTO abc_mirror(a, b, c) VALUES(new.a, new.b, new.c);
END;
INSERT INTO abc VALUES(1, 2, 3);
SELECT * FROM abc_mirror;

INSERT INTO abc VALUES(4, 5, 6);
SELECT * FROM abc_mirror;

CREATE TABLE ab(a PRIMARY KEY, b);
CREATE TABLE de(d PRIMARY KEY, e);
INSERT INTO ab VALUES('Chiang Mai', 100000);
INSERT INTO ab VALUES('Bangkok', 8000000);
INSERT INTO de VALUES('Ubon', 120000);
INSERT INTO de VALUES('Khon Kaen', 200000);

CREATE TABLE def(d, e, f);
INSERT INTO def VALUES('IV', 'V', 'VI');

BEGIN;
SELECT * FROM ab;

BEGIN;
INSERT INTO de VALUES('Pataya', 30000);

SELECT * FROM ab;

SELECT * FROM de;

COMMIT;

SELECT * FROM de;

CREATE TABLE abc(a, b, c);
CREATE TABLE abc2(a, b, c);
BEGIN;
INSERT INTO abc VALUES(1, 2, 3);

ROLLBACK;
PRAGMA read_uncommitted = 1;

INSERT INTO abc2 VALUES(4, 5, 6);
INSERT INTO abc2 VALUES(7, 8, 9);

DELETE FROM abc WHERE 1;

SELECT * FROM sqlite_master;

PRAGMA cache_size = 10;
PRAGMA cache_size;

ATTACH 'test2.db' AS aux2;
ATTACH 'test3.db' AS aux3;
ATTACH 'test4.db' AS aux4;
ATTACH 'test5.db' AS aux5;
DETACH aux2;
DETACH aux3;
DETACH aux4;
ATTACH 'test2.db' AS aux2;
ATTACH 'test3.db' AS aux3;
ATTACH 'test4.db' AS aux4;

CREATE TABLE t1(a, b, c);
CREATE TABLE aux2.t2(a, b, c);
CREATE TABLE aux3.t3(a, b, c);
CREATE TABLE aux4.t4(a, b, c);
CREATE TABLE aux5.t5(a, b, c);
SELECT count(*) FROM 
aux2.sqlite_master, 
aux3.sqlite_master, 
aux4.sqlite_master, 
aux5.sqlite_master;

SELECT name FROM sqlite_master;

SELECT name FROM sqlite_master;

CREATE TABLE t1(a, b, c);
CREATE INDEX i1 ON t1(a, b);
CREATE VIEW v1 AS SELECT * FROM t1; 
CREATE VIEW v2 AS SELECT * FROM t1, v1 
WHERE t1.c=v1.c GROUP BY t1.a ORDER BY v1.b; 
CREATE TRIGGER tr1 AFTER INSERT ON t1 
WHEN new.a!=1
BEGIN
DELETE FROM t1 WHERE a=5;
INSERT INTO t1 VALUES(1, 2, 3);
UPDATE t1 SET c=c+1;
END;
INSERT INTO t1 VALUES(5, 6, 7);
INSERT INTO t1 VALUES(8, 9, 10);
INSERT INTO t1 VALUES(11, 12, 13);
ANALYZE;
SELECT * FROM t1;

DROP TABLE t1;

COMMIT;

BEGIN;
SELECT * FROM abc;

-- ===shared2.test===
BEGIN;
CREATE TABLE numbers(a PRIMARY KEY, b);
INSERT INTO numbers(oid) VALUES(NULL);
INSERT INTO numbers(oid) SELECT NULL FROM numbers;
INSERT INTO numbers(oid) SELECT NULL FROM numbers;
INSERT INTO numbers(oid) SELECT NULL FROM numbers;
INSERT INTO numbers(oid) SELECT NULL FROM numbers;
INSERT INTO numbers(oid) SELECT NULL FROM numbers;
INSERT INTO numbers(oid) SELECT NULL FROM numbers;
UPDATE numbers set a = oid, b = 'abcdefghijklmnopqrstuvwxyz0123456789';
COMMIT;

BEGIN;
INSERT INTO numbers VALUES(5, 'Medium length text field');
INSERT INTO numbers VALUES(6, 'Medium length text field');

ROLLBACK;

CREATE TABLE t0(a, b);
CREATE TABLE t1(a, b DEFAULT 'hello world');

SELECT a, b FROM t0;

INSERT INTO t1(a) VALUES(1);

CREATE TABLE t2(a, b, c);

CREATE INDEX i1 ON t2(a);

pragma read_uncommitted = 1;

SELECT count(*) FROM numbers;

BEGIN;
DELETE FROM numbers;

ROLLBACK;

SELECT count(*) FROM numbers;

DELETE FROM numbers;

INSERT INTO numbers VALUES(1, 'Medium length text field');
INSERT INTO numbers VALUES(2, 'Medium length text field');
INSERT INTO numbers VALUES(3, 'Medium length text field');
INSERT INTO numbers VALUES(4, 'Medium length text field');
BEGIN;
DELETE FROM numbers WHERE (a%2)=0;

ROLLBACK;

-- ===shared3.test===
PRAGMA main.cache_size = 10;

PRAGMA main.cache_size;

PRAGMA main.cache_size;

PRAGMA main.cache_size;

PRAGMA main.cache_size;

BEGIN;
INSERT INTO t1 VALUES(10, randomblob(5000));

INSERT INTO t1 VALUES(10, randomblob(10000));

-- ===shared6.test===
CREATE TABLE t1(a, b);
CREATE TABLE t2(c, d);
CREATE TABLE t3(e, f);

SELECT * FROM t2;

COMMIT;
BEGIN;
SELECT * FROM t1;

SELECT * FROM t1;

COMMIT;

PRAGMA read_uncommitted = 1;

BEGIN;
INSERT INTO t1 VALUES(5, 6);

SELECT * FROM t1;

CREATE TABLE t4(a, b);

COMMIT;
BEGIN;
SELECT * FROM t1;

BEGIN; INSERT INTO t1 VALUES(9, 10);

SELECT * FROM t1;

SELECT * FROM t1;

COMMIT;

BEGIN; INSERT INTO t1 VALUES(11, 12);

SELECT * FROM t1;

COMMIT;

BEGIN EXCLUSIVE;

BEGIN;

BEGIN;

BEGIN ; ROLLBACK;

CREATE TABLE t5(a, b);

BEGIN EXCLUSIVE;

SELECT * FROM t1;

COMMIT;
BEGIN;
INSERT INTO t2 VALUES(3, 4);

SELECT * FROM t1;

COMMIT;

BEGIN;
INSERT INTO t1 VALUES(1, 2);

SELECT * FROM t1;

-- ===shared7.test===
CREATE TABLE t1(x);

ATTACH 'test2.db' AS test2;
CREATE TABLE test2.t2(y);