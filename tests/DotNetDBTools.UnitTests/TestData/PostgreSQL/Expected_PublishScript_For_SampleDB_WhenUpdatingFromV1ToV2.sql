DO $$
BEGIN

-- QUERY START: PostgreSQLDropForeignKeyQuery
EXECUTE 'ALTER TABLE "MyTable1" DROP CONSTRAINT "FK_MyTable1_MyColumn1_MyTable2_MyColumn1";';
-- QUERY END: PostgreSQLDropForeignKeyQuery

-- QUERY START: PostgreSQLDeleteDNDBTSysInfoQuery
EXECUTE 'DELETE FROM "DNDBTDbObjects"
WHERE "ID" = ''d11b2a53-32db-432f-bb6b-f91788844ba9'';';
-- QUERY END: PostgreSQLDeleteDNDBTSysInfoQuery

-- QUERY START: PostgreSQLRenameTypeToTempQuery
EXECUTE 'ALTER TYPE "MyCompositeType1" RENAME TO "_DNDBTTemp_MyCompositeType1";';
-- QUERY END: PostgreSQLRenameTypeToTempQuery

-- QUERY START: PostgreSQLDeleteDNDBTSysInfoQuery
EXECUTE 'DELETE FROM "DNDBTDbObjects"
WHERE "ID" = ''29bf2520-1d74-49ab-a602-14bd692371f2'';';
-- QUERY END: PostgreSQLDeleteDNDBTSysInfoQuery

-- QUERY START: PostgreSQLRenameTypeToTempQuery
EXECUTE 'ALTER DOMAIN "MyDomain1" RENAME TO "_DNDBTTemp_MyDomain1";';
-- QUERY END: PostgreSQLRenameTypeToTempQuery

-- QUERY START: PostgreSQLDeleteDNDBTSysInfoQuery
EXECUTE 'DELETE FROM "DNDBTDbObjects"
WHERE "ID" = ''a28bcb6c-3cbc-467e-a52c-ac740c98a537'';';
-- QUERY END: PostgreSQLDeleteDNDBTSysInfoQuery

-- QUERY START: PostgreSQLDeleteDNDBTSysInfoQuery
EXECUTE 'DELETE FROM "DNDBTDbObjects"
WHERE "ID" = ''7a053cee-abcc-4993-8eea-12b87c5194e6'';';
-- QUERY END: PostgreSQLDeleteDNDBTSysInfoQuery

-- QUERY START: PostgreSQLDeleteDNDBTSysInfoQuery
EXECUTE 'DELETE FROM "DNDBTDbObjects"
WHERE "ID" = ''7905b7a8-cf45-4328-8a2b-00616d98235e'';';
-- QUERY END: PostgreSQLDeleteDNDBTSysInfoQuery

-- QUERY START: PostgreSQLRenameTypeToTempQuery
EXECUTE 'ALTER TYPE "MyEnumType1" RENAME TO "_DNDBTTemp_MyEnumType1";';
-- QUERY END: PostgreSQLRenameTypeToTempQuery

-- QUERY START: PostgreSQLDeleteDNDBTSysInfoQuery
EXECUTE 'DELETE FROM "DNDBTDbObjects"
WHERE "ID" = ''9286cc1d-f0a5-4046-adc0-b9ae298c6f91'';';
-- QUERY END: PostgreSQLDeleteDNDBTSysInfoQuery

