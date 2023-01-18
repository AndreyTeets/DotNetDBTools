DO $DNDBTGeneratedScriptTransactionBlock$
BEGIN

-- QUERY START: AlterTableQuery
EXECUTE 'ALTER TABLE "t_u_5"
    ALTER COLUMN "c1" DROP DEFAULT;';
-- QUERY END: AlterTableQuery

-- QUERY START: RenameTypeToTempQuery
EXECUTE 'ALTER TYPE "tp_u_5" RENAME TO "_DNDBTTemp_tp_u_5";';
-- QUERY END: RenameTypeToTempQuery

-- QUERY START: CreateCompositeTypeQuery
EXECUTE 'CREATE TYPE "tp_u_5" AS
(
    "a1" BIGINT
);';
-- QUERY END: CreateCompositeTypeQuery

-- QUERY START: AlterTableQuery
EXECUTE 'ALTER TABLE "t_u_5" RENAME COLUMN "c1" TO "c1x";';
-- QUERY END: AlterTableQuery

-- QUERY START: DropTypeQuery
EXECUTE 'DROP TYPE "_DNDBTTemp_tp_u_5";';
-- QUERY END: DropTypeQuery

-- QUERY START: AlterTableQuery
EXECUTE 'ALTER TABLE "t_u_5"
    ALTER COLUMN "c1x" SET DEFAULT (''(5)''::tp_u_5).a1::int;';
-- QUERY END: AlterTableQuery

END;
$DNDBTGeneratedScriptTransactionBlock$