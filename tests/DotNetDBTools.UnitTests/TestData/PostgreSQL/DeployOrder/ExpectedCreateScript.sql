DO $DNDBTGeneratedScriptTransactionBlock$
BEGIN

-- QUERY START: CreateSequenceQuery
EXECUTE 'CREATE SEQUENCE "s_1"
    AS INT
    START 100 INCREMENT 1 MINVALUE 1 MAXVALUE 2147483647 CACHE 1 NO CYCLE;';
-- QUERY END: CreateSequenceQuery

-- QUERY START: CreateFunctionQuery
EXECUTE 'create function f_3_s() returns int language sql as $$select (3 + 3)$$';
-- QUERY END: CreateFunctionQuery

-- QUERY START: CreateFunctionQuery
EXECUTE 'create function f_1_p() returns int language plpgsql as $$begin return (select f_2_s(8)); end$$';
-- QUERY END: CreateFunctionQuery

-- QUERY START: CreateFunctionQuery
EXECUTE 'create function f_2_s(x int) returns int language sql as $$select x + f_3_s()$$';
-- QUERY END: CreateFunctionQuery

-- QUERY START: CreateFunctionQuery
EXECUTE 'create function f_7_s() returns int language sql as $$select nextval(''s_1'')::int$$';
-- QUERY END: CreateFunctionQuery

-- QUERY START: CreateFunctionQuery
EXECUTE 'create function f_9_p() returns int language plpgsql as $$begin return (select (7 + 7)); end$$';
-- QUERY END: CreateFunctionQuery

-- QUERY START: CreateViewQuery
EXECUTE 'create view v_1 as select (8 + 8) as c1';
-- QUERY END: CreateViewQuery

-- QUERY START: CreateFunctionQuery
EXECUTE 'create function f_4_s(x int) returns int language sql immutable as $$select x + f_7_s()$$';
-- QUERY END: CreateFunctionQuery

-- QUERY START: CreateFunctionQuery
EXECUTE 'create function f_5_p() returns trigger language plpgsql as $$begin perform (select f_2_s(8)); return NULL; end$$';
-- QUERY END: CreateFunctionQuery

-- QUERY START: CreateFunctionQuery
EXECUTE 'create function f_6_s() returns int language sql as $$select f_1_p()$$';
-- QUERY END: CreateFunctionQuery

-- QUERY START: CreateFunctionQuery
EXECUTE 'create function f_8_p() returns int language plpgsql as $$begin return (select nextval(''s_1'')); end$$';
-- QUERY END: CreateFunctionQuery

-- QUERY START: CreateFunctionQuery
EXECUTE 'create function f_a_4_s() returns int language sql as $$select c1 from v_1$$';
-- QUERY END: CreateFunctionQuery

-- QUERY START: CreateFunctionQuery
EXECUTE 'create function f_a_5_p() returns int language plpgsql as $$begin return 5::tp_3::int; end$$';
-- QUERY END: CreateFunctionQuery

-- QUERY START: CreateFunctionQuery
EXECUTE 'create function f_a_6_p() returns int language plpgsql as $$begin return (''(5)''::tp_9).a1::int; end$$';
-- QUERY END: CreateFunctionQuery

-- QUERY START: CreateFunctionQuery
EXECUTE 'create function f_a_7_p() returns int language plpgsql as $$begin return (select c1::int from t_1); end$$';
-- QUERY END: CreateFunctionQuery

-- QUERY START: CreateFunctionQuery
EXECUTE 'create function f_a_8_p() returns int language plpgsql as $$begin return (select c1 from v_1); end$$';
-- QUERY END: CreateFunctionQuery

-- QUERY START: CreateViewQuery
EXECUTE 'create view v_a_1 as select (1000 + nextval(''s_1''))';
-- QUERY END: CreateViewQuery

-- QUERY START: CreateViewQuery
EXECUTE 'create view v_a_2 as select (1000 + f_3_s())';
-- QUERY END: CreateViewQuery

-- QUERY START: CreateViewQuery
EXECUTE 'create view v_a_3 as select (1000 + f_2_s(8))';
-- QUERY END: CreateViewQuery

-- QUERY START: CreateViewQuery
EXECUTE 'create view v_a_4 as select (1000 + f_9_p())';
-- QUERY END: CreateViewQuery

-- QUERY START: CreateViewQuery
EXECUTE 'create view v_a_5 as select (1000 + f_1_p())';
-- QUERY END: CreateViewQuery

-- QUERY START: CreateViewQuery
EXECUTE 'create view v_a_9 as select c1 from v_1';
-- QUERY END: CreateViewQuery

