namespace CVB.NET.Ui.Generic.WindowsForms.Exception
{
    using System;

    public class PropertyTypeNotHandledException : System.Exception
    {
        public Type PropertyType { get; }

        public PropertyTypeNotHandledException(Type propertyType)
        {
            PropertyType = propertyType;
        }
    }
}