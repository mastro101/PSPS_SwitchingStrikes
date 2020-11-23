using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyType", menuName = "Scriptable/Enemy/Type")]
public class EnemyType : ScriptableObject
{
    [SerializeField] PoolManager enemyPoolManager = null;
    public Type type;
    public Color color = new Color(1,1,1,1);
    [HideInInspector] public Enemy enemyPrefab;

    private void OnEnable()
    {
        Setup();
    }

    public void Setup()
    {
        enemyPoolManager.SetPoolable();
        enemyPrefab = enemyPoolManager.GetPoolablePrefab().gameObject.GetComponent<Enemy>();
    }

    public enum Type
    {
        Blue,
        Red,
    }
}