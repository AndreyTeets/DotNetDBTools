-- ===walslow.test===
PRAGMA journal_mode = wal;

SELECT count(*) FROM t1 WHERE a!=b;

SELECT count(*) FROM t1 WHERE a!=b;

CREATE TABLE t1(a, b);

CREATE INDEX i1 ON t1(a);

CREATE INDEX i2 ON t1(b);

INSERT INTO t1 VALUES(randomblob(w), randomblob(x));

PRAGMA integrity_check;

PRAGMA integrity_check;

PRAGMA journal_mode = WAL;

PRAGMA integrity_check