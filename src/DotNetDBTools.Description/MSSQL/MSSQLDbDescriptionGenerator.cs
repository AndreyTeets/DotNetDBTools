﻿using DotNetDBTools.Description.Core;
using DotNetDBTools.Models.MSSQL;

namespace DotNetDBTools.Description.MSSQL
{
    public static class MSSQLDescriptionSourceGenerator
    {
        public static string GenerateDescription(MSSQLDatabaseInfo databaseInfo)
        {
            return TablesDescriptionGenerator.GenerateTablesDescription(databaseInfo);
        }
    }
}