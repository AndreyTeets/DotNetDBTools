namespace DotNetDBTools.Models.Core
{
    public class DataTypeInfo
    {
        public string Name { get; set; }
        public bool IsUserDefined { get; set; }
        public int Size { get; set; }
        public int Length { get; set; }
        public bool IsFixedLength { get; set; }
        public bool IsUnicode { get; set; }
        public bool WithTimeZone { get; set; }
        public string SqlTypeName { get; set; }
    }
}
