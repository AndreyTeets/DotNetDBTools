DO $DNDBTGeneratedScriptTransactionBlock$
BEGIN

-- QUERY START: GenericQuery
EXECUTE 'DROP TABLE IF EXISTS "_MyTable2";

CREATE TABLE "_MyTable2"
(
    "MyColumn1" BIGINT NOT NULL PRIMARY KEY,
    "MyColumn2" BYTEA
);

INSERT INTO "_MyTable2" ("MyColumn1", "MyColumn2")
SELECT "MyColumn1", "MyColumn2" FROM "MyTable2"';
-- QUERY END: GenericQuery

-- QUERY START: InsertDNDBTScriptExecutionRecordQuery
EXECUTE 'INSERT INTO "DNDBTScriptExecutions"
(
    "ID",
    "Type",
    "Name",
    "Text",
    "MinDbVersionToExecute",
    "MaxDbVersionToExecute",
    "ExecutedOnDbVersion"
)
VALUES
(
    ''7f72f0df-4eda-4063-99d8-99c1f37819d2'',
    ''BeforePublishOnce'',
    ''SaveRecreatedColumnsData'',
    ''DROP TABLE IF EXISTS "_MyTable2";

CREATE TABLE "_MyTable2"
(
    "MyColumn1" BIGINT NOT NULL PRIMARY KEY,
    "MyColumn2" BYTEA
);

INSERT INTO "_MyTable2" ("MyColumn1", "MyColumn2")
SELECT "MyColumn1", "MyColumn2" FROM "MyTable2"'',
    1,
    1,
    1
);';
-- QUERY END: InsertDNDBTScriptExecutionRecordQuery

-- QUERY START: DropTriggerQuery
EXECUTE 'DROP TRIGGER "TR_MyTable2_MyTrigger1" ON "MyTable2";';
-- QUERY END: DropTriggerQuery

-- QUERY START: DeleteDNDBTDbObjectRecordQuery
EXECUTE 'DELETE FROM "DNDBTDbObjects"
WHERE "ID" = ''ee64ffc3-5536-4624-beaf-bc3a61d06a1a'';';
-- QUERY END: DeleteDNDBTDbObjectRecordQuery

-- QUERY START: DropForeignKeyQuery
EXECUTE 'ALTER TABLE "MyTable6"
    DROP CONSTRAINT "FK_MyTable6_MyTable5_CustomName";';
-- QUERY END: DropForeignKeyQuery

-- QUERY START: DeleteDNDBTDbObjectRecordQuery
EXECUTE 'DELETE FROM "DNDBTDbObjects"
WHERE "ID" = ''ae453b22-d270-41fc-8184-9ac26b7a0569'';';
-- QUERY END: DeleteDNDBTDbObjectRecordQuery

-- QUERY START: DropForeignKeyQuery
EXECUTE 'ALTER TABLE "MyTable1"
    DROP CONSTRAINT "FK_MyTable1_MyColumn1_MyTable2_MyColumn1";';
-- QUERY END: DropForeignKeyQuery

-- QUERY START: DeleteDNDBTDbObjectRecordQuery
EXECUTE 'DELETE FROM "DNDBTDbObjects"
WHERE "ID" = ''d11b2a53-32db-432f-bb6b-f91788844ba9'';';
-- QUERY END: DeleteDNDBTDbObjectRecordQuery

-- QUERY START: DropIndexQuery
EXECUTE 'DROP INDEX "IDX_MyTable2_MyIndex1";';
-- QUERY END: DropIndexQuery

-- QUERY START: DeleteDNDBTDbObjectRecordQuery
EXECUTE 'DELETE FROM "DNDBTDbObjects"
WHERE "ID" = ''74390b3c-bc39-4860-a42e-12baa400f927'';';
-- QUERY END: DeleteDNDBTDbObjectRecordQuery

-- QUERY START: DropIndexQuery
EXECUTE 'DROP INDEX "IDX_MyTable5_CustomName";';
-- QUERY END: DropIndexQuery

-- QUERY START: DeleteDNDBTDbObjectRecordQuery
EXECUTE 'DELETE FROM "DNDBTDbObjects"
WHERE "ID" = ''1d632285-9914-4c5d-98e6-a618a99bd799'';';
-- QUERY END: DeleteDNDBTDbObjectRecordQuery

-- QUERY START: AlterTableQuery
EXECUTE 'ALTER TABLE "MyTable1"
    DROP CONSTRAINT "CK_MyTable1_MyCheck1";';
-- QUERY END: AlterTableQuery

-- QUERY START: DeleteDNDBTDbObjectRecordQuery
EXECUTE 'DELETE FROM "DNDBTDbObjects"
WHERE "ID" = ''eb9c59b5-bc7e-49d7-adaa-f5600b6a19a2'';';
-- QUERY END: DeleteDNDBTDbObjectRecordQuery

-- QUERY START: AlterTableQuery
EXECUTE 'ALTER TABLE "MyTable1"
    ALTER COLUMN "MyColumn4" DROP DEFAULT;';
-- QUERY END: AlterTableQuery

-- QUERY START: UpdateDNDBTDbObjectRecordQuery
EXECUTE 'UPDATE "DNDBTDbObjects" SET
    "Name" = ''MyColumn4'',
    "Code" = NULL
WHERE "ID" = ''867ac528-e87e-4c93-b6e3-dd2fcbbb837f'';';
-- QUERY END: UpdateDNDBTDbObjectRecordQuery

-- QUERY START: AlterTableQuery
EXECUTE 'ALTER TABLE "MyTable2"
    ALTER COLUMN "MyColumn2" DROP DEFAULT;';
-- QUERY END: AlterTableQuery

-- QUERY START: UpdateDNDBTDbObjectRecordQuery
EXECUTE 'UPDATE "DNDBTDbObjects" SET
    "Name" = ''MyColumn2'',
    "Code" = NULL
WHERE "ID" = ''5a0d1926-3270-4eb2-92eb-00be56c7af23'';';
-- QUERY END: UpdateDNDBTDbObjectRecordQuery

