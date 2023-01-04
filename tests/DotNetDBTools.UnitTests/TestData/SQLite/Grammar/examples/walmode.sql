-- ===walmode.test===
PRAGMA journal_mode = wal;

PRAGMA journal_mode = persist;

SELECT * FROM t1;

SELECT * FROM t1;

PRAGMA main.journal_mode;

PRAGMA main.journal_mode = wal;

SELECT * FROM t1;

PRAGMA main.journal_mode;

PRAGMA journal_mode = delete;

PRAGMA main.journal_mode;

BEGIN;
SELECT * FROM t1;

PRAGMA main.journal_mode = wal;

PRAGMA main.journal_mode;

PRAGMA main.journal_mode;

PRAGMA main.journal_mode;

PRAGMA main.journal_mode;

PRAGMA main.journal_mode = wal;

BEGIN;
CREATE TABLE t1(a, b);
INSERT INTO t1 VALUES(1, 2);
COMMIT;
SELECT * FROM t1;
PRAGMA main.journal_mode;

PRAGMA main.journal_mode = wal;

INSERT INTO t1 VALUES(3, 4);
SELECT * FROM t1;
PRAGMA main.journal_mode;

PRAGMA main.journal_mode;

PRAGMA main.journal_mode = wal;

PRAGMA main.journal_mode;

BEGIN;
CREATE TABLE t1(a, b);
INSERT INTO t1 VALUES(1, 2);
COMMIT;
SELECT * FROM t1;
PRAGMA main.journal_mode;

PRAGMA main.journal_mode = wal;

INSERT INTO t1 VALUES(3, 4);
SELECT * FROM t1;
PRAGMA main.journal_mode;

PRAGMA temp.journal_mode;

PRAGMA temp.journal_mode = wal;

BEGIN;
CREATE TEMP TABLE t1(a, b);
INSERT INTO t1 VALUES(1, 2);
COMMIT;
SELECT * FROM t1;
PRAGMA temp.journal_mode;

PRAGMA temp.journal_mode = wal;

INSERT INTO t1 VALUES(3, 4);
SELECT * FROM t1;
PRAGMA temp.journal_mode;

PRAGMA journal_mode = WAL;
CREATE TABLE t1(a, b);

PRAGMA two.journal_mode=WAL;
PRAGMA two.journal_mode;

PRAGMA page_size = 1024;

PRAGMA main.journal_mode;

PRAGMA journal_mode = wal;

CREATE TABLE t1(a, b);

SELECT * FROM sqlite_master;

PRAGMA journal_mode = wal;

INSERT INTO t1 VALUES(1, 2)