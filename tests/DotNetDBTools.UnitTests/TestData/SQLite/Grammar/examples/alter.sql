-- ===alter.test===
SELECT 't1', * FROM t1;
SELECT 't1''x1', * FROM "t1'x1";
SELECT * FROM [temp table];

DELETE FROM objlist;
INSERT INTO objlist SELECT type, name, tbl_name
FROM sqlite_master WHERE NAME!='objlist';

SELECT type, name, tbl_name FROM objlist ORDER BY tbl_name, type desc, name;

ATTACH 'test2.db' AS aux;

CREATE TABLE t4(a PRIMARY KEY, b, c);
CREATE TABLE aux.t4(a PRIMARY KEY, b, c);
CREATE INDEX i4 ON t4(b);
CREATE INDEX aux.i4 ON t4(b);

INSERT INTO t4 VALUES('main', 'main', 'main');
INSERT INTO aux.t4 VALUES('aux', 'aux', 'aux');
SELECT * FROM t4 WHERE a = 'main';

ALTER TABLE t4 RENAME TO t5;
SELECT * FROM t4 WHERE a = 'aux';

SELECT * FROM t5;

SELECT * FROM t5 WHERE b = 'main';

ALTER TABLE aux.t4 RENAME TO t5;
SELECT * FROM aux.t5 WHERE b = 'aux';

CREATE TABLE tbl1   (a, b, c);
INSERT INTO tbl1 VALUES(1, 2, 3);

INSERT INTO objlist SELECT type, name, tbl_name 
FROM sqlite_temp_master WHERE NAME!='objlist';

SELECT * FROM tbl1;

ALTER TABLE tbl1 RENAME TO tbl2;
SELECT * FROM tbl2;

DROP TABLE tbl2;

CREATE TABLE t3(p,q,r);

CREATE TABLE t6(a, b, c);
CREATE TRIGGER trig1 AFTER INSERT ON t6 BEGIN
SELECT trigfunc('trig1', new.a, new.b, new.c);
END;

INSERT INTO t6 VALUES(1, 2, 3);

ALTER TABLE t6 RENAME TO t7;
INSERT INTO t7 VALUES(4, 5, 6);

DROP TRIGGER trig1;

CREATE TRIGGER trig2 AFTER INSERT ON main.t7 BEGIN
SELECT trigfunc('trig2', new.a, new.b, new.c);
END;
INSERT INTO t7 VALUES(1, 2, 3);

ALTER TABLE t7 RENAME TO t8;
INSERT INTO t8 VALUES(4, 5, 6);

SELECT type, name, tbl_name FROM objlist ORDER BY tbl_name, type desc, name;

DROP TRIGGER trig2;

CREATE TRIGGER trig3 AFTER INSERT ON main BEGIN
SELECT trigfunc('trig3', new.a, new.b, new.c);
END;
INSERT INTO t8 VALUES(1, 2, 3);

ALTER TABLE t8 RENAME TO t9;
INSERT INTO t9 VALUES(4, 5, 6);

DROP TABLE t10;

INSERT INTO tbl1 VALUES('a', 'b', 'c');

ALTER TABLE tbl1 RENAME TO tbl2;
INSERT INTO tbl2 VALUES('d', 'e', 'f');

ALTER TABLE tbl2 RENAME TO tbl3;
INSERT INTO tbl3 VALUES('g', 'h', 'i');

UPDATE tbl3 SET a = 'G' where a = 'g';

DROP TABLE tbl3;

SELECT * FROM sqlite_temp_master WHERE type = 'trigger';

ALTER TABLE [T1] RENAME to [-t1-];
ALTER TABLE "t1'x1" RENAME TO T2;
ALTER TABLE [temp table] RENAME to TempTab;

CREATE TABLE tbl1(a INTEGER PRIMARY KEY AUTOINCREMENT);
INSERT INTO tbl1 VALUES(10);

INSERT INTO tbl1 VALUES(NULL);
SELECT a FROM tbl1;

