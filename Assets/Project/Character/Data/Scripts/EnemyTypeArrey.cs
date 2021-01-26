using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyTypeArrey", menuName = "Scriptable/Enemy/Arrey")]
public class EnemyTypeArrey : ScriptableObject
{
    [SerializeField] SpawnData[] data;
    [HideInInspector] [NonSerialized] public Enemy[] enemies;
    [NonSerialized] public float[] enemyIndex;

    [NonSerialized] float p;
    public void Setup()
    {
        p = 0;
        enemies = new Enemy[data.Length];
        enemyIndex = new float[data.Length];
        for (int i = 0; i < data.Length; i++)
        {
            if (data[i].enemyPoolManager)
            {
                data[i].enemyPoolManager.SetPoolable();
                enemies[i] = data[i].enemyPoolManager.GetPoolablePrefab().gameObject.GetComponent<Enemy>();
                p += data[i].spawnProbability;
                data[i].spawnIndex = p;
            }
        }
    }

    public Enemy GetEnemy(float probability)
    {
        for (int i = 0; i < data.Length; i++)
        {
            if (probability <= data[i].spawnIndex)
            {
                return enemies[i];
            }
        }
        return enemies[enemies.Length - 1];
    }
}

[Serializable]
public class SpawnData
{
    public PoolManager enemyPoolManager = null;
    [Range(0, 100)] public float spawnProbability = 50f;
    [NonSerialized] public float spawnIndex = 0;
}