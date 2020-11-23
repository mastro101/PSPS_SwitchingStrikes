using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(GenericPoolableObject))]
public class Enemy : MonoBehaviour
{
    [SerializeField] GenericPoolableObject poolable = null;
    [Space]
    [SerializeField] EnemyType _type = null;
    [SerializeField] float minSpeed = 0.1f;
    [SerializeField] float maxSpeed = 10f;
    [SerializeField] int minLife = 1;
    [SerializeField] int maxLife = 3;

    public EnemyType type { get => _type; }

    #region Event
    [SerializeField] UnityEvent<Player, int> OnDamageUE = null;
    public Action<int> OnDamage;
    void InvokeOnDamage(Player p, int i) { OnDamage?.Invoke(i); OnDamageUE?.Invoke(p, i); }

    [SerializeField] UnityEvent OnDeathUE = null;
    public Action OnDeath;
    void InvokeOnDeath() { OnDeath?.Invoke(); OnDeathUE?.Invoke(); } 
    #endregion

    int currentLife;

    [SerializeField] float _currentSpeed = 1f;
    public float currentSpeed {
        get => _currentSpeed;
        set {
            //TODO: do math based on difficult

            if (value < minSpeed) _currentSpeed = minSpeed;
            else if (value > maxSpeed) _currentSpeed = maxSpeed;
            else _currentSpeed = value;

            if (_currentSpeed < 0.1f) _currentSpeed = 0.1f;
        } 
    }

    [SerializeField] int _startLife = 1;
    public int startLife {
        get => _startLife;
        private set
        {
            //TODO: do math based on difficult

            if (value < minLife) _startLife = minLife;
            else if (value > maxLife) _startLife = maxLife;
            else _startLife = value;

            if (_startLife < 1) _startLife = 1;
        }
    }

    public void Spawn(Vector2 pos, Quaternion rot)
    {
        poolable.Take(pos, rot);
    }

    public void TakeDamage(Player player, int i = 1)
    {
        currentLife -= i;
        player.actualScore.Add(i);
        if (currentLife > 0)
            InvokeOnDamage(player, i);
        else
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