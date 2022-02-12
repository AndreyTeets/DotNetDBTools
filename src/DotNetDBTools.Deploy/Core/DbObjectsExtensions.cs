using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.Core;

internal static class DbObjectsExtensions
{
    public static string GetCode(this Column column) => (column.Default as CodePiece)?.Code;
    public static string GetCode(this CheckConstraint ck) => ck.CodePiece.Code;
    public static string GetCode(this Trigger trg) => trg.CodePiece.Code;
    public static string GetCode(this View view) => view.CodePiece.Code;
}
