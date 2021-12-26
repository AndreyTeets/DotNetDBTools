SET NOCOUNT ON;
SET XACT_ABORT ON;
BEGIN TRY;
    BEGIN TRANSACTION;

-- QUERY START: MSSQLDropForeignKeyQuery
EXEC sp_executesql N'ALTER TABLE MyTable1 DROP CONSTRAINT FK_MyTable1_MyColumn1_MyTable2_MyColumn1;';
-- QUERY END: MSSQLDropForeignKeyQuery

-- QUERY START: MSSQLDeleteDNDBTSysInfoQuery
EXEC sp_executesql N'DELETE FROM DNDBTDbObjects
WHERE ID = ''d11b2a53-32db-432f-bb6b-f91788844ba9'';';
-- QUERY END: MSSQLDeleteDNDBTSysInfoQuery

-- QUERY START: MSSQLRenameUserDefinedDataTypeQuery
EXEC sp_executesql N'EXEC sp_rename ''MyUserDefinedType1'', ''_DNDBTTemp_MyUserDefinedType1'', ''USERDATATYPE'';';
-- QUERY END: MSSQLRenameUserDefinedDataTypeQuery

-- QUERY START: MSSQLDeleteDNDBTSysInfoQuery
EXEC sp_executesql N'DELETE FROM DNDBTDbObjects
WHERE ID = ''0cd1e71c-cc9c-440f-ac0b-81a1d6f7ddaa'';';
-- QUERY END: MSSQLDeleteDNDBTSysInfoQuery

-- QUERY START: MSSQLCreateTypeQuery
EXEC sp_executesql N'CREATE TYPE MyUserDefinedType1 FROM VARCHAR(110);';
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
ALTER TABLE MyTable1NewName DROP CONSTRAINT CK_MyTable1_MyCheck1;
ALTER TABLE MyTable1NewName DROP CONSTRAINT UQ_MyTable1_MyColumn2;
ALTER TABLE MyTable1NewName DROP CONSTRAINT PK_MyTable1;
ALTER TABLE [MyTable1NewName] DROP CONSTRAINT DF_MyTable1_MyColumn2;
ALTER TABLE MyTable1NewName DROP COLUMN MyColumn2;
ALTER TABLE MyTable1NewName DROP COLUMN MyColumn3;
ALTER TABLE [MyTable1NewName] DROP CONSTRAINT DF_MyTable1_MyColumn1;
ALTER TABLE MyTable1NewName ALTER COLUMN MyColumn1 BIGINT NULL;
ALTER TABLE MyTable1NewName ADD CONSTRAINT DF_MyTable1NewName_MyColumn1 DEFAULT 15 FOR MyColumn1;
ALTER TABLE [MyTable1NewName] DROP CONSTRAINT DF_MyTable1_MyColumn4;
ALTER TABLE MyTable1NewName ALTER COLUMN MyColumn4 DECIMAL(19, 2) NOT NULL;
ALTER TABLE MyTable1NewName ADD CONSTRAINT DF_MyTable1NewName_MyColumn4 DEFAULT 7.36 FOR MyColumn4;
ALTER TABLE MyTable1NewName ADD CONSTRAINT CK_MyTable1_MyCheck1 CHECK (MyColumn4 >= 1);';
-- QUERY END: MSSQLAlterTableQuery

-- QUERY START: MSSQLDeleteDNDBTSysInfoQuery
EXEC sp_executesql N'DELETE FROM DNDBTDbObjects
WHERE ID = ''eb9c59b5-bc7e-49d7-adaa-f5600b6a19a2'';';
-- QUERY END: MSSQLDeleteDNDBTSysInfoQuery

-- QUERY START: MSSQLDeleteDNDBTSysInfoQuery
EXEC sp_executesql N'DELETE FROM DNDBTDbObjects
WHERE ID = ''f3f08522-26ee-4950-9135-22edf2e4e0cf'';';
-- QUERY END: MSSQLDeleteDNDBTSysInfoQuery

-- QUERY START: MSSQLDeleteDNDBTSysInfoQuery
EXEC sp_executesql N'DELETE FROM DNDBTDbObjects
WHERE ID = ''37a45def-f4a0-4be7-8bfb-8fbed4a7d705'';';
-- QUERY END: MSSQLDeleteDNDBTSysInfoQuery

-- QUERY START: MSSQLDeleteDNDBTSysInfoQuery
EXEC sp_executesql N'DELETE FROM DNDBTDbObjects
WHERE ID = ''fe68ee3d-09d0-40ac-93f9-5e441fbb4f70'';';
-- QUERY END: MSSQLDeleteDNDBTSysInfoQuery

