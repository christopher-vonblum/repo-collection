namespace CVB.NET.Abstractions.Ioc.Container.Model
{
    using System;
    using Container.Activator;

    public struct ServiceRegistration : IServiceRegistration, IEquatable<IServiceRegistration>
    {
        private IServiceActivator activator;
        private IServiceRegistrationKey registrationKey;

        public IServiceRegistrationKey RegistrationKey
        {
            get { return registrationKey ?? new ServiceRegistrationKey(); }
            set { registrationKey = value; }
        }

        public IServiceActivator Activator
        {
            get { return activator ?? new DefaultReflectionActivator(); }
            set { activator = value; }
        }

        public Type ComponentType { get; set; }

        public bool Equals(IServiceRegistration other)
        {
            return this == other;
        }

        public override bool Equals(object obj)
        {
            return obj is IServiceRegistration && this == (IServiceRegistration)obj;
        }

        public override int GetHashCode()
        {
            return RegistrationKey.GetHashCode();
        }

        public static bool operator ==(ServiceRegistration x, IServiceRegistration y)
        {
            return y != null && x.RegistrationKey.Equals(y.RegistrationKey);
        }

        public static bool operator !=(ServiceRegistration x, IServiceRegistration y)
        {
            return !(x == y);
        }
    }
}