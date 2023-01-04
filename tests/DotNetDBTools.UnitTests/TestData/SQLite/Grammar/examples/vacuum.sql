-- ===vacuum.test===
BEGIN;
CREATE TABLE t1(a INTEGER PRIMARY KEY, b, c);
INSERT INTO t1 VALUES(NULL,randstr(10,100),randstr(5,50));
INSERT INTO t1 VALUES(123456,randstr(10,100),randstr(5,50));
INSERT INTO t1 SELECT NULL, b||'-'||rowid, c||'-'||rowid FROM t1;
INSERT INTO t1 SELECT NULL, b||'-'||rowid, c||'-'||rowid FROM t1;
INSERT INTO t1 SELECT NULL, b||'-'||rowid, c||'-'||rowid FROM t1;
INSERT INTO t1 SELECT NULL, b||'-'||rowid, c||'-'||rowid FROM t1;
INSERT INTO t1 SELECT NULL, b||'-'||rowid, c||'-'||rowid FROM t1;
INSERT INTO t1 SELECT NULL, b||'-'||rowid, c||'-'||rowid FROM t1;
INSERT INTO t1 SELECT NULL, b||'-'||rowid, c||'-'||rowid FROM t1;
CREATE INDEX i1 ON t1(b,c);
CREATE UNIQUE INDEX i2 ON t1(c,a);
CREATE TABLE t2 AS SELECT * FROM t1;
COMMIT;
DROP TABLE t2;

SELECT * FROM t7 WHERE a=1234567890;

SELECT * FROM t7 WHERE a=1234567890;

INSERT INTO t7 SELECT * FROM t6;
SELECT count(*) FROM t7;

DELETE FROM t7;
SELECT count(*) FROM t7;

PRAGMA empty_result_callbacks=on;
VACUUM;

CREATE TABLE "abc abc"(a, b, c);
INSERT INTO "abc abc" VALUES(1, 2, 3);
VACUUM;

select * from "abc abc";

DELETE FROM "abc abc";
INSERT INTO "abc abc" VALUES(X'00112233', NULL, NULL);
VACUUM;

select count(*) from "abc abc" WHERE a = X'00112233';

CREATE TABLE t1(t);
VACUUM;

VACUUM;

CREATE TABLE t2(t);
CREATE TABLE t3(t);
DROP TABLE t2;
PRAGMA freelist_count;

VACUUM;
pragma integrity_check;

PRAGMA freelist_count;

PRAGMA auto_vacuum;

PRAGMA auto_vacuum = 1;

PRAGMA auto_vacuum;

PRAGMA auto_vacuum = 1;

VACUUM;

PRAGMA auto_vacuum;

CREATE TABLE t1(t);
VACUUM;

VACUUM;

DROP TABLE 'abc abc';
CREATE TABLE autoinc(a INTEGER PRIMARY KEY AUTOINCREMENT, b);
INSERT INTO autoinc(b) VALUES('hi');
INSERT INTO autoinc(b) VALUES('there');
DELETE FROM autoinc;

VACUUM;

INSERT INTO autoinc(b) VALUES('one');
INSERT INTO autoinc(b) VALUES('two');

VACUUM;

COMMIT;

BEGIN;
CREATE TABLE t4 AS SELECT * FROM t1;
CREATE TABLE t5 AS SELECT * FROM t1;
COMMIT;
DROP TABLE t4;
DROP TABLE t5;

BEGIN;
CREATE TABLE t6 AS SELECT * FROM t1;
CREATE TABLE t7 AS SELECT * FROM t1;
COMMIT;

SELECT * FROM sqlite_master;
SELECT * FROM t7 LIMIT 1;

VACUUM;

INSERT INTO t7 VALUES(1234567890,'hello','world');

-- ===vacuum2.test===
CREATE TABLE t1(x INTEGER PRIMARY KEY AUTOINCREMENT, y);
DROP TABLE t1;
VACUUM;

PRAGMA integrity_check;

PRAGMA integrity_check;

pragma auto_vacuum=1;
create table t(a, b);
insert into t values(1, 2);
insert into t values(1, 2);
pragma auto_vacuum=0;
vacuum;
pragma auto_vacuum;

pragma auto_vacuum=1;
vacuum;
pragma auto_vacuum;

pragma integrity_check;

pragma auto_vacuum;

pragma auto_vacuum=2;
vacuum;
pragma auto_vacuum;

