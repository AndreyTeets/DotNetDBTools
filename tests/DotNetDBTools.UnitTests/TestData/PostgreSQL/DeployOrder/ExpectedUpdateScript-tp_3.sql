DO $DNDBTGeneratedScriptTransactionBlock$
BEGIN

-- QUERY START: AlterDomainTypeQuery
EXECUTE 'ALTER DOMAIN "tp_3" DROP CONSTRAINT "ck_tp_3";';
-- QUERY END: AlterDomainTypeQuery

-- QUERY START: AlterDomainTypeQuery
EXECUTE 'ALTER DOMAIN "tp_3" RENAME TO "tp_3x";';
-- QUERY END: AlterDomainTypeQuery

-- QUERY START: AlterDomainTypeQuery
EXECUTE 'ALTER DOMAIN "tp_3x" SET DEFAULT 444;';
-- QUERY END: AlterDomainTypeQuery

-- QUERY START: AlterDomainTypeQuery
EXECUTE 'ALTER DOMAIN "tp_3x" ADD CONSTRAINT "ck_tp_3" CHECK (value != 9);';
-- QUERY END: AlterDomainTypeQuery

END;
$DNDBTGeneratedScriptTransactionBlock$