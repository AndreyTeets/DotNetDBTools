DO $DNDBTGeneratedScriptTransactionBlock$
BEGIN

-- QUERY START: DropTriggerQuery
EXECUTE 'DROP TRIGGER "TR_MyTable2_MyTrigger1" ON "MyTable2";';
-- QUERY END: DropTriggerQuery

-- QUERY START: DropFunctionQuery
EXECUTE 'DROP FUNCTION "TR_MyTable2_MyTrigger1_Handler";';
-- QUERY END: DropFunctionQuery

-- QUERY START: DropViewQuery
EXECUTE 'DROP VIEW "MyView1";';
-- QUERY END: DropViewQuery

-- QUERY START: DropForeignKeyQuery
EXECUTE 'ALTER TABLE "MyTable1NewName"
    DROP CONSTRAINT "FK_MyTable1_MyColumn1_MyTable2_MyColumn1";';
-- QUERY END: DropForeignKeyQuery

-- QUERY START: DropForeignKeyQuery
EXECUTE 'ALTER TABLE "MyTable2"
    DROP CONSTRAINT "FK_MyTable2_MyColumns34_MyTable3_MyColumns12";';
-- QUERY END: DropForeignKeyQuery

-- QUERY START: DropIndexQuery
EXECUTE 'DROP INDEX "IDX_MyTable2_MyIndex1";';
-- QUERY END: DropIndexQuery

-- QUERY START: RenameTypeToTempQuery
EXECUTE 'ALTER TYPE "MyCompositeType1" RENAME TO "_DNDBTTemp_MyCompositeType1";';
-- QUERY END: RenameTypeToTempQuery

-- QUERY START: RenameTypeToTempQuery
EXECUTE 'ALTER TYPE "MyEnumType1" RENAME TO "_DNDBTTemp_MyEnumType1";';
-- QUERY END: RenameTypeToTempQuery

-- QUERY START: RenameTypeToTempQuery
EXECUTE 'ALTER DOMAIN "MyDomain1" RENAME TO "_DNDBTTemp_MyDomain1";';
-- QUERY END: RenameTypeToTempQuery

-- QUERY START: RenameTypeToTempQuery
EXECUTE 'DO $DNDBTPlPgSqlBlock$
BEGIN
ALTER TYPE "MyRangeType1" RENAME TO "_DNDBTTemp_MyRangeType1";
ALTER FUNCTION "MyRangeType1"(TIMESTAMPTZ,TIMESTAMPTZ) RENAME TO "_DNDBTTemp_MyRangeType1";
ALTER FUNCTION "MyRangeType1" RENAME TO "_DNDBTTemp_MyRangeType1";
IF (SELECT current_setting(''server_version_num'')::int) >= 140000 THEN
ALTER TYPE "MyRangeType1_multirange" RENAME TO "_DNDBTTemp_MyRangeType1_multirange";
ALTER FUNCTION "MyRangeType1_multirange"() RENAME TO "_DNDBTTemp_MyRangeType1_multirange";
ALTER FUNCTION "MyRangeType1_multirange"("_DNDBTTemp_MyRangeType1") RENAME TO "_DNDBTTemp_MyRangeType1_multirange";
ALTER FUNCTION "MyRangeType1_multirange" RENAME TO "_DNDBTTemp_MyRangeType1_multirange";
END IF;
END;
$DNDBTPlPgSqlBlock$';
-- QUERY END: RenameTypeToTempQuery

-- QUERY START: CreateCompositeTypeQuery
EXECUTE 'CREATE TYPE "MyCompositeType1" AS
(
    "MyAttribute1" VARCHAR(100),
    "MyAttribute2" INT
);';
-- QUERY END: CreateCompositeTypeQuery

-- QUERY START: CreateEnumTypeQuery
EXECUTE 'CREATE TYPE "MyEnumType1" AS ENUM
(
    ''Label1'',
    ''Label2''
);';
-- QUERY END: CreateEnumTypeQuery

