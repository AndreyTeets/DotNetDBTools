﻿using DotNetDBTools.Generation.Core;
using DotNetDBTools.Models.Agnostic;

namespace DotNetDBTools.Generation.Agnostic
{
    internal static class AgnosticDescriptionGenerator
    {
        public static string GenerateDescription(AgnosticDatabase database)
        {
            return TablesDescriptionGenerator.GenerateTablesDescription(database);
        }
    }
}
