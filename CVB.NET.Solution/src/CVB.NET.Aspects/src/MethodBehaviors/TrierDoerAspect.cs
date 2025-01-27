using System.Collections;

namespace CVB.NET.Aspects.MethodBehaviors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using PostSharp.Aspects;
    using PostSharp.Extensibility;

    [Serializable]
    public class TrierDoerAspect : OnMethodBoundaryAspect
    {
        /// <summary>
        /// The method info for the intercepted method.
        /// </summary>
        private MethodInfo Method { get; set; }

        /// <summary>
        /// The parameter infos of the intercepted method
        /// </summary>
        private ParameterInfo[] Parameters { get; set; }

        /// <summary>
        /// How to handler the individual return type
        /// </summary>
        private TypeHandling HandleAs { get; set; }

        /// <summary>
        /// Contains the return type. Can also be the element type of a array or a implementation for generic collection interfaces
        /// </summary>
        private Type ResolvedReturnType { get; set; }

        /// <summary>
        /// Only available for collections. Null otherwise.
        /// </summary>
        private ConstructorInfo DefaultConstructor { get; set; }

        /// <summary>
        /// Should null value be preferred for 
        /// </summary>
        private bool PreferNull { get; set; }

        public TrierDoerAspect(bool preferNull = false)
        {
            this.PreferNull = preferNull;
        }

        public override bool CompileTimeValidate(MethodBase method)
        {
            this.Method = method as MethodInfo;

            if (this.Method == null)
            {
                Message.Write(method, SeverityType.Error, string.Empty, "{0} is not a valid target.", method.Name);
                return false;
            }

            Type genericArgument;

            this.Parameters = this.Method.GetParameters();
            this.ResolvedReturnType = this.Method.ReturnType;

            if (this.ResolvedReturnType == typeof (bool))
            {
                this.HandleAs = TypeHandling.Bool;
                return true;
            }
            else if (this.ResolvedReturnType == typeof (object))
            {
                this.HandleAs = TypeHandling.Class;
                return true;
            }
            else if (typeof (string) == ResolvedReturnType)
            {
                this.HandleAs = TypeHandling.String;
                return true;
            }
            else if (ResolvedReturnType.IsArray)
            {
                this.HandleAs = TypeHandling.Array;
                this.ResolvedReturnType = this.ResolvedReturnType.GetElementType();
                return true;
            }
            else if (typeof(IEnumerable).IsAssignableFrom(ResolvedReturnType))
            {
                this.HandleAs = TypeHandling.Collection;

                if (ResolvedReturnType.IsInterface)
                {
                    genericArgument = ResolvedReturnType.GetGenericArguments().SingleOrDefault();

                    if (genericArgument == null)
                    {
                        // wrong amount of generic arguments
                        return false;
                    }

                    Type implementationType = typeof (List<>);

                    this.ResolvedReturnType = implementationType.MakeGenericType(genericArgument);
                }

                DefaultConstructor = ResolvedReturnType.GetConstructor(Type.EmptyTypes);

                return true;
            }

            this.HandleAs = ResolvedReturnType.IsValueType ? TypeHandling.Struct : TypeHandling.Class;

            return true;
        }

        public override void OnEntry(MethodExecutionArgs args)
        {
            string errorParam;

            if (!this.ValidateParameters(this.MapArguments(args.Arguments), out errorParam))
            {
                args.ReturnValue = this.GetFailureReturnValue(this.HandleAs, errorParam);
                args.FlowBehavior = FlowBehavior.Return;
            }
        }

        public override void OnException(MethodExecutionArgs args)
        {
        }

        private Dictionary<ParameterInfo, object> MapArguments(Arguments arguments)
        {
            Dictionary<ParameterInfo, object> mappedArgs = new Dictionary<ParameterInfo, object>();

            int i = 0;

            foreach (ParameterInfo param in this.Parameters)
            {
                mappedArgs.Add(param, arguments[i]);

                i++;
            }

            return mappedArgs;
        }

        private object GetFailureReturnValue(TypeHandling returnTypeHandling, string issuedByParamName)
        {
            if (this.PreferNull
                && (returnTypeHandling == TypeHandling.String
                    || returnTypeHandling == TypeHandling.Collection
                    || returnTypeHandling == TypeHandling.Array))
            {
                return null;
            }

            switch (returnTypeHandling)
            {
                case TypeHandling.Class:
                    return null;

                case TypeHandling.Struct:
                    throw new ArgumentException(issuedByParamName);

                case TypeHandling.Array:
                    return Array.CreateInstance(this.ResolvedReturnType, 0);

                case TypeHandling.Collection:
                    return this.DefaultConstructor.Invoke(null);

                case TypeHandling.String:
                    return string.Empty;

                case TypeHandling.Bool:
                    return false;
            }

            throw new InvalidOperationException("Value " + returnTypeHandling.ToString() + " of enum type " + typeof (TypeHandling).FullName + " was not handeled.");
        }

        private bool ValidateParameters(Dictionary<ParameterInfo, object> @params, out string errorParam)
        {
            errorParam = null;

            foreach (KeyValuePair<ParameterInfo, object> parameter in @params)
            {
                if (parameter.Key.ParameterType.IsValueType)
                {
                    continue;
                }

                // dont check optinal parameters
                if (parameter.Key.HasDefaultValue && parameter.Key.RawDefaultValue.Equals(parameter.Value))
                {
                    continue;
                }

                if (!parameter.Key.ParameterType.IsValueType && parameter.Value != null)
                {
                    continue;
                }

                errorParam = parameter.Key.Name;
                return false;
            }

            return true;
        }

        /// <summary>
        /// Type categorization for return types
        /// </summary>
        protected enum TypeHandling
        {
            Array,
            Collection,
            Struct,
            Class,
            String,
            Bool
        }
    }
}