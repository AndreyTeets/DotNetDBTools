-- QUERY START: MySQLDropTriggerQuery
DROP TRIGGER `TR_MyTable2_MyTrigger1`;
-- QUERY END: MySQLDropTriggerQuery

-- QUERY START: MySQLDeleteDNDBTSysInfoQuery
DELETE FROM `DNDBTDbObjects`
WHERE `ID` = 'ee64ffc3-5536-4624-beaf-bc3a61d06a1a';
-- QUERY END: MySQLDeleteDNDBTSysInfoQuery

-- QUERY START: GenericQuery
DROP VIEW `MyView1`;
-- QUERY END: GenericQuery

-- QUERY START: MySQLDeleteDNDBTSysInfoQuery
DELETE FROM `DNDBTDbObjects`
WHERE `ID` = 'e2569aae-d5da-4a77-b3cd-51adbdb272d9';
-- QUERY END: MySQLDeleteDNDBTSysInfoQuery

-- QUERY START: MySQLDropForeignKeyQuery
ALTER TABLE `MyTable1` DROP CONSTRAINT `FK_MyTable1_MyColumn1_MyTable2_MyColumn1`;
-- QUERY END: MySQLDropForeignKeyQuery

-- QUERY START: MySQLDeleteDNDBTSysInfoQuery
DELETE FROM `DNDBTDbObjects`
WHERE `ID` = 'd11b2a53-32db-432f-bb6b-f91788844ba9';
-- QUERY END: MySQLDeleteDNDBTSysInfoQuery

-- QUERY START: MySQLDropIndexQuery
DROP INDEX `UQ_MyTable1_MyColumn2` ON `MyTable1`;
-- QUERY END: MySQLDropIndexQuery

-- QUERY START: MySQLDeleteDNDBTSysInfoQuery
DELETE FROM `DNDBTDbObjects`
WHERE `ID` = 'f3f08522-26ee-4950-9135-22edf2e4e0cf';
-- QUERY END: MySQLDeleteDNDBTSysInfoQuery

-- QUERY START: MySQLDropIndexQuery
DROP INDEX `IDX_MyTable2_MyIndex1` ON `MyTable2`;
-- QUERY END: MySQLDropIndexQuery

-- QUERY START: MySQLDeleteDNDBTSysInfoQuery
DELETE FROM `DNDBTDbObjects`
WHERE `ID` = '74390b3c-bc39-4860-a42e-12baa400f927';
-- QUERY END: MySQLDeleteDNDBTSysInfoQuery

-- QUERY START: MySQLAlterTableQuery
RENAME TABLE `MyTable1` TO `MyTable1NewName`;
ALTER TABLE `MyTable1NewName` DROP CONSTRAINT `CK_MyTable1_MyCheck1`;
ALTER TABLE `MyTable1NewName` DROP PRIMARY KEY,
    MODIFY COLUMN `MyColumn3` INT NOT NULL;
ALTER TABLE `MyTable1NewName` ALTER COLUMN `MyColumn2` DROP DEFAULT;
ALTER TABLE `MyTable1NewName` DROP COLUMN `MyColumn2`;
ALTER TABLE `MyTable1NewName` DROP COLUMN `MyColumn3`;
ALTER TABLE `MyTable1NewName` ALTER COLUMN `MyColumn1` DROP DEFAULT;
ALTER TABLE `MyTable1NewName` MODIFY COLUMN `MyColumn1` BIGINT NULL;
ALTER TABLE `MyTable1NewName` ALTER COLUMN `MyColumn1` SET DEFAULT (15);
ALTER TABLE `MyTable1NewName` ADD CONSTRAINT `CK_MyTable1_MyCheck1` CHECK (MyColumn4 >= 1);
-- QUERY END: MySQLAlterTableQuery

-- QUERY START: MySQLDeleteDNDBTSysInfoQuery
DELETE FROM `DNDBTDbObjects`
WHERE `ID` = 'eb9c59b5-bc7e-49d7-adaa-f5600b6a19a2';
-- QUERY END: MySQLDeleteDNDBTSysInfoQuery

-- QUERY START: MySQLDeleteDNDBTSysInfoQuery
DELETE FROM `DNDBTDbObjects`
WHERE `ID` = '37a45def-f4a0-4be7-8bfb-8fbed4a7d705';
-- QUERY END: MySQLDeleteDNDBTSysInfoQuery