-- QUERY START: AlterTableQuery
EXECUTE 'ALTER TABLE "MyTable5"
    ALTER COLUMN "MyColumn1" DROP DEFAULT,
    ALTER COLUMN "MyColumn201" DROP DEFAULT;';
-- QUERY END: AlterTableQuery

-- QUERY START: UpdateDNDBTDbObjectRecordQuery
EXECUTE 'UPDATE "DNDBTDbObjects" SET
    "Name" = ''MyColumn1'',
    "Code" = NULL
WHERE "ID" = ''5309d66f-2030-402e-912e-5547babaa072'';';
-- QUERY END: UpdateDNDBTDbObjectRecordQuery

-- QUERY START: UpdateDNDBTDbObjectRecordQuery
EXECUTE 'UPDATE "DNDBTDbObjects" SET
    "Name" = ''MyColumn201'',
    "Code" = NULL
WHERE "ID" = ''5c455ec9-9830-4d0b-a88c-57341899dc4a'';';
-- QUERY END: UpdateDNDBTDbObjectRecordQuery

-- QUERY START: DropProcedureQuery
EXECUTE 'DROP PROCEDURE "MyProcedure1";';
-- QUERY END: DropProcedureQuery

-- QUERY START: DeleteDNDBTDbObjectRecordQuery
EXECUTE 'DELETE FROM "DNDBTDbObjects"
WHERE "ID" = ''c4bf4926-bd3b-4c95-bc3e-1249445aec14'';';
-- QUERY END: DeleteDNDBTDbObjectRecordQuery

-- QUERY START: DropViewQuery
EXECUTE 'DROP VIEW "MyView1";';
-- QUERY END: DropViewQuery

-- QUERY START: DeleteDNDBTDbObjectRecordQuery
EXECUTE 'DELETE FROM "DNDBTDbObjects"
WHERE "ID" = ''e2569aae-d5da-4a77-b3cd-51adbdb272d9'';';
-- QUERY END: DeleteDNDBTDbObjectRecordQuery

-- QUERY START: AlterSequenceQuery
EXECUTE 'ALTER SEQUENCE "MySequence2" RENAME TO "_DNDBTTemp_MySequence2";
ALTER SEQUENCE "_DNDBTTemp_MySequence2"
    OWNED BY NONE';
-- QUERY END: AlterSequenceQuery

-- QUERY START: DeleteDNDBTDbObjectRecordQuery
EXECUTE 'DELETE FROM "DNDBTDbObjects"
WHERE "ID" = ''59c3bf9d-4938-40df-9528-f1aa8367c6e3'';';
-- QUERY END: DeleteDNDBTDbObjectRecordQuery

-- QUERY START: RenameProgrammableObjectToTempQuery
EXECUTE 'ALTER FUNCTION "MyFunction1" RENAME TO "_DNDBTTemp_MyFunction1";';
-- QUERY END: RenameProgrammableObjectToTempQuery

-- QUERY START: DeleteDNDBTDbObjectRecordQuery
EXECUTE 'DELETE FROM "DNDBTDbObjects"
WHERE "ID" = ''63d3a414-2893-4462-b3f8-04633101263a'';';
-- QUERY END: DeleteDNDBTDbObjectRecordQuery

-- QUERY START: RenameProgrammableObjectToTempQuery
EXECUTE 'ALTER FUNCTION "TR_MyTable2_MyTrigger1_Handler" RENAME TO "_DNDBTTemp_TR_MyTable2_MyTrigger1_Handler";';
-- QUERY END: RenameProgrammableObjectToTempQuery

-- QUERY START: DeleteDNDBTDbObjectRecordQuery
EXECUTE 'DELETE FROM "DNDBTDbObjects"
WHERE "ID" = ''8edd4469-e048-48bd-956e-a26113355f80'';';
-- QUERY END: DeleteDNDBTDbObjectRecordQuery

-- QUERY START: RenameTypeToTempQuery
EXECUTE 'ALTER DOMAIN "MyDomain1" RENAME TO "_DNDBTTemp_MyDomain1";';
-- QUERY END: RenameTypeToTempQuery

-- QUERY START: DeleteDNDBTDbObjectRecordQuery
EXECUTE 'DELETE FROM "DNDBTDbObjects"
WHERE "ID" = ''a28bcb6c-3cbc-467e-a52c-ac740c98a537'';';
-- QUERY END: DeleteDNDBTDbObjectRecordQuery

-- QUERY START: DeleteDNDBTDbObjectRecordQuery
EXECUTE 'DELETE FROM "DNDBTDbObjects"
WHERE "ID" = ''7a053cee-abcc-4993-8eea-12b87c5194e6'';';
-- QUERY END: DeleteDNDBTDbObjectRecordQuery

-- QUERY START: DeleteDNDBTDbObjectRecordQuery
EXECUTE 'DELETE FROM "DNDBTDbObjects"
WHERE "ID" = ''7905b7a8-cf45-4328-8a2b-00616d98235e'';';
-- QUERY END: DeleteDNDBTDbObjectRecordQuery

-- QUERY START: RenameTypeToTempQuery
EXECUTE 'ALTER DOMAIN "MyDomain2" RENAME TO "_DNDBTTemp_MyDomain2";';
-- QUERY END: RenameTypeToTempQuery

-- QUERY START: DeleteDNDBTDbObjectRecordQuery
EXECUTE 'DELETE FROM "DNDBTDbObjects"
WHERE "ID" = ''2200d040-a892-43b5-9b5e-db9f6458187f'';';
-- QUERY END: DeleteDNDBTDbObjectRecordQuery

-- QUERY START: RenameTypeToTempQuery
EXECUTE 'ALTER TYPE "MyEnumType1" RENAME TO "_DNDBTTemp_MyEnumType1";';
-- QUERY END: RenameTypeToTempQuery

-- QUERY START: DeleteDNDBTDbObjectRecordQuery
EXECUTE 'DELETE FROM "DNDBTDbObjects"
WHERE "ID" = ''9286cc1d-f0a5-4046-adc0-b9ae298c6f91'';';
-- QUERY END: DeleteDNDBTDbObjectRecordQuery

