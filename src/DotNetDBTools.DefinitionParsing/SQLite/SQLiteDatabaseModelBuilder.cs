using System.Reflection;
using DotNetDBTools.Definition.Core;
using DotNetDBTools.DefinitionParsing.Core;
using DotNetDBTools.Models.SQLite;

namespace DotNetDBTools.DefinitionParsing.SQLite
{
    internal class SQLiteDatabaseModelBuilder : DatabaseModelBuilder
    {
        public SQLiteDatabaseModelBuilder()
            : base(new SQLiteDataTypeMapper(), new SQLiteDefaultValueMapper())
        {
        }

        public SQLiteDatabase BuildDatabaseModel(Assembly dbAssembly)
        {
            return new SQLiteDatabase(DbAssemblyInfoHelper.GetDbName(dbAssembly))
            {
                Tables = BuildTableModels<SQLiteTable>(dbAssembly),
                Views = BuildViewModels<SQLiteView>(dbAssembly),
            };
        }

        protected override string GetOnUpdateActionName(BaseForeignKey fk) => ((Definition.SQLite.ForeignKey)fk).OnUpdate.ToString();
        protected override string GetOnDeleteActionName(BaseForeignKey fk) => ((Definition.SQLite.ForeignKey)fk).OnDelete.ToString();
    }
}
