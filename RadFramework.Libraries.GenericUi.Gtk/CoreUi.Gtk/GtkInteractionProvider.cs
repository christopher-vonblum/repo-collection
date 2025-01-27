using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CoreUi.Attributes;
using CoreUi.Gtk.Dialog;
using CoreUi.Model;
using CoreUi.Objects;
using CoreUi.Proxy;
using CoreUi.Proxy.Factory;
using Gtk;

namespace CoreUi.Gtk
{
    public class GtkInteractionProvider : IInteractionProvider
    {
        public GtkInteractionProvider()
        {
            Application.Init();
        }
        
        public T CreateInputModel<T>()
        {
            return ProxyFactory.CreateInputModel<T>();
        }

        public object CreateInputModel(Type t)
        {
            return ProxyFactory.CreateProxyOrValue(t);
        }

        public bool RequestInput(Type tInput, object template, out object output)
        {
            bool res = RequestInputInternal(tInput, template, out output);
            return res;
        }

        public bool RequestInput<TInput>(TInput template, out TInput value)
        {
            bool res = RequestInput(typeof(TInput), template, out object v);
            value = (TInput) v;
            return res;
        }

        public bool RequestInput<TInput>(out TInput value)
        {
            return RequestInput(default(TInput), out value);
        }

        public bool RequestInput<TInput>(TInput template)
        {
            return RequestInput(template, out TInput o);
        }

        public TElement RequestDecision<TSource, TElement>(TSource source, Func<TElement, string> getDisplayString) where TSource : IEnumerable<TElement>
        {
            getDisplayString = getDisplayString ?? (element => element.ToString()); 
            int i = 0;
            
            IObject dataObject = new O();
            
            Dictionary<int, TElement> choices = new Dictionary<int, TElement>();
            
            foreach (object element in source)
            {
                dataObject.CreateProperty(new PropertyDefinition
                {
                    Name = getDisplayString((TElement) element),
                    ClrType = typeof(bool),
                });

                choices[i] = (TElement) element;
                i++;
            }

            PropertyDefinition choiceProp = null;
            
            using (RequestDecisionDialog d = new RequestDecisionDialog(typeof(TElement), dataObject, this))
            {
                d.Title = typeof(TElement).FullName;
                d.Init();
                if (RunDialog(d) == (int) ResponseType.Ok)
                {
                    choiceProp = d.ChoiceProperty;
                    d.UpdateData();
                    d.Destroy();
                }

                d.Destroy();
            }

            return choices[(int)dataObject[choiceProp]];
        }

        public void Messsage(string message)
        {
            using (global::Gtk.Dialog d = new global::Gtk.Dialog())
            {
                d.HeightRequest = 300;
                d.WidthRequest = 300;

                TextView v = new TextView();
                d.ContentArea.Add(v);

                v.Expand = true;
                v.Buffer.Text = message;
                d.AddButton("Ok", ResponseType.Ok);
                d.ShowAll();

                d.Run();

                d.Destroy();
            }
        }

        public bool RequestInputInternal(Type tInput, object template, out object value, Window parent = null, string propertyName = null)
        {
            IObject dialogData = (template as IObjectProxy)?.Object ?? new O();
            
            if (IsSimpleData(tInput))
            {
                return RequestSimpleValue(tInput, parent, dialogData, out value);
            }

            value = template ?? ProxyFactory.CreateProxyOrValue(tInput);
            ((IObjectProxy) value).Object = dialogData;
            
            if (IsCollectionData(tInput))
            {
                bool res = RequestCollection(tInput, parent, propertyName, dialogData);
                return res;
            }

            if (tInput.IsGenericType && tInput.GetGenericTypeDefinition() == typeof(IChooseEntryField<,>))
            {
                var t = typeof(IChooseEntryField<,>).MakeGenericType(tInput.GetGenericArguments());
                
                t.GetProperty("Selected")
                    .SetValue(template, 
                        this.GetType()
                                    .GetMethod(nameof(RequestDecision))
                                    .MakeGenericMethod(tInput.GetGenericArguments())
                                    .Invoke(
                                        this, 
                                        new[]
                                        {
                                            t.GetProperty("Source").GetValue(template),
                                            null
                                        }));
                
                return true;
            }
            
            return RequestComplexType(tInput, parent, propertyName, dialogData);
        }

        public bool RequestComplexType(Type tInput, Window parent, string propertyName, IObject dialogData)
        {
            IEnumerable<PropertyDefinition> fields = tInput
                .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy)
                .ToList()
                .Select(p => new PropertyDefinition()
                {
                    Name = p.DeclaringType.Name + "." + p.Name,
                    ClrType = p.PropertyType
                });

            using (TypeAwareDataInputDialog d = new TypeAwareDataInputDialog(dialogData, fields, this, parent))
            {
                d.Contract = tInput;
                d.Title = propertyName ?? String.Empty + " : " + tInput.FullName;
                d.Init();
                if (RunDialog(d) == (int) ResponseType.Ok)
                {
                    d.UpdateData();
                    d.Destroy();
                    return true;
                }

                d.Destroy();
            }
            
            return false;
        }

        public bool RequestSimpleValue(Type tInput, Window parent, IObject dialogData, out object value)
        {
            PropertyDefinition prop = new PropertyDefinition
            {
                Name = "Element",
                ClrType = tInput
            };
            
            dialogData.CreateProperty(prop);
            using (TypeAwareDataInputDialog d = new TypeAwareDataInputDialog(dialogData,
                new[]
                {
                    prop
                },
                this,
                parent))
            {
                d.Title = "Element : " + tInput.FullName;
                d.Init();
                if (RunDialog(d) == (int) ResponseType.Ok)
                {
                    d.UpdateData();
                    d.Destroy();
                    value = dialogData[prop];
                    return true;
                }

                d.Destroy();
            }

            value = tInput.IsValueType ? Activator.CreateInstance(tInput) : null;
            return false;
        }

        public virtual int RunDialog(global::Gtk.Dialog d)
        {
            return d.Run();
        }

        public bool RequestCollection(Type tInput, Window parent, string propertyName, IObject dialogData)
        {
            Type el = tInput.GetGenericArguments()[0];
           
            using (CollectionInputDialog d = new CollectionInputDialog(el, dialogData, this, parent))
            {
                d.Contract = tInput;
                d.Title = propertyName ?? String.Empty + " : " + el.FullName;
                d.Init();
                if (RunDialog(d) == (int) ResponseType.Ok)
                {
                    d.UpdateData();
                    d.Destroy();
                    return true;
                }

                d.Destroy();
            }

            return false;
        }

        public bool IsSimpleData(Type fieldDefinition)
        {
            return ProxyFactory.IsSimpleField(fieldDefinition);
        }

        public bool IsCollectionData(Type fieldDefinition)
        {
            return !IsSimpleData(fieldDefinition) 
                   && typeof(IEnumerable).IsAssignableFrom(fieldDefinition);
        }

    }
}