namespace CVB.NET.Abstractions.Ioc.Container.Registration
{
    using System;

    using CVB.NET.Abstractions.Ioc.Container.Registration.Extension;

    using Reflection.Caching.Cached;

    using BasicLifestyle = CVB.NET.Abstractions.Ioc.Container.Registration.Extension.BasicLifestyle;

    public static class RegistrationInstructionExtensions
    {
        public static IRegistrationInstructionProxy<TService> Cast<TService>(this IRegistrationInstructionProxy instruction)
        {
            if (instruction.Service != typeof(TService))
            {
                throw new InvalidCastException();
            }

            return new RegistrationInstructionProxy<TService>(instruction);
        }
        public static IRegistrationInstructionProxy ImplementedBy(this IRegistrationInstructionProxy instruction, Type tImplementation)
        {
            instruction.GetImplementation = () => tImplementation;
            return instruction;
        }
        public static IRegistrationInstructionProxy<TService> ImplementedBy<TService>(this IRegistrationInstructionProxy<TService> instruction, Type tImplementation)
        {
            instruction.GetImplementation = () => tImplementation;
            return instruction;
        }
        public static IRegistrationInstructionProxy Named(this IRegistrationInstructionProxy instruction, string name)
        {
            instruction.AddExtensions(new NamedInstanceExtension(name));
            return instruction;
        }
        public static IRegistrationInstructionProxy<TService> Named<TService>(this IRegistrationInstructionProxy<TService> instruction, string name)
        {
            instruction.AddExtensions(new NamedInstanceExtension(name));
            return instruction;
        }
        public static IRegistrationInstructionProxy BasicLifestyle(this IRegistrationInstructionProxy instruction, BasicLifestyle lifestyle)
        {
            instruction.AddExtensions(new BasicLifestyleExtension(lifestyle));
            return instruction;
        }

        public static IRegistrationInstructionProxy<TService> BasicLifestyle<TService>(this IRegistrationInstructionProxy<TService> instruction, BasicLifestyle lifestyle)
        {
            instruction.AddExtensions(new BasicLifestyleExtension(lifestyle));
            return instruction;
        }

        public static IRegistrationInstruction AutoInjectMethods(this IRegistrationInstructionProxy instruction, Func<CachedMethodInfo, bool> chooseMethods = null, Func<CachedParameterInfo, string> getParameterName = null, bool enable = true)
        {
            instruction.AddExtensions(new AutoInjectMethods(enable, chooseMethods, getParameterName));
            return instruction;
        }
        public static IRegistrationInstructionProxy<TService> AutoInjectMethods<TService>(this IRegistrationInstructionProxy<TService> instruction, Func<CachedMethodInfo, bool> chooseMethods = null, Func<CachedParameterInfo, string> getParameterName = null, bool enable = true)
        {
            instruction.AddExtensions(new AutoInjectMethods(enable, chooseMethods, getParameterName));
            return instruction;
        }
        public static IRegistrationInstruction AutoInjectProperties(this IRegistrationInstructionProxy instruction, Func<CachedPropertyInfo, bool> chooseProperties = null, Func<CachedPropertyInfo, string> getPropertyName = null, bool enable = true)
        {
            instruction.AddExtensions(new AutoInjectProperties(enable, chooseProperties, getPropertyName));
            return instruction;
        }
        public static IRegistrationInstructionProxy<TService> AutoInjectProperties<TService>(this IRegistrationInstructionProxy<TService> instruction, Func<CachedPropertyInfo, bool> chooseProperties = null, Func<CachedPropertyInfo, string> getPropertyName = null, bool enable = true)
        {
            instruction.AddExtensions(new AutoInjectProperties(enable, chooseProperties, getPropertyName));
            return instruction;
        }
        public static IRegistrationInstructionProxy<TService> MixedConstruction<TService>(this IRegistrationInstructionProxy<TService> instruction, Func<TService> ctor)
        {
            instruction.AddExtensions(new MixedConstruction(() => ctor()));
            return instruction;
        }
        public static IRegistrationInstructionProxy<TService> ManualInjection<TService>(this IRegistrationInstructionProxy<TService> instruction, Action<TService> inject)
        {
            instruction.AddExtensions(new ManualInjection(i => inject((TService)i)));
            return instruction;
        }
    }
}