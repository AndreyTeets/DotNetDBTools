using System.Collections.Generic;
using DotNetDBTools.Generation.Core.Definition;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.PostgreSQL;
using DotNetDBTools.Models.PostgreSQL.UserDefinedTypes;

namespace DotNetDBTools.Generation.PostgreSQL.Definition;

internal class PostgreSQLDefinitionGenerator : DefinitionGenerator
{
    protected override void AddDbmsSpecificObjectsFiles(
        List<DefinitionSourceFile> files, Database database, string projectNamespace)
    {
        PostgreSQLDatabase db = (PostgreSQLDatabase)database;
        if (OutputDefinitionKind == OutputDefinitionKind.CSharp)
        {
            files.AddRange(PostgreSQLSequencesCSharpDefinitionGenerator.Create(database, projectNamespace));
            files.AddRange(PostgreSQLTypesCSharpDefinitionGenerator.Create(database, projectNamespace));
            files.AddRange(PostgreSQLFunctionsCSharpDefinitionGenerator.Create(database, projectNamespace));
        }
        else
        {
            foreach (PostgreSQLSequence sequence in db.Sequences)
                AddFile(files, sequence, "Sequences");
            foreach (PostgreSQLCompositeType type in db.CompositeTypes)
                AddFile(files, type, "Types");
            foreach (PostgreSQLDomainType type in db.DomainTypes)
                AddFile(files, type, "Types");
            foreach (PostgreSQLEnumType type in db.EnumTypes)
                AddFile(files, type, "Types");
            foreach (PostgreSQLRangeType type in db.RangeTypes)
                AddFile(files, type, "Types");
            foreach (PostgreSQLFunction function in db.Functions)
                AddFile(files, function, "Functions");
        }
    }
}
