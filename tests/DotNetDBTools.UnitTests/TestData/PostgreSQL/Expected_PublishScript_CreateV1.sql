DO $DNDBTGeneratedScriptTransactionBlock$
BEGIN

-- QUERY START: PostgreSQLCreateCompositeTypeQuery
EXECUTE 'CREATE TYPE "MyCompositeType1" AS
(
    "MyAttribute1" VARCHAR(100),
    "MyAttribute2" INT
);';
-- QUERY END: PostgreSQLCreateCompositeTypeQuery

-- QUERY START: PostgreSQLInsertDNDBTDbObjectRecordQuery
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
-- QUERY END: PostgreSQLInsertDNDBTDbObjectRecordQuery

-- QUERY START: GenericQuery
EXECUTE 'CREATE FUNCTION "MyFunction1"(a INT, b INT)
RETURNS INT
LANGUAGE SQL
IMMUTABLE
AS
$FuncBody$
SELECT a + b;
$FuncBody$';
-- QUERY END: GenericQuery

-- QUERY START: PostgreSQLInsertDNDBTDbObjectRecordQuery
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
-- QUERY END: PostgreSQLInsertDNDBTDbObjectRecordQuery

-- QUERY START: PostgreSQLCreateEnumTypeQuery
EXECUTE 'CREATE TYPE "MyEnumType1" AS ENUM
(
    ''Label1'',
    ''Label2''
);';
-- QUERY END: PostgreSQLCreateEnumTypeQuery

-- QUERY START: PostgreSQLInsertDNDBTDbObjectRecordQuery
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
-- QUERY END: PostgreSQLInsertDNDBTDbObjectRecordQuery

-- QUERY START: PostgreSQLCreateDomainTypeQuery
EXECUTE 'CREATE DOMAIN "MyDomain1" AS VARCHAR(100)    NULL
    CONSTRAINT "MyDomain1_CK1" CHECK (value = lower(value))
    CONSTRAINT "MyDomain1_CK2" CHECK (char_length(value) > 3);';
-- QUERY END: PostgreSQLCreateDomainTypeQuery

-- QUERY START: PostgreSQLInsertDNDBTDbObjectRecordQuery
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
-- QUERY END: PostgreSQLInsertDNDBTDbObjectRecordQuery

-- QUERY START: PostgreSQLInsertDNDBTDbObjectRecordQuery
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
-- QUERY END: PostgreSQLInsertDNDBTDbObjectRecordQuery

-- QUERY START: PostgreSQLInsertDNDBTDbObjectRecordQuery
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
-- QUERY END: PostgreSQLInsertDNDBTDbObjectRecordQuery

-- QUERY START: PostgreSQLCreateRangeTypeQuery
EXECUTE 'DO $DNDBTPlPgSqlQueryBlock$
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
$DNDBTPlPgSqlQueryBlock$';
-- QUERY END: PostgreSQLCreateRangeTypeQuery

-- QUERY START: PostgreSQLInsertDNDBTDbObjectRecordQuery
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
-- QUERY END: PostgreSQLInsertDNDBTDbObjectRecordQuery

-- QUERY START: PostgreSQLCreateTableQuery
EXECUTE 'CREATE TABLE "MyTable1"
(
    "MyColumn1" INT NOT NULL DEFAULT 15,
    "MyColumn2" VARCHAR(10) NOT NULL DEFAULT ''33'',
    "MyColumn3" INT GENERATED ALWAYS AS IDENTITY NOT NULL,
    "MyColumn4" DECIMAL(19, 2) NOT NULL DEFAULT 7.36,
    CONSTRAINT "PK_MyTable1" PRIMARY KEY ("MyColumn3"),
    CONSTRAINT "UQ_MyTable1_MyColumn4" UNIQUE ("MyColumn4"),
    CONSTRAINT "CK_MyTable1_MyCheck1" CHECK ("MyColumn4" >= 0)
);';
-- QUERY END: PostgreSQLCreateTableQuery

-- QUERY START: PostgreSQLInsertDNDBTDbObjectRecordQuery
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
    ''299675e6-4faa-4d0f-a36a-224306ba5bcb'',
    NULL,
    ''Table'',
    ''MyTable1'',
    NULL
);';
-- QUERY END: PostgreSQLInsertDNDBTDbObjectRecordQuery

