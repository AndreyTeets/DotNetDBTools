using System.Diagnostics;
using System.IO;
using System.Text;

namespace DotNetDBTools.IntegrationTests.Utilities;

internal static class ProcessRunner
{
    public static (int, string) RunProcess(string filePath, string args)
    {
        string fullFilePath = Path.GetFullPath(filePath);

        ProcessStartInfo startInfo = new();
        startInfo.FileName = "dotnet";
        startInfo.WorkingDirectory = Path.GetDirectoryName(fullFilePath);
        startInfo.Arguments = fullFilePath;
        startInfo.RedirectStandardOutput = true;
        startInfo.RedirectStandardError = true;
        startInfo.RedirectStandardInput = true;
        startInfo.UseShellExecute = false;
        startInfo.CreateNoWindow = true;

        if (args is not null)
            startInfo.Arguments += $" {args}";

        using Process process = new();
        process.StartInfo = startInfo;

        StringBuilder output = new();
        object outputLock = new();
        process.OutputDataReceived += (_, eventArgs) => { lock (outputLock) { output.AppendLine(eventArgs.Data); } };
        process.ErrorDataReceived += (_, eventArgs) => { lock (outputLock) { output.AppendLine(eventArgs.Data); } };

        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();
        process.WaitForExit();

        return (process.ExitCode, output.ToString());
    }
}
