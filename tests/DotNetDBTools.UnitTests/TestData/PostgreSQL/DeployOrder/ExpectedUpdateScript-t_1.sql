DO $DNDBTGeneratedScriptTransactionBlock$
BEGIN

-- QUERY START: DropFunctionQuery
EXECUTE 'DROP FUNCTION "f_a_3_s";';
-- QUERY END: DropFunctionQuery

-- QUERY START: DropProcedureQuery
EXECUTE 'DROP PROCEDURE "p_a_3_s";';
-- QUERY END: DropProcedureQuery

-- QUERY START: DropViewQuery
EXECUTE 'DROP VIEW "v_a_8";';
-- QUERY END: DropViewQuery

-- QUERY START: AlterTableQuery
EXECUTE 'ALTER TABLE "t_1"
    ALTER COLUMN "c1" SET DATA TYPE SMALLINT
        USING ("c1"::text::SMALLINT);';
-- QUERY END: AlterTableQuery

-- QUERY START: CreateFunctionQuery
EXECUTE 'create function f_a_3_s() returns int language sql as $$select c1::int from t_1$$';
-- QUERY END: CreateFunctionQuery

-- QUERY START: CreateProcedureQuery
EXECUTE 'create procedure p_a_3_s() language sql as $$select c1::int from t_1$$';
-- QUERY END: CreateProcedureQuery

-- QUERY START: CreateViewQuery
EXECUTE 'create view v_a_8 as select c1 from t_1';
-- QUERY END: CreateViewQuery

END;
$DNDBTGeneratedScriptTransactionBlock$