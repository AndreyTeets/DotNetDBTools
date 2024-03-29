﻿namespace DotNetDBTools.Deploy;

public class DeployOptions
{
    /// <summary>
    /// Allow data loss during publish.
    /// </summary>
    /// <remarks>
    /// Default value is false.
    /// </remarks>
    public bool AllowDataLoss { get; set; } = false;

    /// <summary>
    /// Allow changing database during publish without chaning it's version.
    /// </summary>
    /// <remarks>
    /// Default value is false.
    /// </remarks>
    public bool AllowUnchangedDbVersionForNonEmptyDbDiff { get; set; } = false;

    /// <summary>
    /// Don't use transaction for publish.
    /// Ignored for MySQL which never uses transaction.
    /// </summary>
    /// <remarks>
    /// Default value is false.
    /// </remarks>
    public bool NoTransaction { get; set; } = false;
}
