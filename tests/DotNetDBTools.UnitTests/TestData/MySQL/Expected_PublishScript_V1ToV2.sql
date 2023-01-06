-- QUERY START: GenericQuery
DROP TABLE IF EXISTS `_MyTable2`;

CREATE TABLE `_MyTable2`
(
    `MyColumn1` BIGINT NOT NULL PRIMARY KEY,
    `MyColumn2` VARBINARY(22)
);

INSERT INTO `_MyTable2` (`MyColumn1`, `MyColumn2`)
SELECT `MyColumn1`, `MyColumn2` FROM `MyTable2`;
-- QUERY END: GenericQuery

-- QUERY START: InsertDNDBTScriptExecutionRecordQuery
INSERT INTO `DNDBTScriptExecutions`
(
    `ID`,
    `Type`,
    `Name`,
    `Code`,
    `MinDbVersionToExecute`,
    `MaxDbVersionToExecute`,
    `ExecutedOnDbVersion`
)
VALUES
(
    '7f72f0df-4eda-4063-99d8-99c1f37819d2',
    'BeforePublishOnce',
    'SaveRecreatedColumnsData',
    'DROP TABLE IF EXISTS `_MyTable2`;

CREATE TABLE `_MyTable2`
(
    `MyColumn1` BIGINT NOT NULL PRIMARY KEY,
    `MyColumn2` VARBINARY(22)
);

INSERT INTO `_MyTable2` (`MyColumn1`, `MyColumn2`)
SELECT `MyColumn1`, `MyColumn2` FROM `MyTable2`',
    1,
    1,
    1
);
-- QUERY END: InsertDNDBTScriptExecutionRecordQuery

-- QUERY START: DropTriggerQuery
DROP TRIGGER `TR_MyTable2_MyTrigger1`;
-- QUERY END: DropTriggerQuery

-- QUERY START: DeleteDNDBTDbObjectRecordQuery
DELETE FROM `DNDBTDbObjects`
WHERE `ID` = 'ee64ffc3-5536-4624-beaf-bc3a61d06a1a';
-- QUERY END: DeleteDNDBTDbObjectRecordQuery

-- QUERY START: DropViewQuery
DROP VIEW `MyView1`;
-- QUERY END: DropViewQuery

-- QUERY START: DeleteDNDBTDbObjectRecordQuery
DELETE FROM `DNDBTDbObjects`
WHERE `ID` = 'e2569aae-d5da-4a77-b3cd-51adbdb272d9';
-- QUERY END: DeleteDNDBTDbObjectRecordQuery

-- QUERY START: DropForeignKeyQuery
ALTER TABLE `MyTable6` DROP CONSTRAINT `FK_MyTable6_MyTable5_CustomName`;
-- QUERY END: DropForeignKeyQuery

-- QUERY START: DeleteDNDBTDbObjectRecordQuery
DELETE FROM `DNDBTDbObjects`
WHERE `ID` = 'ae453b22-d270-41fc-8184-9ac26b7a0569';
-- QUERY END: DeleteDNDBTDbObjectRecordQuery

-- QUERY START: DropForeignKeyQuery
ALTER TABLE `MyTable1` DROP CONSTRAINT `FK_MyTable1_MyColumn1_MyTable2_MyColumn1`;
-- QUERY END: DropForeignKeyQuery

-- QUERY START: DeleteDNDBTDbObjectRecordQuery
DELETE FROM `DNDBTDbObjects`
WHERE `ID` = 'd11b2a53-32db-432f-bb6b-f91788844ba9';
-- QUERY END: DeleteDNDBTDbObjectRecordQuery

-- QUERY START: DropIndexQuery
DROP INDEX `UQ_MyTable1_MyColumn4` ON `MyTable1`;
-- QUERY END: DropIndexQuery

-- QUERY START: DeleteDNDBTDbObjectRecordQuery
DELETE FROM `DNDBTDbObjects`
WHERE `ID` = 'f3f08522-26ee-4950-9135-22edf2e4e0cf';
-- QUERY END: DeleteDNDBTDbObjectRecordQuery

-- QUERY START: DropIndexQuery
DROP INDEX `IDX_MyTable2_MyIndex1` ON `MyTable2`;
-- QUERY END: DropIndexQuery