-- QUERY START: CreateDomainTypeQuery
EXECUTE 'CREATE DOMAIN "tp_30" AS
    INT NULL DEFAULT 34
    CONSTRAINT "ck_tp_30" CHECK (value != 36);';
-- QUERY END: CreateDomainTypeQuery

-- QUERY START: CreateDomainTypeQuery
EXECUTE 'CREATE DOMAIN "tp_2" AS
    "tp_30" NULL;';
-- QUERY END: CreateDomainTypeQuery

-- QUERY START: CreateDomainTypeQuery
EXECUTE 'CREATE DOMAIN "tp_3" AS
    INT NULL DEFAULT 4
    CONSTRAINT "ck_tp_3" CHECK (value != 6);';
-- QUERY END: CreateDomainTypeQuery

-- QUERY START: CreateCompositeTypeQuery
EXECUTE 'CREATE TYPE "tp_6" AS
(
    "a1" "tp_2"
);';
-- QUERY END: CreateCompositeTypeQuery

-- QUERY START: CreateCompositeTypeQuery
EXECUTE 'CREATE TYPE "tp_9" AS
(
    "a1" INT
);';
-- QUERY END: CreateCompositeTypeQuery

-- QUERY START: CreateDomainTypeQuery
EXECUTE 'CREATE DOMAIN "d_a_1" AS
    INT NULL DEFAULT (1000 + nextval(''s_1''));';
-- QUERY END: CreateDomainTypeQuery

-- QUERY START: CreateDomainTypeQuery
EXECUTE 'CREATE DOMAIN "d_a_2" AS
    INT NULL DEFAULT (1000 + f_3_s());';
-- QUERY END: CreateDomainTypeQuery

-- QUERY START: CreateDomainTypeQuery
EXECUTE 'CREATE DOMAIN "d_a_3" AS
    INT NULL DEFAULT (1000 + f_2_s(8));';
-- QUERY END: CreateDomainTypeQuery

-- QUERY START: CreateDomainTypeQuery
EXECUTE 'CREATE DOMAIN "d_a_4" AS
    INT NULL DEFAULT (1000 + f_9_p());';
-- QUERY END: CreateDomainTypeQuery

-- QUERY START: CreateDomainTypeQuery
EXECUTE 'CREATE DOMAIN "d_a_5" AS
    INT NULL DEFAULT (1000 + f_1_p());';
-- QUERY END: CreateDomainTypeQuery

-- QUERY START: CreateDomainTypeQuery
EXECUTE 'CREATE DOMAIN "d_a_6" AS
    INT NULL;';
-- QUERY END: CreateDomainTypeQuery

-- QUERY START: CreateDomainTypeQuery
EXECUTE 'CREATE DOMAIN "d_a_7" AS
    INT NULL;';
-- QUERY END: CreateDomainTypeQuery

-- QUERY START: CreateDomainTypeQuery
EXECUTE 'CREATE DOMAIN "d_b_1" AS
    INT NULL
    CONSTRAINT "ck_d_b_1" CHECK (value != (1000 + nextval(''s_1'')));';
-- QUERY END: CreateDomainTypeQuery

-- QUERY START: CreateDomainTypeQuery
EXECUTE 'CREATE DOMAIN "d_b_2" AS
    INT NULL
    CONSTRAINT "ck_d_b_2" CHECK (value != (1000 + f_3_s()));';
-- QUERY END: CreateDomainTypeQuery

-- QUERY START: CreateDomainTypeQuery
EXECUTE 'CREATE DOMAIN "d_b_3" AS
    INT NULL
    CONSTRAINT "ck_d_b_3" CHECK (value != (1000 + f_2_s(8)));';
-- QUERY END: CreateDomainTypeQuery

-- QUERY START: CreateDomainTypeQuery
EXECUTE 'CREATE DOMAIN "d_b_4" AS
    INT NULL
    CONSTRAINT "ck_d_b_4" CHECK (value != (1000 + f_9_p()));';
-- QUERY END: CreateDomainTypeQuery

-- QUERY START: CreateDomainTypeQuery
EXECUTE 'CREATE DOMAIN "d_b_5" AS
    INT NULL
    CONSTRAINT "ck_d_b_5" CHECK (value != (1000 + f_1_p()));';
-- QUERY END: CreateDomainTypeQuery

-- QUERY START: CreateDomainTypeQuery
EXECUTE 'CREATE DOMAIN "d_b_6" AS
    INT NULL;';
-- QUERY END: CreateDomainTypeQuery

-- QUERY START: CreateDomainTypeQuery
EXECUTE 'CREATE DOMAIN "d_b_7" AS
    INT NULL;';
