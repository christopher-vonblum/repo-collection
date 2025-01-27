namespace CVB.NET.Abstractions.Ioc.Registration.Information.Name
{
    public class NameInformation : RegistrationInformationBase<NameInformation>
    {
        public string Name { get; }

        public NameInformation(string name)
        {
            this.Name = name;
        }
    }
}
