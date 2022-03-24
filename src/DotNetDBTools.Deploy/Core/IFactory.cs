﻿using System.Data.Common;
using DotNetDBTools.Analysis.Core;
using DotNetDBTools.Deploy.Core.Editors;

namespace DotNetDBTools.Deploy.Core;

internal interface IFactory
{
    public IQueryExecutor CreateQueryExecutor(DbConnection connection, Events events);
    public IGenSqlScriptQueryExecutor CreateGenSqlScriptQueryExecutor();
    public IDbModelConverter CreateDbModelConverter();
    public IDbEditor CreateDbEditor(IQueryExecutor queryExecutor);
    public IDbModelFromDBMSProvider CreateDbModelFromDBMSProvider(IQueryExecutor queryExecutor);
}
