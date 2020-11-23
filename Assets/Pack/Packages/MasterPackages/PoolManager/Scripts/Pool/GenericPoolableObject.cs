using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericPoolableObject : MonoBehaviour, IPoolable
{
    public PoolManager poolManager { get; set; }

    public Action OnSpawn;

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