ALTER TABLE tbl1 RENAME TO tbl2;
DELETE FROM tbl2;
INSERT INTO tbl2 VALUES(NULL);
SELECT a FROM tbl2;

DROP TABLE tbl2;

CREATE TABLE tbl1(a, b, c);
INSERT INTO tbl1 VALUES('x', 'y', 'z');

ALTER TABLE tbl1 RENAME TO tbl2;
SELECT * FROM tbl2;

SELECT name FROM sqlite_master
WHERE type='table' AND name NOT GLOB 'sqlite*';

SELECT max(oid) FROM sqlite_master;

CREATE TABLE t1(a TEXT COLLATE BINARY);
ALTER TABLE t1 ADD COLUMN b INTEGER COLLATE NOCASE;
INSERT INTO t1 VALUES(1,'-2');
INSERT INTO t1 VALUES(5.4e-08,'5.4e-08');
SELECT typeof(a), a, typeof(b), b FROM t1;

CREATE TABLE t2(a INTEGER);
INSERT INTO t2 VALUES(1);
INSERT INTO t2 VALUES(1);
INSERT INTO t2 VALUES(2);
ALTER TABLE t2 ADD COLUMN b INTEGER DEFAULT 9;
SELECT sum(b) FROM t2;

SELECT 't1', * FROM [-t1-];
SELECT 't2', * FROM t2;
SELECT * FROM temptab;

SELECT a, sum(b) FROM t2 GROUP BY a;

SELECT SQLITE_RENAME_TRIGGER(0,0);

SELECT SQLITE_RENAME_TABLE(0,0);
SELECT SQLITE_RENAME_TABLE(10,20);
SELECT SQLITE_RENAME_TABLE('foo', 'foo');

SELECT name FROM sqlite_master WHERE name GLOB 'xyz*';

SELECT name FROM sqlite_master WHERE name GLOB 'sqlite_autoindex*';

SELECT name FROM sqlite_master WHERE name GLOB 'xyz*';

SELECT name FROM sqlite_master WHERE name GLOB 'sqlite_autoindex*';

ALTER TABLE t11 ADD COLUMN abc;

INSERT INTO t11 VALUES(1,2);

ALTER TABLE t11b ADD COLUMN abc;

DELETE FROM objlist;
INSERT INTO objlist SELECT type, name, tbl_name
FROM sqlite_master WHERE NAME!='objlist';

INSERT INTO t11b VALUES(3,4);

ALTER TABLE t11c ADD COLUMN abc;

INSERT INTO t11c VALUES(5,6);

CREATE TABLE t12(a, b, c);
CREATE VIEW v1 AS SELECT * FROM t12;

SELECT * FROM v1;

SELECT * FROM v1;

CREATE TABLE /* hi */ t3102a(x);
CREATE TABLE t3102b (y);
CREATE INDEX t3102c ON t3102a(x);
SELECT name FROM sqlite_master WHERE name GLOB 't3102*' ORDER BY 1;

ALTER TABLE t3102a RENAME TO t3102a_rename;
SELECT name FROM sqlite_master WHERE name GLOB 't3102*' ORDER BY 1;

ALTER TABLE t3102b RENAME TO t3102b_rename;
SELECT name FROM sqlite_master WHERE name GLOB 't3102*' ORDER BY 1;

SELECT type, name, tbl_name FROM objlist ORDER BY tbl_name, type desc, name;

CREATE TEMP TABLE objlist(type, name, tbl_name);
INSERT INTO objlist SELECT type, name, tbl_name FROM sqlite_master;
INSERT INTO objlist 
SELECT type, name, tbl_name FROM sqlite_temp_master 
WHERE NAME!='objlist';
SELECT type, name, tbl_name FROM objlist 
ORDER BY tbl_name, type desc, name;

DROP TABLE TempTab;

-- ===alter2.test===
CREATE TABLE abc(a, b);
INSERT INTO abc VALUES(1, 2);
INSERT INTO abc VALUES(3, 4);
INSERT INTO abc VALUES(5, 6);

