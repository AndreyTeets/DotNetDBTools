-- ===icu.test===
CREATE TABLE test1(i1 int, i2 int, r1 real, r2 real, t1 text, t2 text);

INSERT INTO test1 VALUES(1,2,1.1,2.2,'hello','world');

BEGIN; 
UPDATE test1 SET a=1; 
SELECT a FROM test1; 
ROLLBACK;

CREATE TABLE fruit(name);
INSERT INTO fruit VALUES('plum');
INSERT INTO fruit VALUES('cherry');
INSERT INTO fruit VALUES('apricot');
INSERT INTO fruit VALUES('peach');
INSERT INTO fruit VALUES('chokecherry');
INSERT INTO fruit VALUES('yamot');

SELECT icu_load_collation('en_US', 'AmericanEnglish');
SELECT icu_load_collation('lt_LT', 'Lithuanian');

SELECT name FROM fruit ORDER BY name COLLATE AmericanEnglish ASC;

SELECT name FROM fruit ORDER BY name COLLATE Lithuanian ASC;