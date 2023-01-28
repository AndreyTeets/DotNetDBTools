--ID:#{1C36AE77-B7E4-40C3-824F-BD20DC270A14}#
CREATE unique INDEX "IDX_SomeTable1"
    ON Contacts using GiST (length(Email || "phone"))
    include (Col4, "Col3")