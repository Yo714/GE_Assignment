using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> where T:new()
{
    public static T s_Instance;

    public static T GetInstance()
    {
        if(s_Instance == null)
        {
            s_Instance = new T();
        }
        return s_Instance;
    }
}
