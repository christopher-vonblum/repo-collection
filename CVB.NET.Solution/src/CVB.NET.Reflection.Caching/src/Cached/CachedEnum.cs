namespace CVB.NET.Reflection.Caching.Cached
{
    using System;
    using System.Collections.Generic;
    using Base;
    using Wrapper;

    public sealed class CachedEnum : ReflectionSubInfoWrapperBase<Type, CachedAssembly>
    {
        public Array Values => values.Value;
        public string[] StringValues => stringValues.Value;

        private DebuggableLazy<string[]> stringValues;
        private DebuggableLazy<Array> values;

        internal CachedEnum(Type info) : base(info)
        {
            values = new DebuggableLazy<Array>(() => Enum.GetValues(info));
            stringValues = new DebuggableLazy<string[]>(() =>
                                                        {
                                                            List<string> stringVals = new List<string>();

                                                            foreach (object value in Values)
                                                            {
                                                                stringVals.Add(value.ToString());
                                                            }

                                                            return stringVals.ToArray();
                                                        });
        }

        public override string GetCacheKeyIdentifier()
        {
            return "[Enum]:" + InnerReflectionInfo.MetadataToken;
        }

        protected override CachedAssembly GetDeclaringReflectionInfo()
        {
            return InnerReflectionInfo.Assembly;
        }
    }
}