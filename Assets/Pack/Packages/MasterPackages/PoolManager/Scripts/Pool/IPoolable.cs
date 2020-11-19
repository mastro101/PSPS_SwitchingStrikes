using UnityEngine;
using System.Collections;

public interface IPoolable
{
    GameObject gameObject { get; }
    PoolManager poolManager { get; set; }

    IPoolable Take(Vector3 pos, Quaternion rot);
    void Destroy();
}