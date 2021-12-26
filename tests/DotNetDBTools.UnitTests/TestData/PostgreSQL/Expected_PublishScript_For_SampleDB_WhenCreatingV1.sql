DO $$
BEGIN

-- QUERY START: PostgreSQLCreateCompositeTypeQuery
EXECUTE 'CREATE TYPE "MyCompositeType1" AS
(
    "MyAttribute1" VARCHAR(100),
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
EXECUTE 'CREATE DOMAIN "MyDomain1" AS VARCHAR(100)    NULL
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
    ''Label2''
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
    SUBTYPE = TIMESTAMP,
    SUBTYPE_OPCLASS = "timestamp_ops",
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

-- QUERY START: PostgreSQLCreateTableQuery
EXECUTE 'CREATE TABLE "MyTable1"
(
    "MyColumn1" INT NOT NULL DEFAULT 15,
    "MyColumn2" VARCHAR(10) NOT NULL DEFAULT ''33'',
    "MyColumn3" INT GENERATED ALWAYS AS IDENTITY NOT NULL,
    CONSTRAINT "PK_MyTable1" PRIMARY KEY ("MyColumn3"),
    CONSTRAINT "UQ_MyTable1_MyColumn2" UNIQUE ("MyColumn2")
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
    ''299675e6-4faa-4d0f-a36a-224306ba5bcb'',
    NULL,
    ''Table'',
    ''MyTable1'',
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
    ''a2f2a4de-1337-4594-ae41-72ed4d05f317'',
    ''299675e6-4faa-4d0f-a36a-224306ba5bcb'',
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
    ''fe68ee3d-09d0-40ac-93f9-5e441fbb4f70'',
    ''299675e6-4faa-4d0f-a36a-224306ba5bcb'',
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
    ''6e95de30-e01a-4fb4-b8b7-8f0c40bb682c'',
    ''299675e6-4faa-4d0f-a36a-224306ba5bcb'',
    ''Column'',
    ''MyColumn3'',
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
    ''37a45def-f4a0-4be7-8bfb-8fbed4a7d705'',
    ''299675e6-4faa-4d0f-a36a-224306ba5bcb'',
    ''PrimaryKey'',
    ''PK_MyTable1'',
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
    ''f3f08522-26ee-4950-9135-22edf2e4e0cf'',
    ''299675e6-4faa-4d0f-a36a-224306ba5bcb'',
    ''UniqueConstraint'',
    ''UQ_MyTable1_MyColumn2'',
    NULL
);';
-- QUERY END: PostgreSQLInsertDNDBTSysInfoQuery

-- QUERY START: PostgreSQLCreateTableQuery
EXECUTE 'CREATE TABLE "MyTable2"
(
    "MyColumn1" INT NOT NULL DEFAULT 333,
    "MyColumn2" BYTEA NULL DEFAULT ''\x000102'',
    CONSTRAINT "PK_MyTable2" PRIMARY KEY ("MyColumn1")
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
    ''bfb9030c-a8c3-4882-9c42-1c6ad025cf8f'',
    NULL,
    ''Table'',
    ''MyTable2'',
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
    ''c480f22f-7c01-4f41-b282-35e9f5cd1fe3'',
    ''bfb9030c-a8c3-4882-9c42-1c6ad025cf8f'',
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
    ''5a0d1926-3270-4eb2-92eb-00be56c7af23'',
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

-- QUERY START: PostgreSQLCreateTableQuery
EXECUTE 'CREATE TABLE "MyTable5"
(
    "MyColumn1" INT NOT NULL DEFAULT ABS(-15),
    "MyColumn2" "MyCompositeType1" NOT NULL,
    "MyColumn3" TIMESTAMPTZ(6) NULL,
    "MyColumn4" "MyDomain1" NOT NULL,
    "MyColumn5" "MyEnumType1" NOT NULL,
    "MyColumn6" "MyRangeType1" NOT NULL
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
    ''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'',
    NULL,
    ''Table'',
    ''MyTable5'',
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
    ''5309d66f-2030-402e-912e-5547babaa072'',
    ''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'',
    ''Column'',
    ''MyColumn1'',
    ''ABS(-15)''
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
    ''15ae6061-426d-4485-85e6-ecd3e0f98882'',
    ''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'',
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
    ''4dde852d-ec19-4b61-80f9-da428d8ff41a'',
    ''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'',
    ''Column'',
    ''MyColumn3'',
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
    ''45856161-db66-49f6-afde-9214d2d2d4b0'',
    ''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'',
    ''Column'',
    ''MyColumn4'',
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
    ''b45d163b-f49e-499f-a9e5-2538cd073b80'',
    ''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'',
    ''Column'',
    ''MyColumn5'',
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
    ''c8b03b75-a8a2-47e0-bf5c-f3e4f1b8f500'',
    ''6ca51f29-c1bc-4349-b9c1-6f1ea170f162'',
    ''Column'',
    ''MyColumn6'',
    NULL
);';
-- QUERY END: PostgreSQLInsertDNDBTSysInfoQuery

-- QUERY START: PostgreSQLCreateForeignKeyQuery
EXECUTE 'ALTER TABLE "MyTable1" ADD CONSTRAINT "FK_MyTable1_MyColumn1_MyTable2_MyColumn1" FOREIGN KEY ("MyColumn1")
    REFERENCES "MyTable2" ("MyColumn1")
    ON UPDATE NO ACTION ON DELETE CASCADE;';
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

END;
$$