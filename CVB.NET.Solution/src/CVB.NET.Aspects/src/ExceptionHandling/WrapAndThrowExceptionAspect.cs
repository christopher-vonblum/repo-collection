namespace CVB.NET.Aspects.ExceptionHandling
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Serialization;
    using PostSharp.Aspects;
    using PostSharp.Extensibility;
    using PostSharp.Patterns.Contracts;

    [Serializable]
    public class WrapAndThrowExceptionAspect : OnExceptionAspect
    {
        private Type ExceptionWrapperType { get; }

        private static Type[] WrapperExceptionConstructorSignature { get; } = {typeof (string), typeof (System.Exception)};

        private ConstructorInfo WrapperExceptionCtor { get; }

        private string ExceptionMessage { get; }

        public WrapAndThrowExceptionAspect([NotNull] Type exceptionWrapperType, [NotNull] string exceptionMessage = "")
        {
            ExceptionWrapperType = exceptionWrapperType;
            WrapperExceptionCtor = GetWrapperExceptionConstructor(exceptionWrapperType);
            ExceptionMessage = exceptionMessage;
        }

        public override bool CompileTimeValidate(MethodBase method)
        {
            if (WrapperExceptionCtor == null)
            {
                Message.Write(
                    method,
                    SeverityType.Error,
                    string.Empty,
                    "{0} does not contain a compatible constructor matching definition ({1})",
                    ExceptionWrapperType.FullName,
                    string.Join(", ", WrapperExceptionConstructorSignature.Select(type => type.Name)));

                return false;
            }

            return true;
        }

        public override void OnException(MethodExecutionArgs args)
        {
            if (args.Exception is SerializationException)
            {
                args.FlowBehavior = FlowBehavior.RethrowException;
                return;
            }

            args.Exception = CreateWrapperException(args.Exception);
            args.FlowBehavior = FlowBehavior.ThrowException;
        }

        private ConstructorInfo GetWrapperExceptionConstructor(Type wrapperExceptionType)
        {
            return wrapperExceptionType.GetConstructor(
                BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Instance,
                null,
                WrapperExceptionConstructorSignature,
                null);
        }

        protected virtual System.Exception CreateWrapperException(System.Exception innerException)
        {
            return (System.Exception) WrapperExceptionCtor.Invoke(new object[] {ExceptionMessage, innerException});
        }
    }
}