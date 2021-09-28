PRAGMA foreign_keys=off;
BEGIN TRANSACTION;

--QUERY START: CreateTableQuery
CREATE TABLE MyTable1
(
    MyColumn1 INTEGER NOT NULL CONSTRAINT DF_MyTable1_MyColumn1 DEFAULT 15,
    MyColumn2 TEXT NOT NULL CONSTRAINT DF_MyTable1_MyColumn2 DEFAULT '33',
    MyColumn3 INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
    CONSTRAINT UQ_MyTable1_MyColumn2 UNIQUE (MyColumn2),
    CONSTRAINT FK_MyTable1_MyColumn1_MyTable2_MyColumn1 FOREIGN KEY (MyColumn1)
        REFERENCES MyTable2(MyColumn1)
        ON UPDATE NO ACTION ON DELETE CASCADE
);
--QUERY END: CreateTableQuery

--QUERY START: InsertDNDBTSysInfoQuery
INSERT INTO DNDBTDbObjects
(
    ID,
    ParentID,
    Type,
    Name
)
VALUES
(
    '299675e6-4faa-4d0f-a36a-224306ba5bcb',
    NULL,
    'Table',
    'MyTable1'
);
--QUERY END: InsertDNDBTSysInfoQuery

--QUERY START: InsertDNDBTSysInfoQuery
INSERT INTO DNDBTDbObjects
(
    ID,
    ParentID,
    Type,
    Name
)
VALUES
(
    'a2f2a4de-1337-4594-ae41-72ed4d05f317',
    '299675e6-4faa-4d0f-a36a-224306ba5bcb',
    'Column',
    'MyColumn1'
);
--QUERY END: InsertDNDBTSysInfoQuery

--QUERY START: InsertDNDBTSysInfoQuery
INSERT INTO DNDBTDbObjects
(
    ID,
    ParentID,
    Type,
    Name
)
VALUES
(
    'fe68ee3d-09d0-40ac-93f9-5e441fbb4f70',
    '299675e6-4faa-4d0f-a36a-224306ba5bcb',
    'Column',
    'MyColumn2'
);
--QUERY END: InsertDNDBTSysInfoQuery

--QUERY START: InsertDNDBTSysInfoQuery
INSERT INTO DNDBTDbObjects
(
    ID,
    ParentID,
    Type,
    Name
)
VALUES
(
    '6e95de30-e01a-4fb4-b8b7-8f0c40bb682c',
    '299675e6-4faa-4d0f-a36a-224306ba5bcb',
    'Column',
    'MyColumn3'
);
--QUERY END: InsertDNDBTSysInfoQuery

--QUERY START: InsertDNDBTSysInfoQuery
INSERT INTO DNDBTDbObjects
(
    ID,
    ParentID,
    Type,
    Name
)
VALUES
(
    '37a45def-f4a0-4be7-8bfb-8fbed4a7d705',
    '299675e6-4faa-4d0f-a36a-224306ba5bcb',
    'PrimaryKey',
    'PK_MyTable1'
);
--QUERY END: InsertDNDBTSysInfoQuery

--QUERY START: InsertDNDBTSysInfoQuery
INSERT INTO DNDBTDbObjects
(
    ID,
    ParentID,
    Type,
    Name
)
VALUES
(
    'f3f08522-26ee-4950-9135-22edf2e4e0cf',
    '299675e6-4faa-4d0f-a36a-224306ba5bcb',
    'UniqueConstraint',
    'UQ_MyTable1_MyColumn2'
);
--QUERY END: InsertDNDBTSysInfoQuery

--QUERY START: InsertDNDBTSysInfoQuery
INSERT INTO DNDBTDbObjects
(
    ID,
    ParentID,
    Type,
    Name
)
VALUES
(
    'd11b2a53-32db-432f-bb6b-f91788844ba9',
    '299675e6-4faa-4d0f-a36a-224306ba5bcb',
    'ForeignKey',
    'FK_MyTable1_MyColumn1_MyTable2_MyColumn1'
);
--QUERY END: InsertDNDBTSysInfoQuery

--QUERY START: CreateTableQuery
CREATE TABLE MyTable2
(
    MyColumn1 INTEGER PRIMARY KEY NOT NULL CONSTRAINT DF_MyTable2_MyColumn1 DEFAULT 333,
    MyColumn2 BLOB NULL CONSTRAINT DF_MyTable2_MyColumn2 DEFAULT 0x000102
);
--QUERY END: CreateTableQuery

--QUERY START: InsertDNDBTSysInfoQuery
INSERT INTO DNDBTDbObjects
(
    ID,
    ParentID,
    Type,
    Name
)
VALUES
(
    'bfb9030c-a8c3-4882-9c42-1c6ad025cf8f',
    NULL,
    'Table',
    'MyTable2'
);
--QUERY END: InsertDNDBTSysInfoQuery

--QUERY START: InsertDNDBTSysInfoQuery
INSERT INTO DNDBTDbObjects
(
    ID,
    ParentID,
    Type,
    Name
)
VALUES
(
    'c480f22f-7c01-4f41-b282-35e9f5cd1fe3',
    'bfb9030c-a8c3-4882-9c42-1c6ad025cf8f',
    'Column',
    'MyColumn1'
);
--QUERY END: InsertDNDBTSysInfoQuery

--QUERY START: InsertDNDBTSysInfoQuery
INSERT INTO DNDBTDbObjects
(
    ID,
    ParentID,
    Type,
    Name
)
VALUES
(
    '5a0d1926-3270-4eb2-92eb-00be56c7af23',
    'bfb9030c-a8c3-4882-9c42-1c6ad025cf8f',
    'Column',
    'MyColumn2'
);
--QUERY END: InsertDNDBTSysInfoQuery

--QUERY START: InsertDNDBTSysInfoQuery
INSERT INTO DNDBTDbObjects
(
    ID,
    ParentID,
    Type,
    Name
)
VALUES
(
    '3a43615b-40b3-4a13-99e7-93af7c56e8ce',
    'bfb9030c-a8c3-4882-9c42-1c6ad025cf8f',
    'PrimaryKey',
    'PK_MyTable2'
);
--QUERY END: InsertDNDBTSysInfoQuery

COMMIT TRANSACTION;