﻿using DotNetDBTools.Deploy;

namespace DotNetDBTools.DeployTool.Commands;

internal class ScriptsAsmDiffCommand : BaseCommand
{
    public void Execute(
        Dbms dbms,
        string newDbAssemblyPath,
        string oldDbAssemblyPath,
        string outputPath,
        bool allowDataLoss)
    {
        IDeployManager deployManager = CreateDeployManager(dbms);
        deployManager.Options.AllowDataLoss = allowDataLoss;
        deployManager.GeneratePublishScript(newDbAssemblyPath, oldDbAssemblyPath, outputPath);
    }
}
