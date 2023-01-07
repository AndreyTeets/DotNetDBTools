using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.PostgreSQL;

namespace DotNetDBTools.Analysis.PostgreSQL;

internal static class PostgreSQLHelperMethods
{
    public static HashSet<string> GetUserDefinedTypesName(Database database)
    {
        PostgreSQLDatabase db = (PostgreSQLDatabase)database;
        IEnumerable<DbObject> userDefinedTypes =
            db.CompositeTypes.Select(x => (DbObject)x)
            .Concat(db.DomainTypes.Select(x => (DbObject)x))
            .Concat(db.EnumTypes.Select(x => (DbObject)x))
            .Concat(db.RangeTypes.Select(x => (DbObject)x));

        HashSet<string> userDefinedTypesNames = new();
        foreach (DbObject udt in userDefinedTypes)
            userDefinedTypesNames.Add(udt.Name);
        return userDefinedTypesNames;
    }

    public static bool IsStandardSqlType(string typeName)
    {
        IEnumerable<string> standardSqlTypeNamesBases = typeof(PostgreSQLDataTypeNames).GetFields().Select(x => x.Name);
        return standardSqlTypeNamesBases.Any(typeNameBase => TypeNameMatchesItsBase(typeName, typeNameBase));

        static bool TypeNameMatchesItsBase(string typeName, string typeNameBase)
        {
            return Regex.IsMatch(typeName.ToUpper(), $@"^{typeNameBase}\s*(?:[\(][\s\d\,\-]+[\)])?\s*(?:[\[][\s\d]*[\]])*$");
        }
    }
}
