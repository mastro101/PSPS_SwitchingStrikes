using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachine : MonoBehaviour
{
    [SerializeField] Animator animator;

    private void Awake()
    {
        foreach (var s in animator.GetBehaviours<State>())
        {
            
        }
    }
}