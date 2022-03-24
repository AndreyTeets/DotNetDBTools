﻿using System.Data.Common;
using DotNetDBTools.Analysis.Core;
using DotNetDBTools.Analysis.MySQL;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Editors;
using DotNetDBTools.Deploy.MySQL.Editors;

namespace DotNetDBTools.Deploy.MySQL;

internal class MySQLFactory : IFactory
{
    public IQueryExecutor CreateQueryExecutor(DbConnection connection, Events events)
    {
        return new MySQLQueryExecutor(connection, events);
    }

    public IGenSqlScriptQueryExecutor CreateGenSqlScriptQueryExecutor()
    {
        return new MySQLGenSqlScriptQueryExecutor();
    }

    public IDbModelConverter CreateDbModelConverter()
    {
        return new MySQLDbModelConverter();
    }

    public IDbEditor CreateDbEditor(IQueryExecutor queryExecutor)
    {
        return new MySQLDbEditor(queryExecutor);
    }

    public IDbModelFromDBMSProvider CreateDbModelFromDBMSProvider(IQueryExecutor queryExecutor)
    {
        return new MySQLDbModelFromDBMSProvider(queryExecutor);
    }
}
