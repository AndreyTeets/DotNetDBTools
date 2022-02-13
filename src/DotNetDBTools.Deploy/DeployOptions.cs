namespace DotNetDBTools.Deploy;

public class DeployOptions
{
    /// <summary>
    /// Allow data loss during update.
    /// </summary>
    /// <remarks>
    /// Default value is false.
    /// </remarks>
    public bool AllowDataLoss { get; set; } = false;
}
