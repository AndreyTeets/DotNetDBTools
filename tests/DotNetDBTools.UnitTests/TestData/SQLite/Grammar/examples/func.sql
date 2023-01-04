-- ===func.test===
CREATE TABLE tbl1(t1 text);

SELECT substr(t1,2,1) FROM tbl1 ORDER BY t1;

SELECT testfunc(
'string', 'abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ',
'int', 1234,
'string', 'abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ',
'string', NULL,
'string', 'abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ',
'double', 1.234,
'string', 'abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ',
'int', 1234,
'string', 'abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ',
'string', NULL,
'string', 'abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ',
'double', 1.234
);

SELECT sqlite_version(*);

PRAGMA encoding;

SELECT test_destructor('hello world'), test_destructor_count();

SELECT test_destructor16('hello world'), test_destructor_count();

SELECT test_destructor_count();

SELECT test_destructor('hello')||' world';

SELECT test_destructor_count();

CREATE TABLE t4(x);
INSERT INTO t4 VALUES(test_destructor('hello'));
INSERT INTO t4 VALUES(test_destructor('world'));
SELECT min(test_destructor(x)), max(test_destructor(x)) FROM t4;

SELECT test_destructor_count();

SELECT substr(t1,3,3) FROM tbl1 ORDER BY t1;

DROP TABLE t4;

SELECT test_auxdata('hello world');

CREATE TABLE t4(a, b);
INSERT INTO t4 VALUES('abc', 'def');
INSERT INTO t4 VALUES('ghi', 'jkl');

SELECT test_auxdata('hello world') FROM t4;

SELECT test_auxdata('hello world', 123) FROM t4;

SELECT test_auxdata('hello world', a) FROM t4;

SELECT test_auxdata('hello'||'world', a) FROM t4;

CREATE TABLE tbl2(a, b);

SELECT quote(a), quote(b) FROM tbl2;

CREATE TABLE t5(x);
INSERT INTO t5 VALUES(1);
INSERT INTO t5 VALUES(-99);
INSERT INTO t5 VALUES(10000);
SELECT sum(x) FROM t5;

SELECT substr(t1,-1,1) FROM tbl1 ORDER BY t1;

INSERT INTO t5 VALUES(0.0);
SELECT sum(x) FROM t5;

DELETE FROM t5;
SELECT sum(x), total(x) FROM t5;

INSERT INTO t5 VALUES(NULL);
SELECT sum(x), total(x) FROM t5;

INSERT INTO t5 VALUES(NULL);
SELECT sum(x), total(x) FROM t5;

INSERT INTO t5 VALUES(123);
SELECT sum(x), total(x) FROM t5;

CREATE TABLE t6(x INTEGER);
INSERT INTO t6 VALUES(1);
INSERT INTO t6 VALUES(1<<62);
SELECT sum(x) - ((1<<62)+1) from t6;

SELECT typeof(sum(x)) FROM t6;

SELECT total(x) - ((1<<62)*2.0+1) FROM t6;

SELECT total(x) - ((1<<62)*2+1) FROM t6;

SELECT sum(-9223372036854775805);

SELECT substr(t1,-1,2) FROM tbl1 ORDER BY t1;

SELECT match(a,b) FROM t1 WHERE 0;

SELECT soundex('hello');

SELECT soundex(name);

SELECT typeof(replace("This is the main test string", NULL, "ALT"));

SELECT typeof(replace(NULL, "main", "ALT"));

SELECT typeof(replace("This is the main test string", "main", NULL));

SELECT replace("This is the main test string", "main", "ALT");

SELECT replace("This is the main test string", "main", "larger-main");

SELECT replace("aaaaaaa", "a", "0123456789");

SELECT LENGTH(REPLACE(str, 'C', rep));

SELECT substr(t1,-2,1) FROM tbl1 ORDER BY t1;

SELECT trim('  hi  ');

SELECT ltrim('  hi  ');

SELECT rtrim('  hi  ');

SELECT trim('  hi  ','xyz');

SELECT ltrim('  hi  ','xyz');

SELECT rtrim('  hi  ','xyz');

SELECT trim('xyxzy  hi  zzzy','xyz');

SELECT ltrim('xyxzy  hi  zzzy','xyz');

SELECT rtrim('xyxzy  hi  zzzy','xyz');

SELECT trim('  hi  ','');

