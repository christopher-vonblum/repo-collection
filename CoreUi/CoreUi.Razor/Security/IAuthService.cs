namespace CoreUi.Razor.MultiClient
{
    public interface IAuthService
    {
        bool ValidateCredentials(string contextUsername, string contextPassword);
        string CurrentUser { get; }
    }
}