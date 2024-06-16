using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("")]
[DisallowMultipleComponent]
public abstract class SingletonMono<T> : MonoBehaviour where T : SingletonMono<T>
{
    private static T s_instance;
    public static T Instance => s_instance;

    public static void Create()
    {
        if (s_instance != null)
        {
            return;
        }
        
        GameObject go = new GameObject(typeof(T).Name);
        s_instance = go.AddComponent<T>();
        
        s_instance.Init();
        
        GameObject.DontDestroyOnLoad(go);
    }
    
    protected virtual void Init(){}
    
    protected void OnDestroy()
    {
        if (s_instance == this)
        {
            s_instance = null;
        }
    }
}
