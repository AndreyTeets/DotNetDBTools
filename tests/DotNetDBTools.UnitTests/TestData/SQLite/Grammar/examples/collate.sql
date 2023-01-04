-- ===collate1.test===
CREATE TABLE collate1t1(c1, c2);
INSERT INTO collate1t1 VALUES(45, hex(45));
INSERT INTO collate1t1 VALUES(NULL, NULL);
INSERT INTO collate1t1 VALUES(281, hex(281));

CREATE TABLE collate1t1(c1, c2);
INSERT INTO collate1t1 VALUES('5', '0x11');
INSERT INTO collate1t1 VALUES('5', '0xA');
INSERT INTO collate1t1 VALUES(NULL, NULL);
INSERT INTO collate1t1 VALUES('7', '0xA');
INSERT INTO collate1t1 VALUES('11', '0x11');
INSERT INTO collate1t1 VALUES('11', '0x101');

SELECT c1, c2 FROM collate1t1 ORDER BY 1 COLLATE numeric, 2 COLLATE hex;

SELECT c1, c2 FROM collate1t1 ORDER BY 1 COLLATE binary, 2 COLLATE hex;

SELECT c1, c2 FROM collate1t1 ORDER BY 1 COLLATE binary DESC, 2 COLLATE hex;

SELECT c1, c2 FROM collate1t1 
ORDER BY 1 COLLATE binary DESC, 2 COLLATE hex DESC;

SELECT c1, c2 FROM collate1t1 
ORDER BY 1 COLLATE binary ASC, 2 COLLATE hex ASC;

SELECT c1 COLLATE numeric, c2 FROM collate1t1 
ORDER BY 1, 2 COLLATE hex;

SELECT c1 COLLATE hex, c2 FROM collate1t1 
ORDER BY 1 COLLATE numeric, 2 COLLATE hex;

SELECT c1, c2 COLLATE hex FROM collate1t1 
ORDER BY 1 COLLATE numeric, 2;

SELECT c1 COLLATE numeric, c2 COLLATE hex
FROM collate1t1 
ORDER BY 1, 2;

SELECT c2 FROM collate1t1 ORDER BY 1;

SELECT c1 COLLATE binary, c2 COLLATE hex
FROM collate1t1
ORDER BY 1, 2;

SELECT c1, c2
FROM collate1t1 ORDER BY 1 COLLATE binary DESC, 2 COLLATE hex;

SELECT c1 COLLATE binary, c2 COLLATE hex
FROM collate1t1 
ORDER BY 1 DESC, 2 DESC;

SELECT c1 COLLATE hex, c2 COLLATE binary
FROM collate1t1 
ORDER BY 1 COLLATE binary ASC, 2 COLLATE hex ASC;

DROP TABLE collate1t1;

CREATE TABLE collate1t1(a COLLATE hex, b);
INSERT INTO collate1t1 VALUES( '0x5', 5 );
INSERT INTO collate1t1 VALUES( '1', 1 );
INSERT INTO collate1t1 VALUES( '0x45', 69 );
INSERT INTO collate1t1 VALUES( NULL, NULL );
SELECT * FROM collate1t1 ORDER BY a;

SELECT * FROM collate1t1 ORDER BY 1;

SELECT * FROM collate1t1 ORDER BY collate1t1.a;

SELECT * FROM collate1t1 ORDER BY main.collate1t1.a;

SELECT a as c1, b as c2 FROM collate1t1 ORDER BY c1;

SELECT c2 FROM collate1t1 ORDER BY 1 COLLATE hex;

SELECT a as c1, b as c2 FROM collate1t1 ORDER BY c1 COLLATE binary;

SELECT a COLLATE binary as c1, b as c2
FROM collate1t1 ORDER BY c1;

DROP TABLE collate1t1;

CREATE TABLE collate1t1(c1 numeric, c2 text);
INSERT INTO collate1t1 VALUES(1, 1);
INSERT INTO collate1t1 VALUES(12, 12);
INSERT INTO collate1t1 VALUES(NULL, NULL);
INSERT INTO collate1t1 VALUES(101, 101);

SELECT c1 FROM collate1t1 ORDER BY 1;

SELECT c2 FROM collate1t1 ORDER BY 1;

SELECT c2+0 FROM collate1t1 ORDER BY 1;

SELECT c1||'' FROM collate1t1 ORDER BY 1;

SELECT (c1||'') COLLATE numeric FROM collate1t1 ORDER BY 1;

DROP TABLE collate1t1;

SELECT c2 FROM collate1t1 ORDER BY 1 COLLATE hex DESC;

SELECT c2 FROM collate1t1 ORDER BY 1 COLLATE hex ASC;

SELECT c2 COLLATE hex FROM collate1t1 ORDER BY 1;

SELECT c2 COLLATE hex FROM collate1t1 ORDER BY 1 ASC;

SELECT c2 COLLATE hex FROM collate1t1 ORDER BY 1 DESC;

DROP TABLE collate1t1;