-- QUERY START: MySQLDeleteDNDBTSysInfoQuery
DELETE FROM `DNDBTDbObjects`
WHERE `ID` = 'fe68ee3d-09d0-40ac-93f9-5e441fbb4f70';
-- QUERY END: MySQLDeleteDNDBTSysInfoQuery

-- QUERY START: MySQLDeleteDNDBTSysInfoQuery
DELETE FROM `DNDBTDbObjects`
WHERE `ID` = '6e95de30-e01a-4fb4-b8b7-8f0c40bb682c';
-- QUERY END: MySQLDeleteDNDBTSysInfoQuery

-- QUERY START: MySQLUpdateDNDBTSysInfoQuery
UPDATE `DNDBTDbObjects` SET
    `Name` = 'MyTable1NewName',
    `Code` = NULL
WHERE `ID` = '299675e6-4faa-4d0f-a36a-224306ba5bcb';
-- QUERY END: MySQLUpdateDNDBTSysInfoQuery

-- QUERY START: MySQLUpdateDNDBTSysInfoQuery
UPDATE `DNDBTDbObjects` SET
    `Name` = 'MyColumn1',
    `Code` = NULL
WHERE `ID` = 'a2f2a4de-1337-4594-ae41-72ed4d05f317';
-- QUERY END: MySQLUpdateDNDBTSysInfoQuery

-- QUERY START: MySQLInsertDNDBTSysInfoQuery
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
-- QUERY END: MySQLInsertDNDBTSysInfoQuery

-- QUERY START: MySQLAlterTableQuery

ALTER TABLE `MyTable2` DROP PRIMARY KEY;
ALTER TABLE `MyTable2` ALTER COLUMN `MyColumn2` DROP DEFAULT;
ALTER TABLE `MyTable2` DROP COLUMN `MyColumn2`;
ALTER TABLE `MyTable2` ALTER COLUMN `MyColumn1` DROP DEFAULT;
ALTER TABLE `MyTable2` RENAME COLUMN `MyColumn1` TO `MyColumn1NewName`;
ALTER TABLE `MyTable2` MODIFY COLUMN `MyColumn1NewName` BIGINT NOT NULL;
ALTER TABLE `MyTable2` ALTER COLUMN `MyColumn1NewName` SET DEFAULT (333);
ALTER TABLE `MyTable2` ADD COLUMN `MyColumn2` VARBINARY(9) NULL;
ALTER TABLE `MyTable2` ALTER COLUMN `MyColumn2` SET DEFAULT (0x000102);
ALTER TABLE `MyTable2` ADD PRIMARY KEY (`MyColumn1NewName`);
-- QUERY END: MySQLAlterTableQuery

-- QUERY START: MySQLDeleteDNDBTSysInfoQuery
DELETE FROM `DNDBTDbObjects`
WHERE `ID` = '3a43615b-40b3-4a13-99e7-93af7c56e8ce';
-- QUERY END: MySQLDeleteDNDBTSysInfoQuery

-- QUERY START: MySQLDeleteDNDBTSysInfoQuery
DELETE FROM `DNDBTDbObjects`
WHERE `ID` = '5a0d1926-3270-4eb2-92eb-00be56c7af23';
-- QUERY END: MySQLDeleteDNDBTSysInfoQuery

-- QUERY START: MySQLUpdateDNDBTSysInfoQuery
UPDATE `DNDBTDbObjects` SET
    `Name` = 'MyTable2',
    `Code` = NULL
WHERE `ID` = 'bfb9030c-a8c3-4882-9c42-1c6ad025cf8f';
-- QUERY END: MySQLUpdateDNDBTSysInfoQuery

-- QUERY START: MySQLUpdateDNDBTSysInfoQuery
UPDATE `DNDBTDbObjects` SET
    `Name` = 'MyColumn1NewName',
    `Code` = NULL
WHERE `ID` = 'c480f22f-7c01-4f41-b282-35e9f5cd1fe3';
-- QUERY END: MySQLUpdateDNDBTSysInfoQuery

-- QUERY START: MySQLInsertDNDBTSysInfoQuery
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
    NULL
);
-- QUERY END: MySQLInsertDNDBTSysInfoQuery

