--Leaf objects with no dependencies

--ID:#{9681D59F-A95A-4655-8614-F459705675EE}#
create sequence s_1 as int start 100;

--ID:#{A9864D47-646E-44A7-A20C-5A1AE339A88F}#
create function f_3_s() returns int language sql as $$select (3 + 3)$$;

--ID:#{59AE46D3-6E6D-4119-B7C7-53C8FEB6D4C1}#
create function f_9_p() returns int language plpgsql as $$begin return (select (7 + 7)); end$$;

--ID:#{599B62BD-140E-4FDD-981F-F0303176F82B}#
create type tp_8 as enum ('l1', 'l2');

--ID:#{0E7B0F07-F627-47FA-AFBE-1F0829CE802B}#
create type tp_9 as (a1 int);

--ID:#{19711A29-BF7D-4860-8ADE-DFB871EFD128}#
create domain tp_3 as int default 4
    --ID:#{400068A6-AB8B-4FD0-AA49-F5C0003B9E83}#
    constraint ck_tp_3 check (value != 6);

--ID:#{C16FA77D-D1E2-4387-9CDB-EC067E494690}#
create domain tp_30 as int default 34
    --ID:#{5A1685B8-2877-49A8-AA33-558D32628B8E}#
    constraint ck_tp_30 check (value != 36);

--ID:#{62CA48F0-DA8A-469B-B82F-140A2BC096CD}#
create table t_1(
    --ID:#{AB9F65C5-54E1-4662-BE5C-7C263F9964C6}#
    c1 int default 4,
    --ID:#{282DC612-DF6F-4359-8DC5-FC81BAB42184}#
    constraint ck_t_1 check (c1 != 6)
);

--ID:#{9EE268C6-69F4-4237-BA59-89352DFAFB67}#
create view v_1 as select (8 + 8) as c1;


--Functions depending on sequences/functions

--ID:#{FEEF1184-0317-4361-A639-B5C2FEAF8C1F}#
create function f_7_s() returns int language sql as $$select nextval('s_1')::int$$;

--ID:#{D75BBDF0-FE27-4F06-8A79-F0EF743E3120}#
create function f_8_p() returns int language plpgsql as $$begin return (select nextval('s_1')); end$$;

--ID:#{231182BD-DFE1-47D3-A2E8-9A86DFAA8C6C}#
create function f_2_s(x int) returns int language sql as $$select x + f_3_s()$$;

--ID:#{343CDD13-72DB-4C2C-8B36-9B3D8BC400B3}#
create function f_1_p() returns int language plpgsql as $$begin return (select f_2_s(8)); end$$;

--ID:#{6BB63614-A8C3-45B9-B89E-862A103083F0}#
create function f_6_s() returns int language sql as $$select f_1_p()$$;

--ID:#{9404CA54-065C-4C24-A532-D3DA5BA56D84}#
create function f_4_s(x int) returns int language sql immutable as $$select x + f_7_s()$$;

--ID:#{B4B7C052-D033-45DA-A3B3-79794EEECB24}#
create function f_5_p() returns trigger language plpgsql as $$begin perform (select f_2_s(8)); return NULL; end$$;


--Types depending on types

--ID:#{31387871-8EF5-4573-AC7B-656C838DD055}#
create domain tp_2 as tp_30;

--ID:#{8322187B-2312-44EC-938E-78CFD9CA672A}#
create type tp_4 as range (subtype = tp_30);

--ID:#{6EBFBD77-D1F4-497B-8E1D-EF7B15EFDF58}#
create type tp_5 as (a1 tp_3);

--ID:#{11BDFD15-1D09-4ABC-A7AC-5BA91E02A8C0}#
create type tp_7 as (a1 tp_9);

--ID:#{1EA90140-0AD1-4056-BE0E-D196E8E278C7}#
create type tp_6 as (a1 tp_2);

--ID:#{45CE75F5-22E6-4753-8275-ECC77E0D8AF5}#
create domain tp_1 as tp_6;


--Domain defaults depending on sequences/functions/types

--ID:#{5BD4EEF8-972B-42EA-972E-540471EFF232}#
create domain d_a_1 as int default (1000 + nextval('s_1'));

--ID:#{F9DBF411-0F5E-4510-99F3-04C0B1A933BD}#
create domain d_a_2 as int default (1000 + f_3_s());

