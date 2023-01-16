DO $DNDBTGeneratedScriptTransactionBlock$
BEGIN

-- QUERY START: RenameTypeToTempQuery
EXECUTE 'ALTER DOMAIN "tp_1" RENAME TO "_DNDBTTemp_tp_1";';
-- QUERY END: RenameTypeToTempQuery

-- QUERY START: RenameTypeToTempQuery
EXECUTE 'DO $DNDBTPlPgSqlBlock$
BEGIN
ALTER TYPE "tp_4" RENAME TO "_DNDBTTemp_tp_4";
ALTER FUNCTION "tp_4"("tp_30","tp_30") RENAME TO "_DNDBTTemp_tp_4";
ALTER FUNCTION "tp_4" RENAME TO "_DNDBTTemp_tp_4";
IF (SELECT current_setting(''server_version_num'')::int) >= 140000 THEN
ALTER TYPE "tp_4_multirange" RENAME TO "_DNDBTTemp_tp_4_multirange";
ALTER FUNCTION "tp_4_multirange"() RENAME TO "_DNDBTTemp_tp_4_multirange";
ALTER FUNCTION "tp_4_multirange"("_DNDBTTemp_tp_4") RENAME TO "_DNDBTTemp_tp_4_multirange";
ALTER FUNCTION "tp_4_multirange" RENAME TO "_DNDBTTemp_tp_4_multirange";
END IF;
END;
$DNDBTPlPgSqlBlock$';
-- QUERY END: RenameTypeToTempQuery

-- QUERY START: RenameTypeToTempQuery
EXECUTE 'ALTER TYPE "tp_6" RENAME TO "_DNDBTTemp_tp_6";';
-- QUERY END: RenameTypeToTempQuery

-- QUERY START: RenameTypeToTempQuery
EXECUTE 'ALTER DOMAIN "tp_2" RENAME TO "_DNDBTTemp_tp_2";';
-- QUERY END: RenameTypeToTempQuery

-- QUERY START: RenameTypeToTempQuery
EXECUTE 'ALTER DOMAIN "tp_30" RENAME TO "_DNDBTTemp_tp_30";';
-- QUERY END: RenameTypeToTempQuery

-- QUERY START: CreateDomainTypeQuery
EXECUTE 'CREATE DOMAIN "tp_30" AS
    INT NULL DEFAULT 34
    CONSTRAINT "ck_tp_30" CHECK (value != 39);';
-- QUERY END: CreateDomainTypeQuery

-- QUERY START: CreateDomainTypeQuery
EXECUTE 'CREATE DOMAIN "tp_2" AS
    "tp_30" NULL;';
-- QUERY END: CreateDomainTypeQuery

-- QUERY START: CreateCompositeTypeQuery
EXECUTE 'CREATE TYPE "tp_6" AS
(
    "a1" "tp_2"
);';
-- QUERY END: CreateCompositeTypeQuery

-- QUERY START: CreateDomainTypeQuery
EXECUTE 'CREATE DOMAIN "tp_1" AS
    "tp_6" NULL;';
-- QUERY END: CreateDomainTypeQuery

-- QUERY START: CreateRangeTypeQuery
EXECUTE 'DO $DNDBTPlPgSqlBlock$
BEGIN
IF (SELECT current_setting(''server_version_num'')::int) >= 140000 THEN
CREATE TYPE "tp_4" AS RANGE
(
    SUBTYPE = "tp_30",
    MULTIRANGE_TYPE_NAME = "tp_4_multirange"
);
ELSE
CREATE TYPE "tp_4" AS RANGE
(
    SUBTYPE = "tp_30"
);
END IF;
END;
$DNDBTPlPgSqlBlock$';
-- QUERY END: CreateRangeTypeQuery

-- QUERY START: AlterTableQuery
EXECUTE 'ALTER TABLE "t_c_4"
    ALTER COLUMN "c1" SET DATA TYPE "tp_4"
        USING ("c1"::text::"tp_4");';
-- QUERY END: AlterTableQuery

-- QUERY START: AlterTableQuery
EXECUTE 'ALTER TABLE "t_c_5"
    ALTER COLUMN "c1" SET DATA TYPE "tp_6"
        USING ("c1"::text::"tp_6");';
-- QUERY END: AlterTableQuery

-- QUERY START: DropTypeQuery
EXECUTE 'DROP DOMAIN "_DNDBTTemp_tp_1";';
-- QUERY END: DropTypeQuery

-- QUERY START: DropTypeQuery
EXECUTE 'DROP TYPE "_DNDBTTemp_tp_4";';
-- QUERY END: DropTypeQuery

-- QUERY START: DropTypeQuery
EXECUTE 'DROP TYPE "_DNDBTTemp_tp_6";';
-- QUERY END: DropTypeQuery

-- QUERY START: DropTypeQuery
EXECUTE 'DROP DOMAIN "_DNDBTTemp_tp_2";';
-- QUERY END: DropTypeQuery

-- QUERY START: DropTypeQuery
EXECUTE 'DROP DOMAIN "_DNDBTTemp_tp_30";';
-- QUERY END: DropTypeQuery

END;
$DNDBTGeneratedScriptTransactionBlock$