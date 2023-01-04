-- ===auth.test===
SELECT name FROM sqlite_master;

SELECT name FROM sqlite_master;

PRAGMA database_list;

ATTACH DATABASE ':memory:' AS test1;

PRAGMA database_list;

ATTACH DATABASE ':memory:' AS test1;

PRAGMA database_list;

PRAGMA database_list;

DETACH DATABASE test1;

SELECT name FROM sqlite_temp_master WHERE type='table';

SELECT name FROM sqlite_temp_master WHERE type='table';

SELECT name FROM sqlite_temp_master WHERE type='table';

SELECT name FROM sqlite_master;

DETACH DATABASE test1;

SELECT name FROM sqlite_master WHERE type='table';

SELECT name FROM sqlite_master WHERE type='table';

SELECT name FROM sqlite_master WHERE type='table';

CREATE TABLE t3(a PRIMARY KEY, b, c);
CREATE INDEX t3_idx1 ON t3(c COLLATE BINARY);
CREATE INDEX t3_idx2 ON t3(b COLLATE NOCASE);

REINDEX t3_idx1;

REINDEX BINARY;

REINDEX NOCASE;

REINDEX t3;

DROP TABLE t3;

SELECT name FROM sqlite_master;

CREATE TEMP TABLE t3(a PRIMARY KEY, b, c);
CREATE INDEX t3_idx1 ON t3(c COLLATE BINARY);
CREATE INDEX t3_idx2 ON t3(b COLLATE NOCASE);

REINDEX temp.t3_idx1;

REINDEX BINARY;

REINDEX NOCASE;

REINDEX temp.t3;

DROP TABLE t3;

CREATE TABLE t4(a,b,c);
CREATE INDEX t4i1 ON t4(a);
CREATE INDEX t4i2 ON t4(b,a,c);
INSERT INTO t4 VALUES(1,2,3);
ANALYZE;

SELECT count(*) FROM sqlite_stat1;

SELECT count(*) FROM sqlite_stat1;

CREATE TABLE t5(x);

SELECT name FROM sqlite_temp_master;

SELECT sql FROM sqlite_master WHERE name='t5';

SELECT sql FROM sqlite_master WHERE name='t5';

SELECT sql FROM sqlite_temp_master WHERE type='t5';

DROP TABLE t5;

CREATE TABLE t3(x INTEGER PRIMARY KEY, y, z);

INSERT INTO t3 VALUES(44,55,66);

CREATE TABLE tx(a1,a2,b1,b2,c1,c2);
CREATE TRIGGER r1 AFTER UPDATE ON t2 FOR EACH ROW BEGIN
INSERT INTO tx VALUES(OLD.a,NEW.a,OLD.b,NEW.b,OLD.c,NEW.c);
END;
UPDATE t2 SET a=a+1;
SELECT * FROM tx;

DELETE FROM tx;
UPDATE t2 SET a=a+100;
SELECT * FROM tx;

UPDATE t2 SET a=a+1;

CREATE VIEW v1 AS SELECT a+b AS x FROM t2;
CREATE TABLE v1chng(x1,x2);
CREATE TRIGGER r2 INSTEAD OF UPDATE ON v1 BEGIN
INSERT INTO v1chng VALUES(OLD.x,NEW.x);
END;
SELECT * FROM v1;

SELECT name FROM sqlite_temp_master;

UPDATE v1 SET x=1 WHERE x=117;

CREATE TRIGGER r3 INSTEAD OF DELETE ON v1 BEGIN
INSERT INTO v1chng VALUES(OLD.x,NULL);
END;
SELECT * FROM v1;

DELETE FROM v1 WHERE x=117;

SELECT count(a) AS cnt FROM t4 ORDER BY cnt;

DROP TABLE tx;

DROP TABLE v1chng;

SELECT name FROM (
SELECT * FROM sqlite_master UNION ALL SELECT * FROM sqlite_temp_master)
WHERE type='table'
ORDER BY name;

CREATE TABLE t5 ( x );
CREATE TRIGGER t5_tr1 AFTER INSERT ON t5 BEGIN 
UPDATE t5 SET x = 1 WHERE NEW.x = 0;
END;

INSERT INTO t5 (x) values(0);

SELECT * FROM t5;

SELECT * FROM t2;

SELECT * FROM t2;

SELECT * FROM t2;

ATTACH DATABASE 'test.db' AS two;

DETACH DATABASE two;

SELECT name FROM sqlite_master;

SELECT * FROM t2;

SELECT * FROM t2;

SELECT * FROM t2;

SELECT * FROM t2;

SELECT * FROM t2;

INSERT INTO t2 VALUES(11, 2, 33);

INSERT INTO t2 VALUES(7, 8, 9);

