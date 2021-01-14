using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugPoolManager : MonoBehaviour
{
    [SerializeField] PoolManager pool = null;
    [SerializeField] [SerializeInterface(typeof(IPoolable))] GameObject poolable;
    CorutineOnSingleWork corutineDestroyPoolable;

    IPoolable _poolable;

    private void Awake()
    {
        pool.SpawnObjs();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _poolable = poolable.GetComponent<IPoolable>().Take(Vector3.zero, Quaternion.identity);
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (_poolable != null) _poolable.Destroy();
        }

        if (Input.GetKeyDown(KeyCode.Delete))
        {
            if (_poolable != null) Destroy(_poolable.gameObject);
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SceneNavigation.ReloadScene();
        }

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            SceneNavigation.ChangeScene(0);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SceneNavigation.ChangeScene(1);
        }
    }
}