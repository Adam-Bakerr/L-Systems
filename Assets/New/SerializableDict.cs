using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializableDict<T,U>
{

    [System.Serializable]
    public struct SerializablKeyValuePair
    {
        public T Key;
        public U Value;

        public SerializablKeyValuePair(T key, U value)
        {
            Key = key;
            Value = value;
        }
    }

    

    [SerializeField] public List<SerializablKeyValuePair> values = new List<SerializablKeyValuePair>();
    Dictionary<T, U> Contents = new Dictionary<T, U>();

    public bool Add(T Key,U Value)
    {
        if (!Contents.ContainsKey(Key))
        {
            Contents.Add(Key, Value);
            values.Add(new SerializablKeyValuePair(Key, Value));
            return true;
        }
        
        return false;
    }

    public bool Contains(T key)
    {
        if (Contents.ContainsKey(key))
        {
            return true;
        }
        return false;
    }

    public bool ContainsPair(SerializablKeyValuePair pair)
    {
        if (Contents.ContainsKey(pair.Key) && Contents.ContainsValue(pair.Value))
        {
            return true;
        }
        return false;
    }

    public void AddContents(SerializablKeyValuePair pair)
    {
        Contents.Add(pair.Key,pair.Value);
    }

    public int Count()
    {
        return Contents.Count;
    }

    public int ValuesCount()
    {
        return values.Count;
    }

    //Avoid Using
    public Dictionary<T,U> ExposeDict()
    {
        return Contents;
    }

    public U GetValue(T Key)
    {
        return Contents[Key];
    }

    public bool TryGetValue(T Key , ref U Out)
    {
        if(Contents.TryGetValue(Key,out Out))
        {
            return true;
        }
        return false;
    }
}
