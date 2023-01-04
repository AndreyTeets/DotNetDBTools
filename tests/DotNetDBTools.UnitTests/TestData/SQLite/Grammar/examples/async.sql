-- ===async2.test===
SELECT name FROM sqlite_master;

SELECT * FROM t1;

SELECT * FROM t2;

SELECT * FROM t1;

SELECT * FROM t2;

-- ===async3.test===
CREATE TABLE abc(a, b, c);
BEGIN;
INSERT INTO abc VALUES(1, 2, 3);

SELECT * FROM abc;

-- ===async4.test===
CREATE TABLE t1(a, b, c);

CREATE TABLE t3(a, b, c);

CREATE INDEX i1 ON t2(a);
CREATE INDEX i2 ON t1(a);

pragma integrity_check;

CREATE TABLE t4(a, b);

CREATE TABLE t5(a, b);

CREATE TABLE t6(a, b);

-- ===async5.test===
ATTACH 'test2.db' AS next;
CREATE TABLE main.t1(a, b);
CREATE TABLE next.t2(a, b);
BEGIN;
INSERT INTO t1 VALUES(1, 2);
INSERT INTO t2 VALUES(3, 4);
COMMIT;

SELECT * FROM t1;

SELECT * FROM t2;

BEGIN;
INSERT INTO t1 VALUES('a', 'b');
INSERT INTO t2 VALUES('c', 'd');
COMMIT;

SELECT * FROM t1;

SELECT * FROM t2