-- QUERY START: PostgreSQLInsertDNDBTDbObjectRecordQuery
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
    ''a2f2a4de-1337-4594-ae41-72ed4d05f317'',
    ''299675e6-4faa-4d0f-a36a-224306ba5bcb'',
    ''Column'',
    ''MyColumn1'',
    ''15''
);';
-- QUERY END: PostgreSQLInsertDNDBTDbObjectRecordQuery

-- QUERY START: PostgreSQLInsertDNDBTDbObjectRecordQuery
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
    ''fe68ee3d-09d0-40ac-93f9-5e441fbb4f70'',
    ''299675e6-4faa-4d0f-a36a-224306ba5bcb'',
    ''Column'',
    ''MyColumn2'',
    ''''''33''''''
);';
-- QUERY END: PostgreSQLInsertDNDBTDbObjectRecordQuery

-- QUERY START: PostgreSQLInsertDNDBTDbObjectRecordQuery
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
    ''6e95de30-e01a-4fb4-b8b7-8f0c40bb682c'',
    ''299675e6-4faa-4d0f-a36a-224306ba5bcb'',
    ''Column'',
    ''MyColumn3'',
    NULL
);';
-- QUERY END: PostgreSQLInsertDNDBTDbObjectRecordQuery

-- QUERY START: PostgreSQLInsertDNDBTDbObjectRecordQuery
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
    ''867ac528-e87e-4c93-b6e3-dd2fcbbb837f'',
    ''299675e6-4faa-4d0f-a36a-224306ba5bcb'',
    ''Column'',
    ''MyColumn4'',
    ''7.36''
);';
-- QUERY END: PostgreSQLInsertDNDBTDbObjectRecordQuery

-- QUERY START: PostgreSQLInsertDNDBTDbObjectRecordQuery
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
    ''37a45def-f4a0-4be7-8bfb-8fbed4a7d705'',
    ''299675e6-4faa-4d0f-a36a-224306ba5bcb'',
    ''PrimaryKey'',
    ''PK_MyTable1'',
    NULL
);';
-- QUERY END: PostgreSQLInsertDNDBTDbObjectRecordQuery

-- QUERY START: PostgreSQLInsertDNDBTDbObjectRecordQuery
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
    ''f3f08522-26ee-4950-9135-22edf2e4e0cf'',
    ''299675e6-4faa-4d0f-a36a-224306ba5bcb'',
    ''UniqueConstraint'',
    ''UQ_MyTable1_MyColumn4'',
    NULL
);';
-- QUERY END: PostgreSQLInsertDNDBTDbObjectRecordQuery

-- QUERY START: PostgreSQLInsertDNDBTDbObjectRecordQuery
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
    ''CHECK ("MyColumn4" >= 0)''
);';
-- QUERY END: PostgreSQLInsertDNDBTDbObjectRecordQuery

-- QUERY START: PostgreSQLCreateTableQuery
EXECUTE 'CREATE TABLE "MyTable2"
(
    "MyColumn1" INT NOT NULL DEFAULT 333,
    "MyColumn2" BYTEA NULL DEFAULT ''\x000408'',
    CONSTRAINT "PK_MyTable2_CustomName" PRIMARY KEY ("MyColumn1")
);';
-- QUERY END: PostgreSQLCreateTableQuery

-- QUERY START: PostgreSQLInsertDNDBTDbObjectRecordQuery
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
    ''bfb9030c-a8c3-4882-9c42-1c6ad025cf8f'',
    NULL,
    ''Table'',
    ''MyTable2'',
    NULL
);';
-- QUERY END: PostgreSQLInsertDNDBTDbObjectRecordQuery

-- QUERY START: PostgreSQLInsertDNDBTDbObjectRecordQuery
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
    ''c480f22f-7c01-4f41-b282-35e9f5cd1fe3'',
    ''bfb9030c-a8c3-4882-9c42-1c6ad025cf8f'',
    ''Column'',
    ''MyColumn1'',
    ''333''
);';
-- QUERY END: PostgreSQLInsertDNDBTDbObjectRecordQuery

-- QUERY START: PostgreSQLInsertDNDBTDbObjectRecordQuery
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
    ''5a0d1926-3270-4eb2-92eb-00be56c7af23'',
    ''bfb9030c-a8c3-4882-9c42-1c6ad025cf8f'',
    ''Column'',
    ''MyColumn2'',
    ''''''\x000408''''''
);';
-- QUERY END: PostgreSQLInsertDNDBTDbObjectRecordQuery

