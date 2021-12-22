﻿using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.Core
{
    internal static class DbObjectsExtensions
    {
        public static string GetExtraInfo(this Column column)
        {
            return (column.Default as DefaultValueAsFunction)?.FunctionText;
        }

        public static string GetExtraInfo(this CheckConstraint ck)
        {
            return ck.Code;
        }
    }
}
