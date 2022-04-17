using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Analysis.Core;

public abstract class DbModelPostProcessor : IDbModelPostProcessor
{
    public void Do_CreateDbModelFromAgnostic_PostProcessing(Database database)
    {
        DoAdditional_CreateDbModelFromAgnostic_PostProcessing(database);
    }

    public void Do_CreateDbModelFromCSharpDefinition_PostProcessing(Database database)
    {
        OrderDbObjects(database);
        DoAdditional_CreateDbModelFromCSharpDefinition_PostProcessing(database);
    }

    public void Do_CreateDbModelFromSqlDefinition_PostProcessing(Database database)
    {
        OrderDbObjects(database);
        DoAdditional_CreateDbModelFromSqlDefinition_PostProcessing(database);
    }

    public void Do_CreateDbModelUsingDNDBTSysInfo_PostProcessing(Database database)
    {
        DoAdditional_CreateDbModelUsingDNDBTSysInfo_PostProcessing(database);
    }

    public void Do_CreateDbModelUsingDBMSSysInfo_PostProcessing(Database database)
    {
        OrderDbObjects(database);
        DoAdditional_CreateDbModelUsingDBMSSysInfo_PostProcessing(database);
    }

    protected virtual void DoAdditional_CreateDbModelFromAgnostic_PostProcessing(Database database) { }
    protected virtual void DoAdditional_CreateDbModelFromCSharpDefinition_PostProcessing(Database database) { }
    protected virtual void DoAdditional_CreateDbModelFromSqlDefinition_PostProcessing(Database database) { }
    protected virtual void DoAdditional_CreateDbModelUsingDNDBTSysInfo_PostProcessing(Database database) { }
    protected virtual void DoAdditional_CreateDbModelUsingDBMSSysInfo_PostProcessing(Database database) { }

    private void OrderDbObjects(Database database)
    {
        database.Tables = database.Tables.OrderByName();
        database.Views = database.Views.OrderByName();
        database.Scripts = database.Scripts.OrderByName();
        foreach (Table table in database.Tables)
        {
            table.Columns = table.Columns.OrderByName();
            table.UniqueConstraints = table.UniqueConstraints.OrderByName();
            table.CheckConstraints = table.CheckConstraints.OrderByName();
            table.Indexes = table.Indexes.OrderByName();
            table.Triggers = table.Triggers.OrderByName();
            table.ForeignKeys = table.ForeignKeys.OrderByName();
        }
        OrderAdditionalDbObjects(database);
    }
    protected virtual void OrderAdditionalDbObjects(Database database) { }
}
