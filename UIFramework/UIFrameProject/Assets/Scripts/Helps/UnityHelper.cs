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

    /// <summary>
    /// 获取子节点脚本
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="go"></param>
    /// <param name="childName"></param>
    /// <returns></returns>
    public static T GetTheChildComponetScripts<T>(GameObject go,string childName) where T:Component
    {
        Transform searchTrans = null;
        searchTrans = FindChildNode(go, childName);
        if(searchTrans!=null)
        {
            return searchTrans.gameObject.GetComponent<T>();
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// 给子节点添加脚本
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="go"></param>
    /// <param name="childNode"></param>
    /// <returns></returns>
    public static T AddChildNodeComponent<T>(GameObject go,string childName) where T:Component
    {
        Transform searchTrans = null;
        searchTrans = FindChildNode(go, childName);
        if (searchTrans != null)
        {
            T[] coms = searchTrans.GetComponents<T>();
            for(int i=0;i<coms.Length;i++)
            {
                if(coms[i]!=null)
                {
                    Destroy(coms[i]);
                }
            }
            return searchTrans.gameObject.AddComponent<T>();
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// 给子节点添加父物体
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="child"></param>
    public static void AddChildNodeToParentNode(Transform parent,Transform child)
    {
        child.SetParent(parent);
        child.localPosition = Vector3.zero;
        child.localScale = Vector3.one;
        child.localEulerAngles = Vector3.zero;
    }

}
