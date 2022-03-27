SET NOCOUNT ON;
SET XACT_ABORT ON;
BEGIN TRY;
    BEGIN TRANSACTION;

-- QUERY START: MSSQLCreateTypeQuery
EXEC sp_executesql N'CREATE TYPE MyUserDefinedType1 FROM NVARCHAR(100);';
-- QUERY END: MSSQLCreateTypeQuery

-- QUERY START: MSSQLInsertDNDBTDbObjectRecordQuery
EXEC sp_executesql N'DECLARE @ID UNIQUEIDENTIFIER = N''0cd1e71c-cc9c-440f-ac0b-81a1d6f7ddaa'';
DECLARE @ParentID UNIQUEIDENTIFIER = NULL;
DECLARE @Name NVARCHAR(MAX) = N''MyUserDefinedType1'';
DECLARE @Code NVARCHAR(MAX) = NULL;
INSERT INTO DNDBTDbObjects
(
    ID,
    ParentID,
    Type,
    Name,
    Code
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

-- QUERY START: MSSQLCreateTableQuery
EXEC sp_executesql N'CREATE TABLE MyTable1
(
    MyColumn1 INT NOT NULL CONSTRAINT DF_MyTable1_MyColumn1 DEFAULT 15,
    MyColumn2 NVARCHAR(10) NOT NULL CONSTRAINT DF_MyTable1_MyColumn2 DEFAULT ''33'',
    MyColumn3 INT IDENTITY NOT NULL,
    MyColumn4 DECIMAL(19, 2) NOT NULL CONSTRAINT DF_MyTable1_MyColumn4 DEFAULT 7.36,
    CONSTRAINT PK_MyTable1 PRIMARY KEY (MyColumn3),
    CONSTRAINT UQ_MyTable1_MyColumn2 UNIQUE (MyColumn2),
    CONSTRAINT [CK_MyTable1_MyCheck1] CHECK (MyColumn4 >= 0)
);';
-- QUERY END: MSSQLCreateTableQuery

-- QUERY START: MSSQLInsertDNDBTDbObjectRecordQuery
EXEC sp_executesql N'DECLARE @ID UNIQUEIDENTIFIER = N''299675e6-4faa-4d0f-a36a-224306ba5bcb'';
DECLARE @ParentID UNIQUEIDENTIFIER = NULL;
DECLARE @Name NVARCHAR(MAX) = N''MyTable1'';
DECLARE @Code NVARCHAR(MAX) = NULL;
INSERT INTO DNDBTDbObjects
(
    ID,
    ParentID,
    Type,
    Name,
    Code
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
EXEC sp_executesql N'DECLARE @ID UNIQUEIDENTIFIER = N''a2f2a4de-1337-4594-ae41-72ed4d05f317'';
DECLARE @ParentID UNIQUEIDENTIFIER = N''299675e6-4faa-4d0f-a36a-224306ba5bcb'';
DECLARE @Name NVARCHAR(MAX) = N''MyColumn1'';
DECLARE @Code NVARCHAR(MAX) = N''15'';
INSERT INTO DNDBTDbObjects
(
    ID,
    ParentID,
    Type,
    Name,
    Code
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
EXEC sp_executesql N'DECLARE @ID UNIQUEIDENTIFIER = N''fe68ee3d-09d0-40ac-93f9-5e441fbb4f70'';
DECLARE @ParentID UNIQUEIDENTIFIER = N''299675e6-4faa-4d0f-a36a-224306ba5bcb'';
DECLARE @Name NVARCHAR(MAX) = N''MyColumn2'';
DECLARE @Code NVARCHAR(MAX) = N''''''33'''''';
INSERT INTO DNDBTDbObjects
(
    ID,
    ParentID,
    Type,
    Name,
    Code
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
EXEC sp_executesql N'DECLARE @ID UNIQUEIDENTIFIER = N''6e95de30-e01a-4fb4-b8b7-8f0c40bb682c'';
DECLARE @ParentID UNIQUEIDENTIFIER = N''299675e6-4faa-4d0f-a36a-224306ba5bcb'';
DECLARE @Name NVARCHAR(MAX) = N''MyColumn3'';
DECLARE @Code NVARCHAR(MAX) = NULL;
INSERT INTO DNDBTDbObjects
(
    ID,
    ParentID,
    Type,
    Name,
    Code
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
EXEC sp_executesql N'DECLARE @ID UNIQUEIDENTIFIER = N''867ac528-e87e-4c93-b6e3-dd2fcbbb837f'';
DECLARE @ParentID UNIQUEIDENTIFIER = N''299675e6-4faa-4d0f-a36a-224306ba5bcb'';
DECLARE @Name NVARCHAR(MAX) = N''MyColumn4'';
DECLARE @Code NVARCHAR(MAX) = N''7.36'';
INSERT INTO DNDBTDbObjects
(
    ID,
    ParentID,
    Type,
    Name,
    Code
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
EXEC sp_executesql N'DECLARE @ID UNIQUEIDENTIFIER = N''37a45def-f4a0-4be7-8bfb-8fbed4a7d705'';
DECLARE @ParentID UNIQUEIDENTIFIER = N''299675e6-4faa-4d0f-a36a-224306ba5bcb'';
DECLARE @Name NVARCHAR(MAX) = N''PK_MyTable1'';
DECLARE @Code NVARCHAR(MAX) = NULL;
INSERT INTO DNDBTDbObjects
(
    ID,
    ParentID,
    Type,
    Name,
    Code
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

-- QUERY START: MSSQLInsertDNDBTDbObjectRecordQuery
EXEC sp_executesql N'DECLARE @ID UNIQUEIDENTIFIER = N''f3f08522-26ee-4950-9135-22edf2e4e0cf'';
DECLARE @ParentID UNIQUEIDENTIFIER = N''299675e6-4faa-4d0f-a36a-224306ba5bcb'';
DECLARE @Name NVARCHAR(MAX) = N''UQ_MyTable1_MyColumn2'';
DECLARE @Code NVARCHAR(MAX) = NULL;
INSERT INTO DNDBTDbObjects
(
    ID,
    ParentID,
    Type,
    Name,
    Code
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

-- QUERY START: MSSQLInsertDNDBTDbObjectRecordQuery
EXEC sp_executesql N'DECLARE @ID UNIQUEIDENTIFIER = N''eb9c59b5-bc7e-49d7-adaa-f5600b6a19a2'';
DECLARE @ParentID UNIQUEIDENTIFIER = N''299675e6-4faa-4d0f-a36a-224306ba5bcb'';
DECLARE @Name NVARCHAR(MAX) = N''CK_MyTable1_MyCheck1'';
DECLARE @Code NVARCHAR(MAX) = N''CHECK (MyColumn4 >= 0)'';
INSERT INTO DNDBTDbObjects
(
    ID,
    ParentID,
    Type,
    Name,
    Code
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

-- QUERY START: MSSQLCreateTableQuery
EXEC sp_executesql N'CREATE TABLE MyTable2
(
    MyColumn1 INT NOT NULL CONSTRAINT DF_MyTable2_MyColumn1 DEFAULT 333,
    MyColumn2 VARBINARY(22) NULL CONSTRAINT DF_MyTable2_MyColumn2 DEFAULT 0x000102,
    CONSTRAINT PK_MyTable2 PRIMARY KEY (MyColumn1)
);';
-- QUERY END: MSSQLCreateTableQuery

-- QUERY START: MSSQLInsertDNDBTDbObjectRecordQuery
EXEC sp_executesql N'DECLARE @ID UNIQUEIDENTIFIER = N''bfb9030c-a8c3-4882-9c42-1c6ad025cf8f'';
DECLARE @ParentID UNIQUEIDENTIFIER = NULL;
DECLARE @Name NVARCHAR(MAX) = N''MyTable2'';
DECLARE @Code NVARCHAR(MAX) = NULL;
INSERT INTO DNDBTDbObjects
(
    ID,
    ParentID,
    Type,
    Name,
    Code
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
EXEC sp_executesql N'DECLARE @ID UNIQUEIDENTIFIER = N''c480f22f-7c01-4f41-b282-35e9f5cd1fe3'';
DECLARE @ParentID UNIQUEIDENTIFIER = N''bfb9030c-a8c3-4882-9c42-1c6ad025cf8f'';
DECLARE @Name NVARCHAR(MAX) = N''MyColumn1'';
DECLARE @Code NVARCHAR(MAX) = N''333'';
INSERT INTO DNDBTDbObjects
(
    ID,
    ParentID,
    Type,
    Name,
    Code
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
EXEC sp_executesql N'DECLARE @ID UNIQUEIDENTIFIER = N''5a0d1926-3270-4eb2-92eb-00be56c7af23'';
DECLARE @ParentID UNIQUEIDENTIFIER = N''bfb9030c-a8c3-4882-9c42-1c6ad025cf8f'';
DECLARE @Name NVARCHAR(MAX) = N''MyColumn2'';
DECLARE @Code NVARCHAR(MAX) = N''0x000102'';
INSERT INTO DNDBTDbObjects
(
    ID,
    ParentID,
    Type,
    Name,
    Code
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
DECLARE @Name NVARCHAR(MAX) = N''PK_MyTable2'';
DECLARE @Code NVARCHAR(MAX) = NULL;
INSERT INTO DNDBTDbObjects
(
    ID,
    ParentID,
    Type,
    Name,
    Code
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

-- QUERY START: MSSQLCreateTableQuery
EXEC sp_executesql N'CREATE TABLE MyTable4
(
    MyColumn1 BIGINT NOT NULL
);';
-- QUERY END: MSSQLCreateTableQuery

-- QUERY START: MSSQLInsertDNDBTDbObjectRecordQuery
EXEC sp_executesql N'DECLARE @ID UNIQUEIDENTIFIER = N''b12a6a37-7739-48e0-a9e1-499ae7d2a395'';
DECLARE @ParentID UNIQUEIDENTIFIER = NULL;
DECLARE @Name NVARCHAR(MAX) = N''MyTable4'';
DECLARE @Code NVARCHAR(MAX) = NULL;
INSERT INTO DNDBTDbObjects
(
    ID,
    ParentID,
    Type,
    Name,
    Code
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
EXEC sp_executesql N'DECLARE @ID UNIQUEIDENTIFIER = N''de0425b8-9f99-4d76-9a64-09e52f8b5d5a'';
DECLARE @ParentID UNIQUEIDENTIFIER = N''b12a6a37-7739-48e0-a9e1-499ae7d2a395'';
DECLARE @Name NVARCHAR(MAX) = N''MyColumn1'';
DECLARE @Code NVARCHAR(MAX) = NULL;
INSERT INTO DNDBTDbObjects
(
    ID,
    ParentID,
    Type,
    Name,
    Code
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

-- QUERY START: MSSQLCreateTableQuery
EXEC sp_executesql N'CREATE TABLE MyTable5
(
    MyColumn1 INT NOT NULL CONSTRAINT DF_MyTable5_MyColumn1 DEFAULT ABS(-15),
    MyColumn2 MyUserDefinedType1 NULL CONSTRAINT DF_MyTable5_MyColumn2 DEFAULT ''cc'',
    MyColumn3 DATETIME2 NULL
);';
-- QUERY END: MSSQLCreateTableQuery

-- QUERY START: MSSQLInsertDNDBTDbObjectRecordQuery
EXEC sp_executesql N'DECLARE @ID UNIQUEIDENTIFIER = N''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'';
DECLARE @ParentID UNIQUEIDENTIFIER = NULL;
DECLARE @Name NVARCHAR(MAX) = N''MyTable5'';
DECLARE @Code NVARCHAR(MAX) = NULL;
INSERT INTO DNDBTDbObjects
(
    ID,
    ParentID,
    Type,
    Name,
    Code
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
EXEC sp_executesql N'DECLARE @ID UNIQUEIDENTIFIER = N''5309d66f-2030-402e-912e-5547babaa072'';
DECLARE @ParentID UNIQUEIDENTIFIER = N''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'';
DECLARE @Name NVARCHAR(MAX) = N''MyColumn1'';
DECLARE @Code NVARCHAR(MAX) = N''ABS(-15)'';
INSERT INTO DNDBTDbObjects
(
    ID,
    ParentID,
    Type,
    Name,
    Code
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
EXEC sp_executesql N'DECLARE @ID UNIQUEIDENTIFIER = N''15ae6061-426d-4485-85e6-ecd3e0f98882'';
DECLARE @ParentID UNIQUEIDENTIFIER = N''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'';
DECLARE @Name NVARCHAR(MAX) = N''MyColumn2'';
DECLARE @Code NVARCHAR(MAX) = N''''''cc'''''';
INSERT INTO DNDBTDbObjects
(
    ID,
    ParentID,
    Type,
    Name,
    Code
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
EXEC sp_executesql N'DECLARE @ID UNIQUEIDENTIFIER = N''4dde852d-ec19-4b61-80f9-da428d8ff41a'';
DECLARE @ParentID UNIQUEIDENTIFIER = N''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'';
DECLARE @Name NVARCHAR(MAX) = N''MyColumn3'';
DECLARE @Code NVARCHAR(MAX) = NULL;
INSERT INTO DNDBTDbObjects
(
    ID,
    ParentID,
    Type,
    Name,
    Code
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

-- QUERY START: MSSQLCreateIndexQuery
EXEC sp_executesql N'CREATE UNIQUE INDEX IDX_MyTable2_MyIndex1
ON MyTable2 (MyColumn1, MyColumn2);';
-- QUERY END: MSSQLCreateIndexQuery

-- QUERY START: MSSQLInsertDNDBTDbObjectRecordQuery
EXEC sp_executesql N'DECLARE @ID UNIQUEIDENTIFIER = N''74390b3c-bc39-4860-a42e-12baa400f927'';
DECLARE @ParentID UNIQUEIDENTIFIER = N''bfb9030c-a8c3-4882-9c42-1c6ad025cf8f'';
DECLARE @Name NVARCHAR(MAX) = N''IDX_MyTable2_MyIndex1'';
DECLARE @Code NVARCHAR(MAX) = NULL;
INSERT INTO DNDBTDbObjects
(
    ID,
    ParentID,
    Type,
    Name,
    Code
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
EXEC sp_executesql N'ALTER TABLE MyTable1 ADD CONSTRAINT FK_MyTable1_MyColumn1_MyTable2_MyColumn1 FOREIGN KEY (MyColumn1)
    REFERENCES MyTable2 (MyColumn1)
    ON UPDATE NO ACTION ON DELETE CASCADE;';
-- QUERY END: MSSQLCreateForeignKeyQuery

-- QUERY START: MSSQLInsertDNDBTDbObjectRecordQuery
EXEC sp_executesql N'DECLARE @ID UNIQUEIDENTIFIER = N''d11b2a53-32db-432f-bb6b-f91788844ba9'';
DECLARE @ParentID UNIQUEIDENTIFIER = N''299675e6-4faa-4d0f-a36a-224306ba5bcb'';
DECLARE @Name NVARCHAR(MAX) = N''FK_MyTable1_MyColumn1_MyTable2_MyColumn1'';
DECLARE @Code NVARCHAR(MAX) = NULL;
INSERT INTO DNDBTDbObjects
(
    ID,
    ParentID,
    Type,
    Name,
    Code
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
FROM MyTable1 t1
LEFT JOIN MyTable2 t2
    ON t2.MyColumn1 = t1.MyColumn1;';
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
FROM MyTable1 t1
LEFT JOIN MyTable2 t2
    ON t2.MyColumn1 = t1.MyColumn1;'';
INSERT INTO DNDBTDbObjects
(
    ID,
    ParentID,
    Type,
    Name,
    Code
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
    SELECT i.[MyColumn1] FROM inserted i;
END;';
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
    SELECT i.[MyColumn1] FROM inserted i;
END;'';
INSERT INTO DNDBTDbObjects
(
    ID,
    ParentID,
    Type,
    Name,
    Code
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
EXEC sp_executesql N'INSERT INTO [MyTable4]([MyColumn1])
SELECT * FROM
(
    SELECT 1
    UNION ALL
    SELECT 2
    UNION ALL
    SELECT 3
) t(Col1)
WHERE NOT EXISTS (SELECT COUNT(*) FROM [MyTable4]);';
-- QUERY END: GenericQuery

-- QUERY START: MSSQLInsertDNDBTScriptExecutionRecordQuery
EXEC sp_executesql N'DECLARE @ID UNIQUEIDENTIFIER = N''100d624a-01aa-4730-b86f-f991ac3ed936'';
DECLARE @Type NVARCHAR(MAX) = N''AfterPublishOnce'';
DECLARE @Name NVARCHAR(MAX) = N''InsertSomeInitialData'';
DECLARE @Code NVARCHAR(MAX) = N''INSERT INTO [MyTable4]([MyColumn1])
SELECT * FROM
(
    SELECT 1
    UNION ALL
    SELECT 2
    UNION ALL
    SELECT 3
) t(Col1)
WHERE NOT EXISTS (SELECT COUNT(*) FROM [MyTable4]);'';
DECLARE @MinDbVersionToExecute BIGINT = 0;
DECLARE @MaxDbVersionToExecute BIGINT = 9223372036854775807;
DECLARE @ExecutedOnDbVersion BIGINT = 0;
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
EXEC sp_executesql N'DECLARE @Version BIGINT = 1;
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