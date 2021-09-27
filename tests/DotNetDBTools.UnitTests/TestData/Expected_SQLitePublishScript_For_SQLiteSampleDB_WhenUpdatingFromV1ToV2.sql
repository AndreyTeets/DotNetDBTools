PRAGMA foreign_keys=off;
BEGIN TRANSACTION;

--QUERY START: AlterTableQuery
CREATE TABLE _DNDBTTemp_MyTable1NewName
(
    MyColumn1 INTEGER NULL CONSTRAINT DF_MyTable1NewName_MyColumn1 DEFAULT 15,
    CONSTRAINT FK_MyTable1_MyColumn1_MyTable2_MyColumn1 FOREIGN KEY (MyColumn1)
        REFERENCES MyTable2(MyColumn1NewName)
        ON UPDATE NO ACTION ON DELETE SET NULL
);

INSERT INTO  _DNDBTTemp_MyTable1NewName(MyColumn1)
SELECT MyColumn1
FROM MyTable1;

DROP TABLE MyTable1;

ALTER TABLE _DNDBTTemp_MyTable1NewName RENAME TO MyTable1NewName;
--QUERY END: AlterTableQuery

--QUERY START: DeleteDNDBTSysInfoQuery
DELETE FROM DNDBTDbObjects
WHERE ID = 'd11b2a53-32db-432f-bb6b-f91788844ba9';
--QUERY END: DeleteDNDBTSysInfoQuery

--QUERY START: DeleteDNDBTSysInfoQuery
DELETE FROM DNDBTDbObjects
WHERE ID = 'f3f08522-26ee-4950-9135-22edf2e4e0cf';
--QUERY END: DeleteDNDBTSysInfoQuery

--QUERY START: DeleteDNDBTSysInfoQuery
DELETE FROM DNDBTDbObjects
WHERE ID = '37a45def-f4a0-4be7-8bfb-8fbed4a7d705';
--QUERY END: DeleteDNDBTSysInfoQuery

--QUERY START: DeleteDNDBTSysInfoQuery
DELETE FROM DNDBTDbObjects
WHERE ID = 'fe68ee3d-09d0-40ac-93f9-5e441fbb4f70';
--QUERY END: DeleteDNDBTSysInfoQuery

--QUERY START: DeleteDNDBTSysInfoQuery
DELETE FROM DNDBTDbObjects
WHERE ID = '6e95de30-e01a-4fb4-b8b7-8f0c40bb682c';
--QUERY END: DeleteDNDBTSysInfoQuery

--QUERY START: UpdateDNDBTSysInfoQuery
UPDATE DNDBTDbObjects SET
    Name = 'MyTable1NewName'
WHERE ID = '299675e6-4faa-4d0f-a36a-224306ba5bcb';
--QUERY END: UpdateDNDBTSysInfoQuery

--QUERY START: UpdateDNDBTSysInfoQuery
UPDATE DNDBTDbObjects SET
    Name = 'MyColumn1'
WHERE ID = 'a2f2a4de-1337-4594-ae41-72ed4d05f317';
--QUERY END: UpdateDNDBTSysInfoQuery

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

--QUERY START: AlterTableQuery
CREATE TABLE _DNDBTTemp_MyTable2
(
    MyColumn1NewName INTEGER PRIMARY KEY NOT NULL CONSTRAINT DF_MyTable2_MyColumn1NewName DEFAULT 333,
    MyColumn2 BLOB NULL CONSTRAINT DF_MyTable2_MyColumn2 DEFAULT 0x000102,
    CONSTRAINT FK_MyTable2_MyColumns12_MyTable3_MyColumns12 FOREIGN KEY (MyColumn1NewName, MyColumn2)
        REFERENCES MyTable3(MyColumn1, MyColumn2)
        ON UPDATE NO ACTION ON DELETE SET DEFAULT
);

INSERT INTO  _DNDBTTemp_MyTable2(MyColumn1NewName)
SELECT MyColumn1
FROM MyTable2;

DROP TABLE MyTable2;

ALTER TABLE _DNDBTTemp_MyTable2 RENAME TO MyTable2;
--QUERY END: AlterTableQuery

--QUERY START: DeleteDNDBTSysInfoQuery
DELETE FROM DNDBTDbObjects
WHERE ID = '3a43615b-40b3-4a13-99e7-93af7c56e8ce';
--QUERY END: DeleteDNDBTSysInfoQuery

--QUERY START: DeleteDNDBTSysInfoQuery
DELETE FROM DNDBTDbObjects
WHERE ID = '5a0d1926-3270-4eb2-92eb-00be56c7af23';
--QUERY END: DeleteDNDBTSysInfoQuery

--QUERY START: UpdateDNDBTSysInfoQuery
UPDATE DNDBTDbObjects SET
    Name = 'MyTable2'
WHERE ID = 'bfb9030c-a8c3-4882-9c42-1c6ad025cf8f';
--QUERY END: UpdateDNDBTSysInfoQuery

--QUERY START: UpdateDNDBTSysInfoQuery
UPDATE DNDBTDbObjects SET
    Name = 'MyColumn1NewName'
WHERE ID = 'c480f22f-7c01-4f41-b282-35e9f5cd1fe3';
--QUERY END: UpdateDNDBTSysInfoQuery

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
    'c2df19c2-e029-4014-8a5b-4ab42fecb6b8',
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
    '480f3508-9d51-4190-88aa-45bc20e49119',
    'bfb9030c-a8c3-4882-9c42-1c6ad025cf8f',
    'ForeignKey',
    'FK_MyTable2_MyColumns12_MyTable3_MyColumns12'
);
--QUERY END: InsertDNDBTSysInfoQuery

--QUERY START: CreateTableQuery
CREATE TABLE MyTable3
(
    MyColumn1 INTEGER NOT NULL CONSTRAINT DF_MyTable3_MyColumn1 DEFAULT 333,
    MyColumn2 BLOB NOT NULL,
    CONSTRAINT UQ_MyTable3_MyColumns12 UNIQUE (MyColumn1, MyColumn2)
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
    '474cd761-2522-4529-9d20-2b94115f9626',
    NULL,
    'Table',
    'MyTable3'
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
    '726f503a-d944-46ee-a0ff-6a2c2faab46e',
    '474cd761-2522-4529-9d20-2b94115f9626',
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
    '169824e1-8b74-4b60-af17-99656d6dbbee',
    '474cd761-2522-4529-9d20-2b94115f9626',
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
    'fd288e38-35ba-4bb1-ace3-597c99ef26c7',
    '474cd761-2522-4529-9d20-2b94115f9626',
    'UniqueConstraint',
    'UQ_MyTable3_MyColumns12'
);
--QUERY END: InsertDNDBTSysInfoQuery

COMMIT TRANSACTION;