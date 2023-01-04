-- ===e_expr.test===
ATTACH 'test.db2' AS dbname;
CREATE TABLE dbname.tblname(cname);

PRAGMA encoding = 'utf-16le';

PRAGMA encoding = 'utf-16be';

PRAGMA encoding = 'utf-16le';

PRAGMA encoding = 'utf-16le';

PRAGMA encoding = 'utf-16be';

CREATE TABLE t2(a, b);
INSERT INTO t2 VALUES('one', 'two');
INSERT INTO t2 VALUES('three', NULL);
INSERT INTO t2 VALUES(4, 5.0);