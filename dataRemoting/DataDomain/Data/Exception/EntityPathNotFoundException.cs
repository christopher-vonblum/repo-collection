using System;

namespace DataDomain
{
    internal class EntityPathNotFoundException : Exception
    {
        public EntityPathNotFoundException(string path) : base(path)
        {
        }
    }
}