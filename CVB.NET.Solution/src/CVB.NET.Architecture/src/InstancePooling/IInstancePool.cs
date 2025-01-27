using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVB.NET.Architecture.src.Pooling
{
    using Abstractions.Ioc.Provider;

    interface IInstancePool<TInstance> where TInstance : class
    {
        TInstance GetInstance();
    }
}