-- QUERY END: CreateDomainTypeQuery

-- QUERY START: CreateDomainTypeQuery
EXECUTE 'CREATE DOMAIN "tp_1" AS
    "tp_6" NULL;';
-- QUERY END: CreateDomainTypeQuery

-- QUERY START: CreateRangeTypeQuery
EXECUTE 'DO $DNDBTPlPgSqlBlock$
BEGIN
IF (SELECT current_setting(''server_version_num'')::int) >= 140000 THEN
CREATE TYPE "tp_4" AS RANGE
(
    SUBTYPE = "tp_30",
    MULTIRANGE_TYPE_NAME = "tp_4_multirange"
);
ELSE
CREATE TYPE "tp_4" AS RANGE
(
    SUBTYPE = "tp_30"
);
END IF;
END;
$DNDBTPlPgSqlBlock$';
-- QUERY END: CreateRangeTypeQuery

-- QUERY START: CreateCompositeTypeQuery
EXECUTE 'CREATE TYPE "tp_5" AS
(
    "a1" "tp_3"
);';
-- QUERY END: CreateCompositeTypeQuery

-- QUERY START: CreateCompositeTypeQuery
EXECUTE 'CREATE TYPE "tp_7" AS
(
    "a1" "tp_9"
);';
-- QUERY END: CreateCompositeTypeQuery

-- QUERY START: CreateEnumTypeQuery
EXECUTE 'CREATE TYPE "tp_8" AS ENUM
(
    ''l1'',
    ''l2''
);';
-- QUERY END: CreateEnumTypeQuery

-- QUERY START: CreateTableQuery
EXECUTE 'CREATE TABLE "t_1"
(
    "c1" INT NULL DEFAULT 4,
    CONSTRAINT "ck_t_1" CHECK (c1 != 6)
);';
-- QUERY END: CreateTableQuery

-- QUERY START: CreateTableQuery
EXECUTE 'CREATE TABLE "t_a_1"
(
    "c1" INT NULL DEFAULT (1000 + nextval(''s_1'')),
    "c2" INT NULL
);';
-- QUERY END: CreateTableQuery

-- QUERY START: CreateTableQuery
EXECUTE 'CREATE TABLE "t_a_2"
(
    "c1" INT NULL DEFAULT (1000 + f_3_s()),
    "c2" INT NULL
);';
-- QUERY END: CreateTableQuery

-- QUERY START: CreateTableQuery
EXECUTE 'CREATE TABLE "t_a_3"
(
    "c1" INT NULL DEFAULT (1000 + f_2_s(8)),
    "c2" INT NULL
);';
-- QUERY END: CreateTableQuery

-- QUERY START: CreateTableQuery
EXECUTE 'CREATE TABLE "t_a_4"
(
    "c1" INT NULL DEFAULT (1000 + f_9_p()),
    "c2" INT NULL
);';
-- QUERY END: CreateTableQuery

-- QUERY START: CreateTableQuery
EXECUTE 'CREATE TABLE "t_a_5"
(
    "c1" INT NULL DEFAULT (1000 + f_1_p()),
    "c2" INT NULL
);';
-- QUERY END: CreateTableQuery

-- QUERY START: CreateTableQuery
EXECUTE 'CREATE TABLE "t_a_6"
(
    "c1" INT NULL DEFAULT 5::tp_3::int,
    "c2" INT NULL
);';
-- QUERY END: CreateTableQuery

-- QUERY START: CreateTableQuery
EXECUTE 'CREATE TABLE "t_a_7"
(
    "c1" INT NULL DEFAULT (''(5)''::tp_9).a1::int,
    "c2" INT NULL
);';
-- QUERY END: CreateTableQuery

-- QUERY START: CreateTableQuery
EXECUTE 'CREATE TABLE "t_b_1"
(
    "c1" INT NULL,
    CONSTRAINT "ck_t_b_1" CHECK (c1 != (1000 + nextval(''s_1'')))
);';
-- QUERY END: CreateTableQuery

-- QUERY START: CreateTableQuery
EXECUTE 'CREATE TABLE "t_b_2"
(
    "c1" INT NULL,
    CONSTRAINT "ck_t_b_2" CHECK (c1 != (1000 + f_3_s()))
);';
-- QUERY END: CreateTableQuery

-- QUERY START: CreateTableQuery
EXECUTE 'CREATE TABLE "t_b_3"
(
    "c1" INT NULL,
    CONSTRAINT "ck_t_b_3" CHECK (c1 != (1000 + f_2_s(8)))
);';
-- QUERY END: CreateTableQuery

