-- ===join.test===
CREATE TABLE t1(a,b,c);
INSERT INTO t1 VALUES(1,2,3);
INSERT INTO t1 VALUES(2,3,4);
INSERT INTO t1 VALUES(3,4,5);
SELECT * FROM t1;

CREATE TABLE t3(c,d,e);
INSERT INTO t3 VALUES(2,3,4);
INSERT INTO t3 VALUES(3,4,5);
INSERT INTO t3 VALUES(4,5,6);
SELECT * FROM t3;

SELECT * FROM t1 natural join t2 natural join t3;

CREATE TABLE t4(d,e,f);
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

SELECT * FROM t1 LEFT JOIN t2 ON t1.a=t2.d WHERE t2.b IS NULL OR t2.b>1;

CREATE TABLE t2(b,c,d);
INSERT INTO t2 VALUES(1,2,3);
INSERT INTO t2 VALUES(2,3,4);
INSERT INTO t2 VALUES(3,4,5);
SELECT * FROM t2;

BEGIN;
CREATE TABLE t5(a INTEGER PRIMARY KEY);
CREATE TABLE t6(a INTEGER);
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

SELECT * FROM t6, t5 WHERE t6.a>t5.a;

SELECT b FROM t1 NATURAL JOIN t2;

BEGIN;
create table centros (id integer primary key, centro);
INSERT INTO centros VALUES(1,'xxx');
create table usuarios (id integer primary key, nombre, apellidos,
idcentro integer);
INSERT INTO usuarios VALUES(1,'a','aa',1);
INSERT INTO usuarios VALUES(2,'b','bb',1);
INSERT INTO usuarios VALUES(3,'c','cc',NULL);
create index idcentro on usuarios (idcentro);
END;
select usuarios.id, usuarios.nombre, centros.centro from
usuarios left outer join centros on usuarios.idcentro = centros.id;

CREATE TABLE t7 (x, y);
INSERT INTO t7 VALUES ("pa1", 1);
INSERT INTO t7 VALUES ("pa2", NULL);
INSERT INTO t7 VALUES ("pa3", NULL);
INSERT INTO t7 VALUES ("pa4", 2);
INSERT INTO t7 VALUES ("pa30", 131);
INSERT INTO t7 VALUES ("pa31", 130);
INSERT INTO t7 VALUES ("pa28", NULL);
CREATE TABLE t8 (a integer primary key, b);
INSERT INTO t8 VALUES (1, "pa1");
INSERT INTO t8 VALUES (2, "pa4");
INSERT INTO t8 VALUES (3, NULL);
INSERT INTO t8 VALUES (4, NULL);
INSERT INTO t8 VALUES (130, "pa31");
INSERT INTO t8 VALUES (131, "pa30");
SELECT coalesce(t8.a,999) from t7 LEFT JOIN t8 on y=a;

BEGIN;
CREATE TABLE t9(a INTEGER PRIMARY KEY, b);
INSERT INTO t9 VALUES(1,11);
INSERT INTO t9 VALUES(2,22);
CREATE TABLE t10(x INTEGER PRIMARY KEY, y);
INSERT INTO t10 VALUES(1,2);
INSERT INTO t10 VALUES(3,3);    
CREATE TABLE t11(p INTEGER PRIMARY KEY, q);
INSERT INTO t11 VALUES(2,111);
INSERT INTO t11 VALUES(3,333);    
CREATE VIEW v10_11 AS SELECT x, q FROM t10, t11 WHERE t10.y=t11.p;
COMMIT;
SELECT * FROM t9 LEFT JOIN v10_11 ON( a=x );

SELECT * FROM t9 LEFT JOIN (SELECT x, q FROM t10, t11 WHERE t10.y=t11.p)
ON( a=x);

SELECT * FROM v10_11 LEFT JOIN t9 ON( a=x );

SELECT * FROM t9 LEFT JOIN (SELECT 44, p, q FROM t11) AS sub1 ON p=a;

BEGIN;
CREATE TABLE t12(a,b);
INSERT INTO t12 VALUES(1,11);
INSERT INTO t12 VALUES(2,22);
CREATE TABLE t13(b,c);
INSERT INTO t13 VALUES(22,222);
COMMIT;

SELECT * FROM t12 NATURAL LEFT JOIN t13
EXCEPT
SELECT * FROM t12 NATURAL LEFT JOIN (SELECT * FROM t13 WHERE b>0);

CREATE VIEW v13 AS SELECT * FROM t13 WHERE b>0;
SELECT * FROM t12 NATURAL LEFT JOIN t13
EXCEPT
SELECT * FROM t12 NATURAL LEFT JOIN v13;

CREATE TABLE t21(a,b,c);
CREATE TABLE t22(p,q);
CREATE INDEX i22 ON t22(q);
SELECT a FROM t21 LEFT JOIN t22 ON b=p WHERE q=
(SELECT max(m.q) FROM t22 m JOIN t21 n ON n.b=m.p WHERE n.c=1);

SELECT b FROM t1 JOIN t2 USING(b);

