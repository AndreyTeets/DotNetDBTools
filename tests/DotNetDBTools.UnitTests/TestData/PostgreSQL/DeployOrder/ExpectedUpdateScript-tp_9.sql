DO $DNDBTGeneratedScriptTransactionBlock$
BEGIN

-- QUERY START: AlterTableQuery
EXECUTE 'ALTER TABLE "t_b_7"
    DROP CONSTRAINT "ck_t_b_7";';
-- QUERY END: AlterTableQuery

-- QUERY START: AlterTableQuery
EXECUTE 'ALTER TABLE "t_a_7"
    ALTER COLUMN "c1" DROP DEFAULT;';
-- QUERY END: AlterTableQuery

-- QUERY START: AlterDomainTypeQuery
EXECUTE 'ALTER DOMAIN "d_b_7" DROP CONSTRAINT "ck_d_b_7";';
-- QUERY END: AlterDomainTypeQuery

-- QUERY START: AlterDomainTypeQuery
EXECUTE 'ALTER DOMAIN "d_a_7" DROP DEFAULT;';
-- QUERY END: AlterDomainTypeQuery

-- QUERY START: DropFunctionQuery
EXECUTE 'DROP FUNCTION "f_a_2_s";';
-- QUERY END: DropFunctionQuery

-- QUERY START: DropProcedureQuery
EXECUTE 'DROP PROCEDURE "p_a_2_s";';
-- QUERY END: DropProcedureQuery

-- QUERY START: DropViewQuery
EXECUTE 'DROP VIEW "v_a_7";';
-- QUERY END: DropViewQuery

-- QUERY START: RenameTypeToTempQuery
EXECUTE 'ALTER TYPE "tp_7" RENAME TO "_DNDBTTemp_tp_7";';
-- QUERY END: RenameTypeToTempQuery

-- QUERY START: RenameTypeToTempQuery
EXECUTE 'ALTER TYPE "tp_9" RENAME TO "_DNDBTTemp_tp_9";';
-- QUERY END: RenameTypeToTempQuery

-- QUERY START: CreateCompositeTypeQuery
EXECUTE 'CREATE TYPE "tp_9" AS
(
    "a1" BIGINT
);';
-- QUERY END: CreateCompositeTypeQuery

-- QUERY START: CreateCompositeTypeQuery
EXECUTE 'CREATE TYPE "tp_7" AS
(
    "a1" "tp_9"
);';
-- QUERY END: CreateCompositeTypeQuery

-- QUERY START: AlterTableQuery
EXECUTE 'ALTER TABLE "t_c_2"
    ALTER COLUMN "c1" SET DATA TYPE "tp_9"
        USING ("c1"::text::"tp_9");';
-- QUERY END: AlterTableQuery

-- QUERY START: DropTypeQuery
EXECUTE 'DROP TYPE "_DNDBTTemp_tp_7";';
-- QUERY END: DropTypeQuery

-- QUERY START: DropTypeQuery
EXECUTE 'DROP TYPE "_DNDBTTemp_tp_9";';
-- QUERY END: DropTypeQuery

-- QUERY START: CreateFunctionQuery
EXECUTE 'create function f_a_2_s() returns int language sql as $$select (''(5)''::tp_9).a1::int$$';
-- QUERY END: CreateFunctionQuery

-- QUERY START: CreateProcedureQuery
EXECUTE 'create procedure p_a_2_s() language sql as $$select (''(5)''::tp_9).a1::int$$';
-- QUERY END: CreateProcedureQuery

-- QUERY START: CreateViewQuery
EXECUTE 'create view v_a_7 as select (''(5)''::tp_9).a1::int';
-- QUERY END: CreateViewQuery

-- QUERY START: AlterDomainTypeQuery
EXECUTE 'ALTER DOMAIN "d_a_7" SET DEFAULT (''(5)''::tp_9).a1::int;';
-- QUERY END: AlterDomainTypeQuery

-- QUERY START: AlterDomainTypeQuery
EXECUTE 'ALTER DOMAIN "d_b_7" ADD CONSTRAINT "ck_d_b_7" CHECK (value != (''(5)''::tp_9).a1::int);';
-- QUERY END: AlterDomainTypeQuery

-- QUERY START: AlterTableQuery
EXECUTE 'ALTER TABLE "t_a_7"
    ALTER COLUMN "c1" SET DEFAULT (''(5)''::tp_9).a1::int;';
-- QUERY END: AlterTableQuery

-- QUERY START: AlterTableQuery
EXECUTE 'ALTER TABLE "t_b_7"
    ADD CONSTRAINT "ck_t_b_7" CHECK (c1 != (''(5)''::tp_9).a1::int);';
-- QUERY END: AlterTableQuery

END;
$DNDBTGeneratedScriptTransactionBlock$