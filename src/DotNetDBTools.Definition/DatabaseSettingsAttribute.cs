using System;

namespace DotNetDBTools.Definition;

/// <remarks>
/// If attribute is not declared effect is the same as if default values were used.
/// </remarks>
[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false, Inherited = false)]
public class DatabaseSettingsAttribute : Attribute
{
    /// <summary>
    /// Controls how database model is parsed from assembly.
    /// </summary>
    /// <remarks>
    /// Default value is CSharp.
    /// </remarks>
    public DefinitionKind DefinitionKind { get; set; } = DefinitionKind.CSharp;

    /// <summary>
    /// Useful when Before/After publish scripts are used, mainly to protect from [Min|Max]DbVersionToExecute mistakes.
    /// Otherwise it may be desirable to disable version check during publish in DeployOptions.
    /// </summary>
    /// <remarks>
    /// Default value is 1.
    /// </remarks>
    public long DatabaseVersion { get; set; } = 1;
}

public enum DefinitionKind
{
    /// <summary>
    /// Assembly is scanned for the presence of [Agnostic|MSSQL|MySQL|PostgreSQL|SQLite].ITable implementations.
    /// If one or more implementations from exactly one DBMS are found database model is constructed from ITable, IView, e.t.c. implementations.
    /// Otherwise error is thrown.
    /// </summary>
    CSharp,

    /// <summary>
    /// Not implemented.
    /// </summary>
    MSSQL,

    /// <summary>
    /// Not implemented.
    /// </summary>
    MySQL,

    /// <summary>
    /// Not implemented.
    /// </summary>
    PostgreSQL,

    /// <summary>
    /// Assembly is scanned for embedded files with .sql extension and database model is constructed from definitions in them.
    /// </summary>
    SQLite,
}
