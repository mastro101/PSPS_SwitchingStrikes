using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public abstract class EnemyBehaviuor : MonoBehaviour
{
    protected Enemy enemy;

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
    }
}