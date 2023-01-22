using System.Collections.Generic;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.MSSQL;
using static DotNetDBTools.Generation.Core.Definition.CSharpDefinitionGenerationHelper;

namespace DotNetDBTools.Generation.Core.Definition;

internal class MSSQLTablesCSharpDefinitionGenerator : TablesCSharpDefinitionGenerator
{
    protected override void AddAdditionalColumnPropsDeclarations(List<string> propsDeclarations, Column column)
    {
        MSSQLColumn c = (MSSQLColumn)column;
        if (c.DefaultConstraintName is not null)
            propsDeclarations.Add($@"            DefaultConstraintName = {DeclareString(c.DefaultConstraintName)},");
    }
}
