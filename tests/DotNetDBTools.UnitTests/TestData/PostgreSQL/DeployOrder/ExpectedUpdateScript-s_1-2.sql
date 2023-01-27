DO $DNDBTGeneratedScriptTransactionBlock$
BEGIN

-- QUERY START: DropIndexQuery
EXECUTE 'DROP INDEX "i_a_1";';
-- QUERY END: DropIndexQuery

-- QUERY START: AlterTableQuery
EXECUTE 'ALTER TABLE "t_b_1"
    DROP CONSTRAINT "ck_t_b_1";';
-- QUERY END: AlterTableQuery

-- QUERY START: AlterTableQuery
EXECUTE 'ALTER TABLE "t_a_1"
    ALTER COLUMN "c1" DROP DEFAULT;';
-- QUERY END: AlterTableQuery

-- QUERY START: AlterDomainTypeQuery
EXECUTE 'ALTER DOMAIN "d_b_1" DROP CONSTRAINT "ck_d_b_1";';
-- QUERY END: AlterDomainTypeQuery

-- QUERY START: AlterDomainTypeQuery
EXECUTE 'ALTER DOMAIN "d_a_1" DROP DEFAULT;';
-- QUERY END: AlterDomainTypeQuery

-- QUERY START: AlterSequenceQuery
EXECUTE 'ALTER SEQUENCE "s_1" RENAME TO "_DNDBTTemp_s_1";';
-- QUERY END: AlterSequenceQuery

-- QUERY START: RenameProgrammableObjectToTempQuery
EXECUTE 'ALTER FUNCTION "f_4_s" RENAME TO "_DNDBTTemp_f_4_s";';
-- QUERY END: RenameProgrammableObjectToTempQuery

-- QUERY START: RenameProgrammableObjectToTempQuery
EXECUTE 'ALTER PROCEDURE "p_7_s" RENAME TO "_DNDBTTemp_p_7_s";';
-- QUERY END: RenameProgrammableObjectToTempQuery

-- QUERY START: RenameProgrammableObjectToTempQuery
EXECUTE 'ALTER VIEW "v_a_1" RENAME TO "_DNDBTTemp_v_a_1";';
-- QUERY END: RenameProgrammableObjectToTempQuery

-- QUERY START: RenameProgrammableObjectToTempQuery
EXECUTE 'ALTER FUNCTION "f_7_s" RENAME TO "_DNDBTTemp_f_7_s";';
-- QUERY END: RenameProgrammableObjectToTempQuery

-- QUERY START: CreateSequenceQuery
EXECUTE 'CREATE SEQUENCE "s_1"
    AS INT
    START 100 INCREMENT 1 MINVALUE 1 MAXVALUE 2147483647 CACHE 1 NO CYCLE;';
-- QUERY END: CreateSequenceQuery

-- QUERY START: CreateFunctionQuery
EXECUTE 'create function f_7_s() returns int language sql as $$select nextval(''s_1'')::int$$';
-- QUERY END: CreateFunctionQuery

-- QUERY START: CreateFunctionQuery
EXECUTE 'create function f_4_s(x int) returns int language sql immutable as $$select x + f_7_s()$$';
-- QUERY END: CreateFunctionQuery

-- QUERY START: CreateProcedureQuery
EXECUTE 'create procedure p_7_s() language sql as $$select nextval(''s_1'')$$';
-- QUERY END: CreateProcedureQuery

-- QUERY START: CreateViewQuery
EXECUTE 'create view v_a_1 as select (1000 + nextval(''s_1''))';
-- QUERY END: CreateViewQuery

-- QUERY START: DropFunctionQuery
EXECUTE 'DROP FUNCTION "_DNDBTTemp_f_4_s";';
-- QUERY END: DropFunctionQuery

-- QUERY START: DropProcedureQuery
EXECUTE 'DROP PROCEDURE "_DNDBTTemp_p_7_s";';
-- QUERY END: DropProcedureQuery

-- QUERY START: DropViewQuery
EXECUTE 'DROP VIEW "_DNDBTTemp_v_a_1";';
-- QUERY END: DropViewQuery

-- QUERY START: DropFunctionQuery
EXECUTE 'DROP FUNCTION "_DNDBTTemp_f_7_s";';
-- QUERY END: DropFunctionQuery

-- QUERY START: DropSequenceQuery
EXECUTE 'DROP SEQUENCE "_DNDBTTemp_s_1";';
-- QUERY END: DropSequenceQuery

-- QUERY START: AlterDomainTypeQuery
EXECUTE 'ALTER DOMAIN "d_a_1" SET DEFAULT (1000 + nextval(''s_1''));';
-- QUERY END: AlterDomainTypeQuery

-- QUERY START: AlterDomainTypeQuery
EXECUTE 'ALTER DOMAIN "d_b_1" ADD CONSTRAINT "ck_d_b_1" CHECK (value != (1000 + nextval(''s_1'')));';
-- QUERY END: AlterDomainTypeQuery

-- QUERY START: AlterTableQuery
EXECUTE 'ALTER TABLE "t_a_1"
    ALTER COLUMN "c1" SET DEFAULT (1000 + nextval(''s_1''));';
-- QUERY END: AlterTableQuery

-- QUERY START: AlterTableQuery
EXECUTE 'ALTER TABLE "t_b_1"
    ADD CONSTRAINT "ck_t_b_1" CHECK (c1 != (1000 + nextval(''s_1'')));';
-- QUERY END: AlterTableQuery

-- QUERY START: CreateIndexQuery
EXECUTE 'CREATE INDEX "i_a_1"
    ON "t_1" (f_4_s(c1));';
-- QUERY END: CreateIndexQuery

END;
$DNDBTGeneratedScriptTransactionBlock$