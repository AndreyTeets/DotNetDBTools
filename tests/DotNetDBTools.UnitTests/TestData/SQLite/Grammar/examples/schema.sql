-- ===schema.test===
CREATE TABLE abc(a, b, c);

ATTACH 'test2.db' AS aux;

DETACH aux;

DROP TABLE abc;

CREATE TABLE abc(a, b, c);

CREATE VIEW abcview AS SELECT * FROM abc;

DROP VIEW abcview;

INSERT INTO abc VALUES(1, 2, 3);

SELECT * FROM abc;

BEGIN;
CREATE TABLE t3(a, b, c);

ROLLBACK;
CREATE TABLE t4(a, b, c);

DROP TABLE abc;

CREATE VIEW v1 AS SELECT * FROM sqlite_master;

DROP VIEW v1;

CREATE TABLE abc(a, b, c);

CREATE TRIGGER abc_trig AFTER INSERT ON abc BEGIN
SELECT 1, 2, 3;
END;

DROP TRIGGER abc_trig;

CREATE INDEX abc_index ON abc(a);

DROP INDEX abc_index;

-- ===schema2.test===
CREATE TABLE abc(a, b, c);

ATTACH 'test2.db' AS aux;

DETACH aux;

DROP TABLE abc;

CREATE TABLE abc(a, b, c);

CREATE VIEW abcview AS SELECT * FROM abc;

DROP VIEW abcview;

INSERT INTO abc VALUES(1, 2, 3);

SELECT * FROM abc;

DROP TABLE abc;

CREATE VIEW v1 AS SELECT * FROM sqlite_master;

DROP VIEW v1;

CREATE TABLE abc(a, b, c);

CREATE TRIGGER abc_trig AFTER INSERT ON abc BEGIN
SELECT 1, 2, 3;
END;

DROP TRIGGER abc_trig;

CREATE INDEX abc_index ON abc(a);

DROP INDEX abc_index;