-- QUERY START: DeleteDNDBTDbObjectRecordQuery
DELETE FROM `DNDBTDbObjects`
WHERE `ID` = '74390b3c-bc39-4860-a42e-12baa400f927';
-- QUERY END: DeleteDNDBTDbObjectRecordQuery

-- QUERY START: DropIndexQuery
DROP INDEX `IDX_MyTable5_CustomName` ON `MyTable5`;
-- QUERY END: DropIndexQuery

-- QUERY START: DeleteDNDBTDbObjectRecordQuery
DELETE FROM `DNDBTDbObjects`
WHERE `ID` = '1d632285-9914-4c5d-98e6-a618a99bd799';
-- QUERY END: DeleteDNDBTDbObjectRecordQuery

-- QUERY START: DropIndexQuery
DROP INDEX `UQ_MyTable5_CustomName` ON `MyTable5`;
-- QUERY END: DropIndexQuery

-- QUERY START: DeleteDNDBTDbObjectRecordQuery
DELETE FROM `DNDBTDbObjects`
WHERE `ID` = '5293b58a-9f63-4f0f-8d6f-18416ebbd751';
-- QUERY END: DeleteDNDBTDbObjectRecordQuery

-- QUERY START: DropTableQuery
DROP TABLE `MyTable6`;
-- QUERY END: DropTableQuery

-- QUERY START: DeleteDNDBTDbObjectRecordQuery
DELETE FROM `DNDBTDbObjects`
WHERE `ID` = 'bfa08c82-5c8f-4ab4-bd41-1f1d85cf3c85';
-- QUERY END: DeleteDNDBTDbObjectRecordQuery

-- QUERY START: DeleteDNDBTDbObjectRecordQuery
DELETE FROM `DNDBTDbObjects`
WHERE `ID` = 'a402e2b7-c826-4cfd-a304-97c9bc346ba2';
-- QUERY END: DeleteDNDBTDbObjectRecordQuery

-- QUERY START: DeleteDNDBTDbObjectRecordQuery
DELETE FROM `DNDBTDbObjects`
WHERE `ID` = 'f3064a8c-346a-4b3d-af2c-d967b39841e4';
-- QUERY END: DeleteDNDBTDbObjectRecordQuery

-- QUERY START: AlterTableQuery
RENAME TABLE `MyTable1` TO `MyTable1NewName`;

ALTER TABLE `MyTable1NewName` DROP CONSTRAINT `CK_MyTable1_MyCheck1`;
ALTER TABLE `MyTable1NewName` DROP PRIMARY KEY,
    MODIFY COLUMN `MyColumn3` INT NOT NULL;
ALTER TABLE `MyTable1NewName` DROP COLUMN `MyColumn2`;
ALTER TABLE `MyTable1NewName` DROP COLUMN `MyColumn3`;
ALTER TABLE `MyTable1NewName` MODIFY COLUMN `MyColumn1` BIGINT NULL DEFAULT 15;
ALTER TABLE `MyTable1NewName` ALTER COLUMN `MyColumn4` DROP DEFAULT;
ALTER TABLE `MyTable1NewName` ADD CONSTRAINT `CK_MyTable1_MyCheck1` CHECK (MyColumn4 >= 1);
-- QUERY END: AlterTableQuery

-- QUERY START: DeleteDNDBTDbObjectRecordQuery
DELETE FROM `DNDBTDbObjects`
WHERE `ID` = 'eb9c59b5-bc7e-49d7-adaa-f5600b6a19a2';
-- QUERY END: DeleteDNDBTDbObjectRecordQuery

-- QUERY START: DeleteDNDBTDbObjectRecordQuery
DELETE FROM `DNDBTDbObjects`
WHERE `ID` = '37a45def-f4a0-4be7-8bfb-8fbed4a7d705';
-- QUERY END: DeleteDNDBTDbObjectRecordQuery

-- QUERY START: DeleteDNDBTDbObjectRecordQuery
DELETE FROM `DNDBTDbObjects`
WHERE `ID` = 'fe68ee3d-09d0-40ac-93f9-5e441fbb4f70';
-- QUERY END: DeleteDNDBTDbObjectRecordQuery

