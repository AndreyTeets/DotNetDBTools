namespace DotNetDBTools.Models.SQLite
{
    public class SQLiteForeignKeyDiff
    {
        public SQLiteForeignKeyInfo NewForeignKey { get; set; }
        public SQLiteForeignKeyInfo OldForeignKey { get; set; }
    }
}
