DO $DNDBTGeneratedScriptTransactionBlock$
BEGIN

-- QUERY START: AlterTableQuery
EXECUTE 'ALTER TABLE "t_b_4"
    DROP CONSTRAINT "ck_t_b_4";';
-- QUERY END: AlterTableQuery

-- QUERY START: AlterTableQuery
EXECUTE 'ALTER TABLE "t_a_4"
    ALTER COLUMN "c1" DROP DEFAULT;';
-- QUERY END: AlterTableQuery

-- QUERY START: AlterDomainTypeQuery
EXECUTE 'ALTER DOMAIN "d_b_4" DROP CONSTRAINT "ck_d_b_4";';
-- QUERY END: AlterDomainTypeQuery

-- QUERY START: AlterDomainTypeQuery
EXECUTE 'ALTER DOMAIN "d_a_4" DROP DEFAULT;';
-- QUERY END: AlterDomainTypeQuery

-- QUERY START: RenameProgrammableObjectToTempQuery
EXECUTE 'ALTER VIEW "v_a_4" RENAME TO "_DNDBTTemp_v_a_4";';
-- QUERY END: RenameProgrammableObjectToTempQuery

-- QUERY START: RenameProgrammableObjectToTempQuery
EXECUTE 'ALTER FUNCTION "f_9_p" RENAME TO "_DNDBTTemp_f_9_p";';
-- QUERY END: RenameProgrammableObjectToTempQuery

-- QUERY START: CreateFunctionQuery
EXECUTE 'create function f_9_p() returns int language plpgsql as $$begin return (select (6 + 8)); end$$';
-- QUERY END: CreateFunctionQuery

-- QUERY START: CreateViewQuery
EXECUTE 'create view v_a_4 as select (1000 + f_9_p())';
-- QUERY END: CreateViewQuery

-- QUERY START: DropViewQuery
EXECUTE 'DROP VIEW "_DNDBTTemp_v_a_4";';
-- QUERY END: DropViewQuery

-- QUERY START: DropFunctionQuery
EXECUTE 'DROP FUNCTION "_DNDBTTemp_f_9_p";';
-- QUERY END: DropFunctionQuery

-- QUERY START: AlterDomainTypeQuery
EXECUTE 'ALTER DOMAIN "d_a_4" SET DEFAULT (1000 + f_9_p());';
-- QUERY END: AlterDomainTypeQuery

-- QUERY START: AlterDomainTypeQuery
EXECUTE 'ALTER DOMAIN "d_b_4" ADD CONSTRAINT "ck_d_b_4" CHECK (value != (1000 + f_9_p()));';
-- QUERY END: AlterDomainTypeQuery

-- QUERY START: AlterTableQuery
EXECUTE 'ALTER TABLE "t_a_4"
    ALTER COLUMN "c1" SET DEFAULT (1000 + f_9_p());';
-- QUERY END: AlterTableQuery

-- QUERY START: AlterTableQuery
EXECUTE 'ALTER TABLE "t_b_4"
    ADD CONSTRAINT "ck_t_b_4" CHECK (c1 != (1000 + f_9_p()));';
-- QUERY END: AlterTableQuery

END;
$DNDBTGeneratedScriptTransactionBlock$