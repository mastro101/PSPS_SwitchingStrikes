using UnityEngine;
using System.Collections;
using System;

public interface IPoolable
{
    GameObject gameObject { get; }
    PoolManager poolManager { get; set; }

    Action OnInstantiate { get; set; }
    Action OnSetup { get; set; }
    Action OnSpawn { get; set; }

    IPoolable Take(Vector3 pos, Quaternion rot, Transform parent = null);
    IPoolable Setup(PoolManager poolManager);
    void Destroy();
}

namespace PoolableDefault
{
    public static class DefaultIPoolable
    {
        public static IPoolable DefaultSetup(this IPoolable poolable, PoolManager _poolManager)
        {
            poolable.poolManager = _poolManager;
            poolable.OnSetup?.Invoke();
            return poolable;
        }

        public static IPoolable DefaultTake(this IPoolable poolable, Vector3 pos, Quaternion rot, Transform parent = null)
        {
            IPoolable tempPoolable;
            if (poolable.poolManager)
            {
                tempPoolable = poolable.poolManager.TakeSpawnPoolable();
                if (tempPoolable != null)
                {
                    tempPoolable.gameObject.SetActive(true);
                    tempPoolable.gameObject.transform.SetParent(parent);
                    tempPoolable.gameObject.transform.localPosition = pos;
                    tempPoolable.gameObject.transform.localRotation = rot;
                    poolable.OnSpawn?.Invoke();
                    return tempPoolable;
                }
            }
            return GameObject.Instantiate(poolable.gameObject, pos, rot).GetComponent<IPoolable>();
        }

        public static void DefaultDestroy(this IPoolable poolable)
        {
            if (poolable.poolManager)
            {
                poolable.poolManager.ReturnSpawnPoolable(poolable);
            }
            else
                GameObject.Destroy(poolable.gameObject);
        }

        public static void OnDestroyPoolable(this IPoolable poolable)
        {
            if (poolable.poolManager)
                poolable.poolManager.ReplacePoolable(poolable);
        }
    }
}