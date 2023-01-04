-- ===stat.test===
CREATE TABLE t1(a, b);
CREATE INDEX i1 ON t1(b);
INSERT INTO t1(rowid, a, b) VALUES(2, 2, 3);
INSERT INTO t1(rowid, a, b) VALUES(3, 4, 5);

SELECT * FROM stat WHERE name = 't1';

SELECT * FROM stat WHERE name = 'i1';

SELECT * FROM stat WHERE name = 'sqlite_master';

DROP TABLE t1;