-- ===collate2.test===
CREATE TABLE collate2t1(
a COLLATE BINARY, 
b COLLATE NOCASE, 
c COLLATE BACKWARDS
);
INSERT INTO collate2t1 VALUES( NULL, NULL, NULL );
INSERT INTO collate2t1 VALUES( 'aa', 'aa', 'aa' );
INSERT INTO collate2t1 VALUES( 'ab', 'ab', 'ab' );
INSERT INTO collate2t1 VALUES( 'ba', 'ba', 'ba' );
INSERT INTO collate2t1 VALUES( 'bb', 'bb', 'bb' );
INSERT INTO collate2t1 VALUES( 'aA', 'aA', 'aA' );
INSERT INTO collate2t1 VALUES( 'aB', 'aB', 'aB' );
INSERT INTO collate2t1 VALUES( 'bA', 'bA', 'bA' );
INSERT INTO collate2t1 VALUES( 'bB', 'bB', 'bB' );
INSERT INTO collate2t1 VALUES( 'Aa', 'Aa', 'Aa' );
INSERT INTO collate2t1 VALUES( 'Ab', 'Ab', 'Ab' );
INSERT INTO collate2t1 VALUES( 'Ba', 'Ba', 'Ba' );
INSERT INTO collate2t1 VALUES( 'Bb', 'Bb', 'Bb' );
INSERT INTO collate2t1 VALUES( 'AA', 'AA', 'AA' );
INSERT INTO collate2t1 VALUES( 'AB', 'AB', 'AB' );
INSERT INTO collate2t1 VALUES( 'BA', 'BA', 'BA' );
INSERT INTO collate2t1 VALUES( 'BB', 'BB', 'BB' );

SELECT b FROM collate2t1 WHERE c COLLATE nocase > 'aa'
ORDER BY 1, oid;

SELECT collate2t1.a FROM collate2t1, collate2t2 
WHERE collate2t1.b = collate2t2.b;

SELECT collate2t1.a FROM collate2t1, collate2t2 
WHERE collate2t2.b = collate2t1.b;

SELECT collate2t1.a FROM collate2t1, collate2t3 
WHERE collate2t1.b = collate2t3.b||'';

SELECT collate2t1.a FROM collate2t1, collate2t3 
WHERE collate2t3.b||'' = collate2t1.b;

DROP TABLE collate2t3;

SELECT collate2t1.b FROM collate2t1 JOIN collate2t2 USING (b);

SELECT collate2t1.b FROM collate2t2 JOIN collate2t1 USING (b);

SELECT collate2t1.b FROM collate2t1 NATURAL JOIN collate2t2;

SELECT collate2t1.b FROM collate2t2 NATURAL JOIN collate2t1;

SELECT collate2t2.b FROM collate2t1 LEFT OUTER JOIN collate2t2 USING (b) order by collate2t1.oid;

SELECT b FROM collate2t1 WHERE b > 'aa' ORDER BY +b;

SELECT collate2t1.b, collate2t2.b FROM collate2t2 LEFT OUTER JOIN collate2t1 USING (b);

SELECT b FROM collate2t1 WHERE a COLLATE nocase > 'aa' ORDER BY +b;

SELECT b FROM collate2t1 WHERE b COLLATE nocase > 'aa' ORDER BY +b;

SELECT b FROM collate2t1 WHERE c COLLATE nocase > 'aa' ORDER BY +b;

SELECT c FROM collate2t1 WHERE c > 'aa' ORDER BY 1;

SELECT c FROM collate2t1 WHERE a COLLATE backwards > 'aa'
ORDER BY 1;

SELECT c FROM collate2t1 WHERE b COLLATE backwards > 'aa'
ORDER BY 1;

SELECT c FROM collate2t1 WHERE c COLLATE backwards > 'aa'
ORDER BY 1;

SELECT a FROM collate2t1 WHERE a < 'aa' ORDER BY 1;

CREATE INDEX collate2t1_i1 ON collate2t1(a);
CREATE INDEX collate2t1_i2 ON collate2t1(b);
CREATE INDEX collate2t1_i3 ON collate2t1(c);

SELECT b FROM collate2t1 WHERE b < 'aa' ORDER BY 1, oid;

SELECT b FROM collate2t1 WHERE b < 'aa' ORDER BY +b;

SELECT c FROM collate2t1 WHERE c < 'aa' ORDER BY 1;

SELECT a FROM collate2t1 WHERE a = 'aa';

SELECT b FROM collate2t1 WHERE b = 'aa' ORDER BY oid;

SELECT c FROM collate2t1 WHERE c = 'aa';

SELECT a FROM collate2t1 WHERE a >= 'aa' ORDER BY 1;

SELECT b FROM collate2t1 WHERE b >= 'aa' ORDER BY 1, oid;

SELECT c FROM collate2t1 WHERE c >= 'aa' ORDER BY 1;

SELECT a FROM collate2t1 WHERE a <= 'aa' ORDER BY 1;

SELECT a FROM collate2t1 WHERE a > 'aa' ORDER BY 1;

SELECT b FROM collate2t1 WHERE b <= 'aa' ORDER BY 1, oid;

