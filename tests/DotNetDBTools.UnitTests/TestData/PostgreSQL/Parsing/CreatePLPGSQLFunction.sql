--ID:#{316C7688-D510-4A61-9D09-E15D465D0EFF}#
CREATE OR REPLACE FUNCTION public."_Some_Complex_PLPGSQL_Function"(
        var1 timestamp with time zone,
        OUT var2 boolean
    ) RETURNS record
    LANGUAGE 'plpgsql'
    COST 100
    VOLATILE PARALLEL UNSAFE
AS $FuncBody$

DECLARE
    -- *Special words names which should be usable for variables
    query text;
    temp xml;
    bigint bigint := 2;
    declare "text" text = (SELECT temp."Temp" FROM "MyTable01" temp WHERE TRUE IS FALSE);
    _ts timestamptz(0);
    "arr" text[];
    arr2 bigint[];
    "_arr3" uuid[];
    _Arr_4 text := (select "t"."Col1" from "MyTable02" "t" where "t"."Flag" is true LIMIT 1);

-- Second declare block and cursor declaration
declare
    "MyCursor" CURSOR FOR (
        select MIN("MIN") as "a", MAX("MAX") as b
        from MyTable03 t
        inner join (
            select Col2
            from "MyTable04"
        ) t2 on t2."Col1" = t."Col1"
        group by "ID"
    );

