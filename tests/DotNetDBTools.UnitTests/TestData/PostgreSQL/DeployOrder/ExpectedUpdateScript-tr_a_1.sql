DO $DNDBTGeneratedScriptTransactionBlock$
BEGIN

-- QUERY START: DropTriggerQuery
EXECUTE 'DROP TRIGGER "tr_a_1" ON "t_1";';
-- QUERY END: DropTriggerQuery

-- QUERY START: RenameProgrammableObjectToTempQuery
EXECUTE 'ALTER FUNCTION "f_5_p" RENAME TO "_DNDBTTemp_f_5_p";';
-- QUERY END: RenameProgrammableObjectToTempQuery

-- QUERY START: CreateFunctionQuery
EXECUTE 'create function f_5_p() returns trigger language plpgsql as $$begin perform (select 118); return NULL; end$$';
-- QUERY END: CreateFunctionQuery

-- QUERY START: DropFunctionQuery
EXECUTE 'DROP FUNCTION "_DNDBTTemp_f_5_p";';
-- QUERY END: DropFunctionQuery

-- QUERY START: CreateTriggerQuery
EXECUTE 'create trigger tr_a_1 after insert on t_1 for each row execute function f_5_p()';
-- QUERY END: CreateTriggerQuery

END;
$DNDBTGeneratedScriptTransactionBlock$