-- QUERY START: PostgreSQLInsertDNDBTDbObjectRecordQuery
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
-- QUERY END: PostgreSQLInsertDNDBTDbObjectRecordQuery

-- QUERY START: PostgreSQLCreateTableQuery
EXECUTE 'CREATE TABLE "MyTable4"
(
    "MyColumn1" BIGINT NOT NULL
);';
-- QUERY END: PostgreSQLCreateTableQuery

-- QUERY START: PostgreSQLInsertDNDBTDbObjectRecordQuery
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
    ''b12a6a37-7739-48e0-a9e1-499ae7d2a395'',
    NULL,
    ''Table'',
    ''MyTable4'',
    NULL
);';
-- QUERY END: PostgreSQLInsertDNDBTDbObjectRecordQuery

-- QUERY START: PostgreSQLInsertDNDBTDbObjectRecordQuery
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
    ''de0425b8-9f99-4d76-9a64-09e52f8b5d5a'',
    ''b12a6a37-7739-48e0-a9e1-499ae7d2a395'',
    ''Column'',
    ''MyColumn1'',
    NULL
);';
-- QUERY END: PostgreSQLInsertDNDBTDbObjectRecordQuery

-- QUERY START: PostgreSQLCreateTableQuery
EXECUTE 'CREATE TABLE "MyTable5"
(
    "MyColumn1" INT NOT NULL DEFAULT "MyFunction1"(-25, 10),
    "MyColumn10" TIME NOT NULL DEFAULT ''16:17:18'',
    "MyColumn11" TIMESTAMP NOT NULL DEFAULT ''2022-02-15 16:17:18'',
    "MyColumn12" TIMESTAMPTZ NOT NULL DEFAULT ''2022-02-15 14:47:18+00'',
    "MyColumn13" "MyCompositeType1" NOT NULL,
    "MyColumn14" "MyDomain1" NOT NULL,
    "MyColumn15" "MyEnumType1" NOT NULL,
    "MyColumn16" "MyRangeType1" NOT NULL,
    "MyColumn2" CHAR(4) NOT NULL DEFAULT ''test'',
    "MyColumn3" BYTEA NOT NULL DEFAULT ''\x000204'',
    "MyColumn4" FLOAT4 NOT NULL DEFAULT 123.456,
    "MyColumn5" FLOAT8 NOT NULL DEFAULT 12345.6789,
    "MyColumn6" DECIMAL(6, 1) NOT NULL DEFAULT 12.3,
    "MyColumn7" BOOL NOT NULL DEFAULT TRUE,
    "MyColumn8" UUID NOT NULL DEFAULT ''8e2f99ad-0fc8-456d-b0e4-ec3ba572dd15'',
    "MyColumn9" DATE NOT NULL DEFAULT ''2022-02-15'',
    CONSTRAINT "PK_MyTable5_CustomName" PRIMARY KEY ("MyColumn2", "MyColumn1"),
    CONSTRAINT "UQ_MyTable5_CustomName" UNIQUE ("MyColumn6", "MyColumn3", "MyColumn7")
);';
-- QUERY END: PostgreSQLCreateTableQuery

-- QUERY START: PostgreSQLInsertDNDBTDbObjectRecordQuery
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
    ''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'',
    NULL,
    ''Table'',
    ''MyTable5'',
    NULL
);';
-- QUERY END: PostgreSQLInsertDNDBTDbObjectRecordQuery

-- QUERY START: PostgreSQLInsertDNDBTDbObjectRecordQuery
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
    ''5309d66f-2030-402e-912e-5547babaa072'',
    ''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'',
    ''Column'',
    ''MyColumn1'',
    ''"MyFunction1"(-25, 10)''
);';
-- QUERY END: PostgreSQLInsertDNDBTDbObjectRecordQuery

-- QUERY START: PostgreSQLInsertDNDBTDbObjectRecordQuery
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
    ''cba4849b-3d84-4e38-b2c8-f9dbdff22fa6'',
    ''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'',
    ''Column'',
    ''MyColumn10'',
    ''''''16:17:18''''''
);';
-- QUERY END: PostgreSQLInsertDNDBTDbObjectRecordQuery

-- QUERY START: PostgreSQLInsertDNDBTDbObjectRecordQuery
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
    ''4dde852d-ec19-4b61-80f9-da428d8ff41a'',
    ''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'',
    ''Column'',
    ''MyColumn11'',
    ''''''2022-02-15 16:17:18''''''
);';
-- QUERY END: PostgreSQLInsertDNDBTDbObjectRecordQuery

