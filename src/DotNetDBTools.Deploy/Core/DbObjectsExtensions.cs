using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.Core
{
    internal static class DbObjectsExtensions
    {
        public static string GetCode(this Column column)
        {
            return (column.Default as CodePiece)?.Code;
        }

        public static string GetCode(this CheckConstraint ck)
        {
            return ck.CodePiece.Code;
        }

        public static string GetCode(this View view)
        {
            return view.CodePiece.Code;
        }
    }
}
