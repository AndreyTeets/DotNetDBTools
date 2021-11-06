using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using Dapper;
using SqlKata;
using SqlKata.Compilers;
using SqlKata.Execution;
using static DotNetDBToolsSampleDBv2AgnosticDescription.DotNetDBToolsSampleDBv2AgnosticTables;

namespace DotNetDBTools.SampleBusinessLogicLib.Agnostic
{
    public static class SampleBusinessLogic
    {
        public static void ReadWriteSomeData(DbConnection connection, Compiler compiler)
        {
            InsertSomeData(connection, compiler);
            ReadSomeData(connection, compiler);
        }

        private static void InsertSomeData(DbConnection connection, Compiler compiler)
        {
            Query query = new Query(MyTable3)
                .AsInsert(
                    new[] { MyTable3.MyColumn1, MyTable3.MyColumn2 },
                    new[]
                    {
                        new object[] { 3, Encoding.UTF8.GetBytes("value3") },
                        new object[] { 4, Encoding.UTF8.GetBytes("value4") },
                    });

            SqlResult sqlCompilationResult = compiler.Compile(query);
            string sql = sqlCompilationResult.Sql;
            Dictionary<string, object> bindings = sqlCompilationResult.NamedBindings;

            connection.Execute(sql, bindings);
        }

        private static void ReadSomeData(DbConnection connection, Compiler compiler)
        {
            QueryFactory db = new(connection, compiler);

            List<byte[]> queryResults = db.Query(MyTable3)
                .Select(new[] { MyTable3.MyColumn2 })
                .WhereIn(MyTable3.MyColumn1, new[] { 3, 4 })
                .OrderBy(new[] { MyTable3.MyColumn1 })
                .Get<byte[]>()
                .ToList();

            if (queryResults.Count != 2 ||
                Encoding.UTF8.GetString(queryResults[0]) != "value3" ||
                Encoding.UTF8.GetString(queryResults[1]) != "value4")
            {
                throw new Exception($"Wrong data in '{MyTable3}' table");
            }
        }
    }
}
