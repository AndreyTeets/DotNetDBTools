-- ===nan.test===
PRAGMA auto_vacuum=OFF;
PRAGMA page_size=1024;
CREATE TABLE t1(x FLOAT);

SELECT CAST(x AS text), typeof(x) FROM t1;

SELECT CAST(x AS text), typeof(x) FROM t1;

SELECT CAST(x AS text), typeof(x) FROM t1;

SELECT CAST(x AS text), typeof(x) FROM t1;

SELECT CAST(x AS text), typeof(x) FROM t1;

SELECT CAST(x AS text), typeof(x) FROM t1;

UPDATE t1 SET x=x-x;
SELECT CAST(x AS text), typeof(x) FROM t1;

DELETE FROM T1;

SELECT x, typeof(x) FROM t1;

DELETE FROM t1;
INSERT INTO t1 VALUES(0.5);
PRAGMA auto_vacuum=OFF;
PRAGMA page_size=1024;
VACUUM;

SELECT x, typeof(x) FROM t1;

SELECT x, typeof(x) FROM t1;

SELECT x, typeof(x) FROM t1;

SELECT x, typeof(x) FROM t1;

SELECT x, typeof(x) FROM t1;

SELECT x, typeof(x) FROM t1;

DELETE FROM t1;

SELECT x, typeof(x) FROM t1;

DELETE FROM t1;

SELECT x, typeof(x) FROM t1;

DELETE FROM t1;

SELECT x, typeof(x) FROM t1;

SELECT x, typeof(x) FROM t1;

DELETE FROM t1;

SELECT x, typeof(x) FROM t1;

DELETE FROM t1;

SELECT x, typeof(x) FROM t1;

DELETE FROM t1;

SELECT x, typeof(x) FROM t1;

DELETE FROM t1;

SELECT x, typeof(x) FROM t1;

DELETE FROM t1;

SELECT x, typeof(x) FROM t1;

SELECT x, typeof(x) FROM t1;

DELETE FROM t1;

SELECT CAST(x AS text), typeof(x) FROM t1;

DELETE FROM t1;

SELECT CAST(x AS text), typeof(x) FROM t1;

DELETE FROM t1;

SELECT x, typeof(x) FROM t1;

DELETE FROM t1;

SELECT x, typeof(x) FROM t1;

DELETE FROM t1;

SELECT x, typeof(x) FROM t1;

SELECT x, typeof(x) FROM t1;

DELETE FROM t1;

SELECT x, typeof(x) FROM t1;

DELETE FROM t1;

SELECT x, typeof(x) FROM t1;

DELETE FROM t1;

SELECT x, typeof(x) FROM t1;

DELETE FROM t1;

SELECT CAST(x AS text), typeof(x) FROM t1;

DELETE FROM t1;

SELECT x, typeof(x) FROM t1;

SELECT CAST(x AS text), typeof(x) FROM t1;

DELETE FROM t1;

SELECT x, typeof(x) FROM t1;

SELECT x, typeof(x) FROM t1;

UPDATE t1 SET x=x-x;
SELECT x, typeof(x) FROM t1;

DELETE FROM T1;