-- QUERY START: MSSQLDeleteDNDBTSysInfoQuery
EXEC sp_executesql N'DELETE FROM DNDBTDbObjects
WHERE ID = ''6e95de30-e01a-4fb4-b8b7-8f0c40bb682c'';';
-- QUERY END: MSSQLDeleteDNDBTSysInfoQuery

-- QUERY START: MSSQLUpdateDNDBTSysInfoQuery
EXEC sp_executesql N'DECLARE @ID UNIQUEIDENTIFIER = ''299675e6-4faa-4d0f-a36a-224306ba5bcb'';
DECLARE @Name NVARCHAR(MAX) = ''MyTable1NewName'';
DECLARE @Code NVARCHAR(MAX) = NULL;
UPDATE DNDBTDbObjects SET
    Name = @Name,
    Code = @Code
WHERE ID = @ID;';
-- QUERY END: MSSQLUpdateDNDBTSysInfoQuery

-- QUERY START: MSSQLUpdateDNDBTSysInfoQuery
EXEC sp_executesql N'DECLARE @ID UNIQUEIDENTIFIER = ''a2f2a4de-1337-4594-ae41-72ed4d05f317'';
DECLARE @Name NVARCHAR(MAX) = ''MyColumn1'';
DECLARE @Code NVARCHAR(MAX) = NULL;
UPDATE DNDBTDbObjects SET
    Name = @Name,
    Code = @Code
WHERE ID = @ID;';
-- QUERY END: MSSQLUpdateDNDBTSysInfoQuery

-- QUERY START: MSSQLUpdateDNDBTSysInfoQuery
EXEC sp_executesql N'DECLARE @ID UNIQUEIDENTIFIER = ''867ac528-e87e-4c93-b6e3-dd2fcbbb837f'';
DECLARE @Name NVARCHAR(MAX) = ''MyColumn4'';
DECLARE @Code NVARCHAR(MAX) = NULL;
UPDATE DNDBTDbObjects SET
    Name = @Name,
    Code = @Code
WHERE ID = @ID;';
-- QUERY END: MSSQLUpdateDNDBTSysInfoQuery

-- QUERY START: MSSQLInsertDNDBTSysInfoQuery
EXEC sp_executesql N'DECLARE @ID UNIQUEIDENTIFIER = ''eb9c59b5-bc7e-49d7-adaa-f5600b6a19a2'';
DECLARE @ParentID UNIQUEIDENTIFIER = ''299675e6-4faa-4d0f-a36a-224306ba5bcb'';
DECLARE @Name NVARCHAR(MAX) = ''CK_MyTable1_MyCheck1'';
DECLARE @Code NVARCHAR(MAX) = ''CHECK (MyColumn4 >= 1)'';
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

-- QUERY START: MSSQLAlterTableQuery
EXEC sp_executesql N'
ALTER TABLE MyTable2 DROP CONSTRAINT PK_MyTable2;
ALTER TABLE [MyTable2] DROP CONSTRAINT DF_MyTable2_MyColumn2;
ALTER TABLE MyTable2 DROP COLUMN MyColumn2;
ALTER TABLE [MyTable2] DROP CONSTRAINT DF_MyTable2_MyColumn1;
EXEC sp_rename ''MyTable2.MyColumn1'', ''MyColumn1NewName'', ''COLUMN'';
ALTER TABLE MyTable2 ALTER COLUMN MyColumn1NewName BIGINT NOT NULL;
ALTER TABLE MyTable2 ADD CONSTRAINT DF_MyTable2_MyColumn1NewName DEFAULT 333 FOR MyColumn1NewName;
ALTER TABLE MyTable2 ADD MyColumn2 VARBINARY(22) NULL;
ALTER TABLE MyTable2 ADD CONSTRAINT DF_MyTable2_MyColumn2 DEFAULT 0x000102 FOR MyColumn2;
ALTER TABLE MyTable2 ADD CONSTRAINT PK_MyTable2 PRIMARY KEY (MyColumn1NewName);';
-- QUERY END: MSSQLAlterTableQuery

-- QUERY START: MSSQLDeleteDNDBTSysInfoQuery
EXEC sp_executesql N'DELETE FROM DNDBTDbObjects
WHERE ID = ''3a43615b-40b3-4a13-99e7-93af7c56e8ce'';';
-- QUERY END: MSSQLDeleteDNDBTSysInfoQuery

-- QUERY START: MSSQLDeleteDNDBTSysInfoQuery
EXEC sp_executesql N'DELETE FROM DNDBTDbObjects
WHERE ID = ''5a0d1926-3270-4eb2-92eb-00be56c7af23'';';
-- QUERY END: MSSQLDeleteDNDBTSysInfoQuery

