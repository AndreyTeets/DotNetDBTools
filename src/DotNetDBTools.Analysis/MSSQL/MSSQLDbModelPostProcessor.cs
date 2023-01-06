using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using DotNetDBTools.Analysis.Core;
using DotNetDBTools.Analysis.Extensions;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.MSSQL;
using DotNetDBTools.Models.MSSQL.UserDefinedTypes;

namespace DotNetDBTools.Analysis.MSSQL;

internal class MSSQLDbModelPostProcessor : DbModelPostProcessor
{
    protected override void PostProcessDataTypes(Database database)
    {
        MSSQLDatabase db = (MSSQLDatabase)database;
        IEnumerable<DbObject> userDefinedTypes =
            db.UserDefinedTypes.Select(x => (DbObject)x);

        HashSet<string> userDefinedTypesNames = new();
        foreach (DbObject udt in userDefinedTypes)
            userDefinedTypesNames.Add(udt.Name);

        foreach (Table table in db.Tables)
        {
            foreach (Column column in table.Columns)
            {
                PostProcessDataType(column.DataType);
            }
        }
        foreach (MSSQLUserDefinedType type in db.UserDefinedTypes)
        {
            PostProcessDataType(type.UnderlyingType);
        }

        void PostProcessDataType(DataType dataType)
        {
            dataType.Name = Regex.Replace(dataType.Name, @"\s", "");
            if (!userDefinedTypesNames.Contains(dataType.Name) && IsStandardSqlType(dataType.Name))
                dataType.Name = dataType.Name.ToUpper();
            else if (userDefinedTypesNames.Contains(dataType.Name))
                dataType.IsUserDefined = true;
            else
                throw new Exception($"Unknown data type '{dataType.Name}'");

            static bool IsStandardSqlType(string typeName)
            {
                IEnumerable<string> standardSqlTypeNamesBases = typeof(MSSQLDataTypeNames).GetFields().Select(x => x.Name);
                return standardSqlTypeNamesBases.Any(typeNameBase => TypeNameMatchesItsBase(typeName, typeNameBase));

                static bool TypeNameMatchesItsBase(string typeName, string typeNameBase)
                {
                    return Regex.IsMatch(typeName.ToUpper(), $@"^{typeNameBase}\s*(?:[\(][\s\d\,]+[\)])?$");
                }
            }
        }
    }

    protected override void OrderAdditionalDbObjects(Database database)
    {
        MSSQLDatabase db = (MSSQLDatabase)database;
        db.UserDefinedTypes = db.UserDefinedTypes.OrderByName();
        db.UserDefinedTableTypes = db.UserDefinedTableTypes.OrderByName();
        db.Functions = db.Functions.OrderByName();
        db.Procedures = db.Procedures.OrderByName();
    }
}