-- QUERY START: RenameTypeToTempQuery
EXECUTE 'DO $DNDBTPlPgSqlBlock$
BEGIN
ALTER TYPE "MyRangeType1" RENAME TO "_DNDBTTemp_MyRangeType1";
ALTER FUNCTION "MyRangeType1"(TIMESTAMP,TIMESTAMP) RENAME TO "_DNDBTTemp_MyRangeType1";
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

-- QUERY START: DeleteDNDBTDbObjectRecordQuery
EXECUTE 'DELETE FROM "DNDBTDbObjects"
WHERE "ID" = ''b02db666-fbbc-4cd7-a14d-4049251b9a7b'';';
-- QUERY END: DeleteDNDBTDbObjectRecordQuery

-- QUERY START: RenameTypeToTempQuery
EXECUTE 'ALTER TYPE "MyCompositeType1" RENAME TO "_DNDBTTemp_MyCompositeType1";';
-- QUERY END: RenameTypeToTempQuery

-- QUERY START: DeleteDNDBTDbObjectRecordQuery
EXECUTE 'DELETE FROM "DNDBTDbObjects"
WHERE "ID" = ''29bf2520-1d74-49ab-a602-14bd692371f2'';';
-- QUERY END: DeleteDNDBTDbObjectRecordQuery

-- QUERY START: AlterSequenceQuery
EXECUTE 'ALTER SEQUENCE "MySequence1" RENAME TO "MySequence1NewName";
ALTER SEQUENCE "MySequence1NewName"
    AS SMALLINT
    MINVALUE -2000 MAXVALUE 1500 NO CYCLE';
-- QUERY END: AlterSequenceQuery

-- QUERY START: UpdateDNDBTDbObjectRecordQuery
EXECUTE 'UPDATE "DNDBTDbObjects" SET
    "Name" = ''MySequence1NewName''
WHERE "ID" = ''f54a1a93-8cd2-4125-aede-b38cc7f8a750'';';
-- QUERY END: UpdateDNDBTDbObjectRecordQuery

-- QUERY START: CreateFunctionQuery
EXECUTE 'CREATE FUNCTION "MyFunction1"(a INT, b INT)
RETURNS INT
LANGUAGE SQL
IMMUTABLE
AS
$FuncBody$
SELECT a + b;
$FuncBody$';
-- QUERY END: CreateFunctionQuery

-- QUERY START: InsertDNDBTDbObjectRecordQuery
EXECUTE 'INSERT INTO "DNDBTDbObjects"
(
    "ID",
    "ParentID",
    "Type",
    "Name",
    "Code"
)
VALUES
(
    ''63d3a414-2893-4462-b3f8-04633101263a'',
    NULL,
    ''Function'',
    ''MyFunction1'',
    ''CREATE FUNCTION "MyFunction1"(a INT, b INT)
RETURNS INT
LANGUAGE SQL
IMMUTABLE
AS
$FuncBody$
SELECT a + b;
$FuncBody$''
);';
-- QUERY END: InsertDNDBTDbObjectRecordQuery

-- QUERY START: CreateFunctionQuery
EXECUTE 'CREATE FUNCTION "TR_MyTable2_MyTrigger1_Handler"()
RETURNS TRIGGER
LANGUAGE PLPGSQL
AS
$FuncBody$
BEGIN
    INSERT INTO "MyTable4"("MyColumn1")
    VALUES(NEW."MyColumn1NewName");
    RETURN NULL;
END;
$FuncBody$';
-- QUERY END: CreateFunctionQuery

-- QUERY START: InsertDNDBTDbObjectRecordQuery
EXECUTE 'INSERT INTO "DNDBTDbObjects"
(
    "ID",
    "ParentID",
    "Type",
    "Name",
    "Code"
)
VALUES
(
    ''8edd4469-e048-48bd-956e-a26113355f80'',
    NULL,
    ''Function'',
    ''TR_MyTable2_MyTrigger1_Handler'',
    ''CREATE FUNCTION "TR_MyTable2_MyTrigger1_Handler"()
RETURNS TRIGGER
LANGUAGE PLPGSQL
AS
$FuncBody$
BEGIN
    INSERT INTO "MyTable4"("MyColumn1")
    VALUES(NEW."MyColumn1NewName");
    RETURN NULL;
END;
$FuncBody$''
);';
-- QUERY END: InsertDNDBTDbObjectRecordQuery

-- QUERY START: CreateCompositeTypeQuery
EXECUTE 'CREATE TYPE "MyCompositeType1" AS
(
    "MyAttribute1" VARCHAR(110),
    "MyAttribute2" DECIMAL(7,2)[]
);';
-- QUERY END: CreateCompositeTypeQuery

-- QUERY START: InsertDNDBTDbObjectRecordQuery
EXECUTE 'INSERT INTO "DNDBTDbObjects"
(
    "ID",
    "ParentID",
    "Type",
    "Name",
    "Code"
)
VALUES
(
    ''29bf2520-1d74-49ab-a602-14bd692371f2'',
    NULL,
    ''UserDefinedType'',
    ''MyCompositeType1'',
    NULL
);';
-- QUERY END: InsertDNDBTDbObjectRecordQuery

-- QUERY START: CreateDomainTypeQuery
EXECUTE 'CREATE DOMAIN "MyDomain1" AS
    VARCHAR(111) NULL
    CONSTRAINT "MyDomain1_CK1" CHECK (value = lower(value))
    CONSTRAINT "MyDomain1_CK2" CHECK (char_length(value) > 3);';
-- QUERY END: CreateDomainTypeQuery

-- QUERY START: InsertDNDBTDbObjectRecordQuery
EXECUTE 'INSERT INTO "DNDBTDbObjects"
(
    "ID",
    "ParentID",
    "Type",
    "Name",
    "Code"
)
VALUES
(
    ''a28bcb6c-3cbc-467e-a52c-ac740c98a537'',
    NULL,
    ''UserDefinedType'',
    ''MyDomain1'',
    NULL
);';
-- QUERY END: InsertDNDBTDbObjectRecordQuery

