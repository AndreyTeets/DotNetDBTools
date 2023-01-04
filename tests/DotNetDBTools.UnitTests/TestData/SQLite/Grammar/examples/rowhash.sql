-- ===rowhash.test===
CREATE TABLE t1(id INTEGER PRIMARY KEY, a, b, c);
CREATE INDEX i1 ON t1(a);
CREATE INDEX i2 ON t1(b);
CREATE INDEX i3 ON t1(c);

DELETE FROM t1;

INSERT OR IGNORE INTO t1 VALUES(key, 'a', 'b', 'c');

SELECT id FROM t1 WHERE a = 'a' OR b = 'b' OR c = 'c';