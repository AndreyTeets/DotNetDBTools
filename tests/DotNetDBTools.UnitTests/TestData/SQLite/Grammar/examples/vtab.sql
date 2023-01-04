-- ===vtab1.test===
SELECT name FROM sqlite_master ORDER BY 1;

CREATE TABLE template(a, b, c);

SELECT * FROM echo_c WHERE b IS NULL AND a = 15;

DELETE FROM c;

SELECT * FROM echo_c WHERE rowid IN (1, 2, 3);

SELECT * FROM echo_c WHERE rowid = 1;

SELECT * FROM echo_c WHERE a = 1;

SELECT * FROM echo_c WHERE a IN (1, 2);

CREATE TABLE t1(a, b, c);
CREATE VIRTUAL TABLE echo_t1 USING echo(t1);

INSERT INTO echo_t1(rowid) VALUES(45);
SELECT rowid, * FROM echo_t1;

INSERT INTO echo_t1(rowid) VALUES(NULL);
SELECT rowid, * FROM echo_t1;

CREATE TABLE t2(a PRIMARY KEY, b, c);
INSERT INTO t2 VALUES(1, 2, 3);
INSERT INTO t2 VALUES(4, 5, 6);
CREATE VIRTUAL TABLE echo_t2 USING echo(t2);

PRAGMA table_info(template);

PRAGMA writable_schema = 1;
INSERT INTO sqlite_master VALUES(
'table', 't3', 't3', 0, 'INSERT INTO "%s%s" VALUES(1)'
);

CREATE VIRTUAL TABLE t1 USING echo(template);

PRAGMA table_info(t1);

PRAGMA table_info(t1);

DROP TABLE t1;

PRAGMA table_info(t1);

SELECT sql FROM sqlite_master;

DROP TABLE template;
SELECT sql FROM sqlite_master;

CREATE TABLE treal(a INTEGER, b INTEGER, c); 
CREATE INDEX treal_idx ON treal(b);
CREATE VIRTUAL TABLE t1 USING echo(treal);

SELECT name FROM sqlite_master ORDER BY 1;

SELECT a, b, c FROM t1;

INSERT INTO treal VALUES(1, 2, 3);
INSERT INTO treal VALUES(4, 5, 6);
SELECT * FROM t1;

SELECT a FROM t1;

SELECT rowid FROM t1;

SELECT * FROM t1;

SELECT rowid, * FROM t1;

SELECT a AS d, b AS e, c AS f FROM t1;

SELECT * FROM t1;

SELECT * FROM t1 WHERE b = 5;

SELECT * FROM t1 WHERE b >= 5 AND b <= 10;

SELECT name FROM sqlite_master ORDER BY 1;

SELECT * FROM t1 WHERE b BETWEEN 2 AND 10;

SELECT * FROM t1 WHERE b MATCH 'string';

DROP TABLE t1;
DROP TABLE treal;

CREATE TABLE t1(a, b, c);
CREATE TABLE t2(d, e, f);
INSERT INTO t1 VALUES(1, 'red', 'green');
INSERT INTO t1 VALUES(2, 'blue', 'black');
INSERT INTO t2 VALUES(1, 'spades', 'clubs');
INSERT INTO t2 VALUES(2, 'hearts', 'diamonds');
CREATE VIRTUAL TABLE et1 USING echo(t1);
CREATE VIRTUAL TABLE et2 USING echo(t2);

SELECT * FROM et1, et2;

SELECT * FROM et1, et2 WHERE et2.d = 2;

CREATE INDEX i1 ON t2(d);

SELECT * FROM et1, et2 WHERE et2.d = 2;

DROP TABLE t1;
DROP TABLE t2;
DROP TABLE et1;
DROP TABLE et2;

SELECT sql FROM sqlite_master;

CREATE TABLE t2152b(x,y);

CREATE TABLE treal(a PRIMARY KEY, b, c);
CREATE VIRTUAL TABLE techo USING echo(treal);
SELECT name FROM sqlite_master WHERE type = 'table';

PRAGMA count_changes=ON;
INSERT INTO techo VALUES(1, 2, 3);

