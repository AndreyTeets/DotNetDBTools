namespace DotNetDBTools.Deploy.Core
{
    internal static class DNDBTSysTables
    {
        public class DNDBTDbObjectsTableInfo
        {
            public readonly string ID = nameof(ID);
            public readonly string ParentID = nameof(ParentID);
            public readonly string Type = nameof(Type);
            public readonly string Name = nameof(Name);

            public override string ToString() => nameof(DNDBTDbObjects);
            public static implicit operator string(DNDBTDbObjectsTableInfo info) => info.ToString();
        }

        public static readonly DNDBTDbObjectsTableInfo DNDBTDbObjects = new();
    }
}
