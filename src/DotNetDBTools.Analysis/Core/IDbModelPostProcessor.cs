﻿using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Analysis.Core;

internal interface IDbModelPostProcessor
{
    public void DoSpecificDbmsDbModelCreationFromDefinitionPostProcessing(Database database);
    public void DoPostProcessing(Database database);
}
