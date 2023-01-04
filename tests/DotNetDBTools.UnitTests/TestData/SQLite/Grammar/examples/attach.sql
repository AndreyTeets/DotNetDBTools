-- ===attach.test===
CREATE TABLE t1(a,b);
INSERT INTO t1 VALUES(1,2);
INSERT INTO t1 VALUES(3,4);
SELECT * FROM t1;

CREATE TABLE tx(x1,x2,y1,y2);
CREATE TRIGGER r1 AFTER UPDATE ON t2 FOR EACH ROW BEGIN
INSERT INTO tx(x1,x2,y1,y2) VALUES(OLD.x,NEW.x,OLD.y,NEW.y);
END;
SELECT * FROM tx;

UPDATE t2 SET x=x+10;
SELECT * FROM tx;

CREATE TABLE tx(x1,x2,y1,y2);
SELECT * FROM tx;

ATTACH 'test2.db' AS db2;

UPDATE db2.t2 SET x=x+10;
SELECT * FROM db2.tx;

SELECT * FROM main.tx;

SELECT type, name, tbl_name FROM db2.sqlite_master;

CREATE INDEX i2 ON t2(x);
SELECT * FROM t2 WHERE x>5;

SELECT type, name, tbl_name FROM sqlite_master;

SELECT type, name, tbl_name FROM sqlite_master;

CREATE TABLE t2(x,y);
INSERT INTO t2 VALUES(1,'x');
INSERT INTO t2 VALUES(2,'y');
SELECT * FROM t2;

SELECT type, name, tbl_name FROM db2.sqlite_master;

ATTACH 'test2.db' AS db2;
SELECT type, name, tbl_name FROM db2.sqlite_master;

SELECT * FROM t1;

SELECT * FROM t2;

UPDATE t2 SET x=x+1 WHERE x=50;

SELECT * FROM t2;

UPDATE t2 SET x=0 WHERE 0;

SELECT * FROM t1;

SELECT * FROM t1;

ROLLBACK;

ATTACH DATABASE 'test2.db' AS two;
SELECT * FROM two.t2;

SELECT * FROM t1;

DETACH db2;

CREATE TABLE t3(x,y);
CREATE UNIQUE INDEX t3i1 ON t3(x);
INSERT INTO t3 VALUES(1,2);
SELECT * FROM t3;

CREATE TABLE t3(a,b);
CREATE UNIQUE INDEX t3i1b ON t3(a);
INSERT INTO t3 VALUES(9,10);
SELECT * FROM t3;

ATTACH DATABASE 'test2.db' AS db2;
SELECT * FROM db2.t3;

SELECT * FROM main.t3;

INSERT INTO db2.t3 VALUES(9,10);
SELECT * FROM db2.t3;

DETACH db2;

CREATE TABLE t4(x);
CREATE TRIGGER t3r3 AFTER INSERT ON t3 BEGIN
INSERT INTO t4 VALUES('db2.' || NEW.x);
END;
INSERT INTO t3 VALUES(6,7);
SELECT * FROM t4;

CREATE TABLE t4(y);
CREATE TRIGGER t3r3 AFTER INSERT ON t3 BEGIN
INSERT INTO t4 VALUES('main.' || NEW.a);
END;
INSERT INTO main.t3 VALUES(11,12);
SELECT * FROM main.t4;

SELECT * FROM t2;

CREATE TABLE t4(x);
INSERT INTO t3 VALUES(6,7);
INSERT INTO t4 VALUES('db2.6');
INSERT INTO t4 VALUES('db2.13');

CREATE TABLE t4(y);
INSERT INTO main.t3 VALUES(11,12);
INSERT INTO t4 VALUES('main.11');

ATTACH DATABASE 'test2.db' AS db2;
INSERT INTO db2.t3 VALUES(13,14);
SELECT * FROM db2.t4 UNION ALL SELECT * FROM main.t4;

INSERT INTO main.t4 VALUES('main.15');

INSERT INTO main.t3 VALUES(15,16);
SELECT * FROM db2.t4 UNION ALL SELECT * FROM main.t4;

INSERT INTO main.t4 VALUES('main.15');

ATTACH DATABASE 'test2.db' AS db2;
INSERT INTO db2.t3 VALUES(13,14);
INSERT INTO main.t3 VALUES(15,16);

DETACH DATABASE db2;

CREATE VIEW v3 AS SELECT x*100+y FROM t3;
SELECT * FROM v3;

CREATE VIEW v3 AS SELECT a*100+b FROM t3;
SELECT * FROM v3;

DETACH DATABASE two;
SELECT * FROM t1;

ATTACH DATABASE 'test2.db' AS db2;
SELECT * FROM db2.v3;

SELECT * FROM main.v3;

