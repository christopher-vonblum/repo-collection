namespace CVB.NET.Configuration.Serialization.Serializers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Text;

    public class DictionarySerializer
    {
        private Type T1;
        private Type T2;
        public DictionarySerializer(Type t1, Type t2)
        {
            T1 = t1;
            T2 = t2;
            T1NeedsConversion = T1 == typeof(string) || T1 == typeof(object);
            T1NeedsConversion = T2 == typeof(string) || T2 == typeof(object);
            T1Converter = new ConfigStringConverter(T1);
            T2Converter = new ConfigStringConverter(T2);
            DictionaryCtor = typeof (Dictionary<,>).MakeGenericType(T1, T2).GetConstructor(Type.EmptyTypes);
        } 

        private IStringConverter T1Converter;
        private IStringConverter T2Converter;

        private bool T1NeedsConversion;
        private bool T2NeedsConversion;

        private ConstructorInfo DictionaryCtor;

        public IDictionary Deserialize(string serializedValue)
        {
            string[] pairs = serializedValue.Split('|');

            IDictionary dictionary = (IDictionary)DictionaryCtor.Invoke(null);
            
            foreach (string pair in pairs)
            {
                string[] splitted = pair.Split('=');

                if (splitted.Length > 2)
                {
                    string key = splitted[0];

                    splitted = new string[] {key, serializedValue.Substring(key.Length + 1)};
                }
                
                dictionary.Add(StringToT1(splitted[0]), StringToT2(splitted[1]));
            }

            return dictionary;
        }
        
        public string Serialize(IDictionary value)
        {
            StringBuilder serializer = new StringBuilder();
            
            foreach (var kv in value.Keys)
            {
                serializer.Append(T1ToString(kv));
                serializer.Append("=");
                serializer.Append(T2ToString(value[kv]));
                serializer.Append("|");
            }

            return serializer.ToString().Substring(0, serializer.Length - 1);
        }
        
        private string T1ToString(object t1)
        {
            if (T1NeedsConversion)
            {
                return T1Converter.ConvertToString(t1);
            }

            return t1 as string;
        }

        private string T2ToString(object t2)
        {
            if (T2NeedsConversion)
            {
                return T2Converter.ConvertToString(t2);
            }

            return t2 as string;
        }

        private object StringToT1(string t1)
        {
            if (T1NeedsConversion)
            {
                return T1Converter.ConvertFromString(t1);
            }

            return t1;
        }

        private object StringToT2(string t2)
        {
            if (T2NeedsConversion)
            {
                return T2Converter.ConvertFromString(t2);
            }

            return t2;
        }
    }
}
