-- ===in.test===
BEGIN;
CREATE TABLE t1(a int, b int);

SELECT a FROM t1 WHERE b NOT IN (8,12,16,24,32) ORDER BY a;

SELECT a FROM t1 WHERE b IN (8,12,16,24,32) OR b=512 ORDER BY a;

SELECT a FROM t1 WHERE b NOT IN (8,12,16,24,32) OR b=512 ORDER BY a;

SELECT a+100*(b IN (8,16,24)) FROM t1 ORDER BY b;

SELECT a FROM t1 WHERE b IN (b+8,64);

SELECT a FROM t1 WHERE b IN (max(5,10,b),20);

SELECT a FROM t1 WHERE b IN (8*2,64/2) ORDER BY b;

SELECT a FROM t1 WHERE b IN (max(5,10),20);

SELECT a FROM t1 WHERE min(0,b IN (a,30));

SELECT a FROM t1 WHERE c IN (10,20);

COMMIT;
SELECT count(*) FROM t1;

SELECT a FROM t1
WHERE b IN (SELECT b FROM t1 WHERE a<5)
ORDER BY a;

SELECT a FROM t1
WHERE b IN (SELECT b FROM t1 WHERE a<5) OR b==512
ORDER BY a;

SELECT a + 100*(b IN (SELECT b FROM t1 WHERE a<5)) FROM t1 ORDER BY b;

UPDATE t1 SET b=b*2 
WHERE b IN (SELECT b FROM t1 WHERE a>8);

SELECT b FROM t1 ORDER BY b;

DELETE FROM t1 WHERE b IN (SELECT b FROM t1 WHERE a>8);

SELECT a FROM t1 ORDER BY a;

DELETE FROM t1 WHERE b NOT IN (SELECT b FROM t1 WHERE a>4);

SELECT a FROM t1 ORDER BY a;

INSERT INTO t1 VALUES('hello', 'world');
SELECT * FROM t1
WHERE a IN (
'Do','an','IN','with','a','constant','RHS','but','where','the',
'has','many','elements','We','need','to','test','that',
'collisions','hash','table','are','resolved','properly',
'This','in-set','contains','thirty','one','entries','hello');

SELECT a FROM t1 WHERE b BETWEEN 10 AND 50 ORDER BY a;

CREATE TABLE ta(a INTEGER PRIMARY KEY, b);
INSERT INTO ta VALUES(1,1);
INSERT INTO ta VALUES(2,2);
INSERT INTO ta VALUES(3,3);
INSERT INTO ta VALUES(4,4);
INSERT INTO ta VALUES(6,6);
INSERT INTO ta VALUES(8,8);
INSERT INTO ta VALUES(10,
'This is a key that is long enough to require a malloc in the VDBE');
SELECT * FROM ta WHERE a<10;

CREATE TABLE tb(a INTEGER PRIMARY KEY, b);
INSERT INTO tb VALUES(1,1);
INSERT INTO tb VALUES(2,2);
INSERT INTO tb VALUES(3,3);
INSERT INTO tb VALUES(5,5);
INSERT INTO tb VALUES(7,7);
INSERT INTO tb VALUES(9,9);
INSERT INTO tb VALUES(11,
'This is a key that is long enough to require a malloc in the VDBE');
SELECT * FROM tb WHERE a<10;

SELECT a FROM ta WHERE b IN (SELECT a FROM tb);

SELECT a FROM ta WHERE b NOT IN (SELECT a FROM tb);

SELECT a FROM ta WHERE b IN (SELECT b FROM tb);

SELECT a FROM ta WHERE b NOT IN (SELECT b FROM tb);

SELECT a FROM ta WHERE a IN (SELECT a FROM tb);

SELECT a FROM ta WHERE a NOT IN (SELECT a FROM tb);

SELECT a FROM ta WHERE a IN (SELECT b FROM tb);

SELECT a FROM ta WHERE a NOT IN (SELECT b FROM tb);

SELECT a FROM t1 WHERE b NOT BETWEEN 10 AND 50 ORDER BY a;

SELECT a FROM t1 WHERE a IN ();

SELECT a FROM t1 WHERE a IN (5);

SELECT a FROM t1 WHERE a NOT IN () ORDER BY a;

SELECT a FROM t1 WHERE a IN (5) AND b IN ();

SELECT a FROM t1 WHERE a IN (5) AND b NOT IN ();

SELECT a FROM ta WHERE a IN ();

SELECT a FROM ta WHERE a NOT IN ();

SELECT b FROM t1 WHERE a IN ('hello','there');

SELECT b FROM t1 WHERE a IN ("hello",'there');

CREATE TABLE t4 AS SELECT a FROM tb;
SELECT * FROM t4;

SELECT a FROM t1 WHERE b BETWEEN a AND a*5 ORDER BY a;

SELECT b FROM t1 WHERE a IN t4;

SELECT b FROM t1 WHERE a NOT IN t4;

CREATE TABLE t5(
a INTEGER,
CHECK( a IN (111,222,333) )
);
INSERT INTO t5 VALUES(111);
SELECT * FROM t5;

