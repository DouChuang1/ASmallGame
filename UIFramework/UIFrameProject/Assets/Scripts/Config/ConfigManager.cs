using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigManager : IConfigManager
{
    private static Dictionary<string, string> _AppSetting;

    public Dictionary<string, string> AppSetting
    {
        get
        {
            return _AppSetting;
        }
    }

    public int GetConfigItemCount()
    {
        return _AppSetting.Count;
    }

    public ConfigManager(string jsonPath)
    {
        _AppSetting = new Dictionary<string, string>();
        InitConfigDataByJson(jsonPath);
    }

    private void InitConfigDataByJson(string jsonPath)
    {
        TextAsset textAsset = null;
        KeyValueInfo keyValueObj = null;

        if (String.IsNullOrEmpty(jsonPath)) return;
        try
        {
            textAsset = Resources.Load<TextAsset>(jsonPath);
            Debug.LogError("LOG" + textAsset.text);
            keyValueObj = JsonUtility.FromJson<KeyValueInfo>(textAsset.text);
            //textAsset = Resources.Load<TextAsset>(jsonPath);
        }
        catch
        {
            Debug.LogError("Config Error");
        }

        foreach(KeyValueNode ki in keyValueObj.ConfigInfo)
        {
            _AppSetting.Add(ki.Key, ki.Value);
        }
    }

}
