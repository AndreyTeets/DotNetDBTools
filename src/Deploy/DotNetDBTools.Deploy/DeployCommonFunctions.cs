using System.Linq;
using System.Reflection;

namespace DotNetDBTools.Deploy
{
    public static class DeployCommonFunctions
    {
        public static bool IsAgnosticDb(Assembly dbAssembly)
        {
            bool isAgnosticDb = dbAssembly
                .GetTypes()
                .Any(x => x.GetInterfaces()
                    .Any(y => y == typeof(Definition.Agnostic.ITable)));

            return isAgnosticDb;
        }
    }
}
