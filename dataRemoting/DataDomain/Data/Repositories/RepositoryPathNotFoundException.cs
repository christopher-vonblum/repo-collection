using System;

namespace DataDomain
{
    internal class RepositoryPathNotFoundException : Exception
    {
        public RepositoryPathNotFoundException(string path) : base(path)
        {
        }
    }
}