SELECT substr(t1,-2,2) FROM tbl1 ORDER BY t1;

SELECT hex(trim(x'c280e1bfbff48fbfbf6869',x'6162e1bfbfc280'));

SELECT hex(trim(x'6869c280e1bfbff48fbfbf61',
x'6162e1bfbfc280f48fbfbf'));

SELECT hex(trim(x'ceb1ceb2ceb3',x'ceb1'));

SELECT typeof(trim(NULL));

SELECT typeof(trim(NULL,'xyz'));

SELECT typeof(trim('hello',NULL));

SELECT legacy_count() FROM t6;

SELECT group_concat(t1) FROM tbl1;

SELECT group_concat(t1,' ') FROM tbl1;

SELECT group_concat(t1,' ' || rowid || ' ') FROM tbl1;

SELECT substr(t1,-4,2) FROM tbl1 ORDER BY t1;

SELECT group_concat(NULL,t1) FROM tbl1;

SELECT group_concat(t1,NULL) FROM tbl1;

SELECT 'BEGIN-'||group_concat(t1) FROM tbl1;

SELECT group_concat(CASE t1 WHEN 'this' THEN '' ELSE t1 END) FROM tbl1;

SELECT group_concat(CASE WHEN t1!='software' THEN '' ELSE t1 END) FROM tbl1;

SELECT group_concat(CASE t1 WHEN 'this' THEN null ELSE t1 END) FROM tbl1;

SELECT group_concat(CASE WHEN t1!='software' THEN null ELSE t1 END) FROM tbl1;

SELECT group_concat(CASE t1 WHEN 'this' THEN ''
WHEN 'program' THEN null ELSE t1 END) FROM tbl1;

SELECT test_isolation(t1,t1) FROM tbl1;

CREATE TABLE t28(x, y DEFAULT(nosuchfunc(1)));

SELECT t1 FROM tbl1 ORDER BY substr(t1,2,20);

SELECT substr(a,1,1) FROM t2;

SELECT substr(a,2,2) FROM t2;

SELECT t1 FROM tbl1 ORDER BY t1;

DELETE FROM tbl1;

SELECT t1 FROM tbl1 ORDER BY t1;

SELECT length(t1) FROM tbl1 ORDER BY t1;

SELECT substr(t1,1,2) FROM tbl1 ORDER BY t1;

SELECT substr(t1,1,3) FROM tbl1 ORDER BY t1;

SELECT substr(t1,2,2) FROM tbl1 ORDER BY t1;

SELECT substr(t1,2,3) FROM tbl1 ORDER BY t1;

SELECT substr(t1,3,2) FROM tbl1 ORDER BY t1;

SELECT substr(t1,4,2) FROM tbl1 ORDER BY t1;

SELECT substr(t1,-1,1) FROM tbl1 ORDER BY t1;

CREATE TABLE t2(a);
INSERT INTO t2 VALUES(1);
INSERT INTO t2 VALUES(NULL);
INSERT INTO t2 VALUES(345);
INSERT INTO t2 VALUES(NULL);
INSERT INTO t2 VALUES(67890);
SELECT * FROM t2;

SELECT substr(t1,-3,2) FROM tbl1 ORDER BY t1;

SELECT substr(t1,-4,3) FROM tbl1 ORDER BY t1;

DELETE FROM tbl1;

SELECT t1 FROM tbl1;

CREATE TABLE t1(a,b,c);
INSERT INTO t1 VALUES(1,2,3);
INSERT INTO t1 VALUES(2,12345678901234,-1234567890);
INSERT INTO t1 VALUES(3,-2,-5);

CREATE TABLE t1(a,b,c);
INSERT INTO t1 VALUES(1,2,3);
INSERT INTO t1 VALUES(2,1.2345678901234,-12345.67890);
INSERT INTO t1 VALUES(3,-2,-5);

SELECT abs(a) FROM t2;

SELECT abs(t1) FROM tbl1;

SELECT coalesce(round(a,2),'nil') FROM t2;

SELECT round(t1,2) FROM tbl1;

SELECT length(t1) FROM tbl1 ORDER BY t1;

SELECT typeof(round(5.1,1));

SELECT typeof(round(5.1));

SELECT round(x1);

SELECT round(x1,1);

SELECT round(40223.4999999999);

SELECT round(40224.4999999999);

SELECT round(40225.4999999999);

SELECT round(40223.4999999999,i);

