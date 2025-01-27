namespace PasswordHelper;

internal static class LinuxClipboard
{
    private static bool isWsl;

    static LinuxClipboard()
    {
        isWsl = Environment.GetEnvironmentVariable("WSL_DISTRO_NAME") != null;
    }

    public static async Task SetTextAsync(string text, CancellationToken cancellation)
    {
        var tempFileName = Path.GetTempFileName();
        await File.WriteAllTextAsync(tempFileName, text, cancellation);

        if (cancellation.IsCancellationRequested) return;

        InnerSetText(tempFileName);
    }

    public static void SetText(string text)
    {
        var tempFileName = Path.GetTempFileName();
        File.WriteAllText(tempFileName, text);
        InnerSetText(tempFileName);
    }

    private static void InnerSetText(string tempFileName)
    {
        try
        {
            if (isWsl)
                BashRunner.Run($"cat {tempFileName} | clip.exe ");
            else
                BashRunner.Run($"cat {tempFileName} | xsel -i --clipboard ");
        }
        finally
        {
            File.Delete(tempFileName);
        }
    }

    public static string? GetText()
    {
        var tempFileName = Path.GetTempFileName();
        try
        {
            InnerGetText(tempFileName);
            return File.ReadAllText(tempFileName);
        }
        finally
        {
            File.Delete(tempFileName);
        }
    }

    public static async Task<string?> GetTextAsync(CancellationToken cancellation)
    {
        var tempFileName = Path.GetTempFileName();
        try
        {
            InnerGetText(tempFileName);
            return await File.ReadAllTextAsync(tempFileName, cancellation);
        }
        finally
        {
            File.Delete(tempFileName);
        }
    }

    private static void InnerGetText(string tempFileName)
    {
        if (isWsl)
            BashRunner.Run($"powershell.exe -NoProfile Get-Clipboard  > {tempFileName}");
        else
            BashRunner.Run($"xsel -o --clipboard  > {tempFileName}");
    }
}