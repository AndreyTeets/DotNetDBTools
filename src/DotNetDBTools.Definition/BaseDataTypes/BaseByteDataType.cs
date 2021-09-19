namespace DotNetDBTools.Definition.BaseDataTypes
{
    public abstract class BaseByteDataType : IDataType
    {
        public int Length { get; set; } = 50;
    }
}
