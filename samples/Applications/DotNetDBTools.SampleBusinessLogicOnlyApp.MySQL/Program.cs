using System;
using Dapper;
using MySqlConnector;
using static DotNetDBToolsSampleDBMySQLDescription.DotNetDBToolsSampleDBMySQLTables;

namespace DotNetDBTools.SampleBusinessLogicOnlyApp.MySQL
{
    public static class Program
    {
        private const string MySQLServerPassword = "Strong(!)Passw0rd";
        private const string MySQLServerHostPort = "5006";
        private const string DatabaseName = "AgnosticSampleDB";

        private static readonly string s_connectionString = $"Host=localhost;Port={MySQLServerHostPort};Database={DatabaseName};Username=root;Password={MySQLServerPassword}";

        public static void Main()
        {
            InsertSomeData();
            ReadSomeData();
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
    '\x76616C756534'
);

INSERT INTO {MyTable1}
(
    {MyTable1.MyColumn1},
    {MyTable1.MyColumn2}
)
VALUES
(
    1,
    N'value1'
),
(
    2,
    N'value2'
);";

            using MySqlConnection connection = new(s_connectionString);
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
WHERE t1.{MyTable1.MyColumn2} IN (N'value1', N'value2')
    AND t2.{MyTable2.MyColumn2} IS NULL;";

            using MySqlConnection connection = new(s_connectionString);
            int count = connection.QuerySingle<int>(query);
            if (count != 1)
                throw new Exception($"Wrong data in '{MyTable1}' or '{MyTable2}' table");
        }
    }
}
