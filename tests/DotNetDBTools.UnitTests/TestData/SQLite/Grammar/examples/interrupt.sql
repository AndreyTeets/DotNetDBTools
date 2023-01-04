-- ===interrupt.test===
CREATE TABLE t1(a,b);
SELECT name FROM sqlite_master;

SELECT name FROM sqlite_master;

BEGIN;
CREATE TABLE t1(a,b);
INSERT INTO t1 VALUES(1,randstr(300,400));
INSERT INTO t1 SELECT a+1, randstr(300,400) FROM t1;
INSERT INTO t1 SELECT a+2, a || '-' || b FROM t1;
INSERT INTO t1 SELECT a+4, a || '-' || b FROM t1;
INSERT INTO t1 SELECT a+8, a || '-' || b FROM t1;
INSERT INTO t1 SELECT a+16, a || '-' || b FROM t1;
INSERT INTO t1 SELECT a+32, a || '-' || b FROM t1;
COMMIT;
UPDATE t1 SET b=substr(b,-5,5);
SELECT count(*) from t1;

SELECT md5sum(a || b) FROM t1;

SELECT md5sum(a || b) FROM t1;

BEGIN;
CREATE TEMP TABLE t2(x,y);
SELECT name FROM sqlite_temp_master;

SELECT name FROM sqlite_temp_master;

SELECT name FROM sqlite_temp_master;

CREATE TABLE t2(a,b,c);
INSERT INTO t2 SELECT round(a/10), randstr(50,80), randstr(50,60) FROM t1;