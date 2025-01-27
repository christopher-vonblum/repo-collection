namespace CVB.NET.DataAccess.Sql.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class KeyKeyDictionary<TKeyA, TKeyB>
    {
        private const string InvalidStateMessage = "Inner dictionaries are in invalid state. Object is broken, check implementation.";
        private IDictionary<TKeyA, TKeyB> AKeyDictionary { get; }
        private IDictionary<TKeyB, TKeyA> BKeyDictionary { get; }

        public TKeyA this[TKeyB keyB]
        {
            get { return BKeyDictionary[keyB]; }
            set
            {
                BKeyDictionary[keyB] = value;
                AKeyDictionary[value] = keyB;
            }
        }

        public TKeyB this[TKeyA keyA]
        {
            get { return AKeyDictionary[keyA]; }
            set
            {
                AKeyDictionary[keyA] = value;
                BKeyDictionary[value] = keyA;
            }
        }

        public int Count
        {
            get
            {
                if (AKeyDictionary.Count != BKeyDictionary.Count)
                {
                    throw new InvalidOperationException(InvalidStateMessage);
                }

                return AKeyDictionary.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                if (AKeyDictionary.IsReadOnly != BKeyDictionary.IsReadOnly)
                {
                    throw new InvalidOperationException(InvalidStateMessage);
                }

                return AKeyDictionary.IsReadOnly;
            }
        }

        public KeyKeyDictionary(IDictionary<TKeyA, TKeyB> singleIndexedDictionary)
        {
            AKeyDictionary = singleIndexedDictionary.ToDictionary(pair => pair.Key, pair => pair.Value);
            BKeyDictionary = singleIndexedDictionary.ToDictionary(pair => pair.Value, pair => pair.Key);
        }

        public KeyKeyDictionary(IDictionary<TKeyB, TKeyA> singleIndexedDictionary)
        {
            BKeyDictionary = singleIndexedDictionary.ToDictionary(pair => pair.Key, pair => pair.Value);
            AKeyDictionary = singleIndexedDictionary.ToDictionary(pair => pair.Value, pair => pair.Key);
        }

        public void Add(TKeyA keyA, TKeyB keyB)
        {
            AKeyDictionary.Add(keyA, keyB);
            BKeyDictionary.Add(keyB, keyA);
        }

        public void Add(KeyValuePair<TKeyA, TKeyB> pair)
        {
            AKeyDictionary.Add(pair);
            BKeyDictionary.Add(pair.Value, pair.Key);
        }

        public void Add(TKeyB keyB, TKeyA keyA)
        {
            AKeyDictionary.Add(keyA, keyB);
            BKeyDictionary.Add(keyB, keyA);
        }

        public void Add(KeyValuePair<TKeyB, TKeyA> pair)
        {
            BKeyDictionary.Add(pair);
            AKeyDictionary.Add(pair.Value, pair.Key);
        }

        public bool ContainsKey(TKeyA keyA)
        {
            return AKeyDictionary.ContainsKey(keyA);
        }

        public bool ContainsKey(TKeyB keyB)
        {
            return BKeyDictionary.ContainsKey(keyB);
        }

        public bool Contains(KeyValuePair<TKeyA, TKeyB> item)
        {
            return AKeyDictionary.Contains(item);
        }

        public bool Contains(KeyValuePair<TKeyB, TKeyA> item)
        {
            return BKeyDictionary.Contains(item);
        }

        public bool Remove(TKeyA keyA)
        {
            TKeyB keyB = AKeyDictionary[keyA];

            bool opStateA = AKeyDictionary.Remove(keyA),
                 opStateB = BKeyDictionary.Remove(keyB);

            if (opStateB != opStateA)
            {
                throw new InvalidOperationException(InvalidStateMessage);
            }

            return opStateA;
        }

        public bool Remove(TKeyB keyB)
        {
            TKeyA keyA = BKeyDictionary[keyB];

            bool opStateA = AKeyDictionary.Remove(keyA),
                 opStateB = BKeyDictionary.Remove(keyB);

            if (opStateB != opStateA)
            {
                throw new InvalidOperationException(InvalidStateMessage);
            }

            return opStateA;
        }

        public bool Remove(KeyValuePair<TKeyA, TKeyB> item)
        {
            bool opStateA = AKeyDictionary.Remove(item.Key),
                 opStateB = BKeyDictionary.Remove(item.Value);

            if (opStateB != opStateA)
            {
                throw new InvalidOperationException(InvalidStateMessage);
            }

            return opStateA;
        }

        public bool Remove(KeyValuePair<TKeyB, TKeyA> item)
        {
            bool opStateA = AKeyDictionary.Remove(item.Value),
                 opStateB = BKeyDictionary.Remove(item.Key);

            if (opStateB != opStateA)
            {
                throw new InvalidOperationException(InvalidStateMessage);
            }

            return opStateA;
        }

        public void Clear()
        {
            AKeyDictionary.Clear();
            BKeyDictionary.Clear();
        }

        public bool TryGetValue(TKeyA key, out TKeyB value)
        {
            return AKeyDictionary.TryGetValue(key, out value);
        }

        public bool TryGetValue(TKeyB key, out TKeyA value)
        {
            return BKeyDictionary.TryGetValue(key, out value);
        }

        public void CopyTo(KeyValuePair<TKeyA, TKeyB>[] array, int arrayIndex)
        {
            AKeyDictionary.CopyTo(array, arrayIndex);
        }

        public void CopyTo(KeyValuePair<TKeyB, TKeyA>[] array, int arrayIndex)
        {
            BKeyDictionary.CopyTo(array, arrayIndex);
        }
    }
}