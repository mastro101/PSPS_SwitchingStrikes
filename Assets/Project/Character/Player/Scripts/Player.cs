using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.Events;

public class Player : MonoBehaviour , ICollidable
{
    [SerializeField] PlayerVar playerInstance = null;
    [SerializeField] IntData currentLife;
    [Space]
    [SerializeField] SwipeController swipeController = null;
    [SerializeField] Transform playerGraphics = null;
    [SerializeField] Transform attackTransform = null;
    VFXAttack currentAttack = null;
    VFXAttack nextAttack = null;
    [SerializeField] SpriteRenderer maskSprite = null;
    [Space]
    [SerializeField] MaskVar maskData;
    [SerializeField] int startLife = 1;
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

    Tween attackTween;
    //Tween endAttack;

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
        //endAttack?.Kill();
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
        SetAttackDirection();
        ChangeType();
        attackTransform.gameObject.SetActive(false);
    }

    public void SetupData()
    {
        currentLife.SetValue(startLife);
        actualScore.SetValue(0);
    }

    void SetAttackDirection()
    {
        possibleAttackDirection = new List<AttackDirection>();
        for (int i = 1; i < polygonGenerator.GetVertexPositions().Count; i++)
        {
            possibleAttackDirection.Add(new AttackDirection(VectorUtility.FromV2ToV3XYZ(polygonGenerator.transform.position + polygonGenerator.GetVertexPositions(i))));
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
            if (possibleType.Contains(et))
                continue;
            et.Setup(maskData.value);
            et.VFXAttack.Generate();
            et.VFXAttack.ResetPosition(transform);
            et.VFXAttack.attack.transform.SetParent(attackTransform);
            possibleType.Add(et);
        }
        typeCount = possibleType.Count;
    }

    int _typeIndex;
    int typeIndex { get => _typeIndex; set { _typeIndex = value; if (_typeIndex >= typeCount) _typeIndex = 0; } }
    int typeCount;
    void ChangeType()
    {
        if (!onAttack)
            ChangeType(typeIndex);
    }
    
    void ChangeType(int index)
    {
        if (index >= 0 && index < typeCount)
        {
            currentType = possibleType[index];
            ChangeMaskAndAttack(currentType.GetMaskSprite(), currentType.VFXAttack, currentType.color);
            typeIndex = index + 1;

            
            nextAttack = possibleType[typeIndex].VFXAttack;
        }
        else
            Debug.LogWarning("index out of range");
    }

    void ChangeMaskAndAttack(Sprite sMask, VFXAttack pAttack, Color c)
    {
        ChangeMask(sMask);//, c);
        ChangeAttack(pAttack);
    }

    void ChangeMask(Sprite s)//,Color c)
    {
        if (maskSprite)
        {
            if (s)
                maskSprite.sprite = s;
            //maskSprite.color = c;
        }
    }

    void ChangeAttack(VFXAttack attack)
    {
        if (attackTransform)
        {
            if (attack != null)
            {
                if (currentAttack != null)
                    currentType.VFXAttack.Stop(currentAttack.generateAttack);
                
                currentAttack = attack;

                currentType.VFXAttack.Play(currentAttack.generateAttack);
                currentAttack.attack.gameObject.SetActive(false);
            }
        }
    }

    bool onAttack;

    void Attack(Vector2 v)
    {
        if(!onAttack)
        {
            onAttack = true;
            attackTransform.gameObject.SetActive(true);
            currentAttack.Play(currentAttack.attack);
            currentAttack.Stop(currentAttack.generateAttack);
            attackTween = attackTransform.DOMove(v, PhysicsUtility.TimeFromSpaceAndVelocity(v.magnitude, speed)).OnComplete(EndAttack);
            attackTween.Play();

            nextAttack.Play(nextAttack.generateAttack);
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
        currentAttack.Stop(currentAttack.attack);
        currentAttack.destroiedAttack.transform.position = attackTransform.position;
        currentAttack.Play(currentAttack.destroiedAttack);
        attackTransform.gameObject.SetActive(false);
        attackTransform.position = transform.position;
        onAttack = false;
        ChangeType();
    }

    public void TakeDamage(Enemy enemy)
    {
        enemyKillsCombo = 0;
        enemy.Destroy();
        currentLife.SetValue(currentLife.GetValue() -1);
        OnDamageUE?.Invoke();
        if (currentLife.value <= 0)
            Death();
    }

    void Death()
    {
        OnDeathUE?.Invoke();
        //SceneNavigation.ReloadScene();
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