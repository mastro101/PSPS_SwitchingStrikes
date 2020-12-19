using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.Events;

public class Player : MonoBehaviour , ICollidable
{
    [SerializeField] PlayerVar playerInstance = null;
    [Space]
    [SerializeField] SwipeController swipeController = null;
    [SerializeField] Transform playerGraphics = null;
    [SerializeField] Transform attackTransform = null;
    [SerializeField] SpriteRenderer attackSprite = null;
    [SerializeField] SpriteRenderer maskSprite = null;
    [Space]
    [SerializeField] float startLife = 1f;
    [SerializeField] float speed = 0.5f;
    [SerializeField] public float killsComboConstant = 1f;
    [SerializeField] public int enemyQuantityToRaiseKillMultilpier = 10;
    [Space]
    [SerializeField] UnityEvent OnDamageUE = null;
    [SerializeField] UnityEvent OnDeathUE = null;
    
    PolygonGenerator polygonGenerator = null;

    EnemyTypeArrey _enemyTypeArrey;
    EnemyTypeArrey enemyTypeArrey { get => _enemyTypeArrey; set { _enemyTypeArrey = value; SetPossibleType(); } }
    List<EnemyType> possibleType;
    EnemyType currentType;

    public FloatData actualScore;

    List<AttackDirection> possibleAttackDirection;
    float currentLife;

    Tween attackTween;
    Tween endAttack;

    public CollisionEvent collisionEvent { get; private set; }

    public bool activeCollide { get; private set; }

    [HideInInspector] public int enemyKillsCombo = 0;

    #region Mono
    private void Awake()
    {
        playerInstance.SetValue(this);
    }

    //private void OnEnable()
    //{
    //    collisionEvent = new CollisionEvent(TriggerEnter, null, null, null, null, null);
    //    polygonGenerator.OnGenerate += SetAttackDirection;
    //    swipeController.OnSwipe += CheckSwipe;
    //    swipeController.OnTouchAndRealese += ChangeType;
    //    activeCollide = true;
    //    playerGraphicsSprite = playerGraphics.GetComponent<SpriteRenderer>();
    //    typeCount = _enemyTypeArrey.enemies.Length;
    //    SetupData();
    //}

    private void OnDisable()
    {
        //polygonGenerator.OnGenerate -= SetAttackDirection;
        //swipeController.OnSwipe -= CheckSwipe;
        //swipeController.OnTouchAndRealese -= ChangeType;
        attackTween?.Kill();
        endAttack?.Kill();
        //activeCollide = false;
    }

    void TriggerEnter(Collider other)
    {
        Enemy enemy = other.GetComponentInParent<Enemy>();
        if (onAttack)
        {
            if (enemy)
            {
                if (enemy.type == currentType)
                {
                    enemy.TakeDamage(this);
                    attackTween?.Kill();
                    EndAttack();
                }
                else
                {
                    TakeDamage(enemy);
                    attackTween?.Kill();
                    EndAttack();
                    // Enemy.TakeDamege???
                }
            }
        }
    }
    #endregion

    public void Setup(PolygonGenerator pg, EnemyTypeArrey eta)
    {
        polygonGenerator = pg;
        enemyTypeArrey = eta;
        enemyKillsCombo = 0;
        SetupData();
        collisionEvent = new CollisionEvent(TriggerEnter, null, null, null, null, null);
        polygonGenerator.OnGenerate += SetAttackDirection;
        swipeController.OnSwipe += CheckSwipe;
        swipeController.OnTouchAndRealese += ChangeType;
        activeCollide = true;
        typeCount = _enemyTypeArrey.enemies.Length;
        SetAttackDirection();
        SetPossibleType();
        ChangeType();
        attackTransform.gameObject.SetActive(false);
    }

    public void SetupData()
    {
        currentLife = startLife;
        actualScore.SetValue(0);
    }

    void SetAttackDirection()
    {
        possibleAttackDirection = new List<AttackDirection>();
        for (int i = 1; i < polygonGenerator.GetVertexPositions().Count; i++)
        {
            possibleAttackDirection.Add(new AttackDirection(VectorUtility.FromV2ToV3XYZ(polygonGenerator.GetVertexPositions(i))));
        }
    }

