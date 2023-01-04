-- ===substr.test===
CREATE TABLE t1(t text, b blob);

SELECT ifnull(substr('abcdefg',NULL),'nil');

SELECT ifnull(substr('abcdefg',1,NULL),'nil');

DELETE FROM t1;
INSERT INTO t1(t) VALUES(string);

SELECT substr(t, idx) FROM t1;

SELECT substr(qstr, idx);

DELETE FROM t1;
INSERT INTO t1(t) VALUES(string);

SELECT substr(t, i1, i2) FROM t1;

SELECT substr(qstr, i1, i2);

SELECT hex(substr(b, i1, i2)) FROM t1;

SELECT hex(substr(x'hex', i1, i2));

SELECT ifnull(substr(NULL,1,1),'nil');

SELECT ifnull(substr(NULL,1),'nil');

SELECT ifnull(substr('abcdefg',NULL,1),'nil')