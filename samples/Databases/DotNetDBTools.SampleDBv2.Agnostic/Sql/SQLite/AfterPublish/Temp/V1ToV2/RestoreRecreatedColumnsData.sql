CREATE TABLE IF NOT EXISTS [_MyTable2]
(
    [MyColumn1] INTEGER NOT NULL PRIMARY KEY,
    [MyColumn2] BLOB
);

UPDATE [MyTable2] SET
    [MyColumn2] = (SELECT [t].[MyColumn2] FROM [_MyTable2] AS [t] WHERE [t].[MyColumn1] = [MyTable2].[MyColumn1NewName]);

DROP TABLE [_MyTable2]