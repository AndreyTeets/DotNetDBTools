-- ===e_createtable.test===
pragma database_list;

DELETE FROM t12;

CREATE TABLE t2(x PRIMARY KEY);

CREATE TABLE t2(x PRIMARY KEY);

CREATE TABLE t2(a, b, c);

CREATE TABLE t2(x PRIMARY KEY, y);

CREATE TABLE t3(i, j, UNIQUE(i, j) );

DELETE FROM t1;

DELETE FROM t10;

DELETE FROM t11