-- QUERY START: PostgreSQLInsertDNDBTDbObjectRecordQuery
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
    ''685faf2e-fef7-4e6b-a960-acd093f1f004'',
    ''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'',
    ''Column'',
    ''MyColumn12'',
    ''''''2022-02-15 14:47:18+00''''''
);';
-- QUERY END: PostgreSQLInsertDNDBTDbObjectRecordQuery

-- QUERY START: PostgreSQLInsertDNDBTDbObjectRecordQuery
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
    ''15ae6061-426d-4485-85e6-ecd3e0f98882'',
    ''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'',
    ''Column'',
    ''MyColumn13'',
    NULL
);';
-- QUERY END: PostgreSQLInsertDNDBTDbObjectRecordQuery

-- QUERY START: PostgreSQLInsertDNDBTDbObjectRecordQuery
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
    ''45856161-db66-49f6-afde-9214d2d2d4b0'',
    ''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'',
    ''Column'',
    ''MyColumn14'',
    NULL
);';
-- QUERY END: PostgreSQLInsertDNDBTDbObjectRecordQuery

-- QUERY START: PostgreSQLInsertDNDBTDbObjectRecordQuery
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
    ''b45d163b-f49e-499f-a9e5-2538cd073b80'',
    ''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'',
    ''Column'',
    ''MyColumn15'',
    NULL
);';
-- QUERY END: PostgreSQLInsertDNDBTDbObjectRecordQuery

-- QUERY START: PostgreSQLInsertDNDBTDbObjectRecordQuery
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
    ''c8b03b75-a8a2-47e0-bf5c-f3e4f1b8f500'',
    ''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'',
    ''Column'',
    ''MyColumn16'',
    NULL
);';
-- QUERY END: PostgreSQLInsertDNDBTDbObjectRecordQuery

-- QUERY START: PostgreSQLInsertDNDBTDbObjectRecordQuery
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
    ''11ef8e25-3691-42d4-b2fa-88d724f73b61'',
    ''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'',
    ''Column'',
    ''MyColumn2'',
    ''''''test''''''
);';
-- QUERY END: PostgreSQLInsertDNDBTDbObjectRecordQuery

-- QUERY START: PostgreSQLInsertDNDBTDbObjectRecordQuery
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
    ''6ed0ab37-aad3-4294-9ba6-c0921f0e67af'',
    ''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'',
    ''Column'',
    ''MyColumn3'',
    ''''''\x000204''''''
);';
-- QUERY END: PostgreSQLInsertDNDBTDbObjectRecordQuery

-- QUERY START: PostgreSQLInsertDNDBTDbObjectRecordQuery
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
    ''aca57fd6-80d0-4c18-b2ca-aabcb06bea10'',
    ''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'',
    ''Column'',
    ''MyColumn4'',
    ''123.456''
);';
-- QUERY END: PostgreSQLInsertDNDBTDbObjectRecordQuery

-- QUERY START: PostgreSQLInsertDNDBTDbObjectRecordQuery
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
    ''47666b8b-ca72-4507-86b2-04c47a84aed4'',
    ''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'',
    ''Column'',
    ''MyColumn5'',
    ''12345.6789''
);';
-- QUERY END: PostgreSQLInsertDNDBTDbObjectRecordQuery

-- QUERY START: PostgreSQLInsertDNDBTDbObjectRecordQuery
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
    ''98fded6c-d486-4a2e-9c9a-1ec31c9d5830'',
    ''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'',
    ''Column'',
    ''MyColumn6'',
    ''12.3''
);';
-- QUERY END: PostgreSQLInsertDNDBTDbObjectRecordQuery

-- QUERY START: PostgreSQLInsertDNDBTDbObjectRecordQuery
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
    ''2502cade-458a-48ee-9421-e6d7850493f7'',
    ''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'',
    ''Column'',
    ''MyColumn7'',
    ''TRUE''
);';
-- QUERY END: PostgreSQLInsertDNDBTDbObjectRecordQuery

-- QUERY START: PostgreSQLInsertDNDBTDbObjectRecordQuery
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
    ''ed044a8a-6858-41e2-a867-9e5b01f226c8'',
    ''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'',
    ''Column'',
    ''MyColumn8'',
    ''''''8e2f99ad-0fc8-456d-b0e4-ec3ba572dd15''''''
);';
-- QUERY END: PostgreSQLInsertDNDBTDbObjectRecordQuery

