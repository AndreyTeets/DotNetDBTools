-- ===walnoshm.test===
PRAGMA locking_mode = exclusive; 
PRAGMA journal_mode = delete;
SELECT * FROM t2;

SELECT * FROM t2;

PRAGMA locking_mode = exclusive;

SELECT * FROM t2;

SELECT * FROM t2;

SELECT * FROM t2;

SELECT * FROM t1;
PRAGMA locking_mode = EXCLUSIVE;
INSERT INTO t1 VALUES(5, 6);
PRAGMA locking_mode = NORMAL;
INSERT INTO t1 VALUES(7, 8);

SELECT * FROM t1;

PRAGMA locking_mode = EXCLUSIVE;
INSERT INTO t1 VALUES(9, 10);
PRAGMA locking_mode = NORMAL;
INSERT INTO t1 VALUES(11, 12);