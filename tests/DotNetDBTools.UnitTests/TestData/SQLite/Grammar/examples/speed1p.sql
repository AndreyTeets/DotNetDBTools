-- ===speed1p.test===
PRAGMA page_size=1024;
PRAGMA cache_size=500;
PRAGMA locking_mode=EXCLUSIVE;
CREATE TABLE t1(a INTEGER, b INTEGER, c TEXT);
CREATE TABLE t2(a INTEGER, b INTEGER, c TEXT);
CREATE INDEX i2a ON t2(a);
CREATE INDEX i2b ON t2(b);

SELECT c FROM t1 ORDER BY random() LIMIT 50000;

SELECT c FROM t1 WHERE c=c;

UPDATE t1 SET b=b*2 WHERE a>=lwr AND a<upr;

UPDATE t1 SET b=r WHERE a=i;

UPDATE t1 SET c=x WHERE a=i;

SELECT name FROM sqlite_master ORDER BY 1;

INSERT INTO t1 VALUES(i,r,x);

INSERT INTO t2 VALUES(i,r,x);

SELECT count(*), avg(b) FROM t1 WHERE b>=lwr AND b<upr;

SELECT count(*), avg(b) FROM t1 WHERE c LIKE pattern;

SELECT count(*), avg(b) FROM t1 WHERE b>=lwr AND b<upr;

SELECT c FROM t1 WHERE rowid=id;

SELECT c FROM t1 WHERE a=id