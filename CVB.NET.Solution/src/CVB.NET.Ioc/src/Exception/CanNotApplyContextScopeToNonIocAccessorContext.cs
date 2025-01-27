namespace CVB.NET.Ioc.Exception
{
    using System;

    public class CanNotApplyContextScopeToNonIocAccessorContext : System.Exception
    {
        public CanNotApplyContextScopeToNonIocAccessorContext(Type staticContextType) : base("Type " + staticContextType.FullName + " is not bound to a IocAccessorAspect.")
        {
        }
    }
}