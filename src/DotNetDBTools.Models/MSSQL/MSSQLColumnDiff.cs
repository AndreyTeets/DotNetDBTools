namespace DotNetDBTools.Models.MSSQL
{
    public class MSSQLColumnDiff
    {
        public MSSQLColumnInfo NewColumn { get; set; }
        public MSSQLColumnInfo OldColumn { get; set; }
    }
}
