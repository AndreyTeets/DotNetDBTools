using System;
using DotNetDBTools.Definition.Core;
using DotNetDBTools.DefinitionParsing.Core;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.DefinitionParsing.PostgreSQL
{
    internal class PostgreSQLDbObjectCodeMapper : IDbObjectCodeMapper
    {
        public CodePiece MapToCodePiece(IDbObject dbObject)
        {
            return dbObject switch
            {
                Definition.PostgreSQL.CheckConstraint ck => new CodePiece { Code = ck.Code },
                _ => throw new InvalidOperationException($"Invalid dbObject for code mapping: {dbObject}")
            };
        }
    }
}
