using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckChildCollision : MonoBehaviour
{
    ICollidable collidable;

    private void OnEnable()
    {
        collidable = GetComponentInParent<ICollidable>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (collidable != null)
            if (collidable.activeCollide)
                collidable.collisionEvent.OnEnterTrigger?.Invoke(other);
    }

    private void OnTriggerStay(Collider other)
    {
        if (collidable != null)
            if (collidable.activeCollide)
                collidable.collisionEvent.OnTrigger?.Invoke(other);
    }

    private void OnTriggerExit(Collider other)
    {
        if (collidable != null)
            if (collidable.activeCollide)
                collidable.collisionEvent.OnExitTrigger?.Invoke(other);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collidable != null)
            if (collidable.activeCollide)
                collidable.collisionEvent.OnEnterCollision?.Invoke(collision);
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collidable != null)
            if (collidable.activeCollide)
                collidable.collisionEvent.OnCollision?.Invoke(collision);
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collidable != null)
            if (collidable.activeCollide)
                collidable.collisionEvent.OnExitCollision?.Invoke(collision);
    }
}

public interface ICollidable
{
    CollisionEvent collisionEvent { get; }

    bool activeCollide { get; }
}

public class CollisionEvent
{
    public Action<Collider> OnEnterTrigger;
    public Action<Collider> OnTrigger;
    public Action<Collider> OnExitTrigger;
    public Action<Collision> OnEnterCollision;
    public Action<Collision> OnCollision;
    public Action<Collision> OnExitCollision;

    public CollisionEvent(Action<Collider> onEnterTrigger, Action<Collider> onTrigger, Action<Collider> onExitTrigger, Action<Collision> onEnterCollision, Action<Collision> onCollision, Action<Collision> onExitCollision)
    {
        OnEnterTrigger += onEnterTrigger;
        OnTrigger += onTrigger;
        OnExitTrigger += onExitTrigger;
        OnEnterCollision += onEnterCollision;
        OnCollision += onCollision;
        OnExitCollision += onExitCollision;
    }
    
    public CollisionEvent() { }

    public void AddEvent(Action<Collider> onEnterTrigger, Action<Collider> onTrigger, Action<Collider> onExitTrigger, Action<Collision> onEnterCollision, Action<Collision> onCollision, Action<Collision> onExitCollision)
    {
        OnEnterTrigger += onEnterTrigger;
        OnTrigger += onTrigger;
        OnExitTrigger += onExitTrigger;
        OnEnterCollision += onEnterCollision;
        OnCollision += onCollision;
        OnExitCollision += onExitCollision;
    }
}