SELECT * FROM t2;

SELECT name FROM sqlite_master;

SELECT name FROM sqlite_master;

SELECT name FROM sqlite_temp_master;

SELECT name FROM sqlite_temp_master;

SELECT name FROM sqlite_temp_master;

SELECT name FROM sqlite_master;

SELECT name FROM sqlite_master;

SELECT name FROM sqlite_temp_master;

SELECT name FROM sqlite_temp_master;

SELECT name FROM sqlite_master;

SELECT name FROM sqlite_master;

SELECT name FROM sqlite_temp_master;

SELECT name FROM sqlite_temp_master;

SELECT name FROM sqlite_temp_master;

SELECT name FROM sqlite_master;

SELECT name FROM sqlite_master;

SELECT name FROM sqlite_temp_master;

SELECT name FROM sqlite_temp_master;

SELECT name FROM sqlite_master;

SELECT name FROM sqlite_master;

SELECT name FROM sqlite_master;

SELECT name FROM sqlite_master;

SELECT name FROM sqlite_master;

SELECT name FROM sqlite_temp_master;

SELECT name FROM sqlite_master;

SELECT name FROM sqlite_temp_master;

SELECT name FROM sqlite_temp_master;

SELECT name FROM sqlite_temp_master;

SELECT name FROM sqlite_temp_master;

SELECT name FROM sqlite_master;

SELECT name FROM sqlite_master;

SELECT name FROM sqlite_master;

SELECT name FROM sqlite_master;

SELECT name FROM sqlite_master WHERE type='trigger';

INSERT INTO t2 VALUES(1,2,3);

SELECT name FROM sqlite_master;

SELECT * FROM tx;

SELECT name FROM sqlite_master;

SELECT name FROM sqlite_temp_master;

SELECT name FROM sqlite_temp_master;

SELECT name FROM sqlite_temp_master;

SELECT name FROM sqlite_temp_master;

SELECT name FROM sqlite_temp_master;

SELECT name FROM sqlite_master;

SELECT name FROM sqlite_master;

SELECT name FROM sqlite_master;

SELECT name FROM sqlite_temp_master;

SELECT name FROM sqlite_master;

DROP TABLE tx;
DELETE FROM t2 WHERE a=1 AND b=2 AND c=3;
SELECT name FROM sqlite_master;

SELECT name FROM sqlite_temp_master;

SELECT name FROM sqlite_temp_master;

SELECT name FROM sqlite_temp_master;

SELECT name FROM sqlite_temp_master;

SELECT name FROM sqlite_temp_master;

SELECT name FROM sqlite_master;

SELECT name FROM sqlite_master;

SELECT name FROM sqlite_master;

SELECT name FROM sqlite_temp_master;

SELECT name FROM sqlite_master;

SELECT name FROM sqlite_master;

SELECT name FROM sqlite_temp_master;

SELECT name FROM sqlite_temp_master;

SELECT name FROM sqlite_temp_master;

SELECT name FROM sqlite_temp_master;

SELECT name FROM sqlite_temp_master;

SELECT name FROM sqlite_master;

SELECT name FROM sqlite_master;

SELECT name FROM sqlite_master;

SELECT name FROM sqlite_temp_master;

SELECT name FROM sqlite_master;

SELECT name FROM sqlite_master;

SELECT name FROM sqlite_temp_master;

SELECT name FROM sqlite_temp_master;

SELECT name FROM sqlite_temp_master;

SELECT name FROM sqlite_temp_master;

SELECT name FROM sqlite_temp_master;

SELECT * FROM t2;

SELECT * FROM t2;

PRAGMA database_list;

-- ===auth2.test===
CREATE TABLE t1(a,b,c);
INSERT INTO t1 VALUES(1,2,3);

CREATE TABLE t2(x,y,z);

CREATE VIEW v2 AS SELECT x+y AS a, y+z AS b from t2;

SELECT a, b FROM v2;

SELECT b, a FROM v2;

-- ===auth3.test===
CREATE TABLE t1(a,b,c);
INSERT INTO t1 VALUES(1, 2, 3);
INSERT INTO t1 VALUES(4, 5, 6);

SELECT * FROM t1;

DELETE FROM t1;
SELECT * FROM t1;

INSERT INTO t1 VALUES(1, 2, 3);
INSERT INTO t1 VALUES(4, 5, 6);
DELETE FROM t1;
SELECT * FROM t1;

INSERT INTO t1 VALUES(1, 2, 3);
INSERT INTO t1 VALUES(4, 5, 6);

DELETE FROM t1;

INSERT INTO t1 VALUES(1, 2, 3);
INSERT INTO t1 VALUES(4, 5, 6);

DELETE FROM t1;