-- QUERY START: CreateTableQuery
EXECUTE 'CREATE TABLE "t_b_4"
(
    "c1" INT NULL,
    CONSTRAINT "ck_t_b_4" CHECK (c1 != (1000 + f_9_p()))
);';
-- QUERY END: CreateTableQuery

-- QUERY START: CreateTableQuery
EXECUTE 'CREATE TABLE "t_b_5"
(
    "c1" INT NULL,
    CONSTRAINT "ck_t_b_5" CHECK (c1 != (1000 + f_1_p()))
);';
-- QUERY END: CreateTableQuery

-- QUERY START: CreateTableQuery
EXECUTE 'CREATE TABLE "t_b_6"
(
    "c1" INT NULL,
    CONSTRAINT "ck_t_b_6" CHECK (c1 != 5::tp_3::int)
);';
-- QUERY END: CreateTableQuery

-- QUERY START: CreateTableQuery
EXECUTE 'CREATE TABLE "t_b_7"
(
    "c1" INT NULL,
    CONSTRAINT "ck_t_b_7" CHECK (c1 != (''(5)''::tp_9).a1::int)
);';
-- QUERY END: CreateTableQuery

-- QUERY START: CreateTableQuery
EXECUTE 'CREATE TABLE "t_c_1"
(
    "c1" "tp_3" NULL
);';
-- QUERY END: CreateTableQuery

-- QUERY START: CreateTableQuery
EXECUTE 'CREATE TABLE "t_c_2"
(
    "c1" "tp_9" NULL
);';
-- QUERY END: CreateTableQuery

-- QUERY START: CreateTableQuery
EXECUTE 'CREATE TABLE "t_c_3"
(
    "c1" "tp_8" NULL
);';
-- QUERY END: CreateTableQuery

-- QUERY START: CreateTableQuery
EXECUTE 'CREATE TABLE "t_c_4"
(
    "c1" "tp_4" NULL
);';
-- QUERY END: CreateTableQuery

-- QUERY START: CreateTableQuery
EXECUTE 'CREATE TABLE "t_c_5"
(
    "c1" "tp_6" NULL
);';
-- QUERY END: CreateTableQuery

-- QUERY START: CreateFunctionQuery
EXECUTE 'create function f_a_1_s() returns int language sql as $$select 5::tp_3::int$$';
-- QUERY END: CreateFunctionQuery

-- QUERY START: CreateFunctionQuery
EXECUTE 'create function f_a_2_s() returns int language sql as $$select (''(5)''::tp_9).a1::int$$';
-- QUERY END: CreateFunctionQuery

-- QUERY START: CreateFunctionQuery
EXECUTE 'create function f_a_3_s() returns int language sql as $$select c1::int from t_1$$';
-- QUERY END: CreateFunctionQuery

-- QUERY START: CreateViewQuery
EXECUTE 'create view v_a_6 as select 5::tp_3::int';
-- QUERY END: CreateViewQuery

-- QUERY START: CreateViewQuery
EXECUTE 'create view v_a_7 as select (''(5)''::tp_9).a1::int';
-- QUERY END: CreateViewQuery

-- QUERY START: CreateViewQuery
EXECUTE 'create view v_a_8 as select c1 from t_1';
-- QUERY END: CreateViewQuery

-- QUERY START: AlterDomainTypeQuery
EXECUTE 'ALTER DOMAIN "d_a_6" SET DEFAULT 5::tp_3::int;';
-- QUERY END: AlterDomainTypeQuery

-- QUERY START: AlterDomainTypeQuery
EXECUTE 'ALTER DOMAIN "d_a_7" SET DEFAULT (''(5)''::tp_9).a1::int;';
-- QUERY END: AlterDomainTypeQuery

-- QUERY START: AlterDomainTypeQuery
EXECUTE 'ALTER DOMAIN "d_b_6" ADD CONSTRAINT "ck_d_b_6" CHECK (value != 5::tp_3::int);';
-- QUERY END: AlterDomainTypeQuery

-- QUERY START: AlterDomainTypeQuery
EXECUTE 'ALTER DOMAIN "d_b_7" ADD CONSTRAINT "ck_d_b_7" CHECK (value != (''(5)''::tp_9).a1::int);';
-- QUERY END: AlterDomainTypeQuery

-- QUERY START: CreateIndexQuery
EXECUTE 'CREATE INDEX "i_a_1"
    ON "t_1" (f_4_s(c1));';
-- QUERY END: CreateIndexQuery

-- QUERY START: CreateTriggerQuery
EXECUTE 'create trigger tr_a_1 after insert on t_1 for each row execute function f_5_p()';
-- QUERY END: CreateTriggerQuery

END;
$DNDBTGeneratedScriptTransactionBlock$