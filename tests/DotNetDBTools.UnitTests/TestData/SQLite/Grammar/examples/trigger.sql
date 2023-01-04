-- ===trigger1.test===
CREATE TABLE t1(a);

INSERT INTO t1 VALUES(3,4);
SELECT * FROM t1 UNION ALL SELECT * FROM t2;

INSERT INTO t1 VALUES(5,6);
SELECT * FROM t1 UNION ALL SELECT * FROM t2;

INSERT INTO t1 VALUES(3,4);
SELECT * FROM t1; 
SELECT * FROM t2;

INSERT INTO t1 VALUES(5,6);
SELECT * FROM t1;
SELECT * FROM t2;

CREATE TEMP TRIGGER r1 BEFORE INSERT ON t1 BEGIN
INSERT INTO t2 VALUES(NEW.a,NEW.b);
END;
INSERT INTO t1 VALUES(7,8);
SELECT * FROM t2;

INSERT INTO t1 VALUES(9,10);

SELECT * FROM t2;

DROP TABLE t1;
SELECT * FROM t2;

SELECT * FROM t2;

CREATE TABLE t2(x,y);
DROP TABLE t1;
INSERT INTO t2 VALUES(3, 4);
INSERT INTO t2 VALUES(7, 8);

CREATE TRIGGER tr1 INSERT ON t1 BEGIN
INSERT INTO t1 values(1);
END;

SELECT type, name FROM sqlite_master;

CREATE TRIGGER t2 BEFORE DELETE ON t2 BEGIN
SELECT RAISE(ABORT,'deletes are not permitted');
END;
SELECT type, name FROM sqlite_master;

SELECT * FROM t2;

SELECT type, name FROM sqlite_master;

DROP TRIGGER t2;
SELECT type, name FROM sqlite_master;

SELECT * FROM t2;

SELECT * FROM t2;

CREATE TRIGGER 'trigger' AFTER INSERT ON t2 BEGIN SELECT 1; END;
SELECT name FROM sqlite_master WHERE type='trigger';

DROP TRIGGER 'trigger';
SELECT name FROM sqlite_master WHERE type='trigger';

CREATE TRIGGER "trigger" AFTER INSERT ON t2 BEGIN SELECT 1; END;
SELECT name FROM sqlite_master WHERE type='trigger';

BEGIN;
DROP TRIGGER tr2;
ROLLBACK;
DROP TRIGGER tr2;

DROP TRIGGER "trigger";
SELECT name FROM sqlite_master WHERE type='trigger';

CREATE TRIGGER [trigger] AFTER INSERT ON t2 BEGIN SELECT 1; END;
SELECT name FROM sqlite_master WHERE type='trigger';

DROP TRIGGER [trigger];
SELECT name FROM sqlite_master WHERE type='trigger';

CREATE TABLE t3(a,b);
CREATE TABLE t4(x UNIQUE, b);
CREATE TRIGGER r34 AFTER INSERT ON t3 BEGIN
REPLACE INTO t4 VALUES(new.a,new.b);
END;
INSERT INTO t3 VALUES(1,2);
SELECT * FROM t3 UNION ALL SELECT 99, 99 UNION ALL SELECT * FROM t4;

INSERT INTO t3 VALUES(1,3);
SELECT * FROM t3 UNION ALL SELECT 99, 99 UNION ALL SELECT * FROM t4;

CREATE TABLE t3(a,b);
CREATE TABLE t4(x UNIQUE, b);
CREATE TRIGGER r34 AFTER INSERT ON t3 BEGIN
REPLACE INTO t4 VALUES(new.a,new.b);
END;
INSERT INTO t3 VALUES(1,2);
SELECT * FROM t3; SELECT 99, 99; SELECT * FROM t4;

INSERT INTO t3 VALUES(1,3);
SELECT * FROM t3; SELECT 99, 99; SELECT * FROM t4;

DROP TABLE t3;
DROP TABLE t4;

ATTACH 'test2.db' AS aux;

CREATE TABLE main.t4(a, b, c);
CREATE TABLE temp.t4(a, b, c);
CREATE TABLE aux.t4(a, b, c);
CREATE TABLE insert_log(db, a, b, c);

CREATE TEMP TABLE temp_table(a);

CREATE TEMP TRIGGER trig1 AFTER INSERT ON main.t4 BEGIN 
INSERT INTO insert_log VALUES('main', new.a, new.b, new.c);
END;
CREATE TEMP TRIGGER trig2 AFTER INSERT ON temp.t4 BEGIN 
INSERT INTO insert_log VALUES('temp', new.a, new.b, new.c);
END;
CREATE TEMP TRIGGER trig3 AFTER INSERT ON aux.t4 BEGIN 
INSERT INTO insert_log VALUES('aux', new.a, new.b, new.c);
END;

INSERT INTO main.t4 VALUES(1, 2, 3);
INSERT INTO temp.t4 VALUES(4, 5, 6);
INSERT INTO aux.t4  VALUES(7, 8, 9);

SELECT * FROM insert_log;

BEGIN;
INSERT INTO main.t4 VALUES(1, 2, 3);
INSERT INTO temp.t4 VALUES(4, 5, 6);
INSERT INTO aux.t4  VALUES(7, 8, 9);
ROLLBACK;

SELECT * FROM insert_log;

DELETE FROM insert_log;
INSERT INTO main.t4 VALUES(11, 12, 13);
INSERT INTO temp.t4 VALUES(14, 15, 16);
INSERT INTO aux.t4  VALUES(17, 18, 19);

SELECT * FROM insert_log;

DROP TABLE insert_log;
CREATE TABLE aux.insert_log(db, d, e, f);

INSERT INTO main.t4 VALUES(21, 22, 23);
INSERT INTO temp.t4 VALUES(24, 25, 26);
INSERT INTO aux.t4  VALUES(27, 28, 29);

SELECT * FROM insert_log;

CREATE TRIGGER temp_trig UPDATE ON temp_table BEGIN
SELECT * from sqlite_master;
END;
SELECT count(*) FROM sqlite_master WHERE name = 'temp_trig';

CREATE TABLE tA(a INTEGER PRIMARY KEY, b, c);
CREATE TRIGGER tA_trigger BEFORE UPDATE ON "tA" BEGIN SELECT 1; END;
INSERT INTO tA VALUES(1, 2, 3);

CREATE TABLE t16(a,b,c);
CREATE INDEX t16a ON t16(a);
CREATE INDEX t16b ON t16(b);

create table t1(a,b);
insert into t1 values(1,'a');
insert into t1 values(2,'b');
insert into t1 values(3,'c');
insert into t1 values(4,'d');
create trigger r1 after delete on t1 for each row begin
delete from t1 WHERE a=old.a+2;
end;
delete from t1 where a=1 OR a=3;
select * from t1;
drop table t1;

create table t1(a,b);
insert into t1 values(1,'a');
insert into t1 values(2,'b');
insert into t1 values(3,'c');
insert into t1 values(4,'d');
create trigger r1 after update on t1 for each row begin
delete from t1 WHERE a=old.a+2;
end;
update t1 set b='x-' || b where a=1 OR a=3;
select * from t1;
drop table t1;

CREATE TEMP TABLE t2(x,y);

DROP TABLE t2;
CREATE TABLE t2(x,y);
SELECT * FROM t2;

