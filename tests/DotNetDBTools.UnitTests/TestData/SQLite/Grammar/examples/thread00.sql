-- ===thread001.test===
CREATE TABLE ab(a INTEGER PRIMARY KEY, b);
CREATE INDEX ab_i ON ab(b);
INSERT INTO ab SELECT NULL, md5sum(a, b) FROM ab;
SELECT count(*) FROM ab;

SELECT 
(SELECT md5sum(a, b) FROM ab WHERE a < (SELECT max(a) FROM ab)) ==
(SELECT b FROM ab WHERE a = (SELECT max(a) FROM ab));

PRAGMA integrity_check;

SELECT 
(SELECT md5sum(a, b) FROM ab WHERE a < (SELECT max(a) FROM ab)) ==
(SELECT b FROM ab WHERE a = (SELECT max(a) FROM ab));

INSERT INTO ab SELECT NULL, md5sum(a, b) FROM ab;

SELECT count(*) FROM ab;

SELECT 
(SELECT md5sum(a, b) FROM ab WHERE a < (SELECT max(a) FROM ab)) ==
(SELECT b FROM ab WHERE a = (SELECT max(a) FROM ab));

PRAGMA integrity_check;

-- ===thread002.test===
CREATE TABLE t1(k, v);
CREATE INDEX t1_i ON t1(v);
INSERT INTO t1(v) VALUES(1.0);

SELECT count(*) FROM t1;

PRAGMA integrity_check;

SELECT * FROM aux1.t1;

INSERT INTO aux1.t1(v) SELECT sum(v) FROM aux2.t1;

SELECT * FROM aux2.t1;

INSERT INTO aux2.t1(v) SELECT sum(v) FROM aux3.t1;

SELECT * FROM aux3.t1;

INSERT INTO aux3.t1(v) SELECT sum(v) FROM aux1.t1;

CREATE TABLE IF NOT EXISTS aux1.t2(a,b);

DROP TABLE IF EXISTS aux1.t2;

-- ===thread003.test===
BEGIN;
CREATE TABLE t1(a, b, c);

INSERT INTO t1 VALUES(ii, randomblob(200), randomblob(200));

CREATE INDEX i1 ON t1(a, b); 
COMMIT;

BEGIN;
CREATE TABLE t1(a, b, c);

INSERT INTO t1 VALUES(ii, randomblob(200), randomblob(200));

CREATE INDEX i1 ON t1(a, b); 
COMMIT;

PRAGMA cache_size = 15;

PRAGMA cache_size = 15;

PRAGMA cache_size = 15;

-- ===thread004.test===
CREATE TABLE t1(a, b, c);

-- ===thread005.test===
CREATE TABLE t1(a, b);

ATTACH 'test2.db' AS aux;

CREATE TABLE aux.t1(a INTEGER PRIMARY KEY, b UNIQUE);
INSERT INTO t1 VALUES(1, 1);
INSERT INTO t1 VALUES(2, 2);

ATTACH 'test2.db' AS aux;

ATTACH 'test2.db' AS aux;

SELECT count(*) FROM t1 WHERE b IS NULL