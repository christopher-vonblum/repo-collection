namespace CVB.NET.DataAccess.Sql.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class SqlTypeMapping
    {
        private static Dictionary<string, Type> PrimitiveTypeMapping { get; } =
            new Dictionary<string, Type>
            {
                {"bigint", typeof (long)},
                {"binary", typeof (byte[])},
                {"bit", typeof (bool)},
                {"datetime", typeof (DateTime)},
                {"decimal", typeof (decimal)},
                {"float", typeof (double)},
                {"int", typeof (int)},
                {"nvarchar(MAX)", typeof (string)},
                {"real", typeof (float)},
                {"smallint", typeof (short)},
                {"time", typeof (TimeSpan)},
                {"tinyint", typeof (byte)},
                {"uniqueidentifier", typeof (Guid)},
                {"varbinary", typeof (byte[])}
            };

        public static string GetSqlType(Type clrType)
        {
            /*SqlCommand cmd = new SqlCommand("SELECT SqlServerDataType FROM [System.SqlClrTypeMap] WHERE ClrType = '" + clrType.FullName + "'", connection);

            return (string)cmd.ExecuteScalar();*/

            return PrimitiveTypeMapping.FirstOrDefault(type => type.Value == clrType).Key;
        }

        public static bool HasSqlType(Type clrType)
        {
            return PrimitiveTypeMapping.ContainsValue(clrType);
        }
    }
}