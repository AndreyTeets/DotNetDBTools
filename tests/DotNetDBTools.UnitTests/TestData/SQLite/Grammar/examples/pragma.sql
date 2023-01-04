-- ===pragma.test===
PRAGMA cache_size;
PRAGMA default_cache_size;
PRAGMA synchronous;

VACUUM;
PRAGMA cache_size;
PRAGMA default_cache_size;
PRAGMA synchronous;

BEGIN;
PRAGMA aux.user_version = 10;
PRAGMA user_version = 11;

PRAGMA aux.user_version;

PRAGMA main.user_version;

ROLLBACK;
PRAGMA aux.user_version;

PRAGMA main.user_version;

PRAGMA user_version = -450;

PRAGMA user_version;

CREATE TEMP TABLE IF NOT EXISTS a(b);

PRAGMA database_list;

PRAGMA temp_store;

PRAGMA synchronous=NORMAL;
PRAGMA cache_size;
PRAGMA default_cache_size;
PRAGMA synchronous;

PRAGMA temp_store=file;
PRAGMA temp_store;

PRAGMA temp_store=memory;
PRAGMA temp_store;

PRAGMA temp_store_directory;

PRAGMA temp_store_directory;

PRAGMA temp_store_directory='';

PRAGMA temp_store_directory;
PRAGMA temp_store=FILE;
CREATE TEMP TABLE temp_store_directory_test(a integer);
INSERT INTO temp_store_directory_test values (2);
SELECT * FROM temp_store_directory_test;

PRAGMA temp_store = 0;
PRAGMA temp_store;

PRAGMA temp_store = 1;
PRAGMA temp_store;

PRAGMA temp_store = 2;
PRAGMA temp_store;

PRAGMA temp_store = 3;
PRAGMA temp_store;

PRAGMA synchronous=FULL;
PRAGMA cache_size;
PRAGMA default_cache_size;
PRAGMA synchronous;

SELECT * FROM temp_table;
COMMIT;

INSERT INTO temp_table VALUES('valuable data II');
SELECT * FROM temp_table;

SELECT t FROM temp_table;

pragma temp_store = 1;

PRAGMA count_changes = 1;
CREATE TABLE t1(a PRIMARY KEY);
CREATE TABLE t1_mirror(a);
CREATE TABLE t1_mirror2(a);
CREATE TRIGGER t1_bi BEFORE INSERT ON t1 BEGIN 
INSERT INTO t1_mirror VALUES(new.a);
END;
CREATE TRIGGER t1_ai AFTER INSERT ON t1 BEGIN 
INSERT INTO t1_mirror2 VALUES(new.a);
END;
CREATE TRIGGER t1_bu BEFORE UPDATE ON t1 BEGIN 
UPDATE t1_mirror SET a = new.a WHERE a = old.a;
END;
CREATE TRIGGER t1_au AFTER UPDATE ON t1 BEGIN 
UPDATE t1_mirror2 SET a = new.a WHERE a = old.a;
END;
CREATE TRIGGER t1_bd BEFORE DELETE ON t1 BEGIN 
DELETE FROM t1_mirror WHERE a = old.a;
END;
CREATE TRIGGER t1_ad AFTER DELETE ON t1 BEGIN 
DELETE FROM t1_mirror2 WHERE a = old.a;
END;

INSERT INTO t1 VALUES(randstr(10,10));

UPDATE t1 SET a = randstr(10,10);

DELETE FROM t1;

pragma collation_list;

PRAGMA temp.table_info('abc');

PRAGMA cache_size;
PRAGMA default_cache_size;
PRAGMA synchronous;

PRAGMA temp.default_cache_size = 200;
PRAGMA temp.default_cache_size;

PRAGMA temp.cache_size = 400;
PRAGMA temp.cache_size;

DROP TABLE IF EXISTS t4;
PRAGMA vdbe_trace=on;
PRAGMA vdbe_listing=on;
PRAGMA sql_trace=on;
CREATE TABLE t4(a INTEGER PRIMARY KEY,b);
INSERT INTO t4(b) VALUES(x'0123456789abcdef0123456789abcdef0123456789');
INSERT INTO t4(b) VALUES(randstr(30,30));
INSERT INTO t4(b) VALUES(1.23456);
INSERT INTO t4(b) VALUES(NULL);
INSERT INTO t4(b) VALUES(0);
INSERT INTO t4(b) SELECT b||b||b||b FROM t4;
SELECT * FROM t4;

PRAGMA vdbe_trace=off;
PRAGMA vdbe_listing=off;
PRAGMA sql_trace=off;