-- ===trigger2.test===
INSERT INTO tbl VALUES(1, 2);
INSERT INTO tbl VALUES(3, 4);
CREATE TABLE rlog (idx, old_a, old_b, db_sum_a, db_sum_b, new_a, new_b);
CREATE TABLE clog (idx, old_a, old_b, db_sum_a, db_sum_b, new_a, new_b);
CREATE TRIGGER before_update_row BEFORE UPDATE ON tbl FOR EACH ROW 
BEGIN
INSERT INTO rlog VALUES ( (SELECT coalesce(max(idx),0) + 1 FROM rlog), 
old.a, old.b, 
(SELECT coalesce(sum(a),0) FROM tbl),
(SELECT coalesce(sum(b),0) FROM tbl), 
new.a, new.b);
END;
CREATE TRIGGER after_update_row AFTER UPDATE ON tbl FOR EACH ROW 
BEGIN
INSERT INTO rlog VALUES ( (SELECT coalesce(max(idx),0) + 1 FROM rlog), 
old.a, old.b, 
(SELECT coalesce(sum(a),0) FROM tbl),
(SELECT coalesce(sum(b),0) FROM tbl), 
new.a, new.b);
END;
CREATE TRIGGER conditional_update_row AFTER UPDATE ON tbl FOR EACH ROW
WHEN old.a = 1
BEGIN
INSERT INTO clog VALUES ( (SELECT coalesce(max(idx),0) + 1 FROM clog), 
old.a, old.b, 
(SELECT coalesce(sum(a),0) FROM tbl),
(SELECT coalesce(sum(b),0) FROM tbl), 
new.a, new.b);
END;

CREATE TABLE tbl (a, b, c, d);
CREATE TABLE log (a);
INSERT INTO log VALUES (0);
INSERT INTO tbl VALUES (0, 0, 0, 0);
INSERT INTO tbl VALUES (1, 0, 0, 0);
CREATE TRIGGER tbl_after_update_cd BEFORE UPDATE OF c, d ON tbl
BEGIN
UPDATE log SET a = a + 1;
END;

UPDATE tbl SET b = 1, c = 10; UPDATE tbl SET b = 10; UPDATE tbl SET d = 4 WHERE a = 0; UPDATE tbl SET a = 4, b = 10; SELECT * FROM log;

DROP TABLE tbl;
DROP TABLE log;

CREATE TABLE tbl (a, b, c, d);
CREATE TABLE log (a);
INSERT INTO log VALUES (0);

INSERT INTO tbl VALUES(0, 0, 0, 0);     SELECT * FROM log;
UPDATE log SET a = 0;
INSERT INTO tbl VALUES(0, 0, 0, 0);     SELECT * FROM log;
UPDATE log SET a = 0;
INSERT INTO tbl VALUES(200, 0, 0, 0);     SELECT * FROM log;
UPDATE log SET a = 0;

DROP TABLE tbl;
DROP TABLE log;

CREATE TABLE tblA(a, b);
CREATE TABLE tblB(a, b);
CREATE TABLE tblC(a, b);
CREATE TRIGGER tr1 BEFORE INSERT ON tblA BEGIN
INSERT INTO tblB values(new.a, new.b);
END;
CREATE TRIGGER tr2 BEFORE INSERT ON tblB BEGIN
INSERT INTO tblC values(new.a, new.b);
END;

INSERT INTO tblA values(1, 2);
SELECT * FROM tblA;
SELECT * FROM tblB;
SELECT * FROM tblC;

DROP TABLE tblA;
DROP TABLE tblB;
DROP TABLE tblC;

CREATE TABLE tbl(a, b, c);
CREATE TRIGGER tbl_trig BEFORE INSERT ON tbl 
BEGIN
INSERT INTO tbl VALUES (new.a, new.b, new.c);
END;

UPDATE tbl SET a = a * 10, b = b * 10;
SELECT * FROM rlog ORDER BY idx;
SELECT * FROM clog ORDER BY idx;

INSERT INTO tbl VALUES (1, 2, 3);
select * from tbl;

DROP TABLE tbl;

CREATE TABLE tbl(a, b, c);
CREATE TRIGGER tbl_trig BEFORE INSERT ON tbl 
BEGIN
INSERT INTO tbl VALUES (1, 2, 3);
INSERT INTO tbl VALUES (2, 2, 3);
UPDATE tbl set b = 10 WHERE a = 1;
DELETE FROM tbl WHERE a = 1;
DELETE FROM tbl;
END;

INSERT INTO tbl VALUES(100, 200, 300);

DROP TABLE tbl;

CREATE TABLE tbl (a primary key, b, c);
CREATE TRIGGER ai_tbl AFTER INSERT ON tbl BEGIN
INSERT OR IGNORE INTO tbl values (new.a, 0, 0);
END;

BEGIN;
INSERT INTO tbl values (1, 2, 3);
SELECT * from tbl;

SELECT * from tbl;

SELECT * from tbl;

INSERT OR REPLACE INTO tbl values (2, 2, 3);
SELECT * from tbl;

DELETE FROM rlog;
DELETE FROM tbl;
INSERT INTO tbl VALUES (100, 100);
INSERT INTO tbl VALUES (300, 200);
CREATE TRIGGER delete_before_row BEFORE DELETE ON tbl FOR EACH ROW
BEGIN
INSERT INTO rlog VALUES ( (SELECT coalesce(max(idx),0) + 1 FROM rlog), 
old.a, old.b, 
(SELECT coalesce(sum(a),0) FROM tbl),
(SELECT coalesce(sum(b),0) FROM tbl), 
0, 0);
END;
CREATE TRIGGER delete_after_row AFTER DELETE ON tbl FOR EACH ROW
BEGIN
INSERT INTO rlog VALUES ( (SELECT coalesce(max(idx),0) + 1 FROM rlog), 
old.a, old.b, 
(SELECT coalesce(sum(a),0) FROM tbl),
(SELECT coalesce(sum(b),0) FROM tbl), 
0, 0);
END;

SELECT * from tbl;

DELETE FROM tbl;

INSERT INTO tbl values (4, 2, 3);
INSERT INTO tbl values (6, 3, 4);
CREATE TRIGGER au_tbl AFTER UPDATE ON tbl BEGIN
UPDATE OR IGNORE tbl SET a = new.a, c = 10;
END;

BEGIN;
UPDATE tbl SET a = 1 WHERE a = 4;
SELECT * from tbl;

SELECT * from tbl;

SELECT * from tbl;

UPDATE OR REPLACE tbl SET a = 1 WHERE a = 4;
SELECT * from tbl;

INSERT INTO tbl VALUES (2, 3, 4);
SELECT * FROM tbl;

SELECT * from tbl;

DROP TABLE tbl;

DELETE FROM tbl;
SELECT * FROM rlog;

