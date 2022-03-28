PRAGMA foreign_keys=off;
BEGIN TRANSACTION;

-- QUERY START: GenericQuery
DROP VIEW MyView1;
-- QUERY END: GenericQuery

-- QUERY START: SQLiteDropTableQuery
DROP TABLE MyTable3;
-- QUERY END: SQLiteDropTableQuery

-- QUERY START: SQLiteAlterTableQuery
CREATE TABLE _DNDBTTemp_MyTable1
(
    MyColumn1 INTEGER NOT NULL DEFAULT 15,
    MyColumn2 TEXT NOT NULL DEFAULT '33',
    MyColumn3 INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
    MyColumn4 NUMERIC NOT NULL DEFAULT 7.36,
    CONSTRAINT UQ_MyTable1_MyColumn2 UNIQUE (MyColumn2),
    CONSTRAINT FK_MyTable1_MyColumn1_MyTable2_MyColumn1 FOREIGN KEY (MyColumn1)
        REFERENCES MyTable2(MyColumn1)
        ON UPDATE NO ACTION ON DELETE CASCADE,
    CONSTRAINT [CK_MyTable1_MyCheck1] CHECK (MyColumn4 >= 0)
);

INSERT INTO  _DNDBTTemp_MyTable1(MyColumn1)
SELECT MyColumn1
FROM MyTable1NewName;

DROP TABLE MyTable1NewName;

ALTER TABLE _DNDBTTemp_MyTable1 RENAME TO MyTable1;
-- QUERY END: SQLiteAlterTableQuery

-- QUERY START: SQLiteAlterTableQuery
DROP INDEX IDX_MyTable2_MyIndex1;

DROP TRIGGER TR_MyTable2_MyTrigger1;

CREATE TABLE _DNDBTTemp_MyTable2
(
    MyColumn1 INTEGER PRIMARY KEY NOT NULL DEFAULT 333,
    MyColumn2 BLOB NULL DEFAULT X'000102'
);

INSERT INTO  _DNDBTTemp_MyTable2(MyColumn1)
SELECT MyColumn1NewName
FROM MyTable2;

DROP TABLE MyTable2;

ALTER TABLE _DNDBTTemp_MyTable2 RENAME TO MyTable2;

CREATE TRIGGER [TR_MyTable2_MyTrigger1]
AFTER INSERT
ON [MyTable2]
FOR EACH ROW
BEGIN
    INSERT INTO [MyTable4]([MyColumn1])
    VALUES(NEW.[MyColumn1]);
END;

CREATE UNIQUE INDEX IDX_MyTable2_MyIndex1
ON MyTable2 (MyColumn1, MyColumn2);
-- QUERY END: SQLiteAlterTableQuery

-- QUERY START: GenericQuery
CREATE VIEW MyView1 AS
SELECT
    t1.MyColumn1,
    t1.MyColumn4,
    t2.MyColumn2
FROM MyTable1 t1
LEFT JOIN MyTable2 t2
    ON t2.MyColumn1 = t1.MyColumn1;
-- QUERY END: GenericQuery

COMMIT TRANSACTION;