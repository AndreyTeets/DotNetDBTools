using System;
using System.Data;
using System.Linq;
using DotNetDBTools.Deploy.Core;

namespace DotNetDBTools.Deploy.MSSQL;

internal class MSSQLGenSqlScriptQueryExecutor : GenSqlScriptQueryExecutor
{
    protected override string CreateQueryText(IQuery query)
    {
        string paremeterDeclarations = GetParameterDeclarations(query);
        string queryWithParameterDeclarations = $"{paremeterDeclarations}{query.Sql}";
        string execQueryStatement = $"EXEC sp_executesql N'{queryWithParameterDeclarations.Replace("'", "''")}';";
        return execQueryStatement;
    }

    protected override string CreateBeginTransactionText()
    {
        return
@"SET NOCOUNT ON;
SET XACT_ABORT ON;
BEGIN TRY;
    BEGIN TRANSACTION;";
    }

    protected override string CreateCommitTransactionText()
    {
        return
@"    COMMIT TRANSACTION;
END TRY
BEGIN CATCH;
    ROLLBACK TRANSACTION;

    DECLARE @ErrorMessage NVARCHAR(MAX), @ErrorSeverity INT, @ErrorState INT;
    SELECT @ErrorMessage = ERROR_MESSAGE() + ' Line ' + CAST(ERROR_LINE() AS NVARCHAR(5)), @ErrorSeverity = ERROR_SEVERITY(), @ErrorState = ERROR_STATE();
    RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
END CATCH;";
    }

    private static string GetParameterDeclarations(IQuery query)
    {
        return string.Join("", query.Parameters.Select(x => $"DECLARE {x.Name} {GetSqlType(x)} = {Quote(x)};\n"));
    }

    private static string GetSqlType(QueryParameter queryParameter)
    {
        return queryParameter.Type switch
        {
            DbType.String => "NVARCHAR(MAX)",
            DbType.Guid => "UNIQUEIDENTIFIER",
            DbType.Int64 => "BIGINT",
            _ => throw new InvalidOperationException($"Invalid query parameter type: '{queryParameter.Type}'")
        };
    }

    private static string Quote(QueryParameter queryParameter)
    {
        if (queryParameter.Value is null)
            return "NULL";
        return queryParameter.Type switch
        {
            DbType.String or DbType.Guid => $"N'{queryParameter.Value.ToString().Replace("'", "''")}'",
            DbType.Int64 => $"{queryParameter.Value}",
            _ => throw new InvalidOperationException($"Invalid query parameter type: '{queryParameter.Type}'")
        };
    }
}
