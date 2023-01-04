-- ===walcrash.test===
SELECT sum(a)==max(b) FROM t1;

SELECT * FROM t1 WHERE a = 1;

PRAGMA main.integrity_check;

PRAGMA main.journal_mode;

SELECT count(*)==33 OR count(*)==34 FROM t1 WHERE x != 1;

PRAGMA main.integrity_check;

PRAGMA main.journal_mode;

SELECT count(*)==34 OR count(*)==35 FROM t1 WHERE x != 1;

PRAGMA main.integrity_check;

PRAGMA main.journal_mode;

SELECT b FROM t1 WHERE a = 1;

SELECT sum(a)==max(b) FROM t1;

PRAGMA main.integrity_check;

PRAGMA main.journal_mode;

PRAGMA main.journal_mode;

SELECT sum(a)==max(b) FROM t1;

SELECT sum(a)==max(b) FROM t1;

PRAGMA main.journal_mode;

ATTACH 'test2.db' AS aux;
SELECT * FROM t1 EXCEPT SELECT * FROM t2;

PRAGMA main.integrity_check;

PRAGMA aux.integrity_check;

-- ===walcrash2.test===
PRAGMA page_size = 1024;
PRAGMA auto_vacuum = off;
PRAGMA journal_mode = WAL;
PRAGMA synchronous = NORMAL;
BEGIN;
CREATE TABLE t1(x);
CREATE TABLE t2(x);
CREATE TABLE t3(x);
CREATE TABLE t4(x);
CREATE TABLE t5(x);
CREATE TABLE t6(x);
CREATE TABLE t7(x);
COMMIT;

PRAGMA cache_size = 15;
BEGIN;
INSERT INTO t1 VALUES(randomblob(900));         INSERT INTO t1 SELECT * FROM t1;                INSERT INTO t1 SELECT * FROM t1;                INSERT INTO t1 SELECT * FROM t1;                INSERT INTO t1 SELECT * FROM t1;                INSERT INTO t1 SELECT * FROM t1 LIMIT 3;        -- 20 rows, 20 pages;

SELECT count(*) FROM t1