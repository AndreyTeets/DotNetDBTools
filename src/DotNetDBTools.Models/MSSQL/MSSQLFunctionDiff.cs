namespace DotNetDBTools.Models.MSSQL
{
    public class MSSQLFunctionDiff
    {
        public MSSQLFunctionInfo NewFunction { get; set; }
        public MSSQLFunctionInfo OldFunction { get; set; }
    }
}
