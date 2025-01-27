namespace CVB.NET.Abstractions.Ioc.Container.Registration.Extension
{
    using System;
    using System.Collections.Generic;

    using CVB.NET.Abstractions.Ioc.Container.Registration.Extension.Base;

    public class BasicLifestyleExtension : RegistrationExtensionBase
    {
        private BasicLifestyle lifestyle;

        public BasicLifestyleExtension(BasicLifestyle lifestyle)
        {
            this.lifestyle = lifestyle;
        }

        public override IReadOnlyDictionary<string, string> GetInstanceModifiers()
        {
            if(this.lifestyle == BasicLifestyle.Singleton) return new Dictionary<string, string> { { nameof(BasicLifestyle), this.SingletonLifestyle() } };
            if (this.lifestyle == BasicLifestyle.Transient) return new Dictionary<string, string> { { nameof(BasicLifestyle), this.TransientLifestyle() } };
            throw new NotImplementedException(this.lifestyle.ToString());
        }

        private string TransientLifestyle()
        {
            return Guid.NewGuid().ToString("N");
        }

        private string SingletonLifestyle()
        {
            return String.Empty;
        }
    }
}
