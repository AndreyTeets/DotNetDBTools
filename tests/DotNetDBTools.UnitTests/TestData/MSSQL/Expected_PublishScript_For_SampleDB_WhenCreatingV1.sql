SET NOCOUNT ON;
SET XACT_ABORT ON;
BEGIN TRY;
    BEGIN TRANSACTION;

-- QUERY START: MSSQLCreateTypeQuery
EXEC sp_executesql N'CREATE TYPE MyUserDefinedType1 FROM VARCHAR(100);';
-- QUERY END: MSSQLCreateTypeQuery

-- QUERY START: MSSQLInsertDNDBTSysInfoQuery
EXEC sp_executesql N'DECLARE @ID UNIQUEIDENTIFIER = ''0cd1e71c-cc9c-440f-ac0b-81a1d6f7ddaa'';
DECLARE @ParentID UNIQUEIDENTIFIER = NULL;
DECLARE @Name NVARCHAR(MAX) = ''MyUserDefinedType1'';
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
-- QUERY END: MSSQLInsertDNDBTSysInfoQuery

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

-- QUERY START: MSSQLInsertDNDBTSysInfoQuery
EXEC sp_executesql N'DECLARE @ID UNIQUEIDENTIFIER = ''299675e6-4faa-4d0f-a36a-224306ba5bcb'';
DECLARE @ParentID UNIQUEIDENTIFIER = NULL;
DECLARE @Name NVARCHAR(MAX) = ''MyTable1'';
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
-- QUERY END: MSSQLInsertDNDBTSysInfoQuery

-- QUERY START: MSSQLInsertDNDBTSysInfoQuery
EXEC sp_executesql N'DECLARE @ID UNIQUEIDENTIFIER = ''a2f2a4de-1337-4594-ae41-72ed4d05f317'';
DECLARE @ParentID UNIQUEIDENTIFIER = ''299675e6-4faa-4d0f-a36a-224306ba5bcb'';
DECLARE @Name NVARCHAR(MAX) = ''MyColumn1'';
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
-- QUERY END: MSSQLInsertDNDBTSysInfoQuery

-- QUERY START: MSSQLInsertDNDBTSysInfoQuery
EXEC sp_executesql N'DECLARE @ID UNIQUEIDENTIFIER = ''fe68ee3d-09d0-40ac-93f9-5e441fbb4f70'';
DECLARE @ParentID UNIQUEIDENTIFIER = ''299675e6-4faa-4d0f-a36a-224306ba5bcb'';
DECLARE @Name NVARCHAR(MAX) = ''MyColumn2'';
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
-- QUERY END: MSSQLInsertDNDBTSysInfoQuery

-- QUERY START: MSSQLInsertDNDBTSysInfoQuery
EXEC sp_executesql N'DECLARE @ID UNIQUEIDENTIFIER = ''6e95de30-e01a-4fb4-b8b7-8f0c40bb682c'';
DECLARE @ParentID UNIQUEIDENTIFIER = ''299675e6-4faa-4d0f-a36a-224306ba5bcb'';
DECLARE @Name NVARCHAR(MAX) = ''MyColumn3'';
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
-- QUERY END: MSSQLInsertDNDBTSysInfoQuery

-- QUERY START: MSSQLInsertDNDBTSysInfoQuery
EXEC sp_executesql N'DECLARE @ID UNIQUEIDENTIFIER = ''867ac528-e87e-4c93-b6e3-dd2fcbbb837f'';
DECLARE @ParentID UNIQUEIDENTIFIER = ''299675e6-4faa-4d0f-a36a-224306ba5bcb'';
DECLARE @Name NVARCHAR(MAX) = ''MyColumn4'';
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
-- QUERY END: MSSQLInsertDNDBTSysInfoQuery

-- QUERY START: MSSQLInsertDNDBTSysInfoQuery
EXEC sp_executesql N'DECLARE @ID UNIQUEIDENTIFIER = ''37a45def-f4a0-4be7-8bfb-8fbed4a7d705'';
DECLARE @ParentID UNIQUEIDENTIFIER = ''299675e6-4faa-4d0f-a36a-224306ba5bcb'';
DECLARE @Name NVARCHAR(MAX) = ''PK_MyTable1'';
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
-- QUERY END: MSSQLInsertDNDBTSysInfoQuery

