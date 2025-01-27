namespace CVB.NET.Abstractions.Ioc.Registration.Information.Instance
{
    public class InstanceInformation : RegistrationInformationBase<InstanceInformation>, IRegistrationInformation
    {
        public object Instance { get; }

        public InstanceInformation(object instance)
        {
            this.Instance = instance;
        }
    }
}