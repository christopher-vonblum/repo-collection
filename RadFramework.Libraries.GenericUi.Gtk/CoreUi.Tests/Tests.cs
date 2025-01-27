using System;
using System.Collections;
using System.Collections.Generic;
using CoreUi.Gtk;
using CoreUi.Model;
using CoreUi.Proxy;
using CoreUi.Tests.Ui.Model;
using Gtk;
using NSubstitute;
using NUnit.Framework;

namespace CoreUi.Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }
        
        [Test]
        public void Test1()
        {
            IInteractionProvider gtkInteractionProvider = new GtkInteractionProvider();

            var t = gtkInteractionProvider.CreateInputModel<IComplexModel>();

            
            gtkInteractionProvider.RequestInput(t, out t);
        }
        
        [Test]
        public void Test2()
        {
            IInteractionProvider gtkInteractionProvider = new GtkInteractionProvider();

            var t = gtkInteractionProvider.CreateInputModel<IChooseEntryField<IEnumerable<string>, string>>();

            t.Source = new[] {"A", "B", "C"};
            
            gtkInteractionProvider.RequestInput(t, out t);

            ;
        }
    }
}