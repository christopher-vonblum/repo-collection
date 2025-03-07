﻿namespace CVB.NET.Reflection.Aspects.ParameterValidation
{
    using System;
    using Exceptions.Reflection;
    using PostSharp.Aspects;
    using PostSharp.Patterns.Contracts;
    using PostSharp.Reflection;

    public class IsInterfaceAttribute : LocationContractAttribute, ILocationValidationAspect<Type>
    {
        public System.Exception ValidateValue(Type value, string locationName, LocationKind locationKind)
        {
            if (value == null)
            {
                return new ArgumentNullException(locationName);
            }

            if (value.IsInterface)
            {
                return null;
            }

            return new TypeIsNoInterfaceException(value);
        }
    }
}