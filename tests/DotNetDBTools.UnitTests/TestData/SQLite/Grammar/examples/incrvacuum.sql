-- ===incrvacuum.test===
pragma auto_vacuum;

pragma auto_vacuum;

pragma auto_vacuum = 'incremental';
pragma auto_vacuum;

pragma auto_vacuum = 'full';
pragma auto_vacuum;

pragma auto_vacuum;

PRAGMA auto_vacuum = 2;
BEGIN;
CREATE TABLE tbl2(str);
INSERT INTO tbl2 VALUES(str);
COMMIT;

DROP TABLE abc;
DELETE FROM tbl2;

PRAGMA auto_vacuum = 1;
INSERT INTO tbl2 VALUES('hello world');

PRAGMA auto_vacuum = 2;
INSERT INTO tbl2 VALUES(str);
CREATE TABLE tbl1(a, b, c);

DELETE FROM tbl2;
DROP TABLE tbl1;

pragma incremental_vacuum(10);

pragma auto_vacuum = 'full';
pragma auto_vacuum;

BEGIN;
DROP TABLE tbl2;
PRAGMA incremental_vacuum;
COMMIT;

BEGIN;
CREATE TABLE tbl1(a);
INSERT INTO tbl1 VALUES(str);
PRAGMA incremental_vacuum;                 COMMIT;

BEGIN;
INSERT INTO tbl1 VALUES(str);
INSERT INTO tbl1 SELECT * FROM tbl1;
DELETE FROM tbl1 WHERE oid%2;        COMMIT;

BEGIN;
PRAGMA incremental_vacuum;           CREATE TABLE tbl2(b);                INSERT INTO tbl2 VALUES('a nice string');
COMMIT;

SELECT * FROM tbl2;

DROP TABLE tbl1;
DROP TABLE tbl2;
PRAGMA incremental_vacuum;

SELECT tbl_name FROM sqlite_master WHERE type = 'table';

SELECT tbl_name FROM sqlite_master WHERE type = 'table';

PRAGMA auto_vacuum = 'none';

PRAGMA auto_vacuum = 'incremental';

pragma auto_vacuum = 'incremental';
pragma auto_vacuum;

PRAGMA integrity_check;

PRAGMA integrity_check;

DROP TABLE IF EXISTS tbl1;
DROP TABLE IF EXISTS tbl2;
PRAGMA incremental_vacuum;
CREATE TABLE tbl1(a, b);
CREATE TABLE tbl2(a, b);
BEGIN;

INSERT INTO tbl1 VALUES(ii, ii || ii);

INSERT INTO tbl2 SELECT * FROM tbl1;
COMMIT;
DROP TABLE tbl1;

SELECT a FROM tbl2;

PRAGMA incremental_vacuum;

DROP TABLE IF EXISTS tbl1;
DROP TABLE IF EXISTS tbl2;
PRAGMA incremental_vacuum;
CREATE TABLE tbl1(a, b);
CREATE TABLE tbl2(a, b);
BEGIN;

INSERT INTO tbl1 VALUES(ii, ii || ii);

INSERT INTO tbl2 SELECT * FROM tbl1;
COMMIT;
DROP TABLE tbl1;

pragma auto_vacuum = 'invalid';
pragma auto_vacuum;

PRAGMA incremental_vacuum;

CREATE TABLE tbl1(a, b);
INSERT INTO tbl1 VALUES('hello', 'world');

SELECT * FROM tbl1;

PRAGMA incremental_vacuum(50);

PRAGMA auto_vacuum = 'incremental';
CREATE TABLE t1(a, b, c);
CREATE TABLE t2(a, b, c);
INSERT INTO t2 VALUES(randstr(500,500),randstr(500,500),randstr(500,500));
INSERT INTO t1 VALUES(1, 2, 3);
INSERT INTO t1 SELECT a||a, b||b, c||c FROM t1;
INSERT INTO t1 SELECT a||a, b||b, c||c FROM t1;
INSERT INTO t1 SELECT a||a, b||b, c||c FROM t1;
INSERT INTO t1 SELECT a||a, b||b, c||c FROM t1;
INSERT INTO t1 SELECT a||a, b||b, c||c FROM t1;
INSERT INTO t1 SELECT a||a, b||b, c||c FROM t1;
INSERT INTO t1 SELECT a||a, b||b, c||c FROM t1;
INSERT INTO t1 SELECT a||a, b||b, c||c FROM t1;

