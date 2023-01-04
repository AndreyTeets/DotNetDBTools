-- ===aggnested.test===
CREATE TABLE t1(a1 INTEGER);
INSERT INTO t1 VALUES(1), (2), (3);
CREATE TABLE t2(b1 INTEGER);
INSERT INTO t2 VALUES(4), (5);
SELECT (SELECT group_concat(a1,'x') FROM t2) FROM t1;

SELECT
 (SELECT group_concat(a1,'x') || '-' || group_concat(b1,'y') FROM t2)
FROM t1;

SELECT (SELECT group_concat(b1,a1) FROM t2) FROM t1;

SELECT (SELECT group_concat(a1,b1) FROM t2) FROM t1;

CREATE TABLE t1 (A1 INTEGER NOT NULL,A2 INTEGER NOT NULL,A3 INTEGER NOT 
NULL,A4 INTEGER NOT NULL,PRIMARY KEY(A1));
REPLACE INTO t1 VALUES(1,11,111,1111);
REPLACE INTO t1 VALUES(2,22,222,2222);
REPLACE INTO t1 VALUES(3,33,333,3333);
CREATE TABLE t2 (B1 INTEGER NOT NULL,B2 INTEGER NOT NULL,B3 INTEGER NOT 
NULL,B4 INTEGER NOT NULL,PRIMARY KEY(B1));
REPLACE INTO t2 VALUES(1,88,888,8888);
REPLACE INTO t2 VALUES(2,99,999,9999);
SELECT (SELECT GROUP_CONCAT(CASE WHEN a1=1 THEN'A' ELSE 'B' END) FROM t2),
        t1.* 
FROM t1;

CREATE TABLE AAA (
  aaa_id       INTEGER PRIMARY KEY AUTOINCREMENT
);
CREATE TABLE RRR (
  rrr_id      INTEGER     PRIMARY KEY AUTOINCREMENT,
  rrr_date    INTEGER     NOT NULL,
  rrr_aaa     INTEGER
);
CREATE TABLE TTT (
  ttt_id      INTEGER PRIMARY KEY AUTOINCREMENT,
  target_aaa  INTEGER NOT NULL,
  source_aaa  INTEGER NOT NULL
);
insert into AAA (aaa_id) values (2);
insert into TTT (ttt_id, target_aaa, source_aaa)
values (4469, 2, 2);
insert into TTT (ttt_id, target_aaa, source_aaa)
values (4476, 2, 1);
insert into RRR (rrr_id, rrr_date, rrr_aaa)
values (0, 0, NULL);
insert into RRR (rrr_id, rrr_date, rrr_aaa)
values (2, 4312, 2);
SELECT i.aaa_id,
  (SELECT sum(CASE WHEN (t.source_aaa == i.aaa_id) THEN 1 ELSE 0 END)
     FROM TTT t
  ) AS segfault
FROM
 (SELECT curr.rrr_aaa as aaa_id
    FROM RRR curr
      -- you also can comment out the next line
      -- it causes segfault to happen after one row is outputted
      INNER JOIN AAA a ON (curr.rrr_aaa = aaa_id)
      LEFT JOIN RRR r ON (r.rrr_id <> 0 AND r.rrr_date < curr.rrr_date)
   GROUP BY curr.rrr_id
  HAVING r.rrr_date IS NULL
) i;

DROP TABLE IF EXISTS t1;
DROP TABLE IF EXISTS t2;
CREATE TABLE t1 (
  id1 INTEGER PRIMARY KEY AUTOINCREMENT,
  value1 INTEGER
);
INSERT INTO t1 VALUES(4469,2),(4476,1);
CREATE TABLE t2 (
  id2 INTEGER PRIMARY KEY AUTOINCREMENT,
  value2 INTEGER
);
INSERT INTO t2 VALUES(0,1),(2,2);
SELECT
 (SELECT sum(value2==xyz) FROM t2)
FROM
 (SELECT curr.value1 as xyz
    FROM t1 AS curr LEFT JOIN t1 AS other
   GROUP BY curr.id1);

SELECT
 (SELECT sum(value2==xyz) FROM t2)
FROM
 (SELECT curr.value1 as xyz
    FROM t1 AS other RIGHT JOIN t1 AS curr
   GROUP BY curr.id1);

DROP TABLE IF EXISTS t1;
DROP TABLE IF EXISTS t2;
CREATE TABLE t1 (
  id1 INTEGER,
  value1 INTEGER,
  x1 INTEGER
);
INSERT INTO t1 VALUES(4469,2,98),(4469,1,99),(4469,3,97);
CREATE TABLE t2 (
  value2 INTEGER
);
INSERT INTO t2 VALUES(1);
SELECT
 (SELECT sum(value2==xyz) FROM t2)
