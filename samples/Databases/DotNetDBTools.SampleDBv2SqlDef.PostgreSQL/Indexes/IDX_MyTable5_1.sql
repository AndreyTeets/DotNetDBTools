--ID:#{F7F367DA-088F-48DD-BAD5-2A14A0E77F66}#
CREATE INDEX "IDX_MyTable5_1"
    ON "MyTable5" USING BTREE ((length("MyColumn2" || "MyColumn1") + 1))
    INCLUDE ("MyColumn4", "MyColumn3", "MyColumn5")