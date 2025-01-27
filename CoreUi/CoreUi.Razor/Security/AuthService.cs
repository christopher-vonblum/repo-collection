using System.Linq;
using System.Security.Claims;
using CoreUi.Razor.Data;
using Microsoft.AspNetCore.Http;

namespace CoreUi.Razor.MultiClient
{
    public class AuthService : IAuthService
    {
        public string CurrentUser
        {
            get
            {
                return _httpContextAccessor.HttpContext.User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value;
            }
        }

        private readonly ICredentialService _credentialService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDataProvider _dataProvider;

        public AuthService(ICredentialService credentialService, IHttpContextAccessor httpContextAccessor, IDataProvider dataProvider)
        {
            _credentialService = credentialService;
            _httpContextAccessor = httpContextAccessor;
            _dataProvider = dataProvider;
        }

        public bool ValidateCredentials(string contextUsername, string contextPassword)
        {
            if (_credentialService.ComparePassword(_dataProvider.Load<string>(GetCredentialPath(contextUsername)), contextPassword))
            {
                return true;
            }

            return false;
        }

        private string GetCredentialPath(string username)
        {
            return $"/users/{username}/credential";
        }
    }
}