SELECT c FROM collate2t1 WHERE c <= 'aa' ORDER BY 1;

SELECT a FROM collate2t1 WHERE a BETWEEN 'Aa' AND 'Bb' ORDER BY 1;

SELECT b FROM collate2t1 WHERE b BETWEEN 'Aa' AND 'Bb' ORDER BY 1, oid;

SELECT b FROM collate2t1 WHERE b BETWEEN 'Aa' AND 'Bb' ORDER BY +b;

SELECT c FROM collate2t1 WHERE c BETWEEN 'Aa' AND 'Bb' ORDER BY 1;

SELECT a FROM collate2t1 WHERE 
CASE a WHEN 'aa' THEN 1 ELSE 0 END
ORDER BY 1, oid;

SELECT b FROM collate2t1 WHERE 
CASE b WHEN 'aa' THEN 1 ELSE 0 END
ORDER BY 1, oid;

SELECT c FROM collate2t1 WHERE 
CASE c WHEN 'aa' THEN 1 ELSE 0 END
ORDER BY 1, oid;

SELECT a FROM collate2t1 WHERE a IN ('aa', 'bb') ORDER BY 1, oid;

SELECT a FROM collate2t1 WHERE a COLLATE binary > 'aa' ORDER BY 1;

SELECT b FROM collate2t1 WHERE b IN ('aa', 'bb') ORDER BY 1, oid;

SELECT c FROM collate2t1 WHERE c IN ('aa', 'bb') ORDER BY 1, oid;

SELECT a FROM collate2t1 
WHERE a IN (SELECT a FROM collate2t1 WHERE a IN ('aa', 'bb'));

SELECT b FROM collate2t1 
WHERE b IN (SELECT a FROM collate2t1 WHERE a IN ('aa', 'bb'));

SELECT c FROM collate2t1 
WHERE c IN (SELECT a FROM collate2t1 WHERE a IN ('aa', 'bb'));

SELECT a FROM collate2t1 WHERE NOT a > 'aa' ORDER BY 1;

SELECT b FROM collate2t1 WHERE NOT b > 'aa' ORDER BY 1, oid;

SELECT c FROM collate2t1 WHERE NOT c > 'aa' ORDER BY 1;

SELECT a FROM collate2t1 WHERE NOT a < 'aa' ORDER BY 1;

SELECT b FROM collate2t1 WHERE NOT b < 'aa' ORDER BY 1, oid;

SELECT a FROM collate2t1 WHERE b COLLATE binary > 'aa' ORDER BY 1;

SELECT c FROM collate2t1 WHERE NOT c < 'aa' ORDER BY 1;

SELECT a FROM collate2t1 WHERE NOT a = 'aa';

SELECT b FROM collate2t1 WHERE NOT b = 'aa';

SELECT c FROM collate2t1 WHERE NOT c = 'aa';

SELECT a FROM collate2t1 WHERE NOT a >= 'aa' ORDER BY 1;

SELECT b FROM collate2t1 WHERE NOT b >= 'aa' ORDER BY 1, oid;

SELECT c FROM collate2t1 WHERE NOT c >= 'aa' ORDER BY 1;

SELECT a FROM collate2t1 WHERE NOT a <= 'aa' ORDER BY 1;

SELECT b FROM collate2t1 WHERE NOT b <= 'aa' ORDER BY 1, oid;

SELECT c FROM collate2t1 WHERE NOT c <= 'aa' ORDER BY 1;

SELECT a FROM collate2t1 WHERE c COLLATE binary > 'aa' ORDER BY 1;

SELECT a FROM collate2t1 WHERE a NOT BETWEEN 'Aa' AND 'Bb' ORDER BY 1;

SELECT b FROM collate2t1 WHERE b NOT BETWEEN 'Aa' AND 'Bb' ORDER BY 1, oid;

SELECT c FROM collate2t1 WHERE c NOT BETWEEN 'Aa' AND 'Bb' ORDER BY 1;

SELECT a FROM collate2t1 WHERE NOT CASE a WHEN 'aa' THEN 1 ELSE 0 END;

SELECT b FROM collate2t1 WHERE NOT CASE b WHEN 'aa' THEN 1 ELSE 0 END;

SELECT c FROM collate2t1 WHERE NOT CASE c WHEN 'aa' THEN 1 ELSE 0 END;

SELECT a FROM collate2t1 WHERE NOT a IN ('aa', 'bb');

SELECT b FROM collate2t1 WHERE NOT b IN ('aa', 'bb');

SELECT c FROM collate2t1 WHERE NOT c IN ('aa', 'bb');

SELECT a FROM collate2t1 
WHERE NOT a IN (SELECT a FROM collate2t1 WHERE a IN ('aa', 'bb'));

SELECT b FROM collate2t1 WHERE b > 'aa' ORDER BY 1, oid;

SELECT b FROM collate2t1 
WHERE NOT b IN (SELECT a FROM collate2t1 WHERE a IN ('aa', 'bb'));

SELECT c FROM collate2t1 
WHERE NOT c IN (SELECT a FROM collate2t1 WHERE a IN ('aa', 'bb'));