CREATE TABLE ab(a, b);
CREATE TABLE cd(c, d);
INSERT INTO ab VALUES (1, 2);
INSERT INTO ab VALUES (0, 0);
INSERT INTO cd VALUES (3, 4);
CREATE TABLE tlog(ii INTEGER PRIMARY KEY, 
olda, oldb, oldc, oldd, newa, newb, newc, newd);
CREATE VIEW abcd AS SELECT a, b, c, d FROM ab, cd;
CREATE TRIGGER before_update INSTEAD OF UPDATE ON abcd BEGIN
INSERT INTO tlog VALUES(NULL, 
old.a, old.b, old.c, old.d, new.a, new.b, new.c, new.d);
END;
CREATE TRIGGER after_update INSTEAD OF UPDATE ON abcd BEGIN
INSERT INTO tlog VALUES(NULL, 
old.a, old.b, old.c, old.d, new.a, new.b, new.c, new.d);
END;
CREATE TRIGGER before_delete INSTEAD OF DELETE ON abcd BEGIN
INSERT INTO tlog VALUES(NULL, 
old.a, old.b, old.c, old.d, 0, 0, 0, 0);
END;
CREATE TRIGGER after_delete INSTEAD OF DELETE ON abcd BEGIN
INSERT INTO tlog VALUES(NULL, 
old.a, old.b, old.c, old.d, 0, 0, 0, 0);
END;
CREATE TRIGGER before_insert INSTEAD OF INSERT ON abcd BEGIN
INSERT INTO tlog VALUES(NULL, 
0, 0, 0, 0, new.a, new.b, new.c, new.d);
END;
CREATE TRIGGER after_insert INSTEAD OF INSERT ON abcd BEGIN
INSERT INTO tlog VALUES(NULL, 
0, 0, 0, 0, new.a, new.b, new.c, new.d);
END;

UPDATE abcd SET a = 100, b = 5*5 WHERE a = 1;
DELETE FROM abcd WHERE a = 1;
INSERT INTO abcd VALUES(10, 20, 30, 40);
SELECT * FROM tlog;

DELETE FROM tlog;
INSERT INTO abcd VALUES(10, 20, 30, 40);
UPDATE abcd SET a = 100, b = 5*5 WHERE a = 1;
DELETE FROM abcd WHERE a = 1;
SELECT * FROM tlog;

DELETE FROM tlog;
DELETE FROM abcd WHERE a = 1;
INSERT INTO abcd VALUES(10, 20, 30, 40);
UPDATE abcd SET a = 100, b = 5*5 WHERE a = 1;
SELECT * FROM tlog;

CREATE TABLE t1(a,b,c);
INSERT INTO t1 VALUES(1,2,3);
CREATE VIEW v1 AS
SELECT a+b AS x, b+c AS y, a+c AS z FROM t1;
SELECT * FROM v1;

CREATE TABLE v1log(a,b,c,d,e,f);
CREATE TRIGGER r1 INSTEAD OF DELETE ON v1 BEGIN
INSERT INTO v1log VALUES(OLD.x,NULL,OLD.y,NULL,OLD.z,NULL);
END;
DELETE FROM v1 WHERE x=1;
SELECT * FROM v1log;

DELETE FROM v1 WHERE x=3;
SELECT * FROM v1log;

INSERT INTO t1 VALUES(4,5,6);
DELETE FROM v1log;
DELETE FROM v1 WHERE y=11;
SELECT * FROM v1log;

CREATE TRIGGER r2 INSTEAD OF INSERT ON v1 BEGIN
INSERT INTO v1log VALUES(NULL,NEW.x,NULL,NEW.y,NULL,NEW.z);
END;
DELETE FROM v1log;
INSERT INTO v1 VALUES(1,2,3);
SELECT * FROM v1log;

CREATE TRIGGER r3 INSTEAD OF UPDATE ON v1 BEGIN
INSERT INTO v1log VALUES(OLD.x,NEW.x,OLD.y,NEW.y,OLD.z,NEW.z);
END;
DELETE FROM v1log;
UPDATE v1 SET x=x+100, y=y+200, z=z+300;
SELECT * FROM v1log;

DELETE FROM rlog;
CREATE TRIGGER insert_before_row BEFORE INSERT ON tbl FOR EACH ROW
BEGIN
INSERT INTO rlog VALUES ( (SELECT coalesce(max(idx),0) + 1 FROM rlog), 
0, 0,
(SELECT coalesce(sum(a),0) FROM tbl),
(SELECT coalesce(sum(b),0) FROM tbl), 
new.a, new.b);
END;
CREATE TRIGGER insert_after_row AFTER INSERT ON tbl FOR EACH ROW
BEGIN
INSERT INTO rlog VALUES ( (SELECT coalesce(max(idx),0) + 1 FROM rlog), 
0, 0,
(SELECT coalesce(sum(a),0) FROM tbl),
(SELECT coalesce(sum(b),0) FROM tbl), 
new.a, new.b);
END;

CREATE TABLE t3(a TEXT, b TEXT);
CREATE VIEW v3 AS SELECT t3.a FROM t3;
CREATE TRIGGER trig1 INSTEAD OF DELETE ON v3 BEGIN
SELECT 1;
END;
DELETE FROM v3 WHERE a = 1;

CREATE TABLE other_tbl(a, b);
INSERT INTO other_tbl VALUES(1, 2);
INSERT INTO other_tbl VALUES(3, 4);
INSERT INTO tbl VALUES(5, 6);
DROP TABLE other_tbl;
SELECT * FROM rlog;

CREATE TABLE tbl(a PRIMARY KEY, b, c);
CREATE TABLE log(a, b, c);

-- ===trigger3.test===
CREATE TRIGGER before_tbl_insert BEFORE INSERT ON tbl BEGIN SELECT CASE 
WHEN (new.a = 4) THEN RAISE(IGNORE) END;
END;
CREATE TRIGGER after_tbl_insert AFTER INSERT ON tbl BEGIN SELECT CASE 
WHEN (new.a = 1) THEN RAISE(ABORT,    'Trigger abort') 
WHEN (new.a = 2) THEN RAISE(FAIL,     'Trigger fail') 
WHEN (new.a = 3) THEN RAISE(ROLLBACK, 'Trigger rollback') END;
END;

INSERT INTO tbl VALUES(1, 2, 3);

INSERT INTO tbl VALUES(4, 5, 6);

CREATE TRIGGER before_tbl_update BEFORE UPDATE ON tbl BEGIN
SELECT CASE WHEN (old.a = 1) THEN RAISE(IGNORE) END;
END;
CREATE TRIGGER before_tbl_delete BEFORE DELETE ON tbl BEGIN
SELECT CASE WHEN (old.a = 1) THEN RAISE(IGNORE) END;
END;

UPDATE tbl SET c = 10;
SELECT * FROM tbl;

DELETE FROM tbl;
SELECT * FROM tbl;

CREATE TABLE tbl2(a, b, c);

CREATE TRIGGER after_tbl2_insert AFTER INSERT ON tbl2 BEGIN
UPDATE tbl SET c = 10;
INSERT INTO tbl2 VALUES (new.a, new.b, new.c);
END;

INSERT INTO tbl2 VALUES (1, 2, 3);
SELECT * FROM tbl2;
SELECT * FROM tbl;

CREATE VIEW tbl_view AS SELECT * FROM tbl;

CREATE TRIGGER tbl_view_insert INSTEAD OF INSERT ON tbl_view BEGIN
SELECT CASE WHEN (new.a = 1) THEN RAISE(ROLLBACK, 'View rollback')
WHEN (new.a = 2) THEN RAISE(IGNORE) 
WHEN (new.a = 3) THEN RAISE(ABORT, 'View abort') END;
END;

SELECT * FROM tbl;
ROLLBACK;

SELECT * FROM tbl;

SELECT * FROM tbl;
ROLLBACK;

SELECT * FROM tbl;

SELECT * FROM tbl;

