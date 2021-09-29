using System.Reflection;
using DotNetDBTools.Definition.Core;
using DotNetDBTools.Definition.SQLite;
using DotNetDBTools.DefinitionParser.Core;
using DotNetDBTools.Models.SQLite;

namespace DotNetDBTools.DefinitionParser.SQLite
{
    internal class SQLiteDatabaseModelBuilder : DatabaseModelBuilder
    {
        public SQLiteDatabaseModelBuilder()
            : base(new SQLiteDataTypeMapper(), new SQLiteDefaultValueMapper())
        {
        }

        public SQLiteDatabaseInfo BuildDatabaseModel(Assembly dbAssembly)
        {
            return new SQLiteDatabaseInfo(DbAssemblyInfoHelper.GetDbName(dbAssembly))
            {
                Tables = BuildTableModels<SQLiteTableInfo>(dbAssembly),
                Views = BuildViewModels<SQLiteViewInfo>(dbAssembly),
            };
        }

        protected override string GetOnUpdateActionName(BaseForeignKey fk) => ((ForeignKey)fk).OnUpdate.ToString();
        protected override string GetOnDeleteActionName(BaseForeignKey fk) => ((ForeignKey)fk).OnDelete.ToString();
    }
}
