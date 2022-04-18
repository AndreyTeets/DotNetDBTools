using System.Collections.Generic;
using DotNetDBTools.Analysis.Errors;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Analysis;

public interface IAnalysisManager
{
    public bool DbIsValid(Database database, out List<DbError> dbErrors);
    public bool DatabasesAreEquivalentExcludingDNDBTInfo(Database database1, Database database2, out string diffLog);

    public DatabaseDiff CreateDatabaseDiff(Database newDatabase, Database oldDatabase);
    public bool DiffIsEmpty(DatabaseDiff dbDiff);
    public bool DiffLeadsToDataLoss(DatabaseDiff dbDiff);

    public Database ConvertFromAgnostic(Database database, DatabaseKind targetKind);
    public DataType ConvertDataType(CSharpDataType dataType, DatabaseKind targetKind);
    public CodePiece ConvertDefaultValue(CSharpDefaultValue defaultValue, DatabaseKind targetKind);

    public void DoCreateSpecificDbmsDbModelFromDefinitionPostProcessing(Database database);
    public void OrderDbObjects(Database database);
    public void BuildDependencies(Database database);
}
