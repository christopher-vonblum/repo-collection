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
    using Model.Base;

    /// <summary>
    /// Class to produce the template output
    /// </summary>
#line 1 "G:\$projects\CVB.NET.Solution\src\CVB.NET.DataAccess.Sql\src\T4Queries\Queries\Record\WriteQueryFooter.tt"
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "14.0.0.0")]
    public partial class WriteQueryFooter : CVB.NET.TextTemplating.Runtime.Base.QueryTemplateBase<SchemaScopedQueryDescription>
    {
        /// <summary>
        /// Create the template output
        /// </summary>
        public override string TransformText()
        {
#line 3 "G:\$projects\CVB.NET.Solution\src\CVB.NET.DataAccess.Sql\src\T4Queries\Queries\Record\WriteQueryFooter.tt"
/*Imports*/

#line default
#line hidden
            this.Write("\r\nCOMMIT TRAN writeTransaction");
            return this.GenerationEnvironment.ToString();
        }
    }

#line default
#line hidden
}