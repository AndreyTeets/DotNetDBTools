using System;
using System.IO;
using System.Reflection;
using DotNetDBTools.Definition.Agnostic;

namespace DotNetDBTools.SampleDB.Agnostic
{
    public static class StringExtensions
    {
        public static string Quote(this string identifier, DbmsKind dbmsKind)
        {
            return dbmsKind switch
            {
                DbmsKind.MSSQL => $@"[{identifier}]",
                DbmsKind.MySQL => $@"`{identifier}`",
                DbmsKind.PostgreSQL => $@"""{identifier}""",
                DbmsKind.SQLite => $@"[{identifier}]",
                _ => throw new InvalidOperationException($"Unsupported dbmsKind for idintifier quoting: {dbmsKind}")
            };
        }

        public static string AsSqlResource(this string sqlResource, DbmsKind dbmsKind)
        {
            return GetEmbeddedResourceAsString($"Sql.{dbmsKind}.{sqlResource}");
        }

        private static string GetEmbeddedResourceAsString(string resourceName)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            Stream stream = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.{resourceName}");
            if (stream is null)
                throw new Exception($"Failed to get embedded resource '{resourceName}'");
            using StreamReader sr = new(stream);
            return sr.ReadToEnd();
        }
    }
}
