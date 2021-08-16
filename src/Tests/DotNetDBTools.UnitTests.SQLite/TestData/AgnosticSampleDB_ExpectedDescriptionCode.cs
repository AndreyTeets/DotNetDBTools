﻿namespace SampleDBDescription
{
    public static class SampleDBTables
    {
        public class MyTable1Info
        {
            public readonly string MyColumn1 = nameof(MyColumn1);
            public readonly string MyColumn2 = nameof(MyColumn2);

            public override string ToString() => nameof(MyTable1);
        }
        public class MyTable2Info
        {
            public readonly string MyColumn1 = nameof(MyColumn1);
            public readonly string MyColumn2 = nameof(MyColumn2);

            public override string ToString() => nameof(MyTable2);
        }

        public static readonly MyTable1Info MyTable1 = new();
        public static readonly MyTable2Info MyTable2 = new();
    }
}
