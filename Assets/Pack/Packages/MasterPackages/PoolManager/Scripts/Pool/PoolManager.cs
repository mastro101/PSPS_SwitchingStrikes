using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "NewPool", menuName = "Scriptable/Pool")]
public class PoolManager : ScriptableObject
{
    [SerializeField] int nObj = 10;
    [SerializeField] [SerializeInterface(typeof(IPoolable))] GameObject serializePoolable = null;
    IPoolable poolablePrefab;
    [NonSerialized] List<PoolableContainer> poolables;

    private void OnEnable()
    {
        SetPoolable();
    }

    public IPoolable GetPoolablePrefab()
    {
        return poolablePrefab;
    }

    public PoolManager SetPoolable()
    {
        if (serializePoolable)
        {
            poolablePrefab = serializePoolable.GetComponent<IPoolable>();
            if (poolablePrefab != null)
                poolablePrefab.Setup(this);
        }
        return this;
    }

    public void SpawnObjs()
    {
        if (poolablePrefab == null)
            return;

        poolables = new List<PoolableContainer>(nObj);
        for (int i = 0; i < nObj; i++)
        {
            SpawnObj();
        }
    }

    IPoolable InstantiatePoolable()
    {
        IPoolable tempObj = null;
        GameObject go = ApplicationUtility.SafeInstantiate(poolablePrefab.gameObject, Vector3.zero, Quaternion.identity, null);
        if (go) tempObj = go.GetComponent<IPoolable>();
        if (tempObj == null)
            return null;
        return tempObj.Setup(this);
    }

    private IPoolable SpawnObj()
    {
        IPoolable tempObj = InstantiatePoolable();
        if (tempObj != null)
        {
            poolables.Add(new PoolableContainer(tempObj, true));
            tempObj.OnInstantiate?.Invoke();
        }
        return tempObj;
    }

    private IPoolable SpawnObj(int index)
    {
        if (index >= 0 && index < poolables.Capacity)
        {
            IPoolable tempObj = InstantiatePoolable();
            if (tempObj != null)
            {
                poolables[index] = new PoolableContainer(tempObj, true);
                tempObj.OnInstantiate?.Invoke();
            }
            return tempObj;
        }
        Debug.LogError("index Out of Range, for default the Object is the last of the list");
        return SpawnObj();
    }

    public void AddAndSpawnObj(int n)
    {
        if (n > 0)
        {
            poolables.Capacity += n;
            for (int i = poolables.Capacity - n; i < poolables.Count; i++)
            {
                SpawnObj();
            }
        }
    }

    private IPoolable AddAndSpawnObj()
    {
        return SpawnObj();
    }

    public IPoolable TakeSpawnPoolable()
    {
        if (poolables.Count <= 0)
            SpawnObjs();

        int l = poolables.Count;
        PoolableContainer TempPoolable;
        for (int i = 0; i < l; i++)
        {
            TempPoolable = poolables[i];
            if (TempPoolable != null)
            {
                if (TempPoolable.onPool)
                {
                    TempPoolable.TakePoolable();
                    return TempPoolable.poolable;
                }
            }
        }

        return AddAndSpawnObj();
    }

    public IPoolable TakeSpawnPoolable(Vector3 pos, Quaternion rot, Transform parent = null)
    {
        return TakeSpawnPoolable()?.Take(pos, rot, parent);
    }

    public void ReturnSpawnPoolable(IPoolable _poolable)
    {
        if (Application.isPlaying)
        {
            int l = poolables.Count;
            PoolableContainer tempPoolableConteiner;
            for (int i = 0; i < l; i++)
            {
                tempPoolableConteiner = poolables[i];
                if (tempPoolableConteiner.poolable == _poolable)
                {
                    tempPoolableConteiner.ReturnPoolable();
                    return;
                }
            }

            Destroy(_poolable.gameObject);
        }
    }

    public void ReplacePoolable(IPoolable _poolable)
    {
        if (poolables != null)
        {
            int l = poolables.Count;
            PoolableContainer tempPoolableConteiner;
            for (int i = 0; i < l; i++)
            {
                tempPoolableConteiner = poolables[i];
                if (tempPoolableConteiner.poolable == _poolable)
                {
                    SpawnObj(i);
                    return;
                }
            }
        }
    }

    public int Count()
    {
        if (poolables != null)
            return poolables.Count;
        else
            return 0;
    }

    public void DespawnObjs()
    {
        int l = poolables.Count;
        if (poolables == null)
            return;

        for (int i = 0; i < l; i++)
        {
            IPoolable tempObj = poolables[i].poolable;
            Destroy(tempObj.gameObject);
        }
    }
}

class PoolableContainer
{
    public IPoolable poolable;
    public bool onPool;

    public PoolableContainer(IPoolable _obj, bool _onPool)
    {
        poolable = _obj;
        onPool = _onPool;
        poolable.gameObject.SetActive(!_onPool);
    }

    public void ReturnPoolable()
    {
        poolable.gameObject.SetActive(false);
        onPool = true;
    }

    public void TakePoolable()
    {
        poolable.gameObject.SetActive(true);
        onPool = false;
    }
}