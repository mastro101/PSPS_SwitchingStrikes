using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMaskData", menuName = "Scriptable/PlayerMask")]
public class PlayerMaskScriptable : ScriptableObject
{
    [SerializeField] Sprite defaultSprite = null;
    [SerializeField] MaskData[] masks = null;
    [SerializeField] bool equippable;

    public Sprite GetMaskSprite(EnemyType enemyType)
    {
        int l = masks.Length;
        for (int i = 0; i < l; i++)
        {
            MaskData maskData = masks[i];
            if (maskData.type == enemyType)
            {
                return maskData.maskSprite;
            }
        }
        return defaultSprite;
    }

    public Sprite GetMaskSprite(int i)
    {
        return masks[i].maskSprite;
    }

    public bool IsUnlock()
    {
        return equippable;
    }

    public void Unlock()
    {
        equippable = true;
    }

    [System.Serializable]
    public class MaskData
    {
        public Sprite maskSprite = null;
        public EnemyType type = null;
    }
}