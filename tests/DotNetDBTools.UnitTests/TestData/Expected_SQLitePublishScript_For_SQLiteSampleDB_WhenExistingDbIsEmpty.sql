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
      "ID": "1db86894-78f0-4bc4-97cf-fc1aa5321e77",
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
      "ID": "28c62b20-40e5-463c-973d-a40f25353e63",
      "Name": "MyColumn2",
      "DataType": {
        "Name": "Byte",
        "IsUserDefined": false,
        "Length": 50,
        "IsUnicode": false,
        "IsFixedLength": false
      },
      "Nullable": true,
      "Unique": false,
      "Identity": false,
      "Default": {
        "$type": "System.Byte[], System.Private.CoreLib",
        "$value": "AAE="
      }
    }
  ],
  "ForeignKeys": {
    "$type": "System.Collections.Generic.List`1[[DotNetDBTools.Models.SQLite.SQLiteForeignKeyInfo, DotNetDBTools.Models]], System.Private.CoreLib",
    "$values": []
  },
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
      "ID": "0547ca0d-61ab-4f41-8218-dda0c0216bea",
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
      "Default": 15
    },
    {
      "ID": "60ff7a1f-b4b8-476f-9db2-56617858be35",
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
    "$type": "System.Collections.Generic.List`1[[DotNetDBTools.Models.SQLite.SQLiteForeignKeyInfo, DotNetDBTools.Models]], System.Private.CoreLib",
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
        "ID": "51c1cc5e-306b-447f-804e-0943de7b730d",
        "Name": "FK_MyTable1_MyColumn1_MyTable2_MyColumn1"
      }
    ]
  },
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