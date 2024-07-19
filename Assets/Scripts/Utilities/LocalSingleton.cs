﻿using UnityEngine;

public abstract class LocalSingleton<T> : MonoBehaviour where T : Component
{

    #region Fields

   
    private static T instance;

    #endregion

    #region Properties

   
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>();
                if (instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = typeof(T).Name;
                    instance = obj.AddComponent<T>();
                }
            }
            return instance;
        }
    }

    #endregion

    #region Methods

   
    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
        }
    }

    #endregion

}