using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        EditorApplication.playModeStateChanged += OnPlaymodeStateChange;
        SceneManager.sceneUnloaded += SceneManager_sceneUnloaded;
        SetPoolable();
    }

    private void OnDisable()
    {
        EditorApplication.playModeStateChanged -= OnPlaymodeStateChange;
        SceneManager.sceneUnloaded -= SceneManager_sceneUnloaded;
    }

    private void SceneManager_sceneUnloaded(Scene arg0)
    {
        spawned = false;
    }

    private void OnPlaymodeStateChange(PlayModeStateChange obj)
    {
        if (obj == PlayModeStateChange.ExitingPlayMode)
        {
            spawned = false;
        }
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

        if (spawned)
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

    public void DespawnObjs()
    {
        int l = poolables.Length;
        if (poolables == null)
            return;

        for (int i = 0; i < l; i++)
        {
            IPoolable tempObj = poolables[i].poolable;
            Destroy(tempObj.gameObject);
        }
        spawned = false;
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
    
    public IPoolable TakeSpawnPoolable(Vector3 pos, Quaternion rot, Transform parent = null)
    {
        return TakeSpawnPoolable()?.Take(pos, rot, parent);
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
                tempPoolableConteiner.ReturnPoolable();
                break;
            }
        }
    }

    public int Count()
    {
        return poolables.Length;
    }

    public void ReplacePoolable(IPoolable _poolable)
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