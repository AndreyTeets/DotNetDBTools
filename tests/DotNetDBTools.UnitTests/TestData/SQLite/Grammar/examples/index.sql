-- ===index.test===
CREATE TABLE test1(f1 int, f2 int, f3 int);

CREATE TABLE test1(f1 int, f2 int, f3 int);

SELECT b FROM t1 WHERE typeof(a) IN ('integer','real') ORDER BY b;

CREATE TABLE t7(c UNIQUE PRIMARY KEY);
SELECT count(*) FROM sqlite_master WHERE tbl_name = 't7' AND type = 'index';

DROP TABLE t7;
CREATE TABLE t7(c UNIQUE PRIMARY KEY);
SELECT count(*) FROM sqlite_master WHERE tbl_name = 't7' AND type = 'index';

DROP TABLE t7;
CREATE TABLE t7(c PRIMARY KEY, UNIQUE(c) );
SELECT count(*) FROM sqlite_master WHERE tbl_name = 't7' AND type = 'index';

DROP TABLE t7;
CREATE TABLE t7(c, d , UNIQUE(c, d), PRIMARY KEY(c, d) );
SELECT count(*) FROM sqlite_master WHERE tbl_name = 't7' AND type = 'index';

DROP TABLE t7;
CREATE TABLE t7(c, d , UNIQUE(c), PRIMARY KEY(c, d) );
SELECT count(*) FROM sqlite_master WHERE tbl_name = 't7' AND type = 'index';

DROP TABLE t7;
CREATE TABLE t7(c, d UNIQUE, UNIQUE(c), PRIMARY KEY(c, d) );
SELECT name FROM sqlite_master WHERE tbl_name = 't7' AND type = 'index';

DROP TABLE t7;

CREATE TABLE t7(a UNIQUE PRIMARY KEY);
CREATE TABLE t8(a UNIQUE PRIMARY KEY ON CONFLICT ROLLBACK);
INSERT INTO t7 VALUES(1);
INSERT INTO t8 VALUES(1);

CREATE INDEX "t6i2" ON t6(c);
DROP INDEX "t6i2";

CREATE INDEX index1 ON test1(f4);

DROP INDEX "t6i1";

CREATE INDEX index1 ON test1(f1, f2, f4, f3);

DROP TABLE test1;

CREATE TABLE test1(f1 int, f2 int, f3 int, f4 int, f5 int);

SELECT name FROM sqlite_master 
WHERE type='index' AND tbl_name='test1'
ORDER BY name;

DROP TABLE test1;

SELECT name FROM sqlite_master 
WHERE type='index' AND tbl_name='test1'
ORDER BY name;

CREATE TABLE test1(cnt int, power int);

CREATE INDEX index9 ON test1(cnt);

CREATE INDEX index1 ON test1(f1);

CREATE INDEX indext ON test1(power);

SELECT name FROM sqlite_master WHERE type!='meta' ORDER BY name;

SELECT cnt FROM test1 WHERE power=4;

SELECT cnt FROM test1 WHERE power=1024;

SELECT power FROM test1 WHERE cnt=6;

DROP INDEX indext;

SELECT power FROM test1 WHERE cnt=6;

SELECT cnt FROM test1 WHERE power=1024;

CREATE INDEX indext ON test1(cnt);

SELECT power FROM test1 WHERE cnt=6;

SELECT name FROM sqlite_master WHERE type!='meta' ORDER BY name;

SELECT cnt FROM test1 WHERE power=1024;

DROP INDEX index9;

SELECT power FROM test1 WHERE cnt=6;

SELECT cnt FROM test1 WHERE power=1024;

DROP INDEX indext;

SELECT power FROM test1 WHERE cnt=6;

SELECT cnt FROM test1 WHERE power=1024;

DROP TABLE test1;

SELECT name FROM sqlite_master WHERE type!='meta' ORDER BY name;

CREATE INDEX index1 ON sqlite_master(name);

SELECT name, sql, tbl_name, type FROM sqlite_master 
WHERE name='index1';

