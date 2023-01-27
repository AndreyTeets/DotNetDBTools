DO $DNDBTGeneratedScriptTransactionBlock$
BEGIN

-- QUERY START: RenameProgrammableObjectToTempQuery
EXECUTE 'ALTER FUNCTION "f_12_s" RENAME TO "_DNDBTTemp_f_12_s";';
-- QUERY END: RenameProgrammableObjectToTempQuery

-- QUERY START: RenameProgrammableObjectToTempQuery
EXECUTE 'ALTER PROCEDURE "p_9_p" RENAME TO "_DNDBTTemp_p_9_p";';
-- QUERY END: RenameProgrammableObjectToTempQuery

-- QUERY START: CreateProcedureQuery
EXECUTE 'create procedure p_9_p() language plpgsql as $$begin perform (select (66 + 88)); end$$';
-- QUERY END: CreateProcedureQuery

-- QUERY START: CreateFunctionQuery
EXECUTE 'create function f_12_s() returns int language sql as $$call p_9_p(); select 2$$';
-- QUERY END: CreateFunctionQuery

-- QUERY START: DropFunctionQuery
EXECUTE 'DROP FUNCTION "_DNDBTTemp_f_12_s";';
-- QUERY END: DropFunctionQuery

-- QUERY START: DropProcedureQuery
EXECUTE 'DROP PROCEDURE "_DNDBTTemp_p_9_p";';
-- QUERY END: DropProcedureQuery

END;
$DNDBTGeneratedScriptTransactionBlock$