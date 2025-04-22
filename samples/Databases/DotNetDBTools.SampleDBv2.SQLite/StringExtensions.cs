using System;
using System.IO;
using System.Reflection;

namespace DotNetDBTools.SampleDB.SQLite
{
    public static class StringExtensions
    {
        public static string Quote(this string identifier)
        {
            return $@"[{identifier}]";
        }

        public static string AsSqlResource(this string sqlResource)
        {
            return GetEmbeddedResourceAsString($"Sql.{sqlResource}");
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
