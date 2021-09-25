--QUERY START: DropForeignKeyQuery
ALTER TABLE MyTable1 DROP CONSTRAINT FK_MyTable1_MyColumn1_MyTable2_MyColumn1;
--QUERY END: DropForeignKeyQuery

--QUERY START: RenameUserDefinedDataTypeQuery
EXEC sp_rename 'MyUserDefinedType1', '_DNDBTTemp_MyUserDefinedType1', 'USERDATATYPE';
--QUERY END: RenameUserDefinedDataTypeQuery

--QUERY START: DeleteDNDBTSysInfoQuery
DELETE FROM DNDBTDbObjects
WHERE ID = '0cd1e71c-cc9c-440f-ac0b-81a1d6f7ddaa';
--QUERY END: DeleteDNDBTSysInfoQuery

--QUERY START: CreateTypeQuery
CREATE TYPE MyUserDefinedType1 FROM VARCHAR(110);
--QUERY END: CreateTypeQuery

--QUERY START: InsertDNDBTSysInfoQuery
DECLARE @Metadata NVARCHAR(MAX) = '{
  "Nullable": true,
  "UnderlyingDataType": {
    "Name": "String",
    "IsUserDefined": false,
    "Length": 110,
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

--QUERY START: UseNewUDTInAllTablesQuery
DECLARE @SqlText NVARCHAR(MAX) =
(
    SELECT STUFF ((
        SELECT
            CHAR(10) + CHAR(10) + t.AlterStatement
        FROM
        (
            SELECT
                cci.DropConstraintStatement + CHAR(10) +
                N'ALTER TABLE ' + QUOTENAME(cci.TABLE_NAME) + ' ALTER COLUMN ' + QUOTENAME(cci.COLUMN_NAME) + ' ' + '[MyUserDefinedType1]' + ';' + CHAR(10) +
                cci.AddConstraintStatement AS AlterStatement
            FROM
            (
                SELECT
                    c.TABLE_NAME,
                    c.COLUMN_NAME,
                    ccu.CONSTRAINT_NAME,
                    tc.CONSTRAINT_TYPE,
                    CASE WHEN tc.CONSTRAINT_TYPE IS NOT NULL THEN
                        N'ALTER TABLE ' + QUOTENAME(c.TABLE_NAME) + ' DROP CONSTRAINT ' + QUOTENAME(ccu.CONSTRAINT_NAME) + ';' + CHAR(10)
                    ELSE
                        ''
                    END AS DropConstraintStatement,
                    CASE WHEN tc.CONSTRAINT_TYPE IS NOT NULL THEN
                        N'ALTER TABLE ' + QUOTENAME(c.TABLE_NAME) + ' ADD CONSTRAINT ' + QUOTENAME(ccu.CONSTRAINT_NAME) + ' ' + tc.CONSTRAINT_TYPE + '(' +
                        (SELECT STUFF ((
                            SELECT
                                ', ' + t.COLUMN_NAME
                            FROM
                            (
                                SELECT
                                    ccu2.COLUMN_NAME
                                FROM INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE ccu2
                                WHERE ccu2.CONSTRAINT_NAME = ccu.CONSTRAINT_NAME
                            ) t FOR XML PATH('')), 1, 2, '')) +
                        ')' + ';'
                    ELSE
                        ''
                    END AS ADDConstraintStatement
                FROM INFORMATION_SCHEMA.COLUMNS c
                LEFT JOIN INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE ccu
                    ON ccu.COLUMN_NAME = c.COLUMN_NAME
                        AND ccu.TABLE_NAME = c.TABLE_NAME
                LEFT JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS tc
                    ON tc.CONSTRAINT_NAME = ccu.CONSTRAINT_NAME
                        AND tc.CONSTRAINT_TYPE IN ('UNIQUE', 'PRIMARY KEY')
                WHERE c.DOMAIN_NAME = '_DNDBTTemp_MyUserDefinedType1'
            ) cci
        ) t FOR XML PATH('')), 1, 2, '')
);
EXEC (@SqlText);
--QUERY END: UseNewUDTInAllTablesQuery

--QUERY START: AlterTableQuery
EXEC sp_rename 'MyTable2', 'MyTable2';

ALTER TABLE MyTable2 DROP CONSTRAINT PK_MyTable2;

DECLARE @DropDefaultConstraint_MyTable2_MyColumn2_SqlText NVARCHAR(MAX) =
(
    SELECT
        'ALTER TABLE [MyTable2] DROP CONSTRAINT [' + dc.name + '];'
    FROM sys.tables t
    INNER JOIN sys.columns c
        ON c.object_id = t.object_id
    INNER JOIN sys.default_constraints dc
        ON dc.object_id = c.default_object_id
    WHERE t.name = 'MyTable2'
        AND c.name = 'MyColumn2'
);
EXEC (@DropDefaultConstraint_MyTable2_MyColumn2_SqlText);
--ALTER TABLE [MyTable2] DROP CONSTRAINT [DF_MyTable2_MyColumn2];
ALTER TABLE MyTable2 DROP COLUMN MyColumn2;
DECLARE @DropDefaultConstraint_MyTable2_MyColumn1_SqlText NVARCHAR(MAX) =
(
    SELECT
        'ALTER TABLE [MyTable2] DROP CONSTRAINT [' + dc.name + '];'
    FROM sys.tables t
    INNER JOIN sys.columns c
        ON c.object_id = t.object_id
    INNER JOIN sys.default_constraints dc
        ON dc.object_id = c.default_object_id
    WHERE t.name = 'MyTable2'
        AND c.name = 'MyColumn1'
);
EXEC (@DropDefaultConstraint_MyTable2_MyColumn1_SqlText);
--ALTER TABLE [MyTable2] DROP CONSTRAINT [DF_MyTable2_MyColumn1];
EXEC sp_rename 'MyTable2.MyColumn1', 'MyColumn1NewName', 'COLUMN';
ALTER TABLE MyTable2 ALTER COLUMN MyColumn1NewName INT NOT NULL;
ALTER TABLE MyTable2 ADD CONSTRAINT DF_MyTable2_MyColumn1NewName DEFAULT 333 FOR MyColumn1NewName;
ALTER TABLE MyTable2 ADD MyColumn2 VARBINARY(22) NULL;
ALTER TABLE MyTable2 ADD CONSTRAINT DF_MyTable2_MyColumn2 DEFAULT 0x000102 FOR MyColumn2;

ALTER TABLE MyTable2 ADD CONSTRAINT PK_MyTable2 PRIMARY KEY (MyColumn1NewName);

--QUERY END: AlterTableQuery

--QUERY START: UpdateDNDBTSysInfoQuery
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
      "ReferencedTableName": "MyTable3",
      "ReferencedTableColumnNames": [
        "MyColumn1",
        "MyColumn2"
      ],
      "OnUpdate": "NoAction",
      "OnDelete": "SetDefault",
      "ID": "480f3508-9d51-4190-88aa-45bc20e49119",
      "Name": "FK_MyTable2_MyColumns12_MyTable3_MyColumns12"
    }
  ],
  "ID": "bfb9030c-a8c3-4882-9c42-1c6ad025cf8f",
  "Name": "MyTable2"
}';
UPDATE DNDBTDbObjects SET
    Name = 'MyTable2',
    Metadata = @Metadata
