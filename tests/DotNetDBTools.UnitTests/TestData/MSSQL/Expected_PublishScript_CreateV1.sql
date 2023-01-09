SET NOCOUNT ON;
SET XACT_ABORT ON;
BEGIN TRY;
    BEGIN TRANSACTION;

-- QUERY START: CreateTypeQuery
EXEC sp_executesql N'CREATE TYPE [MyUserDefinedType1] FROM NVARCHAR(100);';
-- QUERY END: CreateTypeQuery

-- QUERY START: InsertDNDBTDbObjectRecordQuery
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
-- QUERY END: InsertDNDBTDbObjectRecordQuery

-- QUERY START: CreateTableQuery
EXEC sp_executesql N'CREATE TABLE [MyTable1]
(
    [MyColumn1] INT NOT NULL CONSTRAINT [DF_MyTable1_MyColumn1] DEFAULT 15,
    [MyColumn2] NVARCHAR(MAX) NULL,
    [MyColumn3] INT IDENTITY NOT NULL,
    [MyColumn4] DECIMAL(18,0) NOT NULL CONSTRAINT [DF_MyTable1_MyColumn4] DEFAULT 736,
    [MyColumn5] VARCHAR(1000) NULL CONSTRAINT [DF_MyTable1_MyColumn5] DEFAULT ''some text'',
    CONSTRAINT [PK_MyTable1] PRIMARY KEY ([MyColumn3]),
    CONSTRAINT [UQ_MyTable1_MyColumn4] UNIQUE ([MyColumn4]),
    CONSTRAINT [CK_MyTable1_MyCheck1] CHECK (MyColumn4 >= 0)
);';
-- QUERY END: CreateTableQuery

-- QUERY START: InsertDNDBTDbObjectRecordQuery
EXEC sp_executesql N'DECLARE @ID UNIQUEIDENTIFIER = N''299675e6-4faa-4d0f-a36a-224306ba5bcb'';
DECLARE @ParentID UNIQUEIDENTIFIER = NULL;
DECLARE @Name NVARCHAR(MAX) = N''MyTable1'';
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
-- QUERY END: InsertDNDBTDbObjectRecordQuery

-- QUERY START: InsertDNDBTDbObjectRecordQuery
EXEC sp_executesql N'DECLARE @ID UNIQUEIDENTIFIER = N''a2f2a4de-1337-4594-ae41-72ed4d05f317'';
DECLARE @ParentID UNIQUEIDENTIFIER = N''299675e6-4faa-4d0f-a36a-224306ba5bcb'';
DECLARE @Name NVARCHAR(MAX) = N''MyColumn1'';
DECLARE @Code NVARCHAR(MAX) = N''15'';
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
-- QUERY END: InsertDNDBTDbObjectRecordQuery

-- QUERY START: InsertDNDBTDbObjectRecordQuery
EXEC sp_executesql N'DECLARE @ID UNIQUEIDENTIFIER = N''fe68ee3d-09d0-40ac-93f9-5e441fbb4f70'';
DECLARE @ParentID UNIQUEIDENTIFIER = N''299675e6-4faa-4d0f-a36a-224306ba5bcb'';
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
-- QUERY END: InsertDNDBTDbObjectRecordQuery

-- QUERY START: InsertDNDBTDbObjectRecordQuery
EXEC sp_executesql N'DECLARE @ID UNIQUEIDENTIFIER = N''6e95de30-e01a-4fb4-b8b7-8f0c40bb682c'';
DECLARE @ParentID UNIQUEIDENTIFIER = N''299675e6-4faa-4d0f-a36a-224306ba5bcb'';
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
-- QUERY END: InsertDNDBTDbObjectRecordQuery

-- QUERY START: InsertDNDBTDbObjectRecordQuery
EXEC sp_executesql N'DECLARE @ID UNIQUEIDENTIFIER = N''867ac528-e87e-4c93-b6e3-dd2fcbbb837f'';
DECLARE @ParentID UNIQUEIDENTIFIER = N''299675e6-4faa-4d0f-a36a-224306ba5bcb'';
DECLARE @Name NVARCHAR(MAX) = N''MyColumn4'';
DECLARE @Code NVARCHAR(MAX) = N''736'';
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
-- QUERY END: InsertDNDBTDbObjectRecordQuery

