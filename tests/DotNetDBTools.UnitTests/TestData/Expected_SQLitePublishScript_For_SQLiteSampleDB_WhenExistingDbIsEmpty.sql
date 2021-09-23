INSERT INTO DNDBTDbObjects
(
    ID,
    Type,
    Name,
    Metadata
)
VALUES
(
    '562ec55b-6c11-4dde-b445-f062b12ca4ac',
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
      "ID": "1db86894-78f0-4bc4-97cf-fc1aa5321e77",
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
      "ID": "28c62b20-40e5-463c-973d-a40f25353e63",
      "Name": "MyColumn2"
    }
  ],
  "PrimaryKey": {
    "Columns": [
      "MyColumn1"
    ],
    "ID": "ffc24537-7c46-4978-b699-ce11a5224a4a",
    "Name": "PK_MyTable2"
  },
  "UniqueConstraints": [],
  "ForeignKeys": [],
  "ID": "562ec55b-6c11-4dde-b445-f062b12ca4ac",
  "Name": "MyTable2"
}'
);

CREATE TABLE MyTable2
(
    MyColumn1 INTEGER UNIQUE,
    MyColumn2 BLOB UNIQUE
);


INSERT INTO DNDBTDbObjects
(
    ID,
    Type,
    Name,
    Metadata
)
VALUES
(
    'bffc40d7-6825-48f9-8442-9712c6993ef9',
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
      "ID": "0547ca0d-61ab-4f41-8218-dda0c0216bea",
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
      "ID": "60ff7a1f-b4b8-476f-9db2-56617858be35",
      "Name": "MyColumn2"
    }
  ],
  "PrimaryKey": {
    "Columns": [
      "MyColumn1"
    ],
    "ID": "f8bd93b0-26ee-4564-9185-1214290622d3",
    "Name": "PK_MyTable1"
  },
  "UniqueConstraints": [
    {
      "Columns": [
        "MyColumn2"
      ],
      "ID": "08845ce3-de5c-4b23-9dd0-91118e772a4d",
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
      "ID": "51c1cc5e-306b-447f-804e-0943de7b730d",
      "Name": "FK_MyTable1_MyColumn1_MyTable2_MyColumn1"
    }
  ],
  "ID": "bffc40d7-6825-48f9-8442-9712c6993ef9",
  "Name": "MyTable1"
}'
);

CREATE TABLE MyTable1
(
    MyColumn1 INTEGER UNIQUE,
    MyColumn2 TEXT UNIQUE,
    CONSTRAINT FK_MyTable1_MyColumn1_MyTable2_MyColumn1 FOREIGN KEY (MyColumn1) REFERENCES MyTable2(MyColumn1)
);