SELECT a > 'aa' FROM collate2t1;

SELECT b > 'aa' FROM collate2t1;

SELECT c > 'aa' FROM collate2t1;

SELECT a < 'aa' FROM collate2t1;

SELECT b < 'aa' FROM collate2t1;

SELECT c < 'aa' FROM collate2t1;

SELECT a = 'aa' FROM collate2t1;

SELECT b = 'aa' FROM collate2t1;

SELECT b FROM collate2t1 WHERE a COLLATE nocase > 'aa'
ORDER BY 1, oid;

SELECT c = 'aa' FROM collate2t1;

SELECT a <= 'aa' FROM collate2t1;

SELECT b <= 'aa' FROM collate2t1;

SELECT c <= 'aa' FROM collate2t1;

SELECT a >= 'aa' FROM collate2t1;

SELECT b >= 'aa' FROM collate2t1;

SELECT c >= 'aa' FROM collate2t1;

SELECT a BETWEEN 'Aa' AND 'Bb' FROM collate2t1;

SELECT b BETWEEN 'Aa' AND 'Bb' FROM collate2t1;

SELECT c BETWEEN 'Aa' AND 'Bb' FROM collate2t1;

SELECT b FROM collate2t1 WHERE b COLLATE nocase > 'aa'
ORDER BY 1, oid;

SELECT CASE a WHEN 'aa' THEN 1 ELSE 0 END FROM collate2t1;

SELECT CASE b WHEN 'aa' THEN 1 ELSE 0 END FROM collate2t1;

SELECT CASE c WHEN 'aa' THEN 1 ELSE 0 END FROM collate2t1;

SELECT a IN ('aa', 'bb') FROM collate2t1;

SELECT b IN ('aa', 'bb') FROM collate2t1;

SELECT c IN ('aa', 'bb') FROM collate2t1;

SELECT a IN (SELECT a FROM collate2t1 WHERE a IN ('aa', 'bb')) 
FROM collate2t1;

SELECT b IN (SELECT a FROM collate2t1 WHERE a IN ('aa', 'bb')) 
FROM collate2t1;

SELECT c IN (SELECT a FROM collate2t1 WHERE a IN ('aa', 'bb')) 
FROM collate2t1;

CREATE TABLE collate2t2(b COLLATE binary);
CREATE TABLE collate2t3(b text);
INSERT INTO collate2t2 VALUES('aa');
INSERT INTO collate2t3 VALUES('aa');

-- ===collate3.test===
CREATE TABLE collate3t1(c1);

DROP TABLE collate3t1;

DROP TABLE collate3t1;
CREATE TABLE collate3t1(a COLLATE unk);

DROP TABLE collate3t1;

DROP TABLE collate3t1;

CREATE TABLE collate3t1(c1 COLLATE string_compare, c2);

CREATE INDEX collate3t1_i1 ON collate3t1(c1);
INSERT INTO collate3t1 VALUES('xxx', 'yyy');

select * from collate3t1;

DROP TABLE collate3t1;

CREATE TABLE collate3t1(a, b);
INSERT INTO collate3t1 VALUES('hello', NULL);
CREATE INDEX collate3i1 ON collate3t1(a COLLATE user_defined);

DROP TABLE collate3t1;

CREATE TABLE collate3t1(a, b);
INSERT INTO collate3t1 VALUES('2', NULL);
INSERT INTO collate3t1 VALUES('101', NULL);
INSERT INTO collate3t1 VALUES('12', NULL);
CREATE VIEW collate3v1 AS SELECT * FROM collate3t1 
ORDER BY 1 COLLATE user_defined;
SELECT * FROM collate3v1;

-- ===collate4.test===
CREATE TABLE collate4t1(a COLLATE NOCASE, b COLLATE TEXT);
INSERT INTO collate4t1 VALUES( 'a', 'a' );
INSERT INTO collate4t1 VALUES( 'b', 'b' );
INSERT INTO collate4t1 VALUES( NULL, NULL );
INSERT INTO collate4t1 VALUES( 'B', 'B' );
INSERT INTO collate4t1 VALUES( 'A', 'A' );
CREATE INDEX collate4i1 ON collate4t1(a);
CREATE INDEX collate4i2 ON collate4t1(b);

PRAGMA automatic_index=OFF;
CREATE TABLE collate4t1(a COLLATE NOCASE);
CREATE TABLE collate4t2(b COLLATE TEXT);
INSERT INTO collate4t1 VALUES('a');
INSERT INTO collate4t1 VALUES('A');
INSERT INTO collate4t1 VALUES('b');
INSERT INTO collate4t1 VALUES('B');
INSERT INTO collate4t1 VALUES('c');
INSERT INTO collate4t1 VALUES('C');
INSERT INTO collate4t1 VALUES('d');
INSERT INTO collate4t1 VALUES('D');
INSERT INTO collate4t1 VALUES('e');
INSERT INTO collate4t1 VALUES('D');
INSERT INTO collate4t2 VALUES('A');
INSERT INTO collate4t2 VALUES('Z');

CREATE INDEX collate4i1 ON collate4t1(a);

