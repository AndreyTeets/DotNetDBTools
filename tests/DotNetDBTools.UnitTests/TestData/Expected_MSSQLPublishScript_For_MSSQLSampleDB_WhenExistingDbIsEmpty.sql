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
    MyColumn1 INT NOT NULL DEFAULT 333,
    MyColumn2 MyUserDefinedType1 NULL DEFAULT 'cc',
    CONSTRAINT UQ_MyTable2_MyColumn1 UNIQUE (MyColumn1)
);
--QUERY END: CreateTableQuery

--QUERY START: InsertDNDBTSysInfoQuery
DECLARE @Metadata NVARCHAR(MAX) = '{
  "Columns": [
    {
      "ID": "68e584a5-c69f-4b57-bb5f-4a899fcc1a74",
      "Name": "MyColumn1",
      "DataType": {
        "Name": "Int",
        "IsUserDefined": false,
        "Length": 0,
        "IsUnicode": false,
        "IsFixedLength": false
      },
      "Nullable": false,
      "Unique": true,
      "Identity": false,
      "Default": 333
    },
    {
      "ID": "8fb7f9e7-b460-478e-aa13-6017bca47b25",
      "Name": "MyColumn2",
      "DataType": {
        "Name": "MyUserDefinedType1",
        "IsUserDefined": true,
        "Length": 100,
        "IsUnicode": false,
        "IsFixedLength": false
      },
      "Nullable": true,
      "Unique": false,
      "Identity": false,
      "Default": "cc"
    }
  ],
  "ForeignKeys": {
    "$type": "System.Collections.Generic.List`1[[DotNetDBTools.Models.MSSQL.MSSQLForeignKeyInfo, DotNetDBTools.Models]], System.Private.CoreLib",
    "$values": []
  },
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
    MyColumn1 INT NOT NULL DEFAULT ABS(-15),
    MyColumn2 VARCHAR(10) NOT NULL DEFAULT '33',
    CONSTRAINT UQ_MyTable1_MyColumn1 UNIQUE (MyColumn1),
    CONSTRAINT FK_MyTable1_MyColumn1_MyTable2_MyColumn1 FOREIGN KEY (MyColumn1)
        REFERENCES MyTable2(MyColumn1)
        ON UPDATE NO ACTION ON DELETE CASCADE
);
--QUERY END: CreateTableQuery

--QUERY START: InsertDNDBTSysInfoQuery
DECLARE @Metadata NVARCHAR(MAX) = '{
  "Columns": [
    {
      "ID": "cacc163c-6fdf-4030-ae11-33eba5086e9e",
      "Name": "MyColumn1",
      "DataType": {
        "Name": "Int",
        "IsUserDefined": false,
        "Length": 0,
        "IsUnicode": false,
        "IsFixedLength": false
      },
      "Nullable": false,
      "Unique": true,
      "Identity": false,
      "Default": {
        "$type": "DotNetDBTools.Models.MSSQL.MSSQLDefaultValueAsFunction, DotNetDBTools.Models",
        "FunctionText": "ABS(-15)"
      }
    },
    {
      "ID": "a9408a3c-d58e-463d-84b7-b99c53c65460",
      "Name": "MyColumn2",
      "DataType": {
        "Name": "String",
        "IsUserDefined": false,
        "Length": 10,
        "IsUnicode": false,
        "IsFixedLength": false
      },
      "Nullable": false,
      "Unique": false,
      "Identity": false,
      "Default": "33"
    }
  ],
  "ForeignKeys": {
    "$type": "System.Collections.Generic.List`1[[DotNetDBTools.Models.MSSQL.MSSQLForeignKeyInfo, DotNetDBTools.Models]], System.Private.CoreLib",
    "$values": [
      {
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
    ]
  },
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