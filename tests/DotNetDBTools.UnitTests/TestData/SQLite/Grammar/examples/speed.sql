-- ===speed1.test===
PRAGMA page_size=1024;
PRAGMA cache_size=8192;
PRAGMA locking_mode=EXCLUSIVE;
CREATE TABLE t1(a INTEGER, b INTEGER, c TEXT);
CREATE TABLE t2(a INTEGER, b INTEGER, c TEXT);
CREATE INDEX i2a ON t2(a);
CREATE INDEX i2b ON t2(b);

SELECT name FROM sqlite_master ORDER BY 1;

SELECT c FROM t1 ORDER BY random() LIMIT 50000;

-- ===speed2.test===
PRAGMA page_size=1024;
PRAGMA cache_size=8192;
PRAGMA locking_mode=EXCLUSIVE;
CREATE TABLE t1(a INTEGER, b INTEGER, c TEXT);
CREATE TABLE t2(a INTEGER, b INTEGER, c TEXT);
CREATE INDEX i2a ON t2(a);
CREATE INDEX i2b ON t2(b);

SELECT name FROM sqlite_master ORDER BY 1;

SELECT c FROM t1 ORDER BY random() LIMIT 50000;

SELECT c FROM t1 ORDER BY random() LIMIT 50000;

-- ===speed3.test===
INSERT INTO main.t1 VALUES(ii, text, ii);

PRAGMA incremental_vacuum(500000);

INSERT INTO aux.t1 SELECT * FROM main.t1;

PRAGMA main.cache_size = 200000;
PRAGMA main.auto_vacuum = 'incremental';
ATTACH 'test2.db' AS 'aux'; 
PRAGMA aux.auto_vacuum = 'none';

CREATE TABLE main.t1(a INTEGER, b TEXT, c INTEGER);

SELECT name FROM sqlite_master ORDER BY 1;

CREATE TABLE aux.t1(a INTEGER, b TEXT, c INTEGER);

SELECT name FROM aux.sqlite_master ORDER BY 1;

SELECT count(*) FROM main.t1;
SELECT count(*) FROM aux.t1;

PRAGMA main.auto_vacuum;
PRAGMA aux.auto_vacuum;

-- ===speed4.test===
BEGIN;
CREATE TABLE t1(rowid INTEGER PRIMARY KEY, i INTEGER, t TEXT);
CREATE TABLE t2(rowid INTEGER PRIMARY KEY, i INTEGER, t TEXT);
CREATE TABLE t3(rowid INTEGER PRIMARY KEY, i INTEGER, t TEXT);
CREATE VIEW v1 AS SELECT rowid, i, t FROM t1;
CREATE VIEW v2 AS SELECT rowid, i, t FROM t2;
CREATE VIEW v3 AS SELECT rowid, i, t FROM t3;

CREATE INDEX i1 ON t1(t);
CREATE INDEX i2 ON t2(t);
CREATE INDEX i3 ON t3(t);
COMMIT;

CREATE TABLE log(op TEXT, r INTEGER, i INTEGER, t TEXT);
CREATE TABLE t4(rowid INTEGER PRIMARY KEY, i INTEGER, t TEXT);
CREATE TRIGGER t4_trigger1 AFTER INSERT ON t4 BEGIN
INSERT INTO log VALUES('INSERT INTO t4', new.rowid, new.i, new.t);
END;
CREATE TRIGGER t4_trigger2 AFTER UPDATE ON t4 BEGIN
INSERT INTO log VALUES('UPDATE OF t4', new.rowid, new.i, new.t);
END;
CREATE TRIGGER t4_trigger3 AFTER DELETE ON t4 BEGIN
INSERT INTO log VALUES('DELETE OF t4', old.rowid, old.i, old.t);
END;
BEGIN;

COMMIT;

DROP TABLE t4;
DROP TABLE log;
VACUUM;
CREATE TABLE t4(rowid INTEGER PRIMARY KEY, i INTEGER, t TEXT);
BEGIN;

COMMIT