-- ===lock.test===
SELECT name FROM sqlite_master WHERE type='table' ORDER BY name;

BEGIN TRANSACTION;

UPDATE t1 SET a = 0 WHERE 0;

SELECT * FROM t1;

ROLLBACK;

CREATE TABLE t2(x int, y int);

INSERT INTO t2 VALUES(8,9);

SELECT * FROM t2;

SELECT * FROM t1;

SELECT * FROM t1;

SELECT * FROM t1;

SELECT name FROM sqlite_master WHERE type='table' ORDER BY name;

SELECT * FROM t2;

SELECT * FROM t1;

UPDATE t1 SET a=b, b=a;

SELECT * FROM t1;

UPDATE t2 SET x=y, y=x;

SELECT * FROM t2;

SELECT * FROM t1;

SELECT a FROM t1;

SELECT * FROM t1;

BEGIN TRANSACTION;

CREATE TABLE t1(a int, b int);

UPDATE t1 SET a = 0 WHERE 0;

BEGIN TRANSACTION;

UPDATE t1 SET a = 0 WHERE 0;

ROLLBACK;

UPDATE t1 SET a=b, b=a;

BEGIN; SELECT rowid FROM sqlite_master LIMIT 1;

UPDATE t1 SET a=b, b=a;

ROLLBACK;

UPDATE t1 SET a=b, b=a;

BEGIN; SELECT rowid FROM sqlite_master LIMIT 1;

SELECT name FROM sqlite_master WHERE type='table' ORDER BY name;

UPDATE t1 SET a=b, b=a;

ROLLBACK;

SELECT * FROM t1;

ROLLBACK;

UPDATE t1 SET a = 0 WHERE 0;

BEGIN TRANSACTION;

BEGIN TRANSACTION;

ROLLBACK;

UPDATE t1 SET a=0 WHERE 0;

ROLLBACK;

INSERT INTO t1 VALUES(1,2);

SELECT * FROM t1;

CREATE TEMP TABLE t3(x);
SELECT * FROM t3;

SELECT * FROM t3;

SELECT * FROM t1;

SELECT * FROM t3;

CREATE TABLE t4(a PRIMARY KEY, b);
INSERT INTO t4 VALUES(1, 'one');
INSERT INTO t4 VALUES(2, 'two');
INSERT INTO t4 VALUES(3, 'three');

DELETE FROM t4;

SELECT * FROM sqlite_master;

SELECT * FROM t4;

BEGIN;
INSERT INTO t4 VALUES(1, 'one');
INSERT INTO t4 VALUES(2, 'two');
INSERT INTO t4 VALUES(3, 'three');
COMMIT;

SELECT * FROM t1;

SELECT * FROM t4;

SELECT a FROM t4 ORDER BY a;

PRAGMA integrity_check;

PRAGMA lock_status;

PRAGMA journal_mode = truncate;
BEGIN;
UPDATE t4 SET a = 10 WHERE 0;
COMMIT;

PRAGMA lock_status;

UPDATE t1 SET a=b, b=a;

SELECT * FROM t1;

SELECT * FROM t1;

-- ===lock2.test===
select * from sqlite_master;

pragma lock_status;

BEGIN;
CREATE TABLE abc(a, b, c);

BEGIN;
SELECT * FROM sqlite_master;

CREATE TABLE def(d, e, f);

SELECT * FROM sqlite_master;
COMMIT;

BEGIN;
SELECT * FROM sqlite_master;

SELECT * FROM sqlite_master;

SELECT * FROM sqlite_master;

-- ===lock3.test===
CREATE TABLE t1(a);
INSERT INTO t1 VALUES(1);

END TRANSACTION;

SELECT * FROM t1;

BEGIN DEFERRED TRANSACTION;

INSERT INTO t1 VALUES(2);

END TRANSACTION;

SELECT * FROM t1;

BEGIN IMMEDIATE TRANSACTION;

END TRANSACTION;

BEGIN EXCLUSIVE TRANSACTION;

-- ===lock4.test===
PRAGMA auto_vacuum=OFF;
CREATE TABLE t1(x);

INSERT INTO t1 VALUES(2);

BEGIN EXCLUSIVE;
INSERT INTO t1 VALUES(1);

COMMIT;

-- ===lock5.test===
BEGIN;
CREATE TABLE t1(a, b);

SELECT * FROM t1;

BEGIN;
SELECT * FROM t1;

SELECT * FROM t1;

SELECT * FROM t1;

INSERT INTO t1 VALUES('a', 'b');
SELECT * FROM t1;

BEGIN;
SELECT * FROM t1;

SELECT * FROM t1;
ROLLBACK;

BEGIN EXCLUSIVE;

CREATE TABLE t1(a, b);
BEGIN;
INSERT INTO t1 VALUES(1, 2);

SELECT * FROM t1;

BEGIN;
INSERT INTO t1 VALUES(3, 4);

SELECT * FROM t1;

-- ===lock6.test===
PRAGMA lock_proxy_file=":auto:";
select * from sqlite_master;
PRAGMA lock_proxy_file;

pragma lock_status;

select * from sqlite_master;

PRAGMA lock_proxy_file=":auto:";
PRAGMA lock_proxy_file;

PRAGMA lock_proxy_file;

BEGIN;
SELECT * FROM sqlite_master;

PRAGMA lock_proxy_file="mine";
select * from sqlite_master;

-- ===lock7.test===
CREATE TABLE t1(a, b);

PRAGMA lock_status;

PRAGMA lock_status;

PRAGMA lock_status;

PRAGMA lock_status;

COMMIT