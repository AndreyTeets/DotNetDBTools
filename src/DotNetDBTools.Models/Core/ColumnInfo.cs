namespace DotNetDBTools.Models.Core
{
    public class ColumnInfo : DBObjectInfo
    {
        public DataTypeInfo DataType { get; set; }
        public bool Nullable { get; set; }
        public bool Identity { get; set; }
        public object Default { get; set; }
        public string DefaultConstraintName { get; set; }
    }
}
