using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using SqlKata;
using SqlKata.Compilers;
using SqlKata.Execution;
using static DotNetDBToolsSampleDBv2AgnosticDescription.DotNetDBToolsSampleDBv2AgnosticTables;

namespace DotNetDBTools.SampleBusinessLogicLib.Agnostic
{
    public static class SampleBusinessLogic
    {
        public static void ReadWriteSomeData(IDbConnection connection, Compiler compiler)
        {
            DropExistingDataIfAny(connection, compiler);
            InsertSomeData(connection, compiler);
            ReadSomeData(connection, compiler);
        }

        private static void DropExistingDataIfAny(IDbConnection connection, Compiler compiler)
        {
            Query query = new Query(MyTable3).AsDelete();
            SqlResult sqlCompilationResult = compiler.Compile(query);
            string sql = sqlCompilationResult.Sql;
            connection.Execute(sql);
        }

        private static void InsertSomeData(IDbConnection connection, Compiler compiler)
        {
            Query query = new Query(MyTable3)
                .AsInsert(
                    new[] { MyTable3.MyColumn1, MyTable3.MyColumn2 },
                    new[]
                    {
                        new object[] { 3, "value3" },
                        new object[] { 4, "value4" },
                    });

            SqlResult sqlCompilationResult = compiler.Compile(query);
            string sql = sqlCompilationResult.Sql;
            Dictionary<string, object> bindings = sqlCompilationResult.NamedBindings;

            connection.Execute(sql, bindings);
        }

        private static void ReadSomeData(IDbConnection connection, Compiler compiler)
        {
            QueryFactory db = new(connection, compiler);

            List<string> queryResults = db.Query(MyTable3)
                .Select(new[] { MyTable3.MyColumn2 })
                .WhereIn(MyTable3.MyColumn1, new[] { 3, 4 })
                .OrderBy(new[] { MyTable3.MyColumn1 })
                .Get<string>()
                .ToList();

            if (queryResults.Count != 2 ||
                queryResults[0] != "value3" ||
                queryResults[1] != "value4")
            {
                throw new Exception($"Wrong data in '{MyTable3}' table");
            }
        }
    }
}
