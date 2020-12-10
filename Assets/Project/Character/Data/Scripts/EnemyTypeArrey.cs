using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyTypeArrey", menuName = "Scriptable/Enemy/Arrey")]
public class EnemyTypeArrey : ScriptableObject
{
    [SerializeField] PoolManager[] enemyPoolManager = null;
    [HideInInspector] public Enemy[] enemies;

    public void Setup()
    {
        enemies = new Enemy[enemyPoolManager.Length];
        for (int i = 0; i < enemyPoolManager.Length; i++)
        {
            if (enemyPoolManager[i])
            {
                enemyPoolManager[i].SetPoolable();
                enemies[i] = enemyPoolManager[i].GetPoolablePrefab().gameObject.GetComponent<Enemy>();
            }
        }
    }
}