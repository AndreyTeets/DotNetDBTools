-- ===crash.test===
SELECT count(*), md5sum(a), md5sum(b), md5sum(c) FROM abc;

pragma auto_vacuum;

SELECT count(*), md5sum(a), md5sum(b), md5sum(c) FROM abc2;

CREATE TABLE abc(a, b, c);
INSERT INTO abc VALUES(1, 2, 3);
INSERT INTO abc VALUES(4, 5, 6);

BEGIN;

COMMIT;

SELECT sum(a), sum(b), sum(c) from abc;

INSERT INTO abc SELECT * FROM abc;
INSERT INTO abc SELECT * FROM abc;
INSERT INTO abc SELECT * FROM abc;
INSERT INTO abc SELECT * FROM abc;
INSERT INTO abc SELECT * FROM abc;

ATTACH 'test2.db' AS aux;
PRAGMA aux.default_cache_size = 10;
CREATE TABLE aux.abc2 AS SELECT 2*a as a, 2*b as b, 2*c as c FROM abc;

CREATE TABLE abc(a, b, c);                          INSERT INTO abc VALUES(randstr(1500,1500), 0, 0);   INSERT INTO abc SELECT * FROM abc;
INSERT INTO abc SELECT * FROM abc;
INSERT INTO abc SELECT * FROM abc;

-- ===crash2.test===
PRAGMA integrity_check;

SELECT count(*), md5sum(a), md5sum(b), md5sum(c) FROM abc;

INSERT INTO abc SELECT * FROM abc;
INSERT INTO abc SELECT * FROM abc;
INSERT INTO abc SELECT * FROM abc;
INSERT INTO abc SELECT * FROM abc;
INSERT INTO abc SELECT * FROM abc;

-- ===crash3.test===
PRAGMA page_size = 1024;
BEGIN;
CREATE TABLE abc(a, b, c);
INSERT INTO abc VALUES(1, 2, 3);
COMMIT;

PRAGMA integrity_check;

SELECT * FROM abc;

BEGIN;
CREATE TABLE abc(a PRIMARY KEY, b, c);
CREATE TABLE def(d PRIMARY KEY, e, f);
PRAGMA default_cache_size = 10;
INSERT INTO abc VALUES(randstr(10,1000),randstr(10,1000),randstr(10,1000));
INSERT INTO abc 
SELECT randstr(10,1000),randstr(10,1000),randstr(10,1000) FROM abc;
INSERT INTO abc 
SELECT randstr(10,1000),randstr(10,1000),randstr(10,1000) FROM abc;
INSERT INTO abc 
SELECT randstr(10,1000),randstr(10,1000),randstr(10,1000) FROM abc;
INSERT INTO abc 
SELECT randstr(10,1000),randstr(10,1000),randstr(10,1000) FROM abc;
INSERT INTO abc 
SELECT randstr(10,1000),randstr(10,1000),randstr(10,1000) FROM abc;
INSERT INTO abc 
SELECT randstr(10,1000),randstr(10,1000),randstr(10,1000) FROM abc;
COMMIT;

PRAGMA integrity_check;

PRAGMA integrity_check;

-- ===crash4.test===
CREATE TABLE a(id INTEGER, name CHAR(50));

INSERT INTO a(id,name) VALUES(9,'nine');

INSERT INTO a(id,name) VALUES(10,'ten');

UPDATE A SET name='new text for row 3' WHERE id=3;

INSERT INTO a(id,name) VALUES(1,'one');

INSERT INTO a(id,name) VALUES(2,'two');

INSERT INTO a(id,name) VALUES(3,'three');

INSERT INTO a(id,name) VALUES(4,'four');

INSERT INTO a(id,name) VALUES(5,'five');

INSERT INTO a(id,name) VALUES(6,'six');

INSERT INTO a(id,name) VALUES(7,'seven');

INSERT INTO a(id,name) VALUES(8,'eight');

-- ===crash5.test===
pragma auto_vacuum = 1;
CREATE TABLE t1(a, b, c);
INSERT INTO t1 VALUES('1111111111', '2222222222', c);

CREATE UNIQUE INDEX i1 ON t1(a);

DELETE FROM t1;  INSERT INTO t1 VALUES('111111111', '2222222222', '33333333');
INSERT INTO t1 SELECT * FROM t1;                     INSERT INTO t1 SELECT * FROM t1;                     INSERT INTO t1 SELECT * FROM t1;                     INSERT INTO t1 SELECT * FROM t1;                     INSERT INTO t1 SELECT * FROM t1;                     INSERT INTO t1 SELECT * FROM t1 WHERE rowid%2;       -- 48;

pragma integrity_check;

SELECT * FROM t1;

-- ===crash6.test===
PRAGMA auto_vacuum=OFF;
PRAGMA page_size=2048;
BEGIN;
CREATE TABLE abc AS SELECT 1 AS a, 2 AS b, 3 AS c;
COMMIT;

SELECT count(*), md5sum(a), md5sum(b), md5sum(c) FROM abc;

