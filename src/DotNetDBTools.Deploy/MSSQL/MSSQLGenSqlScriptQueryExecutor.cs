using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using DotNetDBTools.Deploy.Core;

namespace DotNetDBTools.Deploy.MSSQL
{
    public class MSSQLGenSqlScriptQueryExecutor : IQueryExecutor
    {
        private int _executeQueriesCount = 0;
        private readonly List<string> _queries = new();

        public int Execute(IQuery query)
        {
            string queryName = query.GetType().Name;
            string paremeterDeclarations = GetParameterDeclarations(query);
            string queryWithParameterDeclarations = $"{paremeterDeclarations}{query.Sql}";
            string execQueryStatement = $"EXEC sp_executesql N'{queryWithParameterDeclarations.Replace("'", "''")}';";
            _queries.Add($"--QUERY START: {queryName}\n{execQueryStatement}\n--QUERY END: {queryName}");
            _executeQueriesCount++;
            return 0;
        }

        public void BeginTransaction()
        {
            _queries.Add(
@"SET NOCOUNT ON;
SET XACT_ABORT ON;
BEGIN TRY;
    BEGIN TRANSACTION;");
        }

        public void CommitTransaction()
        {
            _queries.Add(
@"    COMMIT TRANSACTION;
END TRY
BEGIN CATCH;
    ROLLBACK TRANSACTION;

    DECLARE @ErrorMessage NVARCHAR(MAX), @ErrorSeverity INT, @ErrorState INT;
    SELECT @ErrorMessage = ERROR_MESSAGE() + ' Line ' + CAST(ERROR_LINE() AS NVARCHAR(5)), @ErrorSeverity = ERROR_SEVERITY(), @ErrorState = ERROR_STATE();
    RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
END CATCH;");
        }

        public void RollbackTransaction()
        {
        }

        public IEnumerable<TOut> Query<TOut>(IQuery query)
        {
            throw new NotImplementedException();
        }

        public TOut QuerySingleOrDefault<TOut>(IQuery query)
        {
            throw new NotImplementedException();
        }

        public string GetFinalScript()
        {
            if (_executeQueriesCount == 0)
                return "";
            return string.Join("\n\n", _queries);
        }

        private string GetParameterDeclarations(IQuery query)
        {
            return string.Join("", query.Parameters.Select(x => $"DECLARE {x.Name} {GetSqlType(x)} = {Quote(x)};\n"));
        }

        private string GetSqlType(QueryParameter queryParameter)
        {
            return queryParameter.Type switch
            {
                DbType.String => "NVARCHAR(MAX)",
                DbType.Guid => "UNIQUEIDENTIFIER",
                _ => throw new InvalidOperationException($"Invalid query parameter type: '{queryParameter.Type}'")
            };
        }

        private string Quote(QueryParameter queryParameter)
        {
            if (queryParameter.Value is null)
                return "NULL";
            return queryParameter.Type switch
            {
                DbType.String or DbType.Guid => $"'{queryParameter.Value}'",
                _ => throw new InvalidOperationException($"Invalid query parameter type: '{queryParameter.Type}'")
            };
        }
    }
}
