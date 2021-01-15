﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance = null;
    public static T Instance
    {
        get
        {
            if(instance == null)
            {
                instance = (T)FindObjectOfType(typeof(T)) as T;
                if(instance == null)
                {
                    instance = new GameObject(typeof(T).ToString(), typeof(T)).GetComponent<T>();
                }
            }
                        
            return instance;
        }
    }
    private void OnApplicationQuit()
    {
        instance = null;
    }
}