CREATE TABLE abc(a, b, c);

INSERT INTO abc SELECT * FROM abc;
INSERT INTO abc SELECT * FROM abc;
INSERT INTO abc SELECT * FROM abc;
INSERT INTO abc SELECT * FROM abc;
INSERT INTO abc SELECT * FROM abc;

-- ===crash7.test===
SELECT count(*), md5sum(a), md5sum(b), md5sum(c) FROM abc;

-- ===crash8.test===
PRAGMA auto_vacuum=OFF;
CREATE TABLE t1(a, b);
CREATE INDEX i1 ON t1(a, b);
INSERT INTO t1 VALUES(1, randstr(1000,1000));
INSERT INTO t1 VALUES(2, randstr(1000,1000));
INSERT INTO t1 VALUES(3, randstr(1000,1000));
INSERT INTO t1 VALUES(4, randstr(1000,1000));
INSERT INTO t1 VALUES(5, randstr(1000,1000));
INSERT INTO t1 VALUES(6, randstr(1000,1000));
CREATE TABLE t2(a, b);
CREATE TABLE t3(a, b);
CREATE TABLE t4(a, b);
CREATE TABLE t5(a, b);
CREATE TABLE t6(a, b);
CREATE TABLE t7(a, b);
CREATE TABLE t8(a, b);
CREATE TABLE t9(a, b);
CREATE TABLE t10(a, b);
PRAGMA integrity_check;

SELECT count(*) FROM t1;
PRAGMA integrity_check;

SELECT count(*) FROM t1;
PRAGMA integrity_check;

SELECT count(*) FROM t1;
PRAGMA integrity_check;

PRAGMA journal_mode = persist;
CREATE TABLE ab(a, b);
INSERT INTO ab VALUES(0, 'abc');
INSERT INTO ab VALUES(1, NULL);
INSERT INTO ab VALUES(2, NULL);
INSERT INTO ab VALUES(3, NULL);
INSERT INTO ab VALUES(4, NULL);
INSERT INTO ab VALUES(5, NULL);
INSERT INTO ab VALUES(6, NULL);
UPDATE ab SET b = randstr(1000,1000);
ATTACH 'test2.db' AS aux;
PRAGMA aux.journal_mode = persist;
CREATE TABLE aux.ab(a, b);
INSERT INTO aux.ab SELECT * FROM main.ab;
UPDATE aux.ab SET b = randstr(1000,1000) WHERE a>=1;
UPDATE ab SET b = randstr(1000,1000) WHERE a>=1;

BEGIN;
UPDATE aux.ab SET b = 'def' WHERE a = 0;
UPDATE main.ab SET b = 'def' WHERE a = 0;
COMMIT;

UPDATE aux.ab SET b = randstr(1000,1000) WHERE a>=1;
UPDATE ab SET b = randstr(1000,1000) WHERE a>=1;

SELECT b FROM main.ab WHERE a = 1;

SELECT b FROM  aux.ab WHERE a = 1;

SELECT b FROM main.ab WHERE a = 0;
SELECT b FROM aux.ab WHERE a = 0;

SELECT b FROM aux.ab WHERE a = 0;

PRAGMA integrity_check;

SELECT b FROM main.ab WHERE a = 0;

CREATE TABLE t1(x PRIMARY KEY);
INSERT INTO t1 VALUES(randomblob(900));
INSERT INTO t1 SELECT randomblob(900) FROM t1;
INSERT INTO t1 SELECT randomblob(900) FROM t1;
INSERT INTO t1 SELECT randomblob(900) FROM t1;
INSERT INTO t1 SELECT randomblob(900) FROM t1;
INSERT INTO t1 SELECT randomblob(900) FROM t1;
INSERT INTO t1 SELECT randomblob(900) FROM t1;          /* 64 rows */;

PRAGMA integrity_check;

PRAGMA cache_size = 10;
CREATE TABLE t1(x PRIMARY KEY);
INSERT INTO t1 VALUES(randomblob(900));
INSERT INTO t1 SELECT randomblob(900) FROM t1;
INSERT INTO t1 SELECT randomblob(900) FROM t1;
INSERT INTO t1 SELECT randomblob(900) FROM t1;
INSERT INTO t1 SELECT randomblob(900) FROM t1;
INSERT INTO t1 SELECT randomblob(900) FROM t1;
INSERT INTO t1 SELECT randomblob(900) FROM t1;          /* 64 rows */
BEGIN;
UPDATE t1 SET x = randomblob(900);

PRAGMA integrity_check;

PRAGMA integrity_check;

PRAGMA synchronous = off;
BEGIN;
DELETE FROM t1;
SELECT count(*) FROM t1;

COMMIT;
SELECT count(*) FROM t1;

SELECT count(*) FROM t1;
PRAGMA integrity_check;

SELECT count(*) FROM t1;
PRAGMA integrity_check;

SELECT count(*) FROM t1;
PRAGMA integrity_check;

SELECT count(*) FROM t1;
PRAGMA integrity_check