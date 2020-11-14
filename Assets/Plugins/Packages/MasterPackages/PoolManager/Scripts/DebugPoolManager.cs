using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugPoolManager : MonoBehaviour
{
    [SerializeField] PoolManager pool = null;
    [SerializeField] GenericPoolableObject poolable = null;

    private void Awake()
    {
        pool.SpawnObjs();
    }

    IEnumerator corutineDestroyPoolable;
    private void Update()
    {
        IPoolable _poolable;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _poolable = poolable.Take(Vector3.zero, Quaternion.identity);
        }
    }
}