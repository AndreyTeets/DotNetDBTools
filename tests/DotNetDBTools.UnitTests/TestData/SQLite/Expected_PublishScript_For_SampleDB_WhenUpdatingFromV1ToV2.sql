PRAGMA foreign_keys=off;
BEGIN TRANSACTION;

-- QUERY START: GenericQuery
DROP TABLE IF EXISTS [_MyTable2];

CREATE TABLE [_MyTable2]
(
    [MyColumn1] INTEGER NOT NULL PRIMARY KEY,
    [MyColumn2] BLOB
);

INSERT INTO [_MyTable2] ([MyColumn1], [MyColumn2])
SELECT [MyColumn1], [MyColumn2] FROM [MyTable2];
-- QUERY END: GenericQuery

-- QUERY START: SQLiteInsertDNDBTScriptExecutionRecordQuery
INSERT INTO [DNDBTScriptExecutions]
(
    [ID],
    [Type],
    [Name],
    [Code],
    [MinDbVersionToExecute],
    [MaxDbVersionToExecute],
    [ExecutedOnDbVersion]
)
VALUES
(
    '7f72f0df-4eda-4063-99d8-99c1f37819d2',
    'BeforePublishOnce',
    'SaveRecreatedColumnsData',
    'DROP TABLE IF EXISTS [_MyTable2];

CREATE TABLE [_MyTable2]
(
    [MyColumn1] INTEGER NOT NULL PRIMARY KEY,
    [MyColumn2] BLOB
);

INSERT INTO [_MyTable2] ([MyColumn1], [MyColumn2])
SELECT [MyColumn1], [MyColumn2] FROM [MyTable2]',
    1,
    1,
    1
);
-- QUERY END: SQLiteInsertDNDBTScriptExecutionRecordQuery

-- QUERY START: GenericQuery
DROP VIEW [MyView1];
-- QUERY END: GenericQuery

-- QUERY START: SQLiteDeleteDNDBTDbObjectRecordQuery
DELETE FROM [DNDBTDbObjects]
WHERE [ID] = 'e2569aae-d5da-4a77-b3cd-51adbdb272d9';
-- QUERY END: SQLiteDeleteDNDBTDbObjectRecordQuery

-- QUERY START: SQLiteDropTableQuery
DROP TABLE [MyTable6];
-- QUERY END: SQLiteDropTableQuery

-- QUERY START: SQLiteDeleteDNDBTDbObjectRecordQuery
DELETE FROM [DNDBTDbObjects]
WHERE [ID] = 'bfa08c82-5c8f-4ab4-bd41-1f1d85cf3c85';
-- QUERY END: SQLiteDeleteDNDBTDbObjectRecordQuery

-- QUERY START: SQLiteDeleteDNDBTDbObjectRecordQuery
DELETE FROM [DNDBTDbObjects]
WHERE [ID] = 'a402e2b7-c826-4cfd-a304-97c9bc346ba2';
-- QUERY END: SQLiteDeleteDNDBTDbObjectRecordQuery

-- QUERY START: SQLiteDeleteDNDBTDbObjectRecordQuery
DELETE FROM [DNDBTDbObjects]
WHERE [ID] = 'f3064a8c-346a-4b3d-af2c-d967b39841e4';
-- QUERY END: SQLiteDeleteDNDBTDbObjectRecordQuery

-- QUERY START: SQLiteDeleteDNDBTDbObjectRecordQuery
DELETE FROM [DNDBTDbObjects]
WHERE [ID] = 'ae453b22-d270-41fc-8184-9ac26b7a0569';
-- QUERY END: SQLiteDeleteDNDBTDbObjectRecordQuery

-- QUERY START: SQLiteAlterTableQuery
CREATE TABLE [_DNDBTTemp_MyTable1NewName]
(
    [MyColumn1] INTEGER NULL DEFAULT 15,
    [MyColumn4] NUMERIC NOT NULL DEFAULT 7.36,
    CONSTRAINT [FK_MyTable1_MyColumn1_MyTable2_MyColumn1] FOREIGN KEY ([MyColumn1])
        REFERENCES [MyTable2]([MyColumn1NewName])
        ON UPDATE NO ACTION ON DELETE SET NULL,
    CONSTRAINT [CK_MyTable1_MyCheck1] CHECK (MyColumn4 >= 1)
);

