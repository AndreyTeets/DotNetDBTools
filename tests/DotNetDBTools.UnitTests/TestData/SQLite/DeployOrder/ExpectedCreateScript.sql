PRAGMA foreign_keys=off;
BEGIN TRANSACTION;

-- QUERY START: CreateTableQuery
CREATE TABLE [t_1]
(
    [c1] INTEGER NULL DEFAULT 4,
    [c2] INTEGER NULL,
    [c3] TEXT NULL,
    CONSTRAINT [PK_t_1] PRIMARY KEY ([c3], [c1]),
    CONSTRAINT [uq_t_1_1] UNIQUE ([c1]),
    CONSTRAINT [ck_t_1] CHECK (c1 != 6 and c3 != 'zz')
);
-- QUERY END: CreateTableQuery

-- QUERY START: CreateTableQuery
CREATE TABLE [t_a_1]
(
    [c1] TEXT NULL,
    [c2] INTEGER NULL,
    [c3] INTEGER NULL,
    CONSTRAINT [fk_t_a_1] FOREIGN KEY ([c1], [c2])
        REFERENCES [t_1]([c3], [c1])
        ON UPDATE NO ACTION ON DELETE NO ACTION
);
-- QUERY END: CreateTableQuery

-- QUERY START: CreateViewQuery
create view v_1 as select (8 + 8) as c1;
-- QUERY END: CreateViewQuery

-- QUERY START: CreateViewQuery
create view v_a_1 as select c1 from t_1;
-- QUERY END: CreateViewQuery

-- QUERY START: CreateViewQuery
create view v_a_2 as select c1 from v_1;
-- QUERY END: CreateViewQuery

-- QUERY START: CreateViewQuery
create view v_u_1 as select c1 from v_u_1_base;
-- QUERY END: CreateViewQuery

-- QUERY START: CreateViewQuery
create view v_u_1_base as select 3 as c1;
-- QUERY END: CreateViewQuery

-- QUERY START: CreateIndexQuery
CREATE UNIQUE INDEX [i_t_1_1]
    ON [t_1] ([c3]);
-- QUERY END: CreateIndexQuery

-- QUERY START: CreateIndexQuery
CREATE INDEX [i_t_1_2]
    ON [t_1] ([c1]);
-- QUERY END: CreateIndexQuery

-- QUERY START: CreateTriggerQuery
create trigger tr_a_1 after insert on t_1 for each row BEGIN insert into t_1(c1) select count(*) from v_1 where c1 != (NEW.c1); END;
-- QUERY END: CreateTriggerQuery

COMMIT TRANSACTION;