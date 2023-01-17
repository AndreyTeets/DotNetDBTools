create TABLE "Table1"
(
    "Col1" INTEGER NOT NULL DEFAULT 15,
    Col2 numeric NOT NULL DEFAULT 7.36,
    "Col3" INTEGER PRIMARY KEY NOT NULL,
    Col4 TEXT NOT NULL default 'CONSTRAINT CK_String_Check1 CHECK (Col3 >= 0),',
    Col5 TEXT NOT NULL default '$$ text''$$ $x$text$x$ $x$text$xxx$ --notcomm--ent',
    constraint UQ_Table1_Col1 UNIQUE ( "Col1" ),
    CONSTRAINT "UQ_Table1_Col2Col4" unique (Col2, "Col4"),
    constraint FK_Table1_Col1_Table2_Col1 FOREIGN KEY (Col1)
        REFERENCES Table2("Col1")
        ON UPDATE NO ACTION ON DELETE CASCADE,
    CONSTRAINT "FK_Table1_Col1Col2_Table2_Col2Col4" foreign KEY ( Col1, "Col2" ) REFERENCES Table2("Col2",Col4),
    constraint CK_Table1_Check1 CHECK ("Col2" != 'Col2 NUMERIC NOT NULL DEFAULT 7.36,'),
    CONSTRAINT "CK_Table1_Check2"
        CHECK (Col4 = 'CONSTRAINT "CK_String_Check2" CHECK ( "Col3" >= 0 ),' AND f1(f2())=' quo''te g1(g2(g3)))' AND TRUE),
    CONSTRAINT "CK_Table1_Check3" check ( "Col3" >= 0 )
);

--ID:#{DC36AE77-B7E4-40C3-824F-BD20DC270A14}#
CREATE FUNCTION "TR_MyTable2_MyTrigger1_Handler"()
RETURNS TRIGGER
LANGUAGE PLPGSQL
AS
$FuncBody$
DECLARE
    i INT = 4;
BEGIN
    --comm--ent$NotFuncBody$;
    INSERT INTO "MyTable4"("MyColumn1")
    VALUES(NEW."MyColumn1" || '$NotFuncBody$' || '$$');

    i = (SELECT "MyFunc2"(44));
    i = (select c1 FROM "MyView2" LEFT JOIN LATERAL (SELECT * from "MyTable2") t2 ON true INNER JOIN MyView3 ON TRUE);

    RETURN NULL;
END;
$FuncBody$;

CREATE TRIGGER "TR_MyTable2_MyTrigger1"
AFTER INSERT
ON "MyTable2"
--comm--ent;
FOR EACH ROW
EXECUTE FUNCTION "TR_MyTable2_MyTrigger1_Handler"()