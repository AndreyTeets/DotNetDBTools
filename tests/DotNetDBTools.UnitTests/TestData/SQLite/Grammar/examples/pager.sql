-- ===pager1.test===
CREATE TABLE t1(a PRIMARY KEY, b);
CREATE TABLE counter(
i CHECK (i<5), 
u CHECK (u<10)
);
INSERT INTO counter VALUES(0, 0);
CREATE TRIGGER tr1 AFTER INSERT ON t1 BEGIN
UPDATE counter SET i = i+1;
END;
CREATE TRIGGER tr2 AFTER UPDATE ON t1 BEGIN
UPDATE counter SET u = u+1;
END;

SELECT count(*) FROM t1;
PRAGMA integrity_check;

DELETE FROM x1;
INSERT INTO x1 SELECT * FROM x2;
BEGIN;
DELETE FROM x1 WHERE rowid<32;
UPDATE x1 SET z = a_string(299) WHERE rowid>40;

PRAGMA integrity_check;
SELECT count(*) FROM x1;

DELETE FROM x1;
INSERT INTO x1 SELECT * FROM x2;

CREATE TABLE x3(x, y, z);

SELECT * FROM x3;

BEGIN;
SAVEPOINT abc;
CREATE TABLE t1(a, b);
ROLLBACK TO abc;
COMMIT;

SAVEPOINT abc;
CREATE TABLE t1(a, b);
ROLLBACK TO abc;
COMMIT;

PRAGMA page_size = 512;
CREATE TABLE tbl(a PRIMARY KEY, b UNIQUE);
BEGIN;
INSERT INTO tbl VALUES(a_string(25), a_string(600));
INSERT INTO tbl SELECT a_string(25), a_string(600) FROM tbl;
INSERT INTO tbl SELECT a_string(25), a_string(600) FROM tbl;
INSERT INTO tbl SELECT a_string(25), a_string(600) FROM tbl;
INSERT INTO tbl SELECT a_string(25), a_string(600) FROM tbl;
INSERT INTO tbl SELECT a_string(25), a_string(600) FROM tbl;
INSERT INTO tbl SELECT a_string(25), a_string(600) FROM tbl;
INSERT INTO tbl SELECT a_string(25), a_string(600) FROM tbl;
COMMIT;

BEGIN;
CREATE TABLE t1(a, b);

PRAGMA page_size = 1024;
PRAGMA auto_vacuum = full;
PRAGMA locking_mode=exclusive;
CREATE TABLE t1(a, b);
INSERT INTO t1 VALUES(1, 2);

SELECT count(*) FROM t1;
PRAGMA integrity_check;

PRAGMA page_size = 4096;
VACUUM;

PRAGMA locking_mode=EXCLUSIVE;
SELECT count(*) FROM sqlite_master;
PRAGMA lock_status;

PRAGMA cache_size = 10;
PRAGMA page_size = 1024;
CREATE TABLE t1(x, y, UNIQUE(x, y));
INSERT INTO t1 VALUES(randomblob(1500), randomblob(1500));
INSERT INTO t1 SELECT randomblob(1500), randomblob(1500) FROM t1;
INSERT INTO t1 SELECT randomblob(1500), randomblob(1500) FROM t1;
INSERT INTO t1 SELECT randomblob(1500), randomblob(1500) FROM t1;
INSERT INTO t1 SELECT randomblob(1500), randomblob(1500) FROM t1;
INSERT INTO t1 SELECT randomblob(1500), randomblob(1500) FROM t1;
INSERT INTO t1 SELECT randomblob(1500), randomblob(1500) FROM t1;
INSERT INTO t1 SELECT randomblob(1500), randomblob(1500) FROM t1;
INSERT INTO t1 SELECT randomblob(1500), randomblob(1500) FROM t1;
INSERT INTO t1 SELECT randomblob(1500), randomblob(1500) FROM t1;
INSERT INTO t1 SELECT randomblob(1500), randomblob(1500) FROM t1;
BEGIN;
UPDATE t1 SET y = randomblob(1499);

PRAGMA integrity_check;

PRAGMA journal_mode = DELETE;
CREATE TABLE t1(a, b);
INSERT INTO t1 VALUES(1, 2);
INSERT INTO t1 VALUES(3, 4);

SELECT * FROM t1;

BEGIN;
INSERT INTO a SELECT * FROM b WHERE rowid<=3;
INSERT INTO b SELECT * FROM a WHERE rowid<=3;
COMMIT;

