using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour , ICollidable
{
    [SerializeField] SwipeController swipeController = null;
    [SerializeField] PolygonGenerator polygonGenerator = null;
    [SerializeField] Transform playerGraphics = null;
    [Space]
    [SerializeField] float startLife = 1f;

    [SerializeField] EnemyTypeArrey _enemyTypeArrey;
    EnemyTypeArrey enemyTypeArrey { get => _enemyTypeArrey; set { _enemyTypeArrey = value; typeCount = _enemyTypeArrey.enemyTypes.Length; } }
    EnemyType currentType;

    public FloatData score;

    List<AttackDirection> possibleAttackDirection;
    float currentLife;
    SpriteRenderer playerGraphicsSprite;


    public CollisionEvent collisionEvent { get; private set; }

    public bool activeCollide { get; private set; }

    #region Mono
    private void OnEnable()
    {
        collisionEvent = new CollisionEvent(TriggerEnter, null, null, null, null, null);
        polygonGenerator.OnGenerate += SetAttackDirection;
        swipeController.OnSwipe += CheckSwipe;
        swipeController.OnTouchAndRealese += ChangeType;
        activeCollide = true;
        playerGraphicsSprite = playerGraphics.GetComponent<SpriteRenderer>();
        typeCount = _enemyTypeArrey.enemyTypes.Length;
    }

    private void Start()
    {
        SetAttackDirection();
        ChangeType();
    }

    private void OnDisable()
    {
        polygonGenerator.OnGenerate -= SetAttackDirection;
        swipeController.OnSwipe -= CheckSwipe;
        swipeController.OnTouchAndRealese -= ChangeType;
        activeCollide = false;
    }

    void TriggerEnter(Collider other)
    {
        Enemy enemy = other.GetComponentInParent<Enemy>();
        if (onAttack)
        {
            if (enemy)
            {
                if (enemy.type.type == currentType.type)
                    enemy.TakeDamage(this);
            }
        }
    }
    #endregion

    public void Setup()
    {
        currentLife = startLife;
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
                if (i == possibleAttackDirection.Count - 1)
                {
                    tempAttDirTwo = possibleAttackDirection[0];
                    angleTwo = 360f;
                }
                else
                {
                    tempAttDirTwo = possibleAttackDirection[i + 1];
                    angleTwo = tempAttDirTwo.angle;
                }

                if (swipeAngle > tempAttDirOne.angle && swipeAngle < tempAttDirTwo.angle)
                {
                    break;
                }
            }

            if (swipeAngle - tempAttDirOne.angle <= tempAttDirTwo.angle - swipeAngle)
            {
                Attack(tempAttDirOne.targetPos);
            }
            else if (swipeAngle - tempAttDirOne.angle > tempAttDirTwo.angle - swipeAngle)
            {
                Attack(tempAttDirTwo.targetPos);
            }
        }
    }

    int typeIndex;
    int typeCount;
    void ChangeType()
    {
        currentType = enemyTypeArrey.enemyTypes[typeIndex];
        ChangeColor(currentType.color);
        typeIndex++;
        if (typeIndex >= typeCount)
            typeIndex = 0;
    }
    
    void ChangeType(int index)
    {
        if (index >= 0 && index < enemyTypeArrey.enemyTypes.Length)
        {
            currentType = enemyTypeArrey.enemyTypes[index];
            ChangeColor(currentType.color);
            typeIndex = index + 1;
            if (typeIndex >= typeCount)
                typeIndex = 0;
        }
        else
            Debug.LogWarning("index out of range");
    }

    void ChangeColor(Color c)
    {
        if (playerGraphicsSprite)
        {
            playerGraphicsSprite.color = c;
        }
    }

    bool onAttack;

    void Attack(Vector2 v)
    {
        if(!onAttack)
        {
            onAttack = true;
            playerGraphics.DOMove(v, 0.2f).SetLoops(2, LoopType.Yoyo).OnComplete(RestartAttack);
        }
    }

    void RestartAttack()
    {
        onAttack = false;
        ChangeType();
    }

    public void TakeDamage()
    {
        currentLife--;
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