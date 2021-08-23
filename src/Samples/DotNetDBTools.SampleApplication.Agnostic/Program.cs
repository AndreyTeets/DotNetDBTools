using System;
using Dapper;
using Microsoft.Data.Sqlite;
using SampleDBDescription;

namespace DotNetDBTools.SampleApplication.Agnostic
{
    public class Program
    {
        private const string AgnosticDbProjectBinDir = "../../../../../Samples/DotNetDBTools.SampleDB.Agnostic/bin";
        private static readonly string s_dbFilePath = $"{AgnosticDbProjectBinDir}/DbFile/AgnosticSampleDB.db";
        private static readonly string s_connectionString = $"DataSource={s_dbFilePath};Mode=ReadWriteCreate;";

        public static void Main()
        {
            InsertSomeData();
            ReadSomeData();
        }

        private static void InsertSomeData()
        {
            string query =
$@"INSERT INTO {SampleDBTables.MyTable2}
(
    {SampleDBTables.MyTable2.MyColumn1},
    {SampleDBTables.MyTable2.MyColumn2}
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

INSERT INTO {SampleDBTables.MyTable1}
(
    {SampleDBTables.MyTable1.MyColumn1},
    {SampleDBTables.MyTable1.MyColumn2}
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
FROM {SampleDBTables.MyTable1} t1
INNER JOIN {SampleDBTables.MyTable2} t2
    ON t1.{SampleDBTables.MyTable1.MyColumn1} = t2.{SampleDBTables.MyTable2.MyColumn1}
WHERE t1.{SampleDBTables.MyTable1.MyColumn2} IS NULL
    AND t2.{SampleDBTables.MyTable2.MyColumn2} IN ('table2value1', 'table2value2');";

            SqliteConnection connection = new(s_connectionString);
            int count = connection.QuerySingle<int>(query);
            if (count != 2)
                throw new Exception($"Wrong data in '{SampleDBTables.MyTable1}' or '{SampleDBTables.MyTable2}' table");
        }
    }
}