-- QUERY START: InsertDNDBTDbObjectRecordQuery
EXEC sp_executesql N'DECLARE @ID UNIQUEIDENTIFIER = N''ebbef06c-c7de-4b36-a911-827566639630'';
DECLARE @ParentID UNIQUEIDENTIFIER = N''299675e6-4faa-4d0f-a36a-224306ba5bcb'';
DECLARE @Name NVARCHAR(MAX) = N''MyColumn5'';
DECLARE @Code NVARCHAR(MAX) = N''''''some text'''''';
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
-- QUERY END: InsertDNDBTDbObjectRecordQuery

-- QUERY START: InsertDNDBTDbObjectRecordQuery
EXEC sp_executesql N'DECLARE @ID UNIQUEIDENTIFIER = N''37a45def-f4a0-4be7-8bfb-8fbed4a7d705'';
DECLARE @ParentID UNIQUEIDENTIFIER = N''299675e6-4faa-4d0f-a36a-224306ba5bcb'';
DECLARE @Name NVARCHAR(MAX) = N''PK_MyTable1'';
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
-- QUERY END: InsertDNDBTDbObjectRecordQuery

-- QUERY START: InsertDNDBTDbObjectRecordQuery
EXEC sp_executesql N'DECLARE @ID UNIQUEIDENTIFIER = N''f3f08522-26ee-4950-9135-22edf2e4e0cf'';
DECLARE @ParentID UNIQUEIDENTIFIER = N''299675e6-4faa-4d0f-a36a-224306ba5bcb'';
DECLARE @Name NVARCHAR(MAX) = N''UQ_MyTable1_MyColumn4'';
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
-- QUERY END: InsertDNDBTDbObjectRecordQuery

-- QUERY START: InsertDNDBTDbObjectRecordQuery
EXEC sp_executesql N'DECLARE @ID UNIQUEIDENTIFIER = N''eb9c59b5-bc7e-49d7-adaa-f5600b6a19a2'';
DECLARE @ParentID UNIQUEIDENTIFIER = N''299675e6-4faa-4d0f-a36a-224306ba5bcb'';
DECLARE @Name NVARCHAR(MAX) = N''CK_MyTable1_MyCheck1'';
DECLARE @Code NVARCHAR(MAX) = N''MyColumn4 >= 0'';
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
-- QUERY END: InsertDNDBTDbObjectRecordQuery

-- QUERY START: CreateTableQuery
EXEC sp_executesql N'CREATE TABLE [MyTable2]
(
    [MyColumn1] INT NOT NULL CONSTRAINT [DF_MyTable2_MyColumn1] DEFAULT 333,
    [MyColumn2] VARBINARY(22) NULL CONSTRAINT [DF_MyTable2_MyColumn2] DEFAULT 0x000408,
    CONSTRAINT [PK_MyTable2_CustomName] PRIMARY KEY ([MyColumn1])
);';
-- QUERY END: CreateTableQuery

-- QUERY START: InsertDNDBTDbObjectRecordQuery
EXEC sp_executesql N'DECLARE @ID UNIQUEIDENTIFIER = N''bfb9030c-a8c3-4882-9c42-1c6ad025cf8f'';
DECLARE @ParentID UNIQUEIDENTIFIER = NULL;
DECLARE @Name NVARCHAR(MAX) = N''MyTable2'';
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
-- QUERY END: InsertDNDBTDbObjectRecordQuery

-- QUERY START: InsertDNDBTDbObjectRecordQuery
EXEC sp_executesql N'DECLARE @ID UNIQUEIDENTIFIER = N''c480f22f-7c01-4f41-b282-35e9f5cd1fe3'';
DECLARE @ParentID UNIQUEIDENTIFIER = N''bfb9030c-a8c3-4882-9c42-1c6ad025cf8f'';
DECLARE @Name NVARCHAR(MAX) = N''MyColumn1'';
DECLARE @Code NVARCHAR(MAX) = N''333'';
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
-- QUERY END: InsertDNDBTDbObjectRecordQuery

-- QUERY START: InsertDNDBTDbObjectRecordQuery
EXEC sp_executesql N'DECLARE @ID UNIQUEIDENTIFIER = N''5a0d1926-3270-4eb2-92eb-00be56c7af23'';
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
-- QUERY END: InsertDNDBTDbObjectRecordQuery

