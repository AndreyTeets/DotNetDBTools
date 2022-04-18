using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Analysis.Core;

internal interface IDbModelConverter
{
    public Database FromAgnostic(Database database);
}
