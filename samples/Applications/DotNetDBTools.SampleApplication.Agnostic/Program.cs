using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using Microsoft.Data.Sqlite;
using SqlKata;
using SqlKata.Compilers;
using SqlKata.Execution;
using static SampleDBDescription.SampleDBTables;

namespace DotNetDBTools.SampleApplication.Agnostic
{
    public class Program
    {
        private const string RepoRoot = "../../../../../..";

        private static readonly string s_agnosticSampleDbFilePath = $"{RepoRoot}/samples/Databases/DotNetDBTools.SampleDB.Agnostic/bin/DbFile/AgnosticSampleDB.db";
        private static readonly string s_connectionString = $"DataSource={s_agnosticSampleDbFilePath};Mode=ReadWriteCreate;";

        public static void Main()
        {
            InsertSomeData();
            ReadSomeData();
            InsertSomeMoreData();
            ReadSomeMoreData();
        }

        private static void InsertSomeData()
        {
            string query =
$@"INSERT INTO {MyTable2}
(
    {MyTable2.MyColumn1},
    {MyTable2.MyColumn2}
)
VALUES
(
    'key1',
    'table2value1'
),
(
    'key2',
    'table2value2'
);

INSERT INTO {MyTable1}
(
    {MyTable1.MyColumn1},
    {MyTable1.MyColumn2}
)
VALUES
(
    'key1',
    NULL
),
(
    'key2',
    NULL
);";

            SqliteConnection connection = new(s_connectionString);
            connection.Execute(query);
        }

        private static void ReadSomeData()
        {
            string query =
$@"SELECT
    COUNT(*)
FROM {MyTable1} t1
INNER JOIN {MyTable2} t2
    ON t1.{MyTable1.MyColumn1} = t2.{MyTable2.MyColumn1}
WHERE t1.{MyTable1.MyColumn2} IS NULL
    AND t2.{MyTable2.MyColumn2} IN ('table2value1', 'table2value2');";

            SqliteConnection connection = new(s_connectionString);
            int count = connection.QuerySingle<int>(query);
            if (count != 2)
                throw new Exception($"Wrong data in '{MyTable1}' or '{MyTable2}' table");
        }

        private static void InsertSomeMoreData()
        {
            SqliteConnection connection = new(s_connectionString);
            SqliteCompiler compiler = new();

            Query query = new Query(MyTable2)
                .AsInsert(
                    new[] { MyTable2.MyColumn1, MyTable2.MyColumn2 },
                    new[]
                    {
                        new[] { "key3", "table2value3" },
                        new[] { "key4", "table2value4" },
                    });

            SqlResult sqlCompilationResult = compiler.Compile(query);
            string sql = sqlCompilationResult.Sql;
            Dictionary<string, object> bindings = sqlCompilationResult.NamedBindings;

            connection.Execute(sql, bindings);
        }

        private static void ReadSomeMoreData()
        {
            SqliteConnection connection = new(s_connectionString);
            SqliteCompiler compiler = new();
            QueryFactory db = new(connection, compiler);

            List<string> queryResults = db.Query(MyTable2)
                .Select(new[] { MyTable2.MyColumn2 })
                .WhereIn(MyTable2.MyColumn1, new[] { "key3", "key4" })
                .OrderBy(new[] { MyTable2.MyColumn1 })
                .Get<string>()
                .ToList();

            if (queryResults.Count != 2 ||
                queryResults[0] != "table2value3" ||
                queryResults[1] != "table2value4")
            {
                throw new Exception($"Wrong data in '{MyTable2}' table");
            }
        }
    }
}