-- QUERY START: PostgreSQLRenameTypeToTempQuery
EXECUTE 'ALTER TYPE "MyRangeType1" RENAME TO "_DNDBTTemp_MyRangeType1";
ALTER FUNCTION "MyRangeType1"(TIMESTAMP,TIMESTAMP) RENAME TO "_DNDBTTemp_MyRangeType1";
ALTER FUNCTION "MyRangeType1" RENAME TO "_DNDBTTemp_MyRangeType1";
ALTER TYPE "MyRangeType1_multirange" RENAME TO "_DNDBTTemp_MyRangeType1_multirange";
ALTER FUNCTION "MyRangeType1_multirange"() RENAME TO "_DNDBTTemp_MyRangeType1_multirange";
ALTER FUNCTION "MyRangeType1_multirange"("_DNDBTTemp_MyRangeType1") RENAME TO "_DNDBTTemp_MyRangeType1_multirange";
ALTER FUNCTION "MyRangeType1_multirange" RENAME TO "_DNDBTTemp_MyRangeType1_multirange";';
-- QUERY END: PostgreSQLRenameTypeToTempQuery

-- QUERY START: PostgreSQLDeleteDNDBTSysInfoQuery
EXECUTE 'DELETE FROM "DNDBTDbObjects"
WHERE "ID" = ''b02db666-fbbc-4cd7-a14d-4049251b9a7b'';';
-- QUERY END: PostgreSQLDeleteDNDBTSysInfoQuery

-- QUERY START: PostgreSQLCreateCompositeTypeQuery
EXECUTE 'CREATE TYPE "MyCompositeType1" AS
(
    "MyAttribute1" VARCHAR(110),
    "MyAttribute2" INT
);';
-- QUERY END: PostgreSQLCreateCompositeTypeQuery

-- QUERY START: PostgreSQLInsertDNDBTSysInfoQuery
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
-- QUERY END: PostgreSQLInsertDNDBTSysInfoQuery

-- QUERY START: PostgreSQLCreateDomainTypeQuery
EXECUTE 'CREATE DOMAIN "MyDomain1" AS VARCHAR(111)    NULL
    CONSTRAINT "MyDomain1_CK1" CHECK (value = lower(value))
    CONSTRAINT "MyDomain1_CK2" CHECK (char_length(value) > 3);';
-- QUERY END: PostgreSQLCreateDomainTypeQuery

-- QUERY START: PostgreSQLInsertDNDBTSysInfoQuery
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
-- QUERY END: PostgreSQLInsertDNDBTSysInfoQuery

-- QUERY START: PostgreSQLInsertDNDBTSysInfoQuery
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
    ''CHECK (value = lower(value))''
);';
-- QUERY END: PostgreSQLInsertDNDBTSysInfoQuery

-- QUERY START: PostgreSQLInsertDNDBTSysInfoQuery
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
    ''CHECK (char_length(value) > 3)''
);';
-- QUERY END: PostgreSQLInsertDNDBTSysInfoQuery

-- QUERY START: PostgreSQLCreateEnumTypeQuery
EXECUTE 'CREATE TYPE "MyEnumType1" AS ENUM
(
    ''Label1'',
    ''Label2'',
    ''Label3''
);';
-- QUERY END: PostgreSQLCreateEnumTypeQuery

-- QUERY START: PostgreSQLInsertDNDBTSysInfoQuery
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
-- QUERY END: PostgreSQLInsertDNDBTSysInfoQuery

-- QUERY START: PostgreSQLCreateRangeTypeQuery
EXECUTE 'CREATE TYPE "MyRangeType1" AS RANGE
(
    SUBTYPE = TIMESTAMPTZ,
    SUBTYPE_OPCLASS = "timestamptz_ops",
    MULTIRANGE_TYPE_NAME = "MyRangeType1_multirange"
);';
-- QUERY END: PostgreSQLCreateRangeTypeQuery

-- QUERY START: PostgreSQLInsertDNDBTSysInfoQuery
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
-- QUERY END: PostgreSQLInsertDNDBTSysInfoQuery

