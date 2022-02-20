DROP TABLE IF EXISTS "_MyTable2";

CREATE TABLE "_MyTable2"
(
    "MyColumn1" BIGINT NOT NULL PRIMARY KEY,
    "MyColumn2" BYTEA
);

INSERT INTO "_MyTable2" ("MyColumn1", "MyColumn2")
SELECT "MyColumn1", "MyColumn2" FROM "MyTable2";