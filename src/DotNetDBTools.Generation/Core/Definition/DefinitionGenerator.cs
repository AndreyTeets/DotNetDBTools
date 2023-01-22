using System.Collections.Generic;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Generation.Core.Definition;

internal abstract class DefinitionGenerator<TTablesCSharpDefinitionGenerator> : IDefinitionGenerator
    where TTablesCSharpDefinitionGenerator : TablesCSharpDefinitionGenerator, new()
{
    public OutputDefinitionKind OutputDefinitionKind { get; set; }

    public IEnumerable<DefinitionSourceFile> GenerateDefinition(Database database, string projectNamespace)
    {
        List<DefinitionSourceFile> files = new();

        string declaredDefitionKind = OutputDefinitionKind == OutputDefinitionKind.CSharp
            ? OutputDefinitionKind.CSharp.ToString()
            : database.Kind.ToString();
        files.AddRange(CommonDefinitionProjectFilesCreator.Create(
            projectNamespace, OutputDefinitionKind, declaredDefitionKind));

        if (OutputDefinitionKind == OutputDefinitionKind.CSharp)
        {
            files.AddRange(new TTablesCSharpDefinitionGenerator().Create(database, projectNamespace));
            files.AddRange(ViewsCSharpDefinitionGenerator.Create(database, projectNamespace));
        }
        else
        {
            foreach (Table table in database.Tables)
                AddFile(files, table, "Tables");
            foreach (View view in database.Views)
                AddFile(files, view, "Views");
            foreach (Table table in database.Tables)
            {
                foreach (Index index in table.Indexes)
                    AddFile(files, index, "Indexes");
            }
            foreach (Table table in database.Tables)
            {
                foreach (Trigger trigger in table.Triggers)
                    AddFile(files, trigger, "Triggers");
            }
        }
        AddDbmsSpecificObjectsFiles(files, database, projectNamespace);

        return files;
    }
    protected abstract void AddDbmsSpecificObjectsFiles(List<DefinitionSourceFile> files, Database database, string projectNamespace);

    protected void AddFile(List<DefinitionSourceFile> files, DbObject dbObject, string folder)
    {
        DefinitionSourceFile sqlFile = new()
        {
            RelativePath = $"{folder}/{dbObject.Name}.sql",
            SourceText = GenerationManager.GenerateSqlCreateStatement(dbObject, true),
        };
        files.Add(sqlFile);
    }
}
