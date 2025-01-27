using System.Diagnostics;
using System.Text;

namespace PasswordHelper;

internal static class BashRunner
{
    public static string Run(string commandLine)
    {
        StringBuilder errorBuilder = new();
        StringBuilder outputBuilder = new();
        var arguments = $"-c \"{commandLine}\"";
        using Process process = new()
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "bash",
                Arguments = arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = false
            }
        };
        process.Start();
        process.OutputDataReceived += (_, args) => { outputBuilder.AppendLine(args.Data); };
        process.BeginOutputReadLine();
        process.ErrorDataReceived += (_, args) => { errorBuilder.AppendLine(args.Data); };
        process.BeginErrorReadLine();
        if (!process.DoubleWaitForExit())
        {
            var timeoutError = $@"Process timed out. Command line: bash {arguments}.
Output: {outputBuilder}
Error: {errorBuilder}";
            throw new Exception(timeoutError);
        }

        if (process.ExitCode == 0) return outputBuilder.ToString();

        var error = $@"Could not execute process. Command line: bash {arguments}.
Output: {outputBuilder}
Error: {errorBuilder}";
        throw new Exception(error);
    }

    //To work around https://github.com/dotnet/runtime/issues/27128
    private static bool DoubleWaitForExit(this Process process)
    {
        var result = process.WaitForExit(500);
        if (result) process.WaitForExit();
        return result;
    }
}