-- QUERY START: DeleteDNDBTDbObjectRecordQuery
DELETE FROM `DNDBTDbObjects`
WHERE `ID` = '6e95de30-e01a-4fb4-b8b7-8f0c40bb682c';
-- QUERY END: DeleteDNDBTDbObjectRecordQuery

-- QUERY START: UpdateDNDBTDbObjectRecordQuery
UPDATE `DNDBTDbObjects` SET
    `Name` = 'MyTable1NewName',
    `Code` = NULL
WHERE `ID` = '299675e6-4faa-4d0f-a36a-224306ba5bcb';
-- QUERY END: UpdateDNDBTDbObjectRecordQuery

-- QUERY START: UpdateDNDBTDbObjectRecordQuery
UPDATE `DNDBTDbObjects` SET
    `Name` = 'MyColumn1',
    `Code` = '15'
WHERE `ID` = 'a2f2a4de-1337-4594-ae41-72ed4d05f317';
-- QUERY END: UpdateDNDBTDbObjectRecordQuery

-- QUERY START: UpdateDNDBTDbObjectRecordQuery
UPDATE `DNDBTDbObjects` SET
    `Name` = 'MyColumn4',
    `Code` = NULL
WHERE `ID` = '867ac528-e87e-4c93-b6e3-dd2fcbbb837f';
-- QUERY END: UpdateDNDBTDbObjectRecordQuery

-- QUERY START: InsertDNDBTDbObjectRecordQuery
INSERT INTO `DNDBTDbObjects`
(
    `ID`,
    `ParentID`,
    `Type`,
    `Name`,
    `Code`
)
VALUES
(
    'eb9c59b5-bc7e-49d7-adaa-f5600b6a19a2',
    '299675e6-4faa-4d0f-a36a-224306ba5bcb',
    'CheckConstraint',
    'CK_MyTable1_MyCheck1',
    'CHECK (MyColumn4 >= 1)'
);
-- QUERY END: InsertDNDBTDbObjectRecordQuery

-- QUERY START: AlterTableQuery
ALTER TABLE `MyTable2` RENAME COLUMN `MyColumn1` TO `MyColumn1NewName`;

ALTER TABLE `MyTable2` DROP PRIMARY KEY;
ALTER TABLE `MyTable2` DROP COLUMN `MyColumn2`;
ALTER TABLE `MyTable2` MODIFY COLUMN `MyColumn1NewName` BIGINT NOT NULL DEFAULT 333;
ALTER TABLE `MyTable2` ADD COLUMN `MyColumn2` VARBINARY(22) NULL DEFAULT (0x000408);
ALTER TABLE `MyTable2` ADD COLUMN `MyColumn3` BIGINT NULL;
ALTER TABLE `MyTable2` ADD COLUMN `MyColumn4` VARBINARY(50) NULL;
ALTER TABLE `MyTable2` ADD PRIMARY KEY (`MyColumn1NewName`);
-- QUERY END: AlterTableQuery

-- QUERY START: DeleteDNDBTDbObjectRecordQuery
DELETE FROM `DNDBTDbObjects`
WHERE `ID` = '3a43615b-40b3-4a13-99e7-93af7c56e8ce';
-- QUERY END: DeleteDNDBTDbObjectRecordQuery

-- QUERY START: DeleteDNDBTDbObjectRecordQuery
DELETE FROM `DNDBTDbObjects`
WHERE `ID` = '5a0d1926-3270-4eb2-92eb-00be56c7af23';
-- QUERY END: DeleteDNDBTDbObjectRecordQuery

-- QUERY START: UpdateDNDBTDbObjectRecordQuery
UPDATE `DNDBTDbObjects` SET
    `Name` = 'MyTable2',
    `Code` = NULL
WHERE `ID` = 'bfb9030c-a8c3-4882-9c42-1c6ad025cf8f';
-- QUERY END: UpdateDNDBTDbObjectRecordQuery

-- QUERY START: UpdateDNDBTDbObjectRecordQuery
UPDATE `DNDBTDbObjects` SET
    `Name` = 'MyColumn1NewName',
    `Code` = '333'
WHERE `ID` = 'c480f22f-7c01-4f41-b282-35e9f5cd1fe3';
-- QUERY END: UpdateDNDBTDbObjectRecordQuery

