-- ===colname.test===
PRAGMA short_column_names;

PRAGMA full_column_names;

CREATE TABLE tabc(a,b,c);
INSERT INTO tabc VALUES(1,2,3);
CREATE TABLE txyz(x,y,z);
INSERT INTO txyz VALUES(4,5,6);
CREATE TABLE tboth(a,b,c,x,y,z);
INSERT INTO tboth VALUES(11,12,13,14,15,16);
CREATE VIEW v1 AS SELECT tabC.a, txyZ.x, * 
FROM tabc, txyz ORDER BY 1 LIMIT 1;
CREATE VIEW v2 AS SELECT tabC.a, txyZ.x, tboTh.a, tbotH.x, *
FROM tabc, txyz, tboth ORDER BY 1 LIMIT 1;

PRAGMA short_column_names=OFF;
PRAGMA full_column_names=OFF;
CREATE VIEW v3 AS SELECT tabC.a, txyZ.x, *
FROM tabc, txyz ORDER BY 1 LIMIT 1;
CREATE VIEW v4 AS SELECT tabC.a, txyZ.x, tboTh.a, tbotH.x, * 
FROM tabc, txyz, tboth ORDER BY 1 LIMIT 1;

PRAGMA short_column_names=OFF;
PRAGMA full_column_names=ON;
CREATE VIEW v5 AS SELECT tabC.a, txyZ.x, *
FROM tabc, txyz ORDER BY 1 LIMIT 1;
CREATE VIEW v6 AS SELECT tabC.a, txyZ.x, tboTh.a, tbotH.x, * 
FROM tabc, txyz, tboth ORDER BY 1 LIMIT 1;

SELECT x.* FROM sqlite_master X LIMIT 1;

CREATE TABLE t6(a, ['a'], ["a"], "[a]", [`a`]);
INSERT INTO t6 VALUES(1,2,3,4,5);

CREATE TABLE t7(x INTEGER PRIMARY KEY, y);
INSERT INTO t7 VALUES(1,2);

CREATE TABLE "t3893"("x");
INSERT INTO t3893 VALUES(123);
SELECT "y"."x" FROM (SELECT "x" FROM "t3893") AS "y";