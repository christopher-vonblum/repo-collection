﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     Dieser Code wurde von einem Tool generiert.
//     Laufzeitversion: 14.0.0.0
//  
//     Änderungen an dieser Datei können fehlerhaftes Verhalten verursachen und gehen verloren, wenn
//     der Code neu generiert wird.
// </auto-generated>
// ------------------------------------------------------------------------------

namespace CVB.NET.DataAccess.Sql.T4Queries.Queries.Record
{
    using System.Collections.Generic;
    using Model;
    using Model.SubModels;
    using Utils;

    /// <summary>
    /// Class to produce the template output
    /// </summary>
#line 1 "G:\$projects\CVB.NET.Solution\src\CVB.NET.DataAccess.Sql\src\T4Queries\Queries\Record\DeleteRecordQuery.tt"
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "14.0.0.0")]
    public partial class DeleteRecordQuery : CVB.NET.TextTemplating.Runtime.Base.QueryTemplateBase<DeleteRecordDescription>
    {
        /// <summary>
        /// Create the template output
        /// </summary>
        public override string TransformText()
        {
#line 3 "G:\$projects\CVB.NET.Solution\src\CVB.NET.DataAccess.Sql\src\T4Queries\Queries\Record\DeleteRecordQuery.tt"
/*Imports*/

#line default
#line hidden
            this.Write("\r\n");

#line 12 "G:\$projects\CVB.NET.Solution\src\CVB.NET.DataAccess.Sql\src\T4Queries\Queries\Record\DeleteRecordQuery.tt"
            foreach (KeyValuePair<string, List<ColumnValue>> tablePrimaryKeysPair in Description.TablePrimaryKeys)
            {
#line default
#line hidden
                this.Write("\t\t");

#line 14 "G:\$projects\CVB.NET.Solution\src\CVB.NET.DataAccess.Sql\src\T4Queries\Queries\Record\DeleteRecordQuery.tt"
/*Delete Record*/

#line default
#line hidden
                this.Write("\t\tDELETE FROM [");

#line 15 "G:\$projects\CVB.NET.Solution\src\CVB.NET.DataAccess.Sql\src\T4Queries\Queries\Record\DeleteRecordQuery.tt"
                this.Write(this.ToStringHelper.ToStringWithCulture(Description.Schema));

#line default
#line hidden
                this.Write("].[");

#line 15 "G:\$projects\CVB.NET.Solution\src\CVB.NET.DataAccess.Sql\src\T4Queries\Queries\Record\DeleteRecordQuery.tt"
                this.Write(this.ToStringHelper.ToStringWithCulture(tablePrimaryKeysPair.Key));

#line default
#line hidden
                this.Write("]\r\n\t\tWHERE ");

#line 16 "G:\$projects\CVB.NET.Solution\src\CVB.NET.DataAccess.Sql\src\T4Queries\Queries\Record\DeleteRecordQuery.tt"
                this.Write(this.ToStringHelper.ToStringWithCulture(SqlQueryUtils.MakeSqlCausalityChain(tablePrimaryKeysPair.Key, tablePrimaryKeysPair.Value, LogicalOperator.And)));

#line default
#line hidden
                this.Write(";\r\n");

#line 17 "G:\$projects\CVB.NET.Solution\src\CVB.NET.DataAccess.Sql\src\T4Queries\Queries\Record\DeleteRecordQuery.tt"
            }

#line default
#line hidden
            this.Write("\r\n");
            return this.GenerationEnvironment.ToString();
        }
    }

#line default
#line hidden
}