-- QUERY START: InsertDNDBTDbObjectRecordQuery
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
-- QUERY END: InsertDNDBTDbObjectRecordQuery

-- QUERY START: CreateTableQuery
EXEC sp_executesql N'CREATE TABLE [MyTable4]
(
    [MyColumn1] BIGINT NOT NULL,
    [MyColumn2] INT IDENTITY NOT NULL,
    CONSTRAINT [PK_MyTable4] PRIMARY KEY ([MyColumn2])
);';
-- QUERY END: CreateTableQuery

-- QUERY START: InsertDNDBTDbObjectRecordQuery
EXEC sp_executesql N'DECLARE @ID UNIQUEIDENTIFIER = N''b12a6a37-7739-48e0-a9e1-499ae7d2a395'';
DECLARE @ParentID UNIQUEIDENTIFIER = NULL;
DECLARE @Name NVARCHAR(MAX) = N''MyTable4'';
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
-- QUERY END: InsertDNDBTDbObjectRecordQuery

-- QUERY START: InsertDNDBTDbObjectRecordQuery
EXEC sp_executesql N'DECLARE @ID UNIQUEIDENTIFIER = N''de0425b8-9f99-4d76-9a64-09e52f8b5d5a'';
DECLARE @ParentID UNIQUEIDENTIFIER = N''b12a6a37-7739-48e0-a9e1-499ae7d2a395'';
DECLARE @Name NVARCHAR(MAX) = N''MyColumn1'';
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
-- QUERY END: InsertDNDBTDbObjectRecordQuery

-- QUERY START: InsertDNDBTDbObjectRecordQuery
EXEC sp_executesql N'DECLARE @ID UNIQUEIDENTIFIER = N''a6354ea4-7113-4c14-8047-648f0cfc7163'';
DECLARE @ParentID UNIQUEIDENTIFIER = N''b12a6a37-7739-48e0-a9e1-499ae7d2a395'';
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
-- QUERY END: InsertDNDBTDbObjectRecordQuery

-- QUERY START: InsertDNDBTDbObjectRecordQuery
EXEC sp_executesql N'DECLARE @ID UNIQUEIDENTIFIER = N''53ad5415-7fea-4a51-bcae-65e349a2e477'';
DECLARE @ParentID UNIQUEIDENTIFIER = N''b12a6a37-7739-48e0-a9e1-499ae7d2a395'';
DECLARE @Name NVARCHAR(MAX) = N''PK_MyTable4'';
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
-- QUERY END: InsertDNDBTDbObjectRecordQuery

-- QUERY START: CreateTableQuery
EXEC sp_executesql N'CREATE TABLE [MyTable5]
(
    [MyColumn1] INT NOT NULL CONSTRAINT [DF_MyTable5_MyColumn1] DEFAULT ABs(-15),
    [MyColumn10] TIME NOT NULL CONSTRAINT [DF_MyTable5_MyColumn10] DEFAULT ''16:17:18'',
    [MyColumn101] MyUserDefinedType1 NULL CONSTRAINT [DF_MyTable5_MyColumn101] DEFAULT ''cc'',
    [MyColumn11] DATETIME2 NOT NULL CONSTRAINT [DF_MyTable5_MyColumn11] DEFAULT ''2022-02-15 16:17:18'',
    [MyColumn12] DATETIMEOFFSET NOT NULL CONSTRAINT [DF_MyTable5_MyColumn12] DEFAULT ''2022-02-15 16:17:18+01:30'',
    [MyColumn2] NCHAR(4) NOT NULL CONSTRAINT [DF_MyTable5_MyColumn2] DEFAULT ''test'',
    [MyColumn3] BINARY(3) NOT NULL CONSTRAINT [DF_MyTable5_MyColumn3] DEFAULT 0x000204,
    [MyColumn4] FLOAT NOT NULL CONSTRAINT [DF_MyTable5_MyColumn4] DEFAULT 123.456,
    [MyColumn5] REAL NOT NULL CONSTRAINT [DF_MyTable5_MyColumn5] DEFAULT 12345.6789,
    [MyColumn6] DECIMAL(6,1) NOT NULL CONSTRAINT [DF_MyTable5_MyColumn6] DEFAULT 12.3,
    [MyColumn7] BIT NOT NULL CONSTRAINT [DF_MyTable5_MyColumn7] DEFAULT 1,
    [MyColumn8] UNIQUEIDENTIFIER NOT NULL CONSTRAINT [DF_MyTable5_MyColumn8] DEFAULT ''8e2f99ad-0fc8-456d-b0e4-ec3ba572dd15'',
    [MyColumn9] DATE NOT NULL CONSTRAINT [DF_MyTable5_MyColumn9] DEFAULT ''2022-02-15'',
    CONSTRAINT [PK_MyTable5_CustomName] PRIMARY KEY ([MyColumn2], [MyColumn1]),
    CONSTRAINT [UQ_MyTable5_CustomName] UNIQUE ([MyColumn6], [MyColumn3], [MyColumn7])
);';
-- QUERY END: CreateTableQuery

