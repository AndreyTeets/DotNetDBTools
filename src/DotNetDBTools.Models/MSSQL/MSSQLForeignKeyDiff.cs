namespace DotNetDBTools.Models.MSSQL
{
    public class MSSQLForeignKeyDiff
    {
        public MSSQLForeignKeyInfo NewForeignKey { get; set; }
        public MSSQLForeignKeyInfo OldForeignKey { get; set; }
    }
}
