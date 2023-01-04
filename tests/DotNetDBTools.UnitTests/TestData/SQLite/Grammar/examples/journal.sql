-- ===journal1.test===
CREATE TABLE t1(a,b);
INSERT INTO t1 VALUES(1,randstr(10,400));
INSERT INTO t1 VALUES(2,randstr(10,400));
INSERT INTO t1 SELECT a+2, a||b FROM t1;
INSERT INTO t1 SELECT a+4, a||b FROM t1;
SELECT count(*) FROM t1;

BEGIN;
DELETE FROM t1;

ROLLBACK;

-- ===journal2.test===
CREATE TABLE t1(a, b);

CREATE TABLE t2(a UNIQUE, b UNIQUE);
INSERT INTO t2 VALUES(a_string(200), a_string(300));
INSERT INTO t2 SELECT a_string(200), a_string(300) FROM t2;  INSERT INTO t2 SELECT a_string(200), a_string(300) FROM t2;  INSERT INTO t2 SELECT a_string(200), a_string(300) FROM t2;  INSERT INTO t2 SELECT a_string(200), a_string(300) FROM t2;  INSERT INTO t2 SELECT a_string(200), a_string(300) FROM t2;  INSERT INTO t2 SELECT a_string(200), a_string(300) FROM t2;  -- 64;

PRAGMA cache_size = 10;
BEGIN;
INSERT INTO t2 SELECT randomblob(200), randomblob(300) FROM t2;  -- 128;

SELECT count(*) FROM t2;
PRAGMA integrity_check;

PRAGMA journal_mode = persist;

CREATE TABLE t1(x);
INSERT INTO t1 VALUES(3.14159);

PRAGMA journal_mode = WAL;

PRAGMA journal_mode = truncate;
INSERT INTO t1 VALUES(1, 2);

INSERT INTO t1 VALUES(3, 4);

SELECT * FROM t1;

PRAGMA journal_mode = delete;

SELECT * FROM t1;

PRAGMA journal_mode = truncate;

INSERT INTO t1 VALUES(5, 6);

SELECT * FROM t1;

-- ===journal3.test===
CREATE TABLE tx(y, z);

BEGIN;
INSERT INTO tx DEFAULT VALUES;