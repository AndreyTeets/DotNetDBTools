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

-- QUERY START: InsertDNDBTScriptExecutionRecordQuery
EXEC sp_executesql N'INSERT INTO [DNDBTScriptExecutions]
(
    [ID],
    [Type],
    [Name],
    [Text],
    [MinDbVersionToExecute],
    [MaxDbVersionToExecute],
    [ExecutedOnDbVersion]
)
VALUES
(
    N''7f72f0df-4eda-4063-99d8-99c1f37819d2'',
    N''BeforePublishOnce'',
    N''SaveRecreatedColumnsData'',
    N''IF EXISTS (SELECT * FROM [sysobjects] WHERE [name]=''''_MyTable2'''' AND [xtype]=''''U'''')
    DROP TABLE [_MyTable2];

CREATE TABLE [_MyTable2]
(
    [MyColumn1] BIGINT NOT NULL PRIMARY KEY,
    [MyColumn2] VARBINARY(22)
);

INSERT INTO [_MyTable2] ([MyColumn1], [MyColumn2])
SELECT [MyColumn1], [MyColumn2] FROM [MyTable2]'',
    1,
    1,
    1
);';
-- QUERY END: InsertDNDBTScriptExecutionRecordQuery

-- QUERY START: DropTriggerQuery
EXEC sp_executesql N'DROP TRIGGER [TR_MyTable2_MyTrigger1];';
-- QUERY END: DropTriggerQuery

-- QUERY START: DeleteDNDBTDbObjectRecordQuery
EXEC sp_executesql N'DELETE FROM [DNDBTDbObjects]
WHERE [ID] = ''ee64ffc3-5536-4624-beaf-bc3a61d06a1a'';';
-- QUERY END: DeleteDNDBTDbObjectRecordQuery

-- QUERY START: DropForeignKeyQuery
EXEC sp_executesql N'ALTER TABLE [MyTable6] DROP CONSTRAINT [FK_MyTable6_MyTable5_CustomName];';
-- QUERY END: DropForeignKeyQuery

-- QUERY START: DeleteDNDBTDbObjectRecordQuery
EXEC sp_executesql N'DELETE FROM [DNDBTDbObjects]
WHERE [ID] = ''ae453b22-d270-41fc-8184-9ac26b7a0569'';';
-- QUERY END: DeleteDNDBTDbObjectRecordQuery

-- QUERY START: DropForeignKeyQuery
EXEC sp_executesql N'ALTER TABLE [MyTable1] DROP CONSTRAINT [FK_MyTable1_MyColumn1_MyTable2_MyColumn1];';
-- QUERY END: DropForeignKeyQuery

-- QUERY START: DeleteDNDBTDbObjectRecordQuery
EXEC sp_executesql N'DELETE FROM [DNDBTDbObjects]
WHERE [ID] = ''d11b2a53-32db-432f-bb6b-f91788844ba9'';';
-- QUERY END: DeleteDNDBTDbObjectRecordQuery

-- QUERY START: DropIndexQuery
EXEC sp_executesql N'DROP INDEX [IDX_MyTable2_MyIndex1] ON [MyTable2];';
-- QUERY END: DropIndexQuery

-- QUERY START: DeleteDNDBTDbObjectRecordQuery
EXEC sp_executesql N'DELETE FROM [DNDBTDbObjects]
WHERE [ID] = ''74390b3c-bc39-4860-a42e-12baa400f927'';';
-- QUERY END: DeleteDNDBTDbObjectRecordQuery

-- QUERY START: DropIndexQuery
EXEC sp_executesql N'DROP INDEX [IDX_MyTable5_CustomName] ON [MyTable5];';
-- QUERY END: DropIndexQuery

-- QUERY START: DeleteDNDBTDbObjectRecordQuery
EXEC sp_executesql N'DELETE FROM [DNDBTDbObjects]
WHERE [ID] = ''1d632285-9914-4c5d-98e6-a618a99bd799'';';
-- QUERY END: DeleteDNDBTDbObjectRecordQuery

-- QUERY START: DropViewQuery
EXEC sp_executesql N'DROP VIEW [MyView1];';
-- QUERY END: DropViewQuery

-- QUERY START: DeleteDNDBTDbObjectRecordQuery
EXEC sp_executesql N'DELETE FROM [DNDBTDbObjects]
WHERE [ID] = ''e2569aae-d5da-4a77-b3cd-51adbdb272d9'';';
-- QUERY END: DeleteDNDBTDbObjectRecordQuery

-- QUERY START: DropTableQuery
EXEC sp_executesql N'DROP TABLE [MyTable6];';
-- QUERY END: DropTableQuery

-- QUERY START: DeleteDNDBTDbObjectRecordQuery
EXEC sp_executesql N'DELETE FROM [DNDBTDbObjects]
WHERE [ID] = ''bfa08c82-5c8f-4ab4-bd41-1f1d85cf3c85'';';
-- QUERY END: DeleteDNDBTDbObjectRecordQuery

-- QUERY START: DeleteDNDBTDbObjectRecordQuery
EXEC sp_executesql N'DELETE FROM [DNDBTDbObjects]
WHERE [ID] = ''a402e2b7-c826-4cfd-a304-97c9bc346ba2'';';
-- QUERY END: DeleteDNDBTDbObjectRecordQuery

-- QUERY START: DeleteDNDBTDbObjectRecordQuery
EXEC sp_executesql N'DELETE FROM [DNDBTDbObjects]
WHERE [ID] = ''f3064a8c-346a-4b3d-af2c-d967b39841e4'';';
-- QUERY END: DeleteDNDBTDbObjectRecordQuery

-- QUERY START: RenameUserDefinedDataTypeQuery
EXEC sp_executesql N'EXEC sp_rename ''MyUserDefinedType1'', ''_DNDBTTemp_MyUserDefinedType1'', ''USERDATATYPE'';';
-- QUERY END: RenameUserDefinedDataTypeQuery

-- QUERY START: DeleteDNDBTDbObjectRecordQuery
EXEC sp_executesql N'DELETE FROM [DNDBTDbObjects]
WHERE [ID] = ''0cd1e71c-cc9c-440f-ac0b-81a1d6f7ddaa'';';
-- QUERY END: DeleteDNDBTDbObjectRecordQuery

-- QUERY START: CreateTypeQuery
EXEC sp_executesql N'CREATE TYPE [MyUserDefinedType1] FROM NVARCHAR(110);';
-- QUERY END: CreateTypeQuery

-- QUERY START: InsertDNDBTDbObjectRecordQuery
EXEC sp_executesql N'INSERT INTO [DNDBTDbObjects]
(
    [ID],
    [ParentID],
    [Type],
    [Name],
    [Code]
)
VALUES
(
    N''0cd1e71c-cc9c-440f-ac0b-81a1d6f7ddaa'',
    NULL,
    N''UserDefinedType'',
    N''MyUserDefinedType1'',
    NULL
);';
-- QUERY END: InsertDNDBTDbObjectRecordQuery

-- QUERY START: AlterTableQuery
EXEC sp_executesql N'EXEC sp_rename ''MyTable1'', ''MyTable1NewName'';

ALTER TABLE [MyTable1NewName] DROP CONSTRAINT [CK_MyTable1_MyCheck1];
ALTER TABLE [MyTable1NewName] DROP CONSTRAINT [UQ_MyTable1_MyColumn4];
ALTER TABLE [MyTable1NewName] DROP CONSTRAINT [PK_MyTable1];
ALTER TABLE [MyTable1NewName] DROP COLUMN [MyColumn2];
ALTER TABLE [MyTable1NewName] DROP COLUMN [MyColumn3];
ALTER TABLE [MyTable1NewName] DROP CONSTRAINT [DF_MyTable1_MyColumn1];
ALTER TABLE [MyTable1NewName] ALTER COLUMN [MyColumn1] BIGINT NULL;
ALTER TABLE [MyTable1NewName] ADD CONSTRAINT [DF_MyTable1NewName_MyColumn1] DEFAULT 15 FOR [MyColumn1];
ALTER TABLE [MyTable1NewName] DROP CONSTRAINT [DF_MyTable1_MyColumn4];
ALTER TABLE [MyTable1NewName] DROP CONSTRAINT [DF_MyTable1_MyColumn5];
ALTER TABLE [MyTable1NewName] ADD CONSTRAINT [DF_MyTable1NewName_MyColumn5] DEFAULT ''some text'' FOR [MyColumn5];
ALTER TABLE [MyTable1NewName] ADD CONSTRAINT [CK_MyTable1_MyCheck1] CHECK (MyColumn4 >= 1);';
-- QUERY END: AlterTableQuery

-- QUERY START: DeleteDNDBTDbObjectRecordQuery
EXEC sp_executesql N'DELETE FROM [DNDBTDbObjects]
WHERE [ID] = ''eb9c59b5-bc7e-49d7-adaa-f5600b6a19a2'';';
-- QUERY END: DeleteDNDBTDbObjectRecordQuery

-- QUERY START: DeleteDNDBTDbObjectRecordQuery
EXEC sp_executesql N'DELETE FROM [DNDBTDbObjects]
WHERE [ID] = ''f3f08522-26ee-4950-9135-22edf2e4e0cf'';';
-- QUERY END: DeleteDNDBTDbObjectRecordQuery

-- QUERY START: DeleteDNDBTDbObjectRecordQuery
EXEC sp_executesql N'DELETE FROM [DNDBTDbObjects]
WHERE [ID] = ''37a45def-f4a0-4be7-8bfb-8fbed4a7d705'';';
-- QUERY END: DeleteDNDBTDbObjectRecordQuery

-- QUERY START: DeleteDNDBTDbObjectRecordQuery
EXEC sp_executesql N'DELETE FROM [DNDBTDbObjects]
WHERE [ID] = ''fe68ee3d-09d0-40ac-93f9-5e441fbb4f70'';';
-- QUERY END: DeleteDNDBTDbObjectRecordQuery

-- QUERY START: DeleteDNDBTDbObjectRecordQuery
EXEC sp_executesql N'DELETE FROM [DNDBTDbObjects]
WHERE [ID] = ''6e95de30-e01a-4fb4-b8b7-8f0c40bb682c'';';
-- QUERY END: DeleteDNDBTDbObjectRecordQuery

-- QUERY START: UpdateDNDBTDbObjectRecordQuery
EXEC sp_executesql N'UPDATE [DNDBTDbObjects] SET
    [Name] = N''MyTable1NewName''
WHERE [ID] = N''299675e6-4faa-4d0f-a36a-224306ba5bcb'';';
-- QUERY END: UpdateDNDBTDbObjectRecordQuery

-- QUERY START: UpdateDNDBTDbObjectRecordQuery
EXEC sp_executesql N'UPDATE [DNDBTDbObjects] SET
    [Name] = N''MyColumn4'',
    [Code] = NULL
WHERE [ID] = N''867ac528-e87e-4c93-b6e3-dd2fcbbb837f'';';
-- QUERY END: UpdateDNDBTDbObjectRecordQuery

-- QUERY START: InsertDNDBTDbObjectRecordQuery
EXEC sp_executesql N'INSERT INTO [DNDBTDbObjects]
(
    [ID],
    [ParentID],
    [Type],
    [Name],
    [Code]
)
VALUES
(
    N''eb9c59b5-bc7e-49d7-adaa-f5600b6a19a2'',
    N''299675e6-4faa-4d0f-a36a-224306ba5bcb'',
    N''CheckConstraint'',
    N''CK_MyTable1_MyCheck1'',
    N''MyColumn4 >= 1''
);';
-- QUERY END: InsertDNDBTDbObjectRecordQuery

-- QUERY START: AlterTableQuery
EXEC sp_executesql N'EXEC sp_rename ''MyTable2.MyColumn1'', ''MyColumn1NewName'', ''COLUMN'';

ALTER TABLE [MyTable2] DROP CONSTRAINT [PK_MyTable2_CustomName];
ALTER TABLE [MyTable2] DROP CONSTRAINT [DF_MyTable2_MyColumn2];
ALTER TABLE [MyTable2] DROP COLUMN [MyColumn2];
ALTER TABLE [MyTable2] DROP CONSTRAINT [DF_MyTable2_MyColumn1];
ALTER TABLE [MyTable2] ALTER COLUMN [MyColumn1NewName] BIGINT NOT NULL;
ALTER TABLE [MyTable2] ADD CONSTRAINT [DF_MyTable2_MyColumn1NewName] DEFAULT 333 FOR [MyColumn1NewName];
ALTER TABLE [MyTable2] ADD [MyColumn2] VARBINARY(22) NULL CONSTRAINT [DF_MyTable2_MyColumn2] DEFAULT 0x000408 WITH VALUES;
ALTER TABLE [MyTable2] ADD [MyColumn3] BIGINT NULL;
ALTER TABLE [MyTable2] ADD [MyColumn4] NVARCHAR(50) NULL;
ALTER TABLE [MyTable2] ADD CONSTRAINT [PK_MyTable2_CustomName] PRIMARY KEY ([MyColumn1NewName]);';
-- QUERY END: AlterTableQuery

-- QUERY START: DeleteDNDBTDbObjectRecordQuery
EXEC sp_executesql N'DELETE FROM [DNDBTDbObjects]
WHERE [ID] = ''3a43615b-40b3-4a13-99e7-93af7c56e8ce'';';
-- QUERY END: DeleteDNDBTDbObjectRecordQuery

-- QUERY START: DeleteDNDBTDbObjectRecordQuery
EXEC sp_executesql N'DELETE FROM [DNDBTDbObjects]
WHERE [ID] = ''5a0d1926-3270-4eb2-92eb-00be56c7af23'';';
-- QUERY END: DeleteDNDBTDbObjectRecordQuery

-- QUERY START: UpdateDNDBTDbObjectRecordQuery
EXEC sp_executesql N'UPDATE [DNDBTDbObjects] SET
    [Name] = N''MyColumn1NewName''
WHERE [ID] = N''c480f22f-7c01-4f41-b282-35e9f5cd1fe3'';';
-- QUERY END: UpdateDNDBTDbObjectRecordQuery

-- QUERY START: InsertDNDBTDbObjectRecordQuery
EXEC sp_executesql N'INSERT INTO [DNDBTDbObjects]
(
    [ID],
    [ParentID],
    [Type],
    [Name],
    [Code]
)
VALUES
(
    N''c2df19c2-e029-4014-8a5b-4ab42fecb6b8'',
    N''bfb9030c-a8c3-4882-9c42-1c6ad025cf8f'',
    N''Column'',
    N''MyColumn2'',
    N''0x000408''
);';
-- QUERY END: InsertDNDBTDbObjectRecordQuery

-- QUERY START: InsertDNDBTDbObjectRecordQuery
EXEC sp_executesql N'INSERT INTO [DNDBTDbObjects]
(
    [ID],
    [ParentID],
    [Type],
    [Name],
    [Code]
)
VALUES
(
    N''99bc3f49-3151-4f52-87f7-104b424ed7bf'',
    N''bfb9030c-a8c3-4882-9c42-1c6ad025cf8f'',
    N''Column'',
    N''MyColumn3'',
    NULL
);';
-- QUERY END: InsertDNDBTDbObjectRecordQuery

-- QUERY START: InsertDNDBTDbObjectRecordQuery
EXEC sp_executesql N'INSERT INTO [DNDBTDbObjects]
(
    [ID],
    [ParentID],
    [Type],
    [Name],
    [Code]
)
VALUES
(
    N''87950a3f-2072-42db-ac3c-a4e85b79720d'',
    N''bfb9030c-a8c3-4882-9c42-1c6ad025cf8f'',
    N''Column'',
    N''MyColumn4'',
    NULL
);';
-- QUERY END: InsertDNDBTDbObjectRecordQuery

-- QUERY START: InsertDNDBTDbObjectRecordQuery
EXEC sp_executesql N'INSERT INTO [DNDBTDbObjects]
(
    [ID],
    [ParentID],
    [Type],
    [Name],
    [Code]
)
VALUES
(
    N''3a43615b-40b3-4a13-99e7-93af7c56e8ce'',
    N''bfb9030c-a8c3-4882-9c42-1c6ad025cf8f'',
    N''PrimaryKey'',
    N''PK_MyTable2_CustomName'',
    NULL
);';
-- QUERY END: InsertDNDBTDbObjectRecordQuery

-- QUERY START: AlterTableQuery
EXEC sp_executesql N'ALTER TABLE [MyTable5] DROP CONSTRAINT [UQ_MyTable5_CustomName];
ALTER TABLE [MyTable5] DROP CONSTRAINT [PK_MyTable5_CustomName];
ALTER TABLE [MyTable5] DROP CONSTRAINT [DF_MyTable5_MyColumn1];
ALTER TABLE [MyTable5] ADD CONSTRAINT [DF_MyTable5_MyColumn1] DEFAULT abS(-15) FOR [MyColumn1];
ALTER TABLE [MyTable5] DROP CONSTRAINT [DF_MyTable5_MyColumn101];
ALTER TABLE [MyTable5] ALTER COLUMN [MyColumn101] MyUserDefinedType1 NULL;
ALTER TABLE [MyTable5] ADD CONSTRAINT [DF_MyTable5_MyColumn101] DEFAULT ''cc'' FOR [MyColumn101];';
-- QUERY END: AlterTableQuery

-- QUERY START: DeleteDNDBTDbObjectRecordQuery
EXEC sp_executesql N'DELETE FROM [DNDBTDbObjects]
WHERE [ID] = ''5293b58a-9f63-4f0f-8d6f-18416ebbd751'';';
-- QUERY END: DeleteDNDBTDbObjectRecordQuery

-- QUERY START: DeleteDNDBTDbObjectRecordQuery
EXEC sp_executesql N'DELETE FROM [DNDBTDbObjects]
WHERE [ID] = ''79384d48-a39b-4a22-900e-066b2ca67ba2'';';
-- QUERY END: DeleteDNDBTDbObjectRecordQuery

-- QUERY START: UpdateDNDBTDbObjectRecordQuery
EXEC sp_executesql N'UPDATE [DNDBTDbObjects] SET
    [Name] = N''MyColumn1'',
    [Code] = N''abS(-15)''
WHERE [ID] = N''5309d66f-2030-402e-912e-5547babaa072'';';
-- QUERY END: UpdateDNDBTDbObjectRecordQuery

-- QUERY START: DropTypeQuery
EXEC sp_executesql N'DROP TYPE [_DNDBTTemp_MyUserDefinedType1];';
-- QUERY END: DropTypeQuery

-- QUERY START: CreateTableQuery
EXEC sp_executesql N'CREATE TABLE [MyTable3]
(
    [MyColumn1] BIGINT NOT NULL CONSTRAINT [DF_MyTable3_MyColumn1] DEFAULT 444,
    [MyColumn2] NVARCHAR(50) NOT NULL,
    CONSTRAINT [UQ_MyTable3_MyColumns12] UNIQUE ([MyColumn1], [MyColumn2])
);';
-- QUERY END: CreateTableQuery

-- QUERY START: InsertDNDBTDbObjectRecordQuery
EXEC sp_executesql N'INSERT INTO [DNDBTDbObjects]
(
    [ID],
    [ParentID],
    [Type],
    [Name],
    [Code]
)
VALUES
(
    N''474cd761-2522-4529-9d20-2b94115f9626'',
    NULL,
    N''Table'',
    N''MyTable3'',
    NULL
);';
-- QUERY END: InsertDNDBTDbObjectRecordQuery

-- QUERY START: InsertDNDBTDbObjectRecordQuery
EXEC sp_executesql N'INSERT INTO [DNDBTDbObjects]
(
    [ID],
    [ParentID],
    [Type],
    [Name],
    [Code]
)
VALUES
(
    N''726f503a-d944-46ee-a0ff-6a2c2faab46e'',
    N''474cd761-2522-4529-9d20-2b94115f9626'',
    N''Column'',
    N''MyColumn1'',
    N''444''
);';
-- QUERY END: InsertDNDBTDbObjectRecordQuery

-- QUERY START: InsertDNDBTDbObjectRecordQuery
EXEC sp_executesql N'INSERT INTO [DNDBTDbObjects]
(
    [ID],
    [ParentID],
    [Type],
    [Name],
    [Code]
)
VALUES
(
    N''169824e1-8b74-4b60-af17-99656d6dbbee'',
    N''474cd761-2522-4529-9d20-2b94115f9626'',
    N''Column'',
    N''MyColumn2'',
    NULL
);';
-- QUERY END: InsertDNDBTDbObjectRecordQuery

-- QUERY START: InsertDNDBTDbObjectRecordQuery
EXEC sp_executesql N'INSERT INTO [DNDBTDbObjects]
(
    [ID],
    [ParentID],
    [Type],
    [Name],
    [Code]
)
VALUES
(
    N''fd288e38-35ba-4bb1-ace3-597c99ef26c7'',
    N''474cd761-2522-4529-9d20-2b94115f9626'',
    N''UniqueConstraint'',
    N''UQ_MyTable3_MyColumns12'',
    NULL
);';
-- QUERY END: InsertDNDBTDbObjectRecordQuery

-- QUERY START: CreateViewQuery
EXEC sp_executesql N'CREATE VIEW MyView1 AS
SELECT
    t1.MyColumn1,
    t1.MyColumn4,
    t2.MyColumn2
FROM MyTable1NewName t1
LEFT JOIN MyTable2 t2
    ON t2.MyColumn1NewName = t1.MyColumn1';
-- QUERY END: CreateViewQuery

-- QUERY START: InsertDNDBTDbObjectRecordQuery
EXEC sp_executesql N'INSERT INTO [DNDBTDbObjects]
(
    [ID],
    [ParentID],
    [Type],
    [Name],
    [Code]
)
VALUES
(
    N''e2569aae-d5da-4a77-b3cd-51adbdb272d9'',
    NULL,
    N''View'',
    N''MyView1'',
    N''CREATE VIEW MyView1 AS
SELECT
    t1.MyColumn1,
    t1.MyColumn4,
    t2.MyColumn2
FROM MyTable1NewName t1
LEFT JOIN MyTable2 t2
    ON t2.MyColumn1NewName = t1.MyColumn1''
);';
-- QUERY END: InsertDNDBTDbObjectRecordQuery

-- QUERY START: CreateIndexQuery
EXEC sp_executesql N'CREATE UNIQUE INDEX [IDX_MyTable2_MyIndex1]
    ON [MyTable2] ([MyColumn1NewName], [MyColumn2]);';
-- QUERY END: CreateIndexQuery

-- QUERY START: InsertDNDBTDbObjectRecordQuery
EXEC sp_executesql N'INSERT INTO [DNDBTDbObjects]
(
    [ID],
    [ParentID],
    [Type],
    [Name],
    [Code]
)
VALUES
(
    N''74390b3c-bc39-4860-a42e-12baa400f927'',
    N''bfb9030c-a8c3-4882-9c42-1c6ad025cf8f'',
    N''Index'',
    N''IDX_MyTable2_MyIndex1'',
    NULL
);';
-- QUERY END: InsertDNDBTDbObjectRecordQuery

-- QUERY START: CreateForeignKeyQuery
EXEC sp_executesql N'ALTER TABLE [MyTable1NewName] ADD CONSTRAINT [FK_MyTable1_MyColumn1_MyTable2_MyColumn1] FOREIGN KEY ([MyColumn1])
        REFERENCES [MyTable2] ([MyColumn1NewName])
        ON UPDATE NO ACTION ON DELETE SET NULL;';
-- QUERY END: CreateForeignKeyQuery

-- QUERY START: InsertDNDBTDbObjectRecordQuery
EXEC sp_executesql N'INSERT INTO [DNDBTDbObjects]
(
    [ID],
    [ParentID],
    [Type],
    [Name],
    [Code]
)
VALUES
(
    N''d11b2a53-32db-432f-bb6b-f91788844ba9'',
    N''299675e6-4faa-4d0f-a36a-224306ba5bcb'',
    N''ForeignKey'',
    N''FK_MyTable1_MyColumn1_MyTable2_MyColumn1'',
    NULL
);';
-- QUERY END: InsertDNDBTDbObjectRecordQuery

-- QUERY START: CreateForeignKeyQuery
EXEC sp_executesql N'ALTER TABLE [MyTable2] ADD CONSTRAINT [FK_MyTable2_MyColumns34_MyTable3_MyColumns12] FOREIGN KEY ([MyColumn3], [MyColumn4])
        REFERENCES [MyTable3] ([MyColumn1], [MyColumn2])
        ON UPDATE NO ACTION ON DELETE SET DEFAULT;';
-- QUERY END: CreateForeignKeyQuery

-- QUERY START: InsertDNDBTDbObjectRecordQuery
EXEC sp_executesql N'INSERT INTO [DNDBTDbObjects]
(
    [ID],
    [ParentID],
    [Type],
    [Name],
    [Code]
)
VALUES
(
    N''480f3508-9d51-4190-88aa-45bc20e49119'',
    N''bfb9030c-a8c3-4882-9c42-1c6ad025cf8f'',
    N''ForeignKey'',
    N''FK_MyTable2_MyColumns34_MyTable3_MyColumns12'',
    NULL
);';
-- QUERY END: InsertDNDBTDbObjectRecordQuery

-- QUERY START: CreateTriggerQuery
EXEC sp_executesql N'CREATE TRIGGER [TR_MyTable2_MyTrigger1]
ON [MyTable2]
AFTER INSERT
AS
BEGIN
    INSERT INTO [MyTable4]([MyColumn1])
    SELECT i.[MyColumn1NewName] FROM inserted i;
END';
-- QUERY END: CreateTriggerQuery

-- QUERY START: InsertDNDBTDbObjectRecordQuery
EXEC sp_executesql N'INSERT INTO [DNDBTDbObjects]
(
    [ID],
    [ParentID],
    [Type],
    [Name],
    [Code]
)
VALUES
(
    N''ee64ffc3-5536-4624-beaf-bc3a61d06a1a'',
    N''bfb9030c-a8c3-4882-9c42-1c6ad025cf8f'',
    N''Trigger'',
    N''TR_MyTable2_MyTrigger1'',
    N''CREATE TRIGGER [TR_MyTable2_MyTrigger1]
ON [MyTable2]
AFTER INSERT
AS
BEGIN
    INSERT INTO [MyTable4]([MyColumn1])
    SELECT i.[MyColumn1NewName] FROM inserted i;
END''
);';
-- QUERY END: InsertDNDBTDbObjectRecordQuery

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

-- QUERY START: InsertDNDBTScriptExecutionRecordQuery
EXEC sp_executesql N'INSERT INTO [DNDBTScriptExecutions]
(
    [ID],
    [Type],
    [Name],
    [Text],
    [MinDbVersionToExecute],
    [MaxDbVersionToExecute],
    [ExecutedOnDbVersion]
)
VALUES
(
    N''8ccaf36e-e587-466e-86f7-45c0061ae521'',
    N''AfterPublishOnce'',
    N''RestoreRecreatedColumnsData'',
    N''IF OBJECT_ID(''''_MyTable2'''', ''''U'''') IS NOT NULL
BEGIN
    UPDATE [MyTable2] SET
        [MyColumn2] = [t].[MyColumn2]
    FROM [_MyTable2] AS [t]
    WHERE [MyTable2].[MyColumn1NewName] = [t].[MyColumn1];

    DROP TABLE [_MyTable2];
END'',
    1,
    1,
    1
);';
-- QUERY END: InsertDNDBTScriptExecutionRecordQuery

-- QUERY START: UpdateDNDBTDbAttributesRecordQuery
EXEC sp_executesql N'UPDATE [DNDBTDbAttributes] SET
    [Version] = 2;';
-- QUERY END: UpdateDNDBTDbAttributesRecordQuery

    COMMIT TRANSACTION;
END TRY
BEGIN CATCH;
    ROLLBACK TRANSACTION;

    DECLARE @ErrorMessage NVARCHAR(MAX), @ErrorSeverity INT, @ErrorState INT;
    SELECT @ErrorMessage = ERROR_MESSAGE() + ' Line ' + CAST(ERROR_LINE() AS NVARCHAR(5)), @ErrorSeverity = ERROR_SEVERITY(), @ErrorState = ERROR_STATE();
    RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
END CATCH;