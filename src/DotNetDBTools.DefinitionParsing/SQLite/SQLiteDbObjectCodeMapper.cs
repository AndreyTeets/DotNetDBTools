using System;
using DotNetDBTools.Definition.Core;
using DotNetDBTools.DefinitionParsing.Core;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.DefinitionParsing.SQLite
{
    internal class SQLiteDbObjectCodeMapper : IDbObjectCodeMapper
    {
        public CodePiece MapToCodePiece(IDbObject dbObject)
        {
            return dbObject switch
            {
                Definition.SQLite.CheckConstraint ck => new CodePiece { Code = ck.Code },
                _ => throw new InvalidOperationException($"Invalid dbObject for code mapping: {dbObject}")
            };
        }
    }
}