PRAGMA journal_mode = DELETE;
ATTACH 'test.db2' AS two;
CREATE TABLE t1(a, b);
CREATE TABLE two.t2(a, b);
INSERT INTO t1 VALUES(1, 't1.1');
INSERT INTO t2 VALUES(1, 't2.1');
BEGIN;
UPDATE t1 SET b = 't1.2';
UPDATE t2 SET b = 't2.2';
COMMIT;

PRAGMA journal_mode = DELETE;
ATTACH 'test.db3' AS three;
CREATE TABLE three.t3(a, b);
INSERT INTO t3 VALUES(1, 't3.1');
BEGIN;
UPDATE t2 SET b = 't2.3';
UPDATE t3 SET b = 't3.3';
COMMIT;

ATTACH 'test.db2' AS aux;
CREATE TABLE t1(a, b);
CREATE TABLE aux.t2(a, b);
INSERT INTO t1 VALUES(17, 'Lenin');
INSERT INTO t1 VALUES(22, 'Stalin');
INSERT INTO t1 VALUES(53, 'Khrushchev');

BEGIN;
INSERT INTO t1 VALUES(64, 'Brezhnev');
INSERT INTO t2 SELECT * FROM t1;

BEGIN;
SELECT * FROM t2;

SELECT * FROM counter;

SELECT * FROM t2;

PRAGMA journal_mode = memory;
BEGIN;
INSERT INTO t1 VALUES(84, 'Andropov');
INSERT INTO t2 VALUES(84, 'Andropov');
COMMIT;

PRAGMA journal_mode = off;
BEGIN;
INSERT INTO t1 VALUES(85, 'Gorbachev');
INSERT INTO t2 VALUES(85, 'Gorbachev');
COMMIT;

ATTACH 'test.db2' AS aux;

PRAGMA journal_mode = DELETE;
PRAGMA synchronous = NORMAL;
BEGIN;
INSERT INTO t1 VALUES(85, 'Gorbachev');
INSERT INTO t2 VALUES(85, 'Gorbachev');
COMMIT;

PRAGMA synchronous = full;
BEGIN;
DELETE FROM t1 WHERE b = 'Lenin';
DELETE FROM t2 WHERE b = 'Lenin';
COMMIT;

ATTACH 'test.db2' AS aux;
PRAGMA journal_mode = PERSIST;
CREATE TABLE t3(a, b);
INSERT INTO t3 SELECT randomblob(1500), randomblob(1500) FROM t1;
UPDATE t3 SET b = randomblob(1500);

PRAGMA synchronous = full;
BEGIN;
DELETE FROM t1 WHERE b = 'Stalin';
DELETE FROM t2 WHERE b = 'Stalin';
COMMIT;

PRAGMA auto_vacuum = none;
PRAGMA max_page_count = 10;
CREATE TABLE t2(a, b);
CREATE TABLE t3(a, b);
CREATE TABLE t4(a, b);
CREATE TABLE t5(a, b);
CREATE TABLE t6(a, b);
CREATE TABLE t7(a, b);
CREATE TABLE t8(a, b);
CREATE TABLE t9(a, b);
CREATE TABLE t10(a, b);

PRAGMA locking_mode = EXCLUSIVE;
CREATE TABLE t1(a, b);
BEGIN;
PRAGMA journal_mode = delete;
PRAGMA journal_mode = truncate;

PRAGMA auto_vacuum = 2;
PRAGMA cache_size = 10;
CREATE TABLE z(x INTEGER PRIMARY KEY, y);
BEGIN;
INSERT INTO z VALUES(NULL, a_string(800));
INSERT INTO z SELECT NULL, a_string(800) FROM z;     INSERT INTO z SELECT NULL, a_string(800) FROM z;     INSERT INTO z SELECT NULL, a_string(800) FROM z;     INSERT INTO z SELECT NULL, a_string(800) FROM z;     INSERT INTO z SELECT NULL, a_string(800) FROM z;     INSERT INTO z SELECT NULL, a_string(800) FROM z;     INSERT INTO z SELECT NULL, a_string(800) FROM z;     INSERT INTO z SELECT NULL, a_string(800) FROM z;     COMMIT;

INSERT INTO t1 VALUES(1, 2);

PRAGMA journal_mode = persist;

COMMIT;

PRAGMA journal_mode = persist;
PRAGMA journal_size_limit;

PRAGMA auto_vacuum = 1;
CREATE TABLE x1(x);
INSERT INTO x1 VALUES('Charles');
INSERT INTO x1 VALUES('James');
INSERT INTO x1 VALUES('Mary');
SELECT * FROM x1;

