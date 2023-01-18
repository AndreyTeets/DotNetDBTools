DO $DNDBTGeneratedScriptTransactionBlock$
BEGIN

-- QUERY START: RenameTypeToTempQuery
EXECUTE 'ALTER TYPE "tp_u_4" RENAME TO "_DNDBTTemp_tp_u_4";';
-- QUERY END: RenameTypeToTempQuery

-- QUERY START: CreateEnumTypeQuery
EXECUTE 'CREATE TYPE "tp_u_4" AS ENUM
(
    ''l1x''
);';
-- QUERY END: CreateEnumTypeQuery

-- QUERY START: AlterTableQuery
EXECUTE 'ALTER TABLE "t_u_4" RENAME COLUMN "c1" TO "c1x";
ALTER TABLE "t_u_4"
    ALTER COLUMN "c1x" SET DATA TYPE "tp_u_4"
        USING ("c1x"::text::"tp_u_4");';
-- QUERY END: AlterTableQuery

-- QUERY START: DropTypeQuery
EXECUTE 'DROP TYPE "_DNDBTTemp_tp_u_4";';
-- QUERY END: DropTypeQuery

END;
$DNDBTGeneratedScriptTransactionBlock$