PRAGMA synchronous = 'OFF';
BEGIN;
UPDATE t1 SET a = a, b = b, c = c;
DROP TABLE t2;
PRAGMA incremental_vacuum(10);
ROLLBACK;

PRAGMA cache_size = 10;
BEGIN;
UPDATE t1 SET a = a, b = b, c = c;
DROP TABLE t2;
PRAGMA incremental_vacuum(10);
ROLLBACK;

DROP TABLE t1;
DROP TABLE t2;

PRAGMA incremental_vacuum(1);

PRAGMA incremental_vacuum(5);

pragma auto_vacuum = 1;
pragma auto_vacuum;

PRAGMA incremental_vacuum('1');

PRAGMA incremental_vacuum("+3");

PRAGMA incremental_vacuum = 1;

PRAGMA incremental_vacuum(2147483649);

CREATE TABLE t1(x);
INSERT INTO t1 VALUES(hex(randomblob(1000)));
DROP TABLE t1;

PRAGMA incremental_vacuum=-1;

PRAGMA auto_vacuum;

PRAGMA auto_vacuum;

PRAGMA auto_vacuum = incremental;

PRAGMA auto_vacuum;

pragma auto_vacuum = '2';
pragma auto_vacuum;

PRAGMA auto_vacuum;

PRAGMA auto_vacuum = 'full';
PRAGMA auto_vacuum;

PRAGMA auto_vacuum;

PRAGMA auto_vacuum = 1;

BEGIN EXCLUSIVE;

ROLLBACK;

PRAGMA auto_vacuum;

PRAGMA auto_vacuum;

SELECT * FROM sqlite_master;

PRAGMA auto_vacuum;

pragma auto_vacuum = 5;
pragma auto_vacuum;

PRAGMA auto_vacuum = none;
PRAGMA default_cache_size = 1024;
PRAGMA auto_vacuum;

PRAGMA auto_vacuum;

PRAGMA cache_size = 10;
PRAGMA auto_vacuum = incremental;
CREATE TABLE t1(x, y);
INSERT INTO t1 VALUES('a', str);
INSERT INTO t1 VALUES('b', str);
INSERT INTO t1 VALUES('c', str);
INSERT INTO t1 VALUES('d', str);
INSERT INTO t1 VALUES('e', str);
INSERT INTO t1 VALUES('f', str);
INSERT INTO t1 VALUES('g', str);
INSERT INTO t1 VALUES('h', str);
INSERT INTO t1 VALUES('i', str);
INSERT INTO t1 VALUES('j', str);
INSERT INTO t1 VALUES('j', str);
CREATE TABLE t2(x PRIMARY KEY, y);
INSERT INTO t2 VALUES('a', str);
INSERT INTO t2 VALUES('b', str);
INSERT INTO t2 VALUES('c', str);
INSERT INTO t2 VALUES('d', str);
BEGIN;
DELETE FROM t2;
PRAGMA incremental_vacuum;

COMMIT;
PRAGMA integrity_check;

pragma auto_vacuum = 1;
CREATE TABLE abc(a, b, c);

pragma auto_vacuum = 'none';
pragma auto_vacuum;

-- ===incrvacuum2.test===
PRAGMA page_size=1024;
PRAGMA auto_vacuum=incremental;
CREATE TABLE t1(x);
INSERT INTO t1 VALUES(zeroblob(30000));
DELETE FROM t1;

PRAGMA incremental_vacuum(1);

PRAGMA auto_vacuum = 'full';
BEGIN;
CREATE TABLE abc(a);
INSERT INTO abc VALUES(randstr(1500,1500));
COMMIT;

BEGIN;
DELETE FROM abc;
PRAGMA incremental_vacuum;
COMMIT;

PRAGMA incremental_vacuum(1);

PRAGMA incremental_vacuum(5);

PRAGMA incremental_vacuum(1000);

ATTACH DATABASE 'test2.db' AS aux;
PRAGMA aux.auto_vacuum=incremental;
CREATE TABLE aux.t2(x);
INSERT INTO t2 VALUES(zeroblob(30000));
INSERT INTO t1 SELECT * FROM t2;
DELETE FROM t2;
DELETE FROM t1;

PRAGMA aux.incremental_vacuum(1);

PRAGMA aux.incremental_vacuum(5);

PRAGMA main.incremental_vacuum(5);

PRAGMA aux.incremental_vacuum