-- ===utf16align.test===
PRAGMA encoding=UTF16;
CREATE TABLE t1(
id INTEGER PRIMARY KEY,
spacer TEXT,
a TEXT COLLATE utf16_aligned,
b TEXT COLLATE utf16_unaligned
);
INSERT INTO t1(a) VALUES("abc");
INSERT INTO t1(a) VALUES("defghi");
INSERT INTO t1(a) VALUES("jklmnopqrstuv");
INSERT INTO t1(a) VALUES("wxyz0123456789-");
UPDATE t1 SET b=a||'-'||a;
INSERT INTO t1(a,b) SELECT a||b, b||a FROM t1;
INSERT INTO t1(a,b) SELECT a||b, b||a FROM t1;
INSERT INTO t1(a,b) SELECT a||b, b||a FROM t1;
INSERT INTO t1(a,b) VALUES('one','two');
INSERT INTO t1(a,b) SELECT a, b FROM t1;
UPDATE t1 SET spacer = CASE WHEN rowid&1 THEN 'x' ELSE 'xx' END;
SELECT count(*) FROM t1;

CREATE INDEX t1i1 ON t1(spacer, b);

CREATE INDEX t1i2 ON t1(spacer, a);

PRAGMA encoding=UTF16be;
SELECT hex(ltrim(x'6efcda'));