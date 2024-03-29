-- ===null.test===
begin;
create table t1(a,b,c);
insert into t1 values(1,0,0);
insert into t1 values(2,0,1);
insert into t1 values(3,1,0);
insert into t1 values(4,1,1);
insert into t1 values(5,null,0);
insert into t1 values(6,null,1);
insert into t1 values(7,null,null);
commit;
select * from t1;

select ifnull(case b when c then 1 else 0 end, 99) from t1;

select ifnull(case c when b then 1 else 0 end, 99) from t1;

select count(*), count(b), count(c), sum(b), sum(c), 
avg(b), avg(c), min(b), max(b) from t1;

SELECT sum(b), total(b) FROM t1 WHERE b<0;

select a from t1 where b<10;

select a from t1 where not b>10;

select a from t1 where b<10 or c=1;

select a from t1 where b<10 and c=1;

select a from t1 where not (b<10 and c=1);

select distinct b from t1 order by b;

select ifnull(a+b,99) from t1;

select b from t1 union select c from t1 order by b;

select b from t1 union select c from t1 order by 1;

select b from t1 union select c from t1 order by t1.b;

select b from t1 union select c from t1 order by main.t1.b;

create table t2(a, b unique on conflict ignore);
insert into t2 values(1,1);
insert into t2 values(2,null);
insert into t2 values(3,null);
insert into t2 values(4,1);
select a from t2;

create table t3(a, b, c, unique(b,c) on conflict ignore);
insert into t3 values(1,1,1);
insert into t3 values(2,null,1);
insert into t3 values(3,null,1);
insert into t3 values(4,1,1);
select a from t3;

CREATE TABLE t4(x,y);
INSERT INTO t4 VALUES(1,11);
INSERT INTO t4 VALUES(2,NULL);
SELECT x FROM t4 WHERE y=NULL;

SELECT x FROM t4 WHERE y IN (33,NULL);

SELECT x FROM t4 WHERE y<33 ORDER BY x;

SELECT x FROM t4 WHERE y>6 ORDER BY x;

select ifnull(b*c,99) from t1;

SELECT x FROM t4 WHERE y!=33 ORDER BY x;

CREATE INDEX t4i1 ON t4(y);
SELECT x FROM t4 WHERE y=NULL;

SELECT x FROM t4 WHERE y IN (33,NULL);

SELECT x FROM t4 WHERE y<33 ORDER BY x;

SELECT x FROM t4 WHERE y>6 ORDER BY x;

SELECT x FROM t4 WHERE y!=33 ORDER BY x;

select ifnull(case when b<>0 then 1 else 0 end, 99) from t1;

select ifnull(case when not b<>0 then 1 else 0 end, 99) from t1;

select ifnull(case when b<>0 and c<>0 then 1 else 0 end, 99) from t1;

select ifnull(case when not (b<>0 and c<>0) then 1 else 0 end, 99) from t1;

select ifnull(case when b<>0 or c<>0 then 1 else 0 end, 99) from t1;

select ifnull(case when not (b<>0 or c<>0) then 1 else 0 end, 99) from t1;