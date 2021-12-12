-- QUERY START: MySQLDropForeignKeyQuery
ALTER TABLE `MyTable1` DROP CONSTRAINT `FK_MyTable1_MyColumn1_MyTable2_MyColumn1`;
-- QUERY END: MySQLDropForeignKeyQuery

-- QUERY START: MySQLDeleteDNDBTSysInfoQuery
DELETE FROM `DNDBTDbObjects`
WHERE `ID` = 'd11b2a53-32db-432f-bb6b-f91788844ba9';
-- QUERY END: MySQLDeleteDNDBTSysInfoQuery

-- QUERY START: MySQLAlterTableQuery
RENAME TABLE `MyTable1` TO `MyTable1NewName`;
ALTER TABLE `MyTable1NewName` DROP CONSTRAINT `UQ_MyTable1_MyColumn2`;
ALTER TABLE `MyTable1NewName` DROP PRIMARY KEY,
    MODIFY COLUMN `MyColumn3` INT NOT NULL;
ALTER TABLE `MyTable1NewName` ALTER COLUMN `MyColumn2` DROP DEFAULT;
ALTER TABLE `MyTable1NewName` DROP COLUMN `MyColumn2`;
ALTER TABLE `MyTable1NewName` DROP COLUMN `MyColumn3`;
ALTER TABLE `MyTable1NewName` ALTER COLUMN `MyColumn1` DROP DEFAULT;
ALTER TABLE `MyTable1NewName` MODIFY COLUMN `MyColumn1` BIGINT NULL;
ALTER TABLE `MyTable1NewName` ALTER COLUMN `MyColumn1` SET DEFAULT (15);
-- QUERY END: MySQLAlterTableQuery

-- QUERY START: MySQLDeleteDNDBTSysInfoQuery
DELETE FROM `DNDBTDbObjects`
WHERE `ID` = 'f3f08522-26ee-4950-9135-22edf2e4e0cf';
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
    `ExtraInfo` = NULL
WHERE `ID` = '299675e6-4faa-4d0f-a36a-224306ba5bcb';
-- QUERY END: MySQLUpdateDNDBTSysInfoQuery

-- QUERY START: MySQLUpdateDNDBTSysInfoQuery
UPDATE `DNDBTDbObjects` SET
    `Name` = 'MyColumn1',
    `ExtraInfo` = NULL
WHERE `ID` = 'a2f2a4de-1337-4594-ae41-72ed4d05f317';
-- QUERY END: MySQLUpdateDNDBTSysInfoQuery

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
    `ExtraInfo` = NULL
WHERE `ID` = 'bfb9030c-a8c3-4882-9c42-1c6ad025cf8f';
-- QUERY END: MySQLUpdateDNDBTSysInfoQuery

-- QUERY START: MySQLUpdateDNDBTSysInfoQuery
UPDATE `DNDBTDbObjects` SET
    `Name` = 'MyColumn1NewName',
    `ExtraInfo` = NULL
WHERE `ID` = 'c480f22f-7c01-4f41-b282-35e9f5cd1fe3';
-- QUERY END: MySQLUpdateDNDBTSysInfoQuery

-- QUERY START: MySQLInsertDNDBTSysInfoQuery
INSERT INTO `DNDBTDbObjects`
(
    `ID`,
    `ParentID`,
    `Type`,
    `Name`,
    `ExtraInfo`
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
    `ExtraInfo`
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
    `MyColumn2` VARBINARY(9) NOT NULL,
    CONSTRAINT `UQ_MyTable3_MyColumns12` UNIQUE (`MyColumn1`, `MyColumn2`)
);
-- QUERY END: MySQLCreateTableQuery

-- QUERY START: MySQLInsertDNDBTSysInfoQuery
INSERT INTO `DNDBTDbObjects`
(
    `ID`,
    `ParentID`,
    `Type`,
    `Name`,
    `ExtraInfo`
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
    `ExtraInfo`
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
    `ExtraInfo`
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

-- QUERY START: MySQLInsertDNDBTSysInfoQuery
INSERT INTO `DNDBTDbObjects`
(
    `ID`,
    `ParentID`,
    `Type`,
    `Name`,
    `ExtraInfo`
)
VALUES
(
    'fd288e38-35ba-4bb1-ace3-597c99ef26c7',
    '474cd761-2522-4529-9d20-2b94115f9626',
    'UniqueConstraint',
    'UQ_MyTable3_MyColumns12',
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
    `ExtraInfo`
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
    `ExtraInfo`
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