SELECT * FROM techo;

UPDATE techo SET a = 5;

SELECT * FROM techo;

UPDATE techo SET a=6 WHERE a<0;

SELECT * FROM techo;

UPDATE techo set a = a||b||c;

SELECT * FROM techo;

UPDATE techo set rowid = 10;

DROP TABLE t2152a; DROP TABLE t2152b;

SELECT rowid FROM techo;

INSERT INTO techo VALUES(11,12,13);

SELECT * FROM techo ORDER BY a;

UPDATE techo SET b=b+1000;

SELECT * FROM techo ORDER BY a;

DELETE FROM techo WHERE a=5;

SELECT * FROM techo ORDER BY a;

DELETE FROM techo;

SELECT * FROM techo ORDER BY a;

PRAGMA count_changes=OFF;

DROP TABLE treal;
SELECT name FROM sqlite_master ORDER BY 1;

CREATE TABLE techo(a PRIMARY KEY, b, c);

SELECT rowid, * FROM techo;

SELECT rowid, * FROM techo;

CREATE TABLE real_abc(a PRIMARY KEY, b, c);
CREATE VIRTUAL TABLE echo_abc USING echo(real_abc);

INSERT INTO echo_abc VALUES(1, 2, 3);
SELECT last_insert_rowid();

INSERT INTO echo_abc(rowid) VALUES(31427);
SELECT last_insert_rowid();

INSERT INTO echo_abc SELECT a||'.v2', b, c FROM echo_abc;
SELECT last_insert_rowid();

SELECT rowid, a, b, c FROM echo_abc;

UPDATE echo_abc SET c = 5 WHERE b = 2;
SELECT last_insert_rowid();

UPDATE echo_abc SET rowid = 5 WHERE rowid = 1;
SELECT last_insert_rowid();

CREATE TABLE treal(a, b, c);
CREATE VIRTUAL TABLE techo USING echo(treal);

DELETE FROM echo_abc WHERE b = 2;
SELECT last_insert_rowid();

SELECT rowid, a, b, c FROM echo_abc;

DELETE FROM echo_abc WHERE b = 2;
SELECT last_insert_rowid();

SELECT rowid, a, b, c FROM real_abc;

DELETE FROM echo_abc;
SELECT last_insert_rowid();

SELECT rowid, a, b, c FROM real_abc;

ATTACH 'test2.db' AS aux;
CREATE VIRTUAL TABLE aux.e2 USING echo(real_abc);

DROP TABLE treal;
DROP TABLE techo;
DROP TABLE echo_abc;
DROP TABLE real_abc;

CREATE TABLE r(a, b, c);
CREATE VIRTUAL TABLE e USING echo(r, e_log);
SELECT name FROM sqlite_master;

DROP TABLE e;
SELECT name FROM sqlite_master;

DROP TABLE techo;
CREATE TABLE logmsg(log);

CREATE VIRTUAL TABLE e USING echo(r, e_log, virtual, 1, 2, 3, varchar(32));

CREATE TABLE del(d);
CREATE VIRTUAL TABLE e2 USING echo(del);

DROP TABLE del;

EXPLAIN SELECT * FROM e WHERE rowid = 2;
EXPLAIN QUERY PLAN SELECT * FROM e WHERE rowid = 2 ORDER BY rowid;

SELECT * FROM e WHERE rowid||'' MATCH 'pattern';

SELECT * FROM e WHERE match('pattern', rowid, 'pattern2');

INSERT INTO r(a,b,c) VALUES(1,'?',99);
INSERT INTO r(a,b,c) VALUES(2,3,99);
SELECT a GLOB b FROM e;

SELECT a like 'b' FROM e;

SELECT a glob '2' FROM e;

SELECT  glob('2',a) FROM e;

DROP TABLE treal;
DROP TABLE logmsg;
SELECT sql FROM sqlite_master;

SELECT  glob(a,'2') FROM e;

CREATE TABLE b(a, b, c);
CREATE TABLE c(a UNIQUE, b, c);
INSERT INTO b VALUES(1, 'A', 'B');
INSERT INTO b VALUES(2, 'C', 'D');
INSERT INTO b VALUES(3, 'E', 'F');
INSERT INTO c VALUES(3, 'G', 'H');
CREATE VIRTUAL TABLE echo_c USING echo(c);