-- QUERY START: InsertDNDBTDbObjectRecordQuery
EXECUTE 'INSERT INTO "DNDBTDbObjects"
(
    "ID",
    "ParentID",
    "Type",
    "Name",
    "Code"
)
VALUES
(
    ''7a053cee-abcc-4993-8eea-12b87c5194e6'',
    ''a28bcb6c-3cbc-467e-a52c-ac740c98a537'',
    ''CheckConstraint'',
    ''MyDomain1_CK1'',
    ''value = lower(value)''
);';
-- QUERY END: InsertDNDBTDbObjectRecordQuery

-- QUERY START: InsertDNDBTDbObjectRecordQuery
EXECUTE 'INSERT INTO "DNDBTDbObjects"
(
    "ID",
    "ParentID",
    "Type",
    "Name",
    "Code"
)
VALUES
(
    ''7905b7a8-cf45-4328-8a2b-00616d98235e'',
    ''a28bcb6c-3cbc-467e-a52c-ac740c98a537'',
    ''CheckConstraint'',
    ''MyDomain1_CK2'',
    ''char_length(value) > 3''
);';
-- QUERY END: InsertDNDBTDbObjectRecordQuery

-- QUERY START: CreateDomainTypeQuery
EXECUTE 'CREATE DOMAIN "MyDomain2" AS
    "MyCompositeType1" NOT NULL DEFAULT ''("some string", "{42.78, -4, 0}")'';';
-- QUERY END: CreateDomainTypeQuery

-- QUERY START: InsertDNDBTDbObjectRecordQuery
EXECUTE 'INSERT INTO "DNDBTDbObjects"
(
    "ID",
    "ParentID",
    "Type",
    "Name",
    "Code"
)
VALUES
(
    ''2200d040-a892-43b5-9b5e-db9f6458187f'',
    NULL,
    ''UserDefinedType'',
    ''MyDomain2'',
    ''''''("some string", "{42.78, -4, 0}")''''''
);';
-- QUERY END: InsertDNDBTDbObjectRecordQuery

-- QUERY START: CreateEnumTypeQuery
EXECUTE 'CREATE TYPE "MyEnumType1" AS ENUM
(
    ''Label1'',
    ''Label2'',
    ''Label3''
);';
-- QUERY END: CreateEnumTypeQuery

-- QUERY START: InsertDNDBTDbObjectRecordQuery
EXECUTE 'INSERT INTO "DNDBTDbObjects"
(
    "ID",
    "ParentID",
    "Type",
    "Name",
    "Code"
)
VALUES
(
    ''9286cc1d-f0a5-4046-adc0-b9ae298c6f91'',
    NULL,
    ''UserDefinedType'',
    ''MyEnumType1'',
    NULL
);';
-- QUERY END: InsertDNDBTDbObjectRecordQuery

-- QUERY START: CreateRangeTypeQuery
EXECUTE 'DO $DNDBTPlPgSqlBlock$
BEGIN
IF (SELECT current_setting(''server_version_num'')::int) >= 140000 THEN
CREATE TYPE "MyRangeType1" AS RANGE
(
    SUBTYPE = TIMESTAMPTZ,
    SUBTYPE_OPCLASS = "timestamptz_ops",
    MULTIRANGE_TYPE_NAME = "MyRangeType1_multirange"
);
ELSE
CREATE TYPE "MyRangeType1" AS RANGE
(
    SUBTYPE = TIMESTAMPTZ,
    SUBTYPE_OPCLASS = "timestamptz_ops"
);
END IF;
END;
$DNDBTPlPgSqlBlock$';
-- QUERY END: CreateRangeTypeQuery

-- QUERY START: InsertDNDBTDbObjectRecordQuery
EXECUTE 'INSERT INTO "DNDBTDbObjects"
(
    "ID",
    "ParentID",
    "Type",
    "Name",
    "Code"
)
VALUES
(
    ''b02db666-fbbc-4cd7-a14d-4049251b9a7b'',
    NULL,
    ''UserDefinedType'',
    ''MyRangeType1'',
    NULL
);';
-- QUERY END: InsertDNDBTDbObjectRecordQuery

-- QUERY START: DropTableQuery
EXECUTE 'DROP TABLE "MyTable6";';
-- QUERY END: DropTableQuery

-- QUERY START: DeleteDNDBTDbObjectRecordQuery
EXECUTE 'DELETE FROM "DNDBTDbObjects"
WHERE "ID" = ''bfa08c82-5c8f-4ab4-bd41-1f1d85cf3c85'';';
-- QUERY END: DeleteDNDBTDbObjectRecordQuery

-- QUERY START: DeleteDNDBTDbObjectRecordQuery
EXECUTE 'DELETE FROM "DNDBTDbObjects"
WHERE "ID" = ''a402e2b7-c826-4cfd-a304-97c9bc346ba2'';';
-- QUERY END: DeleteDNDBTDbObjectRecordQuery

-- QUERY START: DeleteDNDBTDbObjectRecordQuery
EXECUTE 'DELETE FROM "DNDBTDbObjects"
WHERE "ID" = ''f3064a8c-346a-4b3d-af2c-d967b39841e4'';';
-- QUERY END: DeleteDNDBTDbObjectRecordQuery

-- QUERY START: AlterTableQuery
EXECUTE 'ALTER TABLE "MyTable1" RENAME TO "MyTable1NewName";
ALTER TABLE "MyTable1NewName"
    DROP CONSTRAINT "UQ_MyTable1_MyColumn4",
    DROP CONSTRAINT "PK_MyTable1",
    DROP COLUMN "MyColumn2",
    DROP COLUMN "MyColumn3",
    ALTER COLUMN "MyColumn1" SET DATA TYPE BIGINT
        USING ("MyColumn1"::text::BIGINT),
    ALTER COLUMN "MyColumn1" DROP NOT NULL;';
-- QUERY END: AlterTableQuery

-- QUERY START: DeleteDNDBTDbObjectRecordQuery
EXECUTE 'DELETE FROM "DNDBTDbObjects"
WHERE "ID" = ''f3f08522-26ee-4950-9135-22edf2e4e0cf'';';
-- QUERY END: DeleteDNDBTDbObjectRecordQuery

