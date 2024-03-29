-- ===expr.test===
CREATE TABLE test1(i1 int, i2 int, r1 real, r2 real, t1 text, t2 text);

SELECT CURRENT_TIME;

SELECT CURRENT_DATE;

SELECT CURRENT_TIMESTAMP;

SELECT CURRENT_TIME==time('now');

SELECT CURRENT_DATE==date('now');

SELECT CURRENT_TIMESTAMP==datetime('now');

SELECT round(-('-'||'123'));

SELECT typeof(9223372036854775807);

SELECT typeof(00000009223372036854775807);

SELECT typeof(+9223372036854775807);

INSERT INTO test1 VALUES(1,2,1.1,2.2,'hello','world');

SELECT typeof(+000000009223372036854775807);

SELECT typeof(9223372036854775808);

SELECT typeof(00000009223372036854775808);

SELECT typeof(+9223372036854775808);

SELECT typeof(+0000009223372036854775808);

SELECT typeof(-9223372036854775808);

SELECT typeof(-00000009223372036854775808);

SELECT typeof(-9223372036854775809);

SELECT typeof(-00000009223372036854775809);

SELECT 12345678901234567890;

CREATE TABLE test1(i1 int, i2 int, t1 text, t2 text);

SELECT 0+'9223372036854775807';

SELECT '9223372036854775807'+0;

SELECT 0+'9223372036854775808';

SELECT '9223372036854775808'+0;

SELECT 0+'9223372036854775807.0';

SELECT '9223372036854775807.0'+0;

INSERT INTO test1 VALUES(1,2,'hello','world');

BEGIN; UPDATE test1 SET a=1; SELECT a FROM test1; ROLLBACK;

DROP TABLE test1;

CREATE TABLE test1(a int, b int);

SELECT * FROM test1 ORDER BY a;

SELECT a FROM test1 WHERE b=2 ORDER BY a