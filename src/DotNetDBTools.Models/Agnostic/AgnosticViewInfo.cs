using DotNetDBTools.Models.Shared;

namespace DotNetDBTools.Models.Agnostic
{
    public class AgnosticViewInfo : BaseDBObjectInfo, IViewInfo
    {
        public string Code { get; set; }
    }
}
