-- ===autovacuum_ioerr2.test===
PRAGMA auto_vacuum = 1;
PRAGMA cache_size = 10;
BEGIN;
CREATE TABLE abc(a);
INSERT INTO abc VALUES(randstr(1100,1100)); INSERT INTO abc VALUES(randstr(1100,1100)); -- Page 5 is overflow;

INSERT INTO abc VALUES(randstr(100,100));

PRAGMA auto_vacuum = 1;
BEGIN;
CREATE TABLE abc(a);
INSERT INTO abc VALUES(randstr(1100,1100)); INSERT INTO abc VALUES(randstr(1100,1100)); -- Page 5 is overflow;

INSERT INTO abc VALUES(randstr(100,100));

COMMIT;
PRAGMA cache_size = 10;

PRAGMA cache_size = 10;