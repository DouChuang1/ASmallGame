using System;
using System.Collections;
using System.Collections.Generic;

public  interface IConfigManager {
    Dictionary<string, string> AppSetting { get; }

    /// <summary>
    /// 配置最大条数
    /// </summary>
    /// <returns></returns>
    int GetConfigItemCount();
}

[Serializable]
public class KeyValueNode
{
    public string Key;
    public string Value;
}

[Serializable]
public class KeyValueInfo
{
    public List<KeyValueNode> ConfigInfo = null;
}