SELECT * FROM tbl;
ROLLBACK;

DROP TABLE tbl;

CREATE TABLE tbl (a, b, c);

-- ===trigger4.test===
create table test1(id integer primary key,a);
create table test2(id integer,b);
create view test as
select test1.id as id,a as a,b as b
from test1 join test2 on test2.id =  test1.id;
create trigger I_test instead of insert on test
begin
insert into test1 (id,a) values (NEW.id,NEW.a);
insert into test2 (id,b) values (NEW.id,NEW.b);
end;
insert into test values(1,2,3);
select * from test1;

create table test2(id,b);
insert into test values(7,8,9);
select * from test1;

select * from test2;

update test set b=99 where id=7;
select * from test2;

create table tbl(a integer primary key, b integer);
create view vw as select * from tbl;
create trigger t_del_tbl instead of delete on vw for each row begin
delete from tbl where a = old.a;
end;
create trigger t_upd_tbl instead of update on vw for each row begin
update tbl set a=new.a, b=new.b where a = old.a;
end;
create trigger t_ins_tbl instead of insert on vw for each row begin
insert into tbl values (new.a,new.b);
end;
insert into tbl values(101,1001);
insert into tbl values(102,1002);
insert into tbl select a+2, b+2 from tbl;
insert into tbl select a+4, b+4 from tbl;
insert into tbl select a+8, b+8 from tbl;
insert into tbl select a+16, b+16 from tbl;
insert into tbl select a+32, b+32 from tbl;
insert into tbl select a+64, b+64 from tbl;
select count(*) from vw;

select a, b from vw where a<103 or a>226 order by a;

select * from vw;

select count(*) from vw;

select a, b from vw where a<=102 or a>=227 order by a;

select * from test2;

insert into test values(4,5,6);
select * from test1;

select * from test2;

create trigger U_test instead of update on test
begin
update test1 set a=NEW.a where id=NEW.id;
update test2 set b=NEW.b where id=NEW.id;
end;
update test set a=22 where id=1;
select * from test1;

select * from test2;

update test set b=66 where id=4;
select * from test1;

select * from test2;

select * from test1;

-- ===trigger5.test===
CREATE TABLE Item(
a integer PRIMARY KEY NOT NULL ,
b double NULL ,
c int NOT NULL DEFAULT 0
);
CREATE TABLE Undo(UndoAction TEXT);
INSERT INTO Item VALUES (1,38205.60865,340);
CREATE TRIGGER trigItem_UNDO_AD AFTER DELETE ON Item FOR EACH ROW
BEGIN
INSERT INTO Undo SELECT 'INSERT INTO Item (a,b,c) VALUES ('
|| coalesce(old.a,'NULL') || ',' || quote(old.b) || ',' || old.c || ');';
END;
DELETE FROM Item WHERE a = 1;
SELECT * FROM Undo;

-- ===trigger6.test===
CREATE TABLE t1(x, y);
CREATE TABLE log(a, b, c);
CREATE TRIGGER r1 BEFORE INSERT ON t1 BEGIN
INSERT INTO log VALUES(1, new.x, new.y);
END;
CREATE TRIGGER r2 BEFORE UPDATE ON t1 BEGIN
INSERT INTO log VALUES(2, new.x, new.y);
END;

INSERT INTO t1 VALUES(1,counter());
SELECT * FROM t1;

SELECT * FROM log;

DELETE FROM t1;
DELETE FROM log;
INSERT INTO t1 VALUES(2,counter(2,3)+4);
SELECT * FROM t1;

SELECT * FROM log;

DELETE FROM log;
UPDATE t1 SET y=counter(5);
SELECT * FROM t1;

SELECT * FROM log;

-- ===trigger7.test===
CREATE TABLE t1(x, y);

PRAGMA writable_schema=on;
UPDATE sqlite_master SET sql='nonsense';

CREATE TRIGGER r1 AFTER UPDATE OF x ON t1 BEGIN
SELECT '___update_t1.x___';
END;
CREATE TRIGGER r2 AFTER UPDATE OF y ON t1 BEGIN
SELECT '___update_t1.y___';
END;

EXPLAIN UPDATE t1 SET x=5;

EXPLAIN UPDATE t1 SET x=5;

EXPLAIN UPDATE t1 SET y=5;

EXPLAIN UPDATE t1 SET y=5;

EXPLAIN UPDATE t1 SET rowid=5;

EXPLAIN UPDATE t1 SET rowid=5;

CREATE TABLE t2(x,y,z);
CREATE TRIGGER t2r1 AFTER INSERT ON t2 BEGIN SELECT 1; END;
CREATE TRIGGER t2r2 BEFORE INSERT ON t2 BEGIN SELECT 1; END;
CREATE TRIGGER t2r3 AFTER UPDATE ON t2 BEGIN SELECT 1; END;
CREATE TRIGGER t2r4 BEFORE UPDATE ON t2 BEGIN SELECT 1; END;
CREATE TRIGGER t2r5 AFTER DELETE ON t2 BEGIN SELECT 1; END;
CREATE TRIGGER t2r6 BEFORE DELETE ON t2 BEGIN SELECT 1; END;
CREATE TRIGGER t2r7 AFTER INSERT ON t2 BEGIN SELECT 1; END;
CREATE TRIGGER t2r8 BEFORE INSERT ON t2 BEGIN SELECT 1; END;
CREATE TRIGGER t2r9 AFTER UPDATE ON t2 BEGIN SELECT 1; END;
CREATE TRIGGER t2r10 BEFORE UPDATE ON t2 BEGIN SELECT 1; END;
CREATE TRIGGER t2r11 AFTER DELETE ON t2 BEGIN SELECT 1; END;
CREATE TRIGGER t2r12 BEFORE DELETE ON t2 BEGIN SELECT 1; END;
DROP TRIGGER t2r6;

-- ===trigger8.test===
CREATE TABLE t1(x);
CREATE TABLE t2(y);

INSERT INTO t1 VALUES(5);
SELECT count(*) FROM t2;

-- ===trigger9.test===
PRAGMA page_size = 1024;
CREATE TABLE t1(x, y, z);
INSERT INTO t1 VALUES('1', randstr(10000,10000), '2');
INSERT INTO t1 VALUES('2', randstr(10000,10000), '4');
INSERT INTO t1 VALUES('3', randstr(10000,10000), '6');
CREATE TABLE t2(x);

BEGIN;
CREATE TRIGGER trig1 BEFORE UPDATE ON t1 BEGIN
INSERT INTO t2 VALUES(old.x);
END;
UPDATE t1 SET y = '';
SELECT * FROM t2;

ROLLBACK;

BEGIN;
CREATE TRIGGER trig1 BEFORE UPDATE ON t1 WHEN old.x>='2' BEGIN
INSERT INTO t2 VALUES(old.x);
END;
UPDATE t1 SET y = '';
SELECT * FROM t2;

ROLLBACK;

CREATE TABLE t3(a, b);
INSERT INTO t3 VALUES(1, 'one');
INSERT INTO t3 VALUES(2, 'two');
INSERT INTO t3 VALUES(3, 'three');

BEGIN;
CREATE VIEW v1 AS SELECT * FROM t3;
CREATE TRIGGER trig1 INSTEAD OF UPDATE ON v1 BEGIN
INSERT INTO t2 VALUES(old.a);
END;
UPDATE v1 SET b = 'hello';
SELECT * FROM t2;
ROLLBACK;