-- QUERY START: MySQLInsertDNDBTSysInfoQuery
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
-- QUERY END: MySQLInsertDNDBTSysInfoQuery

-- QUERY START: MySQLCreateTableQuery
CREATE TABLE `MyTable3`
(
    `MyColumn1` BIGINT NOT NULL DEFAULT (333),
    `MyColumn2` VARBINARY(9) NOT NULL
);
-- QUERY END: MySQLCreateTableQuery

-- QUERY START: MySQLInsertDNDBTSysInfoQuery
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
-- QUERY END: MySQLInsertDNDBTSysInfoQuery

-- QUERY START: MySQLInsertDNDBTSysInfoQuery
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
    NULL
);
-- QUERY END: MySQLInsertDNDBTSysInfoQuery

-- QUERY START: MySQLInsertDNDBTSysInfoQuery
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
-- QUERY END: MySQLInsertDNDBTSysInfoQuery

-- QUERY START: MySQLCreateIndexQuery
CREATE UNIQUE INDEX `UQ_MyTable3_MyColumns12`
ON `MyTable3` (`MyColumn1`, `MyColumn2`);
-- QUERY END: MySQLCreateIndexQuery

-- QUERY START: MySQLInsertDNDBTSysInfoQuery
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
-- QUERY END: MySQLInsertDNDBTSysInfoQuery

-- QUERY START: MySQLCreateIndexQuery
CREATE UNIQUE INDEX `IDX_MyTable2_MyIndex1`
ON `MyTable2` (`MyColumn1NewName`, `MyColumn2`);
-- QUERY END: MySQLCreateIndexQuery

-- QUERY START: MySQLInsertDNDBTSysInfoQuery
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
-- QUERY END: MySQLInsertDNDBTSysInfoQuery

-- QUERY START: MySQLCreateForeignKeyQuery
ALTER TABLE `MyTable1NewName` ADD CONSTRAINT `FK_MyTable1_MyColumn1_MyTable2_MyColumn1` FOREIGN KEY (`MyColumn1`)
    REFERENCES `MyTable2` (`MyColumn1NewName`)
    ON UPDATE NO ACTION ON DELETE SET NULL;
-- QUERY END: MySQLCreateForeignKeyQuery

-- QUERY START: MySQLInsertDNDBTSysInfoQuery
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
-- QUERY END: MySQLInsertDNDBTSysInfoQuery

-- QUERY START: MySQLCreateForeignKeyQuery
ALTER TABLE `MyTable2` ADD CONSTRAINT `FK_MyTable2_MyColumns12_MyTable3_MyColumns12` FOREIGN KEY (`MyColumn1NewName`, `MyColumn2`)
    REFERENCES `MyTable3` (`MyColumn1`, `MyColumn2`)
    ON UPDATE NO ACTION ON DELETE SET DEFAULT;
-- QUERY END: MySQLCreateForeignKeyQuery

-- QUERY START: MySQLInsertDNDBTSysInfoQuery
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
    'FK_MyTable2_MyColumns12_MyTable3_MyColumns12',
    NULL
);
-- QUERY END: MySQLInsertDNDBTSysInfoQuery

-- QUERY START: GenericQuery
CREATE VIEW MyView1 AS
SELECT
    t1.MyColumn1,
    t1.MyColumn4,
    t2.MyColumn2
FROM MyTable1NewName t1
LEFT JOIN MyTable2 t2
    ON t2.MyColumn1NewName = t1.MyColumn1;
-- QUERY END: GenericQuery

-- QUERY START: MySQLInsertDNDBTSysInfoQuery
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
    ON t2.MyColumn1NewName = t1.MyColumn1;'
);
-- QUERY END: MySQLInsertDNDBTSysInfoQuery

-- QUERY START: MySQLCreateTriggerQuery
CREATE TRIGGER `TR_MyTable2_MyTrigger1`
AFTER INSERT
ON `MyTable2`
FOR EACH ROW
BEGIN
    INSERT INTO `MyTable4`(`MyColumn1`)
    VALUES (NEW.`MyColumn1NewName`);
END;
-- QUERY END: MySQLCreateTriggerQuery

-- QUERY START: MySQLInsertDNDBTSysInfoQuery
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
END;'
);
-- QUERY END: MySQLInsertDNDBTSysInfoQuery