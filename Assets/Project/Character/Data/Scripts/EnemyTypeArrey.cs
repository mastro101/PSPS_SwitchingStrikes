using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyTypeArrey", menuName = "Scriptable/Enemy/Arrey")]
public class EnemyTypeArrey : ScriptableObject
{
    [SerializeField] SpawnData[] data;
    [HideInInspector] public Enemy[] enemies;

    public void Setup()
    {
        enemies = new Enemy[data.Length];
        for (int i = 0; i < data.Length; i++)
        {
            if (data[i].enemyPoolManager)
            {
                data[i].enemyPoolManager.SetPoolable();
                enemies[i] = data[i].enemyPoolManager.GetPoolablePrefab().gameObject.GetComponent<Enemy>();
            }
        }
    }
}

[Serializable]
public class SpawnData
{
    public PoolManager enemyPoolManager = null;
    [Range(0, 100)] public float spawnProbability = 50f;
}