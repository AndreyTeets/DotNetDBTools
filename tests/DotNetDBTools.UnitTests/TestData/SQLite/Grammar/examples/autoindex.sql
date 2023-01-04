-- ===autoindex1.test===
CREATE TABLE t1(a,b);
INSERT INTO t1 VALUES(1,11);
INSERT INTO t1 VALUES(2,22);
INSERT INTO t1 SELECT a+2, b+22 FROM t1;
INSERT INTO t1 SELECT a+4, b+44 FROM t1;
CREATE TABLE t2(c,d);
INSERT INTO t2 SELECT a, 900+b FROM t1;

INSERT INTO t4 SELECT a+n, b+n FROM t4;

SELECT count(*) FROM t4;

SELECT count(*)
FROM t4 AS x1
JOIN t4 AS x2 ON x2.a=x1.b
JOIN t4 AS x3 ON x3.a=x2.b
JOIN t4 AS x4 ON x4.a=x3.b
JOIN t4 AS x5 ON x5.a=x4.b
JOIN t4 AS x6 ON x6.a=x5.b
JOIN t4 AS x7 ON x7.a=x6.b
JOIN t4 AS x8 ON x8.a=x7.b
JOIN t4 AS x9 ON x9.a=x8.b
JOIN t4 AS x10 ON x10.a=x9.b;

PRAGMA automatic_index=OFF;
SELECT b, d FROM t1 JOIN t2 ON a=c ORDER BY b;

PRAGMA automatic_index=ON;
SELECT b, d FROM t1 JOIN t2 ON a=c ORDER BY b;

PRAGMA automatic_index=OFF;
SELECT b, (SELECT d FROM t2 WHERE c=a) FROM t1;

PRAGMA automatic_index=ON;
SELECT b, (SELECT d FROM t2 WHERE c=a) FROM t1;

SELECT b, d FROM t1 JOIN t2 ON (c=a);

UPDATE t2 SET d=d+1;

SELECT d FROM t2 ORDER BY d;

CREATE TABLE t4(a, b);
INSERT INTO t4 VALUES(1,2);
INSERT INTO t4 VALUES(2,3);