using System.Collections.Generic;
using System.Linq;
using CoreUi.Gtk.Base;
using CoreUi.Model;
using CoreUi.Proxy;
using CoreUi.Proxy.Factory;
using Gtk;

namespace CoreUi.Gtk.Dialog
{
    public class ChooseEntry<TSource, TElement> : FieldBase where TSource : IEnumerable<TElement>
    {
        private IChooseEntryField<TSource, TElement> field;

        public ChooseEntry(IObject controlState, PropertyDefinition propertyDefinition) : base(controlState, propertyDefinition)
        {
            var o = (IObject)ControlState[PropertyDefinition];
            
            if (o == null)
            {
                return;
            }
            
            this.field = (IChooseEntryField<TSource, TElement>)ProxyFactory.CreateProxyOrValue(typeof(IChooseEntryField<TSource, TElement>));

            ((IObjectProxy) field).Object = o;
            
            RadioButton prev = null;
            foreach (var option in field.Source)
            {
                var displayString = option.ToString();
                RadioButton choice = new RadioButton(displayString);
                choice.Name = $"{PropertyDefinition.Name}/{displayString}";
                choice.Label = displayString;

                if (prev != null)
                {
                    choice.JoinGroup(prev);
                }

                prev = choice;

                this.Add(choice);
            }
        }
        
        public override void SaveControlState()
        {
            foreach (Widget wd in this.Children)
            {
                if (wd is RadioButton r)
                {
                    RadioButton activeButton = r.Group.Single(gr => gr.Active);
                    var activeIndex = this.Children.ToList().IndexOf(activeButton);
                    field.Selected = field.Source.Skip(activeIndex + 1).Take(1).Single();
                    // only a single choice
                    break;
                }
            }
        }
    }
}