-- QUERY START: CreateDomainTypeQuery
EXECUTE 'CREATE DOMAIN "MyDomain1" AS
    VARCHAR(100) NULL
    CONSTRAINT "MyDomain1_CK1" CHECK (value = lower(value))
    CONSTRAINT "MyDomain1_CK2" CHECK (char_length(value) > 3);';
-- QUERY END: CreateDomainTypeQuery

-- QUERY START: CreateRangeTypeQuery
EXECUTE 'DO $DNDBTPlPgSqlBlock$
BEGIN
IF (SELECT current_setting(''server_version_num'')::int) >= 140000 THEN
CREATE TYPE "MyRangeType1" AS RANGE
(
    SUBTYPE = TIMESTAMP,
    SUBTYPE_OPCLASS = "timestamp_ops",
    MULTIRANGE_TYPE_NAME = "MyRangeType1_multirange"
);
ELSE
CREATE TYPE "MyRangeType1" AS RANGE
(
    SUBTYPE = TIMESTAMP,
    SUBTYPE_OPCLASS = "timestamp_ops"
);
END IF;
END;
$DNDBTPlPgSqlBlock$';
-- QUERY END: CreateRangeTypeQuery

-- QUERY START: DropTableQuery
EXECUTE 'DROP TABLE "MyTable3";';
-- QUERY END: DropTableQuery

-- QUERY START: AlterTableQuery
EXECUTE 'ALTER TABLE "MyTable1NewName" RENAME TO "MyTable1";
ALTER TABLE "MyTable1"
    DROP CONSTRAINT "CK_MyTable1_MyCheck1",
    ALTER COLUMN "MyColumn1" SET DATA TYPE INT
        USING ("MyColumn1"::text::INT),
    ALTER COLUMN "MyColumn1" SET NOT NULL,
    ALTER COLUMN "MyColumn4" SET DEFAULT 7.36,
    ADD COLUMN "MyColumn2" VARCHAR(10) NOT NULL DEFAULT ''33'',
    ADD COLUMN "MyColumn3" INT GENERATED ALWAYS AS IDENTITY NOT NULL,
    ADD CONSTRAINT "PK_MyTable1" PRIMARY KEY ("MyColumn3"),
    ADD CONSTRAINT "UQ_MyTable1_MyColumn4" UNIQUE ("MyColumn4"),
    ADD CONSTRAINT "CK_MyTable1_MyCheck1" CHECK ("MyColumn4" >= 0);';
-- QUERY END: AlterTableQuery

-- QUERY START: AlterTableQuery
EXECUTE 'ALTER TABLE "MyTable2" RENAME COLUMN "MyColumn1NewName" TO "MyColumn1";
ALTER TABLE "MyTable2"
    DROP CONSTRAINT "PK_MyTable2_CustomName",
    DROP COLUMN "MyColumn2",
    DROP COLUMN "MyColumn3",
    DROP COLUMN "MyColumn4",
    ALTER COLUMN "MyColumn1" SET DATA TYPE INT
        USING ("MyColumn1"::text::INT),
    ADD COLUMN "MyColumn2" BYTEA NULL DEFAULT ''\x000408'',
    ADD CONSTRAINT "PK_MyTable2_CustomName" PRIMARY KEY ("MyColumn1");';
-- QUERY END: AlterTableQuery

-- QUERY START: AlterTableQuery
EXECUTE 'ALTER TABLE "MyTable5"
    ALTER COLUMN "MyColumn13" SET DATA TYPE "MyCompositeType1"
        USING ("MyColumn13"::text::"MyCompositeType1"),
    ALTER COLUMN "MyColumn14" SET DATA TYPE "MyDomain1"
        USING ("MyColumn14"::text::"MyDomain1"),
    ALTER COLUMN "MyColumn15" SET DATA TYPE "MyEnumType1"
        USING ("MyColumn15"::text::"MyEnumType1"),
    ALTER COLUMN "MyColumn16" SET DATA TYPE "MyRangeType1"
        USING ("MyColumn16"::text::"MyRangeType1"),
    ADD CONSTRAINT "PK_MyTable5_CustomName" PRIMARY KEY ("MyColumn2", "MyColumn1"),
    ADD CONSTRAINT "UQ_MyTable5_CustomName" UNIQUE ("MyColumn6", "MyColumn3", "MyColumn7");';
