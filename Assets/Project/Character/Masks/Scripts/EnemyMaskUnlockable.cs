using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMaskUnlockable : MonoBehaviour
{
    PlayerMaskScriptable mask;

    Enemy enemy;

    public void Setup(Enemy _enemy, PlayerMaskScriptable _mask)
    {
        enemy = _enemy;
        mask = _mask;
        enemy.maskPosition.sprite = mask.GetMaskSprite(enemy.type);
        enemy.maskPosition.enabled = true;
        enemy.OnDeath += UnlockMask;
        enemy.OnDestroy += Unsubscribe;
        Debug.Log("MaskSpawned");
    }

    void UnlockMask()
    {
        mask.equippable = true;
        PlayerPrefs.SetInt(mask.name, System.Convert.ToInt32(mask.equippable));
    }

    void Unsubscribe()
    {
        enemy.OnDeath -= UnlockMask;
        enemy.OnDestroy -= Unsubscribe;
        enemy.maskPosition.enabled = false;
        Destroy(this);
    }
}