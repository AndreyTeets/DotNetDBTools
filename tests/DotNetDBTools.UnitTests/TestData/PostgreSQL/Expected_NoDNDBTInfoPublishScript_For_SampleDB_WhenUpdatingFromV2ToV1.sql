DO $DNDBTGeneratedScriptTransactionBlock$
BEGIN

-- QUERY START: PostgreSQLDropTriggerQuery
EXECUTE 'DROP TRIGGER "TR_MyTable2_MyTrigger1" ON "MyTable2";';
-- QUERY END: PostgreSQLDropTriggerQuery

-- QUERY START: GenericQuery
EXECUTE 'DROP FUNCTION "TR_MyTable2_MyTrigger1_Handler";';
-- QUERY END: GenericQuery

-- QUERY START: GenericQuery
EXECUTE 'DROP VIEW "MyView1";';
-- QUERY END: GenericQuery

-- QUERY START: PostgreSQLDropForeignKeyQuery
EXECUTE 'ALTER TABLE "MyTable1NewName" DROP CONSTRAINT "FK_MyTable1_MyColumn1_MyTable2_MyColumn1";';
-- QUERY END: PostgreSQLDropForeignKeyQuery

-- QUERY START: PostgreSQLDropForeignKeyQuery
EXECUTE 'ALTER TABLE "MyTable2" DROP CONSTRAINT "FK_MyTable2_MyColumns12_MyTable3_MyColumns12";';
-- QUERY END: PostgreSQLDropForeignKeyQuery

-- QUERY START: PostgreSQLDropIndexQuery
EXECUTE 'DROP INDEX "IDX_MyTable2_MyIndex1";';
-- QUERY END: PostgreSQLDropIndexQuery

-- QUERY START: PostgreSQLRenameTypeToTempQuery
EXECUTE 'ALTER TYPE "MyCompositeType1" RENAME TO "_DNDBTTemp_MyCompositeType1";';
-- QUERY END: PostgreSQLRenameTypeToTempQuery

-- QUERY START: PostgreSQLRenameTypeToTempQuery
EXECUTE 'ALTER TYPE "MyEnumType1" RENAME TO "_DNDBTTemp_MyEnumType1";';
-- QUERY END: PostgreSQLRenameTypeToTempQuery

-- QUERY START: PostgreSQLRenameTypeToTempQuery
EXECUTE 'ALTER DOMAIN "MyDomain1" RENAME TO "_DNDBTTemp_MyDomain1";';
-- QUERY END: PostgreSQLRenameTypeToTempQuery

-- QUERY START: PostgreSQLRenameTypeToTempQuery
EXECUTE 'ALTER TYPE "MyRangeType1" RENAME TO "_DNDBTTemp_MyRangeType1";
ALTER FUNCTION "MyRangeType1"(TIMESTAMPTZ,TIMESTAMPTZ) RENAME TO "_DNDBTTemp_MyRangeType1";
ALTER FUNCTION "MyRangeType1" RENAME TO "_DNDBTTemp_MyRangeType1";
ALTER TYPE "MyRangeType1_multirange" RENAME TO "_DNDBTTemp_MyRangeType1_multirange";
ALTER FUNCTION "MyRangeType1_multirange"() RENAME TO "_DNDBTTemp_MyRangeType1_multirange";
ALTER FUNCTION "MyRangeType1_multirange"("_DNDBTTemp_MyRangeType1") RENAME TO "_DNDBTTemp_MyRangeType1_multirange";
ALTER FUNCTION "MyRangeType1_multirange" RENAME TO "_DNDBTTemp_MyRangeType1_multirange";';
-- QUERY END: PostgreSQLRenameTypeToTempQuery

-- QUERY START: PostgreSQLCreateCompositeTypeQuery
EXECUTE 'CREATE TYPE "MyCompositeType1" AS
(
    "MyAttribute1" VARCHAR(100),
    "MyAttribute2" INT
);';
-- QUERY END: PostgreSQLCreateCompositeTypeQuery