-- QUERY START: InsertDNDBTDbObjectRecordQuery
EXEC sp_executesql N'DECLARE @ID UNIQUEIDENTIFIER = N''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'';
DECLARE @ParentID UNIQUEIDENTIFIER = NULL;
DECLARE @Name NVARCHAR(MAX) = N''MyTable5'';
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
-- QUERY END: InsertDNDBTDbObjectRecordQuery

-- QUERY START: InsertDNDBTDbObjectRecordQuery
EXEC sp_executesql N'DECLARE @ID UNIQUEIDENTIFIER = N''5309d66f-2030-402e-912e-5547babaa072'';
DECLARE @ParentID UNIQUEIDENTIFIER = N''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'';
DECLARE @Name NVARCHAR(MAX) = N''MyColumn1'';
DECLARE @Code NVARCHAR(MAX) = N''ABs(-15)'';
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
-- QUERY END: InsertDNDBTDbObjectRecordQuery

-- QUERY START: InsertDNDBTDbObjectRecordQuery
EXEC sp_executesql N'DECLARE @ID UNIQUEIDENTIFIER = N''cba4849b-3d84-4e38-b2c8-f9dbdff22fa6'';
DECLARE @ParentID UNIQUEIDENTIFIER = N''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'';
DECLARE @Name NVARCHAR(MAX) = N''MyColumn10'';
DECLARE @Code NVARCHAR(MAX) = N''''''16:17:18'''''';
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
-- QUERY END: InsertDNDBTDbObjectRecordQuery

-- QUERY START: InsertDNDBTDbObjectRecordQuery
EXEC sp_executesql N'DECLARE @ID UNIQUEIDENTIFIER = N''15ae6061-426d-4485-85e6-ecd3e0f98882'';
DECLARE @ParentID UNIQUEIDENTIFIER = N''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'';
DECLARE @Name NVARCHAR(MAX) = N''MyColumn101'';
DECLARE @Code NVARCHAR(MAX) = N''''''cc'''''';
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
-- QUERY END: InsertDNDBTDbObjectRecordQuery

-- QUERY START: InsertDNDBTDbObjectRecordQuery
EXEC sp_executesql N'DECLARE @ID UNIQUEIDENTIFIER = N''4dde852d-ec19-4b61-80f9-da428d8ff41a'';
DECLARE @ParentID UNIQUEIDENTIFIER = N''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'';
DECLARE @Name NVARCHAR(MAX) = N''MyColumn11'';
DECLARE @Code NVARCHAR(MAX) = N''''''2022-02-15 16:17:18'''''';
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
-- QUERY END: InsertDNDBTDbObjectRecordQuery

-- QUERY START: InsertDNDBTDbObjectRecordQuery
EXEC sp_executesql N'DECLARE @ID UNIQUEIDENTIFIER = N''685faf2e-fef7-4e6b-a960-acd093f1f004'';
DECLARE @ParentID UNIQUEIDENTIFIER = N''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'';
DECLARE @Name NVARCHAR(MAX) = N''MyColumn12'';
DECLARE @Code NVARCHAR(MAX) = N''''''2022-02-15 16:17:18+01:30'''''';
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
-- QUERY END: InsertDNDBTDbObjectRecordQuery

