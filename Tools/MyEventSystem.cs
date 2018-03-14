using System;
using System.Collections.Generic;
using UnityEngine.EventSystems;


public class MyEventSystem
{
    static Dictionary<string, Action> actionDic = new Dictionary<string, Action>();

    public static void AddListener(string name,Action action)
    {
        if (actionDic.ContainsKey(name))
        {
            actionDic[name] += action;
            return;
        }

        actionDic[name] = action;
    }

    public static void Dispatch(string name)
    {
        if (actionDic.ContainsKey(name))
        {
            actionDic[name]();
        }
    }

    public static void  RemoveListener(string name,Action action)
    {
        if (actionDic.ContainsKey(name))
        {
            actionDic[name]-=action;
        }
    }

    static Dictionary<string, Action<System.Object>> actionDicWithParam =new Dictionary<string, Action<System.Object>>();


     public static void AddListener(string name,Action<System.Object> action)
    {
        if (actionDicWithParam.ContainsKey(name))
        {
            actionDicWithParam[name] += action;
            return;
        }

        actionDicWithParam[name] = action;
    }

     public static void Dispatch(string name, System.Object param)
    {
        if (actionDicWithParam.ContainsKey(name))
        {
            actionDicWithParam[name](param);
        }
    }

     public static void RemoveListener(string name, Action<System.Object> action)
    {
        if (actionDicWithParam.ContainsKey(name))
        {
            actionDicWithParam[name] -= action;
        }
    }
}
