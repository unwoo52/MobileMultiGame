using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieFSM_Tracking : ZombieFSM_Base
{
    private GameObject target = null;
    private float distance;
    [SerializeField]private float attackRange;
    public void EndStateBehavior()
    {
    }

    public void StartStateBehavior()
    {
        target = _zombieFSM.TrackingScript.GameOnjects[0];
    }
    public void StateMachine()
    {
        _zombieFSM.Anim.OnMove(target.transform.position);

        //look at player
        distance = Vector3.Distance(transform.position, target.transform.position);
        if (distance <= attackRange) AttackMethod();
    }
    private void AttackMethod()
    {
        _zombieFSM.Anim.OnAttack();
    }
}
