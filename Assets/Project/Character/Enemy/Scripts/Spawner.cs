using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] PolygonGenerator polygon = null;

    [SerializeField] EnemyTypeArrey enemies = null;
    [SerializeField] float minTime = 1;
    [SerializeField] float maxTime = 2;
    [SerializeField] float offsetFromVertex = 0;

    CorutineOnSingleWork spawnCorutine;

    private void Awake()
    {
        spawnCorutine = gameObject.AddComponent<CorutineOnSingleWork>().SetCorutine(SpawnCorutine());
        for (int i = 0; i < enemies.enemyTypes.Length; i++)
        {
            GenericPoolableObject genericPoolableObject = enemies.enemyTypes[i].enemyPrefab.GetComponent<GenericPoolableObject>();
            if (genericPoolableObject)
            {
                PoolManager p = genericPoolableObject.poolManager;
                if (p)
                    p.SpawnObjs();
            }
        }
    }

    private void Start()
    {
        StartSpawn();
    }

    void Spawn(Vector3 pos, Quaternion rot)
    {
        Enemy e = enemies.enemyTypes[Random.Range(0, enemies.enemyTypes.Length)].enemyPrefab;
        e.Spawn(pos, rot);
    }

    public void StartSpawn()
    {
        spawnCorutine.StartCo();
    }

    public void StopSpawn()
    {
        spawnCorutine.StopCo();
    }

    IEnumerator SpawnCorutine()
    {
        while (true)
        {
            float s = Random.Range(minTime, maxTime);
            yield return new WaitForSeconds(s);

            int l = polygon.GetVertexPositions().Count;
            int r = Random.Range(1, l);
            Vector3 v = polygon.GetVertexPositions(r);
            Vector3 vDir = v.normalized;
            Spawn(v + (vDir * offsetFromVertex), Quaternion.LookRotation(Vector3.forward, v * -1));
        }
    }
}