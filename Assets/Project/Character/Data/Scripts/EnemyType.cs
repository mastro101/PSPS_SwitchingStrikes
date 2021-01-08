using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyType", menuName = "Scriptable/Enemy/Type")]
public class EnemyType : ScriptableObject
{
    public Color color = new Color(1,1,1,1);
    public Sprite playerMaskSprite = null;
    public Sprite attackSprite = null;
}

[System.Serializable]
public class SpriteMaskAndAttack
{
    public Sprite playerMaskSprite = null;
    public Sprite attackSprite = null;
}