using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_MoveSelfUp_Behaviour : EnemyBehaviuor
{
    private void Update()
    {
        enemy.transform.position += transform.up * enemy.currentSpeed * Time.deltaTime;
    }
}