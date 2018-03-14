using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class ResourceManager:Singleton<ResourceManager>
{
    public Dictionary<string, UnityEngine.Object> resourceDic = new Dictionary<string, UnityEngine.Object>();

    public GameObject Load(string path)
    {
        if (resourceDic.ContainsKey(path))
        {
            return resourceDic[path] as GameObject;
        }

        GameObject go = Resources.Load(path) as GameObject;
        resourceDic[path] = go;
        return go;
    }

    public Hashtable rescourceTab = new Hashtable();

    public T Load<T>(string path) where T:UnityEngine.Object
    {
        if (rescourceTab.ContainsKey(path))
        {
            return rescourceTab[path] as T;
        }

        T t = Resources.Load<T>(path);
        rescourceTab[path] = t;
        return t;
    }
}
