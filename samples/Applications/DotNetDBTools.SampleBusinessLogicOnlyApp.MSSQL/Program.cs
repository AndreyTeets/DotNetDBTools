﻿using System;
using System.Data.SqlClient;
using Dapper;
using static SampleDBDescription.SampleDBTables;

namespace DotNetDBTools.SampleBusinessLogicOnlyApp.MSSQL
{
    public static class Program
    {
        private const string MsSqlServerPassword = "Strong(!)Passw0rd";
        private const string MsSqlServerHostPort = "5005";
        private const string DatabaseName = "AgnosticSampleDB";

        private static readonly string s_connectionString = $"Data Source=localhost,{MsSqlServerHostPort};Initial Catalog={DatabaseName};Integrated Security=False;User ID=SA;Password={MsSqlServerPassword}";

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
    N'value1'
),
(
    2,
    N'value2'
);";

            SqlConnection connection = new(s_connectionString);
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

            SqlConnection connection = new(s_connectionString);
            int count = connection.QuerySingle<int>(query);
            if (count != 1)
                throw new Exception($"Wrong data in '{MyTable1}' or '{MyTable2}' table");
        }
    }
}