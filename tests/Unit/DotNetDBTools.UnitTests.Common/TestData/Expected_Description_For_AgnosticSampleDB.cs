namespace DotNetDBToolsSampleDBAgnosticDescription
{
    public static class DotNetDBToolsSampleDBAgnosticTables
    {
        public class MyTable1Info
        {
            public readonly string MyColumn1 = nameof(MyColumn1);
            public readonly string MyColumn2 = nameof(MyColumn2);

            public override string ToString() => nameof(MyTable1);
            public static implicit operator string(MyTable1Info info) => info.ToString();
        }
        public class MyTable2Info
        {
            public readonly string MyColumn1 = nameof(MyColumn1);
            public readonly string MyColumn2 = nameof(MyColumn2);

            public override string ToString() => nameof(MyTable2);
            public static implicit operator string(MyTable2Info info) => info.ToString();
        }

        public static readonly MyTable1Info MyTable1 = new();
        public static readonly MyTable2Info MyTable2 = new();
    }
}