-- QUERY END: AlterTableQuery

-- QUERY START: CreateTableQuery
EXECUTE 'CREATE TABLE "MyTable6"
(
    "MyColumn1" CHAR(4) NULL,
    "MyColumn2" INT NULL
);';
-- QUERY END: CreateTableQuery

-- QUERY START: DropTypeQuery
EXECUTE 'DROP TYPE "_DNDBTTemp_MyCompositeType1";';
-- QUERY END: DropTypeQuery

-- QUERY START: DropTypeQuery
EXECUTE 'DROP TYPE "_DNDBTTemp_MyEnumType1";';
-- QUERY END: DropTypeQuery

-- QUERY START: DropTypeQuery
EXECUTE 'DROP DOMAIN "_DNDBTTemp_MyDomain1";';
-- QUERY END: DropTypeQuery

-- QUERY START: DropTypeQuery
EXECUTE 'DROP TYPE "_DNDBTTemp_MyRangeType1";';
-- QUERY END: DropTypeQuery

-- QUERY START: CreateIndexQuery
EXECUTE 'CREATE UNIQUE INDEX "IDX_MyTable2_MyIndex1"
    ON "MyTable2" ("MyColumn1", "MyColumn2");';
-- QUERY END: CreateIndexQuery

-- QUERY START: CreateIndexQuery
EXECUTE 'CREATE INDEX "IDX_MyTable5_CustomName"
    ON "MyTable5" ("MyColumn8");';
-- QUERY END: CreateIndexQuery

-- QUERY START: CreateForeignKeyQuery
EXECUTE 'ALTER TABLE "MyTable6"
    ADD CONSTRAINT "FK_MyTable6_MyTable5_CustomName" FOREIGN KEY ("MyColumn1", "MyColumn2")
        REFERENCES "MyTable5" ("MyColumn2", "MyColumn1")
        ON UPDATE NO ACTION ON DELETE NO ACTION;';
-- QUERY END: CreateForeignKeyQuery

-- QUERY START: CreateForeignKeyQuery
EXECUTE 'ALTER TABLE "MyTable1"
    ADD CONSTRAINT "FK_MyTable1_MyColumn1_MyTable2_MyColumn1" FOREIGN KEY ("MyColumn1")
        REFERENCES "MyTable2" ("MyColumn1")
        ON UPDATE NO ACTION ON DELETE CASCADE;';
-- QUERY END: CreateForeignKeyQuery

-- QUERY START: CreateFunctionQuery
EXECUTE 'CREATE FUNCTION "TR_MyTable2_MyTrigger1_Handler"()
RETURNS TRIGGER
LANGUAGE PLPGSQL
AS
$FuncBody$
BEGIN
    INSERT INTO "MyTable4"("MyColumn1")
    VALUES(NEW."MyColumn1");
    RETURN NULL;
END;
$FuncBody$';
-- QUERY END: CreateFunctionQuery

-- QUERY START: CreateViewQuery
EXECUTE 'CREATE VIEW "MyView1" AS
SELECT
    t1."MyColumn1",
    t1."MyColumn4",
    t2."MyColumn2"
FROM "MyTable1" t1
LEFT JOIN "MyTable2" t2
    ON t2."MyColumn1" = t1."MyColumn1"';
-- QUERY END: CreateViewQuery

-- QUERY START: CreateTriggerQuery
EXECUTE 'CREATE TRIGGER "TR_MyTable2_MyTrigger1"
AFTER INSERT
ON "MyTable2"
FOR EACH ROW
EXECUTE FUNCTION "TR_MyTable2_MyTrigger1_Handler"()';
-- QUERY END: CreateTriggerQuery

END;
$DNDBTGeneratedScriptTransactionBlock$