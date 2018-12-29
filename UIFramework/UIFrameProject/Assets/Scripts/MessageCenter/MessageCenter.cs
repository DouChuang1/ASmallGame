using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageCenter  {

    public delegate void DelMessageHandle(KeyValuesUpdate kv);

    public static Dictionary<string, DelMessageHandle> _dicMessage = new Dictionary<string, DelMessageHandle>();

    public static void Register(string message, DelMessageHandle handler)
    {
        if(!_dicMessage.ContainsKey(message))
        {
            _dicMessage.Add(message, null);
        }
        _dicMessage[message] += handler;
    }

    public static void UnRegister(string message, DelMessageHandle handler)
    {
        if (_dicMessage.ContainsKey(message))
        {
            _dicMessage[message] -= handler;
        }
        
    }

    public static void UnRegisterAll()
    {
        if (_dicMessage!=null)
        {
            _dicMessage.Clear();
        }

    }

    public static void Post(string message,KeyValuesUpdate kv)
    {
       if(_dicMessage[message]!=null)
        {
            _dicMessage[message](kv);
        }
    }

}

public class KeyValuesUpdate
{
    private string _key;
    private object _Values;

    public string Key
    {
        get
        {
            return _key;
        }
    }

    public object Values
    {
        get
        {
            return _Values;
        }
    }

    public KeyValuesUpdate(string k,object v)
    {
        _key = k;
        _Values = v;
    }
}
