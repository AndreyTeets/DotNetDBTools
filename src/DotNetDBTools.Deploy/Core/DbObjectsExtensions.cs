using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.Core;

internal static class DbObjectsExtensions
{
    public static string GetCode(this Column column) => column.Default.Code;
    public static string GetCode(this CheckConstraint ck) => ck.CodePiece.Code;
    public static string GetCode(this Trigger trg) => trg.CodePiece.Code;
    public static string GetCode(this View view) => view.CodePiece.Code;
    public static string GetCode(this Script script) => script.CodePiece.Code;

    public static string GetName(this IQuery query) => query.GetType().Name;
    public static string GetDisplayText(this IQuery query)
    {
        string queryName = query.GetName();
        string querySqlText = query.Sql;
        string queryParametersText = GetParametersDisplayText(query.Parameters);
        string res = $"QueryName: {queryName}\nQuerySql:\n{querySqlText}";
        if (!string.IsNullOrEmpty(queryParametersText))
            res += $"\nQueryParameters:\n{queryParametersText}";
        return res;

        static string GetParametersDisplayText(IEnumerable<QueryParameter> parameters)
        {
            return string.Join("\n", parameters.Select(x => $"{x.Name} {x.Type} = {x.Value}"));
        }
    }
}
