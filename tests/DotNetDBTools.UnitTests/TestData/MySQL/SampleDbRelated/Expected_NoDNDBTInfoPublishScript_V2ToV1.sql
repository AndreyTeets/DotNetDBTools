-- QUERY START: DropTriggerQuery
DROP TRIGGER `TR_MyTable2_MyTrigger1`;
-- QUERY END: DropTriggerQuery

-- QUERY START: DropForeignKeyQuery
ALTER TABLE `MyTable1NewName` DROP CONSTRAINT `FK_MyTable1_MyColumn1_MyTable2_MyColumn1`;
-- QUERY END: DropForeignKeyQuery

-- QUERY START: DropForeignKeyQuery
ALTER TABLE `MyTable2` DROP CONSTRAINT `FK_MyTable2_MyColumns34_MyTable3_MyColumns12`;
-- QUERY END: DropForeignKeyQuery

-- QUERY START: DropIndexQuery
DROP INDEX `UQ_MyTable3_MyColumns12` ON `MyTable3`;
-- QUERY END: DropIndexQuery

-- QUERY START: DropIndexQuery
DROP INDEX `IDX_MyTable2_MyIndex1` ON `MyTable2`;
-- QUERY END: DropIndexQuery

-- QUERY START: DropViewQuery
DROP VIEW `MyView1`;
-- QUERY END: DropViewQuery

-- QUERY START: DropTableQuery
DROP TABLE `MyTable3`;
-- QUERY END: DropTableQuery

-- QUERY START: AlterTableQuery
RENAME TABLE `MyTable1NewName` TO `MyTable1`;

ALTER TABLE `MyTable1` DROP CONSTRAINT `CK_MyTable1_MyCheck1`;
ALTER TABLE `MyTable1` ALTER COLUMN `MyColumn1` DROP DEFAULT;
ALTER TABLE `MyTable1` MODIFY COLUMN `MyColumn1` INT NOT NULL;
ALTER TABLE `MyTable1` ALTER COLUMN `MyColumn1` SET DEFAULT 15;
ALTER TABLE `MyTable1` ALTER COLUMN `MyColumn4` SET DEFAULT 736;
ALTER TABLE `MyTable1` ADD COLUMN `MyColumn2` LONGTEXT NULL;
ALTER TABLE `MyTable1` ADD COLUMN `MyColumn3` INT AUTO_INCREMENT NOT NULL,
     ADD PRIMARY KEY (`MyColumn3`);
ALTER TABLE `MyTable1` ADD CONSTRAINT `CK_MyTable1_MyCheck1` CHECK (MyColumn4 >= 0);
-- QUERY END: AlterTableQuery

-- QUERY START: AlterTableQuery
ALTER TABLE `MyTable2` RENAME COLUMN `MyColumn1NewName` TO `MyColumn1`;

ALTER TABLE `MyTable2` DROP PRIMARY KEY;
ALTER TABLE `MyTable2` DROP COLUMN `MyColumn2`;
ALTER TABLE `MyTable2` DROP COLUMN `MyColumn3`;
ALTER TABLE `MyTable2` DROP COLUMN `MyColumn4`;
ALTER TABLE `MyTable2` ALTER COLUMN `MyColumn1` DROP DEFAULT;
ALTER TABLE `MyTable2` MODIFY COLUMN `MyColumn1` INT NOT NULL;
ALTER TABLE `MyTable2` ALTER COLUMN `MyColumn1` SET DEFAULT 333;
ALTER TABLE `MyTable2` ADD COLUMN `MyColumn2` VARBINARY(22) NULL DEFAULT (0x000408);
ALTER TABLE `MyTable2` ADD PRIMARY KEY (`MyColumn1`);
-- QUERY END: AlterTableQuery

-- QUERY START: AlterTableQuery
ALTER TABLE `MyTable5` ALTER COLUMN `MyColumn1` DROP DEFAULT;
ALTER TABLE `MyTable5` ALTER COLUMN `MyColumn1` SET DEFAULT (ABs(-15));
ALTER TABLE `MyTable5` ADD PRIMARY KEY (`MyColumn2`, `MyColumn1`);
-- QUERY END: AlterTableQuery

-- QUERY START: CreateTableQuery
CREATE TABLE `MyTable6`
(
    `MyColumn1` CHAR(4) NULL,
    `MyColumn2` INT NULL
);
-- QUERY END: CreateTableQuery

-- QUERY START: CreateViewQuery
CREATE VIEW MyView1 AS
SELECT
    t1.MyColumn1,
    t1.MyColumn4,
    t2.MyColumn2
FROM MyTable1 t1
LEFT JOIN MyTable2 t2
    ON t2.MyColumn1 = t1.MyColumn1;
-- QUERY END: CreateViewQuery

-- QUERY START: CreateIndexQuery
CREATE UNIQUE INDEX `UQ_MyTable1_MyColumn4`
    ON `MyTable1` (`MyColumn4`);
-- QUERY END: CreateIndexQuery

-- QUERY START: CreateIndexQuery
CREATE UNIQUE INDEX `IDX_MyTable2_MyIndex1`
    ON `MyTable2` (`MyColumn1`, `MyColumn2`);
-- QUERY END: CreateIndexQuery

-- QUERY START: CreateIndexQuery
CREATE INDEX `IDX_MyTable5_CustomName`
    ON `MyTable5` (`MyColumn8`);
-- QUERY END: CreateIndexQuery

-- QUERY START: CreateIndexQuery
CREATE UNIQUE INDEX `UQ_MyTable5_CustomName`
    ON `MyTable5` (`MyColumn6`, `MyColumn3`, `MyColumn7`);
-- QUERY END: CreateIndexQuery

-- QUERY START: CreateForeignKeyQuery
ALTER TABLE `MyTable6` ADD CONSTRAINT `FK_MyTable6_MyTable5_CustomName` FOREIGN KEY (`MyColumn1`, `MyColumn2`)
        REFERENCES `MyTable5` (`MyColumn2`, `MyColumn1`)
        ON UPDATE NO ACTION ON DELETE NO ACTION;
-- QUERY END: CreateForeignKeyQuery

-- QUERY START: CreateForeignKeyQuery
ALTER TABLE `MyTable1` ADD CONSTRAINT `FK_MyTable1_MyColumn1_MyTable2_MyColumn1` FOREIGN KEY (`MyColumn1`)
        REFERENCES `MyTable2` (`MyColumn1`)
        ON UPDATE NO ACTION ON DELETE CASCADE;
-- QUERY END: CreateForeignKeyQuery

-- QUERY START: CreateTriggerQuery
CREATE TRIGGER `TR_MyTable2_MyTrigger1`
AFTER INSERT
ON `MyTable2`
FOR EACH ROW
BEGIN
    INSERT INTO `MyTable4`(`MyColumn1`)
    VALUES (NEW.`MyColumn1`);
END;
-- QUERY END: CreateTriggerQuery