using System;
using System.Collections.Generic;
using CoreUi.Attributes;
using CoreUi.Model;

namespace CoreUi.Tests.Ui.Model
{
    public interface IComplexModel
    {
        [Description("This property is just there to make things more complicated.")]
        IComplexModel Inner { get; set; }
        IEnumerable<string> Test { get; set; }
        IEnumerable<ITestModel> Test2 { get; set; }
        ITestModel Model1 { get; set; }
        
        ITestModel Model2 { get; set; }
        DateTime Cal { get; set; }
        bool Check { get; set; }
        
        bool? CheckButNullable { get; set; }
        
        [File]
        string File { get; set; }
        
        CheckMe Cheeeck { get; set; }
        
        ChooseMe Choose { get; set; }
        
        IChooseEntryField<IEnumerable<string>, string> CollectionChooseTest { get; set; }

        [BindImplementation]
        IRuntimeProvider Provider { get; set; }
    }
}