PRAGMA foreign_keys=off;
BEGIN TRANSACTION;

CREATE TABLE DNDBT_MyTable2
(
    MyColumn1NewName INTEGER UNIQUE,
    MyColumn2 BLOB UNIQUE,
    CONSTRAINT FK_MyTable2_MyColumn12_MyTable3_MyColumn12 FOREIGN KEY (MyColumn1NewName,MyColumn2) REFERENCES MyTable3(MyColumn1,MyColumn2)
);

INSERT INTO  DNDBT_MyTable2(MyColumn1NewName)
SELECT MyColumn1
FROM MyTable2;

DROP TABLE MyTable2;

ALTER TABLE DNDBT_MyTable2 RENAME TO MyTable2;

COMMIT TRANSACTION;
PRAGMA foreign_keys=on;

UPDATE DNDBTDbObjects SET
    Name = 'MyTable2',
    Metadata = '{
  "Columns": [
    {
      "DataType": {
        "Name": "Int",
        "IsUserDefined": false,
        "Length": 0,
        "IsUnicode": false,
        "IsFixedLength": false
      },
      "Nullable": false,
      "Identity": false,
      "Default": 333,
      "ID": "c480f22f-7c01-4f41-b282-35e9f5cd1fe3",
      "Name": "MyColumn1NewName"
    },
    {
      "DataType": {
        "Name": "Byte",
        "IsUserDefined": false,
        "Length": 22,
        "IsUnicode": false,
        "IsFixedLength": false
      },
      "Nullable": true,
      "Identity": false,
      "Default": {
        "$type": "System.Byte[], System.Private.CoreLib",
        "$value": "AAEC"
      },
      "ID": "c2df19c2-e029-4014-8a5b-4ab42fecb6b8",
      "Name": "MyColumn2"
    }
  ],
  "PrimaryKey": {
    "Columns": [
      "MyColumn1NewName"
    ],
    "ID": "3a43615b-40b3-4a13-99e7-93af7c56e8ce",
    "Name": "PK_MyTable2"
  },
  "UniqueConstraints": [],
  "ForeignKeys": [
    {
      "ThisTableName": "MyTable2",
      "ThisColumnNames": [
        "MyColumn1NewName",
        "MyColumn2"
      ],
      "ForeignTableName": "MyTable3",
      "ForeignColumnNames": [
        "MyColumn1",
        "MyColumn2"
      ],
      "OnUpdate": "NoAction",
      "OnDelete": "SetDefault",
      "ID": "480f3508-9d51-4190-88aa-45bc20e49119",
      "Name": "FK_MyTable2_MyColumn12_MyTable3_MyColumn12"
    }
  ],
  "ID": "bfb9030c-a8c3-4882-9c42-1c6ad025cf8f",
  "Name": "MyTable2"
}'
WHERE ID = 'bfb9030c-a8c3-4882-9c42-1c6ad025cf8f';


PRAGMA foreign_keys=off;
BEGIN TRANSACTION;

CREATE TABLE DNDBT_MyTable1NewName
(
    MyColumn1 INTEGER UNIQUE,
    CONSTRAINT FK_MyTable1_MyColumn1_MyTable2_MyColumn1 FOREIGN KEY (MyColumn1) REFERENCES MyTable2(MyColumn1NewName)
);

INSERT INTO  DNDBT_MyTable1NewName(MyColumn1)
SELECT MyColumn1
FROM MyTable1;

DROP TABLE MyTable1;

ALTER TABLE DNDBT_MyTable1NewName RENAME TO MyTable1NewName;

COMMIT TRANSACTION;
PRAGMA foreign_keys=on;

UPDATE DNDBTDbObjects SET
    Name = 'MyTable1NewName',
    Metadata = '{
  "Columns": [
    {
      "DataType": {
        "Name": "Int",
        "IsUserDefined": false,
        "Length": 0,
        "IsUnicode": false,
        "IsFixedLength": false
      },
      "Nullable": true,
      "Identity": false,
      "Default": 15,
      "ID": "a2f2a4de-1337-4594-ae41-72ed4d05f317",
      "Name": "MyColumn1"
    }
  ],
  "PrimaryKey": null,
  "UniqueConstraints": [],
  "ForeignKeys": [
    {
      "ThisTableName": "MyTable1NewName",
      "ThisColumnNames": [
        "MyColumn1"
      ],
      "ForeignTableName": "MyTable2",
      "ForeignColumnNames": [
        "MyColumn1NewName"
      ],
      "OnUpdate": "NoAction",
      "OnDelete": "SetNull",
      "ID": "d11b2a53-32db-432f-bb6b-f91788844ba9",
      "Name": "FK_MyTable1_MyColumn1_MyTable2_MyColumn1"
    }
  ],
  "ID": "299675e6-4faa-4d0f-a36a-224306ba5bcb",
  "Name": "MyTable1NewName"
}'
WHERE ID = '299675e6-4faa-4d0f-a36a-224306ba5bcb';


INSERT INTO DNDBTDbObjects
(
    ID,
    Type,
    Name,
    Metadata
)
VALUES
(
    '474cd761-2522-4529-9d20-2b94115f9626',
    'Table',
    'MyTable3',
    '{
  "Columns": [
    {
      "DataType": {
        "Name": "Int",
        "IsUserDefined": false,
        "Length": 0,
        "IsUnicode": false,
        "IsFixedLength": false
      },
      "Nullable": false,
      "Identity": false,
      "Default": 333,
      "ID": "726f503a-d944-46ee-a0ff-6a2c2faab46e",
      "Name": "MyColumn1"
    },
    {
      "DataType": {
        "Name": "Byte",
        "IsUserDefined": false,
        "Length": 22,
        "IsUnicode": false,
        "IsFixedLength": false
      },
      "Nullable": false,
      "Identity": false,
      "Default": null,
      "ID": "169824e1-8b74-4b60-af17-99656d6dbbee",
      "Name": "MyColumn2"
    }
  ],
  "PrimaryKey": null,
  "UniqueConstraints": [
    {
      "Columns": [
        "MyColumn1",
        "MyColumn2"
      ],
      "ID": "fd288e38-35ba-4bb1-ace3-597c99ef26c7",
      "Name": "UQ_MyTable3_MyColumn2"
    }
  ],
  "ForeignKeys": [],
  "ID": "474cd761-2522-4529-9d20-2b94115f9626",
  "Name": "MyTable3"
}'
);

CREATE TABLE MyTable3
(
    MyColumn1 INTEGER UNIQUE,
    MyColumn2 BLOB UNIQUE
);