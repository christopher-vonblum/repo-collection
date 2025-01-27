namespace CVB.NET.Domain.Model.Base
{
    using System;
    using System.Threading;

    public abstract class AppDomainDriverBase : MarshalByRefObject, IAppDomainDriver
    {
        private AppDomain DriverDomain { get; }
        private Thread DriverThread { get; }

        private AppDomainDriverBase RemoteDriver { get; }

        protected AppDomainDriverBase()
        {
            DriverDomain = AppDomain.CurrentDomain;
            Name = AppDomain.CurrentDomain.FriendlyName;

            DriverThread = new Thread(Run);
            DriverThread.Start();
        }

        protected AppDomainDriverBase(string name)
        {
            Name = name;

            DriverDomain = CreateDomain();

            Type driverType = this.GetType();

            DriverDomain.Load(driverType.Assembly.GetName());

            RemoteDriver = (AppDomainDriverBase) DriverDomain.CreateInstanceAndUnwrap(driverType.Assembly.FullName, driverType.FullName);
        }

        protected virtual AppDomain CreateDomain()
        {
            AppDomainSetup setup = new AppDomainSetup()
            {
                ConfigurationFile = Name + ".config",
                ApplicationBase = AppDomain.CurrentDomain.BaseDirectory
            };

            return AppDomain.CreateDomain(Name, AppDomain.CurrentDomain.Evidence, setup);
        }

        public string Name { get; }

        void IAppDomainDriver.Run()
        {
            if (RemoteDriver == null)
            {
                Run();
            }
            else
            {
                RemoteDriver.Run();
            }
        }

        public abstract void Run();

        void IAppDomainDriver.Stop()
        {
            if (RemoteDriver == null)
            {
                Stop();
            }
            else
            {
                RemoteDriver.Stop();
            }
        }

        public abstract void Stop();

        public virtual void Dispose()
        {
            ((IAppDomainDriver)this).Stop();
        }
    }
}