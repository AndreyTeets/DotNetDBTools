using System;
using System.Collections.Generic;
using DotNetDBTools.Definition.Core;
using DotNetDBTools.DefinitionParsing.Core;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.DefinitionParsing.Agnostic
{
    internal class AgnosticDbObjectCodeMapper : IDbObjectCodeMapper
    {
        public CodePiece MapToCodePiece(IDbObject dbObject)
        {
            return dbObject switch
            {
                Definition.Agnostic.CheckConstraint ck => new AgnosticCodePiece
                {
                    Code = null,
                    DbKindToCodeMap = new Dictionary<DatabaseKind, string>()
                    {
                        { DatabaseKind.MSSQL, ck.Code(Definition.Agnostic.DbmsKind.MSSQL) },
                        { DatabaseKind.MySQL, ck.Code(Definition.Agnostic.DbmsKind.MySQL) },
                        { DatabaseKind.PostgreSQL, ck.Code(Definition.Agnostic.DbmsKind.PostgreSQL) },
                        { DatabaseKind.SQLite, ck.Code(Definition.Agnostic.DbmsKind.SQLite) },
                    }
                },
                _ => throw new InvalidOperationException($"Invalid dbObject for code mapping: {dbObject}"),
            };
        }
    }
}
