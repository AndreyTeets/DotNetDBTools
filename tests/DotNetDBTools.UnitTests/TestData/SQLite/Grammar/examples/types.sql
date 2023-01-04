-- ===types.test===
CREATE TABLE t1(i integer, n numeric, t text, o blob);

INSERT INTO t1 VALUES(2100000000);
INSERT INTO t1 VALUES(-2100000000);

SELECT a FROM t1;

INSERT INTO t1 VALUES(9000000*1000000*1000000);
INSERT INTO t1 VALUES(-9000000*1000000*1000000);

SELECT a FROM t1;

select rootpage from sqlite_master where name = 't1';

select rootpage from sqlite_master where name = 't1';

CREATE TABLE t2(a float);
INSERT INTO t2 VALUES(0.0);
INSERT INTO t2 VALUES(12345.678);
INSERT INTO t2 VALUES(-12345.678);

SELECT a FROM t2;

select rootpage from sqlite_master where name = 't2';

select rootpage from sqlite_master where name = 't2';

SELECT typeof(i), typeof(n), typeof(t), typeof(o) FROM t1;

CREATE TABLE t3(a nullvalue);
INSERT INTO t3 VALUES(NULL);

SELECT a ISNULL FROM t3;

select rootpage from sqlite_master where name = 't3';

SELECT a FROM t4;

pragma encoding;

select rootpage from sqlite_master where name = 't4';

select rootpage from sqlite_master where name = 't4';

DROP TABLE t1;
DROP TABLE t2;
DROP TABLE t3;
DROP TABLE t4;
CREATE TABLE t1(a, b, c);

SELECT * FROM t1;

SELECT typeof(i), typeof(n), typeof(t), typeof(o) FROM t1;

SELECT typeof(i), typeof(n), typeof(t), typeof(o) FROM t1;

DROP TABLE t1;

CREATE TABLE t1(a integer);
INSERT INTO t1 VALUES(0);
INSERT INTO t1 VALUES(120);
INSERT INTO t1 VALUES(-120);

SELECT a FROM t1;

INSERT INTO t1 VALUES(30000);
INSERT INTO t1 VALUES(-30000);

SELECT a FROM t1;

-- ===types2.test===
CREATE TABLE t1(
i1 INTEGER,
i2 INTEGER,
n1 NUMERIC,
n2 NUMERIC,
t1 TEXT,
t2 TEXT,
o1 BLOB,
o2 BLOB
);
INSERT INTO t1 VALUES(NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL);

SELECT e FROM t1;

SELECT 1 FROM t1 WHERE expr;

SELECT 1 FROM t1 WHERE NOT (e);

CREATE TABLE t2(i INTEGER, n NUMERIC, t TEXT, o XBLOBY);
CREATE INDEX t2i1 ON t2(i);
CREATE INDEX t2i2 ON t2(n);
CREATE INDEX t2i3 ON t2(t);
CREATE INDEX t2i4 ON t2(o);

CREATE TABLE t3(i INTEGER, n NUMERIC, t TEXT, o BLOB);
INSERT INTO t3 VALUES(1, 1, 1, 1);
INSERT INTO t3 VALUES(2, 2, 2, 2);
INSERT INTO t3 VALUES(3, 3, 3, 3);
INSERT INTO t3 VALUES('1', '1', '1', '1');
INSERT INTO t3 VALUES('1.0', '1.0', '1.0', '1.0');

CREATE TABLE t4(i INTEGER, n NUMERIC, t VARCHAR(20), o LARGE BLOB);
INSERT INTO t4 VALUES(10, 20, 20, 30);

-- ===types3.test===
SELECT typeof(:V);

SELECT typeof(:V);

SELECT typeof(:V);

SELECT typeof(:V);

SELECT typeof(:V);

SELECT typeof(:V);

SELECT typeof(:V)