-- ===analyze.test===
SELECT count(*) FROM sqlite_master WHERE name='sqlite_stat1';

CREATE INDEX t1i3 ON t1(a,b);
ANALYZE main;
SELECT * FROM sqlite_stat1 ORDER BY idx;

INSERT INTO t1 VALUES(1,2);
INSERT INTO t1 VALUES(1,3);
ANALYZE main.t1;
SELECT idx, stat FROM sqlite_stat1 ORDER BY idx;

INSERT INTO t1 VALUES(1,4);
INSERT INTO t1 VALUES(1,5);
ANALYZE t1;
SELECT idx, stat FROM sqlite_stat1 ORDER BY idx;

INSERT INTO t1 VALUES(2,5);
ANALYZE main;
SELECT idx, stat FROM sqlite_stat1 ORDER BY idx;

CREATE TABLE t2 AS SELECT * FROM t1;
CREATE INDEX t2i1 ON t2(a);
CREATE INDEX t2i2 ON t2(b);
CREATE INDEX t2i3 ON t2(a,b);
ANALYZE;
SELECT idx, stat FROM sqlite_stat1 ORDER BY idx;

DROP INDEX t2i3;
ANALYZE t1;
SELECT idx, stat FROM sqlite_stat1 ORDER BY idx;

ANALYZE t2;
SELECT idx, stat FROM sqlite_stat1 ORDER BY idx;

DROP INDEX t2i2;
ANALYZE t2;
SELECT idx, stat FROM sqlite_stat1 ORDER BY idx;

CREATE TABLE t3 AS SELECT a, b, rowid AS c, 'hi' AS d FROM t1;
CREATE INDEX t3i1 ON t3(a);
CREATE INDEX t3i2 ON t3(a,b,c,d);
CREATE INDEX t3i3 ON t3(d,b,c,a);
DROP TABLE t1;
DROP TABLE t2;
SELECT idx, stat FROM sqlite_stat1 ORDER BY idx;

ANALYZE;
SELECT idx, stat FROM sqlite_stat1 ORDER BY idx;

SELECT count(*) FROM sqlite_master WHERE name='sqlite_stat1';

