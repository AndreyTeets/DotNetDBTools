using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.MSSQL;

namespace DotNetDBTools.Analysis.MSSQL;

internal static class MSSQLHelperMethods
{
    public static HashSet<string> GetUserDefinedTypesName(Database database)
    {
        MSSQLDatabase db = (MSSQLDatabase)database;
        IEnumerable<DbObject> userDefinedTypes =
            db.UserDefinedTypes.Select(x => (DbObject)x);

        HashSet<string> userDefinedTypesNames = new();
        foreach (DbObject udt in userDefinedTypes)
            userDefinedTypesNames.Add(udt.Name);
        return userDefinedTypesNames;
    }

    public static bool IsStandardSqlType(string typeName)
    {
        IEnumerable<string> standardSqlTypeNamesBases = typeof(MSSQLDataTypeNames).GetFields().Select(x => x.Name);
        return standardSqlTypeNamesBases.Any(typeNameBase => TypeNameMatchesItsBase(typeName, typeNameBase));

        static bool TypeNameMatchesItsBase(string typeName, string typeNameBase)
        {
            return Regex.IsMatch(typeName.ToUpper(), $@"^{typeNameBase}\s*(?:[\(](?:[\s\d\,]+|MAX)[\)])?$");
        }
    }
}