PRAGMA cache_size = 10;
BEGIN;
CREATE TABLE ab(a, b, UNIQUE(a, b));
INSERT INTO ab VALUES( a_string(200), a_string(300) );
INSERT INTO ab SELECT a_string(200), a_string(300) FROM ab;
INSERT INTO ab SELECT a_string(200), a_string(300) FROM ab;
INSERT INTO ab SELECT a_string(200), a_string(300) FROM ab;
INSERT INTO ab SELECT a_string(200), a_string(300) FROM ab;
INSERT INTO ab SELECT a_string(200), a_string(300) FROM ab;
INSERT INTO ab SELECT a_string(200), a_string(300) FROM ab;
INSERT INTO ab SELECT a_string(200), a_string(300) FROM ab;
COMMIT;

UPDATE ab SET a = a_string(201);

UPDATE ab SET b = a_string(301);

SELECT count(*) FROM ab;

UPDATE ab SET a = a_string(202);

PRAGMA auto_vacuum;

BEGIN;
UPDATE ab SET b = a_string(301);
ROLLBACK;

SELECT count(*) FROM ab;

PRAGMA page_size = 1024;

PRAGMA page_size = 4096;
PRAGMA synchronous = OFF;
CREATE TABLE t1(a, b);
CREATE TABLE t2(a, b);

PRAGMA page_size = 4096;
CREATE TABLE t1(a, b);
CREATE TABLE t2(a, b);

PRAGMA journal_mode = PERSIST;
PRAGMA page_size = 1024;
BEGIN;
CREATE TABLE t1(a, b);
CREATE TABLE t2(a, b);
CREATE TABLE t3(a, b);
COMMIT;

INSERT INTO t3 VALUES(a_string(300), a_string(300));
INSERT INTO t3 SELECT * FROM t3;        /*  2 */
INSERT INTO t3 SELECT * FROM t3;        /*  4 */
INSERT INTO t3 SELECT * FROM t3;        /*  8 */
INSERT INTO t3 SELECT * FROM t3;        /* 16 */
INSERT INTO t3 SELECT * FROM t3;        /* 32 */;

PRAGMA cache_size = 10;
BEGIN;

COMMIT;
SELECT * FROM t2;

CREATE TABLE t6(a, b);
CREATE TABLE t7(a, b);
CREATE TABLE t5(a, b);
DROP TABLE t6;
DROP TABLE t7;

CREATE TABLE x(y, z);
INSERT INTO x VALUES(1, 2);

BEGIN;
CREATE TABLE t6(a, b);

COMMIT;
SELECT * FROM t5;

PRAGMA auto_vacuum = none;
PRAGMA page_size = 1024;
CREATE TABLE t1(x);

INSERT INTO t1 VALUES(zeroblob(900));

CREATE TABLE t2(x);
DROP TABLE t2;

BEGIN;
CREATE TABLE t2(x);

CREATE TABLE t3(x);
COMMIT;

PRAGMA journal_mode = TRUNCATE;
PRAGMA integrity_check;

SELECT count(*) FROM v;
PRAGMA main.page_size;

SELECT count(*) FROM v;
PRAGMA main.page_size;

SELECT * FROM x;

SELECT sum(length(b)) FROM t1;

PRAGMA integrity_check;

SELECT sum(length(b)) FROM t1;

PRAGMA integrity_check;

PRAGMA page_size = 1024;
CREATE TABLE t1(a, b);
INSERT INTO t1 VALUES(a_string(500), a_string(200));
INSERT INTO t1 SELECT a_string(500), a_string(200) FROM t1;
INSERT INTO t1 SELECT a_string(500), a_string(200) FROM t1;
INSERT INTO t1 SELECT a_string(500), a_string(200) FROM t1;
INSERT INTO t1 SELECT a_string(500), a_string(200) FROM t1;
INSERT INTO t1 SELECT a_string(500), a_string(200) FROM t1;
INSERT INTO t1 SELECT a_string(500), a_string(200) FROM t1;
INSERT INTO t1 SELECT a_string(500), a_string(200) FROM t1;

PRAGMA writable_schema = 1;
UPDATE sqlite_master SET rootpage = lockingpage;

CREATE TABLE t2(x);
INSERT INTO t2 VALUES(a_string(5000));

CREATE TABLE t1(a, b);
CREATE TABLE t2(a, b);
PRAGMA writable_schema = 1;
UPDATE sqlite_master SET rootpage=5 WHERE tbl_name = 't1';
PRAGMA writable_schema = 0;
ALTER TABLE t1 RENAME TO x1;