SELECT typeof(d) FROM abc;

DROP TABLE abc;

CREATE TABLE abc2(a, b, c);
INSERT INTO abc2 VALUES(1, 2, 10);
INSERT INTO abc2 VALUES(3, 4, NULL);
INSERT INTO abc2 VALUES(5, 6, NULL);
CREATE VIEW abc2_v AS SELECT * FROM abc2;
SELECT * FROM abc2_v;

SELECT * FROM abc2_v;

DROP TABLE abc2;
DROP VIEW abc2_v;

CREATE TABLE abc3(a, b);
CREATE TABLE blog(o, n);
CREATE TRIGGER abc3_t AFTER UPDATE OF b ON abc3 BEGIN
INSERT INTO blog VALUES(old.b, new.b);
END;

INSERT INTO abc3 VALUES(1, 4);
UPDATE abc3 SET b = 2 WHERE b = 4;
SELECT * FROM blog;

INSERT INTO abc3 VALUES(3, 4);
INSERT INTO abc3 VALUES(5, 6);

SELECT * FROM abc3;

UPDATE abc3 SET b = b*2 WHERE a<4;
SELECT * FROM abc3;

SELECT * FROM abc;

SELECT * FROM blog;

CREATE TABLE clog(o, n);
CREATE TRIGGER abc3_t2 AFTER UPDATE OF c ON abc3 BEGIN
INSERT INTO clog VALUES(old.c, new.c);
END;
UPDATE abc3 SET c = a*2;
SELECT * FROM clog;

CREATE TABLE abc3(a, b);

SELECT 1 FROM sqlite_master LIMIT 1;

VACUUM;

ATTACH 'test2.db' AS aux;
CREATE TABLE aux.t1(a, b);

CREATE TABLE t1(a, b);

DROP TABLE t1;
CREATE TABLE t1(a);
INSERT INTO t1 VALUES(1);
INSERT INTO t1 VALUES(2);
INSERT INTO t1 VALUES(3);
INSERT INTO t1 VALUES(4);
SELECT * FROM t1;

SELECT * FROM t1 LIMIT 1;

SELECT a, typeof(a), b, typeof(b), c, typeof(c) FROM t1 LIMIT 1;

UPDATE abc SET c = 10 WHERE a = 1;
SELECT * FROM abc;

SELECT a, typeof(a), b, typeof(b), c, typeof(c) FROM t1 LIMIT 1;

SELECT a, typeof(a), b, typeof(b), c, typeof(c) FROM t1 LIMIT 1;

CREATE TRIGGER trig1 BEFORE UPDATE ON t1 BEGIN
SELECT set_val(
old.b||' '||typeof(old.b)||' '||old.c||' '||typeof(old.c)||' '||
new.b||' '||typeof(new.b)||' '||new.c||' '||typeof(new.c) 
);
END;

UPDATE t1 SET c = 10 WHERE a = 1;
SELECT a, typeof(a), b, typeof(b), c, typeof(c) FROM t1 LIMIT 1;

CREATE TRIGGER trig2 BEFORE DELETE ON t1 BEGIN
SELECT set_val(
old.b||' '||typeof(old.b)||' '||old.c||' '||typeof(old.c)
);
END;

DELETE FROM t1 WHERE a = 2;

CREATE TABLE t2(a);
INSERT INTO t2 VALUES('a');
INSERT INTO t2 VALUES('b');
INSERT INTO t2 VALUES('c');
INSERT INTO t2 VALUES('d');

SELECT quote(a), quote(b), quote(c) FROM t2 LIMIT 1;

CREATE INDEX i1 ON t2(b);
SELECT a FROM t2 WHERE b = X'ABCD';

DELETE FROM t2 WHERE a = 'c';
SELECT a FROM t2 WHERE b = X'ABCD';

CREATE INDEX abc_i ON abc(c);

SELECT count(b) FROM t2 WHERE b = X'ABCD';

SELECT c FROM abc ORDER BY c;

SELECT * FROM abc WHERE c = 10;