SELECT name FROM sqlite_master WHERE type!='meta';

CREATE TABLE test1(f1 int, f2 int);

CREATE TABLE test2(g1 real, g2 real);

CREATE INDEX index1 ON test1(f1);

CREATE INDEX index1 ON test2(g1);

SELECT name FROM sqlite_master WHERE type!='meta' ORDER BY name;

CREATE INDEX test1 ON test2(g1);

SELECT name FROM sqlite_master WHERE type!='meta' ORDER BY name;

DROP TABLE test1;

DROP TABLE test2;

SELECT name, sql, tbl_name, type FROM sqlite_master 
WHERE name='index1';

SELECT name FROM sqlite_master WHERE type!='meta' ORDER BY name;

CREATE TABLE test1(a,b);
CREATE INDEX index1 ON test1(a);
CREATE INDEX index2 ON test1(b);
CREATE INDEX index3 ON test1(a,b);
DROP TABLE test1;
SELECT name FROM sqlite_master WHERE type!='meta' ORDER BY name;

CREATE TABLE test1(f1 int, f2 int primary key);

SELECT count(*) FROM test1;

SELECT f1 FROM test1 WHERE f2=65536;

SELECT name FROM sqlite_master 
WHERE type='index' AND tbl_name='test1';

DROP table test1;

SELECT name FROM sqlite_master WHERE type!='meta';

DROP INDEX index1;

CREATE TABLE tab1(a int);

SELECT name FROM sqlite_master WHERE type!='meta' ORDER BY name;

EXPLAIN CREATE INDEX idx1 ON tab1(a);

SELECT name FROM sqlite_master WHERE tbl_name='tab1';

CREATE INDEX idx1 ON tab1(a);

SELECT name FROM sqlite_master WHERE tbl_name='tab1' ORDER BY name;

CREATE TABLE t1(a int, b int);
CREATE INDEX i1 ON t1(a);
INSERT INTO t1 VALUES(1,2);
INSERT INTO t1 VALUES(2,4);
INSERT INTO t1 VALUES(3,8);
INSERT INTO t1 VALUES(1,12);
SELECT b FROM t1 WHERE a=1 ORDER BY b;

SELECT b FROM t1 WHERE a=2 ORDER BY b;

DELETE FROM t1 WHERE b=12;
SELECT b FROM t1 WHERE a=1 ORDER BY b;

DELETE FROM t1 WHERE b=2;
SELECT b FROM t1 WHERE a=1 ORDER BY b;

DELETE FROM t1;
INSERT INTO t1 VALUES (1,1);
INSERT INTO t1 VALUES (1,2);
INSERT INTO t1 VALUES (1,3);
INSERT INTO t1 VALUES (1,4);
INSERT INTO t1 VALUES (1,5);
INSERT INTO t1 VALUES (1,6);
INSERT INTO t1 VALUES (1,7);
INSERT INTO t1 VALUES (1,8);
INSERT INTO t1 VALUES (1,9);
INSERT INTO t1 VALUES (2,0);
SELECT b FROM t1 WHERE a=1 ORDER BY b;

DELETE FROM t1 WHERE b IN (2, 4, 6, 8);

DROP TABLE test1;

DELETE FROM t1 WHERE b = 2 OR b = 4 OR b = 6 OR b = 8;

SELECT b FROM t1 WHERE a=1 ORDER BY b;

DELETE FROM t1 WHERE b>2;
SELECT b FROM t1 WHERE a=1 ORDER BY b;

DELETE FROM t1 WHERE b=1;
SELECT b FROM t1 WHERE a=1 ORDER BY b;

SELECT b FROM t1 ORDER BY b;

CREATE TABLE t3(
a text,
b int,
c float,
PRIMARY KEY(b)
);

SELECT c FROM t3 WHERE b==10;

