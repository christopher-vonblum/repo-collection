using System.Diagnostics;
using Newtonsoft.Json;

public class JsonConfig
{
    public string ContainerPath { get; set; }
    public string MountPath { get; set; }
    public string ContainerPassword { get; set; }
    public string AdminPassword { get; set; }
}

public class Program
{
    public static void Main(string[] args)
    {
        string containerName = Console.ReadLine();

        string containerConfig = $"{containerName}.containerConfig";

        JsonConfig config = JsonConvert.DeserializeObject<JsonConfig>(File.ReadAllText(containerConfig));
        
        string container = config.ContainerPath;
        
        string mountPath = config.MountPath;

        string shortcut = mountPath.Split('/').Last();
        
        string device = 
            GetOutputFromCmdAndFeedInput(
                "/usr/bin/sudo", 
                "-s losetup -f", config.AdminPassword).TrimEnd('\n');

        Console.WriteLine(
            GetOutputFromCmdAndFeedInput(
                "/usr/bin/sudo", 
                "-s losetup " + device + " " + container, config.AdminPassword));
        
        Console.WriteLine(
            GetOutputFromCmdAndFeedInput(
                "/usr/bin/sudo", 
                "-s cryptsetup --type tcrypt open --veracrypt " + device  + " " + shortcut, config.AdminPassword));
        
        Console.WriteLine(
            GetOutputFromCmdAndFeedInput(
                "/usr/bin/sudo", 
                "-s mount -t vfat /dev/mapper/" + shortcut + " " + mountPath, config.AdminPassword));
    }
    
    private static string GetOutputFromCmdAndFeedInput(string cmd, string args, params string[] lines)
    {
        try
        {
            Process proc = new Process();

            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.RedirectStandardInput = true;
            proc.StartInfo.FileName = cmd;
            proc.StartInfo.Arguments = args;
            //proc.StartInfo.WorkingDirectory = "/home/anon/Documents/sync/crypt";

            proc.StartInfo.UseShellExecute = false;
            
            proc.Start();
            
            foreach (var input in lines)
            {
                proc.WaitForInputIdle();
                proc.StandardInput.WriteLine(input);
                proc.StandardInput.Flush();
            }
            
            Thread.Sleep(2000);

            string output = proc.StandardOutput.ReadToEnd();

            proc.WaitForExit();

            return output;
        }
        catch(Exception e)
        {
            ;
            throw e;
        }
    }
}