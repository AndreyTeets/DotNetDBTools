-- ===unique.test===
SELECT * FROM t1 ORDER BY a;

SELECT * FROM t1 ORDER BY a;

SELECT * FROM t1 ORDER BY a;

DROP TABLE t1;
CREATE TABLE t2(a int, b int);
INSERT INTO t2(a,b) VALUES(1,2);
INSERT INTO t2(a,b) VALUES(3,4);
SELECT * FROM t2 ORDER BY a;

CREATE TABLE t4(a UNIQUE, b, c, UNIQUE(b,c));
INSERT INTO t4 VALUES(1,2,3);
INSERT INTO t4 VALUES(NULL, 2, NULL);
SELECT * FROM t4;

SELECT * FROM t4;

SELECT * FROM t4;

SELECT * FROM t4;

CREATE TABLE t5(
first_column_with_long_name,
second_column_with_long_name,
third_column_with_long_name,
fourth_column_with_long_name,
fifth_column_with_long_name,
sixth_column_with_long_name,
UNIQUE(
first_column_with_long_name,
second_column_with_long_name,
third_column_with_long_name,
fourth_column_with_long_name,
fifth_column_with_long_name,
sixth_column_with_long_name
)
);
INSERT INTO t5 VALUES(1,2,3,4,5,6);
SELECT * FROM t5;