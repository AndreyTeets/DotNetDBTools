namespace DotNetDBTools.CodeParsing.Core.Models
{
    public class ColumnInfo : ObjectInfo
    {
        public string DataType { get; set; }
        public DefaultType DefaultType { get; set; }
        public string DefaultValue { get; set; }
        public bool NotNull { get; set; }
        public bool PrimaryKey { get; set; }
        public bool Unique { get; set; }
        public bool Autoincrement { get; set; }
    }
}
