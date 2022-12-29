--ScriptID:#{8CCAF36E-E587-466E-86F7-45C0061AE521}#
--ScriptName:#{RestoreRecreatedColumnsData}#
--ScriptType:#{AfterPublishOnce}#
--ScriptMinDbVersionToExecute:#{1}#
--ScriptMaxDbVersionToExecute:#{1}#

CREATE TABLE IF NOT EXISTS [_MyTable2]
(
    [MyColumn1] INTEGER NOT NULL PRIMARY KEY,
    [MyColumn2] BLOB
);

UPDATE [MyTable2] SET
    [MyColumn2] = (SELECT [t].[MyColumn2] FROM [_MyTable2] AS [t] WHERE [t].[MyColumn1] = [MyTable2].[MyColumn1NewName]);

DROP TABLE [_MyTable2]