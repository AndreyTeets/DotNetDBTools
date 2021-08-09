using System;
using SampleDBDescription;

namespace DotNetDBTools.SampleApplication.Agnostic
{
    public class Program
    {
        public static void Main()
        {
            string mySqlQuery =
$@"select
    {SampleDBTables.MyTable1.MyColumn1},
    {SampleDBTables.MyTable1.MyColumn2}
from {SampleDBTables.MyTable1}
where {SampleDBTables.MyTable1.MyColumn1} is not null";

            Console.WriteLine(mySqlQuery);
        }
    }
}
