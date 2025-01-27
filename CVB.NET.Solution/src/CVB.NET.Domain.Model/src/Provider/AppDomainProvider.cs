namespace CVB.NET.Domain.Model.Provider
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Security;
    using System.Security.Policy;
    using Base;
    using Exception;
    using mscoree;
    using Utils.String;

    public class AppDomainProvider : IAppDomainProvider
    {
        private static AppDomainProvider defaultProvider = new AppDomainProvider();
        public static AppDomainProvider DefaultProvider => defaultProvider;

        AppDomain IAppDomainProvider.GetRootDomain()
        {
            return GetRootDomain();
        }

        public AppDomain GetParentDomain(AppDomain childDomain)
        {
            string parentDomainPath = childDomain.FriendlyName.Substring(0, childDomain.FriendlyName.EnsureNoSuffixes("/").LastIndexOf("/", StringComparison.Ordinal));

            return GetDomain(parentDomainPath);
        }

        public bool HasChildDomain(AppDomain parentDomain, string domainName)
        {
            return GetDomainInternal(GetChildDomainPath(parentDomain, domainName)) != null;
        }

        public AppDomain CreateChildDomain(AppDomain parentDomain, string domainName)
        {
            string childDomainPath = GetChildDomainPath(parentDomain, domainName);

            return CreateAppDomain(parentDomain.BaseDirectory, childDomainPath);
        }

        public AppDomain GetChildDomain(AppDomain parentDomain, string domainName)
        {
            return GetDomainInternal(GetChildDomainPath(parentDomain, domainName));
        }

        public AppDomain GetDomain(string domainPath)
        {
            return GetDomainInternal(domainPath);
        }

        public static AppDomain GetRootDomain()
        {
            if (AppDomain.CurrentDomain.IsDefaultAppDomain())
            {
                return AppDomain.CurrentDomain;
            }

            List<AppDomain> appDomains = GetAppDomains();

            return appDomains.Single(domain => domain.IsDefaultAppDomain());
        }

        private AppDomain GetDomainInternal(string domainPath)
        {
            Func<AppDomain, bool> domainSearchPredicate = domain => domainPath.ToLowerInvariant().Equals(domain.FriendlyName);

            if (domainSearchPredicate(AppDomain.CurrentDomain))
            {
                return AppDomain.CurrentDomain;
            }

            List<AppDomain> appDomains = GetAppDomains();

            int count = appDomains.Count(domainSearchPredicate);

            if (count == 0)
            {
                throw new AppDomainNotFoundException(domainPath);
            }

            if (count > 1)
            {
                throw new DuplicateAppDomainsFoundException(domainPath);
            }

            return appDomains.Single(domainSearchPredicate);
        }

        private AppDomain CreateAppDomain(string parentDomainBaseDirectory, string domainPath)
        {
            string childDomainName = GetDomainNameFromPath(domainPath);

            string childDomainBaseDirectory = parentDomainBaseDirectory.EnsureSuffix("\\") + childDomainName;

            string childDomainBinDirectory = childDomainBaseDirectory.EnsureSuffix("\\") + "bin";

            Evidence childDomainEvidence = new Evidence();

            childDomainEvidence.AddAssemblyEvidence(new Zone(SecurityZone.Trusted));

            AppDomainSetup childDomainSetup = new AppDomainSetup
                                   {
                                       ApplicationBase = childDomainBaseDirectory,
                                       PrivateBinPath = childDomainBinDirectory,
                                       PrivateBinPathProbe = string.Empty,
                                       ConfigurationFile = childDomainBaseDirectory + "domain.config"
                                   };

            return AppDomain.CreateDomain(domainPath, childDomainEvidence, childDomainSetup);
        }

        private string GetDomainNameFromPath(string domainPath)
        {
            string safeDomainPath = domainPath.ToLowerInvariant().EnsureNoSuffixes("/");

            return safeDomainPath.Substring(safeDomainPath.LastIndexOf("/", StringComparison.Ordinal)).EnsureNoPrefix("/");
        }

        protected virtual string GetChildDomainPath(AppDomain parentDomain, string domainName)
        {
            return (parentDomain.FriendlyName.EnsureSuffix("/") + domainName).ToLowerInvariant();
        }

        public static List<AppDomain> GetAppDomains()
        {
            List<AppDomain> appDomains = new List<AppDomain>();
            IntPtr enumHandle = IntPtr.Zero;
            ICorRuntimeHost host = new CorRuntimeHost();

            try
            {
                host.EnumDomains(out enumHandle);

                object domain;

                while (true)
                {
                    host.NextDomain(enumHandle, out domain);

                    if (domain == null)
                    {
                        break;
                    }

                    appDomains.Add((AppDomain) domain);
                }

                return appDomains;
            }
            catch (System.Exception e)
            {
                throw new FailedToEnumerateAppDomainsException(e);
            }
            finally
            {
                host.CloseEnum(enumHandle);
                Marshal.ReleaseComObject(host);
            }
        }

        public virtual List<AppDomain> FilterAppDomains()
        {
            return GetAppDomains();
        }
    }

    public interface IAppDomain
    {
        string GetChildDomainPath(AppDomain parentDomain);
    }

    public class DefaultAppDomain : IAppDomain
    {
        public string GetChildDomainPath(AppDomain parentDomain)
        {
            throw new NotImplementedException();
        }
    }
}