INSERT INTO [_DNDBTTemp_MyTable1NewName]([MyColumn1], [MyColumn4])
SELECT [MyColumn1], [MyColumn4]
FROM [MyTable1];

DROP TABLE [MyTable1];

ALTER TABLE [_DNDBTTemp_MyTable1NewName] RENAME TO [MyTable1NewName];
-- QUERY END: SQLiteAlterTableQuery

-- QUERY START: SQLiteDeleteDNDBTDbObjectRecordQuery
DELETE FROM [DNDBTDbObjects]
WHERE [ID] = 'eb9c59b5-bc7e-49d7-adaa-f5600b6a19a2';
-- QUERY END: SQLiteDeleteDNDBTDbObjectRecordQuery

-- QUERY START: SQLiteDeleteDNDBTDbObjectRecordQuery
DELETE FROM [DNDBTDbObjects]
WHERE [ID] = 'f3f08522-26ee-4950-9135-22edf2e4e0cf';
-- QUERY END: SQLiteDeleteDNDBTDbObjectRecordQuery

-- QUERY START: SQLiteDeleteDNDBTDbObjectRecordQuery
DELETE FROM [DNDBTDbObjects]
WHERE [ID] = '37a45def-f4a0-4be7-8bfb-8fbed4a7d705';
-- QUERY END: SQLiteDeleteDNDBTDbObjectRecordQuery

-- QUERY START: SQLiteDeleteDNDBTDbObjectRecordQuery
DELETE FROM [DNDBTDbObjects]
WHERE [ID] = 'fe68ee3d-09d0-40ac-93f9-5e441fbb4f70';
-- QUERY END: SQLiteDeleteDNDBTDbObjectRecordQuery

-- QUERY START: SQLiteDeleteDNDBTDbObjectRecordQuery
DELETE FROM [DNDBTDbObjects]
WHERE [ID] = '6e95de30-e01a-4fb4-b8b7-8f0c40bb682c';
-- QUERY END: SQLiteDeleteDNDBTDbObjectRecordQuery

-- QUERY START: SQLiteUpdateDNDBTDbObjectRecordQuery
UPDATE [DNDBTDbObjects] SET
    [Name] = 'MyTable1NewName',
    [Code] = NULL
WHERE [ID] = '299675e6-4faa-4d0f-a36a-224306ba5bcb';
-- QUERY END: SQLiteUpdateDNDBTDbObjectRecordQuery

-- QUERY START: SQLiteUpdateDNDBTDbObjectRecordQuery
UPDATE [DNDBTDbObjects] SET
    [Name] = 'MyColumn1',
    [Code] = '15'
WHERE [ID] = 'a2f2a4de-1337-4594-ae41-72ed4d05f317';
-- QUERY END: SQLiteUpdateDNDBTDbObjectRecordQuery

-- QUERY START: SQLiteInsertDNDBTDbObjectRecordQuery
INSERT INTO [DNDBTDbObjects]
(
    [ID],
    [ParentID],
    [Type],
    [Name],
    [Code]
)
VALUES
(
    'eb9c59b5-bc7e-49d7-adaa-f5600b6a19a2',
    '299675e6-4faa-4d0f-a36a-224306ba5bcb',
    'CheckConstraint',
    'CK_MyTable1_MyCheck1',
    'CHECK (MyColumn4 >= 1)'
);
-- QUERY END: SQLiteInsertDNDBTDbObjectRecordQuery

-- QUERY START: SQLiteDeleteDNDBTDbObjectRecordQuery
DELETE FROM [DNDBTDbObjects]
WHERE [ID] = 'd11b2a53-32db-432f-bb6b-f91788844ba9';
-- QUERY END: SQLiteDeleteDNDBTDbObjectRecordQuery

