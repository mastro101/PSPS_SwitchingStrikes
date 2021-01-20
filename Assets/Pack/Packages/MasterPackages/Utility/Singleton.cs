using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    protected static T instance { get; private set; }

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

    public T GetInstance()
    {
        if (instance == null)
        {
            if (!ApplicationUtility.IsQuitting())
                InstantiateSingleton(typeof(T));
        }

        return instance;
    }

    protected abstract T Setup();

    private void InstantiateSingleton(System.Type t)
    {
        if (instance == null)
        {
            Instantiate(this);
        }
    }
}