DROP INDEX collate4i1;
CREATE INDEX collate4i1 ON collate4t1(a COLLATE TEXT);

DROP INDEX collate4i1;
CREATE INDEX collate4i1 ON collate4t1(a);

DROP INDEX collate4i1;
CREATE INDEX collate4i1 ON collate4t1(a COLLATE TEXT);

DROP TABLE collate4t1;
DROP TABLE collate4t2;

CREATE TABLE collate4t1(a COLLATE nocase, b COLLATE text, c);
CREATE TABLE collate4t2(a COLLATE nocase, b COLLATE text, c COLLATE TEXT);
INSERT INTO collate4t1 VALUES('0', '0', '0');
INSERT INTO collate4t1 VALUES('0', '0', '1');
INSERT INTO collate4t1 VALUES('0', '1', '0');
INSERT INTO collate4t1 VALUES('0', '1', '1');
INSERT INTO collate4t1 VALUES('1', '0', '0');
INSERT INTO collate4t1 VALUES('1', '0', '1');
INSERT INTO collate4t1 VALUES('1', '1', '0');
INSERT INTO collate4t1 VALUES('1', '1', '1');
insert into collate4t2 SELECT * FROM collate4t1;

CREATE INDEX collate4i1 ON collate4t1(a, b, c);

DROP INDEX collate4i1;
CREATE INDEX collate4i1 ON collate4t1(a, b, c COLLATE text);

DROP TABLE collate4t1;
DROP TABLE collate4t2;

CREATE TABLE collate4t2(
a PRIMARY KEY COLLATE NOCASE, 
b UNIQUE COLLATE TEXT
);
INSERT INTO collate4t2 VALUES( 'a', 'a' );
INSERT INTO collate4t2 VALUES( NULL, NULL );
INSERT INTO collate4t2 VALUES( 'B', 'B' );

CREATE TABLE collate4t1(a PRIMARY KEY COLLATE NOCASE);

SELECT * FROM collate4t1;

DROP TABLE collate4t1;
CREATE TABLE collate4t1(a COLLATE NOCASE UNIQUE);

SELECT * FROM collate4t1;

DROP TABLE collate4t1;
CREATE TABLE collate4t1(a);
CREATE UNIQUE INDEX collate4i1 ON collate4t1(a COLLATE NOCASE);

SELECT * FROM collate4t1;

DROP TABLE collate4t1;

CREATE TABLE collate4t1(a COLLATE TEXT);
INSERT INTO collate4t1 VALUES('2');
INSERT INTO collate4t1 VALUES('10');
INSERT INTO collate4t1 VALUES('20');
INSERT INTO collate4t1 VALUES('104');

CREATE INDEX collate4i1 ON collate4t1(a);

DROP INDEX collate4i1;
CREATE INDEX collate4i1 ON collate4t1(a COLLATE NUMERIC);

CREATE TABLE collate4t3(
b COLLATE TEXT,  
a COLLATE NOCASE, 
UNIQUE(a), PRIMARY KEY(b)
);
INSERT INTO collate4t3 VALUES( 'a', 'a' );
INSERT INTO collate4t3 VALUES( NULL, NULL );
INSERT INTO collate4t3 VALUES( 'B', 'B' );

DROP TABLE collate4t1;

CREATE TABLE collate4t1(a COLLATE TEXT, b COLLATE NUMERIC);
INSERT INTO collate4t1 VALUES('11', '101');
INSERT INTO collate4t1 VALUES('101', '11');

SELECT max(a, b) FROM collate4t1;

SELECT max(b, a) FROM collate4t1;

SELECT max(a, '101') FROM collate4t1;

SELECT max('101', a) FROM collate4t1;

SELECT max(b, '101') FROM collate4t1;

SELECT max('101', b) FROM collate4t1;

DROP TABLE collate4t1;

CREATE TABLE collate4t1(a INTEGER PRIMARY KEY);
INSERT INTO collate4t1 VALUES(101);
INSERT INTO collate4t1 VALUES(10);
INSERT INTO collate4t1 VALUES(15);

CREATE TABLE collate4t4(a COLLATE NOCASE, b COLLATE TEXT);
INSERT INTO collate4t4 VALUES( 'a', 'a' );
INSERT INTO collate4t4 VALUES( 'b', 'b' );
INSERT INTO collate4t4 VALUES( NULL, NULL );
INSERT INTO collate4t4 VALUES( 'B', 'B' );
INSERT INTO collate4t4 VALUES( 'A', 'A' );
CREATE INDEX collate4i3 ON collate4t4(a COLLATE TEXT);
CREATE INDEX collate4i4 ON collate4t4(b COLLATE NOCASE);

DROP TABLE collate4t1;
DROP TABLE collate4t2;
DROP TABLE collate4t3;
DROP TABLE collate4t4;

CREATE TABLE collate4t1(a COLLATE NOCASE, b COLLATE TEXT);
INSERT INTO collate4t1 VALUES( 'a', 'a' );
INSERT INTO collate4t1 VALUES( 'b', 'b' );
INSERT INTO collate4t1 VALUES( NULL, NULL );
INSERT INTO collate4t1 VALUES( 'B', 'B' );
INSERT INTO collate4t1 VALUES( 'A', 'A' );
CREATE INDEX collate4i1 ON collate4t1(a, b);

