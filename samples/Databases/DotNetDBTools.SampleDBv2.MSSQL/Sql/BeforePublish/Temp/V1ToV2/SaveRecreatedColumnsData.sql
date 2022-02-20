IF EXISTS (SELECT * FROM [sysobjects] WHERE [name]='_MyTable2' AND [xtype]='U')
    DROP TABLE [_MyTable2];

CREATE TABLE [_MyTable2]
(
    [MyColumn1] BIGINT NOT NULL PRIMARY KEY,
    [MyColumn2] VARBINARY(22)
);

INSERT INTO [_MyTable2] ([MyColumn1], [MyColumn2])
SELECT [MyColumn1], [MyColumn2] FROM [MyTable2];