-- QUERY START: PostgreSQLAlterTableQuery
EXECUTE 'ALTER TABLE "MyTable1" RENAME TO "MyTable1NewName";
ALTER TABLE "MyTable1NewName" DROP CONSTRAINT "UQ_MyTable1_MyColumn2";
ALTER TABLE "MyTable1NewName" DROP CONSTRAINT "PK_MyTable1";
ALTER TABLE "MyTable1NewName" ALTER COLUMN "MyColumn2" DROP DEFAULT;
ALTER TABLE "MyTable1NewName" DROP COLUMN "MyColumn2";
ALTER TABLE "MyTable1NewName" DROP COLUMN "MyColumn3";
ALTER TABLE "MyTable1NewName" ALTER COLUMN "MyColumn1" DROP DEFAULT;
ALTER TABLE "MyTable1NewName" ALTER COLUMN "MyColumn1" SET DATA TYPE BIGINT
    USING ("MyColumn1"::text::BIGINT);
ALTER TABLE "MyTable1NewName" ALTER COLUMN "MyColumn1" DROP NOT NULL;
ALTER TABLE "MyTable1NewName" ALTER COLUMN "MyColumn1" SET DEFAULT 15;';
-- QUERY END: PostgreSQLAlterTableQuery

-- QUERY START: PostgreSQLDeleteDNDBTSysInfoQuery
EXECUTE 'DELETE FROM "DNDBTDbObjects"
WHERE "ID" = ''f3f08522-26ee-4950-9135-22edf2e4e0cf'';';
-- QUERY END: PostgreSQLDeleteDNDBTSysInfoQuery

-- QUERY START: PostgreSQLDeleteDNDBTSysInfoQuery
EXECUTE 'DELETE FROM "DNDBTDbObjects"
WHERE "ID" = ''37a45def-f4a0-4be7-8bfb-8fbed4a7d705'';';
-- QUERY END: PostgreSQLDeleteDNDBTSysInfoQuery

-- QUERY START: PostgreSQLDeleteDNDBTSysInfoQuery
EXECUTE 'DELETE FROM "DNDBTDbObjects"
WHERE "ID" = ''fe68ee3d-09d0-40ac-93f9-5e441fbb4f70'';';
-- QUERY END: PostgreSQLDeleteDNDBTSysInfoQuery

-- QUERY START: PostgreSQLDeleteDNDBTSysInfoQuery
EXECUTE 'DELETE FROM "DNDBTDbObjects"
WHERE "ID" = ''6e95de30-e01a-4fb4-b8b7-8f0c40bb682c'';';
-- QUERY END: PostgreSQLDeleteDNDBTSysInfoQuery

-- QUERY START: PostgreSQLUpdateDNDBTSysInfoQuery
EXECUTE 'UPDATE "DNDBTDbObjects" SET
    "Name" = ''MyTable1NewName'',
    "Code" = NULL
WHERE "ID" = ''299675e6-4faa-4d0f-a36a-224306ba5bcb'';';
-- QUERY END: PostgreSQLUpdateDNDBTSysInfoQuery

-- QUERY START: PostgreSQLUpdateDNDBTSysInfoQuery
EXECUTE 'UPDATE "DNDBTDbObjects" SET
    "Name" = ''MyColumn1'',
    "Code" = NULL
WHERE "ID" = ''a2f2a4de-1337-4594-ae41-72ed4d05f317'';';
-- QUERY END: PostgreSQLUpdateDNDBTSysInfoQuery

-- QUERY START: PostgreSQLAlterTableQuery
EXECUTE '
ALTER TABLE "MyTable2" DROP CONSTRAINT "PK_MyTable2";
ALTER TABLE "MyTable2" ALTER COLUMN "MyColumn2" DROP DEFAULT;
ALTER TABLE "MyTable2" DROP COLUMN "MyColumn2";
ALTER TABLE "MyTable2" ALTER COLUMN "MyColumn1" DROP DEFAULT;
ALTER TABLE "MyTable2" RENAME COLUMN "MyColumn1" TO "MyColumn1NewName";
ALTER TABLE "MyTable2" ALTER COLUMN "MyColumn1NewName" SET DATA TYPE BIGINT
    USING ("MyColumn1NewName"::text::BIGINT);