SELECT * FROM c;

BEGIN;

SELECT * FROM c;

COMMIT;

SELECT * FROM c;

SELECT * FROM echo_c WHERE a IS NULL;

INSERT INTO c VALUES(NULL, 15, 16);
SELECT * FROM echo_c WHERE a IS NULL;

INSERT INTO c VALUES(15, NULL, 16);
SELECT * FROM echo_c WHERE b IS NULL;

-- ===vtab2.test===
CREATE VIRTUAL TABLE schema USING schema;
SELECT * FROM schema;

BEGIN TRANSACTION;
CREATE TABLE t1(a INTEGER PRIMARY KEY, b, c, UNIQUE(b, c));
CREATE TABLE fkey(
to_tbl,
to_col
);
INSERT INTO "fkey" VALUES('t1',NULL);
COMMIT;

CREATE VIRTUAL TABLE v_col USING schema;

SELECT name FROM v_col WHERE tablename = 't1' AND pk;

UPDATE fkey 
SET to_col = (SELECT name FROM v_col WHERE tablename = 't1' AND pk);

SELECT * FROM fkey;

SELECT length(tablename) FROM schema GROUP by tablename;

SELECT tablename FROM schema GROUP by length(tablename);

SELECT length(tablename) FROM schema GROUP by length(tablename);

CREATE VIRTUAL TABLE vars USING tclvar;
SELECT * FROM vars WHERE name='abc';

SELECT * FROM vars WHERE name='A';

SELECT name, value FROM vars
WHERE name MATCH 'tcl_*' AND arrayname = '' 
ORDER BY name;

SELECT * FROM schema WHERE dflt_value IS NULL LIMIT 1;

SELECT *, b.rowid
FROM schema a LEFT JOIN schema b ON a.dflt_value=b.dflt_value
WHERE a.rowid=1;

-- ===vtab3.test===
CREATE TABLE elephant(
name VARCHAR(32), 
color VARCHAR(16), 
age INTEGER, 
UNIQUE(name, color)
);

CREATE VIRTUAL TABLE pachyderm USING echo(elephant);

DROP TABLE pachyderm;

SELECT name FROM sqlite_master WHERE type = 'table';

SELECT name FROM sqlite_master WHERE type = 'table';

SELECT name FROM sqlite_master WHERE type = 'table';

DROP TABLE pachyderm;

SELECT name FROM sqlite_master WHERE type = 'table';

SELECT name FROM sqlite_master WHERE type = 'table';

-- ===vtab4.test===
CREATE TABLE treal(a PRIMARY KEY, b, c);
CREATE VIRTUAL TABLE techo USING echo(treal);

BEGIN;
INSERT INTO techo SELECT * FROM secho;
DELETE FROM secho;
ROLLBACK;

SELECT * FROM secho;

SELECT * FROM techo;

INSERT INTO techo VALUES(1, 2, 3);

UPDATE techo SET a = 2;

DELETE FROM techo;

BEGIN;
INSERT INTO techo VALUES(1, 2, 3);
INSERT INTO techo VALUES(4, 5, 6);
INSERT INTO techo VALUES(7, 8, 9);
COMMIT;

CREATE TABLE sreal(a, b, c UNIQUE);
CREATE VIRTUAL TABLE secho USING echo(sreal);

BEGIN;
INSERT INTO secho SELECT * FROM techo;
DELETE FROM techo;
COMMIT;

SELECT * FROM secho;

SELECT * FROM techo;

-- ===vtab5.test===
CREATE TABLE treal(a VARCHAR(16), b INTEGER, c FLOAT);
INSERT INTO treal VALUES('a', 'b', 'c');
CREATE VIRTUAL TABLE techo USING echo(treal);

SELECT * FROM techo;

INSERT INTO techo VALUES('c', 'd', 'e');
SELECT * FROM techo;

UPDATE techo SET a = 10;
SELECT * FROM techo;

