using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonProject<T> : MonoBehaviour where T : SingletonProject<T>
{
    static public T instance;

    virtual protected void Awake()
    {
        if (instance == null)
        {
            instance = (T)this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}