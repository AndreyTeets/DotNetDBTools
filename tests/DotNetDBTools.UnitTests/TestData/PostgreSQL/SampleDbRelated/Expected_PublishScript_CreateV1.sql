DO $DNDBTGeneratedScriptTransactionBlock$
BEGIN

-- QUERY START: CreateSequenceQuery
EXECUTE 'CREATE SEQUENCE "MySequence1"
    AS BIGINT
    START -1000 INCREMENT 2 MINVALUE -2147483648 MAXVALUE 0 CACHE 1 CYCLE;';
-- QUERY END: CreateSequenceQuery

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
    ''f54a1a93-8cd2-4125-aede-b38cc7f8a750'',
    NULL,
    ''Sequence'',
    ''MySequence1'',
    NULL
);';
-- QUERY END: InsertDNDBTDbObjectRecordQuery

-- QUERY START: CreateSequenceQuery
EXECUTE 'CREATE SEQUENCE "MySequence2"
    AS INT
    START 1 INCREMENT 1 MINVALUE 1 MAXVALUE 2147483647 CACHE 1 NO CYCLE;';
-- QUERY END: CreateSequenceQuery

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
    ''59c3bf9d-4938-40df-9528-f1aa8367c6e3'',
    NULL,
    ''Sequence'',
    ''MySequence2'',
    NULL
);';
-- QUERY END: InsertDNDBTDbObjectRecordQuery

-- QUERY START: CreateFunctionQuery
EXECUTE 'CREATE FUNCTION "MyFunction1"(a INT, b INT)
RETURNS INT
LANGUAGE SQL
IMMUTABLE
AS
$FuncBody$
SELECT a + b + 1;
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
SELECT a + b + 1;
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
    VALUES(NEW."MyColumn1");
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
    VALUES(NEW."MyColumn1");
    RETURN NULL;
END;
$FuncBody$''
);';
-- QUERY END: InsertDNDBTDbObjectRecordQuery