DELETE FROM techo WHERE b > 'c';
SELECT * FROM techo;

DROP TABLE techo;
DROP TABLE treal;

CREATE TABLE strings(str COLLATE NOCASE);
INSERT INTO strings VALUES('abc1');
INSERT INTO strings VALUES('Abc3');
INSERT INTO strings VALUES('ABc2');
INSERT INTO strings VALUES('aBc4');
SELECT str FROM strings ORDER BY 1;

CREATE VIRTUAL TABLE echo_strings USING echo(strings);
SELECT str FROM echo_strings ORDER BY 1;

SELECT str||'' FROM echo_strings ORDER BY 1;

-- ===vtab6.test===
CREATE TABLE real_t1(a,b,c);
CREATE TABLE real_t2(b,c,d);
CREATE TABLE real_t3(c,d,e);
CREATE TABLE real_t4(d,e,f);
CREATE TABLE real_t5(a INTEGER PRIMARY KEY);
CREATE TABLE real_t6(a INTEGER);
CREATE TABLE real_t7 (x, y);
CREATE TABLE real_t8 (a integer primary key, b);
CREATE TABLE real_t9(a INTEGER PRIMARY KEY, b);
CREATE TABLE real_t10(x INTEGER PRIMARY KEY, y);
CREATE TABLE real_t11(p INTEGER PRIMARY KEY, q);
CREATE TABLE real_t12(a,b);
CREATE TABLE real_t13(b,c);
CREATE TABLE real_t21(a,b,c);
CREATE TABLE real_t22(p,q);

SELECT * FROM t1 natural inner join t2;

INSERT INTO t3 VALUES(2,3,4);
INSERT INTO t3 VALUES(3,4,5);
INSERT INTO t3 VALUES(4,5,6);
SELECT * FROM t3;

SELECT * FROM t1 natural join t2 natural join t3;

INSERT INTO t4 VALUES(2,3,4);
INSERT INTO t4 VALUES(3,4,5);
INSERT INTO t4 VALUES(4,5,6);
SELECT * FROM t4;

SELECT * FROM t1 natural join t2 natural join t4;

SELECT * FROM t1 natural join t2 natural join t3 WHERE t1.a=1;

SELECT * FROM t1 NATURAL LEFT JOIN t2;

SELECT * FROM t2 NATURAL LEFT OUTER JOIN t1;

SELECT * FROM t1 LEFT JOIN t2 ON t1.a=t2.d;

SELECT * FROM t1 LEFT JOIN t2 ON t1.a=t2.d WHERE t1.a>1;

INSERT INTO t1 VALUES(1,2,3);
INSERT INTO t1 VALUES(2,3,4);
INSERT INTO t1 VALUES(3,4,5);
SELECT * FROM t1;

SELECT * FROM t1 LEFT JOIN t2 ON t1.a=t2.d WHERE t2.b IS NULL OR t2.b>1;

BEGIN;
INSERT INTO t6 VALUES(NULL);
INSERT INTO t6 VALUES(NULL);
INSERT INTO t6 SELECT * FROM t6;
INSERT INTO t6 SELECT * FROM t6;
INSERT INTO t6 SELECT * FROM t6;
INSERT INTO t6 SELECT * FROM t6;
INSERT INTO t6 SELECT * FROM t6;
INSERT INTO t6 SELECT * FROM t6;
COMMIT;

SELECT * FROM t6 NATURAL JOIN t5;

SELECT * FROM t6, t5 WHERE t6.a<t5.a;

SELECT * FROM t6, t5 WHERE t6.a>t5.a;

UPDATE t6 SET a='xyz';
SELECT * FROM t6 NATURAL JOIN t5;

SELECT * FROM t6, t5 WHERE t6.a<t5.a;

SELECT * FROM t6, t5 WHERE t6.a>t5.a;

UPDATE t6 SET a=1;
SELECT * FROM t6 NATURAL JOIN t5;

SELECT * FROM t6, t5 WHERE t6.a<t5.a;

INSERT INTO t2 VALUES(1,2,3);
INSERT INTO t2 VALUES(2,3,4);
INSERT INTO t2 VALUES(3,4,5);
SELECT * FROM t2;