--ID:#{B5FDC4E8-C824-4DCC-BE94-9A6AD45B8D4E}#
create domain d_a_3 as int default (1000 + f_2_s(8));

--ID:#{77EBB676-1CEB-490F-B4B0-258A2465F194}#
create domain d_a_4 as int default (1000 + f_9_p());

--ID:#{F63D6D79-7F0D-40BF-8F12-29C984167801}#
create domain d_a_5 as int default (1000 + f_1_p());

--ID:#{A6B0099F-C342-4967-A1E2-A07CBB224436}#
create domain d_a_6 as int default 5::tp_3::int;

--ID:#{A14A3B89-16B6-491A-9576-77E89A1D8AAF}#
create domain d_a_7 as int default ('(5)'::tp_9).a1::int;


--Domain check constraints depending on sequences/functions/types

--ID:#{D095DFBE-64C8-4264-8579-EE9761EDD95C}#
create domain d_b_1 as int
    --ID:#{64C48642-BB05-4BC5-BD50-73A0469433D6}#
    constraint ck_d_b_1 check (value != (1000 + nextval('s_1')));

--ID:#{375228CC-87F4-4959-8117-CD2C8C2B4526}#
create domain d_b_2 as int
    --ID:#{60CBBDB5-FDF8-4B2A-A563-A045E47F2111}#
    constraint ck_d_b_2 check (value != (1000 + f_3_s()));

--ID:#{84F68C5E-3DEB-4538-A895-528B2EB4DA13}#
create domain d_b_3 as int
    --ID:#{4E774461-0D09-45B7-951D-62F7149F1AB2}#
    constraint ck_d_b_3 check (value != (1000 + f_2_s(8)));

--ID:#{EB7225F3-F5E9-48E1-81EB-FE9056F318AA}#
create domain d_b_4 as int
    --ID:#{2186E994-E7FB-4A36-AB99-BF1BB3B86DFD}#
    constraint ck_d_b_4 check (value != (1000 + f_9_p()));

--ID:#{756E2A13-438C-46F0-B8AC-F0C216AE5C93}#
create domain d_b_5 as int
    --ID:#{98F7301C-67B5-45C9-99BF-0A4EE6AE87A7}#
    constraint ck_d_b_5 check (value != (1000 + f_1_p()));

--ID:#{E5F4C265-2209-443F-A7A8-9BA69C00C3CF}#
create domain d_b_6 as int
    --ID:#{FC6B56F9-7853-46B8-8DD7-CE297EAD9596}#
    constraint ck_d_b_6 check (value != 5::tp_3::int);

--ID:#{BF8ED39E-F971-4DEB-82CB-ECB68A853AD2}#
create domain d_b_7 as int
    --ID:#{583E22E0-396A-42E5-8FAE-50DA97703F85}#
    constraint ck_d_b_7 check (value != ('(5)'::tp_9).a1::int);


--Table defaults depending on sequences/functions/types

--ID:#{3F127587-94DF-47D0-A869-1258BA886678}#
create table t_a_1(
    --ID:#{7C0A18A2-A23A-4E28-A840-0E8A81B95E6F}#
    c1 int default (1000 + nextval('s_1')),
    --ID:#{05A54970-2B49-4C59-BEB6-7191E45EDA06}#
    c2 int
);

--ID:#{C3ABCE44-3A5F-4A13-8588-0AFEDC3B1E11}#
create table t_a_2(
    --ID:#{52EC8775-29A5-4CE6-888D-EF4098A12C75}#
    c1 int default (1000 + f_3_s()),
    --ID:#{75C43428-2F87-40E0-8185-8C118FDDA45A}#
    c2 int
);

--ID:#{18681D5F-77B6-41DB-9D3F-7BC2454210FB}#
create table t_a_3(
    --ID:#{AD2D5C7D-70D6-4EAE-959E-82D12B1D8E14}#
    c1 int default (1000 + f_2_s(8)),
    --ID:#{C0543AA5-A988-4FD9-B2A5-8C79B2AEA521}#
    c2 int
);

--ID:#{5771FC68-4784-4639-9FD0-3C39E42ECC7C}#
create table t_a_4(
    --ID:#{7ED27B03-8843-4259-B243-C72319A7778D}#
    c1 int default (1000 + f_9_p()),
    --ID:#{7ED27B03-8843-4259-B243-C72319A7778D}#
    c2 int
);

