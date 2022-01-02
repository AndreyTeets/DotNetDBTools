PRAGMA foreign_keys=off;
BEGIN TRANSACTION;

-- QUERY START: SQLiteCreateTableQuery
CREATE TABLE MyTable1
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
-- QUERY END: SQLiteCreateTableQuery

-- QUERY START: SQLiteInsertDNDBTSysInfoQuery
INSERT INTO DNDBTDbObjects
(
    ID,
    ParentID,
    Type,
    Name,
    Code
)
VALUES
(
    '299675e6-4faa-4d0f-a36a-224306ba5bcb',
    NULL,
    'Table',
    'MyTable1',
    NULL
);
-- QUERY END: SQLiteInsertDNDBTSysInfoQuery

-- QUERY START: SQLiteInsertDNDBTSysInfoQuery
INSERT INTO DNDBTDbObjects
(
    ID,
    ParentID,
    Type,
    Name,
    Code
)
VALUES
(
    'a2f2a4de-1337-4594-ae41-72ed4d05f317',
    '299675e6-4faa-4d0f-a36a-224306ba5bcb',
    'Column',
    'MyColumn1',
    NULL
);
-- QUERY END: SQLiteInsertDNDBTSysInfoQuery

-- QUERY START: SQLiteInsertDNDBTSysInfoQuery
INSERT INTO DNDBTDbObjects
(
    ID,
    ParentID,
    Type,
    Name,
    Code
)
VALUES
(
    'fe68ee3d-09d0-40ac-93f9-5e441fbb4f70',
    '299675e6-4faa-4d0f-a36a-224306ba5bcb',
    'Column',
    'MyColumn2',
    NULL
);
-- QUERY END: SQLiteInsertDNDBTSysInfoQuery

-- QUERY START: SQLiteInsertDNDBTSysInfoQuery
INSERT INTO DNDBTDbObjects
(
    ID,
    ParentID,
    Type,
    Name,
    Code
)
VALUES
(
    '6e95de30-e01a-4fb4-b8b7-8f0c40bb682c',
    '299675e6-4faa-4d0f-a36a-224306ba5bcb',
    'Column',
    'MyColumn3',
    NULL
);
-- QUERY END: SQLiteInsertDNDBTSysInfoQuery

-- QUERY START: SQLiteInsertDNDBTSysInfoQuery
INSERT INTO DNDBTDbObjects
(
    ID,
    ParentID,
    Type,
    Name,
    Code
)
VALUES
(
    '867ac528-e87e-4c93-b6e3-dd2fcbbb837f',
    '299675e6-4faa-4d0f-a36a-224306ba5bcb',
    'Column',
    'MyColumn4',
    NULL
);
-- QUERY END: SQLiteInsertDNDBTSysInfoQuery

-- QUERY START: SQLiteInsertDNDBTSysInfoQuery
INSERT INTO DNDBTDbObjects
(
    ID,
    ParentID,
    Type,
    Name,
    Code
)
VALUES
(
    '37a45def-f4a0-4be7-8bfb-8fbed4a7d705',
    '299675e6-4faa-4d0f-a36a-224306ba5bcb',
    'PrimaryKey',
    'PK_MyTable1',
    NULL
);
-- QUERY END: SQLiteInsertDNDBTSysInfoQuery

-- QUERY START: SQLiteInsertDNDBTSysInfoQuery
INSERT INTO DNDBTDbObjects
(
    ID,
    ParentID,
    Type,
    Name,
    Code
)
VALUES
(
    'f3f08522-26ee-4950-9135-22edf2e4e0cf',
    '299675e6-4faa-4d0f-a36a-224306ba5bcb',
    'UniqueConstraint',
    'UQ_MyTable1_MyColumn2',
    NULL
);
-- QUERY END: SQLiteInsertDNDBTSysInfoQuery

-- QUERY START: SQLiteInsertDNDBTSysInfoQuery
INSERT INTO DNDBTDbObjects
(
    ID,
    ParentID,
    Type,
    Name,
    Code
)
VALUES
(
    'eb9c59b5-bc7e-49d7-adaa-f5600b6a19a2',
    '299675e6-4faa-4d0f-a36a-224306ba5bcb',
    'CheckConstraint',
    'CK_MyTable1_MyCheck1',
    'CHECK (MyColumn4 >= 0)'
);
-- QUERY END: SQLiteInsertDNDBTSysInfoQuery

-- QUERY START: SQLiteInsertDNDBTSysInfoQuery
INSERT INTO DNDBTDbObjects
(
    ID,
    ParentID,
    Type,
    Name,
    Code
)
VALUES
(
    'd11b2a53-32db-432f-bb6b-f91788844ba9',
    '299675e6-4faa-4d0f-a36a-224306ba5bcb',
    'ForeignKey',
    'FK_MyTable1_MyColumn1_MyTable2_MyColumn1',
    NULL
);
-- QUERY END: SQLiteInsertDNDBTSysInfoQuery

-- QUERY START: SQLiteCreateTableQuery
CREATE TABLE MyTable2
(
    MyColumn1 INTEGER PRIMARY KEY NOT NULL DEFAULT 333,
    MyColumn2 BLOB NULL DEFAULT 0x000102
);

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
-- QUERY END: SQLiteCreateTableQuery

-- QUERY START: SQLiteInsertDNDBTSysInfoQuery
INSERT INTO DNDBTDbObjects
(
    ID,
    ParentID,
    Type,
    Name,
    Code
)
VALUES
(
    'bfb9030c-a8c3-4882-9c42-1c6ad025cf8f',
    NULL,
    'Table',
    'MyTable2',
    NULL
);
-- QUERY END: SQLiteInsertDNDBTSysInfoQuery