SELECT * FROM t6, t5 WHERE t6.a>t5.a;

INSERT INTO t7 VALUES ("pa1", 1);
INSERT INTO t7 VALUES ("pa2", NULL);
INSERT INTO t7 VALUES ("pa3", NULL);
INSERT INTO t7 VALUES ("pa4", 2);
INSERT INTO t7 VALUES ("pa30", 131);
INSERT INTO t7 VALUES ("pa31", 130);
INSERT INTO t7 VALUES ("pa28", NULL);
INSERT INTO t8 VALUES (1, "pa1");
INSERT INTO t8 VALUES (2, "pa4");
INSERT INTO t8 VALUES (3, NULL);
INSERT INTO t8 VALUES (4, NULL);
INSERT INTO t8 VALUES (130, "pa31");
INSERT INTO t8 VALUES (131, "pa30");
SELECT coalesce(t8.a,999) from t7 LEFT JOIN t8 on y=a;

BEGIN;
INSERT INTO t9 VALUES(1,11);
INSERT INTO t9 VALUES(2,22);
INSERT INTO t10 VALUES(1,2);
INSERT INTO t10 VALUES(3,3);    
INSERT INTO t11 VALUES(2,111);
INSERT INTO t11 VALUES(3,333);    
CREATE VIEW v10_11 AS SELECT x, q FROM t10, t11 WHERE t10.y=t11.p;
COMMIT;
SELECT * FROM t9 LEFT JOIN v10_11 ON( a=x );

SELECT * FROM t9 LEFT JOIN (SELECT x, q FROM t10, t11 WHERE t10.y=t11.p)
ON( a=x);

SELECT * FROM v10_11 LEFT JOIN t9 ON( a=x );

BEGIN;
INSERT INTO t12 VALUES(1,11);
INSERT INTO t12 VALUES(2,22);
INSERT INTO t13 VALUES(22,222);
COMMIT;

SELECT * FROM t12 NATURAL LEFT JOIN t13
EXCEPT
SELECT * FROM t12 NATURAL LEFT JOIN (SELECT * FROM t13 WHERE b>0);

CREATE VIEW v13 AS SELECT * FROM t13 WHERE b>0;
SELECT * FROM t12 NATURAL LEFT JOIN t13
EXCEPT
SELECT * FROM t12 NATURAL LEFT JOIN v13;

CREATE INDEX i22 ON real_t22(q);
SELECT a FROM t21 LEFT JOIN t22 ON b=p WHERE q=
(SELECT max(m.q) FROM t22 m JOIN t21 n ON n.b=m.p WHERE n.c=1);

CREATE TABLE ab_r(a, b);
CREATE TABLE bc_r(b, c);
CREATE VIRTUAL TABLE ab USING echo(ab_r); 
CREATE VIRTUAL TABLE bc USING echo(bc_r); 
INSERT INTO ab VALUES(1, 2);
INSERT INTO bc VALUES(2, 3);

SELECT b FROM t1 NATURAL JOIN t2;

SELECT a, b, c FROM ab NATURAL JOIN bc;

SELECT a, b, c FROM bc NATURAL JOIN ab;

SELECT a, b, c FROM ab NATURAL JOIN bc;

SELECT a, b, c FROM bc NATURAL JOIN ab;

CREATE INDEX ab_i ON ab_r(b);
CREATE INDEX bc_i ON bc_r(b);

SELECT a, b, c FROM ab NATURAL JOIN bc;

SELECT a, b, c FROM bc NATURAL JOIN ab;

SELECT a, b, c FROM ab NATURAL JOIN bc;

SELECT a, b, c FROM bc NATURAL JOIN ab;

SELECT a, b, c FROM ab NATURAL JOIN bc;

SELECT b FROM t1 JOIN t2 USING(b);

SELECT a, b, c FROM bc NATURAL JOIN ab;

SELECT a, b, c FROM ab NATURAL JOIN bc;

SELECT a, b, c FROM bc NATURAL JOIN ab;

SELECT * FROM t1 NATURAL CROSS JOIN t2;

SELECT * FROM t1 CROSS JOIN t2 USING(b,c);

