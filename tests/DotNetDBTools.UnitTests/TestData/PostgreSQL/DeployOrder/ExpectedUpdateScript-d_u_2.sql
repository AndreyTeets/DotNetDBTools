DO $DNDBTGeneratedScriptTransactionBlock$
BEGIN

-- QUERY START: AlterDomainTypeQuery
EXECUTE 'ALTER DOMAIN "d_u_2" DROP DEFAULT;';
-- QUERY END: AlterDomainTypeQuery

-- QUERY START: RenameTypeToTempQuery
EXECUTE 'ALTER TYPE "tp_u_2" RENAME TO "_DNDBTTemp_tp_u_2";';
-- QUERY END: RenameTypeToTempQuery

-- QUERY START: AlterDomainTypeQuery
EXECUTE 'ALTER DOMAIN "d_u_2" RENAME TO "d_u_2x";';
-- QUERY END: AlterDomainTypeQuery

-- QUERY START: CreateCompositeTypeQuery
EXECUTE 'CREATE TYPE "tp_u_2" AS
(
    "a1" BIGINT
);';
-- QUERY END: CreateCompositeTypeQuery

-- QUERY START: DropTypeQuery
EXECUTE 'DROP TYPE "_DNDBTTemp_tp_u_2";';
-- QUERY END: DropTypeQuery

-- QUERY START: AlterDomainTypeQuery
EXECUTE 'ALTER DOMAIN "d_u_2x" SET DEFAULT (''(5)''::tp_u_2).a1::int;';
-- QUERY END: AlterDomainTypeQuery

END;
$DNDBTGeneratedScriptTransactionBlock$