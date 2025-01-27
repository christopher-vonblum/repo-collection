using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using dataRemoting;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Driver;
namespace DataDomain
{
    public delegate bool ValidateCredentials(string user, string password);

    public delegate Guid IssueAuthToken(string user);

    public delegate bool ValidateAuthToken(string user, Guid authToken);

    public delegate int GetPreferredResourceCount(string user);

    public delegate bool ShouldUseHighPerformanceMultiplex(string user);
    public delegate bool ShouldEncryptHighPerformanceMultiplex(string user);
    
    public interface IClientResourceManager
    {
        object GetResource(Guid authToken);
    }
    
    public interface ITelemetryServerOptions
    {
        IPEndPoint ListenerEndpoint { get; set; }
        IServiceProvider ServiceProvider { get; set; }
        ValidateCredentials ValidateCredentials { get; set; }
        ValidateAuthToken ValidateAuthToken { get; set; }
        IssueAuthToken IssueAuthToken { get; set; }
        GetPreferredResourceCount GetPreferredResourceCount { get; set; }
        
        /// <summary>
        /// Use when high performance is needed.
        /// When true transfers the connection to an isolated port.
        /// </summary>
        ShouldUseHighPerformanceMultiplex ShouldUseHighPerformanceMultiplex { get; set; }
        
        /// <summary>
        /// Use when connecting containers or server on the same secure network and high performance is needed.
        /// </summary>
        ShouldEncryptHighPerformanceMultiplex ShouldEncryptHighPerformanceMultiplex { get; set; }
    }
    
    public interface ITelemetryServerApi
    {
        bool ValidateCredentials(string user, string password);

        Guid IssueAuthToken(string user);

        bool ValidateAuthToken(string user, Guid authToken);

        int GetPreferredResourceCount(string user);

        bool ShouldUseHighPerformanceMultiplex(string user);
        bool ShouldEncryptHighPerformanceMultiplex(string user);
    }

    class TelemetryServerApi : ITelemetryServerApi
    {
        private readonly ITelemetryServerOptions _options;

        private TcpListener listener;
        
        public TelemetryServerApi(ITelemetryServerOptions options)
        {
            _options = options;
            listener = new TcpListener(options.ListenerEndpoint);
            listener.BeginAcceptSocket(Callback, this);
        }

        private void Callback(IAsyncResult ar)
        {
            
        }

        public bool ValidateCredentials(string user, string password)
        {
            if (_options.ValidateCredentials != null)
            {
                return _options.ValidateCredentials(user, password);
            }

            throw new NotImplementedException();
        }

        public Guid IssueAuthToken(string user)
        {
            throw new NotImplementedException();
        }

        public bool ValidateAuthToken(string user, Guid authToken)
        {
            throw new NotImplementedException();
        }

        public int GetPreferredResourceCount(string user)
        {
            throw new NotImplementedException();
        }

        public bool ShouldUseHighPerformanceMultiplex(string user)
        {
            throw new NotImplementedException();
        }

        public bool ShouldEncryptHighPerformanceMultiplex(string user)
        {
            throw new NotImplementedException();
        }
    }

    public interface IResourceClient
    {
        string User { get; }
        Guid AuthToken { get; }
    }
    
    class Program
    {
        //private Type[] clrTypes = new[] {typeof(IClrAssembly), typeof(IClrType)};
        static void Main(string[] args)
        {
            IMongoClient client = new MongoClient("mongodb://localhost:27017");
            // "tlmts://localhost:8568/"
            client.DropDatabase("test2");
            
            DalDatabase.NewDatabase(new ServiceCollection().BuildServiceProvider(), client, "test2");
            
            
            
            /*IDalDatabase dal = new DalDatabase("mongodb://localhost:27017", "test");


            var typeRepo = dal.GetRepository<Guid>("/clrtypes");
            
            
            var b = typeof(A);
            var c = AppDomain.CurrentDomain.GetAssemblies().Where(a => a.GetName().Name == "dataRemoting").ToArray();
            foreach (var t in c.FirstOrDefault().ExportedTypes)
            {

            }*/
        }
    }
}