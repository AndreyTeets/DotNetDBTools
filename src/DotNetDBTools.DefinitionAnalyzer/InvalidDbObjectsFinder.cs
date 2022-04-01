using System;
using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.Analysis.Core.Errors;
using DotNetDBTools.Definition.Core;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DotNetDBTools.DefinitionAnalyzer;

internal static class InvalidDbObjectsFinder
{
    public static Location GetInvalidDbObjectLocation(Compilation compilation, DbError dbError)
    {
        return dbError switch
        {
            ColumnDbError err => GetClassMemberLocation(compilation, err.TableName, err.ColumnName, "Column"),
            ForeignKeyDbError err => GetClassMemberLocation(compilation, err.TableName, err.ForeignKeyName, "ForeignKey"),
            TriggerDbError err => GetClassMemberLocation(compilation, err.TableName, err.TriggerName, "Trigger"),
            _ => throw new InvalidOperationException($"Invalid dbError type: '{dbError.GetType()}'"),
        };
    }

    private static Location GetClassMemberLocation(
        Compilation compilation,
        string tableName,
        string memberName,
        string memberTypeName)
    {
        foreach (SyntaxTree st in compilation.SyntaxTrees)
        {
            SemanticModel semanticModel = compilation.GetSemanticModel(st);
            IEnumerable<ClassDeclarationSyntax> classes = st.GetRoot()
                .DescendantNodes()
                .OfType<ClassDeclarationSyntax>();

            foreach (ClassDeclarationSyntax c in classes)
            {
                ITypeSymbol classSymbol = (ITypeSymbol)semanticModel.GetDeclaredSymbol(c);
                if (classSymbol.Name == tableName &&
                    classSymbol.AllInterfaces.Any(x => x.Name == nameof(IBaseTable)))
                {
                    ISymbol classMemberSymbol = classSymbol.GetMembers().SingleOrDefault(x =>
                        x.Name == memberName &&
                        IsOfBaseType(x, memberTypeName));

                    if (classMemberSymbol is not null)
                        return classMemberSymbol.Locations.First();
                }
            }
        }
        return Location.None;
    }

    private static bool IsOfBaseType(ISymbol symbol, string typeName)
    {
        if (symbol is IFieldSymbol fieldSymbol &&
            fieldSymbol.Type.Name == typeName)
        {
            return true;
        }
        else if (symbol is IPropertySymbol propertySymbol &&
            propertySymbol.Type.Name == typeName)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
