﻿using System;
using System.Collections.Generic;

namespace DotNetDBTools.Deploy.Core.Queries.DNDBTSysInfo
{
    internal abstract class GetAllDbObjectsFromDNDBTSysInfoQuery : IQuery
    {
        public abstract string Sql { get; }
        public IEnumerable<QueryParameter> Parameters => new List<QueryParameter>();
        public abstract RecordsLoader Loader { get; }

        public abstract class DNDBTDbObjectRecord
        {
            public abstract Guid GetID();
            public abstract Guid? GetParentID();
            public string Type { get; set; }
            public string Name { get; set; }
            public string Code { get; set; }
        }

        public class DNDBTInfo
        {
            public Guid ID { get; set; }
            public string Code { get; set; }
        }

        public abstract class RecordsLoader
        {
            public abstract IEnumerable<DNDBTDbObjectRecord> GetRecords(IQueryExecutor queryExecutor, GetAllDbObjectsFromDNDBTSysInfoQuery query);
        }
    }
}
