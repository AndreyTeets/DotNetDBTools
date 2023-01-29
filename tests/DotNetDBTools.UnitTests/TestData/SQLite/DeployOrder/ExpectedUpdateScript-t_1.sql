PRAGMA foreign_keys=off;
BEGIN TRANSACTION;

-- QUERY START: DropTriggerQuery
DROP TRIGGER [tr_a_1];
-- QUERY END: DropTriggerQuery

-- QUERY START: DropIndexQuery
DROP INDEX [i_t_1_1];
-- QUERY END: DropIndexQuery

-- QUERY START: DropIndexQuery
DROP INDEX [i_t_1_2];
-- QUERY END: DropIndexQuery

-- QUERY START: DropViewQuery
DROP VIEW [v_a_1];
-- QUERY END: DropViewQuery

-- QUERY START: AlterTableQuery
CREATE TABLE [_DNDBTTemp_t_1]
(
    [c1] BLOB NULL DEFAULT 4,
    [c2] INTEGER NULL,
    [c3] TEXT NULL,
    CONSTRAINT [PK_t_1] PRIMARY KEY ([c3], [c1]),
    CONSTRAINT [uq_t_1_1] UNIQUE ([c1]),
    CONSTRAINT [ck_t_1] CHECK (c1 != 6 and c3 != 'zz')
);

INSERT INTO [_DNDBTTemp_t_1]
(
    [c1],
    [c2],
    [c3]
)
SELECT
    [c1],
    [c2],
    [c3]
FROM [t_1];

DROP TABLE [t_1];

ALTER TABLE [_DNDBTTemp_t_1] RENAME TO [t_1];
-- QUERY END: AlterTableQuery

-- QUERY START: CreateViewQuery
create view v_a_1 as select c1 from t_1;
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