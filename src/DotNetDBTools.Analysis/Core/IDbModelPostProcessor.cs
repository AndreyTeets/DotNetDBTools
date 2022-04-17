using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Analysis.Core;

public interface IDbModelPostProcessor
{
    void Do_CreateDbModelFromAgnostic_PostProcessing(Database database);
    void Do_CreateDbModelFromCSharpDefinition_PostProcessing(Database database);
    void Do_CreateDbModelFromSqlDefinition_PostProcessing(Database database);
    void Do_CreateDbModelUsingDNDBTSysInfo_PostProcessing(Database database);
    void Do_CreateDbModelUsingDBMSSysInfo_PostProcessing(Database database);
}
