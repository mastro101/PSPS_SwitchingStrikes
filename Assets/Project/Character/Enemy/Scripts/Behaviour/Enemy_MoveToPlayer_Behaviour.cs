using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_MoveToPlayer_Behaviour : EnemyBehaviuor
{
    [SerializeField] PlayerVar player = null;

    private void Update()
    {
        if(player)
            if (player.value)
            {
                Vector3 dir = (player.value.transform.position - transform.position).normalized;
                Vector3 velocity = dir * enemy.currentSpeed * Time.deltaTime;
                if (Vector3.Distance(transform.position, player.value.transform.position) > velocity.magnitude)
                    enemy.transform.position += velocity;
                else
                    enemy.transform.position += player.value.transform.position - transform.position;
            }
    }
}