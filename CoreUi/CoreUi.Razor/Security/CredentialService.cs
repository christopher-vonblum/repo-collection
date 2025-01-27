using CoreUi.Razor.Data;

namespace CoreUi.Razor.MultiClient
{
    public class CredentialService : ICredentialService
    {
        private const string salt = "$2a$10$NesNg4ZBuxJDXUOCpLpzTe";

        public bool ComparePassword(string protectedPassword, string password)
        {
            return protectedPassword == ProtectPassword(password);
        }
        
        public string ProtectPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, salt);
        }

    }
}