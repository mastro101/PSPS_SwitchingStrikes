using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PoolableDefault;

public class GenericPoolableObject : MonoBehaviour, IPoolable
{
    public PoolManager poolManager { get; set; }
    public Action OnInstantiate { get; set; }

    public Action OnSetup { get; set; }
    public Action OnSpawn { get; set; }

    public IPoolable Setup(PoolManager _poolManager)
    {
        return this.DefaultSetup(_poolManager);
    }

    public IPoolable Take(Vector3 pos, Quaternion rot, Transform parent = null)
    {
        return this.DefaultTake(pos, rot, parent);
    }
    
    public void Destroy()
    {
        this.DefaultDestroy();
    }

    private void OnDestroy()
    {        
        this.OnDestroyPoolable();
    }
}