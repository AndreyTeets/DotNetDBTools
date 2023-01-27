--ID:#{0649FE45-4560-49EA-B01C-C9B7B63338BF}#
create or replace procedure MyPLPGSQLProc2()
language plpgsql    
as $$
declare
    i int = 3;
begin
    --comm--ent$x$;
    call Proc2(i::text, -6);

    commit;
end;$$