-- QUERY START: InsertDNDBTDbObjectRecordQuery
INSERT INTO `DNDBTDbObjects`
(
    `ID`,
    `ParentID`,
    `Type`,
    `Name`,
    `Code`
)
VALUES
(
    'c2df19c2-e029-4014-8a5b-4ab42fecb6b8',
    'bfb9030c-a8c3-4882-9c42-1c6ad025cf8f',
    'Column',
    'MyColumn2',
    '(0x000408)'
);
-- QUERY END: InsertDNDBTDbObjectRecordQuery

-- QUERY START: InsertDNDBTDbObjectRecordQuery
INSERT INTO `DNDBTDbObjects`
(
    `ID`,
    `ParentID`,
    `Type`,
    `Name`,
    `Code`
)
VALUES
(
    '99bc3f49-3151-4f52-87f7-104b424ed7bf',
    'bfb9030c-a8c3-4882-9c42-1c6ad025cf8f',
    'Column',
    'MyColumn3',
    NULL
);
-- QUERY END: InsertDNDBTDbObjectRecordQuery

-- QUERY START: InsertDNDBTDbObjectRecordQuery
INSERT INTO `DNDBTDbObjects`
(
    `ID`,
    `ParentID`,
    `Type`,
    `Name`,
    `Code`
)
VALUES
(
    '87950a3f-2072-42db-ac3c-a4e85b79720d',
    'bfb9030c-a8c3-4882-9c42-1c6ad025cf8f',
    'Column',
    'MyColumn4',
    NULL
);
-- QUERY END: InsertDNDBTDbObjectRecordQuery

-- QUERY START: InsertDNDBTDbObjectRecordQuery
INSERT INTO `DNDBTDbObjects`
(
    `ID`,
    `ParentID`,
    `Type`,
    `Name`,
    `Code`
)
VALUES
(
    '3a43615b-40b3-4a13-99e7-93af7c56e8ce',
    'bfb9030c-a8c3-4882-9c42-1c6ad025cf8f',
    'PrimaryKey',
    'PK_MyTable2',
    NULL
);
-- QUERY END: InsertDNDBTDbObjectRecordQuery

-- QUERY START: AlterTableQuery
ALTER TABLE `MyTable5` DROP PRIMARY KEY;
-- QUERY END: AlterTableQuery

-- QUERY START: DeleteDNDBTDbObjectRecordQuery
DELETE FROM `DNDBTDbObjects`
WHERE `ID` = '79384d48-a39b-4a22-900e-066b2ca67ba2';
-- QUERY END: DeleteDNDBTDbObjectRecordQuery

-- QUERY START: UpdateDNDBTDbObjectRecordQuery
UPDATE `DNDBTDbObjects` SET
    `Name` = 'MyTable5',
    `Code` = NULL
WHERE `ID` = '6ca51f29-c1bc-4349-b9c1-6f1ea170f162';
-- QUERY END: UpdateDNDBTDbObjectRecordQuery

-- QUERY START: CreateTableQuery
CREATE TABLE `MyTable3`
(
    `MyColumn1` BIGINT NOT NULL DEFAULT 444,
    `MyColumn2` VARBINARY(50) NOT NULL
);
-- QUERY END: CreateTableQuery

-- QUERY START: InsertDNDBTDbObjectRecordQuery
INSERT INTO `DNDBTDbObjects`
(
    `ID`,
    `ParentID`,
    `Type`,
    `Name`,
    `Code`
)
VALUES
(
    '474cd761-2522-4529-9d20-2b94115f9626',
    NULL,
    'Table',
    'MyTable3',
    NULL
);
-- QUERY END: InsertDNDBTDbObjectRecordQuery

-- QUERY START: InsertDNDBTDbObjectRecordQuery
INSERT INTO `DNDBTDbObjects`
(
    `ID`,
    `ParentID`,
    `Type`,
    `Name`,
    `Code`
)
VALUES
(
    '726f503a-d944-46ee-a0ff-6a2c2faab46e',
    '474cd761-2522-4529-9d20-2b94115f9626',
    'Column',
    'MyColumn1',
    '444'
);
-- QUERY END: InsertDNDBTDbObjectRecordQuery