-- QUERY START: DeleteDNDBTDbObjectRecordQuery
EXECUTE 'DELETE FROM "DNDBTDbObjects"
WHERE "ID" = ''37a45def-f4a0-4be7-8bfb-8fbed4a7d705'';';
-- QUERY END: DeleteDNDBTDbObjectRecordQuery

-- QUERY START: DeleteDNDBTDbObjectRecordQuery
EXECUTE 'DELETE FROM "DNDBTDbObjects"
WHERE "ID" = ''fe68ee3d-09d0-40ac-93f9-5e441fbb4f70'';';
-- QUERY END: DeleteDNDBTDbObjectRecordQuery

-- QUERY START: DeleteDNDBTDbObjectRecordQuery
EXECUTE 'DELETE FROM "DNDBTDbObjects"
WHERE "ID" = ''6e95de30-e01a-4fb4-b8b7-8f0c40bb682c'';';
-- QUERY END: DeleteDNDBTDbObjectRecordQuery

-- QUERY START: UpdateDNDBTDbObjectRecordQuery
EXECUTE 'UPDATE "DNDBTDbObjects" SET
    "Name" = ''MyTable1NewName''
WHERE "ID" = ''299675e6-4faa-4d0f-a36a-224306ba5bcb'';';
-- QUERY END: UpdateDNDBTDbObjectRecordQuery

-- QUERY START: AlterTableQuery
EXECUTE 'ALTER TABLE "MyTable2" RENAME COLUMN "MyColumn1" TO "MyColumn1NewName";
ALTER TABLE "MyTable2"
    DROP CONSTRAINT "PK_MyTable2_CustomName",
    DROP COLUMN "MyColumn2",
    ALTER COLUMN "MyColumn1NewName" SET DATA TYPE BIGINT
        USING ("MyColumn1NewName"::text::BIGINT),
    ADD COLUMN "MyColumn2" BYTEA NULL DEFAULT ''\x000408'',
    ADD COLUMN "MyColumn3" BIGINT NULL,
    ADD COLUMN "MyColumn4" VARCHAR(50) NULL,
    ADD CONSTRAINT "PK_MyTable2_CustomName" PRIMARY KEY ("MyColumn1NewName");';
-- QUERY END: AlterTableQuery

-- QUERY START: DeleteDNDBTDbObjectRecordQuery
EXECUTE 'DELETE FROM "DNDBTDbObjects"
WHERE "ID" = ''3a43615b-40b3-4a13-99e7-93af7c56e8ce'';';
-- QUERY END: DeleteDNDBTDbObjectRecordQuery

-- QUERY START: DeleteDNDBTDbObjectRecordQuery
EXECUTE 'DELETE FROM "DNDBTDbObjects"
WHERE "ID" = ''5a0d1926-3270-4eb2-92eb-00be56c7af23'';';
-- QUERY END: DeleteDNDBTDbObjectRecordQuery

-- QUERY START: UpdateDNDBTDbObjectRecordQuery
EXECUTE 'UPDATE "DNDBTDbObjects" SET
    "Name" = ''MyColumn1NewName''
WHERE "ID" = ''c480f22f-7c01-4f41-b282-35e9f5cd1fe3'';';
-- QUERY END: UpdateDNDBTDbObjectRecordQuery

-- QUERY START: InsertDNDBTDbObjectRecordQuery
EXECUTE 'INSERT INTO "DNDBTDbObjects"
(
    "ID",
    "ParentID",
    "Type",
    "Name",
    "Code"
)
VALUES
(
    ''c2df19c2-e029-4014-8a5b-4ab42fecb6b8'',
    ''bfb9030c-a8c3-4882-9c42-1c6ad025cf8f'',
    ''Column'',
    ''MyColumn2'',
    ''''''\x000408''''''
);';
-- QUERY END: InsertDNDBTDbObjectRecordQuery

-- QUERY START: InsertDNDBTDbObjectRecordQuery
EXECUTE 'INSERT INTO "DNDBTDbObjects"
(
    "ID",
    "ParentID",
    "Type",
    "Name",
    "Code"
)
VALUES
(
    ''99bc3f49-3151-4f52-87f7-104b424ed7bf'',
    ''bfb9030c-a8c3-4882-9c42-1c6ad025cf8f'',
    ''Column'',
    ''MyColumn3'',
    NULL
);';
-- QUERY END: InsertDNDBTDbObjectRecordQuery

-- QUERY START: InsertDNDBTDbObjectRecordQuery
EXECUTE 'INSERT INTO "DNDBTDbObjects"
(
    "ID",
    "ParentID",
    "Type",
    "Name",
    "Code"
)
VALUES
(
    ''87950a3f-2072-42db-ac3c-a4e85b79720d'',
    ''bfb9030c-a8c3-4882-9c42-1c6ad025cf8f'',
    ''Column'',
    ''MyColumn4'',
    NULL
);';
-- QUERY END: InsertDNDBTDbObjectRecordQuery

-- QUERY START: InsertDNDBTDbObjectRecordQuery
EXECUTE 'INSERT INTO "DNDBTDbObjects"
(
    "ID",
    "ParentID",
    "Type",
    "Name",
    "Code"
)
VALUES
(
    ''3a43615b-40b3-4a13-99e7-93af7c56e8ce'',
    ''bfb9030c-a8c3-4882-9c42-1c6ad025cf8f'',
    ''PrimaryKey'',
    ''PK_MyTable2_CustomName'',
    NULL
);';
-- QUERY END: InsertDNDBTDbObjectRecordQuery

