using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.Core.Editors
{
    internal interface IDbEditor
    {
        public bool DNDBTSysTablesExist();
        public void CreateDNDBTSysTables();
        public void DropDNDBTSysTables();
        public void PopulateDNDBTSysTables(Database database);
        public void ApplyDatabaseDiff(DatabaseDiff databaseDiff);
    }
}
