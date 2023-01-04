-- ===avtrans.test===
PRAGMA auto_vacuum=ON;

BEGIN TRANSACTION 'foo';

INSERT INTO t3 SELECT randstr(10,400) FROM t3 WHERE random()%10==0;

ROLLBACK TRANSACTION 'foo';

BEGIN;
SELECT a FROM one ORDER BY a;
SELECT a FROM two ORDER BY a;
END;

BEGIN;
UPDATE one SET a = 0 WHERE 0;
SELECT a FROM one ORDER BY a;

END TRANSACTION;

SELECT a FROM two ORDER BY a;

SELECT a FROM one ORDER BY a;

SELECT a FROM two ORDER BY a;

SELECT a FROM one ORDER BY a;

COMMIT;

CREATE TABLE one(a int PRIMARY KEY, b text);
INSERT INTO one VALUES(1,'one');
INSERT INTO one VALUES(2,'two');
INSERT INTO one VALUES(3,'three');
SELECT b FROM one ORDER BY a;

ROLLBACK;

END TRANSACTION;
SELECT a FROM two ORDER BY a;

SELECT a FROM two ORDER BY a;

SELECT a FROM one ORDER BY a;

DROP TABLE one;
DROP TABLE two;

SELECT name FROM sqlite_master WHERE type='table' ORDER BY name;

BEGIN TRANSACTION;

SELECT name FROM sqlite_master WHERE type='table' ORDER BY name;

CREATE TABLE one(a text, b int);

SELECT name FROM sqlite_master WHERE type='table' ORDER BY name;

CREATE TABLE two(a int PRIMARY KEY, b text);
INSERT INTO two VALUES(1,'I');
INSERT INTO two VALUES(5,'V');
INSERT INTO two VALUES(10,'X');
SELECT b FROM two ORDER BY a;

SELECT a,b FROM one ORDER BY b;

INSERT INTO one(a,b) VALUES('hello', 1);

SELECT a,b FROM one ORDER BY b;

ROLLBACK;

SELECT name FROM sqlite_master WHERE type='table' ORDER BY name;

SELECT a,b FROM one ORDER BY b;

SELECT name fROM sqlite_master 
WHERE type='table' OR type='index'
ORDER BY name;

BEGIN TRANSACTION;
CREATE TABLE t1(a int, b int, c int);
SELECT name fROM sqlite_master 
WHERE type='table' OR type='index'
ORDER BY name;

CREATE INDEX i1 ON t1(a);
SELECT name fROM sqlite_master 
WHERE type='table' OR type='index'
ORDER BY name;

COMMIT;
SELECT name fROM sqlite_master 
WHERE type='table' OR type='index'
ORDER BY name;

SELECT b FROM one ORDER BY a;

BEGIN TRANSACTION;
CREATE TABLE t2(a int, b int, c int);
CREATE INDEX i2a ON t2(a);
CREATE INDEX i2b ON t2(b);
DROP TABLE t1;
SELECT name fROM sqlite_master 
WHERE type='table' OR type='index'
ORDER BY name;

ROLLBACK;
SELECT name fROM sqlite_master 
WHERE type='table' OR type='index'
ORDER BY name;

BEGIN TRANSACTION;
DROP INDEX i1;
SELECT name fROM sqlite_master 
WHERE type='table' OR type='index'
ORDER BY name;

ROLLBACK;
SELECT name fROM sqlite_master 
WHERE type='table' OR type='index'
ORDER BY name;

BEGIN TRANSACTION;
DROP INDEX i1;
CREATE TABLE t2(x int, y int, z int);
CREATE INDEX i2x ON t2(x);
CREATE INDEX i2y ON t2(y);
INSERT INTO t2 VALUES(1,2,3);
SELECT name fROM sqlite_master 
WHERE type='table' OR type='index'
ORDER BY name;

COMMIT;
SELECT name fROM sqlite_master 
WHERE type='table' OR type='index'
ORDER BY name;

SELECT * FROM t2;

SELECT x FROM t2 WHERE y=2;

BEGIN TRANSACTION;
DROP TABLE t1;
DROP TABLE t2;
SELECT name fROM sqlite_master 
WHERE type='table' OR type='index'
ORDER BY name;

SELECT * FROM t2;

SELECT b FROM two ORDER BY a;

ROLLBACK;
SELECT name fROM sqlite_master 
WHERE type='table' OR type='index'
ORDER BY name;

SELECT * FROM t2;

CREATE TABLE t1(a integer primary key,b,c);
INSERT INTO t1 VALUES(1,-2,-3);
INSERT INTO t1 VALUES(4,-5,-6);
SELECT * FROM t1;

CREATE INDEX i1 ON t1(b);
SELECT * FROM t1 WHERE b<1;

BEGIN TRANSACTION;
DROP INDEX i1;
SELECT * FROM t1 WHERE b<1;
ROLLBACK;

SELECT * FROM t1 WHERE b<1;

BEGIN TRANSACTION;
DROP TABLE t1;
ROLLBACK;
SELECT * FROM t1 WHERE b<1;

BEGIN TRANSACTION;
DROP INDEX i1;
CREATE INDEX i1 ON t1(c);
SELECT * FROM t1 WHERE b<1;

SELECT * FROM t1 WHERE c<1;

ROLLBACK;
SELECT * FROM t1 WHERE b<1;

BEGIN;

