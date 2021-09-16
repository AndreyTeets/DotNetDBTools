using System.Diagnostics;
using System.IO;
using System.Text;

namespace DotNetDBTools.CommonTestsUtils
{
    public static class ProcessHelper
    {
        public static (int, string) RunProcess(string filePath)
        {
            ProcessStartInfo startInfo = new();
            startInfo.FileName = filePath;
            startInfo.WorkingDirectory = Path.GetDirectoryName(filePath);
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.RedirectStandardInput = true;
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;
            using Process process = new();
            process.StartInfo = startInfo;

            StringBuilder output = new();
            object outputLock = new();
            process.OutputDataReceived += (sender, eventArgs) => { lock (outputLock) { output.AppendLine(eventArgs.Data); }; };
            process.ErrorDataReceived += (sender, eventArgs) => { lock (outputLock) { output.AppendLine(eventArgs.Data); }; };

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit();

            return (process.ExitCode, output.ToString());
        }
    }
}
