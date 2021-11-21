using System;
using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.Analysis.Core.Errors;
using DotNetDBTools.Definition.Core;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DotNetDBTools.DefinitionAnalyzer
{
    internal static class InvalidDbObjectsFinder
    {
        public static Location GetInvalidDbObjectLocation(Compilation compilation, DbError dbError)
        {
            if (dbError is InvalidFKDbError invalidFKDbError)
                return GetInvalidFKLocation(compilation, invalidFKDbError);
            if (dbError is InvalidIdentityColumnDbError)
                return Location.None;
            else
                throw new InvalidOperationException($"Invalid dbError type: '{dbError.GetType()}'");
        }

        private static Location GetInvalidFKLocation(Compilation compilation, InvalidFKDbError invalidFKDbError)
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
                    if (!classSymbol.AllInterfaces.Any(x => x.Name == nameof(IBaseTable)))
                        continue;
                    if (classSymbol.Name != invalidFKDbError.TableName)
                        continue;

                    ISymbol foreignKeySymbol = classSymbol.GetMembers()
                        .Where(x => IsForeignKey(x))
                        .SingleOrDefault(x => x.Name == invalidFKDbError.ForeignKeyName);
                    if (foreignKeySymbol is null)
                        continue;

                    return foreignKeySymbol.Locations.First();
                }
            }
            return Location.None;
        }

        private static bool IsForeignKey(ISymbol symbol)
        {
            if (symbol is IFieldSymbol fieldSymbol &&
                fieldSymbol.Type.BaseType.Name == nameof(BaseForeignKey))
            {
                return true;
            }
            else if (symbol is IPropertySymbol propertySymbol &&
                propertySymbol.Type.BaseType.Name == nameof(BaseForeignKey))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
