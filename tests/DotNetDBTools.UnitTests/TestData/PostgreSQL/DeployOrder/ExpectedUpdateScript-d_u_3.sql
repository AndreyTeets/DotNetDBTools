DO $DNDBTGeneratedScriptTransactionBlock$
BEGIN

-- QUERY START: AlterDomainTypeQuery
EXECUTE 'ALTER DOMAIN "d_u_3" DROP CONSTRAINT "ck_d_u_2";';
-- QUERY END: AlterDomainTypeQuery

-- QUERY START: RenameTypeToTempQuery
EXECUTE 'ALTER TYPE "tp_u_3" RENAME TO "_DNDBTTemp_tp_u_3";';
-- QUERY END: RenameTypeToTempQuery

-- QUERY START: AlterDomainTypeQuery
EXECUTE 'ALTER DOMAIN "d_u_3" RENAME TO "d_u_3x";';
-- QUERY END: AlterDomainTypeQuery

-- QUERY START: CreateCompositeTypeQuery
EXECUTE 'CREATE TYPE "tp_u_3" AS
(
    "a1" BIGINT
);';
-- QUERY END: CreateCompositeTypeQuery

-- QUERY START: DropTypeQuery
EXECUTE 'DROP TYPE "_DNDBTTemp_tp_u_3";';
-- QUERY END: DropTypeQuery

-- QUERY START: AlterDomainTypeQuery
EXECUTE 'ALTER DOMAIN "d_u_3x" ADD CONSTRAINT "ck_d_u_2" CHECK (value != (''(5)''::tp_u_3).a1::int);';
-- QUERY END: AlterDomainTypeQuery

END;
$DNDBTGeneratedScriptTransactionBlock$