CREATE TABLE t6(a,b NUMERIC);
INSERT INTO t6 VALUES(1,2);
INSERT INTO t6 VALUES(2,3);
SELECT * FROM t6 WHERE b IN (2);

SELECT * FROM t6 WHERE b IN ('2');

SELECT * FROM t6 WHERE +b IN ('2');

SELECT * FROM t6 WHERE a IN ('2');

SELECT * FROM t6 WHERE a IN (2);

SELECT * FROM t6 WHERE +a IN ('2');

CREATE TABLE t2(a, b, c);
CREATE TABLE t3(a, b, c);

SELECT a FROM t1 WHERE b NOT BETWEEN a AND a*5 ORDER BY a;

SELECT 
1 IN (NULL, 1, 2),     3 IN (NULL, 1, 2),     1 NOT IN (NULL, 1, 2), 3 NOT IN (NULL, 1, 2);  -- Ambiguous, return NULL.

CREATE TABLE t7(a, b, c NOT NULL);
INSERT INTO t7 VALUES(1,    1, 1);
INSERT INTO t7 VALUES(2,    2, 2);
INSERT INTO t7 VALUES(3,    3, 3);
INSERT INTO t7 VALUES(NULL, 4, 4);
INSERT INTO t7 VALUES(NULL, 5, 5);

SELECT 2 IN (SELECT a FROM t7);

SELECT 6 IN (SELECT a FROM t7);

SELECT 2 IN (SELECT b FROM t7);

SELECT 6 IN (SELECT b FROM t7);

SELECT 2 IN (SELECT c FROM t7);

SELECT 6 IN (SELECT c FROM t7);

SELECT
2 NOT IN (SELECT a FROM t7),
6 NOT IN (SELECT a FROM t7),
2 NOT IN (SELECT b FROM t7),
6 NOT IN (SELECT b FROM t7),
2 NOT IN (SELECT c FROM t7),
6 NOT IN (SELECT c FROM t7);

SELECT b IN (
SELECT inside.a 
FROM t7 AS inside 
WHERE inside.b BETWEEN outside.b+1 AND outside.b+2
)
FROM t7 AS outside ORDER BY b;

SELECT a FROM t1 WHERE b BETWEEN a AND a*5 OR b=512 ORDER BY a;

SELECT b NOT IN (
SELECT inside.a 
FROM t7 AS inside 
WHERE inside.b BETWEEN outside.b+1 AND outside.b+2
)
FROM t7 AS outside ORDER BY b;

CREATE INDEX i1 ON t7(a);
CREATE INDEX i2 ON t7(b);
CREATE INDEX i3 ON t7(c);

SELECT
2 IN (SELECT a FROM t7),
6 IN (SELECT a FROM t7),
2 IN (SELECT b FROM t7),
6 IN (SELECT b FROM t7),
2 IN (SELECT c FROM t7),
6 IN (SELECT c FROM t7);

SELECT
2 NOT IN (SELECT a FROM t7),
6 NOT IN (SELECT a FROM t7),
2 NOT IN (SELECT b FROM t7),
6 NOT IN (SELECT b FROM t7),
2 NOT IN (SELECT c FROM t7),
6 NOT IN (SELECT c FROM t7);

BEGIN TRANSACTION;
CREATE TABLE a(id INTEGER);
INSERT INTO a VALUES(1);
INSERT INTO a VALUES(2);
INSERT INTO a VALUES(3);
CREATE TABLE b(id INTEGER);
INSERT INTO b VALUES(NULL);
INSERT INTO b VALUES(3);
INSERT INTO b VALUES(4);
INSERT INTO b VALUES(5);
COMMIT;
SELECT * FROM a WHERE id NOT IN (SELECT id FROM b);

CREATE INDEX i5 ON b(id);
SELECT * FROM a WHERE id NOT IN (SELECT id FROM b);

SELECT a+ 100*(a BETWEEN 1 and 3) FROM t1 ORDER BY b;

SELECT a FROM t1 WHERE b IN (8,12,16,24,32) ORDER BY a;

-- ===in2.test===
CREATE TABLE a(i INTEGER PRIMARY KEY, a);

INSERT INTO a VALUES(ii, ii);

INSERT INTO a VALUES(4000, '');

INSERT INTO a VALUES(NULL, t);

SELECT 1 IN (SELECT a FROM a WHERE (i < ii) OR (i >= N));

-- ===in3.test===
CREATE TABLE t1(a PRIMARY KEY, b);
INSERT INTO t1 VALUES(1, 2);
INSERT INTO t1 VALUES(3, 4);
INSERT INTO t1 VALUES(5, 6);

CREATE TABLE t3(a, b, c);
CREATE UNIQUE INDEX t3_i ON t3(b, a);

INSERT INTO t3 VALUES(1, 'numeric', 2);
INSERT INTO t3 VALUES(2, 'text', 2);
INSERT INTO t3 VALUES(3, 'real', 2);
INSERT INTO t3 VALUES(4, 'none', 2);