-- QUERY START: PostgreSQLInsertDNDBTDbObjectRecordQuery
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
    ''9939d676-73b7-42d1-ba3e-5c13aed5ce34'',
    ''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'',
    ''Column'',
    ''MyColumn9'',
    ''''''2022-02-15''''''
);';
-- QUERY END: PostgreSQLInsertDNDBTDbObjectRecordQuery

-- QUERY START: PostgreSQLInsertDNDBTDbObjectRecordQuery
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
    ''79384d48-a39b-4a22-900e-066b2ca67ba2'',
    ''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'',
    ''PrimaryKey'',
    ''PK_MyTable5_CustomName'',
    NULL
);';
-- QUERY END: PostgreSQLInsertDNDBTDbObjectRecordQuery

-- QUERY START: PostgreSQLInsertDNDBTDbObjectRecordQuery
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
    ''5293b58a-9f63-4f0f-8d6f-18416ebbd751'',
    ''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'',
    ''UniqueConstraint'',
    ''UQ_MyTable5_CustomName'',
    NULL
);';
-- QUERY END: PostgreSQLInsertDNDBTDbObjectRecordQuery

-- QUERY START: PostgreSQLCreateTableQuery
EXECUTE 'CREATE TABLE "MyTable6"
(
    "MyColumn1" CHAR(4) NULL,
    "MyColumn2" INT NULL
);';
-- QUERY END: PostgreSQLCreateTableQuery

-- QUERY START: PostgreSQLInsertDNDBTDbObjectRecordQuery
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
    ''f3064a8c-346a-4b3d-af2c-d967b39841e4'',
    NULL,
    ''Table'',
    ''MyTable6'',
    NULL
);';
-- QUERY END: PostgreSQLInsertDNDBTDbObjectRecordQuery

-- QUERY START: PostgreSQLInsertDNDBTDbObjectRecordQuery
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
    ''bfa08c82-5c8f-4ab4-bd41-1f1d85cf3c85'',
    ''f3064a8c-346a-4b3d-af2c-d967b39841e4'',
    ''Column'',
    ''MyColumn1'',
    NULL
);';
-- QUERY END: PostgreSQLInsertDNDBTDbObjectRecordQuery

-- QUERY START: PostgreSQLInsertDNDBTDbObjectRecordQuery
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
    ''a402e2b7-c826-4cfd-a304-97c9bc346ba2'',
    ''f3064a8c-346a-4b3d-af2c-d967b39841e4'',
    ''Column'',
    ''MyColumn2'',
    NULL
);';
-- QUERY END: PostgreSQLInsertDNDBTDbObjectRecordQuery

-- QUERY START: PostgreSQLCreateIndexQuery
EXECUTE 'CREATE UNIQUE INDEX "IDX_MyTable2_MyIndex1"
ON "MyTable2" ("MyColumn1", "MyColumn2");';
-- QUERY END: PostgreSQLCreateIndexQuery

-- QUERY START: PostgreSQLInsertDNDBTDbObjectRecordQuery
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
-- QUERY END: PostgreSQLInsertDNDBTDbObjectRecordQuery

-- QUERY START: PostgreSQLCreateIndexQuery
EXECUTE 'CREATE INDEX "IDX_MyTable5_CustomName"
ON "MyTable5" ("MyColumn8");';
-- QUERY END: PostgreSQLCreateIndexQuery

-- QUERY START: PostgreSQLInsertDNDBTDbObjectRecordQuery
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
    ''1d632285-9914-4c5d-98e6-a618a99bd799'',
    ''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'',
    ''Index'',
    ''IDX_MyTable5_CustomName'',
    NULL
);';
-- QUERY END: PostgreSQLInsertDNDBTDbObjectRecordQuery

-- QUERY START: PostgreSQLCreateForeignKeyQuery
EXECUTE 'ALTER TABLE "MyTable1" ADD CONSTRAINT "FK_MyTable1_MyColumn1_MyTable2_MyColumn1" FOREIGN KEY ("MyColumn1")
    REFERENCES "MyTable2" ("MyColumn1")
    ON UPDATE NO ACTION ON DELETE CASCADE;';
-- QUERY END: PostgreSQLCreateForeignKeyQuery

