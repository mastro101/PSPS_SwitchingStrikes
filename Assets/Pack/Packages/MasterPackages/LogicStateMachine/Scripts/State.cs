using UnityEngine;
using System.Collections;

public abstract class State : StateMachineBehaviour
{
    protected Context ctx;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Enter();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Tick();
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Exit();
    }

    protected virtual void Enter() { }
    
    protected virtual void Tick() { }
    
    protected virtual void Exit() { }
}