SELECT * FROM t1 NATURAL INNER JOIN t2;

SELECT * FROM t1 INNER JOIN t2 USING(b,c);

-- ===vtab7.test===
CREATE TABLE abc(a, b, c);
CREATE VIRTUAL TABLE abc2 USING echo(abc);

DROP TABLE newtab;

ATTACH 'test2.db' AS db2;
CREATE TABLE db2.stuff(description, shape, color);

INSERT INTO db2.stuff VALUES('abc', 'square', 'green');

INSERT INTO abc2 VALUES(1, 2, 3);
SELECT * from stuff;

INSERT INTO log VALUES('hello');

CREATE TABLE def(d, e, f);
CREATE VIRTUAL TABLE def2 USING echo(def);

INSERT INTO abc2 VALUES(1, 2, 3);

INSERT INTO abc2 VALUES(1, 2, 3);

INSERT INTO abc2 VALUES(1, 2, 3);
SELECT name FROM sqlite_master ORDER BY name;

INSERT INTO abc2 VALUES(1, 2, 3);

INSERT INTO log VALUES('xSync');

CREATE TABLE log(msg);
INSERT INTO abc2 VALUES(4, 5, 6);
SELECT * FROM log;

INSERT INTO abc2 VALUES(4, 5, 6);
SELECT * FROM log;

INSERT INTO abc2 VALUES(4, 5, 6);
SELECT * FROM log;

CREATE TABLE newtab(d, e, f);

INSERT INTO abc2 VALUES(1, 2, 3);
SELECT name FROM sqlite_master ORDER BY name;

INSERT INTO abc2 VALUES(1, 2, 3);
SELECT name FROM sqlite_master ORDER BY name;

-- ===vtab8.test===
CREATE TABLE t2244(a, b);
CREATE VIRTUAL TABLE t2244e USING echo(t2244);
INSERT INTO t2244 VALUES('AA', 'BB');
INSERT INTO t2244 VALUES('CC', 'DD');
SELECT rowid, * FROM t2244e;

SELECT * FROM t2244e WHERE rowid = 10;

UPDATE t2244e SET a = 'hello world' WHERE 0;
SELECT rowid, * FROM t2244e;

CREATE TABLE t2250(a, b);
INSERT INTO t2250 VALUES(10, 20);
CREATE VIRTUAL TABLE t2250e USING echo(t2250);
select max(rowid) from t2250;
select max(rowid) from t2250e;

CREATE TABLE t2260a_real(a, b);
CREATE TABLE t2260b_real(a, b);
CREATE INDEX i2260 ON t2260a_real(a);
CREATE INDEX i2260x ON t2260b_real(a);
CREATE VIRTUAL TABLE t2260a USING echo(t2260a_real);
CREATE VIRTUAL TABLE t2260b USING echo(t2260b_real);
SELECT * FROM t2260a, t2260b WHERE t2260a.a = t2260b.a AND t2260a.a > 101;

-- ===vtab9.test===
CREATE TABLE t0(a);
CREATE VIRTUAL TABLE t1 USING echo(t0);
INSERT INTO t1 SELECT 'hello';
SELECT rowid, * FROM t1;

CREATE TABLE t2(a,b,c);
CREATE VIRTUAL TABLE t3 USING echo(t2);
CREATE TABLE d1(a,b,c);
INSERT INTO d1 VALUES(1,2,3);
INSERT INTO d1 VALUES('a','b','c');
INSERT INTO d1 VALUES(NULL,'x',123.456);
INSERT INTO d1 VALUES(x'6869',123456789,-12345);
INSERT INTO t3(a,b,c) SELECT * FROM d1;
SELECT rowid, * FROM t3;

CREATE TABLE t4(a);
CREATE VIRTUAL TABLE t5 USING echo(t4);
INSERT INTO t4 VALUES('hello');
SELECT rowid, a FROM t5;

INSERT INTO t5(rowid, a) VALUES(1, 'goodbye');

REPLACE INTO t5(rowid, a) VALUES(1, 'goodbye');
SELECT * FROM t5;

