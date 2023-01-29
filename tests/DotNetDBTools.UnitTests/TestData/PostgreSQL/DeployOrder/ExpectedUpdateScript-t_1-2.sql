DO $DNDBTGeneratedScriptTransactionBlock$
BEGIN

-- QUERY START: DropTriggerQuery
EXECUTE 'DROP TRIGGER "tr_a_1" ON "t_1";';
-- QUERY END: DropTriggerQuery

-- QUERY START: DropForeignKeyQuery
EXECUTE 'ALTER TABLE "t_d_1"
    DROP CONSTRAINT "fk_t_d_1";';
-- QUERY END: DropForeignKeyQuery

-- QUERY START: DropIndexQuery
EXECUTE 'DROP INDEX "i_a_1";';
-- QUERY END: DropIndexQuery

-- QUERY START: DropIndexQuery
EXECUTE 'DROP INDEX "i_t_1_1";';
-- QUERY END: DropIndexQuery

-- QUERY START: DropIndexQuery
EXECUTE 'DROP INDEX "i_t_1_2";';
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
    "c2" INT NULL,
    "c3" TEXT NULL,
    CONSTRAINT "pk_t_1" PRIMARY KEY ("c3", "c1"),
    CONSTRAINT "uq_t_1_1" UNIQUE ("c1"),
    CONSTRAINT "ck_t_1" CHECK (c1 != 6 and c3 != ''zz'')
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

-- QUERY START: CreateIndexQuery
EXECUTE 'CREATE UNIQUE INDEX "i_t_1_1"
    ON "t_1" USING BTREE ("c3");';
-- QUERY END: CreateIndexQuery

-- QUERY START: CreateIndexQuery
EXECUTE 'CREATE INDEX "i_t_1_2"
    ON "t_1" USING BTREE ("c3")
    INCLUDE ("c1");';
-- QUERY END: CreateIndexQuery

-- QUERY START: CreateForeignKeyQuery
EXECUTE 'ALTER TABLE "t_d_1"
    ADD CONSTRAINT "fk_t_d_1" FOREIGN KEY ("c1", "c2")
        REFERENCES "t_1" ("c3", "c1")
        ON UPDATE NO ACTION ON DELETE NO ACTION;';
-- QUERY END: CreateForeignKeyQuery

-- QUERY START: CreateTriggerQuery
EXECUTE 'create trigger tr_a_1 after insert on t_1 for each row execute function f_5_p()';
-- QUERY END: CreateTriggerQuery

END;
$DNDBTGeneratedScriptTransactionBlock$