-- QUERY START: MSSQLInsertDNDBTSysInfoQuery
EXEC sp_executesql N'DECLARE @ID UNIQUEIDENTIFIER = ''f3f08522-26ee-4950-9135-22edf2e4e0cf'';
DECLARE @ParentID UNIQUEIDENTIFIER = ''299675e6-4faa-4d0f-a36a-224306ba5bcb'';
DECLARE @Name NVARCHAR(MAX) = ''UQ_MyTable1_MyColumn2'';
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
-- QUERY END: MSSQLInsertDNDBTSysInfoQuery

-- QUERY START: MSSQLInsertDNDBTSysInfoQuery
EXEC sp_executesql N'DECLARE @ID UNIQUEIDENTIFIER = ''eb9c59b5-bc7e-49d7-adaa-f5600b6a19a2'';
DECLARE @ParentID UNIQUEIDENTIFIER = ''299675e6-4faa-4d0f-a36a-224306ba5bcb'';
DECLARE @Name NVARCHAR(MAX) = ''CK_MyTable1_MyCheck1'';
DECLARE @Code NVARCHAR(MAX) = ''CHECK (MyColumn4 >= 0)'';
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
-- QUERY END: MSSQLInsertDNDBTSysInfoQuery

-- QUERY START: MSSQLCreateTableQuery
EXEC sp_executesql N'CREATE TABLE MyTable2
(
    MyColumn1 INT NOT NULL CONSTRAINT DF_MyTable2_MyColumn1 DEFAULT 333,
    MyColumn2 BINARY(22) NULL CONSTRAINT DF_MyTable2_MyColumn2 DEFAULT 0x000102,
    CONSTRAINT PK_MyTable2 PRIMARY KEY (MyColumn1)
);';
-- QUERY END: MSSQLCreateTableQuery

-- QUERY START: MSSQLInsertDNDBTSysInfoQuery
EXEC sp_executesql N'DECLARE @ID UNIQUEIDENTIFIER = ''bfb9030c-a8c3-4882-9c42-1c6ad025cf8f'';
DECLARE @ParentID UNIQUEIDENTIFIER = NULL;
DECLARE @Name NVARCHAR(MAX) = ''MyTable2'';
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
-- QUERY END: MSSQLInsertDNDBTSysInfoQuery

-- QUERY START: MSSQLInsertDNDBTSysInfoQuery
EXEC sp_executesql N'DECLARE @ID UNIQUEIDENTIFIER = ''c480f22f-7c01-4f41-b282-35e9f5cd1fe3'';
DECLARE @ParentID UNIQUEIDENTIFIER = ''bfb9030c-a8c3-4882-9c42-1c6ad025cf8f'';
DECLARE @Name NVARCHAR(MAX) = ''MyColumn1'';
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
-- QUERY END: MSSQLInsertDNDBTSysInfoQuery

-- QUERY START: MSSQLInsertDNDBTSysInfoQuery
EXEC sp_executesql N'DECLARE @ID UNIQUEIDENTIFIER = ''5a0d1926-3270-4eb2-92eb-00be56c7af23'';
DECLARE @ParentID UNIQUEIDENTIFIER = ''bfb9030c-a8c3-4882-9c42-1c6ad025cf8f'';
DECLARE @Name NVARCHAR(MAX) = ''MyColumn2'';
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
-- QUERY END: MSSQLInsertDNDBTSysInfoQuery

-- QUERY START: MSSQLInsertDNDBTSysInfoQuery
EXEC sp_executesql N'DECLARE @ID UNIQUEIDENTIFIER = ''3a43615b-40b3-4a13-99e7-93af7c56e8ce'';
DECLARE @ParentID UNIQUEIDENTIFIER = ''bfb9030c-a8c3-4882-9c42-1c6ad025cf8f'';
DECLARE @Name NVARCHAR(MAX) = ''PK_MyTable2'';
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
-- QUERY END: MSSQLInsertDNDBTSysInfoQuery