CREATE TABLE collate4t2(
a COLLATE NOCASE, 
b COLLATE TEXT, 
PRIMARY KEY(a, b)
);
INSERT INTO collate4t2 VALUES( 'a', 'a' );
INSERT INTO collate4t2 VALUES( NULL, NULL );
INSERT INTO collate4t2 VALUES( 'B', 'B' );

CREATE TABLE collate4t3(a COLLATE NOCASE, b COLLATE TEXT);
INSERT INTO collate4t3 VALUES( 'a', 'a' );
INSERT INTO collate4t3 VALUES( 'b', 'b' );
INSERT INTO collate4t3 VALUES( NULL, NULL );
INSERT INTO collate4t3 VALUES( 'B', 'B' );
INSERT INTO collate4t3 VALUES( 'A', 'A' );
CREATE INDEX collate4i2 ON collate4t3(a COLLATE TEXT, b COLLATE NOCASE);

DROP TABLE collate4t1;
DROP TABLE collate4t2;
DROP TABLE collate4t3;

-- ===collate5.test===
CREATE TABLE collate5t1(a COLLATE nocase, b COLLATE text);
INSERT INTO collate5t1 VALUES('a', 'apple');
INSERT INTO collate5t1 VALUES('A', 'Apple');
INSERT INTO collate5t1 VALUES('b', 'banana');
INSERT INTO collate5t1 VALUES('B', 'banana');
INSERT INTO collate5t1 VALUES('n', NULL);
INSERT INTO collate5t1 VALUES('N', NULL);

SELECT a, b FROM collate5t2 UNION select a, b FROM collate5t1;

SELECT a FROM collate5t1 EXCEPT select a FROM collate5t2;

SELECT a FROM collate5t2 EXCEPT select a FROM collate5t1 WHERE a != 'a';

SELECT a, b FROM collate5t1 EXCEPT select a, b FROM collate5t2;

SELECT a, b FROM collate5t2 EXCEPT select a, b FROM collate5t1 
where a != 'a';

SELECT a FROM collate5t1 INTERSECT select a FROM collate5t2;

SELECT a FROM collate5t2 INTERSECT select a FROM collate5t1 WHERE a != 'a';

SELECT a, b FROM collate5t1 INTERSECT select a, b FROM collate5t2;

SELECT a, b FROM collate5t2 INTERSECT select a, b FROM collate5t1;

BEGIN;
CREATE TABLE collate5t3(a, b);

SELECT DISTINCT a FROM collate5t1;

COMMIT;
SELECT * FROM collate5t3 UNION SELECT * FROM collate5t3;

DROP TABLE collate5t3;

SELECT a FROM collate5t1 UNION ALL SELECT a FROM collate5t2 ORDER BY 1;

SELECT a FROM collate5t2 UNION ALL SELECT a FROM collate5t1 ORDER BY 1;

SELECT a FROM collate5t1 UNION ALL SELECT a FROM collate5t2 
ORDER BY 1 COLLATE TEXT;

CREATE TABLE collate5t_cn(a COLLATE NUMERIC);
CREATE TABLE collate5t_ct(a COLLATE TEXT);
INSERT INTO collate5t_cn VALUES('1');
INSERT INTO collate5t_cn VALUES('11');
INSERT INTO collate5t_cn VALUES('101');
INSERT INTO collate5t_ct SELECT * FROM collate5t_cn;

SELECT a FROM collate5t_cn INTERSECT SELECT a FROM collate5t_ct ORDER BY 1;

SELECT a FROM collate5t_ct INTERSECT SELECT a FROM collate5t_cn ORDER BY 1;

DROP TABLE collate5t_cn;
DROP TABLE collate5t_ct;
DROP TABLE collate5t1;
DROP TABLE collate5t2;

CREATE TABLE collate5t1(a COLLATE NOCASE, b COLLATE NUMERIC); 
INSERT INTO collate5t1 VALUES('a', '1');
INSERT INTO collate5t1 VALUES('A', '1.0');
INSERT INTO collate5t1 VALUES('b', '2');
INSERT INTO collate5t1 VALUES('B', '3');

SELECT DISTINCT b FROM collate5t1;

SELECT a, count(*) FROM collate5t1 GROUP BY a;

SELECT a, b, count(*) FROM collate5t1 GROUP BY a, b ORDER BY a, b;

DROP TABLE collate5t1;

SELECT DISTINCT a, b FROM collate5t1;

CREATE TABLE tkt3376(a COLLATE nocase PRIMARY KEY);
INSERT INTO tkt3376 VALUES('abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz');
INSERT INTO tkt3376 VALUES('ABXYZ012234567890123456789ABXYZ012234567890123456789ABXYZ012234567890123456789ABXYZ012234567890123456789ABXYZ012234567890123456789ABXYZ012234567890123456789ABXYZ012234567890123456789');
SELECT DISTINCT a FROM tkt3376;