-- QUERY START: AlterTableQuery
EXECUTE 'ALTER TABLE "MyTable5"
    DROP CONSTRAINT "UQ_MyTable5_CustomName",
    DROP CONSTRAINT "PK_MyTable5_CustomName",
    ALTER COLUMN "MyColumn203" SET GENERATED BY DEFAULT,
    ALTER COLUMN "MyColumn203" SET START 1,
    ALTER COLUMN "MyColumn203" SET INCREMENT -4,
    ALTER COLUMN "MyColumn203" SET MAXVALUE 9999,
    ALTER COLUMN "MyColumn203" SET CYCLE,
    ALTER COLUMN "MyColumn101" SET DATA TYPE "MyCompositeType1"
        USING ("MyColumn101"::text::"MyCompositeType1"),
    ALTER COLUMN "MyColumn102" SET DATA TYPE "MyDomain1"
        USING ("MyColumn102"::text::"MyDomain1"),
    ALTER COLUMN "MyColumn103" SET DATA TYPE "MyEnumType1"
        USING ("MyColumn103"::text::"MyEnumType1"),
    ALTER COLUMN "MyColumn104" SET DATA TYPE "MyRangeType1"
        USING ("MyColumn104"::text::"MyRangeType1"),
    ALTER COLUMN "MyColumn202" SET DATA TYPE "MyDomain2"
        USING ("MyColumn202"::text::"MyDomain2"),
    ALTER COLUMN "MyColumn339" SET DATA TYPE "MyCompositeType1"[]
        USING ("MyColumn339"::text::"MyCompositeType1"[]),
    ALTER COLUMN "MyColumn340" SET DATA TYPE "MyCompositeType1"[]
        USING ("MyColumn340"::text::"MyCompositeType1"[]);';
-- QUERY END: AlterTableQuery

-- QUERY START: DeleteDNDBTDbObjectRecordQuery
EXECUTE 'DELETE FROM "DNDBTDbObjects"
WHERE "ID" = ''5293b58a-9f63-4f0f-8d6f-18416ebbd751'';';
-- QUERY END: DeleteDNDBTDbObjectRecordQuery

-- QUERY START: DeleteDNDBTDbObjectRecordQuery
EXECUTE 'DELETE FROM "DNDBTDbObjects"
WHERE "ID" = ''79384d48-a39b-4a22-900e-066b2ca67ba2'';';
-- QUERY END: DeleteDNDBTDbObjectRecordQuery

-- QUERY START: CreateTableQuery
EXECUTE 'CREATE TABLE "MyTable3"
(
    "MyColumn1" BIGINT NOT NULL DEFAULT 444,
    "MyColumn2" VARCHAR(50) NOT NULL,
    CONSTRAINT "UQ_MyTable3_MyColumns12" UNIQUE ("MyColumn1", "MyColumn2")
);';
-- QUERY END: CreateTableQuery

-- QUERY START: InsertDNDBTDbObjectRecordQuery
EXECUTE 'INSERT INTO "DNDBTDbObjects"
(
    "ID",
    "ParentID",
    "Type",
    "Name",
    "Code"
)
VALUES
(
    ''474cd761-2522-4529-9d20-2b94115f9626'',
    NULL,
    ''Table'',
    ''MyTable3'',
    NULL
);';
-- QUERY END: InsertDNDBTDbObjectRecordQuery

-- QUERY START: InsertDNDBTDbObjectRecordQuery
EXECUTE 'INSERT INTO "DNDBTDbObjects"
(
    "ID",
    "ParentID",
    "Type",
    "Name",
    "Code"
)
VALUES
(
    ''726f503a-d944-46ee-a0ff-6a2c2faab46e'',
    ''474cd761-2522-4529-9d20-2b94115f9626'',
    ''Column'',
    ''MyColumn1'',
    ''444''
);';
-- QUERY END: InsertDNDBTDbObjectRecordQuery

-- QUERY START: InsertDNDBTDbObjectRecordQuery
EXECUTE 'INSERT INTO "DNDBTDbObjects"
(
    "ID",
    "ParentID",
    "Type",
    "Name",
    "Code"
)
VALUES
(
    ''169824e1-8b74-4b60-af17-99656d6dbbee'',
    ''474cd761-2522-4529-9d20-2b94115f9626'',
    ''Column'',
    ''MyColumn2'',
    NULL
);';
-- QUERY END: InsertDNDBTDbObjectRecordQuery

-- QUERY START: InsertDNDBTDbObjectRecordQuery
EXECUTE 'INSERT INTO "DNDBTDbObjects"
(
    "ID",
    "ParentID",
    "Type",
    "Name",
    "Code"
)
VALUES
(
    ''fd288e38-35ba-4bb1-ace3-597c99ef26c7'',
    ''474cd761-2522-4529-9d20-2b94115f9626'',
    ''UniqueConstraint'',
    ''UQ_MyTable3_MyColumns12'',
    NULL
);';
-- QUERY END: InsertDNDBTDbObjectRecordQuery

-- QUERY START: AlterSequenceQuery
EXECUTE 'ALTER SEQUENCE "MySequence1NewName"
    OWNED BY NONE';
-- QUERY END: AlterSequenceQuery

-- QUERY START: UpdateDNDBTDbObjectRecordQuery
EXECUTE 'UPDATE "DNDBTDbObjects" SET
    "Name" = ''MySequence1NewName''
WHERE "ID" = ''f54a1a93-8cd2-4125-aede-b38cc7f8a750'';';
-- QUERY END: UpdateDNDBTDbObjectRecordQuery

-- QUERY START: DropTypeQuery
EXECUTE 'DROP DOMAIN "_DNDBTTemp_MyDomain1";';
-- QUERY END: DropTypeQuery

-- QUERY START: DropTypeQuery
EXECUTE 'DROP DOMAIN "_DNDBTTemp_MyDomain2";';
-- QUERY END: DropTypeQuery

-- QUERY START: DropTypeQuery
EXECUTE 'DROP TYPE "_DNDBTTemp_MyEnumType1";';
-- QUERY END: DropTypeQuery

-- QUERY START: DropTypeQuery
EXECUTE 'DROP TYPE "_DNDBTTemp_MyRangeType1";';
-- QUERY END: DropTypeQuery

-- QUERY START: DropTypeQuery
EXECUTE 'DROP TYPE "_DNDBTTemp_MyCompositeType1";';
-- QUERY END: DropTypeQuery

-- QUERY START: DropFunctionQuery
EXECUTE 'DROP FUNCTION "_DNDBTTemp_MyFunction1";';
-- QUERY END: DropFunctionQuery

-- QUERY START: DropFunctionQuery
EXECUTE 'DROP FUNCTION "_DNDBTTemp_TR_MyTable2_MyTrigger1_Handler";';
-- QUERY END: DropFunctionQuery

-- QUERY START: DropSequenceQuery
EXECUTE 'DROP SEQUENCE "_DNDBTTemp_MySequence2";';
-- QUERY END: DropSequenceQuery