SELECT round(40224.4999999999,i);

SELECT round(40225.4999999999,i);

SELECT length(*) FROM tbl1 ORDER BY t1;

SELECT round(40223.4999999999,i);

SELECT round(40224.4999999999,i);

SELECT round(40225.4999999999,i);

SELECT round(1234567890.5);

SELECT round(12345678901.5);

SELECT round(123456789012.5);

SELECT round(1234567890123.5);

SELECT round(12345678901234.5);

SELECT round(1234567890123.35,1);

SELECT round(1234567890123.445,2);

SELECT length(t1,5) FROM tbl1 ORDER BY t1;

SELECT round(99999999999994.5);

SELECT round(9999999999999.55,1);

SELECT round(9999999999999.555,2);

SELECT upper(t1) FROM tbl1;

SELECT lower(upper(t1)) FROM tbl1;

SELECT upper(a), lower(a) FROM t2;

SELECT coalesce(a,'xyz') FROM t2;

SELECT coalesce(upper(a),'nil') FROM t2;

SELECT coalesce(nullif(1,1),'nil');

SELECT coalesce(nullif(1,2),'nil');

SELECT length(t1), count(*) FROM tbl1 GROUP BY length(t1)
ORDER BY length(t1);

SELECT coalesce(nullif(1,NULL),'nil');

SELECT last_insert_rowid();

EXPLAIN SELECT sum(a) FROM t2;

SELECT sum(a), count(a), round(avg(a),2), min(a), max(a), count(*) FROM t2;

EXPLAIN SELECT sum(a) FROM t2;

SELECT sum(a), count(a), avg(a), min(a), max(a), count(*) FROM t2;

SELECT max('z+'||a||'abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOP') FROM t2;

CREATE TEMP TABLE t3 AS SELECT a FROM t2 ORDER BY a DESC;
SELECT min('z+'||a||'abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOP') FROM t3;

CREATE TABLE t3 AS SELECT a FROM t2 ORDER BY a DESC;
SELECT min('z+'||a||'abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOP') FROM t3;

SELECT max('z+'||a||'abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOP') FROM t3;

SELECT coalesce(length(a),-1) FROM t2;

SELECT sum(x) FROM (SELECT '9223372036' || '854775807' AS x
UNION ALL SELECT -9223372036854775807);

SELECT typeof(sum(x)) FROM (SELECT '9223372036' || '854775807' AS x
UNION ALL SELECT -9223372036854775807);

SELECT typeof(sum(x)) FROM (SELECT '9223372036' || '854775808' AS x
UNION ALL SELECT -9223372036854775807);

SELECT sum(x)>0.0 FROM (SELECT '9223372036' || '854775808' AS x
UNION ALL SELECT -9223372036850000000);

SELECT sum(x)>0 FROM (SELECT '9223372036' || '854775808' AS x
UNION ALL SELECT -9223372036850000000);

SELECT random() is not null;

SELECT typeof(random());

SELECT randomblob(32) is not null;

SELECT typeof(randomblob(32));

SELECT length(randomblob(32)), length(randomblob(-5)),
length(randomblob(2000));

SELECT substr(t1,1,2) FROM tbl1 ORDER BY t1;

SELECT hex(x'00112233445566778899aAbBcCdDeEfF');

SELECT hex(replace('abcdefg','ef','12'));

SELECT hex(replace('abcdefg','','12'));

SELECT hex(replace('aabcdefg','a','aaa'));

SELECT hex(replace('abcdefg','ef','12'));

SELECT hex(replace('abcdefg','','12'));

SELECT hex(replace('aabcdefg','a','aaa'));

SELECT testfunc(
'string', 'abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ',
'int', 1234
);

SELECT testfunc(
'string', 'abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ',
'string', NULL
);

SELECT testfunc(
'string', 'abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ',
'double', 1.234
);

-- ===func2.test===
SELECT 'Supercalifragilisticexpialidocious';

SELECT SUBSTR('Supercalifragilisticexpialidocious', -1);

SELECT SUBSTR('Supercalifragilisticexpialidocious', -2);

SELECT SUBSTR('Supercalifragilisticexpialidocious', -30);

SELECT SUBSTR('Supercalifragilisticexpialidocious', -34);

SELECT SUBSTR('Supercalifragilisticexpialidocious', -35);

