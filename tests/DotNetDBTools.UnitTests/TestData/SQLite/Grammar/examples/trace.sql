-- ===trace.test===
CREATE TABLE t1(a,b);
INSERT INTO t1 VALUES(1,2);
SELECT * FROM t1;

CREATE TABLE t6([t6int],"?1"); INSERT INTO t6 VALUES(1,2);

SELECT 't6int', [t6int], t6int, ?1, "?1", t6int FROM t6;

PRAGMA encoding=UTF16be;
CREATE TABLE t6([t6str],"?1");
INSERT INTO t6 VALUES(1,2);

SELECT 't6str', [t6str], t6str, ?1, "?1", t6str FROM t6;

PRAGMA encoding=UTF16le;
CREATE TABLE t6([t6str],"?1");
INSERT INTO t6 VALUES(1,2);

SELECT 't6str', [t6str], t6str, ?1, "?1", t6str FROM t6;

SELECT * FROM t1;

CREATE TABLE t2(a,b);
INSERT INTO t2 VALUES(1,2);
SELECT * FROM t2;

SELECT * FROM t1;

CREATE TRIGGER r1t1 AFTER UPDATE ON t1 BEGIN
UPDATE t2 SET a=new.a WHERE rowid=new.rowid;
END;
CREATE TRIGGER r1t2 AFTER UPDATE ON t2 BEGIN
SELECT 'hello';
END;

UPDATE t1 SET a=a+1;

SELECT x'3031323334' AS x;

SELECT t6int, t6real, t6str, t6blob, t6null;

SELECT t6int, ?1, t6int