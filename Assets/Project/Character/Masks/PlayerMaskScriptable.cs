using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMaskData", menuName = "Scriptable/PlayerMask")]
public class PlayerMaskScriptable : ScriptableObject
{
    [SerializeField] Sprite defaultSprite = null;
    [SerializeField] MaskData[] masks = null;

    public Sprite GetMaskSprite(EnemyType enemyType)
    {
        int l = masks.Length;
        for (int i = 0; i < l; i++)
        {
            MaskData maskData = masks[i];
            if (maskData.type == enemyType)
            {
                return maskData.maskPrefab;
            }
        }
        return defaultSprite;
    }

    [System.Serializable]
    public class MaskData
    {
        public Sprite maskPrefab = null;
        public EnemyType type = null;
    }
}