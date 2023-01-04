-- ===incrblob.test===
CREATE TABLE blobs(k PRIMARY KEY, v BLOB);
INSERT INTO blobs VALUES('one', X'0102030405060708090A');
INSERT INTO blobs VALUES('two', X'0A090807060504030201');

CREATE VIEW blobs_view AS SELECT k, v, i FROM blobs;

CREATE VIRTUAL TABLE blobs_echo USING echo(blobs);

ATTACH 'test2.db' AS aux;
CREATE TABLE aux.files(name, text);
INSERT INTO aux.files VALUES('this one', zeroblob(size));

BEGIN;
INSERT INTO blobs(k, v, i) VALUES('a', 'different', 'connection');

SELECT rowid FROM blobs;

SELECT * FROM blobs WHERE rowid = 4;

SELECT * FROM blobs WHERE rowid = 4;

BEGIN;
DROP TABLE blobs;
CREATE TABLE t1 (a, b, c, d BLOB);
INSERT INTO t1(a, b, c, d) VALUES(1, 2, 3, 4);
COMMIT;

UPDATE t1 SET d = zeroblob(10000);

UPDATE t1 SET d = 15;

SELECT v FROM blobs WHERE rowid = 1;

SELECT d FROM t1;

SELECT d FROM t1;

PRAGMA auto_vacuum = "incremental";
CREATE TABLE t1(a INTEGER PRIMARY KEY, b);        INSERT INTO t1 VALUES(123, data);

CREATE TABLE t2(a INTEGER PRIMARY KEY, b);        -- root@page4;

SELECT rootpage FROM sqlite_master;

INSERT INTO t2 VALUES(456, otherdata);

DELETE FROM t1 WHERE a = 123;
PRAGMA INCREMENTAL_VACUUM(0);

INSERT INTO t1 VALUES(314159, 'sqlite');

SELECT b FROM t1 WHERE a = 314159;

SELECT b FROM t1 WHERE a = 314159;

INSERT INTO blobs(rowid, k, v) VALUES(3, 'three', str);

BEGIN;
CREATE TABLE blobs(k PRIMARY KEY, v BLOB, i INTEGER);
DELETE FROM blobs;
INSERT INTO blobs VALUES('one', str || randstr(500,500), 45);
COMMIT;

PRAGMA auto_vacuum;

SELECT i FROM blobs;

INSERT INTO blobs(k, v, i) VALUES(123, 567.765, NULL);

INSERT INTO blobs(k, v, i) VALUES(X'010203040506070809', 'hello', 'world');

CREATE TABLE t3(a INTEGER PRIMARY KEY, b);
INSERT INTO t3 VALUES(1, 2);

-- ===incrblob2.test===
CREATE TABLE blobs(id INTEGER PRIMARY KEY, data BLOB);
INSERT INTO blobs VALUES(NULL, zeroblob(5000));
INSERT INTO blobs VALUES(NULL, zeroblob(5000));
INSERT INTO blobs VALUES(NULL, zeroblob(5000));
INSERT INTO blobs VALUES(NULL, zeroblob(5000));

INSERT INTO t1 SELECT NULL, data FROM t1;

DELETE FROM t1 WHERE id >=1 AND id <= 25;

DELETE FROM t1;

INSERT INTO t1 VALUES(1, 'abcde');

PRAGMA read_uncommitted=1;

DELETE FROM t1;
INSERT INTO t1 VALUES(1, zeroblob(100));

CREATE TABLE t2(B BLOB);
INSERT INTO t2 VALUES(zeroblob(10 * 1024 * 1024));

SELECT rowid FROM t2;

CREATE TABLE t3(a INTEGER UNIQUE, b TEXT);
INSERT INTO t3 VALUES(1, 'aaaaaaaaaaaaaaaaaaaa');
INSERT INTO t3 VALUES(2, 'bbbbbbbbbbbbbbbbbbbb');
INSERT INTO t3 VALUES(3, 'cccccccccccccccccccc');
INSERT INTO t3 VALUES(4, 'dddddddddddddddddddd');
INSERT INTO t3 VALUES(5, 'eeeeeeeeeeeeeeeeeeee');

INSERT INTO blobs VALUES(5, zeroblob(10240));

CREATE TABLE t1(id INTEGER PRIMARY KEY, data BLOB);

INSERT INTO t1 VALUES(ii, data);

UPDATE t1 SET data = data || '' WHERE id = 3;

DELETE FROM t1 WHERE id = 14;

UPDATE t1 SET id = 102 WHERE id = 15;

INSERT OR REPLACE INTO t1 VALUES(92, zeroblob(1000));

UPDATE OR REPLACE t1 SET id = 65 WHERE id = 35;

-- ===incrblob3.test===
UPDATE blobs SET v = '123456789012345678901234567890' WHERE k = 1;

CREATE VIRTUAL TABLE ft USING fts3;
INSERT INTO ft VALUES('rules to open a column to which');

CREATE VIEW v1 AS SELECT * FROM blobs;

CREATE TABLE t1(a, b);
CREATE INDEX i1 ON t1(b);
INSERT INTO t1 VALUES(zeroblob(100), zeroblob(100));

CREATE TABLE p1(a PRIMARY KEY);
CREATE TABLE c1(a, b REFERENCES p1);
PRAGMA foreign_keys = 1;
INSERT INTO p1 VALUES(zeroblob(100));
INSERT INTO c1 VALUES(zeroblob(100), zeroblob(100));

PRAGMA foreign_keys = 0;

CREATE TABLE t2(x)