    void CheckSwipe(SwipeData swipe)
    {
        if (!onAttack)
        {
            float swipeAngle = swipe.angle;
            AttackDirection tempAttDirOne = null, tempAttDirTwo = null;
            float angleOne = 0; float angleTwo = 0;
            for (int i = 0; i < possibleAttackDirection.Count; i++)
            {
                tempAttDirOne = possibleAttackDirection[i];
                angleOne = tempAttDirOne.angle;

                if (i == 0 && swipeAngle < angleOne)
                {
                    Attack(tempAttDirOne.targetPos);
                    return;
                }

                if (i == possibleAttackDirection.Count - 1)
                {
                    tempAttDirTwo = possibleAttackDirection[0];
                    angleTwo = tempAttDirTwo.angle + 360;
                }
                else
                {
                    tempAttDirTwo = possibleAttackDirection[i + 1];
                    angleTwo = tempAttDirTwo.angle;
                }

                if (swipeAngle >= tempAttDirOne.angle && swipeAngle <= tempAttDirTwo.angle)
                {
                    break;
                }
            }

            float distanceOne = swipeAngle - angleOne, distanceTwo = angleTwo - swipeAngle;

            if (distanceOne <= distanceTwo)
            {
                Attack(tempAttDirOne.targetPos);
            }
            else if (distanceOne > distanceTwo)
            {
                Attack(tempAttDirTwo.targetPos);
            }
        }
    }

    void SetPossibleType()
    {
        possibleType = new List<EnemyType>();
        for (int i = 0; i < enemyTypeArrey.enemies.Length; i++)
        {
            EnemyType et = enemyTypeArrey.enemies[i].type;
            if (possibleType.Contains(enemyTypeArrey.enemies[i].type))
                continue;

            possibleType.Add(et);
        }
    }

    int typeIndex;
    int typeCount;
    void ChangeType()
    {
        currentType = possibleType[typeIndex];
        ChangeSprite(currentType.playerMaskSprite, currentType.attackSprite, currentType.color);
        typeIndex++;
        if (typeIndex >= typeCount)
            typeIndex = 0;
    }
    
    void ChangeType(int index)
    {
        if (index >= 0 && index < possibleType.Count)
        {
            currentType = possibleType[index];
            ChangeSprite(currentType.playerMaskSprite, currentType.attackSprite, currentType.color);
            typeIndex = index + 1;
            if (typeIndex >= typeCount)
                typeIndex = 0;
        }
        else
            Debug.LogWarning("index out of range");
    }

    void ChangeSprite(Sprite sMask, Sprite sAttack, Color c)
    {
        ChangeMask(sMask, c);
        ChangeAttack(sAttack, c);
    }

    void ChangeMask(Sprite s,Color c)
    {
        if (maskSprite)
        {
            if (s)
                maskSprite.sprite = s;
            maskSprite.color = c;
        }
    }

    void ChangeAttack(Sprite s, Color c)
    {
        if (attackSprite)
        {
            if (s)
                attackSprite.sprite = s;
            attackSprite.color = c;
        }
    }

    bool onAttack;

    void Attack(Vector2 v)
    {
        if(!onAttack)
        {
            onAttack = true;
            attackTransform.gameObject.SetActive(true);
            attackTween = attackTransform.DOMove(v, PhysicsUtility.TimeFromSpaceAndVelocity(v.magnitude, speed)).OnComplete(EndAttack);
            attackTween.Play();
        }
    }

    void EndAttack()
    {
        Vector3 target = polygonGenerator.GetVertexPositions(0);
        //endAttack = attackSprite.transform.DOMove(target, PhysicsUtility.TimeFromSpaceAndVelocity(polygonGenerator.GetRadius(), speed)).OnComplete(RestartAttack);
        RestartAttack();
    }    

    void RestartAttack()
    {
        attackTransform.gameObject.SetActive(false);
        attackTransform.position = transform.position;
        onAttack = false;
        ChangeType();
    }

    public void TakeDamage(Enemy enemy)
    {
        enemyKillsCombo = 0;
        enemy.Destroy();
        currentLife--;
        OnDamageUE?.Invoke();
        if (currentLife <= 0)
            Death();
    }

    void Death()
    {
        OnDeathUE?.Invoke();
        SceneNavigation.ReloadScene();
    }

    class AttackDirection
    {
        public Vector2 targetPos { get; }
        public float angle { get; }

        public AttackDirection(Vector2 targetPos)
        {
            this.targetPos = targetPos;
            angle = Vector2.SignedAngle(Vector2.up, this.targetPos);
            if (angle < 0)
            {
                angle += 360;
            }
        }
    }
}