CREATE TABLE t23(a, b, c);
CREATE TABLE t24(a, b, c);
INSERT INTO t23 VALUES(1, 2, 3);

SELECT * FROM t23 LEFT JOIN t24;

SELECT * FROM t23 LEFT JOIN (SELECT * FROM t24);

CREATE TABLE t1(a INTEGER PRIMARY KEY, b TEXT);
CREATE TABLE t2(a INTEGER PRIMARY KEY, b TEXT);
INSERT INTO t1 VALUES(1,'abc');
INSERT INTO t1 VALUES(2,'def');
INSERT INTO t2 VALUES(1,'abc');
INSERT INTO t2 VALUES(2,'def');
SELECT * FROM t1 NATURAL JOIN t2;

SELECT a FROM t1 JOIN t1 USING (a);

SELECT a FROM t1 JOIN t1 AS t2 USING (a);

SELECT * FROM t1 NATURAL JOIN t1 AS t2;

SELECT * FROM t1 NATURAL JOIN t1;

CREATE TABLE t1(a COLLATE nocase, b);
CREATE TABLE t2(a, b);
INSERT INTO t1 VALUES('ONE', 1);
INSERT INTO t1 VALUES('two', 2);
INSERT INTO t2 VALUES('one', 1);
INSERT INTO t2 VALUES('two', 2);

SELECT * FROM t1 NATURAL JOIN t2;

SELECT * FROM t1 NATURAL CROSS JOIN t2;

SELECT * FROM t2 NATURAL JOIN t1;

CREATE TABLE t1(a, b TEXT);
CREATE TABLE t2(b INTEGER, a);
INSERT INTO t1 VALUES('one', '1.0');
INSERT INTO t1 VALUES('two', '2');
INSERT INTO t2 VALUES(1, 'one');
INSERT INTO t2 VALUES(2, 'two');

SELECT * FROM t1 NATURAL JOIN t2;

SELECT * FROM t2 NATURAL JOIN t1;

SELECT * FROM t1 CROSS JOIN t2 USING(b,c);

SELECT * FROM t1 NATURAL INNER JOIN t2;

SELECT * FROM t1 INNER JOIN t2 USING(b,c);

SELECT * FROM t1 natural inner join t2;

-- ===join2.test===
CREATE TABLE t1(a,b);
INSERT INTO t1 VALUES(1,11);
INSERT INTO t1 VALUES(2,22);
INSERT INTO t1 VALUES(3,33);
SELECT * FROM t1;

CREATE TABLE t2(b,c);
INSERT INTO t2 VALUES(11,111);
INSERT INTO t2 VALUES(33,333);
INSERT INTO t2 VALUES(44,444);
SELECT * FROM t2;

CREATE TABLE t3(c,d);
INSERT INTO t3 VALUES(111,1111);
INSERT INTO t3 VALUES(444,4444);
INSERT INTO t3 VALUES(555,5555);
SELECT * FROM t3;

SELECT * FROM
t1 NATURAL JOIN t2 NATURAL JOIN t3;

SELECT * FROM
t1 NATURAL JOIN t2 NATURAL LEFT OUTER JOIN t3;

SELECT * FROM
t1 NATURAL LEFT OUTER JOIN t2 NATURAL JOIN t3;

SELECT * FROM
t1 NATURAL LEFT OUTER JOIN (t2 NATURAL JOIN t3);

-- ===join4.test===
create temp table t1(a integer, b varchar(10));
insert into t1 values(1,'one');
insert into t1 values(2,'two');
insert into t1 values(3,'three');
insert into t1 values(4,'four');
create temp table t2(x integer, y varchar(10), z varchar(10));
insert into t2 values(2,'niban','ok');
insert into t2 values(4,'yonban','err');

select * from t1 left outer join t2 on t1.a=t2.x and t2.z>='ok';

select * from t1 left outer join t2 on t1.a=t2.x where t2.z IN ('ok');

select * from t1 left outer join t2 on t1.a=t2.x and t2.z IN ('ok');

select * from t1 left outer join t2 on t1.a=t2.x where t2.z='ok';

create table t1(a integer, b varchar(10));
insert into t1 values(1,'one');
insert into t1 values(2,'two');
insert into t1 values(3,'three');
insert into t1 values(4,'four');
create table t2(x integer, y varchar(10), z varchar(10));
insert into t2 values(2,'niban','ok');
insert into t2 values(4,'yonban','err');

select * from t1 left outer join t2 on t1.a=t2.x where t2.z='ok';

select * from t1 left outer join t2 on t1.a=t2.x and t2.z='ok';

create index i2 on t2(z);

select * from t1 left outer join t2 on t1.a=t2.x where t2.z='ok';

select * from t1 left outer join t2 on t1.a=t2.x and t2.z='ok';

select * from t1 left outer join t2 on t1.a=t2.x where t2.z>='ok';

