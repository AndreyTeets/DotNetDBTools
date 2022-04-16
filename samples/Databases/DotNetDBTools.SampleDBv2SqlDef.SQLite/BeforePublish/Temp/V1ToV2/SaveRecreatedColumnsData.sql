--ScriptID:#{7F72F0DF-4EDA-4063-99D8-99C1F37819D2}#
--ScriptName:#{SaveRecreatedColumnsData}#
--ScriptType:#{BeforePublishOnce}#
--ScriptMinDbVersionToExecute:#{1}#
--ScriptMaxDbVersionToExecute:#{1}#

DROP TABLE IF EXISTS [_MyTable2];

CREATE TABLE [_MyTable2]
(
    [MyColumn1] INTEGER NOT NULL PRIMARY KEY,
    [MyColumn2] BLOB
);

INSERT INTO [_MyTable2] ([MyColumn1], [MyColumn2])
SELECT [MyColumn1], [MyColumn2] FROM [MyTable2]