SELECT sum(a), c FROM abc GROUP BY c;

SELECT * FROM abc;

UPDATE abc SET d = 11 WHERE c IS NULL AND a<4;
SELECT * FROM abc;

-- ===alter3.test===
PRAGMA legacy_file_format=ON;
CREATE TABLE abc(a, b, c);
SELECT sql FROM sqlite_master;

DROP TABLE abc; 
DROP TABLE t1; 
DROP TABLE t3;

CREATE TABLE t1(a, b);

CREATE VIEW v1 AS SELECT * FROM t1;

DROP TABLE t1;

CREATE TABLE t1(a, b);
INSERT INTO t1 VALUES(1, 100);
INSERT INTO t1 VALUES(2, 300);
SELECT * FROM t1;

PRAGMA schema_version = 10;

ALTER TABLE t1 ADD c;
SELECT * FROM t1;

PRAGMA schema_version;

PRAGMA legacy_file_format=ON;
CREATE TABLE t1(a, b);
INSERT INTO t1 VALUES(1, 100);
INSERT INTO t1 VALUES(2, 300);
SELECT * FROM t1;

PRAGMA schema_version = 20;

ALTER TABLE abc ADD d INTEGER;

ALTER TABLE t1 ADD c DEFAULT 'hello world';
SELECT * FROM t1;

PRAGMA schema_version;

DROP TABLE t1;

CREATE TABLE t1(a, b);
INSERT INTO t1 VALUES(1, 'one');
INSERT INTO t1 VALUES(2, 'two');
ATTACH 'test2.db' AS aux;
CREATE TABLE aux.t1 AS SELECT * FROM t1;
PRAGMA aux.schema_version = 30;
SELECT sql FROM aux.sqlite_master;

ALTER TABLE aux.t1 ADD COLUMN c VARCHAR(128);
SELECT sql FROM aux.sqlite_master;

SELECT * FROM aux.t1;

PRAGMA aux.schema_version;

ALTER TABLE aux.t1 ADD COLUMN d DEFAULT 1000;
SELECT sql FROM aux.sqlite_master;

SELECT * FROM aux.t1;

PRAGMA aux.schema_version;

SELECT sql FROM sqlite_master;

SELECT * FROM t1;

DROP TABLE aux.t1;
DROP TABLE t1;

CREATE TABLE t1(a, b);
CREATE TABLE log(trig, a, b);
CREATE TRIGGER t1_a AFTER INSERT ON t1 BEGIN
INSERT INTO log VALUES('a', new.a, new.b);
END;
CREATE TEMP TRIGGER t1_b AFTER INSERT ON t1 BEGIN
INSERT INTO log VALUES('b', new.a, new.b);
END;
INSERT INTO t1 VALUES(1, 2);
SELECT * FROM log;

ALTER TABLE t1 ADD COLUMN c DEFAULT 'c';
INSERT INTO t1(a, b) VALUES(3, 4);
SELECT * FROM log;

VACUUM;

CREATE TABLE abc(a, b, c);
ALTER TABLE abc ADD d DEFAULT NULL;

ALTER TABLE abc ADD e DEFAULT 10;

ALTER TABLE abc ADD f DEFAULT NULL;

VACUUM;

CREATE TABLE t4(c1);

ALTER TABLE abc ADD e;

SELECT sql FROM sqlite_master WHERE name = 't4';

SELECT sql FROM sqlite_master;

CREATE TABLE main.t1(a, b);
ALTER TABLE t1 ADD c;
SELECT sql FROM sqlite_master WHERE tbl_name = 't1';

ALTER TABLE t1 ADD d CHECK (a>d);
SELECT sql FROM sqlite_master WHERE tbl_name = 't1';

CREATE TABLE t2(a, b, UNIQUE(a, b));
ALTER TABLE t2 ADD c REFERENCES t1(c)  ;
SELECT sql FROM sqlite_master WHERE tbl_name = 't2' AND type = 'table';

