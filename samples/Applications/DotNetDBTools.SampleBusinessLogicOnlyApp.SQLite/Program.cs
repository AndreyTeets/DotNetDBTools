using System;
using Dapper;
using Microsoft.Data.Sqlite;
using static DotNetDBToolsSampleDBSQLiteDescription.DotNetDBToolsSampleDBSQLiteTables;

namespace DotNetDBTools.SampleBusinessLogicOnlyApp.SQLite
{
    public static class Program
    {
        private const string RepoRoot = "../../../../../..";
        private static readonly string s_samplesOutputDir = $"{RepoRoot}/SamplesOutput";

        private static readonly string s_connectionString = $"DataSource={s_samplesOutputDir}/sqlite_databases/AgnosticSampleDB.db;Mode=ReadWriteCreate;";

        public static void Main()
        {
            DropExistingDataIfAny();
            InsertSomeData();
            ReadSomeData();
        }

        private static void DropExistingDataIfAny()
        {
            string query =
$@"DELETE FROM {MyTable1};
DELETE FROM {MyTable2};";

            using SqliteConnection connection = new(s_connectionString);
            connection.Execute(query);
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
    1,
    NULL
),
(
    2,
    0x76616C756534
);

INSERT INTO {MyTable1}
(
    {MyTable1.MyColumn1},
    {MyTable1.MyColumn2}
)
VALUES
(
    1,
    'value1'
),
(
    2,
    'value2'
);";

            using SqliteConnection connection = new(s_connectionString);
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
WHERE t1.{MyTable1.MyColumn2} IN ('value1', 'value2')
    AND t2.{MyTable2.MyColumn2} IS NULL;";

            using SqliteConnection connection = new(s_connectionString);
            int count = connection.QuerySingle<int>(query);
            if (count != 1)
                throw new Exception($"Wrong data in '{MyTable1}' or '{MyTable2}' table");
        }
    }
}
