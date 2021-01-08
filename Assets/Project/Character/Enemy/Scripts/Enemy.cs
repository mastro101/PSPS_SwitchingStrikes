using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(GenericPoolableObject))]
public class Enemy : MonoBehaviour
{
    [SerializeField] GenericPoolableObject poolable = null;
    [SerializeField] IntData difficultValue = null;
    [Space]
    [SerializeField] EnemyType _type = null;
    [SerializeField] int scoreBase = 1;
    [SerializeField] float speedModifier = 1.1f;
    [SerializeField] float minSpeed = 0.1f;
    [SerializeField] float maxSpeed = 10f;
    [SerializeField] float lifeModifier = 10f;
    [SerializeField] int minLife = 1;
    [SerializeField] int maxLife = 3;

    Enemy instance;

    public EnemyType type { get => _type; }

    #region Event
    [SerializeField] UnityEvent OnSpawnUE = null;
    public Action OnSpawn;
    void InvokeOnSpawn() { OnSpawn?.Invoke(); OnSpawnUE?.Invoke(); }

    [SerializeField] UnityEvent<Player, int> OnDamageUE = null;
    public Action<int> OnDamage;
    void InvokeOnDamage(Player p, int i) { OnDamage?.Invoke(i); OnDamageUE?.Invoke(p, i); }

    [SerializeField] UnityEvent OnDeathUE = null;
    public Action OnDeath;
    void InvokeOnDeath() { OnDeath?.Invoke(); OnDeathUE?.Invoke(); }
    #endregion

    public int currentLife { get; private set; }

    float _currentSpeed = 1f;
    public float currentSpeed {
        get => _currentSpeed;
        private set {
            if (value < minSpeed) _currentSpeed = minSpeed;
            else if (value > maxSpeed) _currentSpeed = maxSpeed;
            else _currentSpeed = value;

            if (_currentSpeed < 0.1f) _currentSpeed = 0.1f;
        } 
    }

    int _startLife = 1;
    public int startLife {
        get => _startLife;
        private set
        {
            if (value < minLife) _startLife = minLife;
            else if (value > maxLife) _startLife = maxLife;
            else _startLife = value;

            if (_startLife < 1) _startLife = 1;
        }
    }

    public void Setup()
    {
        startLife = (int)((float)difficultValue.value * lifeModifier);
        currentSpeed = difficultValue.value * speedModifier;
        currentLife = startLife;
    }

    public void Spawn(Vector2 pos, Quaternion rot)
    {
        instance = poolable.Take(pos, rot).gameObject.GetComponent<Enemy>();
        instance.Setup();
        instance.InvokeOnSpawn();
    }

    public void TakeDamage(Player player, int i = 1)
    {
        player.enemyKillsCombo++;
        int score = (int)((((currentSpeed + currentLife) * scoreBase) * 1000f) * (player.killsComboConstant + (int)(player.enemyKillsCombo / player.enemyQuantityToRaiseKillMultilpier)));
        currentLife -= i;
        player.actualScore.Add(score);
        InvokeOnDamage(player, i);
        if (currentLife <= 0)
            Death();
    }

    void Death()
    {
        InvokeOnDeath();
        Destroy();
    }

    public void Destroy()
    {
        poolable.Destroy();
    }
}