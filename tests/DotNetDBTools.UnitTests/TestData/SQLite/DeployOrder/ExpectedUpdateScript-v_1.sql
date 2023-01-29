PRAGMA foreign_keys=off;
BEGIN TRANSACTION;

-- QUERY START: DropViewQuery
DROP VIEW [v_1];
-- QUERY END: DropViewQuery

-- QUERY START: CreateViewQuery
create view v_1 as select (5 + 9) as c1;
-- QUERY END: CreateViewQuery

COMMIT TRANSACTION;