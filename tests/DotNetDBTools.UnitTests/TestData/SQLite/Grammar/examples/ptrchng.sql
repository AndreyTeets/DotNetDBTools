-- ===ptrchng.test===
CREATE TABLE t1(x INTEGER PRIMARY KEY, y BLOB);
INSERT INTO t1 VALUES(1, 'abc');
INSERT INTO t1 VALUES(2, 
'abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ01234356789');
INSERT INTO t1 VALUES(3, x'626c6f62');
INSERT INTO t1 VALUES(4,
x'000102030405060708090a0b0c0d0e0f101112131415161718191a1b1c1d1e1f2021222324'
);
SELECT count(*) FROM t1;

SELECT pointer_change(y, 'text', 'noop', 'text16') FROM t1 WHERE x=3;

SELECT pointer_change(y, 'blob', 'noop', 'text16') FROM t1 WHERE x=3;

SELECT pointer_change(y, 'text16', 'noop', 'blob') FROM t1 WHERE x=3;

SELECT pointer_change(y, 'text16', 'noop', 'text') FROM t1 WHERE x=3;

SELECT pointer_change(y, 'text', 'noop', 'blob') FROM t1 WHERE x=2;

SELECT pointer_change(y, 'blob', 'noop', 'text') FROM t1 WHERE x=2;

SELECT pointer_change(y, 'text', 'noop', 'text16') FROM t1 WHERE x=2;

SELECT pointer_change(y, 'blob', 'noop', 'text16') FROM t1 WHERE x=2;

SELECT pointer_change(y, 'text16', 'noop', 'blob') FROM t1 WHERE x=2;

SELECT pointer_change(y, 'text16', 'noop', 'text') FROM t1 WHERE x=2;

SELECT pointer_change(y, 'text', 'noop', 'blob') FROM t1 WHERE x=1;

SELECT pointer_change(y, 'text', 'noop', 'blob') FROM t1 WHERE x=4;

SELECT pointer_change(y, 'blob', 'noop', 'text') FROM t1 WHERE x=4;

SELECT pointer_change(y, 'text', 'noop', 'text16') FROM t1 WHERE x=4;

SELECT pointer_change(y, 'blob', 'noop', 'text16') FROM t1 WHERE x=4;

SELECT pointer_change(y, 'text16', 'noop', 'blob') FROM t1 WHERE x=4;

SELECT pointer_change(y, 'text16', 'noop', 'text') FROM t1 WHERE x=4;

SELECT pointer_change(y, 'text', 'bytes', 'text') FROM t1;

SELECT pointer_change(y, 'blob', 'bytes', 'blob') FROM t1;

SELECT pointer_change(y, 'text', 'bytes', 'blob') FROM t1;

SELECT pointer_change(y, 'text16', 'noop', 'blob') FROM t1;

SELECT pointer_change(y, 'blob', 'noop', 'text') FROM t1 WHERE x=1;

SELECT pointer_change(y, 'text16', 'bytes16', 'blob') FROM t1;

SELECT pointer_change(y, 'text', 'noop', 'text16') FROM t1 WHERE x=1;

SELECT pointer_change(y, 'blob', 'noop', 'text16') FROM t1 WHERE x=1;

SELECT pointer_change(y, 'text16', 'noop', 'blob') FROM t1 WHERE x=1;

SELECT pointer_change(y, 'text16', 'noop', 'text') FROM t1 WHERE x=1;

SELECT pointer_change(y, 'text', 'noop', 'blob') FROM t1 WHERE x=3;

SELECT pointer_change(y, 'blob', 'noop', 'text') FROM t1 WHERE x=3