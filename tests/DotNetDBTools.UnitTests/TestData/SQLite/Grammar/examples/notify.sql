-- ===notify1.test===
CREATE TABLE t1(a, b);

SELECT * FROM t1;

CREATE TABLE t2(a, b);
INSERT INTO t2 VALUES('I', 'II');

BEGIN;
INSERT INTO t1 VALUES(5, 6);

BEGIN;
SELECT * FROM t2;

CREATE TABLE main.t1(a, b);
CREATE TABLE aux2.t2(a, b);
CREATE TABLE aux3.t3(a, b);

BEGIN ; INSERT INTO t1 VALUES(1, 2);

BEGIN ; INSERT INTO t2 VALUES(1, 2);

BEGIN ; INSERT INTO t3 VALUES(1, 2);

COMMIT;

COMMIT;

BEGIN;
INSERT INTO t1 VALUES(1, 2);

COMMIT;

BEGIN;
INSERT INTO t1 VALUES('a', 'b');

COMMIT;

CREATE TABLE t1(a, b);
BEGIN;
INSERT INTO t1 VALUES('a', 'b');

ATTACH 'test2.db' AS two;

CREATE TABLE t1(a, b);
CREATE TABLE two.t2(a, b);

BEGIN;
INSERT INTO t1 VALUES(1, 2);

BEGIN;
INSERT INTO t2 VALUES(1, 2);

COMMIT;

BEGIN;
INSERT INTO t1 VALUES(3, 4);

SELECT * FROM t1;

COMMIT;

BEGIN;
INSERT INTO t2 VALUES(3, 4);

COMMIT;

COMMIT;

BEGIN;
INSERT INTO t1 VALUES(5, 6);

COMMIT;

BEGIN;
INSERT INTO t1 VALUES(7, 8);

COMMIT;
BEGIN EXCLUSIVE;

COMMIT;

CREATE TABLE t2(a, b);
BEGIN;
SELECT * FROM t1;

COMMIT;

SELECT * FROM t1;

COMMIT;

SELECT * FROM t2;

COMMIT;

BEGIN;
SELECT * FROM t1;

SELECT * FROM t1;

COMMIT;

SELECT * FROM t2;

COMMIT;

SELECT * FROM t1;

DROP TABLE t1; CREATE TABLE t1(a, b);

BEGIN;
INSERT INTO t1 VALUES(1, 2);

SELECT * FROM t1;

COMMIT;

-- ===notify2.test===
ATTACH 'test2.db' AS aux2;
ATTACH 'test3.db' AS aux3;
CREATE TABLE main.t1(a INTEGER PRIMARY KEY, b);
CREATE TABLE aux2.t2(a INTEGER PRIMARY KEY, b);
CREATE TABLE aux3.t3(a INTEGER PRIMARY KEY, b);
INSERT INTO t1 SELECT NULL, 0;
INSERT INTO t2 SELECT NULL, 0;
INSERT INTO t3 SELECT NULL, 0;

ATTACH 'test2.db' AS aux2;
ATTACH 'test3.db' AS aux3;

SELECT (SELECT max(a) FROM t1)
+ (SELECT max(a) FROM t2)
+ (SELECT max(a) FROM t3);

-- ===notify3.test===
CREATE TABLE t1(a, b); 
INSERT INTO t1 VALUES('t1 A', 't1 B');

CREATE TABLE t2(a, b);
INSERT INTO t2 VALUES('t2 A', 't2 B');

BEGIN EXCLUSIVE;
INSERT INTO t2 VALUES('t2 C', 't2 D');

COMMIT