BEGIN
    -- Basic plpgsql statements
    --comm--ent$NotFuncBody$;
    insert INTO "MyTable1"("MyColumn1")
    VALUES(NEW."MyColumn1" || '$NotFuncBody$' || '$$');

    i = (SELECT "MyFunc1"(44));
    i = (select c1 FROM "MyView1" LEFT JOIN LATERAL (SELECT * from "MyTable2") t2 ON true INNER JOIN MyView2 ON TRUE);

    insert INTO "MyTable3"("MyColumn1")
    SELECT "MyColumn1" from MyView3;

    insert INTO "MyTable4"("MyColumn1")
    SELECT "MyColumn1" from (SELECT MyFunc2(44)) t;

    RETURN NULL;

    CALL "MyProc1"();
    CALL MyProc2(1, 2);

    -- Misc complex single-line statements
    "var1" = var1 + interval '2 day';
    var1 := "var1" + interval '2' day;
    "_var3" = CASE var2 % 2 WHEN 1 THEN 1.11 ELSE round(2.22) END;
    _var_4 = now() at time zone 'utc';
    temp = $1::text;
    GET DIAGNOSTICS "var2" = ROW_COUNT;

    SELECT * INTO _var_3, "_var_4" FROM "MyFunc3"(_var_3, "var1");
    DROP TABLE IF EXISTS "_some_table";
    create temp table "_some_table"("_c1" int);
    return query select * from "_some_table";
    return query SELECT array_append("var1", var2);
    RETURN QUERY SELECT * from unnest("var1", var2) "x" LIMIT 1;

    -- Complex string functions nested calls
    "var1" = position('/' in _var_3);
    "var1" = substring(_var_3, var2 + 1, length("_var_4") - var2);
    "var1" = strpos(substring(_var_3 from _var_4 + 1), '/');
    "var1" = array_append("_var_3", concat(coalesce(var2, '') || _var_4[array_upper(_var_3, 1)]));
    "var1" = array_cat(_var_3, array(select concat(var2,' ',"_var_4"[array_upper("_var_3", 1)]) from "_some_table" LIMIT 1));

    -- *Special words names which should be usable in statements and select into
    SELECT "Col1", _col2
    INTO query, temp
    FROM "MyTable5"
    WHERE "Col1" <= var1 AND "col2" > var2
    LIMIT 1;

    SELECT count(*)::int, string_agg('DROP FUNCTION ' || oid::regprocedure::text, ';')
    FROM pg_proc
    WHERE proname = var1 AND pg_function_is_visible(oid)
    INTO query, temp;

    -- Control statements
    IF var1 IS NULL THEN
        SELECT _var3 INTO _var_4;
    END IF;

    if ("var1" < 0) then
        return '$$';
    elseif (var1 = 0) then
        return '';
    end if;

    BEGIN
        IF NOT EXISTS (SELECT NULL FROM "MyTable6") THEN
            RETURN QUERY SELECT 0;
            RETURN;
        END IF;

        counter = 0;
        WHILE TRUE LOOP
            counter := counter + 1;

            IF counter > var2 THEN
                RETURN QUERY SELECT 1;
                RETURN;
            END IF;

            PERFORM pg_sleep(_var_3 / 1000.0);
        END LOOP;

        IF NOT FOUND THEN
            RAISE SQLSTATE 'XX12345';
        END IF;

        EXCEPTION
            WHEN SQLSTATE '55P03' THEN
                RETURN QUERY SELECT 1;
                RETURN;
    END;

    IF count > 0 THEN
        EXECUTE query;
    END IF;

    -- WITH statements
    return query with
    "WithRes" as (
        SELECT f.c1, query."c1"
        FROM (
            SELECT DISTINCT t.c1
            FROM "MyView4" t
            INNER JOIN "MyView5" t2 ON t2."_c2" = t._c2
            WHERE t."c1" = '$123'
                AND EXISTS (SELECT 1 FROM "MyView6")
            ) query
        CROSS JOIN LATERAL (SELECT c1 FROM _some_func(query."UserID") limit 1) f
    ),
    "WithRes2" as (
        select * from generate_series(2,4)
    )
    SELECT DISTINCT
        temp.c1 as "Res"
    from "WithRes" temp
    INNER JOIN "WithRes2" query ON query."c1" = temp.c2;

    "var1" := (
        WITH RECURSIVE "WithRes" AS (
            SELECT 1, 1
            UNION ALL
            SELECT 2, t.c1
            FROM "MyTable7"
            JOIN "MyTable8" ON "MyTable8"."c1" = "MyTable7"."c1"
        )
        SELECT "ID" FROM "WithRes" WHERE "WithRes"."c1" = "c2"
    );

    WITH _with_res as
    (
        UPDATE MyTable9 as t SET
            "c1" = t2."c1"
        FROM MyTable10 t2
        WHERE t2._c2 = t."_c2"
            RETURNING
                t."c1" as Col1
    )
    INSERT INTO _some_table
    SELECT Col1 FROM _with_res;

    -- Multilevel subqueries and cases
    RETURN QUERY SELECT
        "t".*,
        CASE
            WHEN ("t"."c1" < 0) THEN
                CASE WHEN ("t"."c2" < (-2 * "var2"))
                    THEN '44'
                    ELSE CAST(abs("t"."c2") * 22 AS text)
                END
            ELSE ''
        END AS "c1"
    FROM (
        SELECT
            "tt"."c1" AS c1,
            ff.c1::int8 AS "c2"
        FROM (
                SELECT
                    "t"."c1"
                FROM "MyTable11" AS "u"
                WHERE TRUE is TRUE
                ORDER BY "t"."c1" desc
                OFFSET "var1" - 1 LIMIT "var2"
            ) AS "tt"
            LEFT JOIN LATERAL (
                SELECT "u"."c1"
                FROM MyTable12 AS "u"
                WHERE "u"."c1" = "t"."c1"
                LIMIT 1
            ) AS "ff" ON true
    ) AS "t";

    -- Delete using
    DELETE FROM "MyTable13" as del
    USING
        MyTable14 t1,
        "MyTable15" t2
    WHERE t1."c1" is false
        AND t2."c1" != del."c1";

    -- Cursor operations
    OPEN "MyCursor";
    FETCH NEXT FROM "MyCursor" INTO query, "temp";
    WHILE FOUND
    loop
        FETCH NEXT FROM "MyCursor" INTO "query", temp;
    end loop;
    CLOSE "MyCursor";

    -- Exception handler
    EXCEPTION WHEN others THEN
        RETURN FALSE;

END;
$FuncBody$