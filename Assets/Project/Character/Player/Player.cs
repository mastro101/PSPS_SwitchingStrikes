using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour
{
    [SerializeField] SwipeController swipeController;
    [SerializeField] PolygonGenerator polygonGenerator;
    [SerializeField] Transform playerGraphics;

    List<AttackDirection> possibleAttackDirection;

    private void OnEnable()
    {
        polygonGenerator.OnGenerate += SetAttackDirection;
        swipeController.OnSwipe += CheckSwipe;
    }

    private void OnDisable()
    {
        polygonGenerator.OnGenerate -= SetAttackDirection;
        swipeController.OnSwipe -= CheckSwipe;
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
        float swipeAngle = swipe.angle;
        AttackDirection tempAttDirOne = null, tempAttDirTwo = null;
        float angleOne = 0; float angleTwo = 0;
        for (int i = 0; i < possibleAttackDirection.Count; i ++)
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

    void Attack(Vector2 v)
    {
        playerGraphics.DOMove(v, 0.2f).SetLoops(1, LoopType.Yoyo);
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