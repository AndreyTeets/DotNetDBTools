namespace DotNetDBTools.DeployInteractor
{
    public static class DNDBTSysTables
    {
        public class DNDBTDbObjectsTableInfo
        {
            public readonly string ID = nameof(ID);
            public readonly string Type = nameof(Type);
            public readonly string Name = nameof(Name);
            public readonly string Metadata = nameof(Metadata);

            public override string ToString() => nameof(DNDBTDbObjects);
        }

        public static readonly DNDBTDbObjectsTableInfo DNDBTDbObjects = new();
    }
}