CREATE TABLE t4(a NUM,b);
INSERT INTO t4 VALUES('0.0',1);
INSERT INTO t4 VALUES('0.00',2);
INSERT INTO t4 VALUES('abc',3);
INSERT INTO t4 VALUES('-1.0',4);
INSERT INTO t4 VALUES('+1.0',5);
INSERT INTO t4 VALUES('0',6);
INSERT INTO t4 VALUES('00000',7);
SELECT a FROM t4 ORDER BY b;

SELECT a FROM t4 WHERE a==0 ORDER BY b;

SELECT a FROM t4 WHERE a<0.5 ORDER BY b;

SELECT name FROM sqlite_master WHERE type!='meta' ORDER BY name;

SELECT a FROM t4 WHERE a>-0.5 ORDER BY b;

CREATE INDEX t4i1 ON t4(a);
SELECT a FROM t4 WHERE a==0 ORDER BY b;

SELECT a FROM t4 WHERE a<0.5 ORDER BY b;

SELECT a FROM t4 WHERE a>-0.5 ORDER BY b;

CREATE TABLE t5(
a int UNIQUE,
b float PRIMARY KEY,
c varchar(10),
UNIQUE(a,c)
);
INSERT INTO t5 VALUES(1,2,3);
SELECT * FROM t5;

SELECT name FROM sqlite_master WHERE type="index" AND tbl_name="t5";

INSERT INTO t5 VALUES('a','b','c');
SELECT * FROM t5;

CREATE TABLE t6(a,b,c);
CREATE INDEX t6i1 ON t6(a,b);
INSERT INTO t6 VALUES('','',1);
INSERT INTO t6 VALUES('',NULL,2);
INSERT INTO t6 VALUES(NULL,'',3);
INSERT INTO t6 VALUES('abc',123,4);
INSERT INTO t6 VALUES(123,'abc',5);
SELECT c FROM t6 ORDER BY a,b;

SELECT c FROM t6 WHERE a='';

SELECT c FROM t6 WHERE b='';

CREATE INDEX index1 ON test1(f1);

SELECT c FROM t6 WHERE a>'';

SELECT c FROM t6 WHERE a>='';

SELECT c FROM t6 WHERE a>123;

SELECT c FROM t6 WHERE a>=123;

SELECT c FROM t6 WHERE a<'abc';

SELECT c FROM t6 WHERE a<='abc';

SELECT c FROM t6 WHERE a<='';

SELECT c FROM t6 WHERE a<'';

DELETE FROM t1;
SELECT * FROM t1;

INSERT INTO t1 VALUES('1.234e5',1);
INSERT INTO t1 VALUES('12.33e04',2);
INSERT INTO t1 VALUES('12.35E4',3);
INSERT INTO t1 VALUES('12.34e',4);
INSERT INTO t1 VALUES('12.32e+4',5);
INSERT INTO t1 VALUES('12.36E+04',6);
INSERT INTO t1 VALUES('12.36E+',7);
INSERT INTO t1 VALUES('+123.10000E+0003',8);
INSERT INTO t1 VALUES('+',9);
INSERT INTO t1 VALUES('+12347.E+02',10);
INSERT INTO t1 VALUES('+12347E+02',11);
INSERT INTO t1 VALUES('+.125E+04',12);
INSERT INTO t1 VALUES('-.125E+04',13);
INSERT INTO t1 VALUES('.125E+0',14);
INSERT INTO t1 VALUES('.125',15);
SELECT b FROM t1 ORDER BY a, b;

-- ===index2.test===
SELECT c123 FROM t1;

SELECT count(*) FROM t1;

SELECT round(sum(c1000)) FROM t1;

EXPLAIN SELECT c9 FROM t1 ORDER BY c1, c2, c3, c4, c5;

SELECT c9 FROM t1 ORDER BY c1, c2, c3, c4, c5, c6 LIMIT 5;

-- ===index3.test===
CREATE TABLE t1(a);
INSERT INTO t1 VALUES(1);
INSERT INTO t1 VALUES(1);
SELECT * FROM t1;

PRAGMA writable_schema=on;
UPDATE sqlite_master SET sql='nonsense';