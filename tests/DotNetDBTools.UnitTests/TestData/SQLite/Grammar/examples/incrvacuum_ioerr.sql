-- ===incrvacuum_ioerr.test===
PRAGMA auto_vacuum = 'full';
PRAGMA cache_size = 10;
BEGIN;
CREATE TABLE abc(a, UNIQUE(a));

pragma freelist_count;

pragma incremental_vacuum(5);

pragma freelist_count;

INSERT INTO abc VALUES(randstr(1500,1500));

PRAGMA auto_vacuum = 'full';
PRAGMA cache_size = 10;
BEGIN;
CREATE TABLE abc(a, UNIQUE(a));

INSERT INTO abc VALUES(randstr(1500,1500));

PRAGMA auto_vacuum = 'incremental';
BEGIN;
CREATE TABLE a(i integer, b blob);
INSERT INTO a VALUES(1, randstr(1500,1500));
INSERT INTO a VALUES(2, randstr(1500,1500));

DELETE FROM a WHERE oid;

PRAGMA page_size = 1024;
PRAGMA locking_mode = exclusive;
PRAGMA auto_vacuum = 'incremental';
BEGIN;
CREATE TABLE a(i integer, b blob);

INSERT INTO a VALUES(ii, randstr(800,1500));

DELETE FROM a WHERE oid