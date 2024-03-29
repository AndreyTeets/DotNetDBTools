-- ===createtab.test===
PRAGMA page_size=1024;
CREATE TABLE t1(x INTEGER PRIMARY KEY, y);
INSERT INTO t1 VALUES(1, hex(randomblob(200)));
INSERT INTO t1 VALUES(2, hex(randomblob(200)));
INSERT INTO t1 VALUES(3, hex(randomblob(200)));
INSERT INTO t1 VALUES(4, hex(randomblob(200)));
SELECT count(*) FROM t1;

PRAGMA encoding;

CREATE TABLE t2(a,b);
INSERT INTO t2 VALUES(1,2);
SELECT * FROM t2;

CREATE TABLE t3(a,b);
INSERT INTO t3 VALUES(4,5);
SELECT * FROM t3;

CREATE TABLE t4(a,b);
INSERT INTO t4 VALUES('abc','xyz');
SELECT * FROM t4;

SELECT name FROM sqlite_master WHERE type='table' ORDER BY 1