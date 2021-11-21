PRAGMA foreign_keys=off;
BEGIN TRANSACTION;

-- QUERY START: CreateTableQuery
CREATE TABLE MyTable1
(
    MyColumn1 INTEGER NOT NULL DEFAULT 15,
    MyColumn2 TEXT NOT NULL DEFAULT '33',
    MyColumn3 INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
    CONSTRAINT UQ_MyTable1_MyColumn2 UNIQUE (MyColumn2),
    CONSTRAINT FK_MyTable1_MyColumn1_MyTable2_MyColumn1 FOREIGN KEY (MyColumn1)
        REFERENCES MyTable2(MyColumn1)
        ON UPDATE NO ACTION ON DELETE CASCADE
);
-- QUERY END: CreateTableQuery

-- QUERY START: InsertDNDBTSysInfoQuery
INSERT INTO DNDBTDbObjects
(
    ID,
    ParentID,
    Type,
    Name,
    ExtraInfo
)
VALUES
(
    '299675e6-4faa-4d0f-a36a-224306ba5bcb',
    NULL,
    'Table',
    'MyTable1',
    NULL
);
-- QUERY END: InsertDNDBTSysInfoQuery

-- QUERY START: InsertDNDBTSysInfoQuery
INSERT INTO DNDBTDbObjects
(
    ID,
    ParentID,
    Type,
    Name,
    ExtraInfo
)
VALUES
(
    'a2f2a4de-1337-4594-ae41-72ed4d05f317',
    '299675e6-4faa-4d0f-a36a-224306ba5bcb',
    'Column',
    'MyColumn1',
    NULL
);
-- QUERY END: InsertDNDBTSysInfoQuery

-- QUERY START: InsertDNDBTSysInfoQuery
INSERT INTO DNDBTDbObjects
(
    ID,
    ParentID,
    Type,
    Name,
    ExtraInfo
)
VALUES
(
    'fe68ee3d-09d0-40ac-93f9-5e441fbb4f70',
    '299675e6-4faa-4d0f-a36a-224306ba5bcb',
    'Column',
    'MyColumn2',
    NULL
);
-- QUERY END: InsertDNDBTSysInfoQuery

-- QUERY START: InsertDNDBTSysInfoQuery
INSERT INTO DNDBTDbObjects
(
    ID,
    ParentID,
    Type,
    Name,
    ExtraInfo
)
VALUES
(
    '6e95de30-e01a-4fb4-b8b7-8f0c40bb682c',
    '299675e6-4faa-4d0f-a36a-224306ba5bcb',
    'Column',
    'MyColumn3',
    NULL
);
-- QUERY END: InsertDNDBTSysInfoQuery

-- QUERY START: InsertDNDBTSysInfoQuery
INSERT INTO DNDBTDbObjects
(
    ID,
    ParentID,
    Type,
    Name,
    ExtraInfo
)
VALUES
(
    '37a45def-f4a0-4be7-8bfb-8fbed4a7d705',
    '299675e6-4faa-4d0f-a36a-224306ba5bcb',
    'PrimaryKey',
    'PK_MyTable1',
    NULL
);
-- QUERY END: InsertDNDBTSysInfoQuery

-- QUERY START: InsertDNDBTSysInfoQuery
INSERT INTO DNDBTDbObjects
(
    ID,
    ParentID,
    Type,
    Name,
    ExtraInfo
)
VALUES
(
    'f3f08522-26ee-4950-9135-22edf2e4e0cf',
    '299675e6-4faa-4d0f-a36a-224306ba5bcb',
    'UniqueConstraint',
    'UQ_MyTable1_MyColumn2',
    NULL
);
-- QUERY END: InsertDNDBTSysInfoQuery

-- QUERY START: InsertDNDBTSysInfoQuery
INSERT INTO DNDBTDbObjects
(
    ID,
    ParentID,
    Type,
    Name,
    ExtraInfo
)
VALUES
(
    'd11b2a53-32db-432f-bb6b-f91788844ba9',
    '299675e6-4faa-4d0f-a36a-224306ba5bcb',
    'ForeignKey',
    'FK_MyTable1_MyColumn1_MyTable2_MyColumn1',
    NULL
);
-- QUERY END: InsertDNDBTSysInfoQuery

