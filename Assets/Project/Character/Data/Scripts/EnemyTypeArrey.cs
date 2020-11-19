using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyTypeArrey", menuName = "Scriptable/Enemy/Arrey")]
public class EnemyTypeArrey : ScriptableObject
{
    public EnemyType[] enemyTypes;
}