-- QUERY START: CreateCompositeTypeQuery
EXECUTE 'CREATE TYPE "MyCompositeType1" AS
(
    "MyAttribute1" VARCHAR(100),
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
    VARCHAR(100) NULL
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
    ''Label2''
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

-- QUERY START: CreateTableQuery
EXECUTE 'CREATE TABLE "MyTable1"
(
    "MyColumn1" INT NOT NULL DEFAULT 15,
    "MyColumn2" TEXT NULL,
    "MyColumn3" INT GENERATED ALWAYS AS IDENTITY (START 1000 INCREMENT -2 MINVALUE -2147483648 MAXVALUE 2147483647 CACHE 1 NO CYCLE) NOT NULL,
    "MyColumn4" DECIMAL NOT NULL DEFAULT 736,
    "MyColumn5" VARCHAR(1000) NULL DEFAULT ''some text'',
    CONSTRAINT "PK_MyTable1" PRIMARY KEY ("MyColumn3"),
    CONSTRAINT "UQ_MyTable1_MyColumn4" UNIQUE ("MyColumn4"),
    CONSTRAINT "CK_MyTable1_MyCheck1" CHECK ("MyColumn4" >= 0)
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
    ''299675e6-4faa-4d0f-a36a-224306ba5bcb'',
    NULL,
    ''Table'',
    ''MyTable1'',
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
    ''a2f2a4de-1337-4594-ae41-72ed4d05f317'',
    ''299675e6-4faa-4d0f-a36a-224306ba5bcb'',
    ''Column'',
    ''MyColumn1'',
    ''15''
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
    ''fe68ee3d-09d0-40ac-93f9-5e441fbb4f70'',
    ''299675e6-4faa-4d0f-a36a-224306ba5bcb'',
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
    ''6e95de30-e01a-4fb4-b8b7-8f0c40bb682c'',
    ''299675e6-4faa-4d0f-a36a-224306ba5bcb'',
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
    ''867ac528-e87e-4c93-b6e3-dd2fcbbb837f'',
    ''299675e6-4faa-4d0f-a36a-224306ba5bcb'',
    ''Column'',
    ''MyColumn4'',
    ''736''
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
    ''ebbef06c-c7de-4b36-a911-827566639630'',
    ''299675e6-4faa-4d0f-a36a-224306ba5bcb'',
    ''Column'',
    ''MyColumn5'',
    ''''''some text''''''
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
    ''37a45def-f4a0-4be7-8bfb-8fbed4a7d705'',
    ''299675e6-4faa-4d0f-a36a-224306ba5bcb'',
    ''PrimaryKey'',
    ''PK_MyTable1'',
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
    ''f3f08522-26ee-4950-9135-22edf2e4e0cf'',
    ''299675e6-4faa-4d0f-a36a-224306ba5bcb'',
    ''UniqueConstraint'',
    ''UQ_MyTable1_MyColumn4'',
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
    ''eb9c59b5-bc7e-49d7-adaa-f5600b6a19a2'',
    ''299675e6-4faa-4d0f-a36a-224306ba5bcb'',
    ''CheckConstraint'',
    ''CK_MyTable1_MyCheck1'',
    ''"MyColumn4" >= 0''
);';
-- QUERY END: InsertDNDBTDbObjectRecordQuery

-- QUERY START: CreateTableQuery
EXECUTE 'CREATE TABLE "MyTable2"
(
    "MyColumn1" INT NOT NULL DEFAULT 333,
    "MyColumn2" BYTEA NULL DEFAULT ''\x000408'',
    CONSTRAINT "PK_MyTable2_CustomName" PRIMARY KEY ("MyColumn1")
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
    ''bfb9030c-a8c3-4882-9c42-1c6ad025cf8f'',
    NULL,
    ''Table'',
    ''MyTable2'',
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
    ''c480f22f-7c01-4f41-b282-35e9f5cd1fe3'',
    ''bfb9030c-a8c3-4882-9c42-1c6ad025cf8f'',
    ''Column'',
    ''MyColumn1'',
    ''333''
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
    ''5a0d1926-3270-4eb2-92eb-00be56c7af23'',
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
    ''3a43615b-40b3-4a13-99e7-93af7c56e8ce'',
    ''bfb9030c-a8c3-4882-9c42-1c6ad025cf8f'',
    ''PrimaryKey'',
    ''PK_MyTable2_CustomName'',
    NULL
);';
-- QUERY END: InsertDNDBTDbObjectRecordQuery

-- QUERY START: CreateTableQuery
EXECUTE 'CREATE TABLE "MyTable4"
(
    "MyColumn1" BIGINT NOT NULL,
    "MyColumn2" INT GENERATED ALWAYS AS IDENTITY (START 1 INCREMENT 1 MINVALUE 1 MAXVALUE 2147483647 CACHE 1 NO CYCLE) NOT NULL,
    CONSTRAINT "PK_MyTable4" PRIMARY KEY ("MyColumn2")
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
    ''b12a6a37-7739-48e0-a9e1-499ae7d2a395'',
    NULL,
    ''Table'',
    ''MyTable4'',
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
    ''de0425b8-9f99-4d76-9a64-09e52f8b5d5a'',
    ''b12a6a37-7739-48e0-a9e1-499ae7d2a395'',
    ''Column'',
    ''MyColumn1'',
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
    ''a6354ea4-7113-4c14-8047-648f0cfc7163'',
    ''b12a6a37-7739-48e0-a9e1-499ae7d2a395'',
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
    ''53ad5415-7fea-4a51-bcae-65e349a2e477'',
    ''b12a6a37-7739-48e0-a9e1-499ae7d2a395'',
    ''PrimaryKey'',
    ''PK_MyTable4'',
    NULL
);';
-- QUERY END: InsertDNDBTDbObjectRecordQuery

-- QUERY START: CreateTableQuery
EXECUTE 'CREATE TABLE "MyTable5"
(
    "MyColumn1" INT NOT NULL DEFAULT ABs(-15),
    "MyColumn10" TIME NOT NULL DEFAULT ''16:17:18'',
    "MyColumn101" "MyCompositeType1" NOT NULL,
    "MyColumn102" "MyDomain1" NOT NULL,
    "MyColumn103" "MyEnumType1" NOT NULL,
    "MyColumn104" "MyRangeType1" NOT NULL,
    "MyColumn11" TIMESTAMP NOT NULL DEFAULT ''2022-02-15 16:17:18'',
    "MyColumn12" TIMESTAMPTZ NOT NULL DEFAULT ''2022-02-15 14:47:18+00'',
    "MyColumn2" CHAR(4) NOT NULL DEFAULT ''test'',
    "MyColumn201" INT NULL DEFAULT "MyFunction1"(-25, 10),
    "MyColumn202" "MyDomain2" NOT NULL,
    "MyColumn203" INT GENERATED ALWAYS AS IDENTITY (START 1000 INCREMENT -2 MINVALUE -2147483648 MAXVALUE 2147483647 CACHE 1 NO CYCLE) NOT NULL,
    "MyColumn3" BYTEA NOT NULL DEFAULT ''\x000204'',
    "MyColumn301" SMALLINT NULL,
    "MyColumn302" SMALLINT NULL,
    "MyColumn303" MONEY NULL,
    "MyColumn304" JSON NULL,
    "MyColumn305" JSONB NULL,
    "MyColumn306" XML NULL,
    "MyColumn307" TSQUERY NULL,
    "MyColumn308" TSVECTOR NULL,
    "MyColumn309" POINT NULL,
    "MyColumn310" LINE NULL,
    "MyColumn311" LSEG NULL,
    "MyColumn312" BOX NULL,
    "MyColumn313" PATH NULL,
    "MyColumn314" POLYGON NULL,
    "MyColumn315" CIRCLE NULL,
    "MyColumn316" INET NULL,
    "MyColumn317" CIDR NULL,
    "MyColumn318" MACADDR NULL,
    "MyColumn319" MACADDR8 NULL,
    "MyColumn320" DECIMAL NULL,
    "MyColumn321" TIMETZ NULL,
    "MyColumn322" TIME(4) NULL,
    "MyColumn323" TIMETZ(4) NULL,
    "MyColumn324" TIMESTAMP(4) NULL,
    "MyColumn325" TIMESTAMPTZ(4) NULL,
    "MyColumn326" INTERVAL NULL,
    "MyColumn327" INTERVAL MINUTE TO SECOND(6) NULL,
    "MyColumn328" BIT NULL,
    "MyColumn329" VARBIT NULL,
    "MyColumn330" BIT(10) NULL,
    "MyColumn331" VARBIT(20) NULL,
    "MyColumn332" CHAR NULL,
    "MyColumn333" VARCHAR NULL,
    "MyColumn334" CHAR(30) NULL,
    "MyColumn335" VARCHAR(40) NULL,
    "MyColumn336" JSONB[][] NULL,
    "MyColumn337" DECIMAL(10,4)[][] NULL,
    "MyColumn338" TIMETZ(4)[] NULL,
    "MyColumn339" "MyCompositeType1"[] NULL,
    "MyColumn340" "MyCompositeType1"[] NULL,
    "MyColumn4" FLOAT4 NOT NULL DEFAULT 123.456,
    "MyColumn5" FLOAT8 NOT NULL DEFAULT 12345.6789,
    "MyColumn6" DECIMAL(6,1) NOT NULL DEFAULT 12.3,
    "MyColumn7" BOOL NOT NULL DEFAULT TRUE,
    "MyColumn8" UUID NOT NULL DEFAULT ''8e2f99ad-0fc8-456d-b0e4-ec3ba572dd15'',
    "MyColumn9" DATE NOT NULL DEFAULT ''2022-02-15'',
    CONSTRAINT "PK_MyTable5_CustomName" PRIMARY KEY ("MyColumn2", "MyColumn1"),
    CONSTRAINT "UQ_MyTable5_CustomName" UNIQUE ("MyColumn6", "MyColumn3", "MyColumn7")
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
    ''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'',
    NULL,
    ''Table'',
    ''MyTable5'',
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
    ''5309d66f-2030-402e-912e-5547babaa072'',
    ''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'',
    ''Column'',
    ''MyColumn1'',
    ''ABs(-15)''
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
    ''cba4849b-3d84-4e38-b2c8-f9dbdff22fa6'',
    ''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'',
    ''Column'',
    ''MyColumn10'',
    ''''''16:17:18''''''
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
    ''15ae6061-426d-4485-85e6-ecd3e0f98882'',
    ''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'',
    ''Column'',
    ''MyColumn101'',
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
    ''45856161-db66-49f6-afde-9214d2d2d4b0'',
    ''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'',
    ''Column'',
    ''MyColumn102'',
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
    ''b45d163b-f49e-499f-a9e5-2538cd073b80'',
    ''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'',
    ''Column'',
    ''MyColumn103'',
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
    ''c8b03b75-a8a2-47e0-bf5c-f3e4f1b8f500'',
    ''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'',
    ''Column'',
    ''MyColumn104'',
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
    ''4dde852d-ec19-4b61-80f9-da428d8ff41a'',
    ''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'',
    ''Column'',
    ''MyColumn11'',
    ''''''2022-02-15 16:17:18''''''
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
    ''685faf2e-fef7-4e6b-a960-acd093f1f004'',
    ''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'',
    ''Column'',
    ''MyColumn12'',
    ''''''2022-02-15 14:47:18+00''''''
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
    ''11ef8e25-3691-42d4-b2fa-88d724f73b61'',
    ''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'',
    ''Column'',
    ''MyColumn2'',
    ''''''test''''''
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
    ''5c455ec9-9830-4d0b-a88c-57341899dc4a'',
    ''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'',
    ''Column'',
    ''MyColumn201'',
    ''"MyFunction1"(-25, 10)''
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
    ''029a13bc-d972-45c8-8a6e-6e1acc3f25b1'',
    ''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'',
    ''Column'',
    ''MyColumn202'',
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
    ''c1f037d5-0656-43d1-8f30-f0b7b452d594'',
    ''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'',
    ''Column'',
    ''MyColumn203'',
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
    ''6ed0ab37-aad3-4294-9ba6-c0921f0e67af'',
    ''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'',
    ''Column'',
    ''MyColumn3'',
    ''''''\x000204''''''
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
    ''5a6fdf6e-f39e-41bf-84fd-6b1becab248b'',
    ''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'',
    ''Column'',
    ''MyColumn301'',
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
    ''4f4b70fd-178d-468f-8575-1d41ed28afc4'',
    ''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'',
    ''Column'',
    ''MyColumn302'',
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
    ''99eae06e-5188-43aa-be38-b353adc5aacf'',
    ''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'',
    ''Column'',
    ''MyColumn303'',
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
    ''e4eea54c-b11b-4c85-b98e-aac299776845'',
    ''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'',
    ''Column'',
    ''MyColumn304'',
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
    ''2624c566-e9d5-4dd9-974a-bf031f73a714'',
    ''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'',
    ''Column'',
    ''MyColumn305'',
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
    ''616314fc-e56a-424d-81ee-ae89be650d42'',
    ''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'',
    ''Column'',
    ''MyColumn306'',
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
    ''b6f006fa-2a78-4965-ac10-0d2cf05d60f0'',
    ''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'',
    ''Column'',
    ''MyColumn307'',
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
    ''129fd264-af69-4aa7-b4ef-6b9923340cd8'',
    ''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'',
    ''Column'',
    ''MyColumn308'',
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
    ''d1ae8ccd-526a-46e1-a5a0-39f25cc391ce'',
    ''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'',
    ''Column'',
    ''MyColumn309'',
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
    ''f3f38026-18cc-49ec-9d4b-3e844427a6f8'',
    ''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'',
    ''Column'',
    ''MyColumn310'',
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
    ''87c48981-bdb7-4da7-ac4a-1e03913133fa'',
    ''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'',
    ''Column'',
    ''MyColumn311'',
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
    ''7b535f7e-bcc3-4950-b8af-a16ea36fb7bb'',
    ''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'',
    ''Column'',
    ''MyColumn312'',
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
    ''1c4c38cc-8266-4992-b87a-179b0affb526'',
    ''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'',
    ''Column'',
    ''MyColumn313'',
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
    ''448d6de4-9d8e-47cb-99e7-9801e78a3e7f'',
    ''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'',
    ''Column'',
    ''MyColumn314'',
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
    ''a1e9678b-12bd-4c2f-88a5-8f203a10d4bf'',
    ''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'',
    ''Column'',
    ''MyColumn315'',
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
    ''f8ef2969-0182-44c0-b9e5-58506b2353be'',
    ''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'',
    ''Column'',
    ''MyColumn316'',
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
    ''1d70b438-3a2c-4d8a-9fb8-2e39bacd2582'',
    ''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'',
    ''Column'',
    ''MyColumn317'',
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
    ''eeb2bca0-4d76-4b75-98c1-130a62268265'',
    ''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'',
    ''Column'',
    ''MyColumn318'',
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
    ''9594f5f1-d746-4efa-a174-65e9eb82eea0'',
    ''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'',
    ''Column'',
    ''MyColumn319'',
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
    ''78aaf5ec-b0c6-4503-8fa7-a87e1d45f532'',
    ''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'',
    ''Column'',
    ''MyColumn320'',
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
    ''dc2e14b0-0fd7-44d9-92a7-39ee99358459'',
    ''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'',
    ''Column'',
    ''MyColumn321'',
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
    ''4c2aa5f2-893f-42f1-a39d-d81123988b2e'',
    ''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'',
    ''Column'',
    ''MyColumn322'',
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
    ''710e422f-7899-45fa-8b1d-d2543519ffc1'',
    ''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'',
    ''Column'',
    ''MyColumn323'',
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
    ''fb0562d8-afba-4939-8cd3-89878c572b56'',
    ''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'',
    ''Column'',
    ''MyColumn324'',
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
    ''baabb5c1-e231-4c0b-97ec-f6be8fba018f'',
    ''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'',
    ''Column'',
    ''MyColumn325'',
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
    ''4763c35f-bdbd-485e-9010-a950cb5a4bdb'',
    ''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'',
    ''Column'',
    ''MyColumn326'',
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
    ''ecbfa2f0-98a1-4d13-9da6-772a80dddac7'',
    ''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'',
    ''Column'',
    ''MyColumn327'',
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
    ''36049ef7-16f2-41f3-ab8c-69d23726ad42'',
    ''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'',
    ''Column'',
    ''MyColumn328'',
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
    ''03f0a0a6-cf85-41dc-8c3a-879ae832e9ab'',
    ''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'',
    ''Column'',
    ''MyColumn329'',
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
    ''9ec1ef6a-9df3-4633-889a-660f53a4866f'',
    ''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'',
    ''Column'',
    ''MyColumn330'',
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
    ''eeb5b0e8-0b1f-4ec6-b47b-068e5b303255'',
    ''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'',
    ''Column'',
    ''MyColumn331'',
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
    ''ab4454ec-a6b4-4d51-b0fa-0e667b26326d'',
    ''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'',
    ''Column'',
    ''MyColumn332'',
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
    ''684f2d90-9eea-4879-b2d5-0197d32654b0'',
    ''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'',
    ''Column'',
    ''MyColumn333'',
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
    ''54b3d4dd-b3a9-4b99-915d-dff9fe24d7c2'',
    ''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'',
    ''Column'',
    ''MyColumn334'',
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
    ''3631d9cb-c041-44e5-a964-c2751918c234'',
    ''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'',
    ''Column'',
    ''MyColumn335'',
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
    ''00dfce94-1c1d-4fc2-bdb0-8e1306a248a2'',
    ''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'',
    ''Column'',
    ''MyColumn336'',
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
    ''9430c94b-7ded-47b1-83bb-a041f0edee88'',
    ''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'',
    ''Column'',
    ''MyColumn337'',
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
    ''86107168-6f36-43f9-8a56-78d19049beda'',
    ''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'',
    ''Column'',
    ''MyColumn338'',
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
    ''a9863561-9309-4911-94d8-c12d21b0884e'',
    ''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'',
    ''Column'',
    ''MyColumn339'',
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
    ''0d33ed85-9909-46e7-8369-eee86b563519'',
    ''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'',
    ''Column'',
    ''MyColumn340'',
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
    ''aca57fd6-80d0-4c18-b2ca-aabcb06bea10'',
    ''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'',
    ''Column'',
    ''MyColumn4'',
    ''123.456''
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
    ''47666b8b-ca72-4507-86b2-04c47a84aed4'',
    ''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'',
    ''Column'',
    ''MyColumn5'',
    ''12345.6789''
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
    ''98fded6c-d486-4a2e-9c9a-1ec31c9d5830'',
    ''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'',
    ''Column'',
    ''MyColumn6'',
    ''12.3''
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
    ''2502cade-458a-48ee-9421-e6d7850493f7'',
    ''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'',
    ''Column'',
    ''MyColumn7'',
    ''TRUE''
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
    ''ed044a8a-6858-41e2-a867-9e5b01f226c8'',
    ''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'',
    ''Column'',
    ''MyColumn8'',
    ''''''8e2f99ad-0fc8-456d-b0e4-ec3ba572dd15''''''
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
    ''9939d676-73b7-42d1-ba3e-5c13aed5ce34'',
    ''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'',
    ''Column'',
    ''MyColumn9'',
    ''''''2022-02-15''''''
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
    ''79384d48-a39b-4a22-900e-066b2ca67ba2'',
    ''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'',
    ''PrimaryKey'',
    ''PK_MyTable5_CustomName'',
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
    ''5293b58a-9f63-4f0f-8d6f-18416ebbd751'',
    ''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'',
    ''UniqueConstraint'',
    ''UQ_MyTable5_CustomName'',
    NULL
);';
-- QUERY END: InsertDNDBTDbObjectRecordQuery

-- QUERY START: CreateTableQuery
EXECUTE 'CREATE TABLE "MyTable6"
(
    "MyColumn1" CHAR(4) NULL,
    "MyColumn2" INT NULL
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
    ''f3064a8c-346a-4b3d-af2c-d967b39841e4'',
    NULL,
    ''Table'',
    ''MyTable6'',
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
    ''bfa08c82-5c8f-4ab4-bd41-1f1d85cf3c85'',
    ''f3064a8c-346a-4b3d-af2c-d967b39841e4'',
    ''Column'',
    ''MyColumn1'',
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
    ''a402e2b7-c826-4cfd-a304-97c9bc346ba2'',
    ''f3064a8c-346a-4b3d-af2c-d967b39841e4'',
    ''Column'',
    ''MyColumn2'',
    NULL
);';
-- QUERY END: InsertDNDBTDbObjectRecordQuery

-- QUERY START: AlterSequenceQuery
EXECUTE 'ALTER SEQUENCE "MySequence1"
    OWNED BY "MyTable1"."MyColumn1"';
-- QUERY END: AlterSequenceQuery

-- QUERY START: UpdateDNDBTDbObjectRecordQuery
EXECUTE 'UPDATE "DNDBTDbObjects" SET
    "Name" = ''MySequence1''
WHERE "ID" = ''f54a1a93-8cd2-4125-aede-b38cc7f8a750'';';
-- QUERY END: UpdateDNDBTDbObjectRecordQuery

-- QUERY START: AlterSequenceQuery
EXECUTE 'ALTER SEQUENCE "MySequence2"
    OWNED BY "MyTable1"."MyColumn2"';
-- QUERY END: AlterSequenceQuery

-- QUERY START: UpdateDNDBTDbObjectRecordQuery
EXECUTE 'UPDATE "DNDBTDbObjects" SET
    "Name" = ''MySequence2''
WHERE "ID" = ''59c3bf9d-4938-40df-9528-f1aa8367c6e3'';';
-- QUERY END: UpdateDNDBTDbObjectRecordQuery

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
FROM "MyTable1" t1
LEFT JOIN "MyTable2" t2
    ON t2."MyColumn1" = t1."MyColumn1"''
);';
-- QUERY END: InsertDNDBTDbObjectRecordQuery

