using System;
using DotNetDBTools.Definition.Agnostic;

namespace DotNetDBTools.SampleDB.Agnostic
{
    public static class StringExtensions
    {
        public static string Quote(this string identifier, DbmsKind dbmsKind) =>
            dbmsKind switch
            {
                DbmsKind.MSSQL => $@"[{identifier}]",
                DbmsKind.MySQL => $@"`{identifier}`",
                DbmsKind.PostgreSQL => $@"""{identifier}""",
                DbmsKind.SQLite => $@"[{identifier}]",
                _ => throw new InvalidOperationException($"Unsupported dbmsKind for idintifier quoting: {dbmsKind}")
            };
    }
}