-- QUERY START: MSSQLUpdateDNDBTSysInfoQuery
EXEC sp_executesql N'DECLARE @ID UNIQUEIDENTIFIER = ''bfb9030c-a8c3-4882-9c42-1c6ad025cf8f'';
DECLARE @Name NVARCHAR(MAX) = ''MyTable2'';
DECLARE @Code NVARCHAR(MAX) = NULL;
UPDATE DNDBTDbObjects SET
    Name = @Name,
    Code = @Code
WHERE ID = @ID;';
-- QUERY END: MSSQLUpdateDNDBTSysInfoQuery

-- QUERY START: MSSQLUpdateDNDBTSysInfoQuery
EXEC sp_executesql N'DECLARE @ID UNIQUEIDENTIFIER = ''c480f22f-7c01-4f41-b282-35e9f5cd1fe3'';
DECLARE @Name NVARCHAR(MAX) = ''MyColumn1NewName'';
DECLARE @Code NVARCHAR(MAX) = NULL;
UPDATE DNDBTDbObjects SET
    Name = @Name,
    Code = @Code
WHERE ID = @ID;';
-- QUERY END: MSSQLUpdateDNDBTSysInfoQuery

-- QUERY START: MSSQLInsertDNDBTSysInfoQuery
EXEC sp_executesql N'DECLARE @ID UNIQUEIDENTIFIER = ''c2df19c2-e029-4014-8a5b-4ab42fecb6b8'';
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

-- QUERY START: MSSQLDropTypeQuery
EXEC sp_executesql N'DROP TYPE _DNDBTTemp_MyUserDefinedType1;';
-- QUERY END: MSSQLDropTypeQuery

-- QUERY START: MSSQLCreateTableQuery
EXEC sp_executesql N'CREATE TABLE MyTable3
(
    MyColumn1 BIGINT NOT NULL CONSTRAINT DF_MyTable3_MyColumn1 DEFAULT 333,
    MyColumn2 VARBINARY(22) NOT NULL,
    CONSTRAINT UQ_MyTable3_MyColumns12 UNIQUE (MyColumn1, MyColumn2)
);';
-- QUERY END: MSSQLCreateTableQuery

-- QUERY START: MSSQLInsertDNDBTSysInfoQuery
EXEC sp_executesql N'DECLARE @ID UNIQUEIDENTIFIER = ''474cd761-2522-4529-9d20-2b94115f9626'';
DECLARE @ParentID UNIQUEIDENTIFIER = NULL;
DECLARE @Name NVARCHAR(MAX) = ''MyTable3'';
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
EXEC sp_executesql N'DECLARE @ID UNIQUEIDENTIFIER = ''726f503a-d944-46ee-a0ff-6a2c2faab46e'';
DECLARE @ParentID UNIQUEIDENTIFIER = ''474cd761-2522-4529-9d20-2b94115f9626'';
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
EXEC sp_executesql N'DECLARE @ID UNIQUEIDENTIFIER = ''169824e1-8b74-4b60-af17-99656d6dbbee'';
DECLARE @ParentID UNIQUEIDENTIFIER = ''474cd761-2522-4529-9d20-2b94115f9626'';
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
EXEC sp_executesql N'DECLARE @ID UNIQUEIDENTIFIER = ''fd288e38-35ba-4bb1-ace3-597c99ef26c7'';
DECLARE @ParentID UNIQUEIDENTIFIER = ''474cd761-2522-4529-9d20-2b94115f9626'';
DECLARE @Name NVARCHAR(MAX) = ''UQ_MyTable3_MyColumns12'';
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

-- QUERY START: MSSQLCreateForeignKeyQuery
EXEC sp_executesql N'ALTER TABLE MyTable1NewName ADD CONSTRAINT FK_MyTable1_MyColumn1_MyTable2_MyColumn1 FOREIGN KEY (MyColumn1)
    REFERENCES MyTable2 (MyColumn1NewName)
    ON UPDATE NO ACTION ON DELETE SET NULL;';
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

-- QUERY START: MSSQLCreateForeignKeyQuery
EXEC sp_executesql N'ALTER TABLE MyTable2 ADD CONSTRAINT FK_MyTable2_MyColumns12_MyTable3_MyColumns12 FOREIGN KEY (MyColumn1NewName, MyColumn2)
    REFERENCES MyTable3 (MyColumn1, MyColumn2)
    ON UPDATE NO ACTION ON DELETE SET DEFAULT;';
-- QUERY END: MSSQLCreateForeignKeyQuery

-- QUERY START: MSSQLInsertDNDBTSysInfoQuery
EXEC sp_executesql N'DECLARE @ID UNIQUEIDENTIFIER = ''480f3508-9d51-4190-88aa-45bc20e49119'';
DECLARE @ParentID UNIQUEIDENTIFIER = ''bfb9030c-a8c3-4882-9c42-1c6ad025cf8f'';
DECLARE @Name NVARCHAR(MAX) = ''FK_MyTable2_MyColumns12_MyTable3_MyColumns12'';
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