ALTER TABLE "MyTable2" ALTER COLUMN "MyColumn1NewName" SET DEFAULT 333;
ALTER TABLE "MyTable2" ADD COLUMN "MyColumn2" BYTEA NULL;
ALTER TABLE "MyTable2" ALTER COLUMN "MyColumn2" SET DEFAULT ''\x000102'';
ALTER TABLE "MyTable2" ADD CONSTRAINT "PK_MyTable2" PRIMARY KEY ("MyColumn1NewName");';
-- QUERY END: PostgreSQLAlterTableQuery

-- QUERY START: PostgreSQLDeleteDNDBTSysInfoQuery
EXECUTE 'DELETE FROM "DNDBTDbObjects"
WHERE "ID" = ''3a43615b-40b3-4a13-99e7-93af7c56e8ce'';';
-- QUERY END: PostgreSQLDeleteDNDBTSysInfoQuery

-- QUERY START: PostgreSQLDeleteDNDBTSysInfoQuery
EXECUTE 'DELETE FROM "DNDBTDbObjects"
WHERE "ID" = ''5a0d1926-3270-4eb2-92eb-00be56c7af23'';';
-- QUERY END: PostgreSQLDeleteDNDBTSysInfoQuery

-- QUERY START: PostgreSQLUpdateDNDBTSysInfoQuery
EXECUTE 'UPDATE "DNDBTDbObjects" SET
    "Name" = ''MyTable2'',
    "Code" = NULL
WHERE "ID" = ''bfb9030c-a8c3-4882-9c42-1c6ad025cf8f'';';
-- QUERY END: PostgreSQLUpdateDNDBTSysInfoQuery

-- QUERY START: PostgreSQLUpdateDNDBTSysInfoQuery
EXECUTE 'UPDATE "DNDBTDbObjects" SET
    "Name" = ''MyColumn1NewName'',
    "Code" = NULL
WHERE "ID" = ''c480f22f-7c01-4f41-b282-35e9f5cd1fe3'';';
-- QUERY END: PostgreSQLUpdateDNDBTSysInfoQuery

-- QUERY START: PostgreSQLInsertDNDBTSysInfoQuery
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
    NULL
);';
-- QUERY END: PostgreSQLInsertDNDBTSysInfoQuery

-- QUERY START: PostgreSQLInsertDNDBTSysInfoQuery
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
    ''PK_MyTable2'',
    NULL
);';
-- QUERY END: PostgreSQLInsertDNDBTSysInfoQuery

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

-- QUERY START: PostgreSQLUpdateDNDBTSysInfoQuery
EXECUTE 'UPDATE "DNDBTDbObjects" SET
    "Name" = ''MyTable5'',
    "Code" = NULL
WHERE "ID" = ''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'';';
-- QUERY END: PostgreSQLUpdateDNDBTSysInfoQuery

-- QUERY START: PostgreSQLUpdateDNDBTSysInfoQuery
EXECUTE 'UPDATE "DNDBTDbObjects" SET
    "Name" = ''MyColumn2'',
    "Code" = NULL
WHERE "ID" = ''15ae6061-426d-4485-85e6-ecd3e0f98882'';';
-- QUERY END: PostgreSQLUpdateDNDBTSysInfoQuery

-- QUERY START: PostgreSQLUpdateDNDBTSysInfoQuery
EXECUTE 'UPDATE "DNDBTDbObjects" SET
    "Name" = ''MyColumn4'',
    "Code" = NULL
WHERE "ID" = ''45856161-db66-49f6-afde-9214d2d2d4b0'';';
-- QUERY END: PostgreSQLUpdateDNDBTSysInfoQuery

-- QUERY START: PostgreSQLUpdateDNDBTSysInfoQuery
EXECUTE 'UPDATE "DNDBTDbObjects" SET
    "Name" = ''MyColumn5'',
    "Code" = NULL
WHERE "ID" = ''b45d163b-f49e-499f-a9e5-2538cd073b80'';';
-- QUERY END: PostgreSQLUpdateDNDBTSysInfoQuery

