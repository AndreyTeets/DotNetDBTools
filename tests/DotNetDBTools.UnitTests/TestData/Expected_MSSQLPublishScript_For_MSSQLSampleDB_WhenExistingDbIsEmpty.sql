--QUERY START: CreateTypeQuery
CREATE TYPE MyUserDefinedType1 FROM VARCHAR(100);
--QUERY END: CreateTypeQuery

--QUERY START: InsertDNDBTSysInfoQuery
DECLARE @Metadata NVARCHAR(MAX) = '{
  "Nullable": true,
  "UnderlyingDataType": {
    "Name": "String",
    "IsUserDefined": false,
    "Length": 100,
    "IsUnicode": false,
    "IsFixedLength": false
  },
  "ID": "0cd1e71c-cc9c-440f-ac0b-81a1d6f7ddaa",
  "Name": "MyUserDefinedType1"
}';
INSERT INTO DNDBTDbObjects
(
    ID,
    Type,
    Name,
    Metadata
)
VALUES
(
    '0cd1e71c-cc9c-440f-ac0b-81a1d6f7ddaa',
    'UserDefinedType',
    'MyUserDefinedType1',
    @Metadata
);
--QUERY END: InsertDNDBTSysInfoQuery

--QUERY START: CreateTableQuery
CREATE TABLE MyTable2
(
    MyColumn1 INT NOT NULL CONSTRAINT DF_MyTable2_MyColumn1 DEFAULT 333,
    MyColumn2 VARBINARY(22) NULL CONSTRAINT DF_MyTable2_MyColumn2 DEFAULT 0x000102,
    MyColumn3 MyUserDefinedType1 NULL CONSTRAINT DF_MyTable2_MyColumn3 DEFAULT 'cc',
    CONSTRAINT PK_MyTable2 PRIMARY KEY (MyColumn1)
);
--QUERY END: CreateTableQuery

--QUERY START: InsertDNDBTSysInfoQuery
DECLARE @Metadata NVARCHAR(MAX) = '{
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
      "ID": "68e584a5-c69f-4b57-bb5f-4a899fcc1a74",
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
      "Nullable": true,
      "Identity": false,
      "Default": {
        "$type": "System.Byte[], System.Private.CoreLib",
        "$value": "AAEC"
      },
      "ID": "d9e8009e-514a-4756-9493-12cbb9070ca0",
      "Name": "MyColumn2"
    },
    {
      "DataType": {
        "Name": "MyUserDefinedType1",
        "IsUserDefined": true,
        "Length": 100,
        "IsUnicode": false,
        "IsFixedLength": false
      },
      "Nullable": true,
      "Identity": false,
      "Default": "cc",
      "ID": "8fb7f9e7-b460-478e-aa13-6017bca47b25",
      "Name": "MyColumn3"
    }
  ],
  "PrimaryKey": {
    "Columns": [
      "MyColumn1"
    ],
    "ID": "2938449c-3308-44e2-989d-0003ef0ecbc5",
    "Name": "PK_MyTable2"
  },
  "UniqueConstraints": [],
  "ForeignKeys": [],
  "ID": "5db28241-adab-4ade-87b8-75cc6cb86c60",
  "Name": "MyTable2"
}';
INSERT INTO DNDBTDbObjects
(
    ID,
    Type,
    Name,
    Metadata
)
VALUES
(
    '5db28241-adab-4ade-87b8-75cc6cb86c60',
    'Table',
    'MyTable2',
    @Metadata
);
--QUERY END: InsertDNDBTSysInfoQuery

--QUERY START: CreateTableQuery
CREATE TABLE MyTable1
(
    MyColumn1 INT NOT NULL CONSTRAINT DF_MyTable1_MyColumn1 DEFAULT ABS(-15),
    MyColumn2 VARCHAR(10) NOT NULL CONSTRAINT DF_MyTable1_MyColumn2 DEFAULT '33',
    CONSTRAINT PK_MyTable1 PRIMARY KEY (MyColumn1),
    CONSTRAINT UQ_MyTable1_MyColumn2 UNIQUE (MyColumn2)
);
--QUERY END: CreateTableQuery

--QUERY START: InsertDNDBTSysInfoQuery
DECLARE @Metadata NVARCHAR(MAX) = '{
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
      "Default": {
        "$type": "DotNetDBTools.Models.MSSQL.MSSQLDefaultValueAsFunction, DotNetDBTools.Models",
        "FunctionText": "ABS(-15)"
      },
      "ID": "cacc163c-6fdf-4030-ae11-33eba5086e9e",
      "Name": "MyColumn1"
    },
    {
      "DataType": {
        "Name": "String",
        "IsUserDefined": false,
        "Length": 10,
        "IsUnicode": false,
        "IsFixedLength": false
      },
      "Nullable": false,
      "Identity": false,
      "Default": "33",
      "ID": "a9408a3c-d58e-463d-84b7-b99c53c65460",
      "Name": "MyColumn2"
    }
  ],
  "PrimaryKey": {
    "Columns": [
      "MyColumn1"
    ],
    "ID": "1afd9763-bef9-489f-b0af-b2c79d0afd78",
    "Name": "PK_MyTable1"
  },
  "UniqueConstraints": [
    {
      "Columns": [
        "MyColumn2"
      ],
      "ID": "7c2e6997-aa1c-49a7-8d2a-dec4389ebc26",
      "Name": "UQ_MyTable1_MyColumn2"
    }
  ],
  "ForeignKeys": [
    {
      "ThisTableName": "MyTable1",
      "ThisColumnNames": [
        "MyColumn1"
      ],
      "ForeignTableName": "MyTable2",
      "ForeignColumnNames": [
        "MyColumn1"
      ],
      "OnUpdate": "NoAction",
      "OnDelete": "Cascade",
      "ID": "99fa848e-d911-46e7-b406-bbd554d1c969",
      "Name": "FK_MyTable1_MyColumn1_MyTable2_MyColumn1"
    }
  ],
  "ID": "de2d4a1e-954f-4d24-80cf-d3dc75f18862",
  "Name": "MyTable1"
}';
INSERT INTO DNDBTDbObjects
(
    ID,
    Type,
    Name,
    Metadata
)
VALUES
(
    'de2d4a1e-954f-4d24-80cf-d3dc75f18862',
    'Table',
    'MyTable1',
    @Metadata
);
--QUERY END: InsertDNDBTSysInfoQuery

--QUERY START: CreateForeignKeyQuery
ALTER TABLE MyTable1 ADD CONSTRAINT FK_MyTable1_MyColumn1_MyTable2_MyColumn1 FOREIGN KEY (MyColumn1)
    REFERENCES MyTable2 (MyColumn1)
    ON UPDATE NO ACTION ON DELETE CASCADE;
--QUERY END: CreateForeignKeyQuery