SELECT * FROM t1 WHERE c<1;

BEGIN TRANSACTION;
DROP TABLE t1;
CREATE TABLE t1(a int unique,b,c);
COMMIT;
INSERT INTO t1 VALUES(1,-2,-3);
INSERT INTO t1 VALUES(4,-5,-6);
SELECT * FROM t1 ORDER BY a;

CREATE INDEX i1 ON t1(b);
SELECT * FROM t1 WHERE b<1;

BEGIN TRANSACTION;
DROP INDEX i1;
SELECT * FROM t1 WHERE b<1;
ROLLBACK;

SELECT * FROM t1 WHERE b<1;

BEGIN TRANSACTION;
DROP TABLE t1;
ROLLBACK;
SELECT * FROM t1 WHERE b<1;

BEGIN TRANSACTION;
DROP INDEX i1;
CREATE INDEX i1 ON t1(c);
SELECT * FROM t1 WHERE b<1;

SELECT * FROM t1 WHERE c<1;

DROP INDEX i1;
SELECT * FROM t1 WHERE c<1;

ROLLBACK;
SELECT * FROM t1 WHERE b<1;

END;

SELECT * FROM t1 WHERE c<1;

BEGIN;

COMMIT;

SELECT md5sum(x,y,z) FROM t2;

SELECT md5sum(type,name,tbl_name,rootpage,sql) FROM sqlite_master;

SELECT count(*) FROM t2;

SELECT md5sum(x,y,z) FROM t2;

SELECT md5sum(type,name,tbl_name,rootpage,sql) FROM sqlite_master;

BEGIN;
DELETE FROM t2;
ROLLBACK;
SELECT md5sum(x,y,z) FROM t2;

BEGIN;
INSERT INTO t2 SELECT * FROM t2;
ROLLBACK;
SELECT md5sum(x,y,z) FROM t2;

BEGIN TRANSACTION;

BEGIN;
DELETE FROM t2;
ROLLBACK;
SELECT md5sum(x,y,z) FROM t2;

BEGIN;
INSERT INTO t2 SELECT * FROM t2;
ROLLBACK;
SELECT md5sum(x,y,z) FROM t2;

BEGIN;
CREATE TABLE t3 AS SELECT * FROM t2;
INSERT INTO t2 SELECT * FROM t3;
ROLLBACK;
SELECT md5sum(x,y,z) FROM t2;

SELECT md5sum(type,name,tbl_name,rootpage,sql) FROM sqlite_master;

BEGIN;
CREATE TEMP TABLE t3 AS SELECT * FROM t2;
INSERT INTO t2 SELECT * FROM t3;
ROLLBACK;
SELECT md5sum(x,y,z) FROM t2;

SELECT md5sum(type,name,tbl_name,rootpage,sql) FROM sqlite_master;

BEGIN;
CREATE TEMP TABLE t3 AS SELECT * FROM t2;
INSERT INTO t2 SELECT * FROM t3;
DROP INDEX i2x;
DROP INDEX i2y;
CREATE INDEX i3a ON t3(x);
ROLLBACK;
SELECT md5sum(x,y,z) FROM t2;

SELECT md5sum(type,name,tbl_name,rootpage,sql) FROM sqlite_master;

BEGIN;
DROP TABLE t2;
ROLLBACK;
SELECT md5sum(x,y,z) FROM t2;

SELECT md5sum(type,name,tbl_name,rootpage,sql) FROM sqlite_master;

COMMIT TRANSACTION;

PRAGMA default_cache_size=20;
BEGIN;
CREATE TABLE t3 AS SELECT * FROM t2;
DELETE FROM t2;

SELECT md5sum(x,y,z) FROM t2;

SELECT md5sum(type,name,tbl_name,rootpage,sql) FROM sqlite_master;

PRAGMA default_cache_size=10;

BEGIN;
CREATE TABLE t3(x TEXT);
INSERT INTO t3 VALUES(randstr(10,400));
INSERT INTO t3 VALUES(randstr(10,400));
INSERT INTO t3 SELECT randstr(10,400) FROM t3;
INSERT INTO t3 SELECT randstr(10,400) FROM t3;
INSERT INTO t3 SELECT randstr(10,400) FROM t3;
INSERT INTO t3 SELECT randstr(10,400) FROM t3;
INSERT INTO t3 SELECT randstr(10,400) FROM t3;
INSERT INTO t3 SELECT randstr(10,400) FROM t3;
INSERT INTO t3 SELECT randstr(10,400) FROM t3;
INSERT INTO t3 SELECT randstr(10,400) FROM t3;
INSERT INTO t3 SELECT randstr(10,400) FROM t3;
COMMIT;
SELECT count(*) FROM t3;

SELECT count(*), md5sum(x) FROM t3;

PRAGMA fullfsync=ON;

PRAGMA fullfsync=OFF;

BEGIN;
DELETE FROM t3 WHERE random()%10!=0;
INSERT INTO t3 SELECT randstr(10,10)||x FROM t3;
INSERT INTO t3 SELECT randstr(10,10)||x FROM t3;
ROLLBACK;

BEGIN;
DELETE FROM t3 WHERE random()%10!=0;
INSERT INTO t3 SELECT randstr(10,10)||x FROM t3;
DELETE FROM t3 WHERE random()%10!=0;
INSERT INTO t3 SELECT randstr(10,10)||x FROM t3;
ROLLBACK;