-- QUERY START: PostgreSQLCreateEnumTypeQuery
EXECUTE 'CREATE TYPE "MyEnumType1" AS ENUM
(
    ''Label1'',
    ''Label2''
);';
-- QUERY END: PostgreSQLCreateEnumTypeQuery

-- QUERY START: PostgreSQLCreateDomainTypeQuery
EXECUTE 'CREATE DOMAIN "MyDomain1" AS VARCHAR(100)    NULL
    CONSTRAINT "MyDomain1_CK1" CHECK (value = lower(value))
    CONSTRAINT "MyDomain1_CK2" CHECK (char_length(value) > 3);';
-- QUERY END: PostgreSQLCreateDomainTypeQuery

-- QUERY START: PostgreSQLCreateRangeTypeQuery
EXECUTE 'CREATE TYPE "MyRangeType1" AS RANGE
(
    SUBTYPE = TIMESTAMP,
    SUBTYPE_OPCLASS = "timestamp_ops",
    MULTIRANGE_TYPE_NAME = "MyRangeType1_multirange"
);';
-- QUERY END: PostgreSQLCreateRangeTypeQuery

-- QUERY START: PostgreSQLDropTableQuery
EXECUTE 'DROP TABLE "MyTable3";';
-- QUERY END: PostgreSQLDropTableQuery

-- QUERY START: PostgreSQLAlterTableQuery
EXECUTE 'ALTER TABLE "MyTable1NewName" RENAME TO "MyTable1";
ALTER TABLE "MyTable1" DROP CONSTRAINT "CK_MyTable1_MyCheck1";
ALTER TABLE "MyTable1" ALTER COLUMN "MyColumn1" DROP DEFAULT;
ALTER TABLE "MyTable1" ALTER COLUMN "MyColumn1" SET DATA TYPE INT
    USING ("MyColumn1"::text::INT);
ALTER TABLE "MyTable1" ALTER COLUMN "MyColumn1" SET NOT NULL;
ALTER TABLE "MyTable1" ALTER COLUMN "MyColumn1" SET DEFAULT 15;
ALTER TABLE "MyTable1" ADD COLUMN "MyColumn2" VARCHAR(10) NOT NULL;
ALTER TABLE "MyTable1" ALTER COLUMN "MyColumn2" SET DEFAULT ''33'';
ALTER TABLE "MyTable1" ADD COLUMN "MyColumn3" INT GENERATED ALWAYS AS IDENTITY NOT NULL;
ALTER TABLE "MyTable1" ADD CONSTRAINT "PK_MyTable1" PRIMARY KEY ("MyColumn3");
ALTER TABLE "MyTable1" ADD CONSTRAINT "UQ_MyTable1_MyColumn2" UNIQUE ("MyColumn2");
ALTER TABLE "MyTable1" ADD CONSTRAINT "CK_MyTable1_MyCheck1" CHECK ("MyColumn4" >= 0);';
-- QUERY END: PostgreSQLAlterTableQuery

-- QUERY START: PostgreSQLAlterTableQuery
EXECUTE '
ALTER TABLE "MyTable2" DROP CONSTRAINT "PK_MyTable2";
ALTER TABLE "MyTable2" ALTER COLUMN "MyColumn2" DROP DEFAULT;
ALTER TABLE "MyTable2" DROP COLUMN "MyColumn2";
ALTER TABLE "MyTable2" ALTER COLUMN "MyColumn1NewName" DROP DEFAULT;
ALTER TABLE "MyTable2" RENAME COLUMN "MyColumn1NewName" TO "MyColumn1";
ALTER TABLE "MyTable2" ALTER COLUMN "MyColumn1" SET DATA TYPE INT
    USING ("MyColumn1"::text::INT);
