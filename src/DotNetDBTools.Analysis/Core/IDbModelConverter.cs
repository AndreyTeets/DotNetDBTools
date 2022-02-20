using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Analysis.Core;

public interface IDbModelConverter
{
    public Database FromAgnostic(Database database);
}
