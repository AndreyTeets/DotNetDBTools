using System;
using DotNetDBTools.Analysis.Core;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Analysis.Agnostic;

internal class AgnosticDbModelPostProcessor : DbModelPostProcessor
{
    public override void DoSpecificDbmsDbModelCreationFromDefinitionPostProcessing(Database database)
    {
        throw new NotImplementedException("Method should never be called on this class.");
    }
}