CREATE TABLE [silly " name](a, b, c);
CREATE INDEX 'foolish '' name' ON [silly " name](a, b);
CREATE INDEX 'another foolish '' name' ON [silly " name](c);
INSERT INTO [silly " name] VALUES(1, 2, 3);
INSERT INTO [silly " name] VALUES(4, 5, 6);
ANALYZE;
SELECT idx, stat FROM sqlite_stat1 ORDER BY idx;

DROP INDEX "foolish ' name";
SELECT idx, stat FROM sqlite_stat1 ORDER BY idx;

DROP TABLE "silly "" name";
SELECT idx, stat FROM sqlite_stat1 ORDER BY idx;

ANALYZE;
SELECT idx, stat FROM sqlite_stat1 ORDER BY idx;

PRAGMA writable_schema=on;
INSERT INTO sqlite_stat1 VALUES(null,null,null);
PRAGMA writable_schema=off;

SELECT * FROM t4 WHERE x=1234;

PRAGMA writable_schema=on;
DELETE FROM sqlite_stat1;
INSERT INTO sqlite_stat1 VALUES('t4','t4i1','nonsense');
INSERT INTO sqlite_stat1 VALUES('t4','t4i2','120897349817238741092873198273409187234918720394817209384710928374109827172901827349871928741910');
PRAGMA writable_schema=off;

SELECT * FROM t4 WHERE x=1234;

INSERT INTO sqlite_stat1 VALUES('t4','xyzzy','0 1 2 3');

SELECT * FROM t4 WHERE x=1234;

SELECT count(*) FROM sqlite_master WHERE name='sqlite_stat1';

PRAGMA writable_schema=on;
UPDATE sqlite_master SET sql='nonsense' WHERE name='sqlite_stat1';

SELECT * FROM sqlite_stat1 WHERE idx NOT NULL;

SELECT * FROM sqlite_stat1 WHERE idx NOT NULL;

SELECT * FROM sqlite_stat1;

SELECT * FROM sqlite_stat1;

CREATE INDEX t1i1 ON t1(a);
ANALYZE main.t1;
SELECT * FROM sqlite_stat1 ORDER BY idx;

CREATE INDEX t1i2 ON t1(b);
ANALYZE t1;
SELECT * FROM sqlite_stat1 ORDER BY idx;

-- ===analyze2.test===
CREATE TABLE t1(x PRIMARY KEY);

SELECT tbl,idx,group_concat(sample,' ') 
FROM sqlite_stat2 
WHERE idx = 't1_x' 
GROUP BY tbl,idx;

SELECT tbl,idx,group_concat(sample,' ') 
FROM sqlite_stat2 
WHERE idx = 't1_y' 
GROUP BY tbl,idx;

CREATE TABLE t3(a COLLATE nocase, b);

CREATE INDEX t3a ON t3(a);

CREATE INDEX t3b ON t3(b);

INSERT INTO t3 VALUES(str, str);

SELECT tbl,idx,group_concat(sample,' ') 
FROM sqlite_stat2 
WHERE idx = 't3a' 
GROUP BY tbl,idx;

SELECT tbl,idx,group_concat(sample,' ') 
FROM sqlite_stat2 
WHERE idx = 't3b' 
GROUP BY tbl,idx;

CREATE TABLE t4(x COLLATE test_collate);

CREATE INDEX t4x ON t4(x);

INSERT INTO t1 VALUES(i);

INSERT INTO t4 VALUES(str);

SELECT tbl,idx,group_concat(sample,' ') 
FROM sqlite_stat2 
WHERE tbl = 't4' 
GROUP BY tbl,idx;

DROP TABLE IF EXISTS t4;
CREATE TABLE t5(a, b); CREATE INDEX t5i ON t5(a, b);
CREATE TABLE t6(a, b); CREATE INDEX t6i ON t6(a, b);

INSERT INTO t5 VALUES(ii, ii);
INSERT INTO t6 VALUES(ii/10, ii/10);

CREATE TABLE master AS 
SELECT * FROM sqlite_master WHERE name LIKE 'sqlite_stat%';

PRAGMA writable_schema = 1;
DELETE FROM sqlite_master WHERE tbl_name = 'sqlite_stat2';

PRAGMA writable_schema = 1;
DELETE FROM sqlite_master WHERE tbl_name = 'sqlite_stat1';

PRAGMA writable_schema = 1;
INSERT INTO sqlite_master SELECT * FROM master;

DELETE FROM sqlite_stat1;
DELETE FROM sqlite_stat2;

PRAGMA writable_schema = 1;
DELETE FROM sqlite_master WHERE tbl_name = 'sqlite_stat1';

ANALYZE;
SELECT * FROM sqlite_stat2;

PRAGMA writable_schema = 1;
DELETE FROM sqlite_master WHERE tbl_name = 'sqlite_stat2';

PRAGMA writable_schema = 1;
INSERT INTO sqlite_master SELECT * FROM master;

SELECT count(*) FROM t5;

SELECT count(*) FROM t5;

SELECT count(*) FROM t5;

SELECT count(*) FROM t5;

SELECT * FROM sqlite_master;

SELECT * FROM sqlite_master;

DELETE FROM sqlite_stat2;

SELECT * FROM sqlite_master;

DELETE FROM t1 WHERe x>9;
ANALYZE;
SELECT tbl, idx, group_concat(sample, ' ') FROM sqlite_stat2;

SELECT * FROM sqlite_master;

SELECT * FROM sqlite_master;

DELETE FROM t1 WHERE x>8;
ANALYZE;
SELECT * FROM sqlite_stat2;

DELETE FROM t1;
ANALYZE;
SELECT * FROM sqlite_stat2;

BEGIN;
DROP TABLE t1;
CREATE TABLE t1(x, y);
CREATE INDEX t1_x ON t1(x);
CREATE INDEX t1_y ON t1(y);

INSERT INTO t1 VALUES(i, i);

INSERT INTO t1 VALUES(str, str);

-- ===analyze3.test===
SELECT sum(y) FROM t1 WHERE x>l AND x<u;

BEGIN;
CREATE TABLE t2(x TEXT, y);
INSERT INTO t2 SELECT * FROM t1;
CREATE INDEX i2 ON t2(x);
COMMIT;
ANALYZE;

SELECT sum(y) FROM t2 WHERE x>12 AND x<20;

SELECT typeof(l), typeof(u), sum(y) FROM t2 WHERE x>l AND x<u;

SELECT typeof(l), typeof(u), sum(y) FROM t2 WHERE x>l AND x<u;

SELECT sum(y) FROM t2 WHERE x>0 AND x<99;

SELECT typeof(l), typeof(u), sum(y) FROM t2 WHERE x>l AND x<u;

SELECT typeof(l), typeof(u), sum(y) FROM t2 WHERE x>l AND x<u;

BEGIN;
CREATE TABLE t3(y TEXT, x INTEGER);
INSERT INTO t3 SELECT y, x FROM t1;
CREATE INDEX i3 ON t3(x);
COMMIT;
ANALYZE;

SELECT sum(y) FROM t3 WHERE x>200 AND x<300;

BEGIN;
CREATE TABLE t1(x INTEGER, y);
CREATE INDEX i1 ON t1(x);

SELECT sum(y) FROM t3 WHERE x>l AND x<u;

SELECT sum(y) FROM t3 WHERE x>l AND x<u;

SELECT sum(y) FROM t3 WHERE x>0 AND x<1100;

SELECT sum(y) FROM t3 WHERE x>l AND x<u;

SELECT sum(y) FROM t3 WHERE x>l AND x<u;

PRAGMA case_sensitive_like=off;
BEGIN;
CREATE TABLE t1(a, b TEXT COLLATE nocase);
CREATE INDEX i1 ON t1(b);

INSERT INTO t1 VALUES(i, t);

SELECT count(*) FROM t1 WHERE b LIKE 'a%';

SELECT count(*) FROM t1 WHERE b LIKE '%a';

SELECT count(*) FROM t1 WHERE b LIKE like;

INSERT INTO t1 VALUES(i+100, i);

SELECT count(*) FROM t1 WHERE b LIKE like;

SELECT count(*) FROM t1 WHERE b LIKE like;

SELECT count(*) FROM t1 WHERE b LIKE like;

SELECT count(*) FROM t1 WHERE b LIKE like;

SELECT count(*) FROM t1 WHERE b LIKE like;

BEGIN;
CREATE TABLE t1(a, b, c);
CREATE INDEX i1 ON t1(b);

INSERT INTO t1 VALUES(i, i, i);

CREATE TABLE t4(x, y TEXT COLLATE NOCASE);
CREATE INDEX i4 ON t4(y);

DROP TABLE t1;

BEGIN;
CREATE TABLE t1(a, b, c);
CREATE INDEX i1 ON t1(b);

COMMIT;
ANALYZE;

INSERT INTO t1 VALUES(i, i, i);

CREATE TABLE t2(d, e, f);

CREATE TABLE t1(x TEXT COLLATE NOCASE);
CREATE INDEX i1 ON t1(x);
INSERT INTO t1 VALUES('aaa');
INSERT INTO t1 VALUES('abb');
INSERT INTO t1 VALUES('acc');
INSERT INTO t1 VALUES('baa');
INSERT INTO t1 VALUES('bbb');
INSERT INTO t1 VALUES('bcc');

SELECT sum(y) FROM t1 WHERE x>200 AND x<300;

SELECT sum(y) FROM t1 WHERE x>l AND x<u;

SELECT sum(y) FROM t1 WHERE x>l AND x<u;

SELECT sum(y) FROM t1 WHERE x>0 AND x<1100;

SELECT sum(y) FROM t1 WHERE x>l AND x<u;

-- ===analyze4.test===
CREATE TABLE t1(a,b);
CREATE INDEX t1a ON t1(a);
CREATE INDEX t1b ON t1(b);
INSERT INTO t1 VALUES(1,NULL);
INSERT INTO t1 SELECT a+1, b FROM t1;
INSERT INTO t1 SELECT a+2, b FROM t1;
INSERT INTO t1 SELECT a+4, b FROM t1;
INSERT INTO t1 SELECT a+8, b FROM t1;
INSERT INTO t1 SELECT a+16, b FROM t1;
INSERT INTO t1 SELECT a+32, b FROM t1;
INSERT INTO t1 SELECT a+64, b FROM t1;
ANALYZE;

EXPLAIN QUERY PLAN SELECT * FROM t1 WHERE a=5 AND b IS NULL;

SELECT idx, stat FROM sqlite_stat1 WHERE tbl='t1' ORDER BY idx;

UPDATE t1 SET b='x' WHERE a%2;
ANALYZE;
SELECT idx, stat FROM sqlite_stat1 WHERE tbl='t1' ORDER BY idx;

UPDATE t1 SET b=NULL;
ALTER TABLE t1 ADD COLUMN c;
ALTER TABLE t1 ADD COLUMN d;
UPDATE t1 SET c=a/4, d=a/2;
CREATE INDEX t1bcd ON t1(b,c,d);
CREATE INDEX t1cdb ON t1(c,d,b);
CREATE INDEX t1cbd ON t1(c,b,d);
ANALYZE;
SELECT idx, stat FROM sqlite_stat1 WHERE tbl='t1' ORDER BY idx;

CREATE TABLE t2(
x INTEGER PRIMARY KEY,
a TEXT COLLATE nocase,
b TEXT COLLATE rtrim,
c TEXT COLLATE binary
);
CREATE INDEX t2a ON t2(a);
CREATE INDEX t2b ON t2(b);
CREATE INDEX t2c ON t2(c);
CREATE INDEX t2c2 ON t2(c COLLATE nocase);
CREATE INDEX t2c3 ON t2(c COLLATE rtrim);
INSERT INTO t2 VALUES(1, 'abc', 'abc', 'abc');
INSERT INTO t2 VALUES(2, 'abC', 'abC', 'abC');
INSERT INTO t2 VALUES(3, 'abc ', 'abc ', 'abc ');
INSERT INTO t2 VALUES(4, 'abC ', 'abC ', 'abC ');
INSERT INTO t2 VALUES(5, 'aBc', 'aBc', 'aBc');
INSERT INTO t2 VALUES(6, 'aBC', 'aBC', 'aBC');
INSERT INTO t2 VALUES(7, 'aBc ', 'aBc ', 'aBc ');
INSERT INTO t2 VALUES(8, 'aBC ', 'aBC ', 'aBC ');
ANALYZE;
SELECT idx, stat FROM sqlite_stat1 WHERE tbl='t2' ORDER BY idx;