-- QUERY START: InsertDNDBTDbObjectRecordQuery
INSERT INTO `DNDBTDbObjects`
(
    `ID`,
    `ParentID`,
    `Type`,
    `Name`,
    `Code`
)
VALUES
(
    '169824e1-8b74-4b60-af17-99656d6dbbee',
    '474cd761-2522-4529-9d20-2b94115f9626',
    'Column',
    'MyColumn2',
    NULL
);
-- QUERY END: InsertDNDBTDbObjectRecordQuery

-- QUERY START: CreateIndexQuery
CREATE UNIQUE INDEX `UQ_MyTable3_MyColumns12`
    ON `MyTable3` (`MyColumn1`, `MyColumn2`);
-- QUERY END: CreateIndexQuery

-- QUERY START: InsertDNDBTDbObjectRecordQuery
INSERT INTO `DNDBTDbObjects`
(
    `ID`,
    `ParentID`,
    `Type`,
    `Name`,
    `Code`
)
VALUES
(
    'fd288e38-35ba-4bb1-ace3-597c99ef26c7',
    '474cd761-2522-4529-9d20-2b94115f9626',
    'Index',
    'UQ_MyTable3_MyColumns12',
    NULL
);
-- QUERY END: InsertDNDBTDbObjectRecordQuery

-- QUERY START: CreateIndexQuery
CREATE UNIQUE INDEX `IDX_MyTable2_MyIndex1`
    ON `MyTable2` (`MyColumn1NewName`, `MyColumn2`);
-- QUERY END: CreateIndexQuery

-- QUERY START: InsertDNDBTDbObjectRecordQuery
INSERT INTO `DNDBTDbObjects`
(
    `ID`,
    `ParentID`,
    `Type`,
    `Name`,
    `Code`
)
VALUES
(
    '74390b3c-bc39-4860-a42e-12baa400f927',
    'bfb9030c-a8c3-4882-9c42-1c6ad025cf8f',
    'Index',
    'IDX_MyTable2_MyIndex1',
    NULL
);
-- QUERY END: InsertDNDBTDbObjectRecordQuery

-- QUERY START: CreateForeignKeyQuery
ALTER TABLE `MyTable1NewName` ADD CONSTRAINT `FK_MyTable1_MyColumn1_MyTable2_MyColumn1` FOREIGN KEY (`MyColumn1`)
        REFERENCES `MyTable2` (`MyColumn1NewName`)
        ON UPDATE NO ACTION ON DELETE SET NULL;
-- QUERY END: CreateForeignKeyQuery

-- QUERY START: InsertDNDBTDbObjectRecordQuery
INSERT INTO `DNDBTDbObjects`
(
    `ID`,
    `ParentID`,
    `Type`,
    `Name`,
    `Code`
)
VALUES
(
    'd11b2a53-32db-432f-bb6b-f91788844ba9',
    '299675e6-4faa-4d0f-a36a-224306ba5bcb',
    'ForeignKey',
    'FK_MyTable1_MyColumn1_MyTable2_MyColumn1',
    NULL
);
-- QUERY END: InsertDNDBTDbObjectRecordQuery

-- QUERY START: CreateForeignKeyQuery
ALTER TABLE `MyTable2` ADD CONSTRAINT `FK_MyTable2_MyColumns34_MyTable3_MyColumns12` FOREIGN KEY (`MyColumn3`, `MyColumn4`)
        REFERENCES `MyTable3` (`MyColumn1`, `MyColumn2`)
        ON UPDATE NO ACTION ON DELETE SET DEFAULT;
-- QUERY END: CreateForeignKeyQuery

-- QUERY START: InsertDNDBTDbObjectRecordQuery
INSERT INTO `DNDBTDbObjects`
(
    `ID`,
    `ParentID`,
    `Type`,
    `Name`,
    `Code`
)
VALUES
(
    '480f3508-9d51-4190-88aa-45bc20e49119',
    'bfb9030c-a8c3-4882-9c42-1c6ad025cf8f',
    'ForeignKey',
    'FK_MyTable2_MyColumns34_MyTable3_MyColumns12',
    NULL
);
-- QUERY END: InsertDNDBTDbObjectRecordQuery

