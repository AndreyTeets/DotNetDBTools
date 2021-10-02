namespace DotNetDBTools.Deploy.Core
{
    internal static class DNDBTSysTables
    {
        public class DNDBTDbObjectsTableDescription
        {
            public readonly string ID = nameof(ID);
            public readonly string ParentID = nameof(ParentID);
            public readonly string Type = nameof(Type);
            public readonly string Name = nameof(Name);

            public override string ToString() => nameof(DNDBTDbObjects);
            public static implicit operator string(DNDBTDbObjectsTableDescription description) => description.ToString();
        }

        public static readonly DNDBTDbObjectsTableDescription DNDBTDbObjects = new();
    }
}
