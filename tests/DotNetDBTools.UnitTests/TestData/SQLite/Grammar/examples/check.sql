-- ===check.test===
CREATE TABLE t1(
x INTEGER CHECK( x<5 ),
y REAL CHECK( y>x )
);

SELECT * FROM t1;

CREATE TABLE t2(
x INTEGER CHECK( typeof(coalesce(x,0))=="integer" ),
y REAL CHECK( typeof(coalesce(y,0.1))=='real' ),
z TEXT CHECK( typeof(coalesce(z,''))=='text' )
);

INSERT INTO t2 VALUES(1,2.2,'three');
SELECT * FROM t2;

INSERT INTO t2 VALUES(NULL, NULL, NULL);
SELECT * FROM t2;

SELECT name FROM sqlite_master ORDER BY name;

SELECT name FROM sqlite_master ORDER BY name;

SELECT name FROM sqlite_master ORDER BY name;

INSERT INTO t3 VALUES(1,2,3);
SELECT * FROM t3;

CREATE TABLE t4(x, y,
CHECK (
x+y==11
OR x*y==12
OR x/y BETWEEN 5 AND 8
OR -x==y+10
)
);

INSERT INTO t4 VALUES(1,10);
SELECT * FROM t4;

INSERT INTO t1 VALUES(3,4);
SELECT * FROM t1;

UPDATE t4 SET x=4, y=3;
SELECT * FROM t4;

UPDATE t4 SET x=12, y=2;
SELECT * FROM t4;

UPDATE t4 SET x=12, y=-22;
SELECT * FROM t4;

SELECT * FROM t4;

PRAGMA ignore_check_constraints=ON;
UPDATE t4 SET x=0, y=1;
SELECT * FROM t4;

SELECT * FROM t1;

UPDATE OR IGNORE t1 SET x=5;
SELECT * FROM t1;

INSERT OR IGNORE INTO t1 VALUES(5,4.0);
SELECT * FROM t1;

INSERT OR IGNORE INTO t1 VALUES(2,20.0);
SELECT * FROM t1;

SELECT * FROM t1;

SELECT * FROM t1;

SELECT * FROM t1;

SELECT * FROM t1;

SELECT * FROM t1;

SELECT * FROM t1;

SELECT * FROM t1;

SELECT * FROM t1;

SELECT * FROM t1;

DELETE FROM t1 WHERE x IS NULL OR x!=3;
UPDATE t1 SET x=2 WHERE x==3;
SELECT * FROM t1;

SELECT * FROM t1;

SELECT * FROM t1;