SELECT SUBSTR('Supercalifragilisticexpialidocious', -36);

SELECT SUBSTR('Supercalifragilisticexpialidocious', 0, 1);

SELECT SUBSTR('Supercalifragilisticexpialidocious', 0, 2);

SELECT SUBSTR('Supercalifragilisticexpialidocious', 1, 1);

SELECT SUBSTR('Supercalifragilisticexpialidocious', 2, 0);

SELECT SUBSTR('Supercalifragilisticexpialidocious', 0);

SELECT SUBSTR('Supercalifragilisticexpialidocious', 2, 1);

SELECT SUBSTR('Supercalifragilisticexpialidocious', 2, 2);

SELECT SUBSTR('Supercalifragilisticexpialidocious', 30, 1);

SELECT SUBSTR('Supercalifragilisticexpialidocious', 34, 1);

SELECT SUBSTR('Supercalifragilisticexpialidocious', 35, 1);

SELECT SUBSTR('Supercalifragilisticexpialidocious', 36, 1);

SELECT SUBSTR('Supercalifragilisticexpialidocious', -0, 1);

SELECT SUBSTR('Supercalifragilisticexpialidocious', -1, 0);

SELECT SUBSTR('Supercalifragilisticexpialidocious', -1, 1);

SELECT SUBSTR('Supercalifragilisticexpialidocious', -1, 2);

SELECT SUBSTR('Supercalifragilisticexpialidocious', 1);

SELECT SUBSTR('Supercalifragilisticexpialidocious', -2, 1);

SELECT SUBSTR('Supercalifragilisticexpialidocious', -30, 1);

SELECT SUBSTR('Supercalifragilisticexpialidocious', -34, 0);

SELECT SUBSTR('Supercalifragilisticexpialidocious', -34, 1);

SELECT SUBSTR('Supercalifragilisticexpialidocious', -34, 2);

SELECT SUBSTR('Supercalifragilisticexpialidocious', -35, 1);

SELECT SUBSTR('Supercalifragilisticexpialidocious', -35, 2);

SELECT SUBSTR('Supercalifragilisticexpialidocious', -36, 0);

SELECT SUBSTR('Supercalifragilisticexpialidocious', -36, 1);

SELECT SUBSTR('Supercalifragilisticexpialidocious', -36, 2);

SELECT SUBSTR('Supercalifragilisticexpialidocious', 2);

SELECT SUBSTR('Supercalifragilisticexpialidocious', -36, 3);

SELECT SUBSTR('Supercalifragilisticexpialidocious', 0, 0);

SELECT SUBSTR('Supercalifragilisticexpialidocious', 0, -1);

SELECT SUBSTR('Supercalifragilisticexpialidocious', 0, -2);

SELECT SUBSTR('Supercalifragilisticexpialidocious', 1, 0);

SELECT SUBSTR('Supercalifragilisticexpialidocious', 1, -1);

SELECT SUBSTR('Supercalifragilisticexpialidocious', 2, 0);

SELECT SUBSTR('Supercalifragilisticexpialidocious', 2, -1);

SELECT SUBSTR('Supercalifragilisticexpialidocious', 2, -2);

SELECT SUBSTR('Supercalifragilisticexpialidocious', 3, 0);

SELECT SUBSTR('Supercalifragilisticexpialidocious', 30);

SELECT SUBSTR('Supercalifragilisticexpialidocious', 3, -1);

SELECT SUBSTR('Supercalifragilisticexpialidocious', 3, -2);

SELECT SUBSTR('Supercalifragilisticexpialidocious', 30, -1);

SELECT SUBSTR('Supercalifragilisticexpialidocious', 30, -2);

SELECT SUBSTR('Supercalifragilisticexpialidocious', 34, -1);

SELECT SUBSTR('Supercalifragilisticexpialidocious', 35, -1);

SELECT SUBSTR('Supercalifragilisticexpialidocious', 36, 0);

SELECT SUBSTR('Supercalifragilisticexpialidocious', 36, -1);

SELECT SUBSTR('Supercalifragilisticexpialidocious', 36, -2);

SELECT SUBSTR('Supercalifragilisticexpialidocious', 34);

SELECT SUBSTR('Supercalifragilisticexpialidocious', 35);

SELECT SUBSTR('Supercalifragilisticexpialidocious', 36);

SELECT SUBSTR('Supercalifragilisticexpialidocious', -0)