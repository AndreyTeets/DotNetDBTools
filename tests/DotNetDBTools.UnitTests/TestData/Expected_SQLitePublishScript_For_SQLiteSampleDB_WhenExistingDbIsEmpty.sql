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
      "DataTypeName": "Int",
      "DefaultValue": 333,
      "Length": null,
      "IsUnicode": null,
      "IsFixedLength": null
    },
    {
      "ID": "28c62b20-40e5-463c-973d-a40f25353e63",
      "Name": "MyColumn2",
      "DataTypeName": "Byte",
      "DefaultValue": {
        "$type": "System.Byte[], System.Private.CoreLib",
        "$value": "AAE="
      },
      "Length": "50",
      "IsUnicode": null,
      "IsFixedLength": null
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
    '299675e6-4faa-4d0f-a36a-224306ba5bcb',
    'Table',
    'MyTable1',
    '{
  "Columns": [
    {
      "ID": "0547ca0d-61ab-4f41-8218-dda0c0216bea",
      "Name": "MyColumn1",
      "DataTypeName": "Int",
      "DefaultValue": 15,
      "Length": null,
      "IsUnicode": null,
      "IsFixedLength": null
    },
    {
      "ID": "60ff7a1f-b4b8-476f-9db2-56617858be35",
      "Name": "MyColumn2",
      "DataTypeName": "String",
      "DefaultValue": "33",
      "Length": "10",
      "IsUnicode": "False",
      "IsFixedLength": null
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
        "ID": "00000000-0000-0000-0000-000000000000",
        "Name": "FK_MyTable1_MyColumn1_MyTable2_MyColumn1"
      }
    ]
  },
  "ID": "299675e6-4faa-4d0f-a36a-224306ba5bcb",
  "Name": "MyTable1"
}'
);

CREATE TABLE MyTable1
(
    MyColumn1 INTEGER UNIQUE,
    MyColumn2 TEXT UNIQUE,
    CONSTRAINT FK_MyTable1_MyColumn1_MyTable2_MyColumn1 FOREIGN KEY (MyColumn1) REFERENCES MyTable2(MyColumn1)
);