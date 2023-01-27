DO $DNDBTGeneratedScriptTransactionBlock$
BEGIN

-- QUERY START: RenameProgrammableObjectToTempQuery
EXECUTE 'ALTER FUNCTION "f_10_s" RENAME TO "_DNDBTTemp_f_10_s";';
-- QUERY END: RenameProgrammableObjectToTempQuery

-- QUERY START: RenameProgrammableObjectToTempQuery
EXECUTE 'ALTER PROCEDURE "p_2_s" RENAME TO "_DNDBTTemp_p_2_s";';
-- QUERY END: RenameProgrammableObjectToTempQuery

-- QUERY START: RenameProgrammableObjectToTempQuery
EXECUTE 'ALTER PROCEDURE "p_3_s" RENAME TO "_DNDBTTemp_p_3_s";';
-- QUERY END: RenameProgrammableObjectToTempQuery

-- QUERY START: CreateProcedureQuery
EXECUTE 'create procedure p_3_s() language sql as $$select (33 + 44)$$';
-- QUERY END: CreateProcedureQuery

-- QUERY START: CreateFunctionQuery
EXECUTE 'create function f_10_s() returns int language sql as $$call p_3_s(); select 2$$';
-- QUERY END: CreateFunctionQuery

-- QUERY START: CreateProcedureQuery
EXECUTE 'create procedure p_2_s() language sql as $$call p_3_s()$$';
-- QUERY END: CreateProcedureQuery

-- QUERY START: DropFunctionQuery
EXECUTE 'DROP FUNCTION "_DNDBTTemp_f_10_s";';
-- QUERY END: DropFunctionQuery

-- QUERY START: DropProcedureQuery
EXECUTE 'DROP PROCEDURE "_DNDBTTemp_p_2_s";';
-- QUERY END: DropProcedureQuery

-- QUERY START: DropProcedureQuery
EXECUTE 'DROP PROCEDURE "_DNDBTTemp_p_3_s";';
-- QUERY END: DropProcedureQuery

END;
$DNDBTGeneratedScriptTransactionBlock$