using DotNetDBTools.Definition.BaseDbTypes;

namespace DotNetDBTools.Definition.MSSQL.DbTypes
{
    public class StringDbType : BaseStringDbType
    {
        public bool IsFixedLength { get; set; } = false;
    }
}
