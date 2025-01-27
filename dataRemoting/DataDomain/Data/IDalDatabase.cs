using System.Reflection;

namespace DataDomain
{
    public interface IDalDatabase : ITreeProjectionRepository
    {
        IIdentityProjectionRepository<string> Repositories { get; }
    }
}