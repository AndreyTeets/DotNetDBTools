-- ===fuzz.test===
SELECT 'abc' LIKE X'ABCD';

SELECT CAST(zeroblob(1000) AS text);

SELECT 1 FROM (SELECT * FROM sqlite_master WHERE random());

SELECT coalesce(1, substr( 1, 2, length('in' IN (SELECT 1))));

SELECT 'A' FROM (SELECT 'B') ORDER BY EXISTS (
SELECT 'C' FROM (SELECT 'D' LIMIT 0)
);

CREATE TABLE abc(b);
INSERT INTO abc VALUES('ABCDE');

SELECT 1 IN ( SELECT b UNION SELECT 1 ) FROM (SELECT b FROM abc);

DROP TABLE abc;

SELECT 'abcd' UNION SELECT 'efgh' ORDER BY 1 ASC, 1 ASC;

CREATE TABLE abc(a, b, c);
INSERT INTO abc VALUES(123, 456, 789);

SELECT 1 FROM abc
GROUP BY c HAVING EXISTS (SELECT a UNION SELECT 123);

SELECT 'abc' LIKE zeroblob(10);

DROP TABLE abc;

SELECT hex(CAST(zeroblob(1000) AS integer));

CREATE TABLE abc(a, b, c);
CREATE TABLE def(a, b, c);
CREATE TABLE ghi(a, b, c);

DROP TABLE abc; DROP TABLE def; DROP TABLE ghi;

CREATE TABLE t1(a);

DROP TABLE t1;

CREATE TABLE abc(a, b, c);
CREATE TABLE def(a, b, c);
CREATE TABLE ghi(a, b, c);

INSERT INTO abc VALUES(1, 2, 3);
INSERT INTO abc VALUES(4, 5, 6);
INSERT INTO abc VALUES(7, 8, 9);
INSERT INTO def VALUES(1, 2, 3);
INSERT INTO def VALUES(4, 5, 6);
INSERT INTO def VALUES(7, 8, 9);
INSERT INTO ghi VALUES(1, 2, 3);
INSERT INTO ghi VALUES(4, 5, 6);
INSERT INTO ghi VALUES(7, 8, 9);
CREATE INDEX abc_i ON abc(a, b, c);
CREATE INDEX def_i ON def(c, a, b);
CREATE INDEX ghi_i ON ghi(b, c, a);

SELECT zeroblob(10) LIKE 'abc';

SELECT (- -21) % NOT (456 LIKE zeroblob(10));

SELECT (SELECT (
SELECT (SELECT -2147483648) FROM (SELECT 1) ORDER BY 1
));

SELECT 'abc', zeroblob(1) FROM (SELECT 1) ORDER BY 1;

SELECT 'abc', zeroblob(1);

SELECT ( SELECT zeroblob(1000) FROM ( 
SELECT * FROM (SELECT 'first') ORDER BY NOT 'in') 
);

SELECT zeroblob(1000);

-- ===fuzz3.test===
SELECT md5sum(a, b, c) FROM t1;

SELECT md5sum(d, e, f) FROM t2;

BEGIN;
CREATE TABLE t1(a, b, c);
CREATE TABLE t2(d, e, f);
CREATE INDEX i1 ON t1(a, b, c);
CREATE INDEX i2 ON t2(d, e, f);