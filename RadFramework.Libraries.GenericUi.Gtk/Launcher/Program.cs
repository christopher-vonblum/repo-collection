using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CoreUi;
using CoreUi.Gtk;
using CoreUi.Proxy;
using Newtonsoft.Json;
using Toolbox.Activities;

namespace Launcher
{
    class Program
    {
        private static Dictionary<string, object> parsedArgs = new Dictionary<string, object>();
        private static Toolbox.ToolBox toolbox = new Toolbox.ToolBox(
            Path.GetDirectoryName(new Uri(typeof(Program).Assembly.EscapedCodeBase).LocalPath + "\\plugins"),
             new GtkInteractionProvider());
        
        static void Main(string[] args)
        {
            ParseArgs(args);

#if DEBUG
            parsedArgs["-gui"] = true;
#endif
            
            Type activityDataModel = ExtractActivityDataModel();

            object dataModel = null;
            
            if (parsedArgs.ContainsKey("-config"))
            {
                string config = File.ReadAllText((string) parsedArgs["-config"]);

                dataModel = JsonConvert.DeserializeObject(config, activityDataModel, new  ProxyNewtonsoftJsonSerializationConverter());
            }
            
            if ((dataModel == null || activityDataModel == null) && ((bool)parsedArgs["-gui"]))
            {
                object r = toolbox.Run(typeof(RunActivityActivity), null);
                Console.WriteLine(r);
                return;
            }
            
            object result = toolbox.Run(GetActivityType(), dataModel);
            
            Console.WriteLine(result);
        }

        private static Type ExtractActivityDataModel()
        {
            Type activity = GetActivityType();

            return activity.GetInterface("IActivity`2").GetGenericArguments()[0];
        }
        
        private static Type GetActivityType()
        {
            string arg = (string)(parsedArgs.ContainsKey("-activity") ?  parsedArgs["-activity"] : null);

            if (arg == null && parsedArgs.ContainsKey("-gui") && (bool)parsedArgs["-gui"])
            {
                return typeof(RunActivityActivity);
            }
            return Type.GetType(arg);
        }
        
        private static void ParseArgs(string[] args)
        {
            int i = 0;
            foreach (string s in args)
            {
                int next = i + 1;
                int previous = i - 1;
                int maxIndex = args.Length - 1;
                
                // element is arg name
                if (s.StartsWith("-") && maxIndex < next && !args[next].StartsWith("-"))
                {
                }
                // element is bool flag
                else if(s.StartsWith("-") && maxIndex < next && args[next].StartsWith("-"))
                {
                    parsedArgs[s] = true;
                }
                // element is value
                else if (!s.StartsWith("-") && args[previous].StartsWith("-"))
                {
                    parsedArgs[args[previous]] = s;
                }
                else
                {
                    throw new Exception("check syntax");
                }
                
                i++;
            }
        }
    }
}