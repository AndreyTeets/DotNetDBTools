DO $DNDBTGeneratedScriptTransactionBlock$
BEGIN

-- QUERY START: DropIndexQuery
EXECUTE 'DROP INDEX "i_a_1";';
-- QUERY END: DropIndexQuery

-- QUERY START: AlterTableQuery
EXECUTE 'ALTER TABLE "t_1"
    DROP CONSTRAINT "ck_t_1";';
-- QUERY END: AlterTableQuery

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
    DROP COLUMN "c1",
    ADD COLUMN "c1" INT NULL DEFAULT 4;';
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

-- QUERY START: AlterTableQuery
EXECUTE 'ALTER TABLE "t_1"
    ADD CONSTRAINT "ck_t_1" CHECK (c1 != 6);';
-- QUERY END: AlterTableQuery

-- QUERY START: CreateIndexQuery
EXECUTE 'CREATE INDEX "i_a_1"
    ON "t_1" USING BTREE (f_4_s(c1));';
-- QUERY END: CreateIndexQuery

END;
$DNDBTGeneratedScriptTransactionBlock$