CREATE TABLE collate5t2(a COLLATE text, b COLLATE nocase);
INSERT INTO collate5t2 VALUES('a', 'apple');
INSERT INTO collate5t2 VALUES('A', 'apple');
INSERT INTO collate5t2 VALUES('b', 'banana');
INSERT INTO collate5t2 VALUES('B', 'Banana');

SELECT a FROM collate5t1 UNION select a FROM collate5t2;

SELECT a FROM collate5t2 UNION select a FROM collate5t1;

SELECT a, b FROM collate5t1 UNION select a, b FROM collate5t2;

-- ===collate6.test===
CREATE TABLE collate6log(a, b);
CREATE TABLE collate6tab(a COLLATE NOCASE, b COLLATE BINARY);

DROP TABLE collate6tab;

CREATE TABLE abc(a COLLATE binary, b, c);
CREATE TABLE def(a, b, c);
CREATE TRIGGER abc_t1 AFTER INSERT ON abc BEGIN
INSERT INTO def SELECT * FROM abc WHERE a < new.a COLLATE nocase;
END;

INSERT INTO abc VALUES('One', 'Two', 'Three');
INSERT INTO abc VALUES('one', 'two', 'three');
SELECT * FROM def;

UPDATE abc SET a = 'four' WHERE a = 'one';
CREATE TRIGGER abc_t2 AFTER UPDATE ON abc BEGIN
INSERT INTO def SELECT * FROM abc WHERE a < new.a COLLATE nocase;
END;
SELECT * FROM def;

SELECT 1 FROM sqlite_master WHERE name COLLATE nocase = 'hello';

SELECT 1 FROM sqlite_master WHERE 'hello' = name COLLATE nocase;

CREATE TRIGGER collate6trig BEFORE INSERT ON collate6tab 
WHEN new.a = 'a' BEGIN
INSERT INTO collate6log VALUES(new.a, new.b);
END;

INSERT INTO collate6tab VALUES('a', 'b');
SELECT * FROM collate6log;

INSERT INTO collate6tab VALUES('A', 'B');
SELECT * FROM collate6log;

DROP TRIGGER collate6trig;
DELETE FROM collate6log;

CREATE TRIGGER collate6trig BEFORE INSERT ON collate6tab BEGIN
INSERT INTO collate6log VALUES(new.a='a', new.b='b');
END;

INSERT INTO collate6tab VALUES('a', 'b');
SELECT * FROM collate6log;

INSERT INTO collate6tab VALUES('A', 'B');
SELECT * FROM collate6log;

DROP TRIGGER collate6trig;
DELETE FROM collate6log;

-- ===collate7.test===
PRAGMA encoding='utf-16';
CREATE TABLE abc16(a COLLATE CASELESS, b, c);

SELECT * FROM abc16 WHERE a < 'abc';

-- ===collate8.test===
CREATE TABLE t1(a TEXT COLLATE nocase);
INSERT INTO t1 VALUES('aaa');
INSERT INTO t1 VALUES('BBB');
INSERT INTO t1 VALUES('ccc');
INSERT INTO t1 VALUES('DDD');
SELECT a FROM t1 ORDER BY a;

SELECT a AS x FROM t1 ORDER BY +x;

CREATE TABLE t2(a);
INSERT INTO t2 VALUES('abc');
INSERT INTO t2 VALUES('ABC');
SELECT a AS x FROM t2 WHERE x='abc';

SELECT a AS x FROM t2 WHERE x='abc' COLLATE nocase;

SELECT a AS x FROM t2 WHERE (x COLLATE nocase)='abc';

SELECT a COLLATE nocase AS x FROM t2 WHERE x='abc';

SELECT a COLLATE nocase AS x FROM t2 WHERE (x COLLATE binary)='abc';

SELECT a COLLATE nocase AS x FROM t2 WHERE x='abc' COLLATE binary;

SELECT * FROM t2 WHERE (a COLLATE nocase)='abc' COLLATE binary;

SELECT a COLLATE nocase AS x FROM t2 WHERE 'abc'=x COLLATE binary;

SELECT rowid FROM t1 WHERE a<'ccc' ORDER BY 1;

SELECT rowid FROM t1 WHERE a<'ccc' COLLATE binary ORDER BY 1;

SELECT rowid FROM t1 WHERE +a<'ccc' ORDER BY 1;

SELECT a FROM t1 ORDER BY +a;

SELECT a AS x FROM t1 ORDER BY "x";

SELECT a AS x FROM t1 WHERE x<'ccc' ORDER BY 1;

SELECT a AS x FROM t1 WHERE x<'ccc' COLLATE binary ORDER BY [x];

SELECT a AS x FROM t1 WHERE +x<'ccc' ORDER BY 1;

-- ===collate9.test===
CREATE TABLE xy(x COLLATE "reverse sort", y COLLATE binary);
INSERT INTO xy VALUES('one', 'one');
INSERT INTO xy VALUES('two', 'two');
INSERT INTO xy VALUES('three', 'three');