-- QUERY START: SQLiteInsertDNDBTDbObjectRecordQuery
INSERT INTO [DNDBTDbObjects]
(
    [ID],
    [ParentID],
    [Type],
    [Name],
    [Code]
)
VALUES
(
    'd11b2a53-32db-432f-bb6b-f91788844ba9',
    '299675e6-4faa-4d0f-a36a-224306ba5bcb',
    'ForeignKey',
    'FK_MyTable1_MyColumn1_MyTable2_MyColumn1',
    NULL
);
-- QUERY END: SQLiteInsertDNDBTDbObjectRecordQuery

-- QUERY START: SQLiteAlterTableQuery
DROP INDEX [IDX_MyTable2_MyIndex1];

DROP TRIGGER [TR_MyTable2_MyTrigger1];

CREATE TABLE [_DNDBTTemp_MyTable2]
(
    [MyColumn1NewName] INTEGER PRIMARY KEY NOT NULL DEFAULT 333,
    [MyColumn2] BLOB NULL DEFAULT X'000408',
    [MyColumn3] INTEGER NULL,
    [MyColumn4] BLOB NULL,
    CONSTRAINT [FK_MyTable2_MyColumns34_MyTable3_MyColumns12] FOREIGN KEY ([MyColumn3], [MyColumn4])
        REFERENCES [MyTable3]([MyColumn1], [MyColumn2])
        ON UPDATE NO ACTION ON DELETE SET DEFAULT
);

INSERT INTO [_DNDBTTemp_MyTable2]([MyColumn1NewName])
SELECT [MyColumn1]
FROM [MyTable2];

DROP TABLE [MyTable2];

ALTER TABLE [_DNDBTTemp_MyTable2] RENAME TO [MyTable2];

CREATE TRIGGER [TR_MyTable2_MyTrigger1]
AFTER INSERT
ON [MyTable2]
FOR EACH ROW
BEGIN
    INSERT INTO [MyTable4]([MyColumn1])
    VALUES(NEW.[MyColumn1NewName]);
END;

CREATE UNIQUE INDEX [IDX_MyTable2_MyIndex1]
ON [MyTable2] ([MyColumn1NewName], [MyColumn2]);
-- QUERY END: SQLiteAlterTableQuery

-- QUERY START: SQLiteDeleteDNDBTDbObjectRecordQuery
DELETE FROM [DNDBTDbObjects]
WHERE [ID] = '3a43615b-40b3-4a13-99e7-93af7c56e8ce';
-- QUERY END: SQLiteDeleteDNDBTDbObjectRecordQuery

-- QUERY START: SQLiteDeleteDNDBTDbObjectRecordQuery
DELETE FROM [DNDBTDbObjects]
WHERE [ID] = '5a0d1926-3270-4eb2-92eb-00be56c7af23';
-- QUERY END: SQLiteDeleteDNDBTDbObjectRecordQuery

-- QUERY START: SQLiteUpdateDNDBTDbObjectRecordQuery
UPDATE [DNDBTDbObjects] SET
    [Name] = 'MyTable2',
    [Code] = NULL
WHERE [ID] = 'bfb9030c-a8c3-4882-9c42-1c6ad025cf8f';
-- QUERY END: SQLiteUpdateDNDBTDbObjectRecordQuery

-- QUERY START: SQLiteUpdateDNDBTDbObjectRecordQuery
UPDATE [DNDBTDbObjects] SET
    [Name] = 'MyColumn1NewName',
    [Code] = '333'
WHERE [ID] = 'c480f22f-7c01-4f41-b282-35e9f5cd1fe3';
-- QUERY END: SQLiteUpdateDNDBTDbObjectRecordQuery

-- QUERY START: SQLiteInsertDNDBTDbObjectRecordQuery
INSERT INTO [DNDBTDbObjects]
(
    [ID],
    [ParentID],
    [Type],
    [Name],
    [Code]
)
VALUES
(
    'c2df19c2-e029-4014-8a5b-4ab42fecb6b8',
    'bfb9030c-a8c3-4882-9c42-1c6ad025cf8f',
    'Column',
    'MyColumn2',
    'X''000408'''
);
-- QUERY END: SQLiteInsertDNDBTDbObjectRecordQuery

