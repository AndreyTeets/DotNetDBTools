using System;
using System.Collections.Generic;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.Common.Editors
{
    internal interface IForeignKeyEditor
    {
        public void CreateForeignKey(ForeignKey fk, Dictionary<Guid, Table> fkToTableMap);
        public void DropForeignKey(ForeignKey fk, Dictionary<Guid, Table> fkToTableMap);
    }
}
