DECLARE @Metadata NVARCHAR(MAX) = 'DotNetDBTools.Models.MSSQL.MSSQLTableInfo';

INSERT INTO DNDBTDbObjects
(
    ID,
    Type,
    Name,
    Metadata
)
VALUES
(
    '299675e6-4faa-4d0f-a36a-224306ba5bcb',
    'Table',
    'MyTable1',
    @Metadata
);

CREATE TABLE MyTable1
(
    MyColumn1 IntDbType,
    MyColumn2 StringDbType,
    CONSTRAINT FK_MyTable1_MyColumn1_MyTable2_MyColumn1 FOREIGN KEY (MyColumn1) REFERENCES MyTable2(MyColumn1)
);


DECLARE @Metadata NVARCHAR(MAX) = 'DotNetDBTools.Models.MSSQL.MSSQLTableInfo';

INSERT INTO DNDBTDbObjects
(
    ID,
    Type,
    Name,
    Metadata
)
VALUES
(
    '562ec55b-6c11-4dde-b445-f062b12ca4ac',
    'Table',
    'MyTable2',
    @Metadata
);

CREATE TABLE MyTable2
(
    MyColumn1 IntDbType,
    MyColumn2 MyUserDefinedType1
);