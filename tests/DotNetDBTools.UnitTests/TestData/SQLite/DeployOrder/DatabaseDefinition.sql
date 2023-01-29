--Leaf objects with no dependencies

--ID:#{62CA48F0-DA8A-469B-B82F-140A2BC096CD}#
create table t_1(
    --ID:#{AB9F65C5-54E1-4662-BE5C-7C263F9964C6}#
    c1 integer default 4,
    --ID:#{CD0727CA-6B98-4FED-8F35-4F36A7C50A6E}#
    c2 int,
    --ID:#{5A42F66B-1657-4E72-991A-15ED5390AD1F}#
    c3 text,
    --ID:#{A53E05E7-221F-4CC0-B4A4-B45249266FE5}#
    constraint pk_t_1 primary key (c3, c1),
    --ID:#{27DD9D35-4755-4D44-A61E-80D2CB44CDA8}#
    constraint uq_t_1_1 unique (c1),
    --ID:#{282DC612-DF6F-4359-8DC5-FC81BAB42184}#
    constraint ck_t_1 check (c1 != 6 and c3 != 'zz')
);
--ID:#{0F6DB84C-C3F2-4F48-B012-41DB2A2ECAE7}#
create unique index i_t_1_1 on t_1 (c3);
--ID:#{65C613D8-DA35-4DA6-936D-119D7168D672}#
create index i_t_1_2 on t_1 (c1);

--ID:#{9EE268C6-69F4-4237-BA59-89352DFAFB67}#
create view v_1 as select (8 + 8) as c1;


--Views depending on tables/views

--ID:#{63F4916B-1C6C-480A-9734-B159B0973DE6}#
create view v_a_1 as select c1 from t_1;

--ID:#{1A9053BA-28F3-45DC-BB8D-58E5717AFD58}#
create view v_a_2 as select c1 from v_1;


--Table foreign keys depending on tables/constraints/indexes/columns

--ID:#{D1019535-9C2C-4569-93D2-8F0FA18D0C25}#
create table t_a_1(
    --ID:#{0CE7A019-BA61-4ABA-B25D-E6FA0A74D0DA}#
    c1 text,
    --ID:#{B2237D41-3033-4D38-AF59-29BB9113C420}#
    c2 integer,
    --ID:#{910F2B9E-2EA5-4327-9238-530B96BF4BB2}#
    c3 integer,
    --ID:#{D59AA538-E67E-4605-B4ED-B2A33DD17BFD}#
    constraint fk_t_a_1 foreign key (c1, c2) references t_1 (c3, c1)
);


--Triggers depending on views/tables/columns

--ID:#{3C20BC64-4CA6-4859-9536-3378F6132CD0}#
create trigger tr_a_1 after insert on t_1 for each row BEGIN insert into t_1(c1) select count(*) from v_1 where c1 != (NEW.c1); END;


--Objects with dependencies unique to them only

--ID:#{23AD495C-3DA1-4CEC-9804-380C0CB3CBF1}#
create view v_u_1_base as select 3 as c1;
--ID:#{93092560-6D56-455B-83BA-47350BFF3D80}#
create view v_u_1 as select c1 from v_u_1_base;