-- QUERY START: PostgreSQLInsertDNDBTDbObjectRecordQuery
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
-- QUERY END: PostgreSQLInsertDNDBTDbObjectRecordQuery

-- QUERY START: PostgreSQLCreateForeignKeyQuery
EXECUTE 'ALTER TABLE "MyTable6" ADD CONSTRAINT "FK_MyTable6_MyTable5_CustomName" FOREIGN KEY ("MyColumn1", "MyColumn2")
    REFERENCES "MyTable5" ("MyColumn2", "MyColumn1")
    ON UPDATE NO ACTION ON DELETE NO ACTION;';
-- QUERY END: PostgreSQLCreateForeignKeyQuery

-- QUERY START: PostgreSQLInsertDNDBTDbObjectRecordQuery
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
    ''ae453b22-d270-41fc-8184-9ac26b7a0569'',
    ''f3064a8c-346a-4b3d-af2c-d967b39841e4'',
    ''ForeignKey'',
    ''FK_MyTable6_MyTable5_CustomName'',
    NULL
);';
-- QUERY END: PostgreSQLInsertDNDBTDbObjectRecordQuery

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
$FuncBody$';
-- QUERY END: GenericQuery

-- QUERY START: PostgreSQLInsertDNDBTDbObjectRecordQuery
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
    VALUES(NEW."MyColumn1");
    RETURN NULL;
END;
$FuncBody$''
);';
-- QUERY END: PostgreSQLInsertDNDBTDbObjectRecordQuery

-- QUERY START: GenericQuery
EXECUTE 'CREATE VIEW "MyView1" AS
SELECT
    t1."MyColumn1",
    t1."MyColumn4",
    t2."MyColumn2"
FROM "MyTable1" t1
LEFT JOIN "MyTable2" t2
    ON t2."MyColumn1" = t1."MyColumn1"';
-- QUERY END: GenericQuery

-- QUERY START: PostgreSQLInsertDNDBTDbObjectRecordQuery
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
FROM "MyTable1" t1
LEFT JOIN "MyTable2" t2
    ON t2."MyColumn1" = t1."MyColumn1"''
);';
-- QUERY END: PostgreSQLInsertDNDBTDbObjectRecordQuery

-- QUERY START: PostgreSQLCreateTriggerQuery
EXECUTE 'CREATE TRIGGER "TR_MyTable2_MyTrigger1"
AFTER INSERT
ON "MyTable2"
FOR EACH ROW
EXECUTE FUNCTION "TR_MyTable2_MyTrigger1_Handler"()';
-- QUERY END: PostgreSQLCreateTriggerQuery

-- QUERY START: PostgreSQLInsertDNDBTDbObjectRecordQuery
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
-- QUERY END: PostgreSQLInsertDNDBTDbObjectRecordQuery

-- QUERY START: GenericQuery
EXECUTE 'INSERT INTO "MyTable4"("MyColumn1")
SELECT * FROM
(
    SELECT 1
    UNION ALL
    SELECT 2
    UNION ALL
    SELECT 3
) t
WHERE NOT EXISTS (SELECT * FROM "MyTable4")';
-- QUERY END: GenericQuery

-- QUERY START: PostgreSQLInsertDNDBTScriptExecutionRecordQuery
EXECUTE 'INSERT INTO "DNDBTScriptExecutions"
(
    "ID",
    "Type",
    "Name",
    "Code",
    "MinDbVersionToExecute",
    "MaxDbVersionToExecute",
    "ExecutedOnDbVersion"
)
VALUES
(
    ''100d624a-01aa-4730-b86f-f991ac3ed936'',
    ''AfterPublishOnce'',
    ''InsertSomeInitialData'',
    ''INSERT INTO "MyTable4"("MyColumn1")
SELECT * FROM
(
    SELECT 1
    UNION ALL
    SELECT 2
    UNION ALL
    SELECT 3
) t
WHERE NOT EXISTS (SELECT * FROM "MyTable4")'',
    0,
    9223372036854775807,
    0
);';
-- QUERY END: PostgreSQLInsertDNDBTScriptExecutionRecordQuery

-- QUERY START: PostgreSQLUpdateDNDBTDbAttributesRecordQuery
EXECUTE 'UPDATE "DNDBTDbAttributes" SET
    "Version" = 1;';
-- QUERY END: PostgreSQLUpdateDNDBTDbAttributesRecordQuery

END;
$DNDBTGeneratedScriptTransactionBlock$