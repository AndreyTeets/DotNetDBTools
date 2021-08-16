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
  "Columns": {
    "$type": "System.Collections.Generic.List`1[[DotNetDBTools.Models.SQLite.SQLiteColumnInfo, DotNetDBTools.Models.SQLite]], System.Private.CoreLib",
    "$values": [
      {
        "DataType": "IntDbType",
        "DefaultValue": 15,
        "ID": "0547ca0d-61ab-4f41-8218-dda0c0216bea",
        "Name": "MyColumn1"
      },
      {
        "DataType": "StringDbType",
        "DefaultValue": "33",
        "ID": "60ff7a1f-b4b8-476f-9db2-56617858be35",
        "Name": "MyColumn2"
      }
    ]
  },
  "ForeignKeys": {
    "$type": "System.Collections.Generic.List`1[[DotNetDBTools.Models.SQLite.SQLiteForeignKeyInfo, DotNetDBTools.Models.SQLite]], System.Private.CoreLib",
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
    MyColumn1 IntDbType,
    MyColumn2 StringDbType,
    CONSTRAINT FK_MyTable1_MyColumn1_MyTable2_MyColumn1 FOREIGN KEY (MyColumn1) REFERENCES MyTable2(MyColumn1)
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
    '562ec55b-6c11-4dde-b445-f062b12ca4ac',
    'Table',
    'MyTable2',
    '{
  "Columns": {
    "$type": "System.Collections.Generic.List`1[[DotNetDBTools.Models.SQLite.SQLiteColumnInfo, DotNetDBTools.Models.SQLite]], System.Private.CoreLib",
    "$values": [
      {
        "DataType": "IntDbType",
        "DefaultValue": 333,
        "ID": "1db86894-78f0-4bc4-97cf-fc1aa5321e77",
        "Name": "MyColumn1"
      },
      {
        "DataType": "ByteDbType",
        "DefaultValue": {
          "$type": "System.Byte[], System.Private.CoreLib",
          "$value": "AAE="
        },
        "ID": "28c62b20-40e5-463c-973d-a40f25353e63",
        "Name": "MyColumn2"
      }
    ]
  },
  "ForeignKeys": {
    "$type": "System.Collections.Generic.List`1[[DotNetDBTools.Models.SQLite.SQLiteForeignKeyInfo, DotNetDBTools.Models.SQLite]], System.Private.CoreLib",
    "$values": []
  },
  "ID": "562ec55b-6c11-4dde-b445-f062b12ca4ac",
  "Name": "MyTable2"
}'
);

CREATE TABLE MyTable2
(
    MyColumn1 IntDbType,
    MyColumn2 ByteDbType
);
