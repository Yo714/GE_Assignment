using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventCenter : Singleton<EventCenter>
{
    public delegate void ProcessEvent(object obj, int Param1, int Param2);

    private Dictionary<string, ProcessEvent> EventMap = new Dictionary<string, ProcessEvent>();

    public void Regist(string name, ProcessEvent func)
    {
        if (EventMap.ContainsKey(name))
        {
            EventMap[name] += func;
        }
        else
        {
            EventMap[name] = func;
        }
    }

    public void UnRegist(string name, ProcessEvent func)
    {
        if (EventMap.ContainsKey(name))
        {
            EventMap[name] -= func;
        }
    }

    public void Trigger(string name, object obj, int Param1, int Param2)
    {
        if (EventMap.ContainsKey(name))
        {
            EventMap[name].Invoke(obj, Param1, Param2);
        }
    }
}