-- QUERY START: PostgreSQLUpdateDNDBTSysInfoQuery
EXECUTE 'UPDATE "DNDBTDbObjects" SET
    "Name" = ''MyColumn6'',
    "Code" = NULL
WHERE "ID" = ''c8b03b75-a8a2-47e0-bf5c-f3e4f1b8f500'';';
-- QUERY END: PostgreSQLUpdateDNDBTSysInfoQuery

-- QUERY START: PostgreSQLDropTypeQuery
EXECUTE 'DROP TYPE "_DNDBTTemp_MyCompositeType1";';
-- QUERY END: PostgreSQLDropTypeQuery

-- QUERY START: PostgreSQLDropTypeQuery
EXECUTE 'DROP DOMAIN "_DNDBTTemp_MyDomain1";';
-- QUERY END: PostgreSQLDropTypeQuery

-- QUERY START: PostgreSQLDropTypeQuery
EXECUTE 'DROP TYPE "_DNDBTTemp_MyEnumType1";';
-- QUERY END: PostgreSQLDropTypeQuery

-- QUERY START: PostgreSQLDropTypeQuery
EXECUTE 'DROP TYPE "_DNDBTTemp_MyRangeType1";';
-- QUERY END: PostgreSQLDropTypeQuery

-- QUERY START: PostgreSQLCreateTableQuery
EXECUTE 'CREATE TABLE "MyTable3"
(
    "MyColumn1" BIGINT NOT NULL DEFAULT 333,
    "MyColumn2" BYTEA NOT NULL,
    CONSTRAINT "UQ_MyTable3_MyColumns12" UNIQUE ("MyColumn1", "MyColumn2")
);';
-- QUERY END: PostgreSQLCreateTableQuery

-- QUERY START: PostgreSQLInsertDNDBTSysInfoQuery
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
-- QUERY END: PostgreSQLInsertDNDBTSysInfoQuery

-- QUERY START: PostgreSQLInsertDNDBTSysInfoQuery
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
    NULL
);';
-- QUERY END: PostgreSQLInsertDNDBTSysInfoQuery

-- QUERY START: PostgreSQLInsertDNDBTSysInfoQuery
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
-- QUERY END: PostgreSQLInsertDNDBTSysInfoQuery

-- QUERY START: PostgreSQLInsertDNDBTSysInfoQuery
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
-- QUERY END: PostgreSQLInsertDNDBTSysInfoQuery

-- QUERY START: PostgreSQLCreateForeignKeyQuery
EXECUTE 'ALTER TABLE "MyTable1NewName" ADD CONSTRAINT "FK_MyTable1_MyColumn1_MyTable2_MyColumn1" FOREIGN KEY ("MyColumn1")
    REFERENCES "MyTable2" ("MyColumn1NewName")
    ON UPDATE NO ACTION ON DELETE SET NULL;';
-- QUERY END: PostgreSQLCreateForeignKeyQuery

-- QUERY START: PostgreSQLInsertDNDBTSysInfoQuery
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
-- QUERY END: PostgreSQLInsertDNDBTSysInfoQuery

-- QUERY START: PostgreSQLCreateForeignKeyQuery
EXECUTE 'ALTER TABLE "MyTable2" ADD CONSTRAINT "FK_MyTable2_MyColumns12_MyTable3_MyColumns12" FOREIGN KEY ("MyColumn1NewName", "MyColumn2")
    REFERENCES "MyTable3" ("MyColumn1", "MyColumn2")
    ON UPDATE NO ACTION ON DELETE SET DEFAULT;';
-- QUERY END: PostgreSQLCreateForeignKeyQuery

-- QUERY START: PostgreSQLInsertDNDBTSysInfoQuery
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
    ''FK_MyTable2_MyColumns12_MyTable3_MyColumns12'',
    NULL
);';
-- QUERY END: PostgreSQLInsertDNDBTSysInfoQuery

END;
$$