-- QUERY START: MSSQLCreateTableQuery
EXEC sp_executesql N'CREATE TABLE MyTable5
(
    MyColumn1 INT NOT NULL CONSTRAINT DF_MyTable5_MyColumn1 DEFAULT ABS(-15),
    MyColumn2 MyUserDefinedType1 NULL CONSTRAINT DF_MyTable5_MyColumn2 DEFAULT ''cc'',
    MyColumn3 DATETIMEOFFSET NULL
);';
-- QUERY END: MSSQLCreateTableQuery

-- QUERY START: MSSQLInsertDNDBTSysInfoQuery
EXEC sp_executesql N'DECLARE @ID UNIQUEIDENTIFIER = ''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'';
DECLARE @ParentID UNIQUEIDENTIFIER = NULL;
DECLARE @Name NVARCHAR(MAX) = ''MyTable5'';
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
-- QUERY END: MSSQLInsertDNDBTSysInfoQuery

-- QUERY START: MSSQLInsertDNDBTSysInfoQuery
EXEC sp_executesql N'DECLARE @ID UNIQUEIDENTIFIER = ''5309d66f-2030-402e-912e-5547babaa072'';
DECLARE @ParentID UNIQUEIDENTIFIER = ''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'';
DECLARE @Name NVARCHAR(MAX) = ''MyColumn1'';
DECLARE @Code NVARCHAR(MAX) = ''ABS(-15)'';
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
-- QUERY END: MSSQLInsertDNDBTSysInfoQuery

-- QUERY START: MSSQLInsertDNDBTSysInfoQuery
EXEC sp_executesql N'DECLARE @ID UNIQUEIDENTIFIER = ''15ae6061-426d-4485-85e6-ecd3e0f98882'';
DECLARE @ParentID UNIQUEIDENTIFIER = ''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'';
DECLARE @Name NVARCHAR(MAX) = ''MyColumn2'';
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
-- QUERY END: MSSQLInsertDNDBTSysInfoQuery

-- QUERY START: MSSQLInsertDNDBTSysInfoQuery
EXEC sp_executesql N'DECLARE @ID UNIQUEIDENTIFIER = ''4dde852d-ec19-4b61-80f9-da428d8ff41a'';
DECLARE @ParentID UNIQUEIDENTIFIER = ''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'';
DECLARE @Name NVARCHAR(MAX) = ''MyColumn3'';
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
-- QUERY END: MSSQLInsertDNDBTSysInfoQuery

-- QUERY START: MSSQLCreateForeignKeyQuery
EXEC sp_executesql N'ALTER TABLE MyTable1 ADD CONSTRAINT FK_MyTable1_MyColumn1_MyTable2_MyColumn1 FOREIGN KEY (MyColumn1)
    REFERENCES MyTable2 (MyColumn1)
    ON UPDATE NO ACTION ON DELETE CASCADE;';
-- QUERY END: MSSQLCreateForeignKeyQuery

-- QUERY START: MSSQLInsertDNDBTSysInfoQuery
EXEC sp_executesql N'DECLARE @ID UNIQUEIDENTIFIER = ''d11b2a53-32db-432f-bb6b-f91788844ba9'';
DECLARE @ParentID UNIQUEIDENTIFIER = ''299675e6-4faa-4d0f-a36a-224306ba5bcb'';
DECLARE @Name NVARCHAR(MAX) = ''FK_MyTable1_MyColumn1_MyTable2_MyColumn1'';
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
-- QUERY END: MSSQLInsertDNDBTSysInfoQuery

    COMMIT TRANSACTION;
END TRY
BEGIN CATCH;
    ROLLBACK TRANSACTION;

    DECLARE @ErrorMessage NVARCHAR(MAX), @ErrorSeverity INT, @ErrorState INT;
    SELECT @ErrorMessage = ERROR_MESSAGE() + ' Line ' + CAST(ERROR_LINE() AS NVARCHAR(5)), @ErrorSeverity = ERROR_SEVERITY(), @ErrorState = ERROR_STATE();
    RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
END CATCH;