PRAGMA page_size = 1024;
CREATE TABLE t1(x);
INSERT INTO t1 VALUES(a_string(800));
INSERT INTO t1 VALUES(a_string(800));

PRAGMA page_size = 512;
PRAGMA auto_vacuum = 1;
CREATE TABLE t1(aa, ab, ac, ad, ae, af, ag, ah, ai, aj, ak, al, am, an,
ba, bb, bc, bd, be, bf, bg, bh, bi, bj, bk, bl, bm, bn,
ca, cb, cc, cd, ce, cf, cg, ch, ci, cj, ck, cl, cm, cn,
da, db, dc, dd, de, df, dg, dh, di, dj, dk, dl, dm, dn,
ea, eb, ec, ed, ee, ef, eg, eh, ei, ej, ek, el, em, en,
fa, fb, fc, fd, fe, ff, fg, fh, fi, fj, fk, fl, fm, fn,
ga, gb, gc, gd, ge, gf, gg, gh, gi, gj, gk, gl, gm, gn,
ha, hb, hc, hd, he, hf, hg, hh, hi, hj, hk, hl, hm, hn,
ia, ib, ic, id, ie, if, ig, ih, ii, ij, ik, il, im, ix,
ja, jb, jc, jd, je, jf, jg, jh, ji, jj, jk, jl, jm, jn,
ka, kb, kc, kd, ke, kf, kg, kh, ki, kj, kk, kl, km, kn,
la, lb, lc, ld, le, lf, lg, lh, li, lj, lk, ll, lm, ln,
ma, mb, mc, md, me, mf, mg, mh, mi, mj, mk, ml, mm, mn
);
CREATE TABLE t2(aa, ab, ac, ad, ae, af, ag, ah, ai, aj, ak, al, am, an,
ba, bb, bc, bd, be, bf, bg, bh, bi, bj, bk, bl, bm, bn,
ca, cb, cc, cd, ce, cf, cg, ch, ci, cj, ck, cl, cm, cn,
da, db, dc, dd, de, df, dg, dh, di, dj, dk, dl, dm, dn,
ea, eb, ec, ed, ee, ef, eg, eh, ei, ej, ek, el, em, en,
fa, fb, fc, fd, fe, ff, fg, fh, fi, fj, fk, fl, fm, fn,
ga, gb, gc, gd, ge, gf, gg, gh, gi, gj, gk, gl, gm, gn,
ha, hb, hc, hd, he, hf, hg, hh, hi, hj, hk, hl, hm, hn,
ia, ib, ic, id, ie, if, ig, ih, ii, ij, ik, il, im, ix,
ja, jb, jc, jd, je, jf, jg, jh, ji, jj, jk, jl, jm, jn,
ka, kb, kc, kd, ke, kf, kg, kh, ki, kj, kk, kl, km, kn,
la, lb, lc, ld, le, lf, lg, lh, li, lj, lk, ll, lm, ln,
ma, mb, mc, md, me, mf, mg, mh, mi, mj, mk, ml, mm, mn
);
INSERT INTO t1(aa) VALUES( a_string(100000) );
INSERT INTO t2(aa) VALUES( a_string(100000) );
VACUUM;

ATTACH 'test.db2' AS aux;
PRAGMA journal_mode = DELETE;
PRAGMA main.cache_size = 10;
PRAGMA aux.cache_size = 10;
CREATE TABLE t1(a UNIQUE, b UNIQUE);
CREATE TABLE aux.t2(a UNIQUE, b UNIQUE);
INSERT INTO t1 VALUES(a_string(200), a_string(300));
INSERT INTO t1 SELECT a_string(200), a_string(300) FROM t1;
INSERT INTO t1 SELECT a_string(200), a_string(300) FROM t1;
INSERT INTO t2 SELECT * FROM t1;
BEGIN;
INSERT INTO t1 SELECT a_string(201), a_string(301) FROM t1;
INSERT INTO t1 SELECT a_string(202), a_string(302) FROM t1;
INSERT INTO t1 SELECT a_string(203), a_string(303) FROM t1;
INSERT INTO t1 SELECT a_string(204), a_string(304) FROM t1;
REPLACE INTO t2 SELECT * FROM t1;
COMMIT;

CREATE TABLE one(two, three);
INSERT INTO one VALUES('a', 'b');

BEGIN EXCLUSIVE;
COMMIT;

PRAGMA locking_mode = exclusive;
PRAGMA journal_mode = persist;
CREATE TABLE one(two, three);
INSERT INTO one VALUES('a', 'b');

