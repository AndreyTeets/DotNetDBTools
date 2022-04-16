DROP TABLE IF EXISTS [_MyTable2];

CREATE TABLE [_MyTable2]
(
    [MyColumn1] INTEGER NOT NULL PRIMARY KEY,
    [MyColumn2] BLOB
);

INSERT INTO [_MyTable2] ([MyColumn1], [MyColumn2])
SELECT [MyColumn1], [MyColumn2] FROM [MyTable2]