declare @Metadata nvarchar(max) = 'DotNetDBTools.Models.SQLite.SQLiteTableInfo'

insert into DNDBTDbObjects
(
    ID,
    Type,
    Name,
    Metadata
)
values
(
    '299675e6-4faa-4d0f-a36a-224306ba5bcb',
    'Table',
    'MyTable1',
    @Metadata
)

create table MyTable1
(
    MyColumn1 IntDbType,
    MyColumn2 StringDbType,
    constraint FK_MyTable1_MyColumn1_MyTable2_MyColumn1 foreign key (MyColumn1) references MyTable2(MyColumn1)
)


declare @Metadata nvarchar(max) = 'DotNetDBTools.Models.SQLite.SQLiteTableInfo'

insert into DNDBTDbObjects
(
    ID,
    Type,
    Name,
    Metadata
)
values
(
    '562ec55b-6c11-4dde-b445-f062b12ca4ac',
    'Table',
    'MyTable2',
    @Metadata
)

create table MyTable2
(
    MyColumn1 IntDbType,
    MyColumn2 ByteDbType
)