pragma auto_vacuum = 0;

pragma page_count;

CREATE TABLE abc(a, b, c);
PRAGMA page_count;

BEGIN;
CREATE TABLE def(a, b, c);
PRAGMA page_count;

ROLLBACK;
PRAGMA page_count;

PRAGMA auto_vacuum = 0;
CREATE TABLE t1(a, b, c);
CREATE TABLE t2(a, b, c);
CREATE TABLE t3(a, b, c);
CREATE TABLE t4(a, b, c);

PRAGMA synchronous=0;
PRAGMA synchronous;

ATTACH 'test2.db' AS aux;
PRAGMA aux.page_count;

PRAGMA cache_size=59;
PRAGMA cache_size;

CREATE TABLE newtable(a, b, c);

SELECT * FROM sqlite_master;

PRAGMA cache_size;

PRAGMA lock_proxy_file="mylittleproxy";
select * from sqlite_master;

PRAGMA lock_proxy_file;

PRAGMA lock_proxy_file="mylittleproxy";

PRAGMA lock_proxy_file=":auto:";
select * from sqlite_master;

PRAGMA lock_proxy_file;

PRAGMA synchronous=2;
PRAGMA synchronous;

PRAGMA lock_proxy_file="myotherproxy";

PRAGMA lock_proxy_file="myoriginalproxy";
PRAGMA lock_proxy_file="myotherproxy";
PRAGMA lock_proxy_file;

PRAGMA lock_proxy_file=":auto:";
PRAGMA lock_proxy_file;

PRAGMA lock_proxy_file=":auto:";
PRAGMA lock_proxy_file;

PRAGMA lock_proxy_file=":auto:";
select * from sqlite_master;

select * from sqlite_master;

PRAGMA lock_proxy_file="yetanotherproxy";
PRAGMA lock_proxy_file;

create table mine(x);

PRAGMA lock_proxy_file=":auto:";
PRAGMA lock_proxy_file;

PRAGMA vdbe_listing=YES;
PRAGMA vdbe_listing;

PRAGMA vdbe_listing=NO;
PRAGMA vdbe_listing;

PRAGMA parser_trace=ON;
PRAGMA parser_trace=OFF;

PRAGMA bogus = -1234;  -- Parsing of negative values;

PRAGMA synchronous=OFF;
PRAGMA cache_size=1234;
PRAGMA cache_size;
PRAGMA default_cache_size;
PRAGMA synchronous;

ATTACH 'test2.db' AS aux;

pragma aux.synchronous;

pragma aux.synchronous = OFF;
pragma aux.synchronous;
pragma synchronous;

pragma aux.synchronous = ON;
pragma synchronous;
pragma aux.synchronous;

PRAGMA auto_vacuum=OFF;
BEGIN;
CREATE TABLE t2(a,b,c);
CREATE INDEX i2 ON t2(a);
INSERT INTO t2 VALUES(11,2,3);
INSERT INTO t2 VALUES(22,3,4);
COMMIT;
SELECT rowid, * from t2;

SELECT rootpage FROM sqlite_master WHERE name='i2';

PRAGMA page_size;

PRAGMA integrity_check;

PRAGMA integrity_check=1;

ATTACH DATABASE 'test.db' AS t2;
PRAGMA integrity_check;

PRAGMA cache_size;
PRAGMA default_cache_size;
PRAGMA synchronous;

PRAGMA integrity_check=4;

PRAGMA integrity_check=xyz;

PRAGMA integrity_check=0;

DETACH t2;

REINDEX t2;

PRAGMA integrity_check;

PRAGMA quick_check;

ATTACH 'testerr.db' AS t2;
PRAGMA integrity_check;

PRAGMA integrity_check=1;

PRAGMA integrity_check=5;

PRAGMA synchronous=OFF;
PRAGMA cache_size;
PRAGMA default_cache_size;
PRAGMA synchronous;

PRAGMA integrity_check=4;

PRAGMA integrity_check=3;

PRAGMA integrity_check(2);

ATTACH 'testerr.db' AS t3;
PRAGMA integrity_check;

PRAGMA integrity_check(10);

PRAGMA integrity_check=8;

PRAGMA integrity_check=4;

PRAGMA integrity_check;

ATTACH 'test2.db' AS aux;
pragma aux.cache_size;
pragma aux.default_cache_size;

pragma aux.cache_size = 50;
pragma aux.cache_size;
pragma aux.default_cache_size;

