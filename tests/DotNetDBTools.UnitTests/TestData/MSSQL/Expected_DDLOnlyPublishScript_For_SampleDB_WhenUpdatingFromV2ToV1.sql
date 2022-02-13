SET NOCOUNT ON;
SET XACT_ABORT ON;
BEGIN TRY;
    BEGIN TRANSACTION;

-- QUERY START: MSSQLDropTriggerQuery
EXEC sp_executesql N'DROP TRIGGER [TR_MyTable2_MyTrigger1];';
-- QUERY END: MSSQLDropTriggerQuery

-- QUERY START: GenericQuery
EXEC sp_executesql N'DROP VIEW MyView1;';
-- QUERY END: GenericQuery

-- QUERY START: MSSQLDropForeignKeyQuery
EXEC sp_executesql N'ALTER TABLE MyTable1NewName DROP CONSTRAINT FK_MyTable1_MyColumn1_MyTable2_MyColumn1;';
-- QUERY END: MSSQLDropForeignKeyQuery

-- QUERY START: MSSQLDropForeignKeyQuery
EXEC sp_executesql N'ALTER TABLE MyTable2 DROP CONSTRAINT FK_MyTable2_MyColumns12_MyTable3_MyColumns12;';
-- QUERY END: MSSQLDropForeignKeyQuery

-- QUERY START: MSSQLDropIndexQuery
EXEC sp_executesql N'DROP INDEX [IDX_MyTable2_MyIndex1] ON [MyTable2];';
-- QUERY END: MSSQLDropIndexQuery

-- QUERY START: MSSQLDropTableQuery
EXEC sp_executesql N'DROP TABLE MyTable3;';
-- QUERY END: MSSQLDropTableQuery

-- QUERY START: MSSQLRenameUserDefinedDataTypeQuery
EXEC sp_executesql N'EXEC sp_rename ''MyUserDefinedType1'', ''_DNDBTTemp_MyUserDefinedType1'', ''USERDATATYPE'';';
-- QUERY END: MSSQLRenameUserDefinedDataTypeQuery

-- QUERY START: MSSQLCreateTypeQuery
EXEC sp_executesql N'CREATE TYPE MyUserDefinedType1 FROM VARCHAR(100);';
-- QUERY END: MSSQLCreateTypeQuery

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
EXEC sp_executesql N'EXEC sp_rename ''MyTable1NewName'', ''MyTable1'';
ALTER TABLE MyTable1 DROP CONSTRAINT CK_MyTable1_MyCheck1;
ALTER TABLE [MyTable1] DROP CONSTRAINT DF_MyTable1NewName_MyColumn1;
ALTER TABLE MyTable1 ALTER COLUMN MyColumn1 INT NOT NULL;
ALTER TABLE MyTable1 ADD CONSTRAINT DF_MyTable1_MyColumn1 DEFAULT 15 FOR MyColumn1;
ALTER TABLE [MyTable1] DROP CONSTRAINT DF_MyTable1NewName_MyColumn4;
ALTER TABLE MyTable1 ALTER COLUMN MyColumn4 DECIMAL(19, 2) NOT NULL;
ALTER TABLE MyTable1 ADD CONSTRAINT DF_MyTable1_MyColumn4 DEFAULT 7.36 FOR MyColumn4;
ALTER TABLE MyTable1 ADD MyColumn2 NVARCHAR(10) NOT NULL;
ALTER TABLE MyTable1 ADD CONSTRAINT DF_MyTable1_MyColumn2 DEFAULT ''33'' FOR MyColumn2;
ALTER TABLE MyTable1 ADD MyColumn3 INT IDENTITY NOT NULL;
ALTER TABLE MyTable1 ADD CONSTRAINT PK_MyTable1 PRIMARY KEY (MyColumn3);
ALTER TABLE MyTable1 ADD CONSTRAINT UQ_MyTable1_MyColumn2 UNIQUE (MyColumn2);
ALTER TABLE MyTable1 ADD CONSTRAINT CK_MyTable1_MyCheck1 CHECK (MyColumn4 >= 0);';
-- QUERY END: MSSQLAlterTableQuery

-- QUERY START: MSSQLAlterTableQuery
EXEC sp_executesql N'
ALTER TABLE MyTable2 DROP CONSTRAINT PK_MyTable2;
ALTER TABLE [MyTable2] DROP CONSTRAINT DF_MyTable2_MyColumn2;
ALTER TABLE MyTable2 DROP COLUMN MyColumn2;
ALTER TABLE [MyTable2] DROP CONSTRAINT DF_MyTable2_MyColumn1NewName;
EXEC sp_rename ''MyTable2.MyColumn1NewName'', ''MyColumn1'', ''COLUMN'';
ALTER TABLE MyTable2 ALTER COLUMN MyColumn1 INT NOT NULL;
ALTER TABLE MyTable2 ADD CONSTRAINT DF_MyTable2_MyColumn1 DEFAULT 333 FOR MyColumn1;
ALTER TABLE MyTable2 ADD MyColumn2 BINARY(22) NULL;
ALTER TABLE MyTable2 ADD CONSTRAINT DF_MyTable2_MyColumn2 DEFAULT 0x000102 FOR MyColumn2;
ALTER TABLE MyTable2 ADD CONSTRAINT PK_MyTable2 PRIMARY KEY (MyColumn1);';
-- QUERY END: MSSQLAlterTableQuery

-- QUERY START: MSSQLDropTypeQuery
EXEC sp_executesql N'DROP TYPE _DNDBTTemp_MyUserDefinedType1;';
-- QUERY END: MSSQLDropTypeQuery

-- QUERY START: MSSQLCreateIndexQuery
EXEC sp_executesql N'CREATE UNIQUE INDEX IDX_MyTable2_MyIndex1
ON MyTable2 (MyColumn1, MyColumn2);';
-- QUERY END: MSSQLCreateIndexQuery

-- QUERY START: MSSQLCreateForeignKeyQuery
EXEC sp_executesql N'ALTER TABLE MyTable1 ADD CONSTRAINT FK_MyTable1_MyColumn1_MyTable2_MyColumn1 FOREIGN KEY (MyColumn1)
    REFERENCES MyTable2 (MyColumn1)
    ON UPDATE NO ACTION ON DELETE CASCADE;';
-- QUERY END: MSSQLCreateForeignKeyQuery

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

    COMMIT TRANSACTION;
END TRY
BEGIN CATCH;
    ROLLBACK TRANSACTION;

    DECLARE @ErrorMessage NVARCHAR(MAX), @ErrorSeverity INT, @ErrorState INT;
    SELECT @ErrorMessage = ERROR_MESSAGE() + ' Line ' + CAST(ERROR_LINE() AS NVARCHAR(5)), @ErrorSeverity = ERROR_SEVERITY(), @ErrorState = ERROR_STATE();
    RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
END CATCH;