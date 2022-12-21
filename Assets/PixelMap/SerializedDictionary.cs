using System;
using System.Collections.Generic;
using UnityEngine;
namespace PixelMapMaker
{
    [Serializable]
    public class SerializedDictionary<Tkey, TValue> : Dictionary<Tkey, TValue>, ISerializationCallbackReceiver
    {
        [Serializable]
        public struct Pair
        {
            public Tkey key;
            public TValue value;

            public static implicit operator KeyValuePair<Tkey, TValue>(Pair i_pair)
            {
                return new KeyValuePair<Tkey, TValue>(i_pair.key, i_pair.value);
            }

            public static implicit operator Pair(KeyValuePair<Tkey, TValue> i_pair)
            {
                return new Pair
                {
                    key = i_pair.Key,
                    value = i_pair.Value
                };
            }
        }

        [SerializeField] private List<Pair> entries = new();

        public SerializedDictionary()
        {
        }

        public SerializedDictionary(IDictionary<Tkey, TValue> dictionary) : base(dictionary)
        {
        }

        public void OnBeforeSerialize()
        {
            entries.Clear();
            foreach (var pair in this)
                entries.Add(pair);
        }

        public void OnAfterDeserialize()
        {
            Clear();
            foreach (var entry in entries)
                this[entry.key] = entry.value;
        }
    }
}

