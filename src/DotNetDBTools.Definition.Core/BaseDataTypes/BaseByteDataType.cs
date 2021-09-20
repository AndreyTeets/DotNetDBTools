namespace DotNetDBTools.Definition.Core.BaseDataTypes
{
    public abstract class BaseByteDataType : IDataType
    {
        public int Length { get; set; } = 50;
    }
}
