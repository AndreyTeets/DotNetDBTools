-- ===exclusive.test===
pragma locking_mode;
pragma main.locking_mode;
pragma temp.locking_mode;

pragma main.locking_mode = normal;

pragma main.locking_mode;
pragma temp.locking_mode;
pragma aux.locking_mode;

pragma locking_mode;

ATTACH 'test3.db' as aux2;

pragma main.locking_mode;
pragma aux.locking_mode;
pragma aux2.locking_mode;

pragma aux.locking_mode = normal;

pragma main.locking_mode;
pragma aux.locking_mode;
pragma aux2.locking_mode;

pragma locking_mode = normal;

pragma main.locking_mode;
pragma temp.locking_mode;
pragma aux.locking_mode;
pragma aux2.locking_mode;

ATTACH 'test4.db' as aux3;

pragma locking_mode = exclusive;

pragma main.locking_mode;
pragma temp.locking_mode;
pragma aux.locking_mode;
pragma aux2.locking_mode;
pragma aux3.locking_mode;

DETACH aux;
DETACH aux2;
DETACH aux3;

CREATE TABLE abc(a, b, c);
INSERT INTO abc VALUES(1, 2, 3);
PRAGMA locking_mode = exclusive;

INSERT INTO abc VALUES(4, 5, 6);
SELECT * FROM abc;

SELECT * FROM abc;

SELECT * FROM abc;

BEGIN;
INSERT INTO abc VALUES(7, 8, 9);

ROLLBACK;

INSERT INTO abc VALUES(7, 8, 9);

PRAGMA locking_mode = normal;

pragma locking_mode;
pragma main.locking_mode;
pragma temp.locking_mode;

SELECT * FROM abc;

SELECT * FROM abc;

PRAGMA locking_mode = exclusive;
BEGIN;
DELETE FROM abc;

COMMIT;

INSERT INTO abc VALUES('A', 'B', 'C');
SELECT * FROM abc;

BEGIN;
UPDATE abc SET a = 1, b = 2, c = 3;
ROLLBACK;
SELECT * FROM abc;

PRAGMA locking_mode = normal;
SELECT * FROM abc;

SELECT count(*), md5sum(x) FROM t3;

PRAGMA locking_mode = exclusive;

PRAGMA default_cache_size = 10;

pragma locking_mode = normal;

BEGIN;
CREATE TABLE t3(x TEXT);
INSERT INTO t3 VALUES(randstr(10,400));
INSERT INTO t3 VALUES(randstr(10,400));
INSERT INTO t3 SELECT randstr(10,400) FROM t3;
INSERT INTO t3 SELECT randstr(10,400) FROM t3;
INSERT INTO t3 SELECT randstr(10,400) FROM t3;
INSERT INTO t3 SELECT randstr(10,400) FROM t3;
COMMIT;

SELECT count(*) FROM t3;

BEGIN;
DELETE FROM t3 WHERE random()%10!=0;
INSERT INTO t3 SELECT randstr(10,10)||x FROM t3;
INSERT INTO t3 SELECT randstr(10,10)||x FROM t3;
SELECT count(*) FROM t3;
ROLLBACK;

BEGIN;
DELETE FROM t3 WHERE random()%10!=0;
INSERT INTO t3 SELECT randstr(10,10)||x FROM t3;
DELETE FROM t3 WHERE random()%10!=0;
INSERT INTO t3 SELECT randstr(10,10)||x FROM t3;
ROLLBACK;

INSERT INTO t3 SELECT randstr(10,400) FROM t3 WHERE random()%10==0;

PRAGMA locking_mode = NORMAL;
DROP TABLE t3;
DROP TABLE abc;

CREATE TABLE abc(a UNIQUE, b UNIQUE, c UNIQUE);
BEGIN;
INSERT INTO abc VALUES(1, 2, 3);
INSERT INTO abc SELECT a+1, b+1, c+1 FROM abc;

COMMIT;

PRAGMA locking_mode = exclusive;
BEGIN;
INSERT INTO abc VALUES(5, 6, 7);

INSERT INTO abc SELECT a+10, b+10, c+10 FROM abc;

pragma locking_mode;
pragma main.locking_mode;
pragma temp.locking_mode;

COMMIT;

PRAGMA locking_mode = normal;
SELECT * FROM abc;

pragma locking_mode = invalid;

pragma locking_mode;
pragma main.locking_mode;
pragma temp.locking_mode;

pragma locking_mode = exclusive;
ATTACH 'test2.db' as aux;

pragma main.locking_mode;
pragma aux.locking_mode;

-- ===exclusive2.test===
SELECT count(*), md5sum(a) FROM t1;

BEGIN;
CREATE TABLE t1(a UNIQUE);
INSERT INTO t1 VALUES(randstr(10, 400));
INSERT INTO t1 VALUES(randstr(10, 400));
COMMIT;

INSERT INTO t1 VALUES(randstr(10, 400));

INSERT INTO t1 VALUES(randstr(10, 400));

PRAGMA locking_mode = exclusive;
INSERT INTO t1 VALUES(randstr(10, 400));

INSERT INTO t1 VALUES(randstr(10, 400));

PRAGMA locking_mode = normal;
INSERT INTO t1 VALUES(randstr(10, 400));

INSERT INTO t1 VALUES(randstr(10, 400));

BEGIN;
CREATE TABLE t1(a, b);
INSERT INTO t1(a) VALUES(randstr(10, 400));
INSERT INTO t1(a) VALUES(randstr(10, 400));
INSERT INTO t1(a) SELECT randstr(10, 400) FROM t1;
INSERT INTO t1(a) SELECT randstr(10, 400) FROM t1;
INSERT INTO t1(a) SELECT randstr(10, 400) FROM t1;
INSERT INTO t1(a) SELECT randstr(10, 400) FROM t1;
INSERT INTO t1(a) SELECT randstr(10, 400) FROM t1;
COMMIT;
SELECT count(*) FROM t1;

PRAGMA cache_size;

UPDATE t1 SET b=a, a=NULL;

PRAGMA cache_size=1000;

PRAGMA locking_mode = exclusive;

BEGIN;
DELETE FROM t1;
INSERT INTO t1(a) VALUES(randstr(10, 400));
INSERT INTO t1(a) VALUES(randstr(10, 400));
INSERT INTO t1(a) SELECT randstr(10, 400) FROM t1;
INSERT INTO t1(a) SELECT randstr(10, 400) FROM t1;
INSERT INTO t1(a) SELECT randstr(10, 400) FROM t1;
INSERT INTO t1(a) SELECT randstr(10, 400) FROM t1;
INSERT INTO t1(a) SELECT randstr(10, 400) FROM t1;
COMMIT;
SELECT count(*) FROM t1;

PRAGMA cache_size;

PRAGMA locking_mode = normal