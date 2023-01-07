using System.Collections.Generic;
using System.Reflection;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.DefinitionParsing;

public interface IDefinitionParsingManager
{
    /// <summary>
    /// Loads assembly from the specified path to .dll file.
    /// </summary>
    public Assembly LoadDbAssembly(string dbAssemblyPath);

    /// <summary>
    /// Creates database model from from the provided dbAssembly.
    /// DBMS type is chosen based on the specified DatabaseSettings.DefinitionKind.
    /// </summary>
    public Database CreateDbModel(string dbAssemblyPath);
    /// <summary>
    /// Creates database model from from the provided dbAssembly.
    /// DBMS type is chosen based on the specified DatabaseSettings.DefinitionKind.
    /// </summary>
    public Database CreateDbModel(Assembly dbAssembly);
    /// <summary>
    /// Creates database model from from the provided list of sql create statements with ID declarations.
    /// DBMS type is chosen based on the specified dbKind parameter.
    /// </summary>
    public Database CreateDbModel(IEnumerable<string> definitionSqlStatements, long dbVersion, DatabaseKind dbKind);
}
