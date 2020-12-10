using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericPoolableObject : MonoBehaviour, IPoolable
{
    public PoolManager poolManager { get; private set; }
    public Action OnInstantiate { get; set; }

    public Action OnSetup;
    public Action OnSpawn;

    public void Setup(PoolManager _poolManager)
    {
        poolManager = _poolManager;
        OnSetup?.Invoke();
    }

    public IPoolable Take(Vector3 pos, Quaternion rot)
    {
        IPoolable tempPoolable;
        if (poolManager)
        {
            tempPoolable = poolManager.TakeSpawnPoolable();
            if (tempPoolable != null)
            {
                tempPoolable.gameObject.SetActive(true);
                tempPoolable.gameObject.transform.SetPositionAndRotation(pos, rot);
                OnSpawn?.Invoke();
                return tempPoolable;
            }
        }
        return Instantiate(this, pos, rot);
    }
    
    public void Destroy()
    {
        if (poolManager)
        {
            poolManager.ReturnSpawnPoolable(this);
        }
        else
            Destroy(gameObject);
    }
}