-- ===join5.test===
BEGIN;
CREATE TABLE t1(a integer primary key, b integer, c integer);
CREATE TABLE t2(x integer primary key, y);
CREATE TABLE t3(p integer primary key, q);
INSERT INTO t3 VALUES(11,'t3-11');
INSERT INTO t3 VALUES(12,'t3-12');
INSERT INTO t2 VALUES(11,'t2-11');
INSERT INTO t2 VALUES(12,'t2-12');
INSERT INTO t1 VALUES(1, 5, 0);
INSERT INTO t1 VALUES(2, 11, 2);
INSERT INTO t1 VALUES(3, 12, 1);
COMMIT;

SELECT * FROM xy LEFT JOIN ab ON 0 WHERE 0;

SELECT * FROM xy LEFT JOIN ab ON 1 WHERE 0;

SELECT * FROM xy LEFT JOIN ab ON NULL WHERE 0;

SELECT * FROM xy LEFT JOIN ab ON 0 WHERE 1;

SELECT * FROM xy LEFT JOIN ab ON 1 WHERE 1;

SELECT * FROM xy LEFT JOIN ab ON NULL WHERE 1;

SELECT * FROM xy LEFT JOIN ab ON 0 WHERE NULL;

SELECT * FROM xy LEFT JOIN ab ON 1 WHERE NULL;

SELECT * FROM xy LEFT JOIN ab ON NULL WHERE NULL;

select * from t1 left join t2 on t1.b=t2.x and t1.c=1;

select * from t1 left join t2 on t1.b=t2.x where t1.c=1;

select * from t1 left join t2 on t1.b=t2.x and t1.c=1
left join t3 on t1.b=t3.p and t1.c=2;

select * from t1 left join t2 on t1.b=t2.x and t1.c=1
left join t3 on t1.b=t3.p where t1.c=2;

CREATE TABLE ab(a,b);
INSERT INTO "ab" VALUES(1,2);
INSERT INTO "ab" VALUES(3,NULL);
CREATE TABLE xy(x,y);
INSERT INTO "xy" VALUES(2,3);
INSERT INTO "xy" VALUES(NULL,1);

SELECT * FROM xy LEFT JOIN ab ON 0;

SELECT * FROM xy LEFT JOIN ab ON 1;

SELECT * FROM xy LEFT JOIN ab ON NULL;

-- ===join6.test===
CREATE TABLE t1(a);
CREATE TABLE t2(a);
CREATE TABLE t3(a,b);
INSERT INTO t1 VALUES(1);
INSERT INTO t3 VALUES(1,2);
SELECT * FROM t1 LEFT JOIN t2 USING(a) LEFT JOIN t3 USING(a);

SELECT * FROM t1 NATURAL JOIN t2 JOIN t3 USING(x);

SELECT * FROM t1 NATURAL JOIN t2 JOIN t3 USING(z);

SELECT * FROM
(SELECT 1 AS a, 91 AS x, 92 AS y UNION SELECT 2, 93, 94)
NATURAL JOIN t2 NATURAL JOIN t3;

SELECT * FROM t1 NATURAL JOIN
(SELECT 3 AS b, 92 AS y, 93 AS z UNION SELECT 4, 94, 95)
NATURAL JOIN t3;

SELECT * FROM t1 NATURAL JOIN t2 NATURAL JOIN
(SELECT 5 AS c, 91 AS x, 93 AS z UNION SELECT 6, 99, 95);

SELECT t1.a, t3.b 
FROM t1 LEFT JOIN t2 ON t1.a=t2.a LEFT JOIN t3 ON t2.a=t3.a;

SELECT t1.a, t3.b
FROM t1 LEFT JOIN t2 ON t1.a=t2.a LEFT JOIN t3 ON t1.a=t3.a;

DROP TABLE t1;
DROP TABLE t2;
DROP TABLE t3;
CREATE TABLE t1(x,y);
CREATE TABLE t2(y,z);
CREATE TABLE t3(x,z);
INSERT INTO t1 VALUES(1,2);
INSERT INTO t1 VALUES(3,4);
INSERT INTO t2 VALUES(2,3);
INSERT INTO t2 VALUES(4,5);
INSERT INTO t3 VALUES(1,3);
INSERT INTO t3 VALUES(3,5);
SELECT * FROM t1 JOIN t2 USING (y) JOIN t3 USING(x);

SELECT * FROM t1 NATURAL JOIN t2 NATURAL JOIN t3;

DROP TABLE t1;
DROP TABLE t2;
DROP TABLE t3;
CREATE TABLE t1(a,x,y);
INSERT INTO t1 VALUES(1,91,92);
INSERT INTO t1 VALUES(2,93,94);
CREATE TABLE t2(b,y,z);
INSERT INTO t2 VALUES(3,92,93);
INSERT INTO t2 VALUES(4,94,95);
CREATE TABLE t3(c,x,z);
INSERT INTO t3 VALUES(5,91,93);
INSERT INTO t3 VALUES(6,99,95);
SELECT * FROM t1 NATURAL JOIN t2 NATURAL JOIN t3;

SELECT * FROM t1 JOIN t2 NATURAL JOIN t3;

SELECT * FROM t1 JOIN t2 USING(y) NATURAL JOIN t3;

SELECT * FROM t1 NATURAL JOIN t2 JOIN t3 USING(x,z);