-- QUERY START: SQLiteInsertDNDBTSysInfoQuery
INSERT INTO DNDBTDbObjects
(
    ID,
    ParentID,
    Type,
    Name,
    Code
)
VALUES
(
    'c480f22f-7c01-4f41-b282-35e9f5cd1fe3',
    'bfb9030c-a8c3-4882-9c42-1c6ad025cf8f',
    'Column',
    'MyColumn1',
    NULL
);
-- QUERY END: SQLiteInsertDNDBTSysInfoQuery

-- QUERY START: SQLiteInsertDNDBTSysInfoQuery
INSERT INTO DNDBTDbObjects
(
    ID,
    ParentID,
    Type,
    Name,
    Code
)
VALUES
(
    '5a0d1926-3270-4eb2-92eb-00be56c7af23',
    'bfb9030c-a8c3-4882-9c42-1c6ad025cf8f',
    'Column',
    'MyColumn2',
    NULL
);
-- QUERY END: SQLiteInsertDNDBTSysInfoQuery

-- QUERY START: SQLiteInsertDNDBTSysInfoQuery
INSERT INTO DNDBTDbObjects
(
    ID,
    ParentID,
    Type,
    Name,
    Code
)
VALUES
(
    '3a43615b-40b3-4a13-99e7-93af7c56e8ce',
    'bfb9030c-a8c3-4882-9c42-1c6ad025cf8f',
    'PrimaryKey',
    'PK_MyTable2',
    NULL
);
-- QUERY END: SQLiteInsertDNDBTSysInfoQuery

-- QUERY START: SQLiteInsertDNDBTSysInfoQuery
INSERT INTO DNDBTDbObjects
(
    ID,
    ParentID,
    Type,
    Name,
    Code
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
    VALUES(NEW.[MyColumn1]);
END;'
);
-- QUERY END: SQLiteInsertDNDBTSysInfoQuery

-- QUERY START: SQLiteInsertDNDBTSysInfoQuery
INSERT INTO DNDBTDbObjects
(
    ID,
    ParentID,
    Type,
    Name,
    Code
)
VALUES
(
    '74390b3c-bc39-4860-a42e-12baa400f927',
    'bfb9030c-a8c3-4882-9c42-1c6ad025cf8f',
    'Index',
    'IDX_MyTable2_MyIndex1',
    NULL
);
-- QUERY END: SQLiteInsertDNDBTSysInfoQuery

-- QUERY START: SQLiteCreateTableQuery
CREATE TABLE MyTable4
(
    MyColumn1 INTEGER NOT NULL
);
-- QUERY END: SQLiteCreateTableQuery

-- QUERY START: SQLiteInsertDNDBTSysInfoQuery
INSERT INTO DNDBTDbObjects
(
    ID,
    ParentID,
    Type,
    Name,
    Code
)
VALUES
(
    'b12a6a37-7739-48e0-a9e1-499ae7d2a395',
    NULL,
    'Table',
    'MyTable4',
    NULL
);
-- QUERY END: SQLiteInsertDNDBTSysInfoQuery

-- QUERY START: SQLiteInsertDNDBTSysInfoQuery
INSERT INTO DNDBTDbObjects
(
    ID,
    ParentID,
    Type,
    Name,
    Code
)
VALUES
(
    'de0425b8-9f99-4d76-9a64-09e52f8b5d5a',
    'b12a6a37-7739-48e0-a9e1-499ae7d2a395',
    'Column',
    'MyColumn1',
    NULL
);
-- QUERY END: SQLiteInsertDNDBTSysInfoQuery

-- QUERY START: SQLiteCreateTableQuery
CREATE TABLE MyTable5
(
    MyColumn1 INTEGER NOT NULL DEFAULT (ABS(-15)),
    MyColumn3 NUMERIC NULL
);
-- QUERY END: SQLiteCreateTableQuery

-- QUERY START: SQLiteInsertDNDBTSysInfoQuery
INSERT INTO DNDBTDbObjects
(
    ID,
    ParentID,
    Type,
    Name,
    Code
)
VALUES
(
    '6ca51f29-c1bc-4349-b9c1-6f1ea170f162',
    NULL,
    'Table',
    'MyTable5',
    NULL
);
-- QUERY END: SQLiteInsertDNDBTSysInfoQuery

-- QUERY START: SQLiteInsertDNDBTSysInfoQuery
INSERT INTO DNDBTDbObjects
(
    ID,
    ParentID,
    Type,
    Name,
    Code
)
VALUES
(
    '5309d66f-2030-402e-912e-5547babaa072',
    '6ca51f29-c1bc-4349-b9c1-6f1ea170f162',
    'Column',
    'MyColumn1',
    'ABS(-15)'
);
-- QUERY END: SQLiteInsertDNDBTSysInfoQuery

-- QUERY START: SQLiteInsertDNDBTSysInfoQuery
INSERT INTO DNDBTDbObjects
(
    ID,
    ParentID,
    Type,
    Name,
    Code
)
VALUES
(
    '4dde852d-ec19-4b61-80f9-da428d8ff41a',
    '6ca51f29-c1bc-4349-b9c1-6f1ea170f162',
    'Column',
    'MyColumn3',
    NULL
);
-- QUERY END: SQLiteInsertDNDBTSysInfoQuery

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

-- QUERY START: SQLiteInsertDNDBTSysInfoQuery
INSERT INTO DNDBTDbObjects
(
    ID,
    ParentID,
    Type,
    Name,
    Code
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
FROM MyTable1 t1
LEFT JOIN MyTable2 t2
    ON t2.MyColumn1 = t1.MyColumn1;'
);
-- QUERY END: SQLiteInsertDNDBTSysInfoQuery

COMMIT TRANSACTION;