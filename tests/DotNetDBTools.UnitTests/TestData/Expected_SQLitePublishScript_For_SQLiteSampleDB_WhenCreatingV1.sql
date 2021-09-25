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
    Type,
    Name,
    Metadata
)
VALUES
(
    'bfb9030c-a8c3-4882-9c42-1c6ad025cf8f',
    'Table',
    'MyTable2',
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
      "ID": "c480f22f-7c01-4f41-b282-35e9f5cd1fe3",
      "Name": "MyColumn1"
    },
    {
      "DataType": {
        "Name": "Byte",
        "IsUserDefined": false,
        "Length": 0,
        "IsUnicode": false,
        "IsFixedLength": false
      },
      "Nullable": true,
      "Identity": false,
      "Default": {
        "$type": "System.Byte[], System.Private.CoreLib",
        "$value": "AAEC"
      },
      "ID": "5a0d1926-3270-4eb2-92eb-00be56c7af23",
      "Name": "MyColumn2"
    }
  ],
  "PrimaryKey": {
    "Columns": [
      "MyColumn1"
    ],
    "ID": "3a43615b-40b3-4a13-99e7-93af7c56e8ce",
    "Name": "PK_MyTable2"
  },
  "UniqueConstraints": [],
  "ForeignKeys": [],
  "ID": "bfb9030c-a8c3-4882-9c42-1c6ad025cf8f",
  "Name": "MyTable2"
}'
);
--QUERY END: InsertDNDBTSysInfoQuery

--QUERY START: CreateTableQuery
CREATE TABLE MyTable1
(
    MyColumn1 INTEGER NOT NULL CONSTRAINT DF_MyTable1_MyColumn1 DEFAULT 15,
    MyColumn2 TEXT NOT NULL CONSTRAINT DF_MyTable1_MyColumn2 DEFAULT '33',
    MyColumn3 INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL ,
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
    Type,
    Name,
    Metadata
)
VALUES
(
    '299675e6-4faa-4d0f-a36a-224306ba5bcb',
    'Table',
    'MyTable1',
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
      "Default": 15,
      "ID": "a2f2a4de-1337-4594-ae41-72ed4d05f317",
      "Name": "MyColumn1"
    },
    {
      "DataType": {
        "Name": "String",
        "IsUserDefined": false,
        "Length": 0,
        "IsUnicode": false,
        "IsFixedLength": false
      },
      "Nullable": false,
      "Identity": false,
      "Default": "33",
      "ID": "fe68ee3d-09d0-40ac-93f9-5e441fbb4f70",
      "Name": "MyColumn2"
    },
    {
      "DataType": {
        "Name": "Int",
        "IsUserDefined": false,
        "Length": 0,
        "IsUnicode": false,
        "IsFixedLength": false
      },
      "Nullable": false,
      "Identity": true,
      "Default": null,
      "ID": "6e95de30-e01a-4fb4-b8b7-8f0c40bb682c",
      "Name": "MyColumn3"
    }
  ],
  "PrimaryKey": {
    "Columns": [
      "MyColumn3"
    ],
    "ID": "37a45def-f4a0-4be7-8bfb-8fbed4a7d705",
    "Name": "PK_MyTable1"
  },
  "UniqueConstraints": [
    {
      "Columns": [
        "MyColumn2"
      ],
      "ID": "f3f08522-26ee-4950-9135-22edf2e4e0cf",
      "Name": "UQ_MyTable1_MyColumn2"
    }
  ],
  "ForeignKeys": [
    {
      "ThisTableName": "MyTable1",
      "ThisColumnNames": [
        "MyColumn1"
      ],
      "ReferencedTableName": "MyTable2",
      "ReferencedTableColumnNames": [
        "MyColumn1"
      ],
      "OnUpdate": "NoAction",
      "OnDelete": "Cascade",
      "ID": "d11b2a53-32db-432f-bb6b-f91788844ba9",
      "Name": "FK_MyTable1_MyColumn1_MyTable2_MyColumn1"
    }
  ],
  "ID": "299675e6-4faa-4d0f-a36a-224306ba5bcb",
  "Name": "MyTable1"
}'
);
--QUERY END: InsertDNDBTSysInfoQuery