SELECT y COLLATE "reverse sort" AS aaa FROM xy ORDER BY aaa;

CREATE INDEX xy_i2 ON xy(y COLLATE "reverse sort");

REINDEX "reverse sort";

PRAGMA integrity_check;

REINDEX "reverse sort";

PRAGMA integrity_check;

SELECT x FROM xy ORDER BY x;

SELECT y FROM xy ORDER BY y;

CREATE INDEX xy_i ON xy(x);

SELECT x, x < 'seven' FROM xy ORDER BY x;

SELECT y, y < 'seven' FROM xy ORDER BY x;

SELECT y, y COLLATE "reverse sort" < 'seven' FROM xy ORDER BY x;

SELECT y FROM xy ORDER BY y;

SELECT y FROM xy ORDER BY y COLLATE "reverse sort";

-- ===collateA.test===
CREATE TABLE t1(
a INTEGER PRIMARY KEY,
b TEXT COLLATE BINARY,
c TEXT COLLATE RTRIM
);
INSERT INTO t1 VALUES(1, 'abcde','abcde');
INSERT INTO t1 VALUES(2, 'xyzzy ','xyzzy ');
INSERT INTO t1 VALUES(3, 'xyzzy  ','xyzzy  ');
INSERT INTO t1 VALUES(4, 'xyzzy   ','xyzzy   ');
INSERT INTO t1 VALUES(5, '   ', '   ');
INSERT INTO t1 VALUES(6, '', '');
SELECT count(*) FROM t1;

SELECT a FROM t1 WHERE c='xyzzy                                  ';

SELECT 'abc123'='abc123                         ' COLLATE RTRIM;

SELECT 'abc123                         '='abc123' COLLATE RTRIM;

SELECT '  '='' COLLATE RTRIM, '  '='' COLLATE BINARY, '  '='';

SELECT ''='  ' COLLATE RTRIM, ''='  ' COLLATE BINARY, ''='  ';

SELECT '  '='      ' COLLATE RTRIM, '  '='        ';

SELECT ''<>'  ' COLLATE RTRIM, ''<>'  ' COLLATE BINARY, ''<>'  ';

SELECT a FROM t1 WHERE c='xyzz';

SELECT a FROM t1 WHERE c='xyzzyy   ';

SELECT a FROM t1 WHERE c='xyzz   ';

SELECT a FROM t1 WHERE b='abcde     ';

SELECT a FROM t1 WHERE c='abcd   ';

SELECT a FROM t1 WHERE c='abcd';

SELECT a FROM t1 WHERE c='abc';

SELECT a FROM t1 WHERE c='abcdef    ';

SELECT a FROM t1 WHERE c='';

SELECT a FROM t1 WHERE c=' ';

SELECT a FROM t1 WHERE c='                    ';

CREATE INDEX i1b ON t1(b);
CREATE INDEX i1c ON t1(c);
PRAGMA integrity_check;

SELECT a FROM t1 WHERE b='abcde     ';

SELECT a FROM t1 WHERE c='abcde     ';

SELECT a FROM t1 WHERE c='abcde     ';

SELECT a FROM t1 WHERE b='xyzzy';

SELECT a FROM t1 WHERE c='xyzzy';

SELECT a FROM t1 WHERE c='xyzzy ';

SELECT a FROM t1 WHERE c='xyzzy  ';

SELECT a FROM t1 WHERE c='xyzzy   ';

SELECT a FROM t1 WHERE c='xyzzy    ';

SELECT a FROM t1 WHERE c='xyzzy                                  ';

SELECT a FROM t1 WHERE c='xyzz';

SELECT a FROM t1 WHERE c='xyzzyy   ';

SELECT a FROM t1 WHERE c='xyzz   ';

SELECT a FROM t1 WHERE b='xyzzy';

SELECT a FROM t1 WHERE c='abcd   ';

SELECT a FROM t1 WHERE c='abcd';

SELECT a FROM t1 WHERE c='abc';

SELECT a FROM t1 WHERE c='abcdef    ';

SELECT a FROM t1 WHERE c='';

SELECT a FROM t1 WHERE c=' ';

SELECT a FROM t1 WHERE c='                    ';

REINDEX;
PRAGMA integrity_check;

SELECT a FROM t1 WHERE b='abcde     ';

SELECT a FROM t1 WHERE c='abcde     ';

SELECT a FROM t1 WHERE c='xyzzy';

SELECT a FROM t1 WHERE b='xyzzy';

SELECT a FROM t1 WHERE c='xyzzy';

SELECT a FROM t1 WHERE c='xyzzy ';

SELECT a FROM t1 WHERE c='xyzzy  ';

SELECT a FROM t1 WHERE c='xyzzy   ';

SELECT a FROM t1 WHERE c='xyzzy    ';

SELECT a FROM t1 WHERE c='xyzzy                                  ';

SELECT a FROM t1 WHERE c='xyzzy ';

SELECT a FROM t1 WHERE c='xyzzy  ';

SELECT a FROM t1 WHERE c='xyzzy   ';

SELECT a FROM t1 WHERE c='xyzzy    '