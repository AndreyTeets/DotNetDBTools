--ID:#{6CA51F29-C1BC-4349-B9C1-6F1EA170F162}#
CREATE TABLE MyTable5
(
    --ID:#{5309D66F-2030-402E-912E-5547BABAA072}#
    MyColumn1 integer NOT NULL DEFAULT (abS(-15)),

    --ID:#{11EF8E25-3691-42D4-B2FA-88D724F73B61}#
    MyColumn2 TEXT NOT NULL DEFAULT 'test',
    
    --ID:#{6ED0AB37-AAD3-4294-9BA6-C0921F0E67AF}#
    MyColumn3 BLOB NOT NULL DEFAULT X'000204',

    --ID:#{ACA57FD6-80D0-4C18-B2CA-AABCB06BEA10}#
    MyColumn4 REAL NOT NULL DEFAULT 123.456,

    --ID:#{47666B8B-CA72-4507-86B2-04C47A84AED4}#
    MyColumn5 REAL NOT NULL DEFAULT 12345.6789,

    --ID:#{98FDED6C-D486-4A2E-9C9A-1EC31C9D5830}#
    MyColumn6 NUMERIC NOT NULL DEFAULT 12.3,

    --ID:#{2502CADE-458A-48EE-9421-E6D7850493F7}#
    MyColumn7 INTEGER NOT NULL DEFAULT TRUE,

    --ID:#{ED044A8A-6858-41E2-A867-9E5B01F226C8}#
    MyColumn8 BLOB NOT NULL DEFAULT X'8e2f99ad0fc8456db0e4ec3ba572dd15',

    --ID:#{9939D676-73B7-42D1-BA3E-5C13AED5CE34}#
    MyColumn9 NUMERIC NOT NULL DEFAULT '2022-02-15',

    --ID:#{CBA4849B-3D84-4E38-B2C8-F9DBDFF22FA6}#
    MyColumn10 NUMERIC NOT NULL DEFAULT '16:17:18',

    --ID:#{4DDE852D-EC19-4B61-80F9-DA428D8FF41A}#
    MyColumn11 NUMERIC NOT NULL DEFAULT '2022-02-15 16:17:18',

    --ID:#{685FAF2E-FEF7-4E6B-A960-ACD093F1F004}#
    MyColumn12 NUMERIC NOT NULL DEFAULT '2022-02-15 16:17:18+01:30'
)