--ID:#{23E9F792-4A85-44A8-A09A-728BF6DF9D2B}#
create table t_a_5(
    --ID:#{1F809BCD-39DE-4D4C-A9CD-237636BD94D6}#
    c1 int default (1000 + f_1_p()),
    --ID:#{CA096997-AA57-488A-B963-B78D6F43C07B}#
    c2 int
);

--ID:#{672AB173-755B-4261-9B9C-6565892832FD}#
create table t_a_6(
    --ID:#{35C219EA-B0C3-4CDB-88B2-795486DEC551}#
    c1 int default 5::tp_3::int,
    --ID:#{35C219EA-B0C3-4CDB-88B2-795486DEC551}#
    c2 int
);

--ID:#{43939A75-386F-42EF-8A16-95EB12D281ED}#
create table t_a_7(
    --ID:#{508F61A6-BE9B-4A39-9C35-3C48D91D1C87}#
    c1 int default ('(5)'::tp_9).a1::int,
    --ID:#{165846BD-9E78-4BEE-8F99-70B69268E419}#
    c2 int
);


--Table check constraints depending on sequences/functions/types

--ID:#{794DF519-E7C8-49F3-A3E2-793D59B0DCE1}#
create table t_b_1(
    --ID:#{788BBCD9-D65E-4732-9DAA-5506ACAC3720}#
    c1 int,
    --ID:#{12AAB02D-A67E-4D58-A361-6C169E5A0A3B}#
    constraint ck_t_b_1 check (c1 != (1000 + nextval('s_1')))
);

--ID:#{205B728C-13C8-4D14-AF34-5AB9D834CE1C}#
create table t_b_2(
    --ID:#{F6C64F9A-B31F-45EE-9EDC-1C8411708179}#
    c1 int,
    --ID:#{161C29E2-54DD-4FEE-8A30-42A5E1DE246B}#
    constraint ck_t_b_2 check (c1 != (1000 + f_3_s()))
);

--ID:#{345D67A5-B9C9-4559-BC10-24E623A56837}#
create table t_b_3(
    --ID:#{2156DC68-23D0-43D0-9826-E88F755ED1B4}#
    c1 int,
    --ID:#{1AD0FBAF-FD54-411C-81E9-4C88B4F2A9B9}#
    constraint ck_t_b_3 check (c1 != (1000 + f_2_s(8)))
);

--ID:#{E48383B7-6F25-4595-B9B0-EBBCF5F8C60C}#
create table t_b_4(
    --ID:#{8080F0E2-ACA8-4824-A6DB-C4074CE4F4FA}#
    c1 int,
    --ID:#{9FAA0CEE-10F8-4284-864C-A1F0E937191B}#
    constraint ck_t_b_4 check (c1 != (1000 + f_9_p()))
);

--ID:#{A40416E6-E3C3-48CB-B5E2-C377225A52C1}#
create table t_b_5(
    --ID:#{B65EE51A-2AB2-4FF3-B12A-7DD9A168C6AB}#
    c1 int,
    --ID:#{AE546D7E-EFBA-420E-B644-F2CA01116376}#
    constraint ck_t_b_5 check (c1 != (1000 + f_1_p()))
);

--ID:#{7EC7C79A-E969-4DC0-90CF-AF35CD0E3EC6}#
create table t_b_6(
    --ID:#{B77F5ADD-3D77-4F4A-B04C-8BEF8EF8F050}#
    c1 int,
    --ID:#{F0479335-FE92-4E67-83E7-2FD6AD752C21}#
    constraint ck_t_b_6 check (c1 != 5::tp_3::int)
);

--ID:#{DBDB92CC-789E-469A-A3D6-3836DAFC15F2}#
create table t_b_7(
    --ID:#{4BE26347-3F57-4F3F-BC75-DBEAE73B863C}#
    c1 int,
    --ID:#{082A7A74-EEFE-4CB5-BE67-82C6A5394A56}#
    constraint ck_t_b_7 check (c1 != ('(5)'::tp_9).a1::int)
);


--Table columns depending on types

--ID:#{F249764E-FBB2-4807-B750-E05160E1F08B}#
create table t_c_1(
    --ID:#{1AEDD294-7173-431B-BD30-94C1496FCC98}#
    c1 tp_3
);

