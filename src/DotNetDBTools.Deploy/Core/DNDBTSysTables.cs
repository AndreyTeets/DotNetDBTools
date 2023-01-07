namespace DotNetDBTools.Deploy.Core;

internal static class DNDBTSysTables
{
    public static string AllTablesForInClause => $"'{DNDBTDbAttributes}','{DNDBTDbObjects}','{DNDBTScriptExecutions}'";

    public static readonly DNDBTDbAttributesTableDescription DNDBTDbAttributes = new();
    public static readonly DNDBTDbObjectsTableDescription DNDBTDbObjects = new();
    public static readonly DNDBTScriptExecutionsTableDescription DNDBTScriptExecutions = new();

    public class DNDBTDbAttributesTableDescription
    {
        public readonly string Version = nameof(Version);

        public override string ToString() => nameof(DNDBTDbAttributes);
        public static implicit operator string(DNDBTDbAttributesTableDescription description) => description.ToString();
    }

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

    public class DNDBTScriptExecutionsTableDescription
    {
        public readonly string ID = nameof(ID);
        public readonly string Type = nameof(Type);
        public readonly string Name = nameof(Name);
        public readonly string Text = nameof(Text);
        public readonly string MinDbVersionToExecute = nameof(MinDbVersionToExecute);
        public readonly string MaxDbVersionToExecute = nameof(MaxDbVersionToExecute);
        public readonly string ExecutedOnDbVersion = nameof(ExecutedOnDbVersion);

        public override string ToString() => nameof(DNDBTScriptExecutions);
        public static implicit operator string(DNDBTScriptExecutionsTableDescription description) => description.ToString();
    }
}
