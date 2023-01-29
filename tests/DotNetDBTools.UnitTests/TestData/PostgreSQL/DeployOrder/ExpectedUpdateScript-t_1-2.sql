DO $DNDBTGeneratedScriptTransactionBlock$
BEGIN

-- QUERY START: DropTriggerQuery
EXECUTE 'DROP TRIGGER "tr_a_1" ON "t_1";';
-- QUERY END: DropTriggerQuery

-- QUERY START: DropIndexQuery
EXECUTE 'DROP INDEX "i_a_1";';
-- QUERY END: DropIndexQuery

-- QUERY START: DropFunctionQuery
EXECUTE 'DROP FUNCTION "f_a_3_s";';
-- QUERY END: DropFunctionQuery

-- QUERY START: DropProcedureQuery
EXECUTE 'DROP PROCEDURE "p_a_3_s";';
-- QUERY END: DropProcedureQuery

-- QUERY START: DropViewQuery
EXECUTE 'DROP VIEW "v_a_8";';
-- QUERY END: DropViewQuery

-- QUERY START: DropTableQuery
EXECUTE 'DROP TABLE "t_1";';
-- QUERY END: DropTableQuery

-- QUERY START: CreateTableQuery
EXECUTE 'CREATE TABLE "t_1"
(
    "c1" INT NULL DEFAULT 4,
    CONSTRAINT "ck_t_1" CHECK (c1 != 6)
);';
-- QUERY END: CreateTableQuery

-- QUERY START: CreateFunctionQuery
EXECUTE 'create function f_a_3_s() returns int language sql as $$select c1::int from t_1$$';
-- QUERY END: CreateFunctionQuery

-- QUERY START: CreateProcedureQuery
EXECUTE 'create procedure p_a_3_s() language sql as $$select c1::int from t_1$$';
-- QUERY END: CreateProcedureQuery

-- QUERY START: CreateViewQuery
EXECUTE 'create view v_a_8 as select c1 from t_1';
-- QUERY END: CreateViewQuery

-- QUERY START: CreateIndexQuery
EXECUTE 'CREATE INDEX "i_a_1"
    ON "t_1" USING BTREE (f_4_s(c1));';
-- QUERY END: CreateIndexQuery

-- QUERY START: CreateTriggerQuery
EXECUTE 'create trigger tr_a_1 after insert on t_1 for each row execute function f_5_p()';
-- QUERY END: CreateTriggerQuery

END;
$DNDBTGeneratedScriptTransactionBlock$