-- QUERY START: SQLiteInsertDNDBTDbObjectRecordQuery
INSERT INTO [DNDBTDbObjects]
(
    [ID],
    [ParentID],
    [Type],
    [Name],
    [Code]
)
VALUES
(
    '99bc3f49-3151-4f52-87f7-104b424ed7bf',
    'bfb9030c-a8c3-4882-9c42-1c6ad025cf8f',
    'Column',
    'MyColumn3',
    NULL
);
-- QUERY END: SQLiteInsertDNDBTDbObjectRecordQuery

-- QUERY START: SQLiteInsertDNDBTDbObjectRecordQuery
INSERT INTO [DNDBTDbObjects]
(
    [ID],
    [ParentID],
    [Type],
    [Name],
    [Code]
)
VALUES
(
    '87950a3f-2072-42db-ac3c-a4e85b79720d',
    'bfb9030c-a8c3-4882-9c42-1c6ad025cf8f',
    'Column',
    'MyColumn4',
    NULL
);
-- QUERY END: SQLiteInsertDNDBTDbObjectRecordQuery

-- QUERY START: SQLiteInsertDNDBTDbObjectRecordQuery
INSERT INTO [DNDBTDbObjects]
(
    [ID],
    [ParentID],
    [Type],
    [Name],
    [Code]
)
VALUES
(
    '3a43615b-40b3-4a13-99e7-93af7c56e8ce',
    'bfb9030c-a8c3-4882-9c42-1c6ad025cf8f',
    'PrimaryKey',
    'PK_MyTable2',
    NULL
);
-- QUERY END: SQLiteInsertDNDBTDbObjectRecordQuery

-- QUERY START: SQLiteDeleteDNDBTDbObjectRecordQuery
DELETE FROM [DNDBTDbObjects]
WHERE [ID] = '74390b3c-bc39-4860-a42e-12baa400f927';
-- QUERY END: SQLiteDeleteDNDBTDbObjectRecordQuery

-- QUERY START: SQLiteDeleteDNDBTDbObjectRecordQuery
DELETE FROM [DNDBTDbObjects]
WHERE [ID] = 'ee64ffc3-5536-4624-beaf-bc3a61d06a1a';
-- QUERY END: SQLiteDeleteDNDBTDbObjectRecordQuery

-- QUERY START: SQLiteInsertDNDBTDbObjectRecordQuery
INSERT INTO [DNDBTDbObjects]
(
    [ID],
    [ParentID],
    [Type],
    [Name],
    [Code]
)
VALUES
(
    '480f3508-9d51-4190-88aa-45bc20e49119',
    'bfb9030c-a8c3-4882-9c42-1c6ad025cf8f',
    'ForeignKey',
    'FK_MyTable2_MyColumns34_MyTable3_MyColumns12',
    NULL
);
-- QUERY END: SQLiteInsertDNDBTDbObjectRecordQuery

-- QUERY START: SQLiteInsertDNDBTDbObjectRecordQuery
INSERT INTO [DNDBTDbObjects]
(
    [ID],
    [ParentID],
    [Type],
    [Name],
    [Code]
)
VALUES
(
    'ee64ffc3-5536-4624-beaf-bc3a61d06a1a',
    'bfb9030c-a8c3-4882-9c42-1c6ad025cf8f',
    'Trigger',
    'TR_MyTable2_MyTrigger1',
    'CREATE TRIGGER [TR_MyTable2_MyTrigger1]
AFTER INSERT
ON [MyTable2]
FOR EACH ROW
BEGIN
    INSERT INTO [MyTable4]([MyColumn1])
    VALUES(NEW.[MyColumn1NewName]);
END'
);
-- QUERY END: SQLiteInsertDNDBTDbObjectRecordQuery

-- QUERY START: SQLiteInsertDNDBTDbObjectRecordQuery
INSERT INTO [DNDBTDbObjects]
(
    [ID],
    [ParentID],
    [Type],
    [Name],
    [Code]
)
VALUES
(
    '74390b3c-bc39-4860-a42e-12baa400f927',
    'bfb9030c-a8c3-4882-9c42-1c6ad025cf8f',
    'Index',
    'IDX_MyTable2_MyIndex1',
    NULL
);
-- QUERY END: SQLiteInsertDNDBTDbObjectRecordQuery

