using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Analysis.Core
{
    public interface IDbModelConverter
    {
        DatabaseInfo FromAgnostic(DatabaseInfo databaseInfo);
    }
}
