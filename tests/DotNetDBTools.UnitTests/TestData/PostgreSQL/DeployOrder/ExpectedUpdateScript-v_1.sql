DO $DNDBTGeneratedScriptTransactionBlock$
BEGIN

-- QUERY START: RenameProgrammableObjectToTempQuery
EXECUTE 'ALTER FUNCTION "f_a_4_s" RENAME TO "_DNDBTTemp_f_a_4_s";';
-- QUERY END: RenameProgrammableObjectToTempQuery

-- QUERY START: RenameProgrammableObjectToTempQuery
EXECUTE 'ALTER PROCEDURE "p_a_4_s" RENAME TO "_DNDBTTemp_p_a_4_s";';
-- QUERY END: RenameProgrammableObjectToTempQuery

-- QUERY START: RenameProgrammableObjectToTempQuery
EXECUTE 'ALTER VIEW "v_a_9" RENAME TO "_DNDBTTemp_v_a_9";';
-- QUERY END: RenameProgrammableObjectToTempQuery

-- QUERY START: RenameProgrammableObjectToTempQuery
EXECUTE 'ALTER VIEW "v_1" RENAME TO "_DNDBTTemp_v_1";';
-- QUERY END: RenameProgrammableObjectToTempQuery

-- QUERY START: CreateViewQuery
EXECUTE 'create view v_1 as select (5 + 9) as c1';
-- QUERY END: CreateViewQuery

-- QUERY START: CreateFunctionQuery
EXECUTE 'create function f_a_4_s() returns int language sql as $$select c1 from v_1$$';
-- QUERY END: CreateFunctionQuery

-- QUERY START: CreateProcedureQuery
EXECUTE 'create procedure p_a_4_s() language sql as $$select c1 from v_1$$';
-- QUERY END: CreateProcedureQuery

-- QUERY START: CreateViewQuery
EXECUTE 'create view v_a_9 as select c1 from v_1';
-- QUERY END: CreateViewQuery

-- QUERY START: DropFunctionQuery
EXECUTE 'DROP FUNCTION "_DNDBTTemp_f_a_4_s";';
-- QUERY END: DropFunctionQuery

-- QUERY START: DropProcedureQuery
EXECUTE 'DROP PROCEDURE "_DNDBTTemp_p_a_4_s";';
-- QUERY END: DropProcedureQuery

-- QUERY START: DropViewQuery
EXECUTE 'DROP VIEW "_DNDBTTemp_v_a_9";';
-- QUERY END: DropViewQuery

-- QUERY START: DropViewQuery
EXECUTE 'DROP VIEW "_DNDBTTemp_v_1";';
-- QUERY END: DropViewQuery

END;
$DNDBTGeneratedScriptTransactionBlock$