DO $DNDBTGeneratedScriptTransactionBlock$
BEGIN

-- QUERY START: AlterSequenceQuery
EXECUTE 'ALTER SEQUENCE "s_1" RENAME TO "s_1x";
ALTER SEQUENCE "s_1x"
    MINVALUE -777';
-- QUERY END: AlterSequenceQuery

-- QUERY START: AlterSequenceQuery
EXECUTE 'ALTER SEQUENCE "s_1x"
    OWNED BY "t_1"."c1"';
-- QUERY END: AlterSequenceQuery

END;
$DNDBTGeneratedScriptTransactionBlock$