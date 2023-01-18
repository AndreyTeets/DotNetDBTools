DO $DNDBTGeneratedScriptTransactionBlock$
BEGIN

-- QUERY START: RenameTypeToTempQuery
EXECUTE 'ALTER DOMAIN "d_u_1" RENAME TO "_DNDBTTemp_d_u_1";';
-- QUERY END: RenameTypeToTempQuery

-- QUERY START: RenameTypeToTempQuery
EXECUTE 'ALTER TYPE "tp_u_1" RENAME TO "_DNDBTTemp_tp_u_1";';
-- QUERY END: RenameTypeToTempQuery

-- QUERY START: CreateEnumTypeQuery
EXECUTE 'CREATE TYPE "tp_u_1" AS ENUM
(
    ''l1x''
);';
-- QUERY END: CreateEnumTypeQuery

-- QUERY START: CreateDomainTypeQuery
EXECUTE 'CREATE DOMAIN "d_u_1x" AS
    "tp_u_1" NULL;';
-- QUERY END: CreateDomainTypeQuery

-- QUERY START: DropTypeQuery
EXECUTE 'DROP DOMAIN "_DNDBTTemp_d_u_1";';
-- QUERY END: DropTypeQuery

-- QUERY START: DropTypeQuery
EXECUTE 'DROP TYPE "_DNDBTTemp_tp_u_1";';
-- QUERY END: DropTypeQuery

END;
$DNDBTGeneratedScriptTransactionBlock$