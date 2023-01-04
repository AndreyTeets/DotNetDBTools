-- ===incrblob_err.test===
CREATE TABLE blobs(k, v BLOB);
INSERT INTO blobs VALUES(1, zeroblob(bytes));

CREATE TABLE blobs(k, v BLOB);
INSERT INTO blobs VALUES(1, data);

CREATE TABLE blobs(k, v BLOB);
INSERT INTO blobs VALUES(1, data);