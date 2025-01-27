using System;
using System.Collections.Generic;
using System.Linq;

using CVB.NET.Abstractions.Ioc.Registration.Information;

namespace CVB.NET.Abstractions.Ioc.Registration
{
    public class Registration : IRegistration
    {
        public Type ServiceType { get; set; }

        public Type ImplementationType { get; set; }

        private List<IRegistrationInformation> informations = new List<IRegistrationInformation>();

        private bool HasInformationOfRole(Type informationRole)
        {
            return Enumerable.Any(this.informations, i => i.ExtensionRoleInterface == informationRole);
        }

        public TInformationRole GetInformation<TInformationRole>()
            where TInformationRole : IRegistrationInformation
        {
            return (TInformationRole)Enumerable.FirstOrDefault(this.informations, i => i.ExtensionRoleInterface == typeof(TInformationRole));
        }

        public void RegisterInformation(IRegistrationInformation information)
        {
            if (this.HasInformationOfRole(information.ExtensionRoleInterface))
            {
                throw new InvalidOperationException();
            }

            this.informations.Add(information);
        }
    }

    public class Registration<TService> : Registration, IRegistration<TService>
    {
    }
}