ATTACH 'test4.db' AS aux1;
CREATE TABLE aux1.t1(a, b);
INSERT INTO aux1.t1 VALUES(1, 2);
ATTACH 'test4.db' AS aux2;
SELECT * FROM aux2.t1;

COMMIT;
SELECT * FROM aux2.t1;

ATTACH '' AS noname;
ATTACH ':memory:' AS inmem;
BEGIN;
CREATE TABLE noname.noname(x);
CREATE TABLE inmem.inmem(y);
CREATE TABLE main.main(z);
COMMIT;
SELECT name FROM noname.sqlite_master;
SELECT name FROM inmem.sqlite_master;

PRAGMA database_list;

ATTACH 'test.db' AS db2;
ATTACH 'test.db' AS db3;
ATTACH 'test.db' AS db4;
ATTACH 'test.db' AS db5;
ATTACH 'test.db' AS db6;
ATTACH 'test.db' AS db7;
ATTACH 'test.db' AS db8;
ATTACH 'test.db' AS db9;

PRAGMA database_list;

DETACH db5;

select * from sqlite_temp_master;

-- ===attach2.test===
CREATE TABLE t1(a,b);
CREATE INDEX x1 ON t1(a);

BEGIN;

SELECT * FROM t1;

SELECT * FROM t1;

BEGIN;

INSERT INTO file2.t1 VALUES(1, 2);

SELECT * FROM t1;

SELECT * FROM file2.t1;

INSERT INTO t1 VALUES(1, 2);

SELECT * FROM t1;

ATTACH 'test.db2' AS aux;

PRAGMA database_list;

BEGIN;
CREATE TABLE tbl(a, b, c);
CREATE TABLE aux.tbl(a, b, c);
COMMIT;

BEGIN;
DROP TABLE aux.tbl;
DROP TABLE tbl;
ROLLBACK;

BEGIN;

COMMIT;
DETACH aux;

DETACH t2;

SELECT * FROM t1;

rollback;

SELECT * FROM t1;

PRAGMA lock_status;

ATTACH 'test2.db' as file2;

ATTACH 'test2.db' as file2;

-- ===attach3.test===
CREATE TABLE t1(a, b);
CREATE TABLE t2(c, d);

SELECT * FROM aux.sqlite_master WHERE name = 'i1';

DROP INDEX aux.i1;
SELECT * FROM aux.sqlite_master WHERE name = 'i1';

CREATE INDEX aux.i1 on t3(e);
SELECT * FROM aux.sqlite_master WHERE name = 'i1';

DROP INDEX i1;
SELECT * FROM aux.sqlite_master WHERE name = 'i1';

DROP TABLE aux.t1;
SELECT name FROM aux.sqlite_master;

DROP TABLE t2;
SELECT name FROM aux.sqlite_master;

DROP TABLE t2;
SELECT name FROM aux.sqlite_master;

CREATE VIEW aux.v1 AS SELECT * FROM t3;

SELECT * FROM aux.sqlite_master WHERE name = 'v1';

INSERT INTO aux.t3 VALUES('hello', 'world');
SELECT * FROM v1;

CREATE TABLE t1(a, b);
CREATE TABLE t2(c, d);

DROP VIEW aux.v1;

SELECT * FROM aux.sqlite_master WHERE name = 'v1';

CREATE TRIGGER aux.tr1 AFTER INSERT ON t3 BEGIN
INSERT INTO t3 VALUES(new.e*2, new.f*2);
END;

DELETE FROM t3;
INSERT INTO t3 VALUES(10, 20);
SELECT * FROM t3;

SELECT * FROM aux.sqlite_master WHERE name = 'tr1';

DROP TRIGGER aux.tr1;

SELECT * FROM aux.sqlite_master WHERE name = 'tr1';

CREATE TABLE main.t4(a, b, c);
CREATE TABLE aux.t4(a, b, c);
CREATE TEMP TRIGGER tst_trigger BEFORE INSERT ON aux.t4 BEGIN 
SELECT 'hello world';
END;
SELECT count(*) FROM sqlite_temp_master;

DROP TABLE main.t4;
SELECT count(*) FROM sqlite_temp_master;

DROP TABLE aux.t4;
SELECT count(*) FROM sqlite_temp_master;

ATTACH 'test2.db' AS aux;

PRAGMA database_list;

create temp table dummy(dummy);

DETACH aux;

ATTACH DATABASE '' AS '';

DETACH '';

DETACH '';

ATTACH DATABASE '' AS NULL;

CREATE TABLE aux.t3(e, f);

DETACH '';

SELECT * FROM sqlite_master WHERE name = 't3';

SELECT * FROM aux.sqlite_master WHERE name = 't3';

INSERT INTO t3 VALUES(1, 2);
SELECT * FROM t3;

CREATE INDEX aux.i1 on t3(e);

SELECT * FROM sqlite_master WHERE name = 'i1';