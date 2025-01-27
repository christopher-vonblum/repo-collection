namespace CoreUi.Razor.MultiClient
{
    public interface ICredentialService
    {
        bool ComparePassword(string credentialPath, string password);
        string ProtectPassword(string password);
    }
}