-- QUERY START: SQLiteAlterTableQuery
DROP INDEX [IDX_MyTable5_CustomName];

CREATE TABLE [_DNDBTTemp_MyTable5]
(
    [MyColumn1] INTEGER NOT NULL DEFAULT (ABS(-15)),
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
    [MyColumn9] NUMERIC NOT NULL DEFAULT '2022-02-15'
);

INSERT INTO [_DNDBTTemp_MyTable5]([MyColumn1], [MyColumn10], [MyColumn11], [MyColumn12], [MyColumn2], [MyColumn3], [MyColumn4], [MyColumn5], [MyColumn6], [MyColumn7], [MyColumn8], [MyColumn9])
SELECT [MyColumn1], [MyColumn10], [MyColumn11], [MyColumn12], [MyColumn2], [MyColumn3], [MyColumn4], [MyColumn5], [MyColumn6], [MyColumn7], [MyColumn8], [MyColumn9]
FROM [MyTable5];

DROP TABLE [MyTable5];

ALTER TABLE [_DNDBTTemp_MyTable5] RENAME TO [MyTable5];
-- QUERY END: SQLiteAlterTableQuery

-- QUERY START: SQLiteDeleteDNDBTDbObjectRecordQuery
DELETE FROM [DNDBTDbObjects]
WHERE [ID] = '5293b58a-9f63-4f0f-8d6f-18416ebbd751';
-- QUERY END: SQLiteDeleteDNDBTDbObjectRecordQuery

-- QUERY START: SQLiteDeleteDNDBTDbObjectRecordQuery
DELETE FROM [DNDBTDbObjects]
WHERE [ID] = '79384d48-a39b-4a22-900e-066b2ca67ba2';
-- QUERY END: SQLiteDeleteDNDBTDbObjectRecordQuery

-- QUERY START: SQLiteUpdateDNDBTDbObjectRecordQuery
UPDATE [DNDBTDbObjects] SET
    [Name] = 'MyTable5',
    [Code] = NULL
WHERE [ID] = '6ca51f29-c1bc-4349-b9c1-6f1ea170f162';
-- QUERY END: SQLiteUpdateDNDBTDbObjectRecordQuery

-- QUERY START: SQLiteDeleteDNDBTDbObjectRecordQuery
DELETE FROM [DNDBTDbObjects]
WHERE [ID] = '1d632285-9914-4c5d-98e6-a618a99bd799';
-- QUERY END: SQLiteDeleteDNDBTDbObjectRecordQuery

-- QUERY START: SQLiteCreateTableQuery
CREATE TABLE [MyTable3]
(
    [MyColumn1] INTEGER NOT NULL DEFAULT 444,
    [MyColumn2] BLOB NOT NULL,
    CONSTRAINT [UQ_MyTable3_MyColumns12] UNIQUE ([MyColumn1], [MyColumn2])
);
-- QUERY END: SQLiteCreateTableQuery

-- QUERY START: SQLiteInsertDNDBTDbObjectRecordQuery
INSERT INTO [DNDBTDbObjects]
(
    [ID],
    [ParentID],
    [Type],
    [Name],
    [Code]
)
VALUES
(
    '474cd761-2522-4529-9d20-2b94115f9626',
    NULL,
    'Table',
    'MyTable3',
    NULL
);
-- QUERY END: SQLiteInsertDNDBTDbObjectRecordQuery

-- QUERY START: SQLiteInsertDNDBTDbObjectRecordQuery
INSERT INTO [DNDBTDbObjects]
(
    [ID],
    [ParentID],
    [Type],
    [Name],
    [Code]
)
VALUES
(
    '726f503a-d944-46ee-a0ff-6a2c2faab46e',
    '474cd761-2522-4529-9d20-2b94115f9626',
    'Column',
    'MyColumn1',
    '444'
);
-- QUERY END: SQLiteInsertDNDBTDbObjectRecordQuery

