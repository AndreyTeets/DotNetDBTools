IF OBJECT_ID('_MyTable2', 'U') IS NOT NULL
BEGIN
    UPDATE [MyTable2] SET
        [MyColumn2] = [t].[MyColumn2]
    FROM [_MyTable2] AS [t]
    WHERE [MyTable2].[MyColumn1NewName] = [t].[MyColumn1];

    DROP TABLE [_MyTable2];
END;