pragma integrity_check;

pragma auto_vacuum;

SELECT a, b FROM t1;

CREATE TABLE t1(x);
CREATE TABLE t2(y);
INSERT INTO t1 VALUES(1);

SELECT 1, 2, 3;

SELECT a, b FROM t1 WHERE a<=10;

VACUUM;

pragma page_size;

INSERT INTO t1 VALUES('hello');
INSERT INTO t2 VALUES('out there');

PRAGMA auto_vacuum=FULL;
VACUUM;

PRAGMA integrity_check;

PRAGMA integrity_check;

PRAGMA auto_vacuum=NONE;
VACUUM;

-- ===vacuum3.test===
PRAGMA auto_vacuum=OFF;
PRAGMA page_size = 1024;
CREATE TABLE t1(a, b, c);
INSERT INTO t1 VALUES(1, 2, 3);

PRAGMA page_size;

PRAGMA page_size;

PRAGMA page_size=1024;
CREATE TABLE abc(a, b, c);
INSERT INTO abc VALUES(1, 2, 3);
INSERT INTO abc VALUES(4, 5, 6);

SELECT * FROM abc;

SELECT * FROM abc;

PRAGMA page_size = 2048;
VACUUM;

SELECT * FROM abc;

SELECT * FROM abc;

PRAGMA page_size=16384;
VACUUM;

SELECT * FROM abc;

PRAGMA page_size;

PRAGMA page_size=1024;
VACUUM;

SELECT * FROM abc;

PRAGMA page_size;

SELECT * FROM t1;

PRAGMA page_size = 1024;
VACUUM;
ALTER TABLE t1 ADD COLUMN d;
UPDATE t1 SET d = randomblob(1000);

PRAGMA page_size;

PRAGMA page_size;

SELECT * FROM t1;

SELECT count(*), md5sum(a), md5sum(b), md5sum(c) FROM abc;

-- ===vacuum4.test===
PRAGMA auto_vacuum=FULL;
CREATE TABLE t1(
c000, c001, c002, c003, c004, c005, c006, c007, c008, c009,
c010, c011, c012, c013, c014, c015, c016, c017, c018, c019,
c020, c021, c022, c023, c024, c025, c026, c027, c028, c029,
c030, c031, c032, c033, c034, c035, c036, c037, c038, c039,
c040, c041, c042, c043, c044, c045, c046, c047, c048, c049,
c050, c051, c052, c053, c054, c055, c056, c057, c058, c059,
c060, c061, c062, c063, c064, c065, c066, c067, c068, c069,
c070, c071, c072, c073, c074, c075, c076, c077, c078, c079,
c080, c081, c082, c083, c084, c085, c086, c087, c088, c089,
c090, c091, c092, c093, c094, c095, c096, c097, c098, c099,
c100, c101, c102, c103, c104, c105, c106, c107, c108, c109,
c110, c111, c112, c113, c114, c115, c116, c117, c118, c119,
c120, c121, c122, c123, c124, c125, c126, c127, c128, c129,
c130, c131, c132, c133, c134, c135, c136, c137, c138, c139,
c140, c141, c142, c143, c144, c145, c146, c147, c148, c149
);
CREATE TABLE t2(
c000, c001, c002, c003, c004, c005, c006, c007, c008, c009,
c010, c011, c012, c013, c014, c015, c016, c017, c018, c019,
c020, c021, c022, c023, c024, c025, c026, c027, c028, c029,
c030, c031, c032, c033, c034, c035, c036, c037, c038, c039,
c040, c041, c042, c043, c044, c045, c046, c047, c048, c049,
c050, c051, c052, c053, c054, c055, c056, c057, c058, c059,
c060, c061, c062, c063, c064, c065, c066, c067, c068, c069,
c070, c071, c072, c073, c074, c075, c076, c077, c078, c079,
c080, c081, c082, c083, c084, c085, c086, c087, c088, c089,
c090, c091, c092, c093, c094, c095, c096, c097, c098, c099,
c100, c101, c102, c103, c104, c105, c106, c107, c108, c109,
c110, c111, c112, c113, c114, c115, c116, c117, c118, c119,
c120, c121, c122, c123, c124, c125, c126, c127, c128, c129,
c130, c131, c132, c133, c134, c135, c136, c137, c138, c139,
c140, c141, c142, c143, c144, c145, c146, c147, c148, c149
);
VACUUM;