-- ===table.test===
CREATE TABLE test1 (
one varchar(10),
two text
);

DROP TABLE "create";

SELECT name FROM "sqlite_master" WHERE type!='meta';

CREATE TABLE test1("f1 ho" int);

SELECT name as "X" FROM sqlite_master WHERE type!='meta';

DROP TABLE "TEST1";

SELECT name FROM "sqlite_master" WHERE type!='meta';

CREATE TABLE TEST2(one text);

CREATE TABLE sqlite_master(two text);

CREATE TABLE sqlite_master(two text);

DROP TABLE test2; SELECT name FROM sqlite_master WHERE type!='meta';

SELECT sql FROM sqlite_master WHERE type!='meta';

CREATE TABLE test2(one text);

CREATE INDEX test3 ON test2(one);

CREATE TABLE test3(two text);

SELECT name FROM sqlite_master WHERE type!='meta' ORDER BY name;

DROP INDEX test3;

CREATE TABLE test3(two text);

SELECT name FROM sqlite_master WHERE type!='meta' ORDER BY name;

DROP TABLE test2; DROP TABLE test3;

SELECT name FROM sqlite_master WHERE type!='meta' ORDER BY name;

SELECT sql FROM sqlite_master WHERE type=='table';

SELECT name, tbl_name, type FROM sqlite_master WHERE type!='meta';

CREATE TABLE BIG(xyz foo);

CREATE TABLE biG(xyz foo);

CREATE TABLE bIg(xyz foo);

CREATE TABLE Big(xyz foo);

DROP TABLE big;

SELECT name FROM sqlite_master WHERE type!='meta';

SELECT name FROM sqlite_master WHERE type!='meta' ORDER BY name;

SELECT name FROM sqlite_master WHERE type!='meta' ORDER BY name;

pragma vdbe_trace=on;

SELECT name FROM sqlite_master WHERE type!='meta' ORDER BY name;

SELECT name, tbl_name, type from sqlite_master WHERE type!='meta';

SELECT name FROM sqlite_master WHERE type!='meta' ORDER BY name;

EXPLAIN CREATE TABLE test1(f1 int);

SELECT name FROM sqlite_master WHERE type!='meta';

CREATE TABLE test1(f1 int);

EXPLAIN DROP TABLE test1;

SELECT name FROM sqlite_master WHERE type!='meta';

CREATE TABLE 'Spaces In This Name!'(x int);

INSERT INTO 'spaces in this name!' VALUES(1);

CREATE TABLE weird(
desc text,
asc text,
key int,
[14_vac] boolean,
fuzzy_dog_12 varchar(10),
begin blob,
end clob
);

INSERT INTO weird VALUES('a','b',9,0,'xyz','hi','y''all');
SELECT * FROM weird;

DROP TABLE test1;

CREATE TABLE savepoint(release);
INSERT INTO savepoint(release) VALUES(10);
UPDATE savepoint SET release = 5;
SELECT release FROM savepoint;

SELECT sql FROM sqlite_master WHERE name='t2';

CREATE TABLE "t3""xyz"(a,b,c);
INSERT INTO [t3"xyz] VALUES(1,2,3);
SELECT * FROM [t3"xyz];

SELECT sql FROM sqlite_master WHERE name='t4"abc';

CREATE TABLE t10("col.1" [char.3]);
CREATE TABLE t11 AS SELECT * FROM t10;
SELECT sql FROM sqlite_master WHERE name = 't11';

CREATE TABLE t12(
a INTEGER,
b VARCHAR(10),
c VARCHAR(1,10),
d VARCHAR(+1,-10),
e VARCHAR (+1,-10),
f "VARCHAR (+1,-10, 5)",
g BIG INTEGER
);
CREATE TABLE t13 AS SELECT * FROM t12;
SELECT sql FROM sqlite_master WHERE name = 't13';

CREATE TABLE t7(
a integer primary key,
b number(5,10),
c character varying (8),
d VARCHAR(9),
e clob,
f BLOB,
g Text,
h
);
INSERT INTO t7(a) VALUES(1);
SELECT typeof(a), typeof(b), typeof(c), typeof(d),
typeof(e), typeof(f), typeof(g), typeof(h)
FROM t7 LIMIT 1;

SELECT typeof(a+b), typeof(a||b), typeof(c+d), typeof(c||d)
FROM t7 LIMIT 1;

CREATE TABLE t8 AS SELECT b, h, a as i, (SELECT f FROM t7) as j FROM t7;

CREATE TABLE t8 AS SELECT b, h, a as i, f as j FROM t7;

SELECT * FROM sqlite_master WHERE type!='meta';

SELECT sql FROM sqlite_master WHERE tbl_name = 't8';

CREATE TABLE tablet8(
a integer primary key,
tm text DEFAULT CURRENT_TIME,
dt text DEFAULT CURRENT_DATE,
dttm text DEFAULT CURRENT_TIMESTAMP
);
SELECT * FROM tablet8;

pragma vdbe_trace = 0;

SELECT * FROM tablet8 LIMIT 1;

CREATE TABLE t9(a, b, c);

SELECT * FROM tablet8 LIMIT 1;

DROP TABLE t9;

ATTACH 'test2.db' as aux;

SELECT * FROM tablet8 LIMIT 1;

CREATE TABLE aux.t1(a, b, c);

SELECT name FROM sqlite_master WHERE type!='meta';

SELECT * FROM tablet8 LIMIT 1;

DROP TABLE aux.t1;

BEGIN;

COMMIT;

BEGIN;

COMMIT;

CREATE TABLE "create" (f1 int);

SELECT name FROM sqlite_master WHERE type!='meta'