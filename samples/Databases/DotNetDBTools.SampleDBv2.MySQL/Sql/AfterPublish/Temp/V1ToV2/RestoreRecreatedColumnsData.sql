CREATE TABLE IF NOT EXISTS `_MyTable2`
(
    `MyColumn1` BIGINT NOT NULL PRIMARY KEY,
    `MyColumn2` VARBINARY(22)
);

UPDATE `MyTable2`
INNER JOIN `_MyTable2` AS `t`
    ON `t`.`MyColumn1` = `MyTable2`.`MyColumn1NewName`
SET `MyTable2`.`MyColumn2` = `t`.`MyColumn2`;

DROP TABLE `_MyTable2`