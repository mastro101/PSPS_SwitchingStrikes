using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyType", menuName = "Scriptable/Enemy/Type")]
public class EnemyType : ScriptableObject
{
    public Color color = new Color(1,1,1,1);
    PlayerMaskScriptable maskData;
    [SerializeField] PoolManager attackPrefabSerialized;
    public IPoolable attackPrefab = null;

    public void Setup(PlayerMaskScriptable _maskData)
    {
        if (attackPrefabSerialized)
        {
            maskData = _maskData;
            attackPrefabSerialized.SetPoolable();
            attackPrefabSerialized.SpawnObjs();
            attackPrefab = attackPrefabSerialized.GetPoolablePrefab();
        }
    }

    public Sprite GetMaskSprite()
    {
        return maskData.GetMaskSprite(this);
    }
}

[System.Serializable]
public class SpriteMaskAndAttack
{
    public Sprite playerMaskSprite = null;
    public Sprite attackSprite = null;
}