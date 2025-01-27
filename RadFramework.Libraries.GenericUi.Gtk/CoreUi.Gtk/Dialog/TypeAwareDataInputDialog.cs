using System;
using System.Collections.Generic;
using System.Linq;
using CoreUi.Attributes;
using CoreUi.Gtk.Base;
using CoreUi.Gtk.Widgets.Field;
using CoreUi.Model;
using CoreUi.Proxy;
using Gtk;

namespace CoreUi.Gtk.Dialog
{
    public class TypeAwareDataInputDialog : DataDialogBase
    {
        public TypeAwareDataInputDialog(IObject dataObject, IEnumerable<PropertyDefinition> fields, GtkInteractionProvider interactionProvider, Window parentDialog = null) : base(dataObject, fields, interactionProvider, parentDialog)
        {

        }

        protected override Widget RenderField(PropertyDefinition property)
        {
            if (property.ClrType.IsGenericType && typeof(IChooseEntryField<,>) == property.ClrType.GetGenericTypeDefinition())
            {
                return (Widget)typeof(ChooseEntry<,>)
                    .MakeGenericType(property.ClrType.GetGenericArguments())
                    .GetConstructor(new []{typeof(IObject), typeof(PropertyDefinition)})
                    .Invoke(new object[]{ControlState, property});
            }
            else if (property.ClrType.IsGenericType && typeof(ISelectEntriesField<,>) == property.ClrType.GetGenericTypeDefinition())
            {
                
            }
            else if (property.ClrType == typeof(string) && property.Attributes.OfType<FileAttribute>().Any())
            {
                return new SelectFile(ControlState, property);
            }
            else if (property.ClrType == typeof(DateTime))
            {
                return new CalendarDateTime(ControlState, property);
            } 
            else if (property.ClrType == typeof(bool))
            {
                return new CheckBool(ControlState, property);
            }
            else if (typeof(Enum).IsAssignableFrom(property.ClrType))
            {
                if (property.ClrType.GetCustomAttributes(false).OfType<FlagsAttribute>().Any())
                {
                    return new CheckFlagsOnEnum(ControlState, property);
                }
                
                return new ChooseEntryFromEnum(ControlState, property);
            }
            else if (property.ClrType.IsGenericType && typeof(Nullable<>) == property.ClrType.GetGenericTypeDefinition())
            {
                property.ClrType = property.ClrType.GetGenericArguments()[0];
                return this.RenderField(property);
            }

            return base.RenderField(property);
        }
    }
}