-- ===vtabA.test===
CREATE TABLE t1(a, b HIDDEN VARCHAR, c INTEGER);

CREATE VIRTUAL TABLE t1e USING echo(t1);

DROP TABLE IF EXISTS t1;
DROP TABLE IF EXISTS t2;
CREATE TABLE t1(a,b);
INSERT INTO t1 VALUES(1,2);
CREATE TABLE t2(x,y);
INSERT INTO t2 VALUES(3,4);
CREATE VIRTUAL TABLE vt1 USING echo(t1);
CREATE VIRTUAL TABLE vt2 USING echo(t2);
UPDATE vt2 SET x=(SELECT a FROM vt1 WHERE b=2) WHERE y=4;
SELECT * FROM t2;

CREATE VIRTUAL TABLE t1e USING echo(t1);

PRAGMA table_info(t1e);

SELECT a, b, c FROM t1e;

SELECT * FROM t1e;

INSERT INTO t1e SELECT * FROM t1e;

SELECT * FROM t1e;

DROP TABLE IF EXISTS t1e;

DROP TABLE IF EXISTS t1;

-- ===vtabB.test===
CREATE TABLE t1(x);
BEGIN;
CREATE VIRTUAL TABLE temp.echo_test1 USING echo(t1);
DROP TABLE echo_test1;
ROLLBACK;

INSERT INTO t1 VALUES(2);
INSERT INTO t1 VALUES(3);
CREATE TABLE t2(y);
INSERT INTO t2 VALUES(1);
INSERT INTO t2 VALUES(2);
CREATE VIRTUAL TABLE echo_t2 USING echo(t2);
SELECT * FROM t1 WHERE x IN (SELECT rowid FROM t2);

SELECT rowid FROM echo_t2;

SELECT * FROM t1 WHERE x IN (SELECT rowid FROM t2);

SELECT * FROM t1 WHERE x IN (SELECT rowid FROM echo_t2);

SELECT * FROM t1 WHERE x IN (SELECT y FROM t2);

SELECT * FROM t1 WHERE x IN (SELECT y FROM echo_t2);

-- ===vtabC.test===
SELECT count(*) FROM sqlite_master;

SELECT name FROM sqlite_master;

CREATE TABLE m(a);

SELECT count(*) FROM sqlite_master;

INSERT INTO m VALUES(1000);
SELECT * FROM m;

SELECT count(*) FROM sqlite_master;

INSERT INTO m VALUES(9000000);
SELECT * FROM m;

-- ===vtabD.test===
CREATE TABLE t1(a, b);
CREATE INDEX i1 ON t1(a);
CREATE INDEX i2 ON t1(b);
CREATE VIRTUAL TABLE tv1 USING echo(t1);

SELECT * FROM tv1 WHERE a = 90001 OR b = 810000;

INSERT INTO t1 VALUES(i, i*i);

SELECT * FROM tv1 WHERE a = 1 OR b = 4;

SELECT * FROM tv1 WHERE a = 1 OR b = 1;

SELECT * FROM tv1 WHERE (a > 0 AND a < 5) OR (b > 15 AND b < 65);

SELECT * FROM tv1 WHERE a < 500 OR b = 810000;

SELECT * FROM t1 WHERE a < 500
UNION ALL
SELECT * FROM t1 WHERE b = 810000 AND NOT (a < 500);

SELECT * FROM tv1 WHERE a < 90000 OR b = 8100000000;

SELECT * FROM t1 WHERE a < 90000
UNION ALL
SELECT * FROM t1 WHERE b = 8100000000 AND NOT (a < 90000);

-- ===vtabE.test===
CREATE VIRTUAL TABLE t1 USING tclvar;
CREATE VIRTUAL TABLE t2 USING tclvar;
CREATE TABLE t3(a INTEGER PRIMARY KEY, b);
SELECT t1.*, t2.*, abs(t3.b + abs(t2.value + abs(t1.value)))
FROM t1 LEFT JOIN t2 ON t2.name = t1.arrayname
LEFT JOIN t3 ON t3.a=t2.value
WHERE t1.name = 'vtabE'
ORDER BY t1.value, t2.value;