-- QUERY START: CreateViewQuery
CREATE VIEW MyView1 AS
SELECT
    t1.MyColumn1,
    t1.MyColumn4,
    t2.MyColumn2
FROM MyTable1NewName t1
LEFT JOIN MyTable2 t2
    ON t2.MyColumn1NewName = t1.MyColumn1;
-- QUERY END: CreateViewQuery

-- QUERY START: InsertDNDBTDbObjectRecordQuery
INSERT INTO `DNDBTDbObjects`
(
    `ID`,
    `ParentID`,
    `Type`,
    `Name`,
    `Code`
)
VALUES
(
    'e2569aae-d5da-4a77-b3cd-51adbdb272d9',
    NULL,
    'View',
    'MyView1',
    'CREATE VIEW MyView1 AS
SELECT
    t1.MyColumn1,
    t1.MyColumn4,
    t2.MyColumn2
FROM MyTable1NewName t1
LEFT JOIN MyTable2 t2
    ON t2.MyColumn1NewName = t1.MyColumn1'
);
-- QUERY END: InsertDNDBTDbObjectRecordQuery

-- QUERY START: CreateTriggerQuery
CREATE TRIGGER `TR_MyTable2_MyTrigger1`
AFTER INSERT
ON `MyTable2`
FOR EACH ROW
BEGIN
    INSERT INTO `MyTable4`(`MyColumn1`)
    VALUES (NEW.`MyColumn1NewName`);
END;
-- QUERY END: CreateTriggerQuery

-- QUERY START: InsertDNDBTDbObjectRecordQuery
INSERT INTO `DNDBTDbObjects`
(
    `ID`,
    `ParentID`,
    `Type`,
    `Name`,
    `Code`
)
VALUES
(
    'ee64ffc3-5536-4624-beaf-bc3a61d06a1a',
    'bfb9030c-a8c3-4882-9c42-1c6ad025cf8f',
    'Trigger',
    'TR_MyTable2_MyTrigger1',
    'CREATE TRIGGER `TR_MyTable2_MyTrigger1`
AFTER INSERT
ON `MyTable2`
FOR EACH ROW
BEGIN
    INSERT INTO `MyTable4`(`MyColumn1`)
    VALUES (NEW.`MyColumn1NewName`);
END'
);
-- QUERY END: InsertDNDBTDbObjectRecordQuery

-- QUERY START: GenericQuery
CREATE TABLE IF NOT EXISTS `_MyTable2`
(
    `MyColumn1` BIGINT NOT NULL PRIMARY KEY,
    `MyColumn2` VARBINARY(22)
);

UPDATE `MyTable2`
INNER JOIN `_MyTable2` AS `t`
    ON `t`.`MyColumn1` = `MyTable2`.`MyColumn1NewName`
SET `MyTable2`.`MyColumn2` = `t`.`MyColumn2`;

DROP TABLE `_MyTable2`;
-- QUERY END: GenericQuery

-- QUERY START: InsertDNDBTScriptExecutionRecordQuery
INSERT INTO `DNDBTScriptExecutions`
(
    `ID`,
    `Type`,
    `Name`,
    `Code`,
    `MinDbVersionToExecute`,
    `MaxDbVersionToExecute`,
    `ExecutedOnDbVersion`
)
VALUES
(
    '8ccaf36e-e587-466e-86f7-45c0061ae521',
    'AfterPublishOnce',
    'RestoreRecreatedColumnsData',
    'CREATE TABLE IF NOT EXISTS `_MyTable2`
(
    `MyColumn1` BIGINT NOT NULL PRIMARY KEY,
    `MyColumn2` VARBINARY(22)
);

UPDATE `MyTable2`
INNER JOIN `_MyTable2` AS `t`
    ON `t`.`MyColumn1` = `MyTable2`.`MyColumn1NewName`
SET `MyTable2`.`MyColumn2` = `t`.`MyColumn2`;

DROP TABLE `_MyTable2`',
    1,
    1,
    1
);
-- QUERY END: InsertDNDBTScriptExecutionRecordQuery

-- QUERY START: UpdateDNDBTDbAttributesRecordQuery
UPDATE `DNDBTDbAttributes` SET
    `Version` = 2;
-- QUERY END: UpdateDNDBTDbAttributesRecordQuery