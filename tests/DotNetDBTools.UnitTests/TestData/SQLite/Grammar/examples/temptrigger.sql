-- ===temptrigger.test===
CREATE TABLE t1(a, b);
CREATE TEMP TABLE tt1(a, b);
CREATE TEMP TRIGGER tr1 AFTER INSERT ON t1 BEGIN
INSERT INTO tt1 VALUES(new.a, new.b);
END;

SELECT * FROM tt1;

DROP TRIGGER tr1;

DELETE FROM t1;
CREATE TEMP TABLE tt1(a, b);
CREATE TEMP TRIGGER tr1 AFTER INSERT ON t1 BEGIN
INSERT INTO tt1 VALUES(new.a, new.b);
END;

INSERT INTO t1 VALUES(10, 20);
SELECT * FROM tt1;

INSERT INTO t1 VALUES(30, 40);
SELECT * FROM tt1;

DROP TRIGGER tr1;

CREATE TABLE t2(a, b);

ATTACH 'test2.db' AS aux;
CREATE TEMP TABLE tt2(a, b);
CREATE TEMP TRIGGER tr2 AFTER INSERT ON t2 BEGIN
INSERT INTO tt2 VALUES(new.a, new.b);
END;

INSERT INTO aux.t2 VALUES(1, 2);
SELECT * FROM aux.t2;

SELECT * FROM tt2;

INSERT INTO t1 VALUES(1, 2);

CREATE TABLE t3(a, b);

INSERT INTO aux.t2 VALUES(3, 4);
SELECT * FROM aux.t2;

SELECT * FROM tt2;

DROP TRIGGER tr2;

SELECT * FROM t1;

SELECT * FROM tt1;

INSERT INTO t1 VALUES(3, 4);

SELECT * FROM t1;

SELECT * FROM tt1;

BEGIN; CREATE TABLE t3(a, b); ROLLBACK;

INSERT INTO t1 VALUES(5, 6)