using System.Collections.Generic;
using DotNetDBTools.Analysis.Errors;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Analysis;

public interface IAnalysisManager
{
    /// <summary>
    /// Analyzes database model for known problems and reports all found errors.
    /// DBMS type is chosen based on the passed database model type.
    /// </summary>
    public bool DbIsValid(Database database, out List<DbError> dbErrors);
    /// <summary>
    /// Compares two database models ignoring properties Database.Version, Database.Scripts, DbObject.ID, CodePiece.Code.
    /// </summary>
    public bool DatabasesAreEquivalentExcludingDNDBTInfo(Database database1, Database database2, out string diffLog);
    /// <summary>
    /// Compares two db-object models ignoring properties DbObject.ID, CodePiece.Code.
    /// </summary>
    public bool DbObjectsAreEquivalentExcludingDNDBTInfo(DbObject dbObject1, DbObject dbObject2, out string diffLog);

    /// <summary>
    /// Creates database-diff model for the two provided.
    /// DBMS type is chosen based on the passed database model type.
    /// </summary>
    public DatabaseDiff CreateDatabaseDiff(Database newDatabase, Database oldDatabase);
    /// <summary>
    /// Checks if database-diff model contains any changes that will result in data loss if applied.
    /// </summary>
    public bool DiffLeadsToDataLoss(DatabaseDiff dbDiff);
    /// <summary>
    /// Checks if database-diff model contains no changes.
    /// </summary>
    public bool DiffIsEmpty(DatabaseDiff dbDiff);
    /// <summary>
    /// Checks if db-object-diff model contains no changes.
    /// </summary>
    public bool DiffIsEmpty(DbObjectDiff dbObjectDiff);

    /// <summary>
    /// Creates specific DBMS database model from the provided agnostic database model.
    /// DBMS type is chosen based on the specified targetKind parameter.
    /// </summary>
    public Database ConvertFromAgnostic(Database database, DatabaseKind targetKind);
    /// <summary>
    /// Creates specific DBMS DataType model from the provided CSharpDataType model.
    /// DBMS type is chosen based on the specified targetKind parameter.
    /// </summary>
    public DataType ConvertDataType(CSharpDataType dataType, DatabaseKind targetKind);
    /// <summary>
    /// Creates specific DBMS CodePiece model which represents default value from the provided CSharpDefaultValue model.
    /// DBMS type is chosen based on the specified targetKind parameter.
    /// </summary>
    public CodePiece ConvertDefaultValue(CSharpDefaultValue defaultValue, DatabaseKind targetKind);

    /// <summary>
    /// Makes postprocessing/normalizing changes to the provided database model required when creating it from definition.
    /// DBMS type is chosen based on the passed database model type.
    /// </summary>
    public void DoCreateSpecificDbmsDbModelFromDefinitionPostProcessing(Database database);
    /// <summary>
    /// Makes postprocessing/normalizing changes to the provided database model required when creating it.
    /// DBMS type is chosen based on the passed database model type.
    /// </summary>
    public void DoPostProcessing(Database database);
    /// <summary>
    /// Makes changes related to objects dependencies to the provided database model required when creating it.
    /// DBMS type is chosen based on the passed database model type.
    /// </summary>
    public void BuildDependencies(Database database);
}
