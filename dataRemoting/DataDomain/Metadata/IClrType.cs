using System;
using System.Collections.Generic;

namespace DataDomain
{
    public interface IClrType
    {
        IList<IProperty> Properties { get; set; }
        Type GetRuntimeType();
    }
}