-- QUERY START: SQLiteInsertDNDBTDbObjectRecordQuery
INSERT INTO [DNDBTDbObjects]
(
    [ID],
    [ParentID],
    [Type],
    [Name],
    [Code]
)
VALUES
(
    '169824e1-8b74-4b60-af17-99656d6dbbee',
    '474cd761-2522-4529-9d20-2b94115f9626',
    'Column',
    'MyColumn2',
    NULL
);
-- QUERY END: SQLiteInsertDNDBTDbObjectRecordQuery

-- QUERY START: SQLiteInsertDNDBTDbObjectRecordQuery
INSERT INTO [DNDBTDbObjects]
(
    [ID],
    [ParentID],
    [Type],
    [Name],
    [Code]
)
VALUES
(
    'fd288e38-35ba-4bb1-ace3-597c99ef26c7',
    '474cd761-2522-4529-9d20-2b94115f9626',
    'UniqueConstraint',
    'UQ_MyTable3_MyColumns12',
    NULL
);
-- QUERY END: SQLiteInsertDNDBTDbObjectRecordQuery

-- QUERY START: GenericQuery
CREATE VIEW MyView1 AS
SELECT
    t1.MyColumn1,
    t1.MyColumn4,
    t2.MyColumn2
FROM MyTable1NewName t1
LEFT JOIN MyTable2 t2
    ON t2.MyColumn1NewName = t1.MyColumn1;
-- QUERY END: GenericQuery

-- QUERY START: SQLiteInsertDNDBTDbObjectRecordQuery
INSERT INTO [DNDBTDbObjects]
(
    [ID],
    [ParentID],
    [Type],
    [Name],
    [Code]
)
VALUES
(
    'e2569aae-d5da-4a77-b3cd-51adbdb272d9',
    NULL,
    'View',
    'MyView1',
    'CREATE VIEW MyView1 AS
SELECT
    t1.MyColumn1,
    t1.MyColumn4,
    t2.MyColumn2
FROM MyTable1NewName t1
LEFT JOIN MyTable2 t2
    ON t2.MyColumn1NewName = t1.MyColumn1'
);
-- QUERY END: SQLiteInsertDNDBTDbObjectRecordQuery

-- QUERY START: GenericQuery
CREATE TABLE IF NOT EXISTS [_MyTable2]
(
    [MyColumn1] INTEGER NOT NULL PRIMARY KEY,
    [MyColumn2] BLOB
);

UPDATE [MyTable2] SET
    [MyColumn2] = [t].[MyColumn2]
FROM [_MyTable2] AS [t]
WHERE [MyTable2].[MyColumn1NewName] = [t].[MyColumn1];

DROP TABLE [_MyTable2];
-- QUERY END: GenericQuery

-- QUERY START: SQLiteInsertDNDBTScriptExecutionRecordQuery
INSERT INTO [DNDBTScriptExecutions]
(
    [ID],
    [Type],
    [Name],
    [Code],
    [MinDbVersionToExecute],
    [MaxDbVersionToExecute],
    [ExecutedOnDbVersion]
)
VALUES
(
    '8ccaf36e-e587-466e-86f7-45c0061ae521',
    'AfterPublishOnce',
    'RestoreRecreatedColumnsData',
    'CREATE TABLE IF NOT EXISTS [_MyTable2]
(
    [MyColumn1] INTEGER NOT NULL PRIMARY KEY,
    [MyColumn2] BLOB
);

UPDATE [MyTable2] SET
    [MyColumn2] = [t].[MyColumn2]
FROM [_MyTable2] AS [t]
WHERE [MyTable2].[MyColumn1NewName] = [t].[MyColumn1];

DROP TABLE [_MyTable2]',
    1,
    1,
    1
);
-- QUERY END: SQLiteInsertDNDBTScriptExecutionRecordQuery

-- QUERY START: SQLiteUpdateDNDBTDbAttributesRecordQuery
UPDATE [DNDBTDbAttributes] SET
    [Version] = 2;
-- QUERY END: SQLiteUpdateDNDBTDbAttributesRecordQuery

COMMIT TRANSACTION;