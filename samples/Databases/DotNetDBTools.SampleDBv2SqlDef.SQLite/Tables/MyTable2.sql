--ID:#{BFB9030C-A8C3-4882-9C42-1C6AD025CF8F}#
CREATE TABLE MyTable2
(
    --ID:#{C480F22F-7C01-4F41-B282-35E9F5CD1FE3}#
    MyColumn1NewName INTEGER NOT NULL DEFAULT 333,

    --ID:#{C2DF19C2-E029-4014-8A5B-4AB42FECB6B8}#
    MyColumn2 BLOB DEFAULT 0x000102,

    --ID:#{3A43615B-40B3-4A13-99E7-93AF7C56E8CE}#
    CONSTRAINT PK_MyTable2 PRIMARY KEY (MyColumn1NewName),

    --ID:#{480F3508-9D51-4190-88AA-45BC20E49119}#
    CONSTRAINT FK_MyTable2_MyColumns12_MyTable3_MyColumns12 FOREIGN KEY (MyColumn1NewName, MyColumn2)
        REFERENCES MyTable3(MyColumn1, MyColumn2)
        ON UPDATE NO ACTION ON DELETE SET DEFAULT
);
