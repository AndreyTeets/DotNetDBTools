SET NOCOUNT ON;
SET XACT_ABORT ON;
BEGIN TRY;
    BEGIN TRANSACTION;

-- QUERY START: GenericQuery
EXEC sp_executesql N'IF EXISTS (SELECT * FROM [sysobjects] WHERE [name]=''_MyTable2'' AND [xtype]=''U'')
    DROP TABLE [_MyTable2];

CREATE TABLE [_MyTable2]
(
    [MyColumn1] BIGINT NOT NULL PRIMARY KEY,
    [MyColumn2] VARBINARY(22)
);

INSERT INTO [_MyTable2] ([MyColumn1], [MyColumn2])
SELECT [MyColumn1], [MyColumn2] FROM [MyTable2]';
-- QUERY END: GenericQuery

-- QUERY START: MSSQLInsertDNDBTScriptExecutionRecordQuery
EXEC sp_executesql N'DECLARE @ID UNIQUEIDENTIFIER = N''7f72f0df-4eda-4063-99d8-99c1f37819d2'';
DECLARE @Type NVARCHAR(MAX) = N''BeforePublishOnce'';
DECLARE @Name NVARCHAR(MAX) = N''SaveRecreatedColumnsData'';
DECLARE @Code NVARCHAR(MAX) = N''IF EXISTS (SELECT * FROM [sysobjects] WHERE [name]=''''_MyTable2'''' AND [xtype]=''''U'''')
    DROP TABLE [_MyTable2];

CREATE TABLE [_MyTable2]
(
    [MyColumn1] BIGINT NOT NULL PRIMARY KEY,
    [MyColumn2] VARBINARY(22)
);

INSERT INTO [_MyTable2] ([MyColumn1], [MyColumn2])
SELECT [MyColumn1], [MyColumn2] FROM [MyTable2]'';
DECLARE @MinDbVersionToExecute BIGINT = 1;
DECLARE @MaxDbVersionToExecute BIGINT = 1;
DECLARE @ExecutedOnDbVersion BIGINT = 1;
INSERT INTO [DNDBTScriptExecutions]
(
    [ID],
    [Type],
    [Name],
    [Code],
    [MinDbVersionToExecute],
    [MaxDbVersionToExecute],
    [ExecutedOnDbVersion]
)
VALUES
(
    @ID,
    @Type,
    @Name,
    @Code,
    @MinDbVersionToExecute,
    @MaxDbVersionToExecute,
    @ExecutedOnDbVersion
);';
-- QUERY END: MSSQLInsertDNDBTScriptExecutionRecordQuery

-- QUERY START: MSSQLDropTriggerQuery
EXEC sp_executesql N'DROP TRIGGER [TR_MyTable2_MyTrigger1];';
-- QUERY END: MSSQLDropTriggerQuery

-- QUERY START: MSSQLDeleteDNDBTDbObjectRecordQuery
EXEC sp_executesql N'DELETE FROM [DNDBTDbObjects]
WHERE [ID] = ''ee64ffc3-5536-4624-beaf-bc3a61d06a1a'';';
-- QUERY END: MSSQLDeleteDNDBTDbObjectRecordQuery

-- QUERY START: GenericQuery
EXEC sp_executesql N'DROP VIEW [MyView1];';
-- QUERY END: GenericQuery

-- QUERY START: MSSQLDeleteDNDBTDbObjectRecordQuery
EXEC sp_executesql N'DELETE FROM [DNDBTDbObjects]
WHERE [ID] = ''e2569aae-d5da-4a77-b3cd-51adbdb272d9'';';
-- QUERY END: MSSQLDeleteDNDBTDbObjectRecordQuery

-- QUERY START: MSSQLDropForeignKeyQuery
EXEC sp_executesql N'ALTER TABLE [MyTable6] DROP CONSTRAINT [FK_MyTable6_MyTable5_CustomName];';
-- QUERY END: MSSQLDropForeignKeyQuery

-- QUERY START: MSSQLDeleteDNDBTDbObjectRecordQuery
EXEC sp_executesql N'DELETE FROM [DNDBTDbObjects]
WHERE [ID] = ''ae453b22-d270-41fc-8184-9ac26b7a0569'';';
-- QUERY END: MSSQLDeleteDNDBTDbObjectRecordQuery

-- QUERY START: MSSQLDropForeignKeyQuery
EXEC sp_executesql N'ALTER TABLE [MyTable1] DROP CONSTRAINT [FK_MyTable1_MyColumn1_MyTable2_MyColumn1];';
-- QUERY END: MSSQLDropForeignKeyQuery

-- QUERY START: MSSQLDeleteDNDBTDbObjectRecordQuery
EXEC sp_executesql N'DELETE FROM [DNDBTDbObjects]
WHERE [ID] = ''d11b2a53-32db-432f-bb6b-f91788844ba9'';';
-- QUERY END: MSSQLDeleteDNDBTDbObjectRecordQuery

-- QUERY START: MSSQLDropIndexQuery
EXEC sp_executesql N'DROP INDEX [IDX_MyTable2_MyIndex1] ON [MyTable2];';
-- QUERY END: MSSQLDropIndexQuery

-- QUERY START: MSSQLDeleteDNDBTDbObjectRecordQuery
EXEC sp_executesql N'DELETE FROM [DNDBTDbObjects]
WHERE [ID] = ''74390b3c-bc39-4860-a42e-12baa400f927'';';
-- QUERY END: MSSQLDeleteDNDBTDbObjectRecordQuery

-- QUERY START: MSSQLDropIndexQuery
EXEC sp_executesql N'DROP INDEX [IDX_MyTable5_CustomName] ON [MyTable5];';
-- QUERY END: MSSQLDropIndexQuery

-- QUERY START: MSSQLDeleteDNDBTDbObjectRecordQuery
EXEC sp_executesql N'DELETE FROM [DNDBTDbObjects]
WHERE [ID] = ''1d632285-9914-4c5d-98e6-a618a99bd799'';';
-- QUERY END: MSSQLDeleteDNDBTDbObjectRecordQuery

-- QUERY START: MSSQLDropTableQuery
EXEC sp_executesql N'DROP TABLE [MyTable6];';
-- QUERY END: MSSQLDropTableQuery

-- QUERY START: MSSQLDeleteDNDBTDbObjectRecordQuery
EXEC sp_executesql N'DELETE FROM [DNDBTDbObjects]
WHERE [ID] = ''bfa08c82-5c8f-4ab4-bd41-1f1d85cf3c85'';';
-- QUERY END: MSSQLDeleteDNDBTDbObjectRecordQuery

-- QUERY START: MSSQLDeleteDNDBTDbObjectRecordQuery
EXEC sp_executesql N'DELETE FROM [DNDBTDbObjects]
WHERE [ID] = ''a402e2b7-c826-4cfd-a304-97c9bc346ba2'';';
-- QUERY END: MSSQLDeleteDNDBTDbObjectRecordQuery

-- QUERY START: MSSQLDeleteDNDBTDbObjectRecordQuery
EXEC sp_executesql N'DELETE FROM [DNDBTDbObjects]
WHERE [ID] = ''f3064a8c-346a-4b3d-af2c-d967b39841e4'';';
-- QUERY END: MSSQLDeleteDNDBTDbObjectRecordQuery

-- QUERY START: MSSQLRenameUserDefinedDataTypeQuery
EXEC sp_executesql N'EXEC sp_rename ''MyUserDefinedType1'', ''_DNDBTTemp_MyUserDefinedType1'', ''USERDATATYPE'';';
-- QUERY END: MSSQLRenameUserDefinedDataTypeQuery

-- QUERY START: MSSQLDeleteDNDBTDbObjectRecordQuery
EXEC sp_executesql N'DELETE FROM [DNDBTDbObjects]
WHERE [ID] = ''0cd1e71c-cc9c-440f-ac0b-81a1d6f7ddaa'';';
-- QUERY END: MSSQLDeleteDNDBTDbObjectRecordQuery

-- QUERY START: MSSQLCreateTypeQuery
EXEC sp_executesql N'CREATE TYPE [MyUserDefinedType1] FROM NVARCHAR(110);';
-- QUERY END: MSSQLCreateTypeQuery

-- QUERY START: MSSQLInsertDNDBTDbObjectRecordQuery
EXEC sp_executesql N'DECLARE @ID UNIQUEIDENTIFIER = N''0cd1e71c-cc9c-440f-ac0b-81a1d6f7ddaa'';
DECLARE @ParentID UNIQUEIDENTIFIER = NULL;
DECLARE @Name NVARCHAR(MAX) = N''MyUserDefinedType1'';
DECLARE @Code NVARCHAR(MAX) = NULL;
INSERT INTO [DNDBTDbObjects]
(
    [ID],
    [ParentID],
    [Type],
    [Name],
    [Code]
)
VALUES
(
    @ID,
    @ParentID,
    ''UserDefinedType'',
    @Name,
    @Code
);';
-- QUERY END: MSSQLInsertDNDBTDbObjectRecordQuery

-- QUERY START: MSSQLUseNewUDTInAllTablesQuery
EXEC sp_executesql N'DECLARE @SqlText NVARCHAR(MAX) =
(
    SELECT STUFF ((
        SELECT
            CHAR(10) + CHAR(10) + t.AlterStatement
        FROM
        (
            SELECT
                cci.DropConstraintStatement + CHAR(10) +
                N''ALTER TABLE '' + QUOTENAME(cci.TABLE_NAME) + '' ALTER COLUMN '' + QUOTENAME(cci.COLUMN_NAME) + '' '' + ''[MyUserDefinedType1]'' + '';'' + CHAR(10) +
                cci.AddConstraintStatement AS AlterStatement
            FROM
            (
                SELECT
                    c.TABLE_NAME,
                    c.COLUMN_NAME,
                    ccu.CONSTRAINT_NAME,
                    tc.CONSTRAINT_TYPE,
                    CASE WHEN tc.CONSTRAINT_TYPE IS NOT NULL THEN
                        N''ALTER TABLE '' + QUOTENAME(c.TABLE_NAME) + '' DROP CONSTRAINT '' + QUOTENAME(ccu.CONSTRAINT_NAME) + '';'' + CHAR(10)
                    ELSE
                        ''''
                    END AS DropConstraintStatement,
                    CASE WHEN tc.CONSTRAINT_TYPE IS NOT NULL THEN
                        N''ALTER TABLE '' + QUOTENAME(c.TABLE_NAME) + '' ADD CONSTRAINT '' + QUOTENAME(ccu.CONSTRAINT_NAME) + '' '' + tc.CONSTRAINT_TYPE + ''('' +
                        (SELECT STUFF ((
                            SELECT
                                '', '' + t.COLUMN_NAME
                            FROM
                            (
                                SELECT
                                    ccu2.COLUMN_NAME
                                FROM INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE ccu2
                                WHERE ccu2.CONSTRAINT_NAME = ccu.CONSTRAINT_NAME
                            ) t FOR XML PATH('''')), 1, 2, '''')) +
                        '')'' + '';''
                    ELSE
                        ''''
                    END AS ADDConstraintStatement
                FROM INFORMATION_SCHEMA.COLUMNS c
                LEFT JOIN INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE ccu
                    ON ccu.COLUMN_NAME = c.COLUMN_NAME
                        AND ccu.TABLE_NAME = c.TABLE_NAME
                LEFT JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS tc
                    ON tc.CONSTRAINT_NAME = ccu.CONSTRAINT_NAME
                        AND tc.CONSTRAINT_TYPE IN (''UNIQUE'', ''PRIMARY KEY'')
                WHERE c.DOMAIN_NAME = ''_DNDBTTemp_MyUserDefinedType1''
            ) cci
        ) t FOR XML PATH('''')), 1, 2, '''')
);
EXEC (@SqlText);';
-- QUERY END: MSSQLUseNewUDTInAllTablesQuery

-- QUERY START: MSSQLAlterTableQuery
EXEC sp_executesql N'EXEC sp_rename ''MyTable1'', ''MyTable1NewName'';

ALTER TABLE [MyTable1NewName] DROP CONSTRAINT [CK_MyTable1_MyCheck1];
ALTER TABLE [MyTable1NewName] DROP CONSTRAINT [UQ_MyTable1_MyColumn4];
ALTER TABLE [MyTable1NewName] DROP CONSTRAINT [PK_MyTable1];
ALTER TABLE [MyTable1NewName] DROP CONSTRAINT [DF_MyTable1_MyColumn2];
ALTER TABLE [MyTable1NewName] DROP COLUMN [MyColumn2];
ALTER TABLE [MyTable1NewName] DROP COLUMN [MyColumn3];
ALTER TABLE [MyTable1NewName] DROP CONSTRAINT [DF_MyTable1_MyColumn1];
ALTER TABLE [MyTable1NewName] ALTER COLUMN [MyColumn1] BIGINT NULL;
ALTER TABLE [MyTable1NewName] ADD CONSTRAINT [DF_MyTable1NewName_MyColumn1] DEFAULT 15 FOR [MyColumn1];
ALTER TABLE [MyTable1NewName] DROP CONSTRAINT [DF_MyTable1_MyColumn4];
ALTER TABLE [MyTable1NewName] ADD CONSTRAINT [CK_MyTable1_MyCheck1] CHECK (MyColumn4 >= 1);
';
-- QUERY END: MSSQLAlterTableQuery

-- QUERY START: MSSQLDeleteDNDBTDbObjectRecordQuery
EXEC sp_executesql N'DELETE FROM [DNDBTDbObjects]
WHERE [ID] = ''eb9c59b5-bc7e-49d7-adaa-f5600b6a19a2'';';
-- QUERY END: MSSQLDeleteDNDBTDbObjectRecordQuery

-- QUERY START: MSSQLDeleteDNDBTDbObjectRecordQuery
EXEC sp_executesql N'DELETE FROM [DNDBTDbObjects]
WHERE [ID] = ''f3f08522-26ee-4950-9135-22edf2e4e0cf'';';
-- QUERY END: MSSQLDeleteDNDBTDbObjectRecordQuery

-- QUERY START: MSSQLDeleteDNDBTDbObjectRecordQuery
EXEC sp_executesql N'DELETE FROM [DNDBTDbObjects]
WHERE [ID] = ''37a45def-f4a0-4be7-8bfb-8fbed4a7d705'';';
-- QUERY END: MSSQLDeleteDNDBTDbObjectRecordQuery

-- QUERY START: MSSQLDeleteDNDBTDbObjectRecordQuery
EXEC sp_executesql N'DELETE FROM [DNDBTDbObjects]
WHERE [ID] = ''fe68ee3d-09d0-40ac-93f9-5e441fbb4f70'';';
-- QUERY END: MSSQLDeleteDNDBTDbObjectRecordQuery

-- QUERY START: MSSQLDeleteDNDBTDbObjectRecordQuery
EXEC sp_executesql N'DELETE FROM [DNDBTDbObjects]
WHERE [ID] = ''6e95de30-e01a-4fb4-b8b7-8f0c40bb682c'';';
-- QUERY END: MSSQLDeleteDNDBTDbObjectRecordQuery

-- QUERY START: MSSQLUpdateDNDBTDbObjectRecordQuery
EXEC sp_executesql N'DECLARE @ID UNIQUEIDENTIFIER = N''299675e6-4faa-4d0f-a36a-224306ba5bcb'';
DECLARE @Name NVARCHAR(MAX) = N''MyTable1NewName'';
DECLARE @Code NVARCHAR(MAX) = NULL;
UPDATE [DNDBTDbObjects] SET
    [Name] = @Name,
    [Code] = @Code
WHERE [ID] = @ID;';
-- QUERY END: MSSQLUpdateDNDBTDbObjectRecordQuery

-- QUERY START: MSSQLUpdateDNDBTDbObjectRecordQuery
EXEC sp_executesql N'DECLARE @ID UNIQUEIDENTIFIER = N''a2f2a4de-1337-4594-ae41-72ed4d05f317'';
DECLARE @Name NVARCHAR(MAX) = N''MyColumn1'';
DECLARE @Code NVARCHAR(MAX) = N''15'';
UPDATE [DNDBTDbObjects] SET
    [Name] = @Name,
    [Code] = @Code
WHERE [ID] = @ID;';
-- QUERY END: MSSQLUpdateDNDBTDbObjectRecordQuery

-- QUERY START: MSSQLUpdateDNDBTDbObjectRecordQuery
EXEC sp_executesql N'DECLARE @ID UNIQUEIDENTIFIER = N''867ac528-e87e-4c93-b6e3-dd2fcbbb837f'';
DECLARE @Name NVARCHAR(MAX) = N''MyColumn4'';
DECLARE @Code NVARCHAR(MAX) = NULL;
UPDATE [DNDBTDbObjects] SET
    [Name] = @Name,
    [Code] = @Code
WHERE [ID] = @ID;';
-- QUERY END: MSSQLUpdateDNDBTDbObjectRecordQuery

-- QUERY START: MSSQLInsertDNDBTDbObjectRecordQuery
EXEC sp_executesql N'DECLARE @ID UNIQUEIDENTIFIER = N''eb9c59b5-bc7e-49d7-adaa-f5600b6a19a2'';
DECLARE @ParentID UNIQUEIDENTIFIER = N''299675e6-4faa-4d0f-a36a-224306ba5bcb'';
DECLARE @Name NVARCHAR(MAX) = N''CK_MyTable1_MyCheck1'';
DECLARE @Code NVARCHAR(MAX) = N''CHECK (MyColumn4 >= 1)'';
INSERT INTO [DNDBTDbObjects]
(
    [ID],
    [ParentID],
    [Type],
    [Name],
    [Code]
)
VALUES
(
    @ID,
    @ParentID,
    ''CheckConstraint'',
    @Name,
    @Code
);';
-- QUERY END: MSSQLInsertDNDBTDbObjectRecordQuery

-- QUERY START: MSSQLAlterTableQuery
EXEC sp_executesql N'
EXEC sp_rename ''MyTable2.MyColumn1'', ''MyColumn1NewName'', ''COLUMN'';

ALTER TABLE [MyTable2] DROP CONSTRAINT [PK_MyTable2_CustomName];
ALTER TABLE [MyTable2] DROP CONSTRAINT [DF_MyTable2_MyColumn2];
ALTER TABLE [MyTable2] DROP COLUMN [MyColumn2];
ALTER TABLE [MyTable2] DROP CONSTRAINT [DF_MyTable2_MyColumn1];
ALTER TABLE [MyTable2] ALTER COLUMN [MyColumn1NewName] BIGINT NOT NULL;
ALTER TABLE [MyTable2] ADD CONSTRAINT [DF_MyTable2_MyColumn1NewName] DEFAULT 333 FOR [MyColumn1NewName];
ALTER TABLE [MyTable2] ADD [MyColumn2] VARBINARY(22) NULL CONSTRAINT [DF_MyTable2_MyColumn2] DEFAULT 0x000408 WITH VALUES;
ALTER TABLE [MyTable2] ADD [MyColumn3] BIGINT NULL;
ALTER TABLE [MyTable2] ADD [MyColumn4] VARBINARY(50) NULL;
ALTER TABLE [MyTable2] ADD CONSTRAINT [PK_MyTable2_CustomName] PRIMARY KEY ([MyColumn1NewName]);
';
-- QUERY END: MSSQLAlterTableQuery

-- QUERY START: MSSQLDeleteDNDBTDbObjectRecordQuery
EXEC sp_executesql N'DELETE FROM [DNDBTDbObjects]
WHERE [ID] = ''3a43615b-40b3-4a13-99e7-93af7c56e8ce'';';
-- QUERY END: MSSQLDeleteDNDBTDbObjectRecordQuery

-- QUERY START: MSSQLDeleteDNDBTDbObjectRecordQuery
EXEC sp_executesql N'DELETE FROM [DNDBTDbObjects]
WHERE [ID] = ''5a0d1926-3270-4eb2-92eb-00be56c7af23'';';
-- QUERY END: MSSQLDeleteDNDBTDbObjectRecordQuery

-- QUERY START: MSSQLUpdateDNDBTDbObjectRecordQuery
EXEC sp_executesql N'DECLARE @ID UNIQUEIDENTIFIER = N''bfb9030c-a8c3-4882-9c42-1c6ad025cf8f'';
DECLARE @Name NVARCHAR(MAX) = N''MyTable2'';
DECLARE @Code NVARCHAR(MAX) = NULL;
UPDATE [DNDBTDbObjects] SET
    [Name] = @Name,
    [Code] = @Code
WHERE [ID] = @ID;';
-- QUERY END: MSSQLUpdateDNDBTDbObjectRecordQuery

-- QUERY START: MSSQLUpdateDNDBTDbObjectRecordQuery
EXEC sp_executesql N'DECLARE @ID UNIQUEIDENTIFIER = N''c480f22f-7c01-4f41-b282-35e9f5cd1fe3'';
DECLARE @Name NVARCHAR(MAX) = N''MyColumn1NewName'';
DECLARE @Code NVARCHAR(MAX) = N''333'';
UPDATE [DNDBTDbObjects] SET
    [Name] = @Name,
    [Code] = @Code
WHERE [ID] = @ID;';
-- QUERY END: MSSQLUpdateDNDBTDbObjectRecordQuery

-- QUERY START: MSSQLInsertDNDBTDbObjectRecordQuery
EXEC sp_executesql N'DECLARE @ID UNIQUEIDENTIFIER = N''c2df19c2-e029-4014-8a5b-4ab42fecb6b8'';
DECLARE @ParentID UNIQUEIDENTIFIER = N''bfb9030c-a8c3-4882-9c42-1c6ad025cf8f'';
DECLARE @Name NVARCHAR(MAX) = N''MyColumn2'';
DECLARE @Code NVARCHAR(MAX) = N''0x000408'';
INSERT INTO [DNDBTDbObjects]
(
    [ID],
    [ParentID],
    [Type],
    [Name],
    [Code]
)
VALUES
(
    @ID,
    @ParentID,
    ''Column'',
    @Name,
    @Code
);';
-- QUERY END: MSSQLInsertDNDBTDbObjectRecordQuery

-- QUERY START: MSSQLInsertDNDBTDbObjectRecordQuery
EXEC sp_executesql N'DECLARE @ID UNIQUEIDENTIFIER = N''99bc3f49-3151-4f52-87f7-104b424ed7bf'';
DECLARE @ParentID UNIQUEIDENTIFIER = N''bfb9030c-a8c3-4882-9c42-1c6ad025cf8f'';
DECLARE @Name NVARCHAR(MAX) = N''MyColumn3'';
DECLARE @Code NVARCHAR(MAX) = NULL;
INSERT INTO [DNDBTDbObjects]
(
    [ID],
    [ParentID],
    [Type],
    [Name],
    [Code]
)
VALUES
(
    @ID,
    @ParentID,
    ''Column'',
    @Name,
    @Code
);';
-- QUERY END: MSSQLInsertDNDBTDbObjectRecordQuery

-- QUERY START: MSSQLInsertDNDBTDbObjectRecordQuery
EXEC sp_executesql N'DECLARE @ID UNIQUEIDENTIFIER = N''87950a3f-2072-42db-ac3c-a4e85b79720d'';
DECLARE @ParentID UNIQUEIDENTIFIER = N''bfb9030c-a8c3-4882-9c42-1c6ad025cf8f'';
DECLARE @Name NVARCHAR(MAX) = N''MyColumn4'';
DECLARE @Code NVARCHAR(MAX) = NULL;
INSERT INTO [DNDBTDbObjects]
(
    [ID],
    [ParentID],
    [Type],
    [Name],
    [Code]
)
VALUES
(
    @ID,
    @ParentID,
    ''Column'',
    @Name,
    @Code
);';
-- QUERY END: MSSQLInsertDNDBTDbObjectRecordQuery

-- QUERY START: MSSQLInsertDNDBTDbObjectRecordQuery
EXEC sp_executesql N'DECLARE @ID UNIQUEIDENTIFIER = N''3a43615b-40b3-4a13-99e7-93af7c56e8ce'';
DECLARE @ParentID UNIQUEIDENTIFIER = N''bfb9030c-a8c3-4882-9c42-1c6ad025cf8f'';
DECLARE @Name NVARCHAR(MAX) = N''PK_MyTable2_CustomName'';
DECLARE @Code NVARCHAR(MAX) = NULL;
INSERT INTO [DNDBTDbObjects]
(
    [ID],
    [ParentID],
    [Type],
    [Name],
    [Code]
)
VALUES
(
    @ID,
    @ParentID,
    ''PrimaryKey'',
    @Name,
    @Code
);';
-- QUERY END: MSSQLInsertDNDBTDbObjectRecordQuery

-- QUERY START: MSSQLAlterTableQuery
EXEC sp_executesql N'
ALTER TABLE [MyTable5] DROP CONSTRAINT [UQ_MyTable5_CustomName];
ALTER TABLE [MyTable5] DROP CONSTRAINT [PK_MyTable5_CustomName];
';
-- QUERY END: MSSQLAlterTableQuery

-- QUERY START: MSSQLDeleteDNDBTDbObjectRecordQuery
EXEC sp_executesql N'DELETE FROM [DNDBTDbObjects]
WHERE [ID] = ''5293b58a-9f63-4f0f-8d6f-18416ebbd751'';';
-- QUERY END: MSSQLDeleteDNDBTDbObjectRecordQuery

-- QUERY START: MSSQLDeleteDNDBTDbObjectRecordQuery
EXEC sp_executesql N'DELETE FROM [DNDBTDbObjects]
WHERE [ID] = ''79384d48-a39b-4a22-900e-066b2ca67ba2'';';
-- QUERY END: MSSQLDeleteDNDBTDbObjectRecordQuery

-- QUERY START: MSSQLUpdateDNDBTDbObjectRecordQuery
EXEC sp_executesql N'DECLARE @ID UNIQUEIDENTIFIER = N''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'';
DECLARE @Name NVARCHAR(MAX) = N''MyTable5'';
DECLARE @Code NVARCHAR(MAX) = NULL;
UPDATE [DNDBTDbObjects] SET
    [Name] = @Name,
    [Code] = @Code
WHERE [ID] = @ID;';
-- QUERY END: MSSQLUpdateDNDBTDbObjectRecordQuery

-- QUERY START: MSSQLDropTypeQuery
EXEC sp_executesql N'DROP TYPE [_DNDBTTemp_MyUserDefinedType1];';
-- QUERY END: MSSQLDropTypeQuery

-- QUERY START: MSSQLCreateTableQuery
EXEC sp_executesql N'CREATE TABLE [MyTable3]
(
    [MyColumn1] BIGINT NOT NULL CONSTRAINT [DF_MyTable3_MyColumn1] DEFAULT 444,
    [MyColumn2] VARBINARY(50) NOT NULL,
    CONSTRAINT [UQ_MyTable3_MyColumns12] UNIQUE ([MyColumn1], [MyColumn2])
);';
-- QUERY END: MSSQLCreateTableQuery

-- QUERY START: MSSQLInsertDNDBTDbObjectRecordQuery
EXEC sp_executesql N'DECLARE @ID UNIQUEIDENTIFIER = N''474cd761-2522-4529-9d20-2b94115f9626'';
DECLARE @ParentID UNIQUEIDENTIFIER = NULL;
DECLARE @Name NVARCHAR(MAX) = N''MyTable3'';
DECLARE @Code NVARCHAR(MAX) = NULL;
INSERT INTO [DNDBTDbObjects]
(
    [ID],
    [ParentID],
    [Type],
    [Name],
    [Code]
)
VALUES
(
    @ID,
    @ParentID,
    ''Table'',
    @Name,
    @Code
);';
-- QUERY END: MSSQLInsertDNDBTDbObjectRecordQuery

-- QUERY START: MSSQLInsertDNDBTDbObjectRecordQuery
EXEC sp_executesql N'DECLARE @ID UNIQUEIDENTIFIER = N''726f503a-d944-46ee-a0ff-6a2c2faab46e'';
DECLARE @ParentID UNIQUEIDENTIFIER = N''474cd761-2522-4529-9d20-2b94115f9626'';
DECLARE @Name NVARCHAR(MAX) = N''MyColumn1'';
DECLARE @Code NVARCHAR(MAX) = N''444'';
INSERT INTO [DNDBTDbObjects]
(
    [ID],
    [ParentID],
    [Type],
    [Name],
    [Code]
)
VALUES
(
    @ID,
    @ParentID,
    ''Column'',
    @Name,
    @Code
);';
-- QUERY END: MSSQLInsertDNDBTDbObjectRecordQuery

-- QUERY START: MSSQLInsertDNDBTDbObjectRecordQuery
EXEC sp_executesql N'DECLARE @ID UNIQUEIDENTIFIER = N''169824e1-8b74-4b60-af17-99656d6dbbee'';
DECLARE @ParentID UNIQUEIDENTIFIER = N''474cd761-2522-4529-9d20-2b94115f9626'';
DECLARE @Name NVARCHAR(MAX) = N''MyColumn2'';
DECLARE @Code NVARCHAR(MAX) = NULL;
INSERT INTO [DNDBTDbObjects]
(
    [ID],
    [ParentID],
    [Type],
    [Name],
    [Code]
)
VALUES
(
    @ID,
    @ParentID,
    ''Column'',
    @Name,
    @Code
);';
-- QUERY END: MSSQLInsertDNDBTDbObjectRecordQuery

-- QUERY START: MSSQLInsertDNDBTDbObjectRecordQuery
EXEC sp_executesql N'DECLARE @ID UNIQUEIDENTIFIER = N''fd288e38-35ba-4bb1-ace3-597c99ef26c7'';
DECLARE @ParentID UNIQUEIDENTIFIER = N''474cd761-2522-4529-9d20-2b94115f9626'';
DECLARE @Name NVARCHAR(MAX) = N''UQ_MyTable3_MyColumns12'';
DECLARE @Code NVARCHAR(MAX) = NULL;
INSERT INTO [DNDBTDbObjects]
(
    [ID],
    [ParentID],
    [Type],
    [Name],
    [Code]
)
VALUES
(
    @ID,
    @ParentID,
    ''UniqueConstraint'',
    @Name,
    @Code
);';
-- QUERY END: MSSQLInsertDNDBTDbObjectRecordQuery

-- QUERY START: MSSQLCreateIndexQuery
EXEC sp_executesql N'CREATE UNIQUE INDEX [IDX_MyTable2_MyIndex1]
ON [MyTable2] ([MyColumn1NewName], [MyColumn2]);';
-- QUERY END: MSSQLCreateIndexQuery

-- QUERY START: MSSQLInsertDNDBTDbObjectRecordQuery
EXEC sp_executesql N'DECLARE @ID UNIQUEIDENTIFIER = N''74390b3c-bc39-4860-a42e-12baa400f927'';
DECLARE @ParentID UNIQUEIDENTIFIER = N''bfb9030c-a8c3-4882-9c42-1c6ad025cf8f'';
DECLARE @Name NVARCHAR(MAX) = N''IDX_MyTable2_MyIndex1'';
DECLARE @Code NVARCHAR(MAX) = NULL;
INSERT INTO [DNDBTDbObjects]
(
    [ID],
    [ParentID],
    [Type],
    [Name],
    [Code]
)
VALUES
(
    @ID,
    @ParentID,
    ''Index'',
    @Name,
    @Code
);';
-- QUERY END: MSSQLInsertDNDBTDbObjectRecordQuery

-- QUERY START: MSSQLCreateForeignKeyQuery
EXEC sp_executesql N'ALTER TABLE [MyTable1NewName] ADD CONSTRAINT [FK_MyTable1_MyColumn1_MyTable2_MyColumn1] FOREIGN KEY ([MyColumn1])
    REFERENCES [MyTable2] ([MyColumn1NewName])
    ON UPDATE NO ACTION ON DELETE SET NULL;';
-- QUERY END: MSSQLCreateForeignKeyQuery

-- QUERY START: MSSQLInsertDNDBTDbObjectRecordQuery
EXEC sp_executesql N'DECLARE @ID UNIQUEIDENTIFIER = N''d11b2a53-32db-432f-bb6b-f91788844ba9'';
DECLARE @ParentID UNIQUEIDENTIFIER = N''299675e6-4faa-4d0f-a36a-224306ba5bcb'';
DECLARE @Name NVARCHAR(MAX) = N''FK_MyTable1_MyColumn1_MyTable2_MyColumn1'';
DECLARE @Code NVARCHAR(MAX) = NULL;
INSERT INTO [DNDBTDbObjects]
(
    [ID],
    [ParentID],
    [Type],
    [Name],
    [Code]
)
VALUES
(
    @ID,
    @ParentID,
    ''ForeignKey'',
    @Name,
    @Code
);';
-- QUERY END: MSSQLInsertDNDBTDbObjectRecordQuery

-- QUERY START: MSSQLCreateForeignKeyQuery
EXEC sp_executesql N'ALTER TABLE [MyTable2] ADD CONSTRAINT [FK_MyTable2_MyColumns34_MyTable3_MyColumns12] FOREIGN KEY ([MyColumn3], [MyColumn4])
    REFERENCES [MyTable3] ([MyColumn1], [MyColumn2])
    ON UPDATE NO ACTION ON DELETE SET DEFAULT;';
-- QUERY END: MSSQLCreateForeignKeyQuery

-- QUERY START: MSSQLInsertDNDBTDbObjectRecordQuery
EXEC sp_executesql N'DECLARE @ID UNIQUEIDENTIFIER = N''480f3508-9d51-4190-88aa-45bc20e49119'';
DECLARE @ParentID UNIQUEIDENTIFIER = N''bfb9030c-a8c3-4882-9c42-1c6ad025cf8f'';
DECLARE @Name NVARCHAR(MAX) = N''FK_MyTable2_MyColumns34_MyTable3_MyColumns12'';
DECLARE @Code NVARCHAR(MAX) = NULL;
INSERT INTO [DNDBTDbObjects]
(
    [ID],
    [ParentID],
    [Type],
    [Name],
    [Code]
)
VALUES
(
    @ID,
    @ParentID,
    ''ForeignKey'',
    @Name,
    @Code
);';
-- QUERY END: MSSQLInsertDNDBTDbObjectRecordQuery

-- QUERY START: GenericQuery
EXEC sp_executesql N'CREATE VIEW MyView1 AS
SELECT
    t1.MyColumn1,
    t1.MyColumn4,
    t2.MyColumn2
FROM MyTable1NewName t1
LEFT JOIN MyTable2 t2
    ON t2.MyColumn1NewName = t1.MyColumn1';
-- QUERY END: GenericQuery

-- QUERY START: MSSQLInsertDNDBTDbObjectRecordQuery
EXEC sp_executesql N'DECLARE @ID UNIQUEIDENTIFIER = N''e2569aae-d5da-4a77-b3cd-51adbdb272d9'';
DECLARE @ParentID UNIQUEIDENTIFIER = NULL;
DECLARE @Name NVARCHAR(MAX) = N''MyView1'';
DECLARE @Code NVARCHAR(MAX) = N''CREATE VIEW MyView1 AS
SELECT
    t1.MyColumn1,
    t1.MyColumn4,
    t2.MyColumn2
FROM MyTable1NewName t1
LEFT JOIN MyTable2 t2
    ON t2.MyColumn1NewName = t1.MyColumn1'';
INSERT INTO [DNDBTDbObjects]
(
    [ID],
    [ParentID],
    [Type],
    [Name],
    [Code]
)
VALUES
(
    @ID,
    @ParentID,
    ''View'',
    @Name,
    @Code
);';
-- QUERY END: MSSQLInsertDNDBTDbObjectRecordQuery

-- QUERY START: MSSQLCreateTriggerQuery
EXEC sp_executesql N'CREATE TRIGGER [TR_MyTable2_MyTrigger1]
ON [MyTable2]
AFTER INSERT
AS
BEGIN
    INSERT INTO [MyTable4]([MyColumn1])
    SELECT i.[MyColumn1NewName] FROM inserted i;
END';
-- QUERY END: MSSQLCreateTriggerQuery

-- QUERY START: MSSQLInsertDNDBTDbObjectRecordQuery
EXEC sp_executesql N'DECLARE @ID UNIQUEIDENTIFIER = N''ee64ffc3-5536-4624-beaf-bc3a61d06a1a'';
DECLARE @ParentID UNIQUEIDENTIFIER = N''bfb9030c-a8c3-4882-9c42-1c6ad025cf8f'';
DECLARE @Name NVARCHAR(MAX) = N''TR_MyTable2_MyTrigger1'';
DECLARE @Code NVARCHAR(MAX) = N''CREATE TRIGGER [TR_MyTable2_MyTrigger1]
ON [MyTable2]
AFTER INSERT
AS
BEGIN
    INSERT INTO [MyTable4]([MyColumn1])
    SELECT i.[MyColumn1NewName] FROM inserted i;
END'';
INSERT INTO [DNDBTDbObjects]
(
    [ID],
    [ParentID],
    [Type],
    [Name],
    [Code]
)
VALUES
(
    @ID,
    @ParentID,
    ''Trigger'',
    @Name,
    @Code
);';
-- QUERY END: MSSQLInsertDNDBTDbObjectRecordQuery

-- QUERY START: GenericQuery
EXEC sp_executesql N'IF OBJECT_ID(''_MyTable2'', ''U'') IS NOT NULL
BEGIN
    UPDATE [MyTable2] SET
        [MyColumn2] = [t].[MyColumn2]
    FROM [_MyTable2] AS [t]
    WHERE [MyTable2].[MyColumn1NewName] = [t].[MyColumn1];

    DROP TABLE [_MyTable2];
END';
-- QUERY END: GenericQuery

-- QUERY START: MSSQLInsertDNDBTScriptExecutionRecordQuery
EXEC sp_executesql N'DECLARE @ID UNIQUEIDENTIFIER = N''8ccaf36e-e587-466e-86f7-45c0061ae521'';
DECLARE @Type NVARCHAR(MAX) = N''AfterPublishOnce'';
DECLARE @Name NVARCHAR(MAX) = N''RestoreRecreatedColumnsData'';
DECLARE @Code NVARCHAR(MAX) = N''IF OBJECT_ID(''''_MyTable2'''', ''''U'''') IS NOT NULL
BEGIN
    UPDATE [MyTable2] SET
        [MyColumn2] = [t].[MyColumn2]
    FROM [_MyTable2] AS [t]
    WHERE [MyTable2].[MyColumn1NewName] = [t].[MyColumn1];

    DROP TABLE [_MyTable2];
END'';
DECLARE @MinDbVersionToExecute BIGINT = 1;
DECLARE @MaxDbVersionToExecute BIGINT = 1;
DECLARE @ExecutedOnDbVersion BIGINT = 1;
INSERT INTO [DNDBTScriptExecutions]
(
    [ID],
    [Type],
    [Name],
    [Code],
    [MinDbVersionToExecute],
    [MaxDbVersionToExecute],
    [ExecutedOnDbVersion]
)
VALUES
(
    @ID,
    @Type,
    @Name,
    @Code,
    @MinDbVersionToExecute,
    @MaxDbVersionToExecute,
    @ExecutedOnDbVersion
);';
-- QUERY END: MSSQLInsertDNDBTScriptExecutionRecordQuery

-- QUERY START: MSSQLUpdateDNDBTDbAttributesRecordQuery
EXEC sp_executesql N'DECLARE @Version BIGINT = 2;
UPDATE [DNDBTDbAttributes] SET
    [Version] = @Version;';
-- QUERY END: MSSQLUpdateDNDBTDbAttributesRecordQuery

    COMMIT TRANSACTION;
END TRY
BEGIN CATCH;
    ROLLBACK TRANSACTION;

    DECLARE @ErrorMessage NVARCHAR(MAX), @ErrorSeverity INT, @ErrorState INT;
    SELECT @ErrorMessage = ERROR_MESSAGE() + ' Line ' + CAST(ERROR_LINE() AS NVARCHAR(5)), @ErrorSeverity = ERROR_SEVERITY(), @ErrorState = ERROR_STATE();
    RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
END CATCH;