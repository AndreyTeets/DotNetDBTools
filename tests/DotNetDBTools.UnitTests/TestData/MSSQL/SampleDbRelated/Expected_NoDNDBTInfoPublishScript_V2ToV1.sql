SET NOCOUNT ON;
SET XACT_ABORT ON;
BEGIN TRY;
    BEGIN TRANSACTION;

-- QUERY START: DropTriggerQuery
EXEC sp_executesql N'DROP TRIGGER [TR_MyTable2_MyTrigger1];';
-- QUERY END: DropTriggerQuery

-- QUERY START: DropForeignKeyQuery
EXEC sp_executesql N'ALTER TABLE [MyTable1NewName] DROP CONSTRAINT [FK_MyTable1_MyColumn1_MyTable2_MyColumn1];';
-- QUERY END: DropForeignKeyQuery

-- QUERY START: DropForeignKeyQuery
EXEC sp_executesql N'ALTER TABLE [MyTable2] DROP CONSTRAINT [FK_MyTable2_MyColumns34_MyTable3_MyColumns12];';
-- QUERY END: DropForeignKeyQuery

-- QUERY START: DropIndexQuery
EXEC sp_executesql N'DROP INDEX [IDX_MyTable2_MyIndex1] ON [MyTable2];';
-- QUERY END: DropIndexQuery

-- QUERY START: DropViewQuery
EXEC sp_executesql N'DROP VIEW [MyView1];';
-- QUERY END: DropViewQuery

-- QUERY START: DropTableQuery
EXEC sp_executesql N'DROP TABLE [MyTable3];';
-- QUERY END: DropTableQuery

-- QUERY START: RenameUserDefinedDataTypeQuery
EXEC sp_executesql N'EXEC sp_rename ''MyUserDefinedType1'', ''_DNDBTTemp_MyUserDefinedType1'', ''USERDATATYPE'';';
-- QUERY END: RenameUserDefinedDataTypeQuery

-- QUERY START: CreateTypeQuery
EXEC sp_executesql N'CREATE TYPE [MyUserDefinedType1] FROM NVARCHAR(100);';
-- QUERY END: CreateTypeQuery

-- QUERY START: AlterTableQuery
EXEC sp_executesql N'EXEC sp_rename ''MyTable1NewName'', ''MyTable1'';

ALTER TABLE [MyTable1] DROP CONSTRAINT [CK_MyTable1_MyCheck1];
ALTER TABLE [MyTable1] DROP CONSTRAINT [DF_MyTable1NewName_MyColumn1];
ALTER TABLE [MyTable1] ALTER COLUMN [MyColumn1] INT NOT NULL;
ALTER TABLE [MyTable1] ADD CONSTRAINT [DF_MyTable1_MyColumn1] DEFAULT 15 FOR [MyColumn1];
ALTER TABLE [MyTable1] ADD CONSTRAINT [DF_MyTable1_MyColumn4] DEFAULT 736 FOR [MyColumn4];
ALTER TABLE [MyTable1] DROP CONSTRAINT [DF_MyTable1NewName_MyColumn5];
ALTER TABLE [MyTable1] ADD CONSTRAINT [DF_MyTable1_MyColumn5] DEFAULT ''some text'' FOR [MyColumn5];
ALTER TABLE [MyTable1] ADD [MyColumn2] NVARCHAR(MAX) NULL;
ALTER TABLE [MyTable1] ADD [MyColumn3] INT IDENTITY NOT NULL;
ALTER TABLE [MyTable1] ADD CONSTRAINT [PK_MyTable1] PRIMARY KEY ([MyColumn3]);
ALTER TABLE [MyTable1] ADD CONSTRAINT [UQ_MyTable1_MyColumn4] UNIQUE ([MyColumn4]);
ALTER TABLE [MyTable1] ADD CONSTRAINT [CK_MyTable1_MyCheck1] CHECK (MyColumn4 >= 0);';
-- QUERY END: AlterTableQuery

-- QUERY START: AlterTableQuery
EXEC sp_executesql N'EXEC sp_rename ''MyTable2.MyColumn1NewName'', ''MyColumn1'', ''COLUMN'';

