-- ===sidedelete.test===
CREATE TABLE sequence(a INTEGER PRIMARY KEY);
INSERT INTO sequence VALUES(1);
INSERT INTO sequence VALUES(2);

INSERT INTO sequence SELECT a+(SELECT max(a) FROM sequence) FROM sequence;

SELECT count(*) FROM sequence;

CREATE TABLE t1(a PRIMARY KEY, b);
CREATE TABLE chng(a PRIMARY KEY, b);
SELECT count(*) FROM t1;
SELECT count(*) FROM chng;

DELETE FROM t1;
INSERT INTO t1 SELECT a, a FROM sequence WHERE a<=i;
DELETE FROM chng;
INSERT INTO chng SELECT a*2, a*2+1 FROM sequence WHERE a<=i/2;
UPDATE OR REPLACE t1 SET a=(SELECT b FROM chng WHERE a=t1.a);
SELECT count(*), sum(a) FROM t1;

DROP TABLE t1;
CREATE TABLE t1(a PRIMARY KEY);
SELECT * FROM t1;

DELETE FROM t1;
INSERT INTO t1 SELECT a FROM sequence WHERE a<=i;
UPDATE OR REPLACE t1 SET a=a+1;
SELECT count(*), sum(a) FROM t1;