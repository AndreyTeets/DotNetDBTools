-- ===indexedby.test===
CREATE TABLE t1(a, b);
CREATE INDEX i1 ON t1(a);
CREATE INDEX i2 ON t1(b);
CREATE TABLE t2(c, d);
CREATE INDEX i3 ON t2(c);
CREATE INDEX i4 ON t2(d);
CREATE TABLE t3(e PRIMARY KEY, f);
CREATE VIEW v1 AS SELECT * FROM t1;

CREATE TABLE indexed(x,y);
INSERT INTO indexed VALUES(1,2);
SELECT * FROM indexed;

CREATE INDEX i10 ON indexed(x);
SELECT * FROM indexed indexed by i10 where x>0;

DROP TABLE indexed;
CREATE TABLE t10(indexed INTEGER);
INSERT INTO t10 VALUES(1);
CREATE INDEX indexed ON t10(indexed);
SELECT * FROM t10 indexed by indexed WHERE indexed>0;

EXPLAIN QUERY PLAN DROP TABLE X;

SELECT * FROM t1 NOT INDEXED WHERE a = 'one' AND b = 'two';

SELECT * FROM t1 INDEXED BY i1 WHERE a = 'one' AND b = 'two';

SELECT * FROM t1 INDEXED BY i2 WHERE a = 'one' AND b = 'two';

DROP INDEX i1;

CREATE INDEX i1 ON t1(b);

DROP INDEX i1 ; CREATE INDEX i1 ON t1(a);

CREATE TABLE maintable( id integer);
CREATE TABLE joinme(id_int integer, id_text text);
CREATE INDEX joinme_id_text_idx on joinme(id_text);
CREATE INDEX joinme_id_int_idx on joinme(id_int);