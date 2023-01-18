DO $DNDBTGeneratedScriptTransactionBlock$
BEGIN

-- QUERY START: DropViewQuery
EXECUTE 'DROP VIEW "v_u_7";';
-- QUERY END: DropViewQuery

-- QUERY START: RenameTypeToTempQuery
EXECUTE 'ALTER TYPE "tp_u_7" RENAME TO "_DNDBTTemp_tp_u_7";';
-- QUERY END: RenameTypeToTempQuery

-- QUERY START: CreateCompositeTypeQuery
EXECUTE 'CREATE TYPE "tp_u_7" AS
(
    "a1" BIGINT
);';
-- QUERY END: CreateCompositeTypeQuery

-- QUERY START: DropTypeQuery
EXECUTE 'DROP TYPE "_DNDBTTemp_tp_u_7";';
-- QUERY END: DropTypeQuery

-- QUERY START: CreateViewQuery
EXECUTE 'create view v_u_7x as select (''(5)''::tp_u_7).a1::int';
-- QUERY END: CreateViewQuery

END;
$DNDBTGeneratedScriptTransactionBlock$