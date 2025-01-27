namespace CVB.NET.DataAccess.Repository.GenericModel
{
    using System.Collections.Generic;
    using System.Linq;
    using PostSharp.Patterns.Contracts;

    public static class DictionaryExtensions
    {
        public static IDictionary<TKey, TValue> MergeLeft<TDictionary, TKey, TValue>([NotNull] this TDictionary dictionary, [NotEmpty] params IDictionary<TKey, TValue>[] others)
            where TDictionary : IDictionary<TKey, TValue>
        {
            IDictionary<TKey, TValue> newMap = dictionary.ToDictionary(key => key.Key, val => val.Value);

            List<IDictionary<TKey, TValue>> dictionariesList = new List<IDictionary<TKey, TValue>>();

            dictionariesList.Add(dictionary);

            dictionariesList.AddRange(others);

            foreach (IDictionary<TKey, TValue> mergeDictionary in dictionariesList)
            {
                if (mergeDictionary == null)
                {
                    continue;
                }

                foreach (KeyValuePair<TKey, TValue> mergePair in mergeDictionary)
                {
                    newMap[mergePair.Key] = mergePair.Value;
                }
            }

            return newMap;
        }
    }
}