BEGIN;
CREATE VIEW v1 AS SELECT a, b AS c FROM t3 WHERE c > 'one';
CREATE TRIGGER trig1 INSTEAD OF UPDATE ON v1 BEGIN
INSERT INTO t2 VALUES(old.a);
END;
UPDATE v1 SET c = 'hello';
SELECT * FROM t2;
ROLLBACK;

BEGIN;
INSERT INTO t3 VALUES(3, 'three');
INSERT INTO t3 VALUES(3, 'four');
CREATE VIEW v1 AS SELECT DISTINCT a, b FROM t3;
CREATE TRIGGER trig1 INSTEAD OF UPDATE ON v1 BEGIN
INSERT INTO t2 VALUES(old.a);
END;
UPDATE v1 SET b = 'hello';
SELECT * FROM t2;
ROLLBACK;

BEGIN;
INSERT INTO t3 VALUES(1, 'uno');
CREATE VIEW v1 AS SELECT a, b FROM t3 EXCEPT SELECT 1, 'one';
CREATE TRIGGER trig1 INSTEAD OF UPDATE ON v1 BEGIN
INSERT INTO t2 VALUES(old.a);
END;
UPDATE v1 SET b = 'hello';
SELECT * FROM t2;
ROLLBACK;

BEGIN;
INSERT INTO t3 VALUES(1, 'zero');
CREATE VIEW v1 AS 
SELECT sum(a) AS a, max(b) AS b FROM t3 GROUP BY t3.a HAVING b>'two';
CREATE TRIGGER trig1 INSTEAD OF UPDATE ON v1 BEGIN
INSERT INTO t2 VALUES(old.a);
END;
UPDATE v1 SET b = 'hello';
SELECT * FROM t2;
ROLLBACK;

BEGIN;
CREATE TRIGGER trig1 BEFORE DELETE ON t1 BEGIN
INSERT INTO t2 VALUES(old.rowid);
END;
DELETE FROM t1;
SELECT * FROM t2;

ROLLBACK;

BEGIN;
CREATE TRIGGER trig1 BEFORE DELETE ON t1 BEGIN
INSERT INTO t2 VALUES(old.x);
END;
DELETE FROM t1;
SELECT * FROM t2;

ROLLBACK;

BEGIN;
CREATE TRIGGER trig1 BEFORE DELETE ON t1 WHEN old.x='1' BEGIN
INSERT INTO t2 VALUES(old.rowid);
END;
DELETE FROM t1;
SELECT * FROM t2;

ROLLBACK;

BEGIN;
CREATE TRIGGER trig1 BEFORE UPDATE ON t1 BEGIN
INSERT INTO t2 VALUES(old.rowid);
END;
UPDATE t1 SET y = '';
SELECT * FROM t2;

ROLLBACK;

-- ===triggerA.test===
CREATE TABLE t1(x INTEGER PRIMARY KEY, y TEXT UNIQUE);
CREATE TABLE t2(a INTEGER PRIMARY KEY, b INTEGER UNIQUE, c TEXT);

CREATE TABLE result4(a,b,c,d);
CREATE TRIGGER r1u INSTEAD OF UPDATE ON v1 BEGIN
INSERT INTO result4(a,b,c,d) VALUES(old.y, old.x, new.y, new.x);
END;
UPDATE v1 SET y=y||'-extra' WHERE x BETWEEN 3 AND 5;
SELECT * FROM result4 ORDER BY a;

DELETE FROM result2;
CREATE TRIGGER r2d INSTEAD OF DELETE ON v2 BEGIN
INSERT INTO result2(a,b) VALUES(old.y, old.x);
END;
DELETE FROM v2 WHERE x=5;
SELECT * FROM result2;

DELETE FROM result4;
CREATE TRIGGER r2u INSTEAD OF UPDATE ON v2 BEGIN
INSERT INTO result4(a,b,c,d) VALUES(old.y, old.x, new.y, new.x);
END;
UPDATE v2 SET y=y||'-extra' WHERE x BETWEEN 3 AND 5;
SELECT * FROM result4 ORDER BY a;

CREATE TABLE result1(a);
CREATE TRIGGER r3d INSTEAD OF DELETE ON v3 BEGIN
INSERT INTO result1(a) VALUES(old.c1);
END;
DELETE FROM v3 WHERE c1 BETWEEN '8' AND 'eight';
SELECT * FROM result1 ORDER BY a;

DELETE FROM result2;
CREATE TRIGGER r3u INSTEAD OF UPDATE ON v3 BEGIN
INSERT INTO result2(a,b) VALUES(old.c1, new.c1);
END;
UPDATE v3 SET c1 = c1 || '-extra' WHERE c1 BETWEEN '8' and 'eight';
SELECT * FROM result2 ORDER BY a;

DELETE FROM result1;
CREATE TRIGGER r4d INSTEAD OF DELETE ON v4 BEGIN
INSERT INTO result1(a) VALUES(old.c1);
END;
DELETE FROM v4 WHERE c1 BETWEEN '8' AND 'eight';
SELECT * FROM result1 ORDER BY a;

DELETE FROM result2;
CREATE TRIGGER r4u INSTEAD OF UPDATE ON v4 BEGIN
INSERT INTO result2(a,b) VALUES(old.c1, new.c1);
END;
UPDATE v4 SET c1 = c1 || '-extra' WHERE c1 BETWEEN '8' and 'eight';
SELECT * FROM result2 ORDER BY a;

DELETE FROM result2;
CREATE TRIGGER r5d INSTEAD OF DELETE ON v5 BEGIN
INSERT INTO result2(a,b) VALUES(old.x, old.b);
END;
DELETE FROM v5 WHERE x=5;
SELECT * FROM result2;

DELETE FROM result4;
CREATE TRIGGER r5u INSTEAD OF UPDATE ON v5 BEGIN
INSERT INTO result4(a,b,c,d) VALUES(old.x, old.b, new.x, new.b);
END;
UPDATE v5 SET b = b+9900000 WHERE x BETWEEN 3 AND 5;
SELECT * FROM result4 ORDER BY a;

SELECT * FROM v5; -- warm up the cache;

INSERT INTO t1 VALUES(i,word);
INSERT INTO t2 VALUES(20-i,j,word);

SELECT count(*) FROM t1 UNION ALL SELECT count(*) FROM t2;

CREATE VIEW v1 AS SELECT y, x FROM t1;
SELECT * FROM v1 ORDER BY 1;

CREATE VIEW v2 AS SELECT x, y FROM t1 WHERE y GLOB '*e*';
SELECT * FROM v2 ORDER BY 1;

CREATE VIEW v3 AS
SELECT CAST(x AS TEXT) AS c1 FROM t1 UNION SELECT y FROM t1;
SELECT * FROM v3 ORDER BY c1;

CREATE VIEW v4 AS
SELECT CAST(x AS TEXT) AS c1 FROM t1
UNION SELECT y FROM t1 WHERE x BETWEEN 3 and 5;
SELECT * FROM v4 ORDER BY 1;

CREATE VIEW v5 AS SELECT x, b FROM t1, t2 WHERE y=c;
SELECT * FROM v5 ORDER BY x DESC;

CREATE TABLE result2(a,b);
CREATE TRIGGER r1d INSTEAD OF DELETE ON v1 BEGIN
INSERT INTO result2(a,b) VALUES(old.y, old.x);
END;
DELETE FROM v1 WHERE x=5;
SELECT * FROM result2;