-- QUERY START: InsertDNDBTDbObjectRecordQuery
EXEC sp_executesql N'DECLARE @ID UNIQUEIDENTIFIER = N''11ef8e25-3691-42d4-b2fa-88d724f73b61'';
DECLARE @ParentID UNIQUEIDENTIFIER = N''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'';
DECLARE @Name NVARCHAR(MAX) = N''MyColumn2'';
DECLARE @Code NVARCHAR(MAX) = N''''''test'''''';
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
-- QUERY END: InsertDNDBTDbObjectRecordQuery

-- QUERY START: InsertDNDBTDbObjectRecordQuery
EXEC sp_executesql N'DECLARE @ID UNIQUEIDENTIFIER = N''6ed0ab37-aad3-4294-9ba6-c0921f0e67af'';
DECLARE @ParentID UNIQUEIDENTIFIER = N''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'';
DECLARE @Name NVARCHAR(MAX) = N''MyColumn3'';
DECLARE @Code NVARCHAR(MAX) = N''0x000204'';
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
-- QUERY END: InsertDNDBTDbObjectRecordQuery

-- QUERY START: InsertDNDBTDbObjectRecordQuery
EXEC sp_executesql N'DECLARE @ID UNIQUEIDENTIFIER = N''aca57fd6-80d0-4c18-b2ca-aabcb06bea10'';
DECLARE @ParentID UNIQUEIDENTIFIER = N''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'';
DECLARE @Name NVARCHAR(MAX) = N''MyColumn4'';
DECLARE @Code NVARCHAR(MAX) = N''123.456'';
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
-- QUERY END: InsertDNDBTDbObjectRecordQuery

-- QUERY START: InsertDNDBTDbObjectRecordQuery
EXEC sp_executesql N'DECLARE @ID UNIQUEIDENTIFIER = N''47666b8b-ca72-4507-86b2-04c47a84aed4'';
DECLARE @ParentID UNIQUEIDENTIFIER = N''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'';
DECLARE @Name NVARCHAR(MAX) = N''MyColumn5'';
DECLARE @Code NVARCHAR(MAX) = N''12345.6789'';
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
-- QUERY END: InsertDNDBTDbObjectRecordQuery

-- QUERY START: InsertDNDBTDbObjectRecordQuery
EXEC sp_executesql N'DECLARE @ID UNIQUEIDENTIFIER = N''98fded6c-d486-4a2e-9c9a-1ec31c9d5830'';
DECLARE @ParentID UNIQUEIDENTIFIER = N''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'';
DECLARE @Name NVARCHAR(MAX) = N''MyColumn6'';
DECLARE @Code NVARCHAR(MAX) = N''12.3'';
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
-- QUERY END: InsertDNDBTDbObjectRecordQuery

-- QUERY START: InsertDNDBTDbObjectRecordQuery
EXEC sp_executesql N'DECLARE @ID UNIQUEIDENTIFIER = N''2502cade-458a-48ee-9421-e6d7850493f7'';
DECLARE @ParentID UNIQUEIDENTIFIER = N''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'';
DECLARE @Name NVARCHAR(MAX) = N''MyColumn7'';
DECLARE @Code NVARCHAR(MAX) = N''1'';
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
-- QUERY END: InsertDNDBTDbObjectRecordQuery

-- QUERY START: InsertDNDBTDbObjectRecordQuery
EXEC sp_executesql N'DECLARE @ID UNIQUEIDENTIFIER = N''ed044a8a-6858-41e2-a867-9e5b01f226c8'';
DECLARE @ParentID UNIQUEIDENTIFIER = N''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'';
DECLARE @Name NVARCHAR(MAX) = N''MyColumn8'';
DECLARE @Code NVARCHAR(MAX) = N''''''8e2f99ad-0fc8-456d-b0e4-ec3ba572dd15'''''';
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
-- QUERY END: InsertDNDBTDbObjectRecordQuery

-- QUERY START: InsertDNDBTDbObjectRecordQuery
EXEC sp_executesql N'DECLARE @ID UNIQUEIDENTIFIER = N''9939d676-73b7-42d1-ba3e-5c13aed5ce34'';
DECLARE @ParentID UNIQUEIDENTIFIER = N''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'';
DECLARE @Name NVARCHAR(MAX) = N''MyColumn9'';
DECLARE @Code NVARCHAR(MAX) = N''''''2022-02-15'''''';
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
-- QUERY END: InsertDNDBTDbObjectRecordQuery