-- QUERY START: CreateProcedureQuery
EXECUTE 'CREATE PROCEDURE "MyProcedure1"(in a INT, in b INT)
LANGUAGE SQL
AS
$ProcBody$
INSERT INTO "MyTable4"("MyColumn1") VALUES (a);
INSERT INTO "MyTable4"("MyColumn1") VALUES (b);
$ProcBody$';
-- QUERY END: CreateProcedureQuery

-- QUERY START: InsertDNDBTDbObjectRecordQuery
EXECUTE 'INSERT INTO "DNDBTDbObjects"
(
    "ID",
    "ParentID",
    "Type",
    "Name",
    "Code"
)
VALUES
(
    ''c4bf4926-bd3b-4c95-bc3e-1249445aec14'',
    NULL,
    ''Procedure'',
    ''MyProcedure1'',
    ''CREATE PROCEDURE "MyProcedure1"(in a INT, in b INT)
LANGUAGE SQL
AS
$ProcBody$
INSERT INTO "MyTable4"("MyColumn1") VALUES (a);
INSERT INTO "MyTable4"("MyColumn1") VALUES (b);
$ProcBody$''
);';
-- QUERY END: InsertDNDBTDbObjectRecordQuery

-- QUERY START: CreateViewQuery
EXECUTE 'CREATE VIEW "MyView1" AS
SELECT
    t1."MyColumn1",
    t1."MyColumn4",
    t2."MyColumn2"
FROM "MyTable1NewName" t1
LEFT JOIN "MyTable2" t2
    ON t2."MyColumn1NewName" = t1."MyColumn1"';
-- QUERY END: CreateViewQuery

-- QUERY START: InsertDNDBTDbObjectRecordQuery
EXECUTE 'INSERT INTO "DNDBTDbObjects"
(
    "ID",
    "ParentID",
    "Type",
    "Name",
    "Code"
)
VALUES
(
    ''e2569aae-d5da-4a77-b3cd-51adbdb272d9'',
    NULL,
    ''View'',
    ''MyView1'',
    ''CREATE VIEW "MyView1" AS
SELECT
    t1."MyColumn1",
    t1."MyColumn4",
    t2."MyColumn2"
FROM "MyTable1NewName" t1
LEFT JOIN "MyTable2" t2
    ON t2."MyColumn1NewName" = t1."MyColumn1"''
);';
-- QUERY END: InsertDNDBTDbObjectRecordQuery

-- QUERY START: AlterTableQuery
EXECUTE 'ALTER TABLE "MyTable2"
    ALTER COLUMN "MyColumn2" SET DEFAULT ''\x000408'';';
-- QUERY END: AlterTableQuery

-- QUERY START: UpdateDNDBTDbObjectRecordQuery
EXECUTE 'UPDATE "DNDBTDbObjects" SET
    "Name" = ''MyColumn2'',
    "Code" = ''''''\x000408''''''
WHERE "ID" = ''c2df19c2-e029-4014-8a5b-4ab42fecb6b8'';';
-- QUERY END: UpdateDNDBTDbObjectRecordQuery

-- QUERY START: AlterTableQuery
EXECUTE 'ALTER TABLE "MyTable5"
    ALTER COLUMN "MyColumn1" SET DEFAULT abS(-15),
    ALTER COLUMN "MyColumn201" SET DEFAULT "MyFunction1"(-25, 10);';
-- QUERY END: AlterTableQuery

-- QUERY START: UpdateDNDBTDbObjectRecordQuery
EXECUTE 'UPDATE "DNDBTDbObjects" SET
    "Name" = ''MyColumn1'',
    "Code" = ''abS(-15)''
WHERE "ID" = ''5309d66f-2030-402e-912e-5547babaa072'';';
-- QUERY END: UpdateDNDBTDbObjectRecordQuery

-- QUERY START: UpdateDNDBTDbObjectRecordQuery
EXECUTE 'UPDATE "DNDBTDbObjects" SET
    "Name" = ''MyColumn201'',
    "Code" = ''"MyFunction1"(-25, 10)''
WHERE "ID" = ''5c455ec9-9830-4d0b-a88c-57341899dc4a'';';
-- QUERY END: UpdateDNDBTDbObjectRecordQuery

-- QUERY START: AlterTableQuery
EXECUTE 'ALTER TABLE "MyTable1NewName"
    ADD CONSTRAINT "CK_MyTable1_MyCheck1" CHECK ("MyColumn4" >= 1);';
-- QUERY END: AlterTableQuery

-- QUERY START: InsertDNDBTDbObjectRecordQuery
EXECUTE 'INSERT INTO "DNDBTDbObjects"
(
    "ID",
    "ParentID",
    "Type",
    "Name",
    "Code"
)
VALUES
(
    ''eb9c59b5-bc7e-49d7-adaa-f5600b6a19a2'',
    ''299675e6-4faa-4d0f-a36a-224306ba5bcb'',
    ''CheckConstraint'',
    ''CK_MyTable1_MyCheck1'',
    ''"MyColumn4" >= 1''
);';
-- QUERY END: InsertDNDBTDbObjectRecordQuery

-- QUERY START: CreateIndexQuery
EXECUTE 'CREATE UNIQUE INDEX "IDX_MyTable2_MyIndex1"
    ON "MyTable2" USING BTREE ("MyColumn1NewName", "MyColumn2");';
-- QUERY END: CreateIndexQuery

-- QUERY START: InsertDNDBTDbObjectRecordQuery
EXECUTE 'INSERT INTO "DNDBTDbObjects"
(
    "ID",
    "ParentID",
    "Type",
    "Name",
    "Code"
)
VALUES
(
    ''74390b3c-bc39-4860-a42e-12baa400f927'',
    ''bfb9030c-a8c3-4882-9c42-1c6ad025cf8f'',
    ''Index'',
    ''IDX_MyTable2_MyIndex1'',
    NULL
);';
-- QUERY END: InsertDNDBTDbObjectRecordQuery

