--ID:#{474CD761-2522-4529-9D20-2B94115F9626}#
CREATE TABLE "MyTable3"
(
    --ID:#{726F503A-D944-46EE-A0FF-6A2C2FAAB46E}#
    "MyColumn1" BIGINT NOT NULL DEFAULT 444,

    --ID:#{169824E1-8B74-4B60-AF17-99656D6DBBEE}#
    "MyColumn2" BYTEA NOT NULL,

    --ID:#{FD288E38-35BA-4BB1-ACE3-597C99EF26C7}#
    CONSTRAINT "UQ_MyTable3_MyColumns12" UNIQUE ("MyColumn1", "MyColumn2")
)