-- QUERY START: InsertDNDBTDbObjectRecordQuery
EXEC sp_executesql N'DECLARE @ID UNIQUEIDENTIFIER = N''79384d48-a39b-4a22-900e-066b2ca67ba2'';
DECLARE @ParentID UNIQUEIDENTIFIER = N''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'';
DECLARE @Name NVARCHAR(MAX) = N''PK_MyTable5_CustomName'';
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
-- QUERY END: InsertDNDBTDbObjectRecordQuery

-- QUERY START: InsertDNDBTDbObjectRecordQuery
EXEC sp_executesql N'DECLARE @ID UNIQUEIDENTIFIER = N''5293b58a-9f63-4f0f-8d6f-18416ebbd751'';
DECLARE @ParentID UNIQUEIDENTIFIER = N''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'';
DECLARE @Name NVARCHAR(MAX) = N''UQ_MyTable5_CustomName'';
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
-- QUERY END: InsertDNDBTDbObjectRecordQuery

-- QUERY START: CreateTableQuery
EXEC sp_executesql N'CREATE TABLE [MyTable6]
(
    [MyColumn1] NCHAR(4) NULL,
    [MyColumn2] INT NULL
);';
-- QUERY END: CreateTableQuery

-- QUERY START: InsertDNDBTDbObjectRecordQuery
EXEC sp_executesql N'DECLARE @ID UNIQUEIDENTIFIER = N''f3064a8c-346a-4b3d-af2c-d967b39841e4'';
DECLARE @ParentID UNIQUEIDENTIFIER = NULL;
DECLARE @Name NVARCHAR(MAX) = N''MyTable6'';
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
-- QUERY END: InsertDNDBTDbObjectRecordQuery

-- QUERY START: InsertDNDBTDbObjectRecordQuery
EXEC sp_executesql N'DECLARE @ID UNIQUEIDENTIFIER = N''bfa08c82-5c8f-4ab4-bd41-1f1d85cf3c85'';
DECLARE @ParentID UNIQUEIDENTIFIER = N''f3064a8c-346a-4b3d-af2c-d967b39841e4'';
DECLARE @Name NVARCHAR(MAX) = N''MyColumn1'';
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
-- QUERY END: InsertDNDBTDbObjectRecordQuery

-- QUERY START: InsertDNDBTDbObjectRecordQuery
EXEC sp_executesql N'DECLARE @ID UNIQUEIDENTIFIER = N''a402e2b7-c826-4cfd-a304-97c9bc346ba2'';
DECLARE @ParentID UNIQUEIDENTIFIER = N''f3064a8c-346a-4b3d-af2c-d967b39841e4'';
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
-- QUERY END: InsertDNDBTDbObjectRecordQuery

-- QUERY START: CreateIndexQuery
EXEC sp_executesql N'CREATE UNIQUE INDEX [IDX_MyTable2_MyIndex1]
    ON [MyTable2] ([MyColumn1], [MyColumn2]);';
-- QUERY END: CreateIndexQuery

-- QUERY START: InsertDNDBTDbObjectRecordQuery
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
-- QUERY END: InsertDNDBTDbObjectRecordQuery

-- QUERY START: CreateIndexQuery
EXEC sp_executesql N'CREATE INDEX [IDX_MyTable5_CustomName]
    ON [MyTable5] ([MyColumn8]);';
-- QUERY END: CreateIndexQuery

-- QUERY START: InsertDNDBTDbObjectRecordQuery
EXEC sp_executesql N'DECLARE @ID UNIQUEIDENTIFIER = N''1d632285-9914-4c5d-98e6-a618a99bd799'';
DECLARE @ParentID UNIQUEIDENTIFIER = N''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'';
DECLARE @Name NVARCHAR(MAX) = N''IDX_MyTable5_CustomName'';
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
-- QUERY END: InsertDNDBTDbObjectRecordQuery

-- QUERY START: CreateForeignKeyQuery
EXEC sp_executesql N'ALTER TABLE [MyTable1] ADD CONSTRAINT [FK_MyTable1_MyColumn1_MyTable2_MyColumn1] FOREIGN KEY ([MyColumn1])
        REFERENCES [MyTable2] ([MyColumn1])
        ON UPDATE NO ACTION ON DELETE CASCADE;';
