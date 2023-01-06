PRAGMA foreign_keys=off;
BEGIN TRANSACTION;

-- QUERY START: DropTriggerQuery
DROP TRIGGER [TR_MyTable2_MyTrigger1];
-- QUERY END: DropTriggerQuery

-- QUERY START: DropViewQuery
DROP VIEW [MyView1];
-- QUERY END: DropViewQuery

-- QUERY START: DropIndexQuery
DROP INDEX [IDX_MyTable2_MyIndex1];
-- QUERY END: DropIndexQuery

-- QUERY START: DropTableQuery
DROP TABLE [MyTable3];
-- QUERY END: DropTableQuery

-- QUERY START: AlterTableQuery
CREATE TABLE [_DNDBTTemp_MyTable1]
(
    [MyColumn1] INTEGER NOT NULL DEFAULT 15,
    [MyColumn2] TEXT NOT NULL DEFAULT '33',
    [MyColumn3] INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
    [MyColumn4] NUMERIC NOT NULL DEFAULT 7.36,
    [MyColumn5] TEXT NULL DEFAULT 'some text',
    CONSTRAINT [UQ_MyTable1_MyColumn4] UNIQUE ([MyColumn4]),
    CONSTRAINT [CK_MyTable1_MyCheck1] CHECK (MyColumn4 >= 0),
    CONSTRAINT [FK_MyTable1_MyColumn1_MyTable2_MyColumn1] FOREIGN KEY ([MyColumn1])
        REFERENCES [MyTable2]([MyColumn1])
        ON UPDATE NO ACTION ON DELETE CASCADE
);

INSERT INTO [_DNDBTTemp_MyTable1]
(
    [MyColumn1],
    [MyColumn4],
    [MyColumn5]
)
SELECT
    [MyColumn1],
    [MyColumn4],
    [MyColumn5]
FROM [MyTable1NewName];

DROP TABLE [MyTable1NewName];

ALTER TABLE [_DNDBTTemp_MyTable1] RENAME TO [MyTable1];
-- QUERY END: AlterTableQuery

-- QUERY START: AlterTableQuery
CREATE TABLE [_DNDBTTemp_MyTable2]
(
    [MyColumn1] INTEGER PRIMARY KEY NOT NULL DEFAULT 333,
    [MyColumn2] BLOB NULL DEFAULT X'000408'
);

INSERT INTO [_DNDBTTemp_MyTable2]
(
    [MyColumn1]
)
SELECT
    [MyColumn1NewName]
FROM [MyTable2];

DROP TABLE [MyTable2];

ALTER TABLE [_DNDBTTemp_MyTable2] RENAME TO [MyTable2];
-- QUERY END: AlterTableQuery

-- QUERY START: AlterTableQuery
CREATE TABLE [_DNDBTTemp_MyTable5]
(
    [MyColumn1] INTEGER NOT NULL DEFAULT (ABs(-15)),
    [MyColumn10] NUMERIC NOT NULL DEFAULT '16:17:18',
    [MyColumn11] NUMERIC NOT NULL DEFAULT '2022-02-15 16:17:18',
    [MyColumn12] NUMERIC NOT NULL DEFAULT '2022-02-15 16:17:18+01:30',
    [MyColumn2] TEXT NOT NULL DEFAULT 'test',
    [MyColumn3] BLOB NOT NULL DEFAULT X'000204',
    [MyColumn4] REAL NOT NULL DEFAULT 123.456,
    [MyColumn5] REAL NOT NULL DEFAULT 12345.6789,
    [MyColumn6] NUMERIC NOT NULL DEFAULT 12.3,
    [MyColumn7] INTEGER NOT NULL DEFAULT TRUE,
    [MyColumn8] BLOB NOT NULL DEFAULT X'8e2f99ad0fc8456db0e4ec3ba572dd15',
    [MyColumn9] NUMERIC NOT NULL DEFAULT '2022-02-15',
    CONSTRAINT [PK_MyTable5] PRIMARY KEY ([MyColumn2], [MyColumn1]),
    CONSTRAINT [UQ_MyTable5_CustomName] UNIQUE ([MyColumn6], [MyColumn3], [MyColumn7])
);

INSERT INTO [_DNDBTTemp_MyTable5]
(
    [MyColumn1],
    [MyColumn10],
    [MyColumn11],
    [MyColumn12],
    [MyColumn2],
    [MyColumn3],
    [MyColumn4],
    [MyColumn5],
    [MyColumn6],
    [MyColumn7],
    [MyColumn8],
    [MyColumn9]
)
SELECT
    [MyColumn1],
    [MyColumn10],
    [MyColumn11],
    [MyColumn12],
    [MyColumn2],
    [MyColumn3],
    [MyColumn4],
    [MyColumn5],
    [MyColumn6],
    [MyColumn7],
    [MyColumn8],
    [MyColumn9]
FROM [MyTable5];

DROP TABLE [MyTable5];

ALTER TABLE [_DNDBTTemp_MyTable5] RENAME TO [MyTable5];
-- QUERY END: AlterTableQuery

-- QUERY START: CreateTableQuery
CREATE TABLE [MyTable6]
(
    [MyColumn1] TEXT NULL,
    [MyColumn2] INTEGER NULL,
    CONSTRAINT [FK_MyTable6_MyTable5_CustomName] FOREIGN KEY ([MyColumn1], [MyColumn2])
        REFERENCES [MyTable5]([MyColumn2], [MyColumn1])
        ON UPDATE NO ACTION ON DELETE NO ACTION
);
-- QUERY END: CreateTableQuery

-- QUERY START: CreateIndexQuery
CREATE UNIQUE INDEX [IDX_MyTable2_MyIndex1]
    ON [MyTable2] ([MyColumn1], [MyColumn2]);
-- QUERY END: CreateIndexQuery

-- QUERY START: CreateIndexQuery
CREATE INDEX [IDX_MyTable5_CustomName]
    ON [MyTable5] ([MyColumn8]);
-- QUERY END: CreateIndexQuery

-- QUERY START: CreateViewQuery
CREATE VIEW MyView1 AS
SELECT
    t1.MyColumn1,
    t1.MyColumn4,
    t2.MyColumn2
FROM MyTable1 t1
LEFT JOIN MyTable2 t2
    ON t2.MyColumn1 = t1.MyColumn1;
-- QUERY END: CreateViewQuery

-- QUERY START: CreateTriggerQuery
CREATE TRIGGER [TR_MyTable2_MyTrigger1]
AFTER INSERT
ON [MyTable2]
FOR EACH ROW
BEGIN
    INSERT INTO [MyTable4]([MyColumn1])
    VALUES(NEW.[MyColumn1]);
END;
-- QUERY END: CreateTriggerQuery

COMMIT TRANSACTION;