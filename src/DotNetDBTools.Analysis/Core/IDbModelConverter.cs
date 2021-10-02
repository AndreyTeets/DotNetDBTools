using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Analysis.Core
{
    public interface IDbModelConverter
    {
        Database FromAgnostic(Database database);
    }
}