-- QUERY START: CreateTableQuery
CREATE TABLE MyTable2
(
    MyColumn1 INTEGER PRIMARY KEY NOT NULL DEFAULT 333,
    MyColumn2 BLOB NULL DEFAULT 0x000102
);
-- QUERY END: CreateTableQuery

-- QUERY START: InsertDNDBTSysInfoQuery
INSERT INTO DNDBTDbObjects
(
    ID,
    ParentID,
    Type,
    Name,
    ExtraInfo
)
VALUES
(
    'bfb9030c-a8c3-4882-9c42-1c6ad025cf8f',
    NULL,
    'Table',
    'MyTable2',
    NULL
);
-- QUERY END: InsertDNDBTSysInfoQuery

-- QUERY START: InsertDNDBTSysInfoQuery
INSERT INTO DNDBTDbObjects
(
    ID,
    ParentID,
    Type,
    Name,
    ExtraInfo
)
VALUES
(
    'c480f22f-7c01-4f41-b282-35e9f5cd1fe3',
    'bfb9030c-a8c3-4882-9c42-1c6ad025cf8f',
    'Column',
    'MyColumn1',
    NULL
);
-- QUERY END: InsertDNDBTSysInfoQuery

-- QUERY START: InsertDNDBTSysInfoQuery
INSERT INTO DNDBTDbObjects
(
    ID,
    ParentID,
    Type,
    Name,
    ExtraInfo
)
VALUES
(
    '5a0d1926-3270-4eb2-92eb-00be56c7af23',
    'bfb9030c-a8c3-4882-9c42-1c6ad025cf8f',
    'Column',
    'MyColumn2',
    NULL
);
-- QUERY END: InsertDNDBTSysInfoQuery

-- QUERY START: InsertDNDBTSysInfoQuery
INSERT INTO DNDBTDbObjects
(
    ID,
    ParentID,
    Type,
    Name,
    ExtraInfo
)
VALUES
(
    '3a43615b-40b3-4a13-99e7-93af7c56e8ce',
    'bfb9030c-a8c3-4882-9c42-1c6ad025cf8f',
    'PrimaryKey',
    'PK_MyTable2',
    NULL
);
-- QUERY END: InsertDNDBTSysInfoQuery

-- QUERY START: CreateTableQuery
CREATE TABLE MyTable5
(
    MyColumn1 INTEGER NOT NULL DEFAULT (ABS(-15)),
    MyColumn3 NUMERIC NULL
);
-- QUERY END: CreateTableQuery

-- QUERY START: InsertDNDBTSysInfoQuery
INSERT INTO DNDBTDbObjects
(
    ID,
    ParentID,
    Type,
    Name,
    ExtraInfo
)
VALUES
(
    '6ca51f29-c1bc-4349-b9c1-6f1ea170f162',
    NULL,
    'Table',
    'MyTable5',
    NULL
);
-- QUERY END: InsertDNDBTSysInfoQuery

-- QUERY START: InsertDNDBTSysInfoQuery
INSERT INTO DNDBTDbObjects
(
    ID,
    ParentID,
    Type,
    Name,
    ExtraInfo
)
VALUES
(
    '5309d66f-2030-402e-912e-5547babaa072',
    '6ca51f29-c1bc-4349-b9c1-6f1ea170f162',
    'Column',
    'MyColumn1',
    'ABS(-15)'
);
-- QUERY END: InsertDNDBTSysInfoQuery

-- QUERY START: InsertDNDBTSysInfoQuery
INSERT INTO DNDBTDbObjects
(
    ID,
    ParentID,
    Type,
    Name,
    ExtraInfo
)
VALUES
(
    '4dde852d-ec19-4b61-80f9-da428d8ff41a',
    '6ca51f29-c1bc-4349-b9c1-6f1ea170f162',
    'Column',
    'MyColumn3',
    NULL
);
-- QUERY END: InsertDNDBTSysInfoQuery

COMMIT TRANSACTION;