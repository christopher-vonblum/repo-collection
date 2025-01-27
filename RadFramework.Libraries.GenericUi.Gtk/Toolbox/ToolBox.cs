using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using CoreUi;
using iX.Shared.TemplateTransformation.Abstractions;
using iX.Shared.TemplateTransformation.Abstractions.Logging;
using iX.Shared.TemplateTransformation.Abstractions.Plugins;
using iX.Shared.TemplateTransformation.Json.Patching;
using iX.Shared.TemplateTransformation.Json.Patching.Arguments;
using iX.Shared.TemplateTransformation.Json.Patching.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json.Linq;
using Toolbox.Activities;

namespace Toolbox
{
    public class ToolBox
    {
        public IEnumerable<Type> Activities { get; }
        private IServiceProvider _serviceProvider { get; }
        
        public ToolBox(string packagesFolder, IInteractionProvider interactionProvider)
        {
            JObject runtimeTemplate = JObject.Parse(@"
            {
                    ""registeredPlugins"": [
                        ""{$PATCH sctool}""
                    ]
            }");
            
            TransformationContext tc = new TransformationContext();
            
            tc.SetPluginPolicy<IIncludeContext>(new IncludeContext
            {
                IncludeRoot = packagesFolder,
                ConfigRoot = packagesFolder,
                ResolvedIncludeRoots = new []{packagesFolder}
            });
            
            tc.SetPluginPolicy<ILogMessageSink>(new ConsoleLogMessageSink());
            
            new JsonTransformationEngine(new[]{new JsonPatchFileMakroPlugin(new PatchCollector(new PluginIncludeConditionDelegate[0]))})
                .EvaluateMakros(tc, runtimeTemplate);

            RegisteredPluginsSection registeredPlugins = runtimeTemplate.ToObject<RegisteredPluginsSection>();

            List<Assembly> targets = new List<Assembly>();
            
            foreach (ToolboxPlugin plugin in registeredPlugins.RegisteredPlugins)
            {
                targets.Add(AssemblyLoadContext.Default.LoadFromAssemblyPath($"{packagesFolder}\\{plugin.DllName}"));
            }
            
            ServiceCollection activityCollection = new ServiceCollection();

            activityCollection.Add(ServiceDescriptor.Singleton(this));
            activityCollection.Add(ServiceDescriptor.Singleton(interactionProvider));
            
            Activities = targets
                .SelectMany(a => a.GetTypes())
                .Where(t => typeof(IActivity).IsAssignableFrom(t) && !t.IsAbstract && t != typeof(IActivity<,>))
                .ToList();
            
            ((List<Type>)Activities)
                .ForEach(t =>
                {
                    activityCollection.Add(ServiceDescriptor.Transient(t, t));
                });
            
            _serviceProvider = activityCollection.BuildServiceProvider();
            
        }

        public object Run(Type activity, object inputModel)
        {
            return ((IActivity) _serviceProvider.GetService(activity)).Execute(inputModel);
        }
    }
}