FROM
 (SELECT value1 as xyz, max(x1) AS pqr
    FROM t1
   GROUP BY id1);
SELECT
 (SELECT sum(value2<>xyz) FROM t2)
FROM
 (SELECT value1 as xyz, max(x1) AS pqr
    FROM t1
   GROUP BY id1);

DROP TABLE IF EXISTS t1;
DROP TABLE IF EXISTS t2;
CREATE TABLE t1(id1, value1);
INSERT INTO t1 VALUES(4469,2),(4469,1);
CREATE TABLE t2 (value2);
INSERT INTO t2 VALUES(1);
SELECT (SELECT sum(value2=value1) FROM t2), max(value1)
  FROM t1
 GROUP BY id1;

DROP TABLE IF EXISTS t1;
DROP TABLE IF EXISTS t2;
CREATE TABLE t1(id1, value1);
INSERT INTO t1 VALUES(4469,12),(4469,11),(4470,34);
CREATE INDEX t1id1 ON t1(id1);
CREATE TABLE t2 (value2);
INSERT INTO t2 VALUES(12),(34),(34);
INSERT INTO t2 SELECT value2 FROM t2;

SELECT max(value1), (SELECT count(*) FROM t2 WHERE value2=max(value1))
  FROM t1
 GROUP BY id1;

SELECT max(value1), (SELECT count(*) FROM t2 WHERE value2=value1)
  FROM t1
 GROUP BY id1;

SELECT value1, (SELECT sum(value2=value1) FROM t2)
  FROM t1;

SELECT value1, (SELECT sum(value2=value1) FROM t2)
  FROM t1
 WHERE value1 IN (SELECT max(value1) FROM t1 GROUP BY id1);

SELECT max(value1), (SELECT sum(value2=max(value1)) FROM t2)
  FROM t1
 GROUP BY id1;

SELECT max(value1), (SELECT sum(value2=value1) FROM t2)
  FROM t1
 GROUP BY id1;

DROP TABLE IF EXISTS aa;
DROP TABLE IF EXISTS bb;
CREATE TABLE aa(x INT);  INSERT INTO aa(x) VALUES(123);
CREATE TABLE bb(y INT);  INSERT INTO bb(y) VALUES(456);
SELECT (SELECT sum(x+(SELECT y)) FROM bb) FROM aa;

SELECT (SELECT sum(x+y) FROM bb) FROM aa;

DROP TABLE IF EXISTS tx;
DROP TABLE IF EXISTS ty;
CREATE TABLE tx(x INT);
INSERT INTO tx VALUES(1),(2),(3),(4),(5);
CREATE TABLE ty(y INT);
INSERT INTO ty VALUES(91),(92),(93);
SELECT min((SELECT count(y) FROM ty)) FROM tx;

SELECT max((SELECT a FROM (SELECT count(*) AS a FROM ty) AS s)) FROM tx;

CREATE TABLE x1(a, b);
INSERT INTO x1 VALUES(1, 2);
CREATE TABLE x2(x);
INSERT INTO x2 VALUES(NULL), (NULL), (NULL);

SELECT ( SELECT total( (SELECT b FROM x1) ) ) FROM x2;

SELECT ( SELECT total( (SELECT 2 FROM x1) ) ) FROM x2;

CREATE TABLE t1(a);
CREATE TABLE t2(b);

SELECT(
  SELECT max(b) LIMIT (
    SELECT total( (SELECT a FROM t1) )
  )
)
FROM t2;

CREATE TABLE a(b);
WITH c AS(SELECT a)
  SELECT(SELECT(SELECT group_concat(b, b)
        LIMIT(SELECT 0.100000 *
          AVG(DISTINCT(SELECT 0 FROM a ORDER BY b, b, b))))
      FROM a GROUP BY b,
      b, b) FROM a EXCEPT SELECT b FROM a ORDER BY b,
  b, b;

CREATE TABLE t1(a);
CREATE TABLE t2(b);

INSERT INTO t1 VALUES('x');
INSERT INTO t2 VALUES(1);

SELECT ( 
  SELECT t2.b FROM (SELECT t2.b AS c FROM t1) GROUP BY 1 HAVING t2.b
)
FROM t2 GROUP BY 'constant_string';

SELECT ( 
  SELECT c FROM (SELECT t2.b AS c FROM t1) GROUP BY c HAVING t2.b
)
FROM t2 GROUP BY 'constant_string';

UPDATE t2 SET b=0;

SELECT ( 
  SELECT t2.b FROM (SELECT t2.b AS c FROM t1) GROUP BY 1 HAVING t2.b
)
FROM t2 GROUP BY 'constant_string';

SELECT ( 
  SELECT c FROM (SELECT t2.b AS c FROM t1) GROUP BY c HAVING t2.b
)
FROM t2 GROUP BY 'constant_string';