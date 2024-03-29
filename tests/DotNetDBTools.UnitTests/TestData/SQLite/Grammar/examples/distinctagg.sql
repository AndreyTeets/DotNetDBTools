-- ===distinctagg.test===
CREATE TABLE t1(a,b,c);
INSERT INTO t1 VALUES(1,2,3);
INSERT INTO t1 VALUES(1,3,4);
INSERT INTO t1 VALUES(1,3,5);
SELECT count(distinct a),
       count(distinct b),
       count(distinct c),
       count(a) FROM t1;

SELECT b, count(distinct c) FROM t1 GROUP BY b ORDER BY b;

INSERT INTO t1 SELECT a+1, b+3, c+5 FROM t1;
INSERT INTO t1 SELECT a+2, b+6, c+10 FROM t1;
INSERT INTO t1 SELECT a+4, b+12, c+20 FROM t1;
SELECT count(*), count(distinct a), count(distinct b) FROM t1;

SELECT a, count(distinct c) FROM t1 GROUP BY a ORDER BY a