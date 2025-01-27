using System;
using System.Linq;
using CVB.NET.Reflection.Caching.Cached;
using CVB.NET.Rewriting.Compiler.BuildIntegration.MsBuild.Project.ProjectWrappers;
using PostSharp.Aspects;
using PostSharp.Extensibility;
using PostSharp.Reflection;

namespace CVB.NET.Rewriting.Compiler.BuildIntegration.MsBuild.Project.Aspect
{
    [MulticastAttributeUsage(MulticastTargets.Property, Inheritance = MulticastInheritance.Multicast)]
    [AttributeUsage(AttributeTargets.Interface)]
    [Serializable]
    public class ProjectWrapperAspect : LocationInterceptionAspect
    {
        public override bool CompileTimeValidate(LocationInfo locationInfo)
        {
            return locationInfo.DeclaringType.GetInterfaces().Contains(typeof (IMsBuildProjectWrapper)) && locationInfo.LocationType == typeof (string);
        }

        public override void OnGetValue(LocationInterceptionArgs args)
        {
            CachedPropertyInfo cachedProperty = args.Location.PropertyInfo;

            if (cachedProperty.InheritedAttributes.OfType<IgnoreCustomPropertyAttribute>().Any())
            {
                args.ProceedGetValue();
                return;
            }

            IMsBuildProjectWrapper wrapper = (IMsBuildProjectWrapper) args.Instance;

            args.Value = wrapper.InnerProject.GetProperty(args.LocationName).EvaluatedValue;
        }

        public override void OnSetValue(LocationInterceptionArgs args)
        {
            CachedPropertyInfo cachedProperty = args.Location.PropertyInfo;

            if (cachedProperty.InheritedAttributes.OfType<IgnoreCustomPropertyAttribute>().Any())
            {
                args.ProceedSetValue();
                return;
            }

            IMsBuildProjectWrapper wrapper = (IMsBuildProjectWrapper) args.Instance;

            wrapper.InnerProject.SetProperty(args.LocationName, (string) args.Value);
        }
    }
}