WHERE ID = 'bfb9030c-a8c3-4882-9c42-1c6ad025cf8f';
--QUERY END: UpdateDNDBTSysInfoQuery

--QUERY START: AlterTableQuery
EXEC sp_rename 'MyTable1', 'MyTable1NewName';

ALTER TABLE MyTable1NewName DROP CONSTRAINT UQ_MyTable1_MyColumn2;
ALTER TABLE MyTable1NewName DROP CONSTRAINT PK_MyTable1;

DECLARE @DropDefaultConstraint_MyTable1NewName_MyColumn2_SqlText NVARCHAR(MAX) =
(
    SELECT
        'ALTER TABLE [MyTable1NewName] DROP CONSTRAINT [' + dc.name + '];'
    FROM sys.tables t
    INNER JOIN sys.columns c
        ON c.object_id = t.object_id
    INNER JOIN sys.default_constraints dc
        ON dc.object_id = c.default_object_id
    WHERE t.name = 'MyTable1NewName'
        AND c.name = 'MyColumn2'
);
EXEC (@DropDefaultConstraint_MyTable1NewName_MyColumn2_SqlText);
--ALTER TABLE [MyTable1NewName] DROP CONSTRAINT [DF_MyTable1NewName_MyColumn2];
ALTER TABLE MyTable1NewName DROP COLUMN MyColumn2;
DECLARE @DropDefaultConstraint_MyTable1NewName_MyColumn1_SqlText NVARCHAR(MAX) =
(
    SELECT
        'ALTER TABLE [MyTable1NewName] DROP CONSTRAINT [' + dc.name + '];'
    FROM sys.tables t
    INNER JOIN sys.columns c
        ON c.object_id = t.object_id
    INNER JOIN sys.default_constraints dc
        ON dc.object_id = c.default_object_id
    WHERE t.name = 'MyTable1NewName'
        AND c.name = 'MyColumn1'
);
EXEC (@DropDefaultConstraint_MyTable1NewName_MyColumn1_SqlText);
--ALTER TABLE [MyTable1NewName] DROP CONSTRAINT [DF_MyTable1NewName_MyColumn1];
EXEC sp_rename 'MyTable1NewName.MyColumn1', 'MyColumn1', 'COLUMN';
ALTER TABLE MyTable1NewName ALTER COLUMN MyColumn1 INT NULL;
ALTER TABLE MyTable1NewName ADD CONSTRAINT DF_MyTable1NewName_MyColumn1 DEFAULT 15 FOR MyColumn1;


