namespace DotNetDBTools.Models.SQLite
{
    public class SQLiteColumnDiff
    {
        public SQLiteColumnInfo NewColumn { get; set; }
        public SQLiteColumnInfo OldColumn { get; set; }
    }
}
