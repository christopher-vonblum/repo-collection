﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     Dieser Code wurde von einem Tool generiert.
//     Laufzeitversion: 14.0.0.0
//  
//     Änderungen an dieser Datei können fehlerhaftes Verhalten verursachen und gehen verloren, wenn
//     der Code neu generiert wird.
// </auto-generated>
// ------------------------------------------------------------------------------

namespace CVB.NET.DataAccess.Sql.T4Queries.Queries.Table
{
    using System.Collections.Generic;
    using Model.SubModels;
    using Utils;

    /// <summary>
    /// Class to produce the template output
    /// </summary>
#line 1 "G:\$projects\CVB.NET.Solution\src\CVB.NET.DataAccess.Sql\src\T4Queries\Queries\Table\CreateTableConstraintsQuery.tt"
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "14.0.0.0")]
    public partial class CreateTableConstraintsQuery : CVB.NET.TextTemplating.Runtime.Base.QueryTemplateBase<ITableDescription>
    {
        /// <summary>
        /// Create the template output
        /// </summary>
        public override string TransformText()
        {
#line 3 "G:\$projects\CVB.NET.Solution\src\CVB.NET.DataAccess.Sql\src\T4Queries\Queries\Table\CreateTableConstraintsQuery.tt"
/*Imports*/

#line default
#line hidden
            this.Write("\r\n");

#line 12 "G:\$projects\CVB.NET.Solution\src\CVB.NET.DataAccess.Sql\src\T4Queries\Queries\Table\CreateTableConstraintsQuery.tt"
/*Set Foreign Keys*/

#line default
#line hidden

#line 13 "G:\$projects\CVB.NET.Solution\src\CVB.NET.DataAccess.Sql\src\T4Queries\Queries\Table\CreateTableConstraintsQuery.tt"
            foreach (KeyValuePair<string, List<IColumnDescription>> columnGroup in Description.ForeignKeyDescriptionsGroupedByTable)
            {
#line default
#line hidden
                this.Write("\t\tALTER TABLE [");

#line 15 "G:\$projects\CVB.NET.Solution\src\CVB.NET.DataAccess.Sql\src\T4Queries\Queries\Table\CreateTableConstraintsQuery.tt"
                this.Write(this.ToStringHelper.ToStringWithCulture(Description.Schema));

#line default
#line hidden
                this.Write("].[");

#line 15 "G:\$projects\CVB.NET.Solution\src\CVB.NET.DataAccess.Sql\src\T4Queries\Queries\Table\CreateTableConstraintsQuery.tt"
                this.Write(this.ToStringHelper.ToStringWithCulture(Description.Table));

#line default
#line hidden
                this.Write("]\r\n\t\tADD CONSTRAINT [FK___TABLE_");

#line 16 "G:\$projects\CVB.NET.Solution\src\CVB.NET.DataAccess.Sql\src\T4Queries\Queries\Table\CreateTableConstraintsQuery.tt"
                this.Write(this.ToStringHelper.ToStringWithCulture(Description.Table));

#line default
#line hidden
                this.Write("_");

#line 16 "G:\$projects\CVB.NET.Solution\src\CVB.NET.DataAccess.Sql\src\T4Queries\Queries\Table\CreateTableConstraintsQuery.tt"
                this.Write(this.ToStringHelper.ToStringWithCulture(SqlQueryUtils.GetSeperatedColumnNames(columnGroup.Value, "_", false)));

#line default
#line hidden
                this.Write("___REFS_");

#line 16 "G:\$projects\CVB.NET.Solution\src\CVB.NET.DataAccess.Sql\src\T4Queries\Queries\Table\CreateTableConstraintsQuery.tt"
                this.Write(this.ToStringHelper.ToStringWithCulture(columnGroup.Key));

#line default
#line hidden
                this.Write("_");

#line 16 "G:\$projects\CVB.NET.Solution\src\CVB.NET.DataAccess.Sql\src\T4Queries\Queries\Table\CreateTableConstraintsQuery.tt"
                this.Write(this.ToStringHelper.ToStringWithCulture(SqlQueryUtils.GetSeperatedReferenceColumnNames(columnGroup.Value, "_", false)));

#line default
#line hidden
                this.Write("]\r\n\t\t\tFOREIGN KEY\r\n\t\t\t(\r\n\t\t\t\t");

#line 19 "G:\$projects\CVB.NET.Solution\src\CVB.NET.DataAccess.Sql\src\T4Queries\Queries\Table\CreateTableConstraintsQuery.tt"
                this.Write(this.ToStringHelper.ToStringWithCulture(SqlQueryUtils.GetSeperatedColumnNames(columnGroup.Value, ", ", true)));

#line default
#line hidden
                this.Write("\r\n\t\t\t)\r\n\t\t\tREFERENCES [");

#line 21 "G:\$projects\CVB.NET.Solution\src\CVB.NET.DataAccess.Sql\src\T4Queries\Queries\Table\CreateTableConstraintsQuery.tt"
                this.Write(this.ToStringHelper.ToStringWithCulture(Description.Schema));

#line default
#line hidden
                this.Write("].[");

#line 21 "G:\$projects\CVB.NET.Solution\src\CVB.NET.DataAccess.Sql\src\T4Queries\Queries\Table\CreateTableConstraintsQuery.tt"
                this.Write(this.ToStringHelper.ToStringWithCulture(columnGroup.Key));

#line default
#line hidden
                this.Write("]\r\n\t\t\t(\r\n\t\t\t\t");

#line 23 "G:\$projects\CVB.NET.Solution\src\CVB.NET.DataAccess.Sql\src\T4Queries\Queries\Table\CreateTableConstraintsQuery.tt"
                this.Write(this.ToStringHelper.ToStringWithCulture(SqlQueryUtils.GetSeperatedReferenceColumnNames(columnGroup.Value, ", ", true)));

#line default
#line hidden
                this.Write("\r\n\t\t\t);\r\n");

#line 25 "G:\$projects\CVB.NET.Solution\src\CVB.NET.DataAccess.Sql\src\T4Queries\Queries\Table\CreateTableConstraintsQuery.tt"
            }

#line default
#line hidden
            return this.GenerationEnvironment.ToString();
        }
    }

#line default
#line hidden
}