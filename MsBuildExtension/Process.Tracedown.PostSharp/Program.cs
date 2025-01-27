using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Process.Tracedown.PostSharp
{
    class Program
    {
        static FileSystemWatcher watcher = new FileSystemWatcher();

        static void Main()
        {
            Thread watcherThread = new Thread(new ThreadStart(Run));
            watcherThread.Start();

            Console.ReadLine();

            watcherThread.Join();
        }

        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public static void Run()
        {
            watcher.Path = "G:\\";
            watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.LastAccess;
            watcher.Filter = "*.*";
            watcher.IncludeSubdirectories = true;
            while (true)
            {
                WatcherOnChanged(watcher.WaitForChanged(WatcherChangeTypes.All));
            }
        }

        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        private static void WatcherOnChanged(WaitForChangedResult result)
        {
            if(result.ChangeType == WatcherChangeTypes.All || result.ChangeType == WatcherChangeTypes.Deleted)
            {
                LogResult(result, ConsoleColor.Red, "- ");
            }
            else if(result.ChangeType == WatcherChangeTypes.Renamed)
            {
                LogResultRename(result);
            }
            else if (result.ChangeType == WatcherChangeTypes.Created)
            {
                LogResult(result, ConsoleColor.Green, "+ ");
            }
            else
            {
                LogResult(result);
            }
        }

        private static void LogPartial(string text, ConsoleColor? color)
        {
            BackupConsoleColor();
            Console.ForegroundColor = color ?? ConsoleColor.White;
            Console.WriteLine(text);
            RestoreConsoleColor();
        }

        private static void Log(string text, ConsoleColor? color)
        {
            LogPartial(text + "\n", color);
        }

        private static void LogResult(WaitForChangedResult result, ConsoleColor? overrideColor = null, string prefix = "", string suffix = "")
        {
            LogPartial(prefix + result.Name, overrideColor ?? ConsoleColor.Yellow);
            Log(" was " + result.ChangeType + suffix, ConsoleColor.White);
        }

        private static void LogResultRename(WaitForChangedResult result)
        {
            LogResult(result, ConsoleColor.DarkYellow, "R ", " from " + result.OldName);
        }

        private static Stack<ConsoleColor> colorsStack = new Stack<ConsoleColor>();
        private static void BackupConsoleColor()
        {
            colorsStack.Push(Console.ForegroundColor);
        }
        private static void RestoreConsoleColor()
        {
            Console.ForegroundColor = colorsStack.Pop();
        }
    }
}
