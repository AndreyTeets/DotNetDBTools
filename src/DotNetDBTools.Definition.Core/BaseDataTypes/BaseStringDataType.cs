namespace DotNetDBTools.Definition.Core.BaseDataTypes
{
    public abstract class BaseStringDataType : IDataType
    {
        public bool IsUnicode { get; set; } = false;
        public int Length { get; set; } = 50;
    }
}
