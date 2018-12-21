using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityHelper : MonoBehaviour {

    /// <summary>
    /// 遍历查找子节点
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="childName"></param>
    /// <returns></returns>
	public static Transform FindChildNode(GameObject parent,string childName)
    {
        Transform searchTrans = null;
        searchTrans = parent.transform.Find(childName);
        if(searchTrans==null)
        {
            foreach(Transform trans in parent.transform)
            {
                searchTrans = FindChildNode(trans.gameObject, childName);
                if(searchTrans!=null)
                {
                    return searchTrans;
                }
            }
        }

        return searchTrans;
    }
}
