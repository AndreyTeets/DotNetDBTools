using System;
using System.Collections.Generic;
using DotNetDBTools.Deploy.Common.Queries.DDL;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Queries.DNDBTSysInfo;
using DotNetDBTools.Models.Core;
using static DotNetDBTools.Deploy.Core.InstanceCreator;

namespace DotNetDBTools.Deploy.Common.Editors
{
    internal abstract class ForeignKeyEditor<
        TInsertDNDBTSysInfoQuery,
        TDeleteDNDBTSysInfoQuery,
        TCreateForeignKeyQuery,
        TDropForeignKeyQuery>
        : IForeignKeyEditor
        where TInsertDNDBTSysInfoQuery : InsertDNDBTSysInfoQuery
        where TDeleteDNDBTSysInfoQuery : DeleteDNDBTSysInfoQuery
        where TCreateForeignKeyQuery : CreateForeignKeyQuery
        where TDropForeignKeyQuery : DropForeignKeyQuery
    {
        protected readonly IQueryExecutor QueryExecutor;

        protected ForeignKeyEditor(IQueryExecutor queryExecutor)
        {
            QueryExecutor = queryExecutor;
        }

        public void CreateForeignKey(ForeignKey fk, Dictionary<Guid, Table> fkToTableMap)
        {
            QueryExecutor.Execute(Create<TCreateForeignKeyQuery>(fk, fkToTableMap[fk.ID].Name));
            QueryExecutor.Execute(Create<TInsertDNDBTSysInfoQuery>(fk.ID, fkToTableMap[fk.ID].ID, DbObjectsTypes.ForeignKey, fk.Name));
        }

        public void DropForeignKey(ForeignKey fk, Dictionary<Guid, Table> fkToTableMap)
        {
            QueryExecutor.Execute(Create<TDropForeignKeyQuery>(fk, fkToTableMap[fk.ID].Name));
            QueryExecutor.Execute(Create<TDeleteDNDBTSysInfoQuery>(fk.ID));
        }
    }
}
