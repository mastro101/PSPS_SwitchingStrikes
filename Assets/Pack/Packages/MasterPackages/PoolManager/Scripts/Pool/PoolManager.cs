using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPool", menuName = "Scriptable/Pool")]
public class PoolManager : ScriptableObject
{
    [SerializeField] int nObj = 10;
    [SerializeField] [HideInInspector] GameObject serializePoolable = null;
    [SerializeField] IPoolable poolablePrefab;
    PoolableContainer[] poolables;
    bool spawned;

    private void OnEnable()
    {
        SetPoolable();
    }

    public IPoolable GetPoolablePrefab()
    {
        return poolablePrefab;
    }

    public void SetPoolable()
    {
        if (serializePoolable)
        {
            poolablePrefab = serializePoolable.GetComponent<IPoolable>();
            if (poolablePrefab != null)
                poolablePrefab.Setup(this);
        }
    }

    public void SpawnObjs()
    {
        if (poolablePrefab == null)
            return;

        poolables = new PoolableContainer[nObj];
        for (int i = 0; i < nObj; i++)
        {
            IPoolable tempObj = Instantiate(poolablePrefab.gameObject).GetComponent<IPoolable>();
            tempObj.Setup(this);
            poolables[i] = new PoolableContainer(tempObj, true);
            tempObj.gameObject.SetActive(false);
            tempObj.OnInstantiate?.Invoke();
        }
        spawned = true;
    }

    public IPoolable TakeSpawnPoolable()
    {
        if (!spawned)
            SpawnObjs();

        int l = poolables.Length;
        PoolableContainer TempPoolable;
        for (int i = 0; i < l; i++)
        {
            TempPoolable = poolables[i];
            if (TempPoolable.onPool)
            {
                TempPoolable.onPool = false;
                return TempPoolable.poolable;
            }
        }
        return null;
    }

    public void ReturnSpawnPoolable(IPoolable _poolable)
    {
        int l = poolables.Length;
        PoolableContainer tempPoolableConteiner;
        for (int i = 0; i < l; i++)
        {
            tempPoolableConteiner = poolables[i];
            if (tempPoolableConteiner.poolable == _poolable)
            {
                tempPoolableConteiner.poolable.gameObject.SetActive(false);
                tempPoolableConteiner.onPool = true;
                break;
            }
        }
    }

    public void replacePoolable(IPoolable _poolable)
    {
        int l = poolables.Length;
        PoolableContainer tempPoolableConteiner;
        for (int i = 0; i < l; i++)
        {
            tempPoolableConteiner = poolables[i];
            if (tempPoolableConteiner.poolable == _poolable)
            {
                tempPoolableConteiner.ReturnPoolable();
            }
        }
    }

    private void OnDestroy()
    {
        int l = poolables.Length;
        if (poolables == null)
            return;

        for (int i = 0; i < l; i++)
        {
            IPoolable tempObj = poolables[i].poolable;
            Destroy(tempObj.gameObject);
        }
        poolables = null;
        spawned = false;
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
    }

    public void ReturnPoolable()
    {
        poolable.gameObject.SetActive(false);
        onPool = true;
    }
}