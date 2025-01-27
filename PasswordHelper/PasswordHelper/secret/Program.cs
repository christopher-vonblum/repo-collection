namespace PasswordHelper;

public class Program
{
    private static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine(
                "get\ngenerate-password\nrenew-password\nimport-secret\nupdate-secret\nimport-file\nupdate-file");
            return;
        }

        switch (args[0])
        {
            case "generate-password":
                Generate(args[1]);
                break;
            case "renew-password":
                Renew(args[1]);
                break;
            case "get":
                Get(args[1]);
                break;
            case "import-secret":
                Import(args[1]);
                break;
            case "update-secret":
                Update(args[1]);
                break;
            case "import-file":
                ImportFile(args[1], args[2]);
                break;
            case "update-file":
                UpdateFile(args[1], args[2]);
                break;
        }
    }

    private static void UpdateFile(string s, string s1)
    {
        throw new NotImplementedException();
    }

    private static void ImportFile(string s, string s1)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Copies API keys from clipboard
    /// </summary>
    private static void Update(string secretName)
    {
        if (!File.Exists(secretName)) throw new FileNotFoundException(secretName);

        CreateSecretVersion(secretName);
        File.WriteAllText(secretName, LinuxClipboard.GetText());
        Get(secretName);
    }

    /// <summary>
    /// Renews a password, versions the old one and copies the new one to clipboard
    /// </summary>
    private static void Renew(string secretName)
    {
        if (!File.Exists(secretName)) throw new FileNotFoundException(secretName);

        CreateSecretVersion(secretName);
        Generate(secretName);
        Get(secretName);
    }

    /// <summary>
    /// Imports a secret from clipboard
    /// </summary>
    private static void Import(string secretName)
    {
        if (File.Exists(secretName)) throw new Exception(secretName + "exists.");

        File.WriteAllText(secretName, LinuxClipboard.GetText());
    }

    /// <summary>
    /// Reads a secret to clipboard
    /// </summary>
    private static void Get(string secretName)
    {
        if (!File.Exists(secretName)) throw new FileNotFoundException(secretName);

        LinuxClipboard.SetText(File.ReadAllText(secretName));
    }

    /// <summary>
    /// Generates a password and copies it to clipboard
    /// </summary>
    private static void Generate(string secretName)
    {
        if (File.Exists(secretName)) throw new Exception($"File {secretName} already exists");

        var random = new Random();
        var str = Guid.NewGuid().ToString("B");
        var charArray = str.ToCharArray();
        var index = 0;
        foreach (var c in str)
        {
            if (char.IsLetter(c) && random.Next(0, 2) == 0)
                charArray[index] = char.ToUpper(c);
            ++index;
        }

        var password = new string(charArray);

        File.WriteAllText(secretName, password);

        LinuxClipboard.SetText(password);
    }

    private static void CreateSecretVersion(string secretName)
    {
        var version = 1;
        var versionedName = BuildSecretVersionName(secretName, version);

        while (File.Exists(versionedName))
        {
            version++;
            versionedName = BuildSecretVersionName(secretName, version);
        }

        File.Move(secretName, versionedName);
    }

    private static string BuildSecretVersionName(string secretName, int version)
    {
        return $"{secretName}_{version}";
    }
}