CREATE UNIQUE INDEX t3_i2 ON t3(b);

DROP INDEX t3_i2;

CREATE TABLE Folders(
folderid INTEGER PRIMARY KEY, 
parentid INTEGER, 
rootid INTEGER, 
path VARCHAR(255)
);

DROP TABLE IF EXISTS t1;
CREATE TABLE t1(w int, x int, y int);
CREATE TABLE t2(p int, q int, r int, s int);

select max(y) from t1;

INSERT INTO t2 SELECT 101-w, x, maxy+1-y, y FROM t1;

SELECT rowid 
FROM t1 
WHERE rowid IN (SELECT rowid FROM t1 WHERE rowid IN (1, 2));

select rowid from t1 where rowid IN (-1,2,4);

SELECT rowid FROM t1 WHERE rowid IN 
(select rowid from t1 where rowid IN (-1,2,4));

DROP TABLE t1;
DROP TABLE t2;

CREATE TABLE t1(a BLOB, b NUMBER ,c TEXT);
CREATE UNIQUE INDEX t1_i1 ON t1(a);        /* no affinity */
CREATE UNIQUE INDEX t1_i2 ON t1(b);        /* numeric affinity */
CREATE UNIQUE INDEX t1_i3 ON t1(c);        /* text affinity */
CREATE TABLE t2(x BLOB, y NUMBER, z TEXT);
CREATE UNIQUE INDEX t2_i1 ON t2(x);        /* no affinity */
CREATE UNIQUE INDEX t2_i2 ON t2(y);        /* numeric affinity */
CREATE UNIQUE INDEX t2_i3 ON t2(z);        /* text affinity */
INSERT INTO t1 VALUES(1, 1, 1);
INSERT INTO t2 VALUES('1', '1', '1');

-- ===in4.test===
CREATE TABLE t1(a, b);
CREATE INDEX i1 ON t1(a);

SELECT b FROM t2 WHERE a IN (2, -1);

SELECT b FROM t2 WHERE a IN (NULL, 3);

SELECT b FROM t2 WHERE a IN (1.0, 2.1);

SELECT b FROM t2 WHERE a IN ('1', '2');

SELECT b FROM t2 WHERE a IN ('', '0.0.0', '2');

DROP TABLE IF EXISTS t1;
DROP TABLE IF EXISTS t2;
CREATE TABLE t1(x, id);
CREATE TABLE t2(x, id);
INSERT INTO t1 VALUES(NULL, NULL);
INSERT INTO t1 VALUES(0, NULL);
INSERT INTO t1 VALUES(1, 3);
INSERT INTO t1 VALUES(2, 4);
INSERT INTO t1 VALUES(3, 5);
INSERT INTO t1 VALUES(4, 6);
INSERT INTO t2 VALUES(0, NULL);
INSERT INTO t2 VALUES(4, 1);
INSERT INTO t2 VALUES(NULL, 1);
INSERT INTO t2 VALUES(NULL, NULL);

SELECT x FROM t1 WHERE id IN () AND x IN (SELECT x FROM t2 WHERE id=1);

CREATE TABLE t3(x, y, z);
CREATE INDEX t3i1 ON t3(x, y);
INSERT INTO t3 VALUES(1, 1, 1);
INSERT INTO t3 VALUES(10, 10, 10);

SELECT * FROM t3 WHERE x IN ();

SELECT * FROM t3 WHERE x = 10 AND y IN ();

SELECT * FROM t1 WHERE a IN ('aaa', 'bbb', 'ccc');

SELECT * FROM t3 WHERE x IN () AND y = 10;

SELECT * FROM t3 WHERE x IN () OR x = 10;

SELECT * FROM t3 WHERE y IN ();

SELECT x IN() AS a FROM t3 WHERE a;

SELECT x IN() AS a FROM t3 WHERE NOT a;

SELECT * FROM t3 WHERE oid IN ();

SELECT * FROM t3 WHERE x IN (1, 2) OR y IN ();

SELECT * FROM t3 WHERE x IN (1, 2) AND y IN ();

INSERT INTO t1 VALUES('aaa', 1);
INSERT INTO t1 VALUES('ddd', 2);
INSERT INTO t1 VALUES('ccc', 3);
INSERT INTO t1 VALUES('eee', 4);
SELECT b FROM t1 WHERE a IN ('aaa', 'bbb', 'ccc');

SELECT a FROM t1 WHERE rowid IN (1, 3);

SELECT a FROM t1 WHERE rowid IN ();

SELECT a FROM t1 WHERE a IN ('ddd');

CREATE TABLE t2(a INTEGER PRIMARY KEY, b TEXT);
INSERT INTO t2 VALUES(-1, '-one');
INSERT INTO t2 VALUES(0, 'zero');
INSERT INTO t2 VALUES(1, 'one');
INSERT INTO t2 VALUES(2, 'two');
INSERT INTO t2 VALUES(3, 'three');

SELECT b FROM t2 WHERE a IN (0, 2);

SELECT b FROM t2 WHERE a IN (2, 0)