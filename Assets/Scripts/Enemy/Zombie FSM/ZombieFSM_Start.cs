using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieFSM_Start : ZombieFSM_Base
{
    Animator animator;
    AnimatorStateInfo stateInfo;
    public override void EndStateBehavior()
    {
    }
    public override void StartStateBehavior()
    {
        animator = GetComponent<Animator>();
        _zombieFSM.Anim.OnSetTrigger_Spawn();
    }
    public override void StateMachine()
    {
        stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.normalizedTime >= 1.0f && !animator.IsInTransition(0))
        {
            _zombieFSM.ChangerState(ZombieFSM.ZOMBIEBEHAVIOR.Idle);
        }
    }
}
