using System;
using System.Collections.Generic;

namespace DataDomain.Migrations
{
    public interface IClrTypeMigrationIdentity
    {
        Guid TypeId { get; set; }
        IEnumerable<IProperty> Properties { get; set; }
    }
}