--QUERY END: AlterTableQuery

--QUERY START: UpdateDNDBTSysInfoQuery
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
      "ReferencedTableName": "MyTable2",
      "ReferencedTableColumnNames": [
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
}';
UPDATE DNDBTDbObjects SET
    Name = 'MyTable1NewName',
    Metadata = @Metadata
WHERE ID = '299675e6-4faa-4d0f-a36a-224306ba5bcb';
--QUERY END: UpdateDNDBTSysInfoQuery

--QUERY START: AlterTableQuery
EXEC sp_rename 'MyTable5', 'MyTable5';


DECLARE @DropDefaultConstraint_MyTable5_MyColumn2_SqlText NVARCHAR(MAX) =
(
    SELECT
        'ALTER TABLE [MyTable5] DROP CONSTRAINT [' + dc.name + '];'
    FROM sys.tables t
    INNER JOIN sys.columns c
        ON c.object_id = t.object_id
    INNER JOIN sys.default_constraints dc
        ON dc.object_id = c.default_object_id
    WHERE t.name = 'MyTable5'
        AND c.name = 'MyColumn2'
);
EXEC (@DropDefaultConstraint_MyTable5_MyColumn2_SqlText);
--ALTER TABLE [MyTable5] DROP CONSTRAINT [DF_MyTable5_MyColumn2];
EXEC sp_rename 'MyTable5.MyColumn2', 'MyColumn2', 'COLUMN';
ALTER TABLE MyTable5 ALTER COLUMN MyColumn2 MyUserDefinedType1 NULL;
ALTER TABLE MyTable5 ADD CONSTRAINT DF_MyTable5_MyColumn2 DEFAULT 'cc' FOR MyColumn2;


--QUERY END: AlterTableQuery

--QUERY START: UpdateDNDBTSysInfoQuery
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
      "ID": "5309d66f-2030-402e-912e-5547babaa072",
      "Name": "MyColumn1"
    },
    {
      "DataType": {
        "Name": "MyUserDefinedType1",
        "IsUserDefined": true,
        "Length": 110,
        "IsUnicode": false,
        "IsFixedLength": false
      },
      "Nullable": true,
      "Identity": false,
      "Default": "cc",
      "ID": "15ae6061-426d-4485-85e6-ecd3e0f98882",
      "Name": "MyColumn2"
    }
  ],
  "PrimaryKey": null,
  "UniqueConstraints": [],
  "ForeignKeys": [],
  "ID": "6ca51f29-c1bc-4349-b9c1-6f1ea170f162",
  "Name": "MyTable5"
}';
UPDATE DNDBTDbObjects SET
    Name = 'MyTable5',
    Metadata = @Metadata
WHERE ID = '6ca51f29-c1bc-4349-b9c1-6f1ea170f162';
--QUERY END: UpdateDNDBTSysInfoQuery

--QUERY START: DropTypeQuery
DROP TYPE _DNDBTTemp_MyUserDefinedType1;
--QUERY END: DropTypeQuery

--QUERY START: CreateTableQuery
CREATE TABLE MyTable3
(
    MyColumn1 INT NOT NULL CONSTRAINT DF_MyTable3_MyColumn1 DEFAULT 333,
    MyColumn2 VARBINARY(22) NOT NULL ,
    CONSTRAINT UQ_MyTable3_MyColumns12 UNIQUE (MyColumn1, MyColumn2)
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
      "Name": "UQ_MyTable3_MyColumns12"
    }
  ],
  "ForeignKeys": [],
  "ID": "474cd761-2522-4529-9d20-2b94115f9626",
  "Name": "MyTable3"
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
    '474cd761-2522-4529-9d20-2b94115f9626',
    'Table',
    'MyTable3',
    @Metadata
);
--QUERY END: InsertDNDBTSysInfoQuery

--QUERY START: CreateForeignKeyQuery
ALTER TABLE MyTable2 ADD CONSTRAINT FK_MyTable2_MyColumns12_MyTable3_MyColumns12 FOREIGN KEY (MyColumn1NewName, MyColumn2)
    REFERENCES MyTable3 (MyColumn1, MyColumn2)
    ON UPDATE NO ACTION ON DELETE SET DEFAULT;
--QUERY END: CreateForeignKeyQuery

--QUERY START: CreateForeignKeyQuery
ALTER TABLE MyTable1NewName ADD CONSTRAINT FK_MyTable1_MyColumn1_MyTable2_MyColumn1 FOREIGN KEY (MyColumn1)
    REFERENCES MyTable2 (MyColumn1NewName)
    ON UPDATE NO ACTION ON DELETE SET NULL;
--QUERY END: CreateForeignKeyQuery