--ID:#{CD2E7752-3E81-42C0-BDA5-DFF4D53CE96A}#
create table t_c_2(
    --ID:#{A07533CC-F434-4393-80C8-861D3B440621}#
    c1 tp_9
);

--ID:#{27F9E210-107C-4EB0-A350-4088E17497A0}#
create table t_c_3(
    --ID:#{EBD30C1D-0715-4EE8-8B92-CDF607B02A06}#
    c1 tp_8
);

--ID:#{7D4E75D1-EE74-4901-B74D-DCC22091FD18}#
create table t_c_4(
    --ID:#{966A0B8F-12BF-432D-8061-4AE876751A27}#
    c1 tp_4
);

--ID:#{380FF7CC-C4B7-463E-8A84-CE5529F44F3D}#
create table t_c_5(
    --ID:#{AB37DCB6-F27E-4452-BB9C-9AA4A1FE5D19}#
    c1 tp_6
);


--Functions depending on types/tables/views

--ID:#{D9C38C8A-EEFD-4994-AC7E-3A11231D9B22}#
create function f_a_1_s() returns int language sql as $$select 5::tp_3::int$$;

--ID:#{DC816942-CC17-46E4-81FE-1C239D53F4B2}#
create function f_a_2_s() returns int language sql as $$select ('(5)'::tp_9).a1::int$$;

--ID:#{D9A4790A-4DBF-460B-9BC7-577B77E63CC7}#
create function f_a_3_s() returns int language sql as $$select c1::int from t_1$$;

--ID:#{436C5064-A4AB-4447-989F-D97A3EFD30D9}#
create function f_a_4_s() returns int language sql as $$select c1 from v_1$$;

--ID:#{F0EFE507-5C21-49FD-A56C-5AAE3580D3C4}#
create function f_a_5_p() returns int language plpgsql as $$begin return 5::tp_3::int; end$$;

--ID:#{F9A299C4-1718-47C7-A9DC-A48909E3E555}#
create function f_a_6_p() returns int language plpgsql as $$begin return ('(5)'::tp_9).a1::int; end$$;

--ID:#{0BF05C09-01A0-48DD-B307-399801445F08}#
create function f_a_7_p() returns int language plpgsql as $$begin return (select c1::int from t_1); end$$;

--ID:#{4076696D-53E7-4004-B54A-117B89B06C88}#
create function f_a_8_p() returns int language plpgsql as $$begin return (select c1 from v_1); end$$;


--Views depending on sequences/functions/types/tables/views

--ID:#{0D49BD25-4F3F-46C2-AC93-4B7280C2E7DD}#
create view v_a_1 as select (1000 + nextval('s_1'));

--ID:#{C6E25CBA-0ABE-4CB9-8DF7-C352D4551822}#
create view v_a_2 as select (1000 + f_3_s());

--ID:#{08904DC8-AA25-41D3-9421-07C2D242DDA0}#
create view v_a_3 as select (1000 + f_2_s(8));

--ID:#{5CC5D090-CFE2-4A9C-A0FB-F802BF99742C}#
create view v_a_4 as select (1000 + f_9_p());

--ID:#{4E7297DA-1897-4FB9-B9BC-BC088FD12DCA}#
create view v_a_5 as select (1000 + f_1_p());

--ID:#{0ECF7587-6C53-4515-BA2A-61598C403449}#
create view v_a_6 as select 5::tp_3::int;

--ID:#{C6A8DB12-B489-4F1E-B8A5-383878ED3E1C}#
create view v_a_7 as select ('(5)'::tp_9).a1::int;

--ID:#{63F4916B-1C6C-480A-9734-B159B0973DE6}#
create view v_a_8 as select c1 from t_1;

--ID:#{1A9053BA-28F3-45DC-BB8D-58E5717AFD58}#
create view v_a_9 as select c1 from v_1;


--Indexes and triggers depending on functions

--ID:#{70B777CA-6A71-4882-8DC3-23029C804063}#
create index i_a_1 on t_1 (f_4_s(c1));

--ID:#{3C20BC64-4CA6-4859-9536-3378F6132CD0}#
create trigger tr_a_1 after insert on t_1 for each row execute function f_5_p();
