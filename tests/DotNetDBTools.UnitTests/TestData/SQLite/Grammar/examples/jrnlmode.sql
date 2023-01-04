-- ===jrnlmode.test===
PRAGMA temp.journal_mode;

PRAGMA journal_mode;
PRAGMA main.journal_mode;
PRAGMA temp.journal_mode;

PRAGMA journal_mode = off;
PRAGMA journal_mode = invalid;

PRAGMA journal_mode = PERSIST;
ATTACH ':memory:' as aux1;

PRAGMA main.journal_mode;
PRAGMA aux1.journal_mode;

PRAGMA main.journal_mode = OFF;

PRAGMA main.journal_mode;
PRAGMA temp.journal_mode;
PRAGMA aux1.journal_mode;

PRAGMA journal_mode;

ATTACH ':memory:' as aux2;

PRAGMA main.journal_mode;
PRAGMA aux1.journal_mode;
PRAGMA aux2.journal_mode;

PRAGMA aux1.journal_mode = DELETE;

PRAGMA journal_mode;
PRAGMA main.journal_mode;
PRAGMA temp.journal_mode;

PRAGMA main.journal_mode;
PRAGMA aux1.journal_mode;
PRAGMA aux2.journal_mode;

PRAGMA journal_mode = delete;

PRAGMA main.journal_mode;
PRAGMA temp.journal_mode;
PRAGMA aux1.journal_mode;
PRAGMA aux2.journal_mode;

ATTACH ':memory:' as aux3;

PRAGMA main.journal_mode;
PRAGMA temp.journal_mode;
PRAGMA aux1.journal_mode;
PRAGMA aux2.journal_mode;
PRAGMA aux3.journal_mode;

PRAGMA journal_mode = TRUNCATE;

PRAGMA main.journal_mode;
PRAGMA temp.journal_mode;
PRAGMA aux1.journal_mode;
PRAGMA aux2.journal_mode;
PRAGMA aux3.journal_mode;

DETACH aux1;
DETACH aux2;
DETACH aux3;

ATTACH 'test2.db' AS aux;
PRAGMA main.journal_mode = persist;
PRAGMA aux.journal_mode = persist;
CREATE TABLE abc(a, b, c);
CREATE TABLE aux.def(d, e, f);

BEGIN;
INSERT INTO abc VALUES(1, 2, 3);
INSERT INTO def VALUES(4, 5, 6);
COMMIT;

PRAGMA journal_mode = persist;

SELECT * FROM abc;

SELECT * FROM def;

CREATE TABLE x(n INTEGER); 
ATTACH 'test2.db' AS a; 
create table a.x ( n integer ); 
insert into a.x values(1); 
insert into a.x values (2); 
insert into a.x values (3); 
insert into a.x values (4);

PRAGMA journal_mode=off;

BEGIN IMMEDIATE;
INSERT OR IGNORE INTO main.x SELECT * FROM a.x;
COMMIT;

PRAGMA cache_size = 1;
PRAGMA auto_vacuum = 1;
CREATE TABLE abc(a, b, c);

PRAGMA page_count;

PRAGMA journal_mode = off;

INSERT INTO abc VALUES(1, 2, randomblob(2000));

DELETE FROM abc;

PRAGMA journal_mode;
PRAGMA main.journal_mode;
PRAGMA temp.journal_mode;

pragma page_size=1024;

pragma journal_mode=persist;

PRAGMA journal_size_limit;

ATTACH 'test2.db' AS aux;
PRAGMA aux.journal_mode=persist;
PRAGMA aux.journal_size_limit;

PRAGMA aux.journal_size_limit = 999999999999;

PRAGMA aux.journal_size_limit = 10240;

PRAGMA main.journal_size_limit = 20480;

PRAGMA journal_size_limit;

PRAGMA aux.journal_size_limit;

ATTACH 'test3.db' AS aux2;
PRAGMA aux2.journal_mode=persist;

PRAGMA journal_mode = off;

