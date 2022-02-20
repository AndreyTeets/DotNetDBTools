-- QUERY START: MySQLDropTriggerQuery
DROP TRIGGER `TR_MyTable2_MyTrigger1`;
-- QUERY END: MySQLDropTriggerQuery

-- QUERY START: GenericQuery
DROP VIEW `MyView1`;
-- QUERY END: GenericQuery

-- QUERY START: MySQLDropForeignKeyQuery
ALTER TABLE `MyTable1NewName` DROP CONSTRAINT `FK_MyTable1_MyColumn1_MyTable2_MyColumn1`;
-- QUERY END: MySQLDropForeignKeyQuery

-- QUERY START: MySQLDropForeignKeyQuery
ALTER TABLE `MyTable2` DROP CONSTRAINT `FK_MyTable2_MyColumns12_MyTable3_MyColumns12`;
-- QUERY END: MySQLDropForeignKeyQuery

-- QUERY START: MySQLDropIndexQuery
DROP INDEX `UQ_MyTable3_MyColumns12` ON `MyTable3`;
-- QUERY END: MySQLDropIndexQuery

-- QUERY START: MySQLDropIndexQuery
DROP INDEX `IDX_MyTable2_MyIndex1` ON `MyTable2`;
-- QUERY END: MySQLDropIndexQuery

-- QUERY START: MySQLDropTableQuery
DROP TABLE `MyTable3`;
-- QUERY END: MySQLDropTableQuery

-- QUERY START: MySQLAlterTableQuery
RENAME TABLE `MyTable1NewName` TO `MyTable1`;
ALTER TABLE `MyTable1` DROP CONSTRAINT `CK_MyTable1_MyCheck1`;
ALTER TABLE `MyTable1` ALTER COLUMN `MyColumn1` DROP DEFAULT;
ALTER TABLE `MyTable1` MODIFY COLUMN `MyColumn1` INT NOT NULL;
ALTER TABLE `MyTable1` ALTER COLUMN `MyColumn1` SET DEFAULT (15);
ALTER TABLE `MyTable1` ADD COLUMN `MyColumn2` VARCHAR(7) NOT NULL;
ALTER TABLE `MyTable1` ALTER COLUMN `MyColumn2` SET DEFAULT ('33');
ALTER TABLE `MyTable1` ADD COLUMN `MyColumn3` INT AUTO_INCREMENT NOT NULL,
     ADD PRIMARY KEY (`MyColumn3`);
ALTER TABLE `MyTable1` ADD CONSTRAINT `CK_MyTable1_MyCheck1` CHECK (MyColumn4 >= 0);
-- QUERY END: MySQLAlterTableQuery

-- QUERY START: MySQLAlterTableQuery

ALTER TABLE `MyTable2` DROP PRIMARY KEY;
ALTER TABLE `MyTable2` ALTER COLUMN `MyColumn2` DROP DEFAULT;
ALTER TABLE `MyTable2` DROP COLUMN `MyColumn2`;
ALTER TABLE `MyTable2` ALTER COLUMN `MyColumn1NewName` DROP DEFAULT;
ALTER TABLE `MyTable2` RENAME COLUMN `MyColumn1NewName` TO `MyColumn1`;
ALTER TABLE `MyTable2` MODIFY COLUMN `MyColumn1` INT NOT NULL;
ALTER TABLE `MyTable2` ALTER COLUMN `MyColumn1` SET DEFAULT (333);
ALTER TABLE `MyTable2` ADD COLUMN `MyColumn2` BINARY(6) NULL;
ALTER TABLE `MyTable2` ALTER COLUMN `MyColumn2` SET DEFAULT (0x000102);
ALTER TABLE `MyTable2` ADD PRIMARY KEY (`MyColumn1`);
-- QUERY END: MySQLAlterTableQuery

-- QUERY START: MySQLCreateIndexQuery
CREATE UNIQUE INDEX `UQ_MyTable1_MyColumn2`
ON `MyTable1` (`MyColumn2`);
-- QUERY END: MySQLCreateIndexQuery

-- QUERY START: MySQLCreateIndexQuery
CREATE UNIQUE INDEX `IDX_MyTable2_MyIndex1`
ON `MyTable2` (`MyColumn1`, `MyColumn2`);
-- QUERY END: MySQLCreateIndexQuery

-- QUERY START: MySQLCreateForeignKeyQuery
ALTER TABLE `MyTable1` ADD CONSTRAINT `FK_MyTable1_MyColumn1_MyTable2_MyColumn1` FOREIGN KEY (`MyColumn1`)
    REFERENCES `MyTable2` (`MyColumn1`)
    ON UPDATE NO ACTION ON DELETE CASCADE;
-- QUERY END: MySQLCreateForeignKeyQuery

-- QUERY START: GenericQuery
CREATE VIEW MyView1 AS
SELECT
    t1.MyColumn1,
    t1.MyColumn4,
    t2.MyColumn2
FROM MyTable1 t1
LEFT JOIN MyTable2 t2
    ON t2.MyColumn1 = t1.MyColumn1;
-- QUERY END: GenericQuery

-- QUERY START: MySQLCreateTriggerQuery
CREATE TRIGGER `TR_MyTable2_MyTrigger1`
AFTER INSERT
ON `MyTable2`
FOR EACH ROW
BEGIN
    INSERT INTO `MyTable4`(`MyColumn1`)
    VALUES (NEW.`MyColumn1`);
END;
-- QUERY END: MySQLCreateTriggerQuery