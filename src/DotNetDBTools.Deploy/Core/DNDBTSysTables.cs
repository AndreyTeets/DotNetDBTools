namespace DotNetDBTools.Deploy.Core;

internal static class DNDBTSysTables
{
    public static readonly DNDBTDbObjectsTableDescription DNDBTDbObjects = new();

    public class DNDBTDbObjectsTableDescription
    {
        public readonly string ID = nameof(ID);
        public readonly string ParentID = nameof(ParentID);
        public readonly string Type = nameof(Type);
        public readonly string Name = nameof(Name);
        public readonly string Code = nameof(Code);

        public override string ToString() => nameof(DNDBTDbObjects);
        public static implicit operator string(DNDBTDbObjectsTableDescription description) => description.ToString();
    }
}