-- QUERY START: CreateIndexQuery
EXECUTE 'CREATE INDEX "IDX_MyTable5_1"
    ON "MyTable5" USING BTREE ((length("MyColumn2" || "MyColumn1") + 1))
    INCLUDE ("MyColumn4", "MyColumn3", "MyColumn5");';
-- QUERY END: CreateIndexQuery

-- QUERY START: InsertDNDBTDbObjectRecordQuery
EXECUTE 'INSERT INTO "DNDBTDbObjects"
(
    "ID",
    "ParentID",
    "Type",
    "Name",
    "Code"
)
VALUES
(
    ''f7f367da-088f-48dd-bad5-2a14a0e77f66'',
    ''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'',
    ''Index'',
    ''IDX_MyTable5_1'',
    ''(length("MyColumn2" || "MyColumn1") + 1)''
);';
-- QUERY END: InsertDNDBTDbObjectRecordQuery

-- QUERY START: CreateForeignKeyQuery
EXECUTE 'ALTER TABLE "MyTable1NewName"
    ADD CONSTRAINT "FK_MyTable1_MyColumn1_MyTable2_MyColumn1" FOREIGN KEY ("MyColumn1")
        REFERENCES "MyTable2" ("MyColumn1NewName")
        ON UPDATE NO ACTION ON DELETE SET NULL;';
-- QUERY END: CreateForeignKeyQuery

-- QUERY START: InsertDNDBTDbObjectRecordQuery
EXECUTE 'INSERT INTO "DNDBTDbObjects"
(
    "ID",
    "ParentID",
    "Type",
    "Name",
    "Code"
)
VALUES
(
    ''d11b2a53-32db-432f-bb6b-f91788844ba9'',
    ''299675e6-4faa-4d0f-a36a-224306ba5bcb'',
    ''ForeignKey'',
    ''FK_MyTable1_MyColumn1_MyTable2_MyColumn1'',
    NULL
);';
-- QUERY END: InsertDNDBTDbObjectRecordQuery

-- QUERY START: CreateForeignKeyQuery
EXECUTE 'ALTER TABLE "MyTable2"
    ADD CONSTRAINT "FK_MyTable2_MyColumns34_MyTable3_MyColumns12" FOREIGN KEY ("MyColumn3", "MyColumn4")
        REFERENCES "MyTable3" ("MyColumn1", "MyColumn2")
        ON UPDATE NO ACTION ON DELETE SET DEFAULT;';
-- QUERY END: CreateForeignKeyQuery

-- QUERY START: InsertDNDBTDbObjectRecordQuery
EXECUTE 'INSERT INTO "DNDBTDbObjects"
(
    "ID",
    "ParentID",
    "Type",
    "Name",
    "Code"
)
VALUES
(
    ''480f3508-9d51-4190-88aa-45bc20e49119'',
    ''bfb9030c-a8c3-4882-9c42-1c6ad025cf8f'',
    ''ForeignKey'',
    ''FK_MyTable2_MyColumns34_MyTable3_MyColumns12'',
    NULL
);';
-- QUERY END: InsertDNDBTDbObjectRecordQuery

-- QUERY START: CreateTriggerQuery
EXECUTE 'CREATE TRIGGER "TR_MyTable2_MyTrigger1"
AFTER INSERT
ON "MyTable2"
FOR EACH ROW
EXECUTE FUNCTION "TR_MyTable2_MyTrigger1_Handler"()';
-- QUERY END: CreateTriggerQuery

-- QUERY START: InsertDNDBTDbObjectRecordQuery
EXECUTE 'INSERT INTO "DNDBTDbObjects"
(
    "ID",
    "ParentID",
    "Type",
    "Name",
    "Code"
)
VALUES
(
    ''ee64ffc3-5536-4624-beaf-bc3a61d06a1a'',
    ''bfb9030c-a8c3-4882-9c42-1c6ad025cf8f'',
    ''Trigger'',
    ''TR_MyTable2_MyTrigger1'',
    ''CREATE TRIGGER "TR_MyTable2_MyTrigger1"
AFTER INSERT
ON "MyTable2"
FOR EACH ROW
EXECUTE FUNCTION "TR_MyTable2_MyTrigger1_Handler"()''
);';
-- QUERY END: InsertDNDBTDbObjectRecordQuery

-- QUERY START: GenericQuery
EXECUTE 'DO $Block$
BEGIN

IF EXISTS (SELECT TRUE FROM "information_schema"."tables" WHERE "table_name" = ''_MyTable2'')
THEN
    UPDATE "MyTable2" SET
        "MyColumn2" = "t"."MyColumn2"
    FROM "_MyTable2" AS "t"
    WHERE "MyTable2"."MyColumn1NewName" = "t"."MyColumn1";

    DROP TABLE "_MyTable2";
END IF;

END;
$Block$';
-- QUERY END: GenericQuery

-- QUERY START: InsertDNDBTScriptExecutionRecordQuery
EXECUTE 'INSERT INTO "DNDBTScriptExecutions"
(
    "ID",
    "Type",
    "Name",
    "Text",
    "MinDbVersionToExecute",
    "MaxDbVersionToExecute",
    "ExecutedOnDbVersion"
)
VALUES
(
    ''8ccaf36e-e587-466e-86f7-45c0061ae521'',
    ''AfterPublishOnce'',
    ''RestoreRecreatedColumnsData'',
    ''DO $Block$
BEGIN

IF EXISTS (SELECT TRUE FROM "information_schema"."tables" WHERE "table_name" = ''''_MyTable2'''')
THEN
    UPDATE "MyTable2" SET
        "MyColumn2" = "t"."MyColumn2"
    FROM "_MyTable2" AS "t"
    WHERE "MyTable2"."MyColumn1NewName" = "t"."MyColumn1";

    DROP TABLE "_MyTable2";
END IF;

END;
$Block$'',
    1,
    1,
    1
);';
-- QUERY END: InsertDNDBTScriptExecutionRecordQuery

-- QUERY START: UpdateDNDBTDbAttributesRecordQuery
EXECUTE 'UPDATE "DNDBTDbAttributes" SET
    "Version" = 2;';
-- QUERY END: UpdateDNDBTDbAttributesRecordQuery

END;
$DNDBTGeneratedScriptTransactionBlock$