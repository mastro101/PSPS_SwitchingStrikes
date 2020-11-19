using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyType", menuName = "Scriptable/Enemy/Type")]
public class EnemyType : ScriptableObject
{
    public Enemy enemyPrefab;
    public Type type;
    public Color color = new Color(1,1,1,1);

    public enum Type
    {
        Blue,
        Red,
    }
}