-- ===triggerB.test===
CREATE TABLE x(x INTEGER PRIMARY KEY, y INT NOT NULL);
INSERT INTO x(y) VALUES(1);
INSERT INTO x(y) VALUES(1);
CREATE TEMP VIEW vx AS SELECT x, y, 0 AS yy FROM x;
CREATE TEMP TRIGGER tx INSTEAD OF UPDATE OF y ON vx
BEGIN
UPDATE x SET y = new.y WHERE x = new.x;
END;
SELECT * FROM vx;

SELECT * FROM t3_changes WHERE colnum=i;

UPDATE vx SET y = yy;
SELECT * FROM vx;

CREATE TABLE t2(a INTEGER PRIMARY KEY, b);
INSERT INTO t2 VALUES(1,2);
CREATE TABLE changes(x,y);
CREATE TRIGGER r1t2 AFTER UPDATE ON t2 BEGIN
INSERT INTO changes VALUES(new.a, new.b);
END;

UPDATE t2 SET a=a+10;
SELECT * FROM changes;

CREATE TRIGGER r2t2 AFTER DELETE ON t2 BEGIN
INSERT INTO changes VALUES(old.a, old.c);
END;

CREATE TABLE t3(
c0,  c1,  c2,  c3,  c4,  c5,  c6,  c7,  c8,  c9,
c10, c11, c12, c13, c14, c15, c16, c17, c18, c19,
c20, c21, c22, c23, c24, c25, c26, c27, c28, c29,
c30, c31, c32, c33, c34, c35, c36, c37, c38, c39,
c40, c41, c42, c43, c44, c45, c46, c47, c48, c49,
c50, c51, c52, c53, c54, c55, c56, c57, c58, c59,
c60, c61, c62, c63, c64, c65
);
CREATE TABLE t3_changes(colnum, oldval, newval);
INSERT INTO t3 VALUES(
'a0', 'a1', 'a2', 'a3', 'a4', 'a5', 'a6', 'a7', 'a8', 'a9',
'a10','a11','a12','a13','a14','a15','a16','a17','a18','a19',
'a20','a21','a22','a23','a24','a25','a26','a27','a28','a29',
'a30','a31','a32','a33','a34','a35','a36','a37','a38','a39',
'a40','a41','a42','a43','a44','a45','a46','a47','a48','a49',
'a50','a51','a52','a53','a54','a55','a56','a57','a58','a59',
'a60','a61','a62','a63','a64','a65'
);

SELECT * FROM t3_changes;

UPDATE t3 SET ci='bi';
SELECT * FROM t3_changes ORDER BY rowid DESC LIMIT 1;

SELECT count(*) FROM t3_changes;

-- ===triggerC.test===
PRAGMA recursive_triggers = on;

INSERT INTO t4 VALUES(1, 2);

SELECT * FROM t4;

CREATE TABLE t5 (a primary key, b, c);
INSERT INTO t5 values (1, 2, 3);
CREATE TRIGGER au_tbl AFTER UPDATE ON t5 BEGIN
UPDATE OR IGNORE t5 SET a = new.a, c = 10;
END;

CREATE TABLE t6(a INTEGER PRIMARY KEY, b);
INSERT INTO t6 VALUES(1, 2);
create trigger r1 after update on t6 for each row begin
SELECT 1;
end;
UPDATE t6 SET a=a;

DROP TABLE t1;
CREATE TABLE cnt(n);
INSERT INTO cnt VALUES(0);
CREATE TABLE t1(a INTEGER PRIMARY KEY, b UNIQUE, c, d, e);
CREATE INDEX t1cd ON t1(c,d);
CREATE TRIGGER t1r1 AFTER UPDATE ON t1 BEGIN UPDATE cnt SET n=n+1; END;
INSERT INTO t1 VALUES(1,2,3,4,5);
INSERT INTO t1 VALUES(6,7,8,9,10);
INSERT INTO t1 VALUES(11,12,13,14,15);

CREATE TABLE t2(a PRIMARY KEY);

DELETE FROM t2;

CREATE TABLE t22(x);
CREATE TRIGGER t22a AFTER INSERT ON t22 BEGIN
INSERT INTO t22 SELECT x + (SELECT max(x) FROM t22) FROM t22;
END;
CREATE TRIGGER t22b BEFORE INSERT ON t22 BEGIN
SELECT CASE WHEN (SELECT count(*) FROM t22) >= 100
THEN RAISE(IGNORE)
ELSE NULL END;
END;
INSERT INTO t22 VALUES(1);
SELECT count(*) FROM t22;

CREATE TABLE t23(x PRIMARY KEY);
CREATE TRIGGER t23a AFTER INSERT ON t23 BEGIN
INSERT INTO t23 VALUES(new.x + 1);
END;
CREATE TRIGGER t23b BEFORE INSERT ON t23 BEGIN
SELECT CASE WHEN new.x>500
THEN RAISE(IGNORE)
ELSE NULL END;
END;
INSERT INTO t23 VALUES(1);
SELECT count(*) FROM t23;

CREATE TABLE t3(a, b);
CREATE TRIGGER t3i AFTER INSERT ON t3 BEGIN
DELETE FROM t3 WHERE rowid = new.rowid;
END;
CREATE TRIGGER t3d AFTER DELETE ON t3 BEGIN
INSERT INTO t3 VALUES(old.a, old.b);
END;

CREATE TABLE t1(a, b, c);
CREATE TABLE log(t, a1, b1, c1, a2, b2, c2);
CREATE TRIGGER trig1 BEFORE INSERT ON t1 BEGIN
INSERT INTO log VALUES('before', NULL, NULL, NULL, new.a, new.b, new.c);
END;
CREATE TRIGGER trig2 AFTER INSERT ON t1 BEGIN
INSERT INTO log VALUES('after', NULL, NULL, NULL, new.a, new.b, new.c);
END;
CREATE TRIGGER trig3 BEFORE UPDATE ON t1 BEGIN
INSERT INTO log VALUES('before', old.a,old.b,old.c, new.a,new.b,new.c);
END;
CREATE TRIGGER trig4 AFTER UPDATE ON t1 BEGIN
INSERT INTO log VALUES('after', old.a,old.b,old.c, new.a,new.b,new.c);
END;
CREATE TRIGGER trig5 BEFORE DELETE ON t1 BEGIN
INSERT INTO log VALUES('before', old.a,old.b,old.c, NULL,NULL,NULL);
END;
CREATE TRIGGER trig6 AFTER DELETE ON t1 BEGIN
INSERT INTO log VALUES('after', old.a,old.b,old.c, NULL,NULL,NULL);
END;

SELECT * FROM t3;

CREATE TABLE t3b(x);
CREATE TRIGGER t3bi AFTER INSERT ON t3b WHEN new.x<2000 BEGIN
INSERT INTO t3b VALUES(new.x+1);
END;

SELECT * FROM t3b;

SELECT count(*), max(x), min(x) FROM t3b;

SELECT count(*), max(x), min(x) FROM t3b;

SELECT count(*), max(x), min(x) FROM t3b;

SELECT count(*), max(x), min(x) FROM t3b;

SELECT count(*), max(x), min(x) FROM t3b;

SELECT count(*), max(x), min(x) FROM t3b;