CREATE TABLE main.t1(a, b, c);
CREATE TABLE aux.t2(a, b, c);
CREATE TABLE aux2.t3(a, b, c);

BEGIN;
INSERT INTO t3 VALUES(randomblob(1000),randomblob(1000),randomblob(1000));
INSERT INTO t3 
SELECT randomblob(1000),randomblob(1000),randomblob(1000) FROM t3;
INSERT INTO t3 
SELECT randomblob(1000),randomblob(1000),randomblob(1000) FROM t3;
INSERT INTO t3 
SELECT randomblob(1000),randomblob(1000),randomblob(1000) FROM t3;
INSERT INTO t3 
SELECT randomblob(1000),randomblob(1000),randomblob(1000) FROM t3;
INSERT INTO t3 
SELECT randomblob(1000),randomblob(1000),randomblob(1000) FROM t3;
INSERT INTO t2 SELECT * FROM t3;
INSERT INTO t1 SELECT * FROM t2;
COMMIT;

BEGIN;
UPDATE t1 SET a = randomblob(1000);

BEGIN;
UPDATE t2 SET a = randomblob(1000);

BEGIN;
UPDATE t3 SET a = randomblob(1000);

PRAGMA journal_size_limit = -4;
BEGIN;
UPDATE t1 SET a = randomblob(1000);

PRAGMA journal_size_limit = 0;
BEGIN;
UPDATE t1 SET a = randomblob(1000);

PRAGMA journal_mode = truncate;
CREATE TABLE t4(a, b);
BEGIN;
INSERT INTO t4 VALUES(1, 2);
PRAGMA journal_mode = memory;

COMMIT;
SELECT * FROM t4;

PRAGMA journal_mode = MEMORY;
BEGIN;
INSERT INTO t4 VALUES(3, 4);

PRAGMA journal_mode;
PRAGMA main.journal_mode;
PRAGMA temp.journal_mode;

COMMIT;
SELECT * FROM t4;

PRAGMA journal_mode = DELETE;
BEGIN IMMEDIATE; INSERT INTO t4 VALUES(1,2); COMMIT;

PRAGMA journal_mode = memory;
PRAGMA auto_vacuum = 0;
PRAGMA page_size = 1024;
PRAGMA user_version = 5;
PRAGMA user_version;

PRAGMA journal_mode = delete;

PRAGMA journal_mode;
PRAGMA main.journal_mode;
PRAGMA Temp.journal_mode;

PRAGMA journal_mode = truncate;

-- ===jrnlmode2.test===
PRAGMA journal_mode = persist;
CREATE TABLE t1(a, b);
INSERT INTO t1 VALUES(1, 2);

SELECT * FROM t1;

INSERT INTO t1 VALUES(3, 4);

BEGIN;
SELECT * FROM t1;

PRAGMA lock_status;

COMMIT;

PRAGMA journal_mode = truncate;

INSERT INTO t1 VALUES(5, 6);

-- ===jrnlmode3.test===
PRAGMA journal_mode=OFF;
PRAGMA locking_mode=EXCLUSIVE;
CREATE TABLE t1(x);
INSERT INTO t1 VALUES(1);
SELECT * FROM t1;

BEGIN;
INSERT INTO t1 VALUES(2);
ROLLBACK;
SELECT * FROM t1;

PRAGMA locking_mode=EXCLUSIVE;
PRAGMA journal_mode=OFF;
CREATE TABLE t1(x);
INSERT INTO t1 VALUES(1);
SELECT * FROM t1;

BEGIN;
INSERT INTO t1 VALUES(2);
ROLLBACK;
SELECT * FROM t1;

PRAGMA main.journal_mode;

CREATE TABLE t1(x);
BEGIN;
INSERT INTO t1 VALUES(cnt);

ROLLBACK;
SELECT * FROM t1;

DROP TABLE IF EXISTS t1;
CREATE TABLE t1(x);
BEGIN;
INSERT INTO t1 VALUES(1);

SELECT * FROM t1;