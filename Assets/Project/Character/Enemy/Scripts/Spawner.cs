using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] PolygonGenerator polygon;

    [SerializeField] Enemy enemy = null;
    [SerializeField] float minTime = 1;
    [SerializeField] float maxTime = 2;

    CorutineOnSingleWork spawnCorutine;

    private void Awake()
    {
        spawnCorutine = gameObject.AddComponent<CorutineOnSingleWork>().SetCorutine(SpawnCorutine());
    }

    private void Start()
    {
        StartSpawn();
    }

    void Spawn(Vector3 pos, Quaternion rot)
    {
        enemy.Spawn(pos, rot);
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
            Vector3 v = polygon.GetVertexPositions(Random.Range(1, l));
            Spawn(v, Quaternion.LookRotation(Vector3.forward, v * -1));
        }
    }
}