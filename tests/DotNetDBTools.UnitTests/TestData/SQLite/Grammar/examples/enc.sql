-- ===enc.test===
CREATE TABLE ab(a COLLATE test_collate, b);
INSERT INTO ab VALUES(CAST (X'C388' AS TEXT), X'888800');
INSERT INTO ab VALUES(CAST (X'C0808080808080808080808080808080808080808080808080808080808080808080808080808080808080808080808080808080808388' AS TEXT), X'888800');
CREATE INDEX ab_i ON ab(a, b);

SELECT count(*) FROM ab WHERE a = cp200;

-- ===enc2.test===
SELECT * FROM t1;

PRAGMA encoding;

PRAGMA encoding=UTF8;

PRAGMA encoding;

PRAGMA encoding=UTF16le;

PRAGMA encoding;

PRAGMA encoding=UTF16be;

PRAGMA encoding;

CREATE TABLE t5(a);
INSERT INTO t5 VALUES('one');
INSERT INTO t5 VALUES('two');
INSERT INTO t5 VALUES('five');
INSERT INTO t5 VALUES('three');
INSERT INTO t5 VALUES('four');

SELECT * FROM t5 ORDER BY 1 COLLATE test_collate;

SELECT * FROM t5 ORDER BY 1 COLLATE test_collate;

INSERT INTO t1 VALUES('two', 'II', 2);

SELECT * FROM t5 ORDER BY 1 COLLATE test_collate;

pragma encoding = 'UTF-16LE';

CREATE TABLE t5(a);
INSERT INTO t5 VALUES('one');
INSERT INTO t5 VALUES('two');
INSERT INTO t5 VALUES('five');
INSERT INTO t5 VALUES('three');
INSERT INTO t5 VALUES('four');

SELECT * FROM t5 ORDER BY 1 COLLATE test_collate;

SELECT * FROM t5 ORDER BY 1 COLLATE test_collate;

SELECT * FROM t5 ORDER BY 1 COLLATE test_collate;

pragma encoding = 'UTF-16BE';

CREATE TABLE t5(a);
INSERT INTO t5 VALUES('one');
INSERT INTO t5 VALUES('two');
INSERT INTO t5 VALUES('five');
INSERT INTO t5 VALUES('three');
INSERT INTO t5 VALUES('four');

SELECT * FROM t5 ORDER BY 1 COLLATE test_collate;

SELECT * FROM t5 ORDER BY 1 COLLATE test_collate;

SELECT * FROM t1;

SELECT * FROM t5 ORDER BY 1 COLLATE test_collate;

SELECT * FROM t5 ORDER BY 1 COLLATE test_collate;

CREATE TABLE t1(a varchar collate test_collate);

pragma encoding = 'UTF-8';

CREATE TABLE t5(a);
INSERT INTO t5 VALUES('one');

SELECT test_function('sqlite');

SELECT test_function('sqlite');

SELECT test_function('sqlite');

pragma encoding = 'UTF-16LE';

CREATE TABLE t5(a);
INSERT INTO t5 VALUES('sqlite');

INSERT INTO t1 VALUES('three','III',3);
INSERT INTO t1 VALUES('four','IV',4);
INSERT INTO t1 VALUES('five','V',5);

SELECT test_function('sqlite');

SELECT test_function('sqlite');

SELECT test_function('sqlite');

pragma encoding = 'UTF-16BE';

CREATE TABLE t5(a);
INSERT INTO t5 VALUES('sqlite');

SELECT test_function('sqlite');

SELECT test_function('sqlite');

SELECT test_function('sqlite');

PRAGMA encoding = 'UTF-16';
SELECT * FROM sqlite_master;

PRAGMA encoding;

SELECT * FROM t1;

PRAGMA encoding = 'UTF-8';
CREATE TABLE abc(a, b, c);

SELECT * FROM sqlite_master;

PRAGMA encoding;

PRAGMA encoding = 'UTF-8';
PRAGMA encoding;

PRAGMA encoding = 'UTF-16le';
PRAGMA encoding;

SELECT * FROM sqlite_master;
PRAGMA encoding = 'UTF-8';
PRAGMA encoding;

PRAGMA encoding = 'UTF-16le';
CREATE TABLE abc(a, b, c);
PRAGMA encoding;

PRAGMA encoding = 'UTF-8';
PRAGMA encoding;

PRAGMA encoding=UTF16;
CREATE TABLE t1(a);
PRAGMA encoding=UTF8;
CREATE TABLE t2(b);

SELECT name FROM sqlite_master;

SELECT * FROM t1 WHERE a = 'one';

SELECT * FROM t1 WHERE a = 'four';

SELECT * FROM t1 WHERE a IN ('one', 'two');

PRAGMA encoding;

-- ===enc3.test===
PRAGMA encoding=utf16le;
PRAGMA encoding;

SELECT 1 FROM sqlite_master LIMIT 1;

CREATE TABLE t1(x,y);
INSERT INTO t1 VALUES('abc''123',5);
SELECT * FROM t1;

SELECT quote(x) || ' ' || quote(y) FROM t1;

DELETE FROM t1;
INSERT INTO t1 VALUES(x'616263646566',NULL);
SELECT * FROM t1;

SELECT quote(x) || ' ' || quote(y) FROM t1;

PRAGMA encoding;

CREATE TABLE t2(a);
INSERT INTO t2 VALUES(x'61006200630064006500');
SELECT CAST(a AS text) FROM t2 WHERE a LIKE 'abc%';

SELECT CAST(x'61006200630064006500' AS text);

SELECT rowid FROM t2 WHERE a LIKE x'610062002500';

-- ===enc4.test===
PRAGMA encoding