BEGIN EXCLUSIVE;
COMMIT;

PRAGMA cache_size = 10;
PRAGMA journal_mode = wal;
BEGIN;
CREATE TABLE t1(x);
CREATE TABLE t2(y);
INSERT INTO t1 VALUES(a_string(800));
INSERT INTO t1 SELECT a_string(800) FROM t1;         /*   2 */
INSERT INTO t1 SELECT a_string(800) FROM t1;         /*   4 */
INSERT INTO t1 SELECT a_string(800) FROM t1;         /*   8 */
INSERT INTO t1 SELECT a_string(800) FROM t1;         /*  16 */
INSERT INTO t1 SELECT a_string(800) FROM t1;         /*  32 */
COMMIT;

BEGIN;
INSERT INTO t2 VALUES('xxxx');

PRAGMA journal_mode = WAL;
CREATE TABLE ko(c DEFAULT 'abc', b DEFAULT 'def');
INSERT INTO ko DEFAULT VALUES;

CREATE TABLE ko(c DEFAULT 'abc', b DEFAULT 'def');
INSERT INTO ko DEFAULT VALUES;

PRAGMA wal_checkpoint;

PRAGMA synchronous = off;
PRAGMA journal_mode = WAL;
INSERT INTO ko DEFAULT VALUES;

SELECT count(*) FROM t1;
PRAGMA integrity_check;

PRAGMA wal_checkpoint;

PRAGMA journal_mode = PERSIST;
CREATE TABLE t1(a, b);

PRAGMA journal_mode = DELETE;

PRAGMA journal_mode = PERSIST;
INSERT INTO t1 VALUES('Canberra', 'ACT');

SELECT * FROM t1;

PRAGMA journal_mode = DELETE;

PRAGMA journal_mode;

PRAGMA journal_mode = PERSIST;
INSERT INTO t1 VALUES('Darwin', 'NT');
BEGIN IMMEDIATE;

PRAGMA journal_mode = DELETE;

PRAGMA journal_mode;

SELECT count(*) FROM t1;
PRAGMA integrity_check;

PRAGMA journal_mode = PERSIST;
INSERT INTO t1 VALUES('Adelaide', 'SA');
BEGIN EXCLUSIVE;

PRAGMA journal_mode = DELETE;

PRAGMA journal_mode;

PRAGMA locking_mode = normal;

PRAGMA locking_mode = exclusive;

PRAGMA locking_mode;

PRAGMA main.locking_mode;

PRAGMA cache_size = 10;
PRAGMA auto_vacuum = FULL;
CREATE TABLE x1(x, y, z, PRIMARY KEY(y, z));
CREATE TABLE x2(x, y, z, PRIMARY KEY(y, z));
INSERT INTO x2 VALUES(a_string(400), a_string(500), a_string(600));
INSERT INTO x2 SELECT a_string(600), a_string(400), a_string(500) FROM x2;
INSERT INTO x2 SELECT a_string(500), a_string(600), a_string(400) FROM x2;
INSERT INTO x2 SELECT a_string(400), a_string(500), a_string(600) FROM x2;
INSERT INTO x2 SELECT a_string(600), a_string(400), a_string(500) FROM x2;
INSERT INTO x2 SELECT a_string(500), a_string(600), a_string(400) FROM x2;
INSERT INTO x2 SELECT a_string(400), a_string(500), a_string(600) FROM x2;
INSERT INTO x1 SELECT * FROM x2;

BEGIN;
DELETE FROM x1 WHERE rowid<32;

UPDATE x1 SET z = a_string(300) WHERE rowid>40;
COMMIT;
PRAGMA integrity_check;
SELECT count(*) FROM x1;

-- ===pager2.test===
PRAGMA cache_size = 10;
CREATE TABLE t1(i INTEGER PRIMARY KEY, j blob);

COMMIT ; BEGIN;

SELECT COALESCE(max(i), 0) FROM t1;
PRAGMA integrity_check;

INSERT INTO t1(j) VALUES(randomblob(1500));

CREATE TABLE t1(a, b);
PRAGMA journal_mode = off;
BEGIN;
INSERT INTO t1 VALUES(1, 2);
ROLLBACK;
SELECT * FROM t1;

PRAGMA auto_vacuum = incremental;
PRAGMA page_size = 1024;
PRAGMA journal_mode = off;
CREATE TABLE t1(a, b);
INSERT INTO t1 VALUES(zeroblob(5000), zeroblob(5000));
DELETE FROM t1;
PRAGMA incremental_vacuum;