PRAGMA cache_size=-4321;
PRAGMA cache_size;
PRAGMA default_cache_size;
PRAGMA synchronous;

pragma aux.default_cache_size = 456;
pragma aux.cache_size;
pragma aux.default_cache_size;

pragma cache_size;
pragma default_cache_size;

DETACH aux;
ATTACH 'test3.db' AS aux;
pragma aux.cache_size;
pragma aux.default_cache_size;

DETACH aux;
ATTACH 'test2.db' AS aux;
pragma aux.cache_size;
pragma aux.default_cache_size;

pragma synchronous;

pragma synchronous;

SELECT * FROM sqlite_temp_master;

pragma database_list;

CREATE TABLE t2(a,b,c);
pragma table_info(t2);

pragma table_info;

PRAGMA synchronous=ON;
PRAGMA cache_size;
PRAGMA default_cache_size;
PRAGMA synchronous;

CREATE TABLE t5(
a TEXT DEFAULT CURRENT_TIMESTAMP, 
b DEFAULT (5+3),
c TEXT,
d INTEGER DEFAULT NULL,
e TEXT DEFAULT ''
);
PRAGMA table_info(t5);

CREATE TABLE t3(a int references t2(b), b UNIQUE);
pragma foreign_key_list(t3);

pragma foreign_key_list;

pragma foreign_key_list(t3_bogus);

pragma foreign_key_list(t5);

pragma index_list(t3);

CREATE TABLE t3(a,b UNIQUE);

CREATE INDEX t3i1 ON t3(a,b);
pragma index_info(t3i1);

pragma index_info(t3i1_bogus);

CREATE TABLE trial(col_main);
CREATE TEMP TABLE trial(col_temp);

PRAGMA cache_size;
PRAGMA default_cache_size;
PRAGMA synchronous;

PRAGMA table_info(trial);

PRAGMA temp.table_info(trial);

PRAGMA main.table_info(trial);

CREATE TABLE test_table(
one INT NOT NULL DEFAULT -1, 
two text,
three VARCHAR(45, 65) DEFAULT 'abcde',
four REAL DEFAULT X'abcdef',
five DEFAULT CURRENT_TIME
);
PRAGMA table_info(test_table);

pragma index_list(t3);

pragma index_list(t3_bogus);

pragma lock_status;

pragma lock_status;

PRAGMA schema_version = 105;

PRAGMA schema_version = 106;

PRAGMA default_cache_size=-123;
PRAGMA cache_size;
PRAGMA default_cache_size;
PRAGMA synchronous;

PRAGMA schema_version;

CREATE TABLE t4(a, b, c);
INSERT INTO t4 VALUES(1, 2, 3);
SELECT * FROM t4;

PRAGMA schema_version;

SELECT * FROM t4;

PRAGMA schema_version = 108;

ATTACH 'test2.db' AS aux;
CREATE TABLE aux.t1(a, b, c);
PRAGMA aux.schema_version = 205;

PRAGMA aux.schema_version;

PRAGMA schema_version;

ATTACH 'test2.db' AS aux;
SELECT * FROM aux.t1;

PRAGMA aux.schema_version = 206;

PRAGMA cache_size;
PRAGMA default_cache_size;
PRAGMA synchronous;

PRAGMA user_version = 2;

PRAGMA user_version;

PRAGMA schema_version;

VACUUM;
PRAGMA user_version;

PRAGMA schema_version;

ATTACH 'test2.db' AS aux;

PRAGMA aux.user_version;

PRAGMA aux.user_version = 3;

PRAGMA aux.user_version;

PRAGMA main.user_version;

-- ===pragma2.test===
PRAGMA auto_vacuum=0;

PRAGMA aux.freelist_count;
PRAGMA main.freelist_count;
PRAGMA freelist_count;

PRAGMA freelist_count = 500;
PRAGMA freelist_count;

PRAGMA aux.freelist_count = 500;
PRAGMA aux.freelist_count;

PRAGMA freelist_count;

CREATE TABLE abc(a, b, c);
PRAGMA freelist_count;

DROP TABLE abc;
PRAGMA freelist_count;

PRAGMA main.freelist_count;

ATTACH 'test2.db' AS aux;
PRAGMA aux.auto_vacuum=OFF;
PRAGMA aux.freelist_count;

CREATE TABLE aux.abc(a, b, c);
PRAGMA aux.freelist_count;

INSERT INTO aux.abc VALUES(1, 2, val);
PRAGMA aux.freelist_count;

DELETE FROM aux.abc;
PRAGMA aux.freelist_count;