CREATE TABLE log(t);
CREATE TABLE t4(a TEXT,b INTEGER,c REAL);
CREATE TRIGGER t4bi BEFORE INSERT ON t4 BEGIN
INSERT INTO log VALUES(new.rowid || ' ' || typeof(new.rowid) || ' ' ||
new.a     || ' ' || typeof(new.a)     || ' ' ||
new.b     || ' ' || typeof(new.b)     || ' ' ||
new.c     || ' ' || typeof(new.c)
);
END;
CREATE TRIGGER t4ai AFTER INSERT ON t4 BEGIN
INSERT INTO log VALUES(new.rowid || ' ' || typeof(new.rowid) || ' ' ||
new.a     || ' ' || typeof(new.a)     || ' ' ||
new.b     || ' ' || typeof(new.b)     || ' ' ||
new.c     || ' ' || typeof(new.c)
);
END;
CREATE TRIGGER t4bd BEFORE DELETE ON t4 BEGIN
INSERT INTO log VALUES(old.rowid || ' ' || typeof(old.rowid) || ' ' ||
old.a     || ' ' || typeof(old.a)     || ' ' ||
old.b     || ' ' || typeof(old.b)     || ' ' ||
old.c     || ' ' || typeof(old.c)
);
END;
CREATE TRIGGER t4ad AFTER DELETE ON t4 BEGIN
INSERT INTO log VALUES(old.rowid || ' ' || typeof(old.rowid) || ' ' ||
old.a     || ' ' || typeof(old.a)     || ' ' ||
old.b     || ' ' || typeof(old.b)     || ' ' ||
old.c     || ' ' || typeof(old.c)
);
END;
CREATE TRIGGER t4bu BEFORE UPDATE ON t4 BEGIN
INSERT INTO log VALUES(old.rowid || ' ' || typeof(old.rowid) || ' ' ||
old.a     || ' ' || typeof(old.a)     || ' ' ||
old.b     || ' ' || typeof(old.b)     || ' ' ||
old.c     || ' ' || typeof(old.c)
);
INSERT INTO log VALUES(new.rowid || ' ' || typeof(new.rowid) || ' ' ||
new.a     || ' ' || typeof(new.a)     || ' ' ||
new.b     || ' ' || typeof(new.b)     || ' ' ||
new.c     || ' ' || typeof(new.c)
);
END;
CREATE TRIGGER t4au AFTER UPDATE ON t4 BEGIN
INSERT INTO log VALUES(old.rowid || ' ' || typeof(old.rowid) || ' ' ||
old.a     || ' ' || typeof(old.a)     || ' ' ||
old.b     || ' ' || typeof(old.b)     || ' ' ||
old.c     || ' ' || typeof(old.c)
);
INSERT INTO log VALUES(new.rowid || ' ' || typeof(new.rowid) || ' ' ||
new.a     || ' ' || typeof(new.a)     || ' ' ||
new.b     || ' ' || typeof(new.b)     || ' ' ||
new.c     || ' ' || typeof(new.c)
);
END;

INSERT INTO t1 VALUES('A', 'B', 'C');
SELECT * FROM log;

DROP TABLE IF EXISTS t5;
CREATE TABLE t5(a INTEGER PRIMARY KEY, b);
CREATE UNIQUE INDEX t5i ON t5(b);
INSERT INTO t5 VALUES(1, 'a');
INSERT INTO t5 VALUES(2, 'b');
INSERT INTO t5 VALUES(3, 'c');
CREATE TABLE t5g(a, b, c);
CREATE TRIGGER t5t BEFORE DELETE ON t5 BEGIN
INSERT INTO t5g VALUES(old.a, old.b, (SELECT count(*) FROM t5));
END;

DROP TRIGGER t5t;
CREATE TRIGGER t5t AFTER DELETE ON t5 BEGIN
INSERT INTO t5g VALUES(old.a, old.b, (SELECT count(*) FROM t5));
END;

PRAGMA recursive_triggers = off;

PRAGMA recursive_triggers = on;

PRAGMA recursive_triggers;

PRAGMA recursive_triggers = off;
PRAGMA recursive_triggers;

PRAGMA recursive_triggers = on;
PRAGMA recursive_triggers;

CREATE TABLE t8(x);
CREATE TABLE t7(a, b);
INSERT INTO t7 VALUES(1, 2);
INSERT INTO t7 VALUES(3, 4);
INSERT INTO t7 VALUES(5, 6);
CREATE TRIGGER t7t BEFORE UPDATE ON t7 BEGIN
DELETE FROM t7 WHERE a = 1;
END;
CREATE TRIGGER t7ta AFTER UPDATE ON t7 BEGIN
INSERT INTO t8 VALUES('after fired ' || old.rowid || '->' || new.rowid);
END;

BEGIN;
UPDATE t7 SET b=7 WHERE a = 5;
SELECT * FROM t7;
SELECT * FROM t8;
ROLLBACK;

BEGIN;
UPDATE t7 SET b=7 WHERE a = 1;
SELECT * FROM t7;
SELECT * FROM t8;
ROLLBACK;

SELECT * FROM t1;

DROP TRIGGER t7t;
CREATE TRIGGER t7t BEFORE UPDATE ON t7 WHEN (old.rowid!=1 OR new.rowid!=8)
BEGIN
UPDATE t7 set rowid = 8 WHERE rowid=1;
END;

BEGIN;
UPDATE t7 SET b=7 WHERE a = 5;
SELECT rowid, * FROM t7;
SELECT * FROM t8;
ROLLBACK;

BEGIN;
UPDATE t7 SET b=7 WHERE a = 1;
SELECT rowid, * FROM t7;
SELECT * FROM t8;
ROLLBACK;

DROP TRIGGER t7t;
DROP TRIGGER t7ta;
CREATE TRIGGER t7t BEFORE DELETE ON t7 BEGIN
UPDATE t7 set rowid = 8 WHERE rowid=1;
END;
CREATE TRIGGER t7ta AFTER DELETE ON t7 BEGIN
INSERT INTO t8 VALUES('after fired ' || old.rowid);
END;

BEGIN;
DELETE FROM t7 WHERE a = 3;
SELECT rowid, * FROM t7;
SELECT * FROM t8;
ROLLBACK;

BEGIN;
DELETE FROM t7 WHERE a = 1;
SELECT rowid, * FROM t7;
SELECT * FROM t8;
ROLLBACK;

CREATE TABLE t9(a,b);
CREATE INDEX t9b ON t9(b);
INSERT INTO t9 VALUES(1,0);
INSERT INTO t9 VALUES(2,1);
INSERT INTO t9 VALUES(3,2);
INSERT INTO t9 SELECT a+3, a+2 FROM t9;
INSERT INTO t9 SELECT a+6, a+5 FROM t9;
SELECT a FROM t9 ORDER BY a;

CREATE TRIGGER t9r1 AFTER DELETE ON t9 BEGIN
DELETE FROM t9 WHERE b=old.a;
END;
DELETE FROM t9 WHERE b=4;
SELECT a FROM t9 ORDER BY a;

CREATE TABLE t10(a, updatecnt DEFAULT 0);
CREATE TRIGGER t10_bu BEFORE UPDATE OF a ON t10 BEGIN
UPDATE t10 SET updatecnt = updatecnt+1 WHERE rowid = old.rowid;
END;
INSERT INTO t10(a) VALUES('hello');

