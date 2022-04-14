using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Analysis.Core;

public interface IDbModelPostProcessor
{
    void OrderDbObjects(Database database);
}