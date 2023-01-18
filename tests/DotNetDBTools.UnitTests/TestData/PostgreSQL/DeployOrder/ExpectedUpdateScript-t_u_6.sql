DO $DNDBTGeneratedScriptTransactionBlock$
BEGIN

-- QUERY START: AlterTableQuery
EXECUTE 'ALTER TABLE "t_u_6"
    DROP CONSTRAINT "ck_t_u_6";';
-- QUERY END: AlterTableQuery

-- QUERY START: RenameTypeToTempQuery
EXECUTE 'ALTER TYPE "tp_u_6" RENAME TO "_DNDBTTemp_tp_u_6";';
-- QUERY END: RenameTypeToTempQuery

-- QUERY START: CreateCompositeTypeQuery
EXECUTE 'CREATE TYPE "tp_u_6" AS
(
    "a1" BIGINT
);';
-- QUERY END: CreateCompositeTypeQuery

-- QUERY START: AlterTableQuery
EXECUTE 'ALTER TABLE "t_u_6" RENAME TO "t_u_6x";';
-- QUERY END: AlterTableQuery

-- QUERY START: DropTypeQuery
EXECUTE 'DROP TYPE "_DNDBTTemp_tp_u_6";';
-- QUERY END: DropTypeQuery

-- QUERY START: AlterTableQuery
EXECUTE 'ALTER TABLE "t_u_6x"
    ADD CONSTRAINT "ck_t_u_6" CHECK (c1 != (''(5)''::tp_u_6).a1::int);';
-- QUERY END: AlterTableQuery

END;
$DNDBTGeneratedScriptTransactionBlock$