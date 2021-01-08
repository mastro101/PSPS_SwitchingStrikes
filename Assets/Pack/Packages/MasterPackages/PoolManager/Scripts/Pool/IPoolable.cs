using UnityEngine;
using System.Collections;
using System;

public interface IPoolable
{
    GameObject gameObject { get; }
    PoolManager poolManager { get; }

    Action OnInstantiate { get; set; }

    IPoolable Take(Vector3 pos, Quaternion rot, Transform parent = null);
    void Setup(PoolManager poolManager);
    void Destroy();
}