CREATE TABLE t3(a, b, UNIQUE(a, b));
ALTER TABLE t3 ADD COLUMN c VARCHAR(10, 20);
SELECT sql FROM sqlite_master WHERE tbl_name = 't3' AND type = 'table';

-- ===alter4.test===
CREATE TEMP TABLE abc(a, b, c);
SELECT sql FROM sqlite_temp_master;

DROP TABLE abc; 
DROP TABLE t1; 
DROP TABLE t3;

CREATE TABLE temp.t1(a, b);

CREATE TEMPORARY VIEW v1 AS SELECT * FROM t1;

DROP TABLE t1;

CREATE TEMP TABLE t1(a, b);
INSERT INTO t1 VALUES(1, 100);
INSERT INTO t1 VALUES(2, 300);
SELECT * FROM t1;

PRAGMA schema_version = 10;

ALTER TABLE t1 ADD c;
SELECT * FROM t1;

PRAGMA schema_version;

CREATE TEMP TABLE t1(a, b);
INSERT INTO t1 VALUES(1, 100);
INSERT INTO t1 VALUES(2, 300);
SELECT * FROM t1;

PRAGMA schema_version = 20;

ALTER TABLE abc ADD d INTEGER;

ALTER TABLE t1 ADD c DEFAULT 'hello world';
SELECT * FROM t1;

PRAGMA schema_version;

DROP TABLE t1;

CREATE TEMP TABLE t1(a, b);
INSERT INTO t1 VALUES(1, 'one');
INSERT INTO t1 VALUES(2, 'two');
ATTACH 'test2.db' AS aux;
CREATE TABLE aux.t1 AS SELECT * FROM t1;
PRAGMA aux.schema_version = 30;
SELECT sql FROM aux.sqlite_master;

ALTER TABLE aux.t1 ADD COLUMN c VARCHAR(128);
SELECT sql FROM aux.sqlite_master;

SELECT * FROM aux.t1;

PRAGMA aux.schema_version;

ALTER TABLE aux.t1 ADD COLUMN d DEFAULT 1000;
SELECT sql FROM aux.sqlite_master;

SELECT * FROM aux.t1;

PRAGMA aux.schema_version;

SELECT sql FROM sqlite_temp_master;

SELECT * FROM t1;

DROP TABLE aux.t1;
DROP TABLE t1;

CREATE TEMP TABLE t1(a, b);
CREATE TEMP TABLE log(trig, a, b);
CREATE TRIGGER t1_a AFTER INSERT ON t1 BEGIN
INSERT INTO log VALUES('a', new.a, new.b);
END;
CREATE TEMP TRIGGER t1_b AFTER INSERT ON t1 BEGIN
INSERT INTO log VALUES('b', new.a, new.b);
END;
INSERT INTO t1 VALUES(1, 2);
SELECT * FROM log;

ALTER TABLE t1 ADD COLUMN c DEFAULT 'c';
INSERT INTO t1(a, b) VALUES(3, 4);
SELECT * FROM log;

CREATE TEMP TABLE t4(c1);

SELECT sql FROM sqlite_temp_master WHERE name = 't4';

ALTER TABLE abc ADD e;

SELECT sql FROM sqlite_temp_master;

CREATE TABLE temp.t1(a, b);
ALTER TABLE t1 ADD c;
SELECT sql FROM sqlite_temp_master WHERE tbl_name = 't1';

ALTER TABLE t1 ADD d CHECK (a>d);
SELECT sql FROM sqlite_temp_master WHERE tbl_name = 't1';

CREATE TEMP TABLE t2(a, b, UNIQUE(a, b));
ALTER TABLE t2 ADD c REFERENCES t1(c)  ;
SELECT sql FROM sqlite_temp_master
WHERE tbl_name = 't2' AND type = 'table';

CREATE TEMPORARY TABLE t3(a, b, UNIQUE(a, b));
ALTER TABLE t3 ADD COLUMN c VARCHAR(10, 20);
SELECT sql FROM sqlite_temp_master
WHERE tbl_name = 't3' AND type = 'table';