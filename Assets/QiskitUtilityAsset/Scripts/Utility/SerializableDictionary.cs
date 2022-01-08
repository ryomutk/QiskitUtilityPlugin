using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class SerializableDictionary<T, M>
{
    [Serializable]
    public class SerializableKeyValuePair<K, V>
    {
        public K key { get { return _key; } private set { _key = value; } }
        public V value { get { return _value; } set { _value = value; } }

        [SerializeField] K _key;
        [SerializeField] V _value;

        public SerializableKeyValuePair(K key, V value)
        {
            this.key = key;
            this.value = value;
        }
    }
    
    public M this[T index]
    {
        get
        {
            for (int i = 0; i < rawData.Count; i++)
            {
                if (rawData[i].key.Equals(index))
                {
                    return rawData[i].value;
                }
            }

            throw new KeyNotFoundException();
        }

        set
        {
            for (int i = 0; i < rawData.Count; i++)
            {
                if (rawData[i].key.Equals(index))
                {
                    rawData[i].value = value;
                    return;
                }
            }

            var item = new SerializableKeyValuePair<T, M>(index, value);
            rawData.Add(item);
        }
    }
    [SerializeField] List<SerializableKeyValuePair<T, M>> rawData = new List<SerializableKeyValuePair<T, M>>();

    public bool TryGetItem(T key,out M output)
    {
        for (int i = 0; i < rawData.Count; i++)
        {
            if (rawData[i].key.Equals(key))
            {
                output = rawData[i].value;
                return true;
            }
        }

        output = default(M);
        return false;
    }

    public bool ContainsKey(T key)
    {
        for(int i = 0;i < rawData.Count;i++)
        {
            if(rawData[i].key.Equals(key))
            {
                return true;
            }
        }

        return false;
    }

    public IEnumerator<SerializableKeyValuePair<T,M>> GetEnumerator()
    {
        return rawData.GetEnumerator();
    }
    
}