-- QUERY START: CreateIndexQuery
EXECUTE 'CREATE UNIQUE INDEX "IDX_MyTable2_MyIndex1"
    ON "MyTable2" ("MyColumn1", "MyColumn2");';
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
EXECUTE 'CREATE INDEX "IDX_MyTable5_CustomName"
    ON "MyTable5" ("MyColumn8");';
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
    ''1d632285-9914-4c5d-98e6-a618a99bd799'',
    ''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'',
    ''Index'',
    ''IDX_MyTable5_CustomName'',
    NULL
);';
-- QUERY END: InsertDNDBTDbObjectRecordQuery

-- QUERY START: CreateForeignKeyQuery
EXECUTE 'ALTER TABLE "MyTable1"
    ADD CONSTRAINT "FK_MyTable1_MyColumn1_MyTable2_MyColumn1" FOREIGN KEY ("MyColumn1")
        REFERENCES "MyTable2" ("MyColumn1")
        ON UPDATE NO ACTION ON DELETE CASCADE;';
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
EXECUTE 'ALTER TABLE "MyTable6"
    ADD CONSTRAINT "FK_MyTable6_MyTable5_CustomName" FOREIGN KEY ("MyColumn1", "MyColumn2")
        REFERENCES "MyTable5" ("MyColumn2", "MyColumn1")
        ON UPDATE NO ACTION ON DELETE NO ACTION;';
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
    ''ae453b22-d270-41fc-8184-9ac26b7a0569'',
    ''f3064a8c-346a-4b3d-af2c-d967b39841e4'',
    ''ForeignKey'',
    ''FK_MyTable6_MyTable5_CustomName'',
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
-- QUERY END: InsertDNDBTScriptExecutionRecordQuery

-- QUERY START: UpdateDNDBTDbAttributesRecordQuery
EXECUTE 'UPDATE "DNDBTDbAttributes" SET
    "Version" = 1;';
-- QUERY END: UpdateDNDBTDbAttributesRecordQuery

END;
$DNDBTGeneratedScriptTransactionBlock$