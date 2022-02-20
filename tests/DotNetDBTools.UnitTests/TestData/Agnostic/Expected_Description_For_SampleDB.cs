﻿namespace DotNetDBToolsSampleDBAgnosticDescription
{
    public static class DotNetDBToolsSampleDBAgnosticTables
    {
        public static readonly MyTable1Description MyTable1 = new();
        public static readonly MyTable2Description MyTable2 = new();
        public static readonly MyTable4Description MyTable4 = new();
        public static readonly MyTable5Description MyTable5 = new();

        public class MyTable1Description
        {
            public readonly string MyColumn1 = nameof(MyColumn1);
            public readonly string MyColumn2 = nameof(MyColumn2);
            public readonly string MyColumn3 = nameof(MyColumn3);
            public readonly string MyColumn4 = nameof(MyColumn4);

            public override string ToString() => nameof(MyTable1);
            public static implicit operator string(MyTable1Description description) => description.ToString();
        }
        public class MyTable2Description
        {
            public readonly string MyColumn1 = nameof(MyColumn1);
            public readonly string MyColumn2 = nameof(MyColumn2);

            public override string ToString() => nameof(MyTable2);
            public static implicit operator string(MyTable2Description description) => description.ToString();
        }
        public class MyTable4Description
        {
            public readonly string MyColumn1 = nameof(MyColumn1);

            public override string ToString() => nameof(MyTable4);
            public static implicit operator string(MyTable4Description description) => description.ToString();
        }
        public class MyTable5Description
        {
            public readonly string MyColumn1 = nameof(MyColumn1);
            public readonly string MyColumn3 = nameof(MyColumn3);

            public override string ToString() => nameof(MyTable5);
            public static implicit operator string(MyTable5Description description) => description.ToString();
        }
    }
}