ALTER TABLE [MyTable2] DROP CONSTRAINT [PK_MyTable2_CustomName];
ALTER TABLE [MyTable2] DROP CONSTRAINT [DF_MyTable2_MyColumn2];
ALTER TABLE [MyTable2] DROP COLUMN [MyColumn2];
ALTER TABLE [MyTable2] DROP COLUMN [MyColumn3];
ALTER TABLE [MyTable2] DROP COLUMN [MyColumn4];
ALTER TABLE [MyTable2] DROP CONSTRAINT [DF_MyTable2_MyColumn1NewName];
ALTER TABLE [MyTable2] ALTER COLUMN [MyColumn1] INT NOT NULL;
ALTER TABLE [MyTable2] ADD CONSTRAINT [DF_MyTable2_MyColumn1] DEFAULT 333 FOR [MyColumn1];
ALTER TABLE [MyTable2] ADD [MyColumn2] VARBINARY(22) NULL CONSTRAINT [DF_MyTable2_MyColumn2] DEFAULT 0x000408 WITH VALUES;
ALTER TABLE [MyTable2] ADD CONSTRAINT [PK_MyTable2_CustomName] PRIMARY KEY ([MyColumn1]);';
-- QUERY END: AlterTableQuery

-- QUERY START: AlterTableQuery
EXEC sp_executesql N'ALTER TABLE [MyTable5] DROP CONSTRAINT [DF_MyTable5_MyColumn1];
ALTER TABLE [MyTable5] ADD CONSTRAINT [DF_MyTable5_MyColumn1] DEFAULT ABs(-15) FOR [MyColumn1];
ALTER TABLE [MyTable5] DROP CONSTRAINT [DF_MyTable5_MyColumn101];
ALTER TABLE [MyTable5] ALTER COLUMN [MyColumn101] MyUserDefinedType1 NULL;
ALTER TABLE [MyTable5] ADD CONSTRAINT [DF_MyTable5_MyColumn101] DEFAULT ''cc'' FOR [MyColumn101];
ALTER TABLE [MyTable5] ADD CONSTRAINT [PK_MyTable5_CustomName] PRIMARY KEY ([MyColumn2], [MyColumn1]);
ALTER TABLE [MyTable5] ADD CONSTRAINT [UQ_MyTable5_CustomName] UNIQUE ([MyColumn6], [MyColumn3], [MyColumn7]);';
-- QUERY END: AlterTableQuery

-- QUERY START: DropTypeQuery
EXEC sp_executesql N'DROP TYPE [_DNDBTTemp_MyUserDefinedType1];';
-- QUERY END: DropTypeQuery

-- QUERY START: CreateTableQuery
EXEC sp_executesql N'CREATE TABLE [MyTable6]
(
    [MyColumn1] NCHAR(4) NULL,
    [MyColumn2] INT NULL
);';
-- QUERY END: CreateTableQuery

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

-- QUERY START: CreateIndexQuery
EXEC sp_executesql N'CREATE UNIQUE INDEX [IDX_MyTable2_MyIndex1]
    ON [MyTable2] ([MyColumn1], [MyColumn2]);';
-- QUERY END: CreateIndexQuery

-- QUERY START: CreateIndexQuery
EXEC sp_executesql N'CREATE INDEX [IDX_MyTable5_CustomName]
    ON [MyTable5] ([MyColumn8]);';
-- QUERY END: CreateIndexQuery

-- QUERY START: CreateForeignKeyQuery
EXEC sp_executesql N'ALTER TABLE [MyTable6] ADD CONSTRAINT [FK_MyTable6_MyTable5_CustomName] FOREIGN KEY ([MyColumn1], [MyColumn2])
        REFERENCES [MyTable5] ([MyColumn2], [MyColumn1])
        ON UPDATE NO ACTION ON DELETE NO ACTION;';
-- QUERY END: CreateForeignKeyQuery

-- QUERY START: CreateForeignKeyQuery
EXEC sp_executesql N'ALTER TABLE [MyTable1] ADD CONSTRAINT [FK_MyTable1_MyColumn1_MyTable2_MyColumn1] FOREIGN KEY ([MyColumn1])
        REFERENCES [MyTable2] ([MyColumn1])
        ON UPDATE NO ACTION ON DELETE CASCADE;';
-- QUERY END: CreateForeignKeyQuery

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

    COMMIT TRANSACTION;
END TRY
BEGIN CATCH;
    ROLLBACK TRANSACTION;

    DECLARE @ErrorMessage NVARCHAR(MAX), @ErrorSeverity INT, @ErrorState INT;
    SELECT @ErrorMessage = ERROR_MESSAGE() + ' Line ' + CAST(ERROR_LINE() AS NVARCHAR(5)), @ErrorSeverity = ERROR_SEVERITY(), @ErrorState = ERROR_STATE();
    RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
END CATCH;