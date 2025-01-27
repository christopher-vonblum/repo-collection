namespace CVB.NET.DataAccess.Sql.T4Queries.Model.SubModels
{
    using System;
    using MetaData;
    using Reflection.Caching.Cached;

    public class ColumnDescription : INamedColumn, IColumnDescription
    {
        private bool allowNull;

        private string referenceColumn;
        private string referenceTable;

        public ColumnDescription(OrmSqlTypeMetaDataInfoView declaringType, CachedPropertyInfo property, string columnType, string columnName)
        {
            DeclaringType = declaringType;
            PropertyInfo = property;
            Type = columnType;
            Name = columnName;
        }

        public CachedPropertyInfo PropertyInfo { get; set; }
        public string Type { get; }

        public bool IsPrimaryKey { get; set; }

        public bool AllowNull
        {
            get { return allowNull; }
            set
            {
                if (IsPrimaryKey)
                {
                    throw new NotSupportedException("Null values not supported for Foreign or primary keys.");
                }

                allowNull = value;
            }
        }

        public bool IsForeignKey => ReferenceTable != null;

        public string ReferenceTable
        {
            get { return referenceTable; }
            set { referenceTable = value; }
        }

        public string ReferenceColumn
        {
            get
            {
                if (referenceColumn == null)
                {
                    return Name;
                }

                return referenceColumn;
            }
            set { referenceColumn = value; }
        }

        public OrmSqlTypeMetaDataInfoView DeclaringType { get; }

        public string Name { get; }
    }
}