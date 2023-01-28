DO $DNDBTGeneratedScriptTransactionBlock$
BEGIN

-- QUERY START: DropIndexQuery
EXECUTE 'DROP INDEX "i_a_1";';
-- QUERY END: DropIndexQuery

-- QUERY START: RenameProgrammableObjectToTempQuery
EXECUTE 'ALTER FUNCTION "f_4_s" RENAME TO "_DNDBTTemp_f_4_s";';
-- QUERY END: RenameProgrammableObjectToTempQuery

-- QUERY START: RenameProgrammableObjectToTempQuery
EXECUTE 'ALTER FUNCTION "f_7_s" RENAME TO "_DNDBTTemp_f_7_s";';
-- QUERY END: RenameProgrammableObjectToTempQuery

-- QUERY START: CreateFunctionQuery
EXECUTE 'create function f_7_s() returns int language sql as $$select 112::int$$';
-- QUERY END: CreateFunctionQuery

-- QUERY START: CreateFunctionQuery
EXECUTE 'create function f_4_s(x int) returns int language sql immutable as $$select x + f_7_s()$$';
-- QUERY END: CreateFunctionQuery

-- QUERY START: DropFunctionQuery
EXECUTE 'DROP FUNCTION "_DNDBTTemp_f_4_s";';
-- QUERY END: DropFunctionQuery

-- QUERY START: DropFunctionQuery
EXECUTE 'DROP FUNCTION "_DNDBTTemp_f_7_s";';
-- QUERY END: DropFunctionQuery

-- QUERY START: CreateIndexQuery
EXECUTE 'CREATE INDEX "i_a_1"
    ON "t_1" USING BTREE (f_4_s(c1));';
-- QUERY END: CreateIndexQuery

END;
$DNDBTGeneratedScriptTransactionBlock$