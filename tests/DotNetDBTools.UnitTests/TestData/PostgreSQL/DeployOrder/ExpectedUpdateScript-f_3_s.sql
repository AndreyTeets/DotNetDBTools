DO $DNDBTGeneratedScriptTransactionBlock$
BEGIN

-- QUERY START: AlterTableQuery
EXECUTE 'ALTER TABLE "t_b_2"
    DROP CONSTRAINT "ck_t_b_2";';
-- QUERY END: AlterTableQuery

-- QUERY START: AlterTableQuery
EXECUTE 'ALTER TABLE "t_b_3"
    DROP CONSTRAINT "ck_t_b_3";';
-- QUERY END: AlterTableQuery

-- QUERY START: AlterTableQuery
EXECUTE 'ALTER TABLE "t_a_2"
    ALTER COLUMN "c1" DROP DEFAULT;';
-- QUERY END: AlterTableQuery

-- QUERY START: AlterTableQuery
EXECUTE 'ALTER TABLE "t_a_3"
    ALTER COLUMN "c1" DROP DEFAULT;';
-- QUERY END: AlterTableQuery

-- QUERY START: AlterDomainTypeQuery
EXECUTE 'ALTER DOMAIN "d_b_2" DROP CONSTRAINT "ck_d_b_2";';
-- QUERY END: AlterDomainTypeQuery

-- QUERY START: AlterDomainTypeQuery
EXECUTE 'ALTER DOMAIN "d_b_3" DROP CONSTRAINT "ck_d_b_3";';
-- QUERY END: AlterDomainTypeQuery

-- QUERY START: AlterDomainTypeQuery
EXECUTE 'ALTER DOMAIN "d_a_2" DROP DEFAULT;';
-- QUERY END: AlterDomainTypeQuery

-- QUERY START: AlterDomainTypeQuery
EXECUTE 'ALTER DOMAIN "d_a_3" DROP DEFAULT;';
-- QUERY END: AlterDomainTypeQuery

-- QUERY START: RenameProgrammableObjectToTempQuery
EXECUTE 'ALTER PROCEDURE "p_10_s" RENAME TO "_DNDBTTemp_p_10_s";';
-- QUERY END: RenameProgrammableObjectToTempQuery

-- QUERY START: RenameProgrammableObjectToTempQuery
EXECUTE 'ALTER VIEW "v_a_2" RENAME TO "_DNDBTTemp_v_a_2";';
-- QUERY END: RenameProgrammableObjectToTempQuery

-- QUERY START: RenameProgrammableObjectToTempQuery
EXECUTE 'ALTER VIEW "v_a_3" RENAME TO "_DNDBTTemp_v_a_3";';
-- QUERY END: RenameProgrammableObjectToTempQuery

-- QUERY START: RenameProgrammableObjectToTempQuery
EXECUTE 'ALTER FUNCTION "f_2_s" RENAME TO "_DNDBTTemp_f_2_s";';
-- QUERY END: RenameProgrammableObjectToTempQuery

-- QUERY START: RenameProgrammableObjectToTempQuery
EXECUTE 'ALTER FUNCTION "f_3_s" RENAME TO "_DNDBTTemp_f_3_s";';
-- QUERY END: RenameProgrammableObjectToTempQuery

-- QUERY START: CreateFunctionQuery
EXECUTE 'create function f_3_s() returns int language sql as $$select (3 + 4)$$';
-- QUERY END: CreateFunctionQuery

-- QUERY START: CreateFunctionQuery
EXECUTE 'create function f_2_s(x int) returns int language sql as $$select x + f_3_s()$$';
-- QUERY END: CreateFunctionQuery

-- QUERY START: CreateProcedureQuery
EXECUTE 'create procedure p_10_s() language sql as $$select f_3_s()$$';
-- QUERY END: CreateProcedureQuery

-- QUERY START: CreateViewQuery
EXECUTE 'create view v_a_2 as select (1000 + f_3_s())';
-- QUERY END: CreateViewQuery

-- QUERY START: CreateViewQuery
EXECUTE 'create view v_a_3 as select (1000 + f_2_s(8))';
-- QUERY END: CreateViewQuery

-- QUERY START: DropProcedureQuery
EXECUTE 'DROP PROCEDURE "_DNDBTTemp_p_10_s";';
-- QUERY END: DropProcedureQuery

-- QUERY START: DropViewQuery
EXECUTE 'DROP VIEW "_DNDBTTemp_v_a_2";';
-- QUERY END: DropViewQuery

-- QUERY START: DropViewQuery
EXECUTE 'DROP VIEW "_DNDBTTemp_v_a_3";';
-- QUERY END: DropViewQuery

-- QUERY START: DropFunctionQuery
EXECUTE 'DROP FUNCTION "_DNDBTTemp_f_2_s";';
-- QUERY END: DropFunctionQuery

-- QUERY START: DropFunctionQuery
EXECUTE 'DROP FUNCTION "_DNDBTTemp_f_3_s";';
-- QUERY END: DropFunctionQuery

-- QUERY START: AlterDomainTypeQuery
EXECUTE 'ALTER DOMAIN "d_a_2" SET DEFAULT (1000 + f_3_s());';
-- QUERY END: AlterDomainTypeQuery

-- QUERY START: AlterDomainTypeQuery
EXECUTE 'ALTER DOMAIN "d_a_3" SET DEFAULT (1000 + f_2_s(8));';
-- QUERY END: AlterDomainTypeQuery

-- QUERY START: AlterDomainTypeQuery
EXECUTE 'ALTER DOMAIN "d_b_2" ADD CONSTRAINT "ck_d_b_2" CHECK (value != (1000 + f_3_s()));';
-- QUERY END: AlterDomainTypeQuery

-- QUERY START: AlterDomainTypeQuery
EXECUTE 'ALTER DOMAIN "d_b_3" ADD CONSTRAINT "ck_d_b_3" CHECK (value != (1000 + f_2_s(8)));';
-- QUERY END: AlterDomainTypeQuery

-- QUERY START: AlterTableQuery
EXECUTE 'ALTER TABLE "t_a_2"
    ALTER COLUMN "c1" SET DEFAULT (1000 + f_3_s());';
-- QUERY END: AlterTableQuery

-- QUERY START: AlterTableQuery
EXECUTE 'ALTER TABLE "t_a_3"
    ALTER COLUMN "c1" SET DEFAULT (1000 + f_2_s(8));';
-- QUERY END: AlterTableQuery

-- QUERY START: AlterTableQuery
EXECUTE 'ALTER TABLE "t_b_2"
    ADD CONSTRAINT "ck_t_b_2" CHECK (c1 != (1000 + f_3_s()));';
-- QUERY END: AlterTableQuery

-- QUERY START: AlterTableQuery
EXECUTE 'ALTER TABLE "t_b_3"
    ADD CONSTRAINT "ck_t_b_3" CHECK (c1 != (1000 + f_2_s(8)));';
-- QUERY END: AlterTableQuery

END;
$DNDBTGeneratedScriptTransactionBlock$