UPDATE t10 SET a = 'world';
SELECT * FROM t10;

DELETE FROM log;
UPDATE t1 SET a = 'a';
SELECT * FROM log;

UPDATE t10 SET a = 'tcl', updatecnt = 5;
SELECT * FROM t10;

CREATE TABLE t11(
c1,   c2,  c3,  c4,  c5,  c6,  c7,  c8,  c9, c10,
c11, c12, c13, c14, c15, c16, c17, c18, c19, c20,
c21, c22, c23, c24, c25, c26, c27, c28, c29, c30,
c31, c32, c33, c34, c35, c36, c37, c38, c39, c40
);
CREATE TRIGGER t11_bu BEFORE UPDATE OF c1 ON t11 BEGIN
UPDATE t11 SET c31 = c31+1, c32=c32+1 WHERE rowid = old.rowid;
END;
INSERT INTO t11 VALUES(
1,   2,  3,  4,  5,  6,  7,  8,  9, 10,
11, 12, 13, 14, 15, 16, 17, 18, 19, 20,
21, 22, 23, 24, 25, 26, 27, 28, 29, 30,
31, 32, 33, 34, 35, 36, 37, 38, 39, 40
);

UPDATE t11 SET c4=35, c33=22, c1=5;
SELECT * FROM t11;

CREATE TABLE log(a, b);

DELETE FROM log;

CREATE TRIGGER tt1 BEFORE INSERT ON t1 BEGIN 
INSERT INTO log VALUES(new.a, new.b);
END;
INSERT INTO t1 DEFAULT VALUES;
SELECT * FROM log;

DELETE FROM log;

CREATE TRIGGER tt2 AFTER INSERT ON t1 BEGIN 
INSERT INTO log VALUES(new.a, new.b);
END;
INSERT INTO t1 DEFAULT VALUES;
SELECT * FROM log;

DROP TRIGGER tt1;

DELETE FROM log;

SELECT * FROM t1;

INSERT INTO t1 DEFAULT VALUES;
SELECT * FROM log;

DELETE FROM log;
CREATE TABLE t2(a, b);
CREATE VIEW v2 AS SELECT * FROM t2;
CREATE TRIGGER tv2 INSTEAD OF INSERT ON v2 BEGIN
INSERT INTO log VALUES(new.a, new.b);
END;
INSERT INTO v2 DEFAULT VALUES;
SELECT a, b, a IS NULL, b IS NULL FROM log;

CREATE TABLE t1(a, b);
INSERT INTO t1 VALUES(1, 2);
INSERT INTO t1 VALUES(3, 4);
INSERT INTO t1 VALUES(5, 6);
CREATE TRIGGER tr1 AFTER INSERT ON t1 BEGIN SELECT 1 ; END ;
SELECT count(*) FROM sqlite_master;

SELECT * FROM t1;

DROP TRIGGER tr1;

SELECT count(*) FROM sqlite_master;

DELETE FROM log;
DELETE FROM t1;
SELECT * FROM log;

SELECT * FROM t1;

CREATE TABLE t4(a, b);
CREATE TRIGGER t4t AFTER DELETE ON t4 BEGIN
SELECT RAISE(ABORT, 'delete is not supported');
END;

-- ===triggerD.test===
CREATE TABLE t1(rowid, oid, _rowid_, x);
CREATE TABLE log(a,b,c,d,e);
CREATE TRIGGER r1 BEFORE INSERT ON t1 BEGIN
INSERT INTO log VALUES('r1', new.rowid, new.oid, new._rowid_, new.x);
END;
CREATE TRIGGER r2 AFTER INSERT ON t1 BEGIN
INSERT INTO log VALUES('r2', new.rowid, new.oid, new._rowid_, new.x);
END;
CREATE TRIGGER r3 BEFORE UPDATE ON t1 BEGIN
INSERT INTO log VALUES('r3.old', old.rowid, old.oid, old._rowid_, old.x);
INSERT INTO log VALUES('r3.new', new.rowid, new.oid, new._rowid_, new.x);
END;
CREATE TRIGGER r4 AFTER UPDATE ON t1 BEGIN
INSERT INTO log VALUES('r4.old', old.rowid, old.oid, old._rowid_, old.x);
INSERT INTO log VALUES('r4.new', new.rowid, new.oid, new._rowid_, new.x);
END;
CREATE TRIGGER r5 BEFORE DELETE ON t1 BEGIN
INSERT INTO log VALUES('r5', old.rowid, old.oid, old._rowid_, old.x);
END;
CREATE TRIGGER r6 AFTER DELETE ON t1 BEGIN
INSERT INTO log VALUES('r6', old.rowid, old.oid, old._rowid_, old.x);
END;

DELETE FROM t301;
CREATE TRIGGER temp.r301 AFTER INSERT ON t300 BEGIN
INSERT INTO t301 VALUES(20000 + new.x);
END;
INSERT INTO main.t300 VALUES(3);
INSERT INTO temp.t300 VALUES(4);
SELECT * FROM t301;

INSERT INTO t1 VALUES(100,200,300,400);
SELECT * FROM log;

DELETE FROM log;
UPDATE t1 SET rowid=rowid+1;
SELECT * FROM log;

DELETE FROM log;
DELETE FROM t1;
SELECT * FROM log;

DROP TABLE t1;
CREATE TABLE t1(w,x,y,z);
CREATE TRIGGER r1 BEFORE INSERT ON t1 BEGIN
INSERT INTO log VALUES('r1', new.rowid, new.oid, new._rowid_, new.x);
END;
CREATE TRIGGER r2 AFTER INSERT ON t1 BEGIN
INSERT INTO log VALUES('r2', new.rowid, new.oid, new._rowid_, new.x);
END;
CREATE TRIGGER r3 BEFORE UPDATE ON t1 BEGIN
INSERT INTO log VALUES('r3.old', old.rowid, old.oid, old._rowid_, old.x);
INSERT INTO log VALUES('r3.new', new.rowid, new.oid, new._rowid_, new.x);
END;
CREATE TRIGGER r4 AFTER UPDATE ON t1 BEGIN
INSERT INTO log VALUES('r4.old', old.rowid, old.oid, old._rowid_, old.x);
INSERT INTO log VALUES('r4.new', new.rowid, new.oid, new._rowid_, new.x);
END;
CREATE TRIGGER r5 BEFORE DELETE ON t1 BEGIN
INSERT INTO log VALUES('r5', old.rowid, old.oid, old._rowid_, old.x);
END;
CREATE TRIGGER r6 AFTER DELETE ON t1 BEGIN
INSERT INTO log VALUES('r6', old.rowid, old.oid, old._rowid_, old.x);
END;

DELETE FROM log;
INSERT INTO t1 VALUES(100,200,300,400);
SELECT * FROM log;

DELETE FROM log;
UPDATE t1 SET x=x+1;
SELECT * FROM log;

DELETE FROM log;
DELETE FROM t1;
SELECT * FROM log;

CREATE TABLE t300(x);
CREATE TEMP TABLE t300(x);
CREATE TABLE t301(y);
CREATE TRIGGER main.r300 AFTER INSERT ON t300 BEGIN
INSERT INTO t301 VALUES(10000 + new.x);
END;
INSERT INTO main.t300 VALUES(3);
INSERT INTO temp.t300 VALUES(4);
SELECT * FROM t301;