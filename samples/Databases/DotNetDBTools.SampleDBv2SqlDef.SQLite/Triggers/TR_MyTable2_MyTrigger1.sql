--ID:#{EE64FFC3-5536-4624-BEAF-BC3A61D06A1A}#
CREATE TRIGGER [TR_MyTable2_MyTrigger1]
AFTER INSERT
ON [MyTable2]
FOR EACH ROW
BEGIN
    INSERT INTO [MyTable4]([MyColumn1])
    VALUES(NEW.[MyColumn1NewName]);
END