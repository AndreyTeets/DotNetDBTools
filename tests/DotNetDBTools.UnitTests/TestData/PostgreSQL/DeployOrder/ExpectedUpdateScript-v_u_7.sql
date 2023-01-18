DO $DNDBTGeneratedScriptTransactionBlock$
BEGIN

-- QUERY START: DropViewQuery
EXECUTE 'DROP VIEW "v_u_7";';
-- QUERY END: DropViewQuery

-- QUERY START: CreateViewQuery
EXECUTE 'create view v_u_7x as select (''(5)''::tp_u_7).a1::int';
-- QUERY END: CreateViewQuery

END;
$DNDBTGeneratedScriptTransactionBlock$