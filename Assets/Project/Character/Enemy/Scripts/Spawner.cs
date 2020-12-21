using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] PlayerVar player = null;
    [SerializeField] PolygonGenerator polygon = null;

    [SerializeField] EnemyTypeArrey enemies = null;
    [SerializeField] float minTime = 1;
    [SerializeField] float maxTime = 2;
    [SerializeField] float offsetFromVertex = 0;

    CorutineOnSingleWork spawnCorutine;

    private void Start()
    {
        enemies.Setup();
        spawnCorutine = gameObject.AddComponent<CorutineOnSingleWork>().SetCorutine(SpawnCorutine());
        for (int i = 0; i < enemies.enemies.Length; i++)
        {
            GenericPoolableObject genericPoolableObject = enemies.enemies[i].GetComponent<GenericPoolableObject>();
            if (genericPoolableObject)
            {
                PoolManager p = genericPoolableObject.poolManager;
                if (p)
                    p.SpawnObjs();
            }
        }
        player.GetValue().Setup(polygon, enemies);
        StartSpawn();
    }

    void Spawn(Vector3 pos, Quaternion rot)
    {
        Enemy e = enemies.enemies[Random.Range(0, enemies.enemies.Length)];
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
            Spawn(v + (vDir * offsetFromVertex), Quaternion.identity); //Quaternion.LookRotation(Vector3.forward, v * -1));
        }
    }
}