ALTER TABLE "MyTable2" ALTER COLUMN "MyColumn1" SET DEFAULT 333;
ALTER TABLE "MyTable2" ADD COLUMN "MyColumn2" BYTEA NULL;
ALTER TABLE "MyTable2" ALTER COLUMN "MyColumn2" SET DEFAULT ''\x000102'';
ALTER TABLE "MyTable2" ADD CONSTRAINT "PK_MyTable2" PRIMARY KEY ("MyColumn1");';
-- QUERY END: PostgreSQLAlterTableQuery

-- QUERY START: PostgreSQLAlterTableQuery
EXECUTE '
ALTER TABLE "MyTable5" ALTER COLUMN "MyColumn2" SET DATA TYPE "MyCompositeType1"
    USING ("MyColumn2"::text::"MyCompositeType1");
ALTER TABLE "MyTable5" ALTER COLUMN "MyColumn4" SET DATA TYPE "MyDomain1"
    USING ("MyColumn4"::text::"MyDomain1");
ALTER TABLE "MyTable5" ALTER COLUMN "MyColumn5" SET DATA TYPE "MyEnumType1"
    USING ("MyColumn5"::text::"MyEnumType1");
ALTER TABLE "MyTable5" ALTER COLUMN "MyColumn6" SET DATA TYPE "MyRangeType1"
    USING ("MyColumn6"::text::"MyRangeType1");';
-- QUERY END: PostgreSQLAlterTableQuery

-- QUERY START: PostgreSQLDropTypeQuery
EXECUTE 'DROP TYPE "_DNDBTTemp_MyCompositeType1";';
-- QUERY END: PostgreSQLDropTypeQuery

-- QUERY START: PostgreSQLDropTypeQuery
EXECUTE 'DROP TYPE "_DNDBTTemp_MyEnumType1";';
-- QUERY END: PostgreSQLDropTypeQuery

-- QUERY START: PostgreSQLDropTypeQuery
EXECUTE 'DROP DOMAIN "_DNDBTTemp_MyDomain1";';
-- QUERY END: PostgreSQLDropTypeQuery

-- QUERY START: PostgreSQLDropTypeQuery
EXECUTE 'DROP TYPE "_DNDBTTemp_MyRangeType1";';
-- QUERY END: PostgreSQLDropTypeQuery

-- QUERY START: PostgreSQLCreateIndexQuery
EXECUTE 'CREATE UNIQUE INDEX "IDX_MyTable2_MyIndex1"
ON "MyTable2" ("MyColumn1", "MyColumn2");';
-- QUERY END: PostgreSQLCreateIndexQuery

-- QUERY START: PostgreSQLCreateForeignKeyQuery
EXECUTE 'ALTER TABLE "MyTable1" ADD CONSTRAINT "FK_MyTable1_MyColumn1_MyTable2_MyColumn1" FOREIGN KEY ("MyColumn1")
    REFERENCES "MyTable2" ("MyColumn1")
    ON UPDATE NO ACTION ON DELETE CASCADE;';
-- QUERY END: PostgreSQLCreateForeignKeyQuery

-- QUERY START: GenericQuery
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
$FuncBody$;';
-- QUERY END: GenericQuery

-- QUERY START: GenericQuery
EXECUTE 'CREATE VIEW "MyView1" AS
SELECT
    t1."MyColumn1",
    t1."MyColumn4",
    t2."MyColumn2"
FROM "MyTable1" t1
LEFT JOIN "MyTable2" t2
    ON t2."MyColumn1" = t1."MyColumn1";';
-- QUERY END: GenericQuery

-- QUERY START: PostgreSQLCreateTriggerQuery
EXECUTE 'CREATE TRIGGER "TR_MyTable2_MyTrigger1"
AFTER INSERT
ON "MyTable2"
FOR EACH ROW
EXECUTE FUNCTION "TR_MyTable2_MyTrigger1_Handler"();';
-- QUERY END: PostgreSQLCreateTriggerQuery

END;
$DNDBTGeneratedScriptTransactionBlock$