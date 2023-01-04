-- ===cast.test===
SELECT x'616263';

SELECT typeof(CAST(x'616263' AS integer));

SELECT null;

SELECT typeof(NULL);

SELECT CAST(NULL AS text);

SELECT typeof(CAST(NULL AS text));

SELECT CAST(NULL AS numeric);

SELECT typeof(CAST(NULL AS numeric));

SELECT CAST(NULL AS blob);

SELECT typeof(CAST(NULL AS blob));

SELECT CAST(NULL AS integer);

SELECT typeof(x'616263');

SELECT typeof(CAST(NULL AS integer));

SELECT 123;

SELECT typeof(123);

SELECT CAST(123 AS text);

SELECT typeof(CAST(123 AS text));

SELECT CAST(123 AS numeric);

SELECT typeof(CAST(123 AS numeric));

SELECT CAST(123 AS blob);

SELECT typeof(CAST(123 AS blob));

SELECT CAST(123 AS integer);

SELECT CAST(x'616263' AS text);

SELECT typeof(CAST(123 AS integer));

SELECT 123.456;

SELECT typeof(123.456);

SELECT CAST(123.456 AS text);

SELECT typeof(CAST(123.456 AS text));

SELECT CAST(123.456 AS numeric);

SELECT typeof(CAST(123.456 AS numeric));

SELECT CAST(123.456 AS blob);

SELECT typeof(CAST(123.456 AS blob));

SELECT CAST(123.456 AS integer);

SELECT typeof(CAST(x'616263' AS text));

SELECT typeof(CAST(123.456 AS integer));

SELECT '123abc';

SELECT typeof('123abc');

SELECT CAST('123abc' AS text);

SELECT typeof(CAST('123abc' AS text));

SELECT CAST('123abc' AS numeric);

SELECT typeof(CAST('123abc' AS numeric));

SELECT CAST('123abc' AS blob);

SELECT typeof(CAST('123abc' AS blob));

SELECT CAST('123abc' AS integer);

SELECT CAST(x'616263' AS numeric);

SELECT typeof(CAST('123abc' AS integer));

SELECT CAST('123.5abc' AS numeric);

SELECT CAST('123.5abc' AS integer);

SELECT CAST(null AS REAL);

SELECT typeof(CAST(null AS REAL));

SELECT CAST(1 AS REAL);

SELECT typeof(CAST(1 AS REAL));

SELECT CAST('1' AS REAL);

SELECT typeof(CAST('1' AS REAL));

SELECT CAST('abc' AS REAL);

SELECT typeof(CAST(x'616263' AS numeric));

SELECT typeof(CAST('abc' AS REAL));

SELECT CAST(x'31' AS REAL);

SELECT typeof(CAST(x'31' AS REAL));

SELECT CAST('   123' AS integer);

SELECT CAST('   -123.456' AS real);

SELECT CAST(9223372036854774800 AS integer);

SELECT CAST(9223372036854774800 AS numeric);

SELECT CAST(9223372036854774800 AS real);

SELECT CAST(CAST(9223372036854774800 AS real) AS integer);

SELECT CAST(-9223372036854774800 AS integer);

SELECT CAST(x'616263' AS blob);

SELECT CAST(-9223372036854774800 AS numeric);

SELECT CAST(-9223372036854774800 AS real);

SELECT CAST(CAST(-9223372036854774800 AS real) AS integer);

SELECT CAST('9223372036854774800' AS integer);

SELECT CAST('9223372036854774800' AS numeric);

SELECT CAST('9223372036854774800' AS real);

SELECT CAST(CAST('9223372036854774800' AS real) AS integer);

SELECT CAST('-9223372036854774800' AS integer);

SELECT CAST('-9223372036854774800' AS numeric);

SELECT CAST('-9223372036854774800' AS real);

SELECT typeof(CAST(x'616263' AS blob));

SELECT CAST(CAST('-9223372036854774800' AS real) AS integer);

PRAGMA encoding;

SELECT CAST(x'39323233333732303336383534373734383030' AS integer);

SELECT CAST(x'39323233333732303336383534373734383030' AS numeric);

SELECT CAST(x'39323233333732303336383534373734383030' AS real);

SELECT CAST(CAST(x'39323233333732303336383534373734383030' AS real)
AS integer);

SELECT CAST(NULL AS numeric);

CREATE TABLE t1(a);
INSERT INTO t1 VALUES('abc');
SELECT a, CAST(a AS integer) FROM t1;

SELECT CAST(a AS integer), a FROM t1;

SELECT a, CAST(a AS integer), a FROM t1;

SELECT CAST(x'616263' AS integer);

SELECT CAST(a AS integer), a, CAST(a AS real), a FROM t1;