-- QUERY END: CreateForeignKeyQuery

-- QUERY START: InsertDNDBTDbObjectRecordQuery
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
-- QUERY END: InsertDNDBTDbObjectRecordQuery

-- QUERY START: CreateForeignKeyQuery
EXEC sp_executesql N'ALTER TABLE [MyTable6] ADD CONSTRAINT [FK_MyTable6_MyTable5_CustomName] FOREIGN KEY ([MyColumn1], [MyColumn2])
        REFERENCES [MyTable5] ([MyColumn2], [MyColumn1])
        ON UPDATE NO ACTION ON DELETE NO ACTION;';
-- QUERY END: CreateForeignKeyQuery

-- QUERY START: InsertDNDBTDbObjectRecordQuery
EXEC sp_executesql N'DECLARE @ID UNIQUEIDENTIFIER = N''ae453b22-d270-41fc-8184-9ac26b7a0569'';
DECLARE @ParentID UNIQUEIDENTIFIER = N''f3064a8c-346a-4b3d-af2c-d967b39841e4'';
DECLARE @Name NVARCHAR(MAX) = N''FK_MyTable6_MyTable5_CustomName'';
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
-- QUERY END: InsertDNDBTDbObjectRecordQuery

-- QUERY START: CreateViewQuery
EXEC sp_executesql N'CREATE VIEW MyView1 AS
SELECT
    t1.MyColumn1,
    t1.MyColumn4,
    t2.MyColumn2
FROM MyTable1 t1
LEFT JOIN MyTable2 t2
    ON t2.MyColumn1 = t1.MyColumn1';
-- QUERY END: CreateViewQuery

-- QUERY START: InsertDNDBTDbObjectRecordQuery
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
    ON t2.MyColumn1 = t1.MyColumn1'';
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
-- QUERY END: InsertDNDBTDbObjectRecordQuery

-- QUERY START: CreateTriggerQuery
EXEC sp_executesql N'CREATE TRIGGER [TR_MyTable2_MyTrigger1]
ON [MyTable2]
AFTER INSERT
AS
BEGIN
    INSERT INTO [MyTable4]([MyColumn1])
    SELECT i.[MyColumn1] FROM inserted i;
END';
-- QUERY END: CreateTriggerQuery

-- QUERY START: InsertDNDBTDbObjectRecordQuery
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
-- QUERY END: InsertDNDBTDbObjectRecordQuery

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
WHERE NOT EXISTS (SELECT * FROM [MyTable4])';
-- QUERY END: GenericQuery

-- QUERY START: InsertDNDBTScriptExecutionRecordQuery
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
WHERE NOT EXISTS (SELECT * FROM [MyTable4])'';
DECLARE @MinDbVersionToExecute BIGINT = 0;
DECLARE @MaxDbVersionToExecute BIGINT = 9223372036854775807;
DECLARE @ExecutedOnDbVersion BIGINT = 0;
INSERT INTO [DNDBTScriptExecutions]
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
    @ID,
    @Type,
    @Name,
    @Code,
    @MinDbVersionToExecute,
    @MaxDbVersionToExecute,
    @ExecutedOnDbVersion
);';
-- QUERY END: InsertDNDBTScriptExecutionRecordQuery

-- QUERY START: UpdateDNDBTDbAttributesRecordQuery
EXEC sp_executesql N'DECLARE @Version BIGINT = 1;
UPDATE [DNDBTDbAttributes] SET
    [Version] = @Version;';
-- QUERY END: UpdateDNDBTDbAttributesRecordQuery

    COMMIT TRANSACTION;
END TRY
BEGIN CATCH;
    ROLLBACK TRANSACTION;

    DECLARE @ErrorMessage NVARCHAR(MAX), @ErrorSeverity INT, @ErrorState INT;
    SELECT @ErrorMessage = ERROR_MESSAGE() + ' Line ' + CAST(ERROR_LINE() AS NVARCHAR(5)), @ErrorSeverity = ERROR_SEVERITY(), @ErrorState = ERROR_STATE();
    RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
END CATCH;