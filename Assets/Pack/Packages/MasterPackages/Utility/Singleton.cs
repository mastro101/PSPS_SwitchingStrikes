using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SingletonMaster<T> : MonoBehaviour where T : SingletonMaster<T>
{
    protected static T instance;

    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = gameObject.GetComponent<T>();
            instance.Setup();
            DontDestroyOnLoad(instance.gameObject);
        }
        else
            Destroy(this.gameObject);
    }

    public static T GetInstance()
    {
        if (instance == null)
        {
            if (!ApplicationUtility.IsQuitting())
                return instance = InstantiateSingleton(typeof(T));
        }

        return instance;
    }

    protected abstract T Setup();

    private static T InstantiateSingleton(System.Type t)
    {
        if (instance == null)
        {
            GameObject go = new GameObject(t.Name, t);
        }

        return instance;
    }
}